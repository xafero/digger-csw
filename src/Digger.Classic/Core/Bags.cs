namespace DiggerClassic.Core
{
	internal class Bags
	{
		Bag[] bagdat1 = { new(), new(), new(), new(), new(), new(), new(), new() };
		Bag[] bagdat2 = { new(), new(), new(), new(), new(), new(), new(), new() };
		Bag[] bagdat = { new(), new(), new(), new(), new(), new(), new(), new() };

		int pushcount;
		int goldtime;

		/// <summary>
		/// 4
		/// </summary>
		int[] wblanim = { 2, 0, 1, 0 };

		Digger dig;

		internal Bags(Digger d)
		{
			dig = d;
		}

		internal int bagbits()
		{
			int bag, b, bags = 0;
			for (bag = 1, b = 2; bag < 8; bag++, b <<= 1)
				if (bagdat[bag].Exist)
					bags |= b;
			return bags;
		}

		void baghitground(int bag)
		{
			int bn, b, clbits;
			if (bagdat[bag].Dir == 6 && bagdat[bag].Fallh > 1)
				bagdat[bag].Gt = 1;
			else
				bagdat[bag].Fallh = 0;
			bagdat[bag].Dir = -1;
			bagdat[bag].Wt = 15;
			bagdat[bag].Wobbling = false;
			clbits = dig.Drawing.drawgold(bag, 0, bagdat[bag].X, bagdat[bag].Y);
			dig.Main.incpenalty();
			for (bn = 1, b = 2; bn < 8; bn++, b <<= 1)
				if ((b & clbits) != 0)
					removebag(bn);
		}

		internal int bagy(int bag)
		{
			return bagdat[bag].Y;
		}

		internal void cleanupbags()
		{
			int bpa;
			dig.Sound.soundfalloff();
			for (bpa = 1; bpa < 8; bpa++)
			{
				if (bagdat[bpa].Exist && ((bagdat[bpa].H == 7 && bagdat[bpa].V == 9) ||
				                          bagdat[bpa].Xr != 0 || bagdat[bpa].Yr != 0 || bagdat[bpa].Gt != 0 ||
				                          bagdat[bpa].Fallh != 0 || bagdat[bpa].Wobbling))
				{
					bagdat[bpa].Exist = false;
					dig.Sprite.erasespr(bpa);
				}
				if (dig.Main.getcplayer() == 0)
					bagdat1[bpa].CopyFrom(bagdat[bpa]);
				else
					bagdat2[bpa].CopyFrom(bagdat[bpa]);
			}
		}

		internal void dobags()
		{
			int bag;
			bool soundfalloffflag = true, soundwobbleoffflag = true;
			for (bag = 1; bag < 8; bag++)
				if (bagdat[bag].Exist)
				{
					if (bagdat[bag].Gt != 0)
					{
						if (bagdat[bag].Gt == 1)
						{
							dig.Sound.soundbreak();
							dig.Drawing.drawgold(bag, 4, bagdat[bag].X, bagdat[bag].Y);
							dig.Main.incpenalty();
						}
						if (bagdat[bag].Gt == 3)
						{
							dig.Drawing.drawgold(bag, 5, bagdat[bag].X, bagdat[bag].Y);
							dig.Main.incpenalty();
						}
						if (bagdat[bag].Gt == 5)
						{
							dig.Drawing.drawgold(bag, 6, bagdat[bag].X, bagdat[bag].Y);
							dig.Main.incpenalty();
						}
						bagdat[bag].Gt++;
						if (bagdat[bag].Gt == goldtime)
							removebag(bag);
						else if (bagdat[bag].V < 9 && bagdat[bag].Gt < goldtime - 10)
							if ((dig.Monster.getfield(bagdat[bag].H, bagdat[bag].V + 1) & 0x2000) == 0)
								bagdat[bag].Gt = goldtime - 10;
					}
					else
						updatebag(bag);
				}
			for (bag = 1; bag < 8; bag++)
			{
				if (bagdat[bag].Dir == 6 && bagdat[bag].Exist)
					soundfalloffflag = false;
				if (bagdat[bag].Dir != 6 && bagdat[bag].Wobbling && bagdat[bag].Exist)
					soundwobbleoffflag = false;
			}
			if (soundfalloffflag)
				dig.Sound.soundfalloff();
			if (soundwobbleoffflag)
				dig.Sound.soundwobbleoff();
		}

		internal void drawbags()
		{
			int bag;
			for (bag = 1; bag < 8; bag++)
			{
				if (dig.Main.getcplayer() == 0)
					bagdat[bag].CopyFrom(bagdat1[bag]);
				else
					bagdat[bag].CopyFrom(bagdat2[bag]);
				if (bagdat[bag].Exist)
					dig.Sprite.movedrawspr(bag, bagdat[bag].X, bagdat[bag].Y);
			}
		}

		internal int getbagdir(int bag)
		{
			if (bagdat[bag].Exist)
				return bagdat[bag].Dir;
			return -1;
		}

		void getgold(int bag)
		{
			int clbits;
			clbits = dig.Drawing.drawgold(bag, 6, bagdat[bag].X, bagdat[bag].Y);
			dig.Main.incpenalty();
			if ((clbits & 1) != 0)
			{
				dig.Scores.scoregold();
				dig.Sound.soundgold();
				dig.digtime = 0;
			}
			else
				dig.Monster.mongold();
			removebag(bag);
		}

		internal int getnmovingbags()
		{
			int bag, n = 0;
			for (bag = 1; bag < 8; bag++)
				if (bagdat[bag].Exist && bagdat[bag].Gt < 10 &&
				    (bagdat[bag].Gt != 0 || bagdat[bag].Wobbling))
					n++;
			return n;
		}

		internal void initbags()
		{
			int bag, x, y;
			pushcount = 0;
			goldtime = 150 - dig.Main.levof10() * 10;
			for (bag = 1; bag < 8; bag++)
				bagdat[bag].Exist = false;
			bag = 1;
			for (x = 0; x < 15; x++)
			for (y = 0; y < 10; y++)
				if (dig.Main.getlevch(x, y, dig.Main.levplan()) == 'B')
					if (bag < 8)
					{
						bagdat[bag].Exist = true;
						bagdat[bag].Gt = 0;
						bagdat[bag].Fallh = 0;
						bagdat[bag].Dir = -1;
						bagdat[bag].Wobbling = false;
						bagdat[bag].Wt = 15;
						bagdat[bag].Unfallen = true;
						bagdat[bag].X = x * 20 + 12;
						bagdat[bag].Y = y * 18 + 18;
						bagdat[bag].H = x;
						bagdat[bag].V = y;
						bagdat[bag].Xr = 0;
						bagdat[bag++].Yr = 0;
					}
			if (dig.Main.getcplayer() == 0)
				for (var i = 1; i < 8; i++)
					bagdat1[i].CopyFrom(bagdat[i]);
			else
				for (var i = 1; i < 8; i++)
					bagdat2[i].CopyFrom(bagdat[i]);
		}

		bool pushbag(int bag, int dir)
		{
			int x, y, h, v, ox, oy, clbits;
			var push = true;
			ox = x = bagdat[bag].X;
			oy = y = bagdat[bag].Y;
			h = bagdat[bag].H;
			v = bagdat[bag].V;
			if (bagdat[bag].Gt != 0)
			{
				getgold(bag);
				return true;
			}
			if (bagdat[bag].Dir == 6 && (dir == 4 || dir == 0))
			{
				clbits = dig.Drawing.drawgold(bag, 3, x, y);
				dig.Main.incpenalty();
				if (((clbits & 1) != 0) && (dig.diggery >= y))
					dig.killdigger(1, bag);
				if ((clbits & 0x3f00) != 0)
					dig.Monster.squashmonsters(bag, clbits);
				return true;
			}
			if ((x == 292 && dir == 0) || (x == 12 && dir == 4) || (y == 180 && dir == 6) ||
			    (y == 18 && dir == 2))
				push = false;
			if (push)
			{
				switch (dir)
				{
					case 0:
						x += 4;
						break;
					case 4:
						x -= 4;
						break;
					case 6:
						if (bagdat[bag].Unfallen)
						{
							bagdat[bag].Unfallen = false;
							dig.Drawing.drawsquareblob(x, y);
							dig.Drawing.drawtopblob(x, y + 21);
						}
						else
							dig.Drawing.drawfurryblob(x, y);
						dig.Drawing.eatfield(x, y, dir);
						dig.killemerald(h, v);
						y += 6;
						break;
				}
				switch (dir)
				{
					case 6:
						clbits = dig.Drawing.drawgold(bag, 3, x, y);
						dig.Main.incpenalty();
						if (((clbits & 1) != 0) && dig.diggery >= y)
							dig.killdigger(1, bag);
						if ((clbits & 0x3f00) != 0)
							dig.Monster.squashmonsters(bag, clbits);
						break;
					case 0:
					case 4:
						bagdat[bag].Wt = 15;
						bagdat[bag].Wobbling = false;
						clbits = dig.Drawing.drawgold(bag, 0, x, y);
						dig.Main.incpenalty();
						pushcount = 1;
						if ((clbits & 0xfe) != 0)
							if (!pushbags(dir, clbits))
							{
								x = ox;
								y = oy;
								dig.Drawing.drawgold(bag, 0, ox, oy);
								dig.Main.incpenalty();
								push = false;
							}
						if (((clbits & 1) != 0) || ((clbits & 0x3f00) != 0))
						{
							x = ox;
							y = oy;
							dig.Drawing.drawgold(bag, 0, ox, oy);
							dig.Main.incpenalty();
							push = false;
						}
						break;
				}
				if (push)
					bagdat[bag].Dir = dir;
				else
					bagdat[bag].Dir = dig.reversedir(dir);
				bagdat[bag].X = x;
				bagdat[bag].Y = y;
				bagdat[bag].H = (x - 12) / 20;
				bagdat[bag].V = (y - 18) / 18;
				bagdat[bag].Xr = (x - 12) % 20;
				bagdat[bag].Yr = (y - 18) % 18;
			}
			return push;
		}

		internal bool pushbags(int dir, int bits)
		{
			int bag, bit;
			var push = true;
			for (bag = 1, bit = 2; bag < 8; bag++, bit <<= 1)
				if ((bits & bit) != 0)
					if (!pushbag(bag, dir))
						push = false;
			return push;
		}

		internal bool pushudbags(int bits)
		{
			int bag, b;
			var push = true;
			for (bag = 1, b = 2; bag < 8; bag++, b <<= 1)
				if ((bits & b) != 0)
					if (bagdat[bag].Gt != 0)
						getgold(bag);
					else
						push = false;
			return push;
		}

		void removebag(int bag)
		{
			if (bagdat[bag].Exist)
			{
				bagdat[bag].Exist = false;
				dig.Sprite.erasespr(bag);
			}
		}

		internal void removebags(int bits)
		{
			int bag, b;
			for (bag = 1, b = 2; bag < 8; bag++, b <<= 1)
				if ((bagdat[bag].Exist) && ((bits & b) != 0))
					removebag(bag);
		}

		void updatebag(int bag)
		{
			int x, h, xr, y, v, yr, wbl;
			x = bagdat[bag].X;
			h = bagdat[bag].H;
			xr = bagdat[bag].Xr;
			y = bagdat[bag].Y;
			v = bagdat[bag].V;
			yr = bagdat[bag].Yr;
			switch (bagdat[bag].Dir)
			{
				case -1:
					if (y < 180 && xr == 0)
					{
						if (bagdat[bag].Wobbling)
						{
							if (bagdat[bag].Wt == 0)
							{
								bagdat[bag].Dir = 6;
								dig.Sound.soundfall();
								break;
							}
							bagdat[bag].Wt--;
							wbl = bagdat[bag].Wt % 8;
							if (!((wbl & 1) != 0))
							{
								dig.Drawing.drawgold(bag, wblanim[wbl >> 1], x, y);
								dig.Main.incpenalty();
								dig.Sound.soundwobble();
							}
						}
						else if ((dig.Monster.getfield(h, v + 1) & 0xfdf) != 0xfdf)
							if (!dig.checkdiggerunderbag(h, v + 1))
								bagdat[bag].Wobbling = true;
					}
					else
					{
						bagdat[bag].Wt = 15;
						bagdat[bag].Wobbling = false;
					}
					break;
				case 0:
				case 4:
					if (xr == 0)
						if (y < 180 && (dig.Monster.getfield(h, v + 1) & 0xfdf) != 0xfdf)
						{
							bagdat[bag].Dir = 6;
							bagdat[bag].Wt = 0;
							dig.Sound.soundfall();
						}
						else
							baghitground(bag);
					break;
				case 6:
					if (yr == 0)
						bagdat[bag].Fallh++;
					if (y >= 180)
						baghitground(bag);
					else if ((dig.Monster.getfield(h, v + 1) & 0xfdf) == 0xfdf)
						if (yr == 0)
							baghitground(bag);
					dig.Monster.checkmonscared(bagdat[bag].H);
					break;
			}
			if (bagdat[bag].Dir != -1)
				if (bagdat[bag].Dir != 6 && pushcount != 0)
					pushcount--;
				else
					pushbag(bag, bagdat[bag].Dir);
		}
	}
}