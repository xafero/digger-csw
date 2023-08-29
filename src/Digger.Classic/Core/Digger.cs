using System.Threading;
using DiggerAPI;
using DiggerClassic.Graphics;
using DiggerClassic.Score;

namespace DiggerClassic.Core
{
	public class Digger : IDigger
	{
		internal static int MAX_RATE = 200;
		internal static int MIN_RATE = 40;

		public int width = 320;
		public int height = 200;
		public int frametime = 66;

		Thread gamethread;

		internal string subaddr;

		private readonly IFactory _factory;

		internal Bags Bags;
		internal Main Main;
		internal Sound Sound;
		internal Monsters Monster;
		internal Scores Scores;
		internal Sprite Sprite;
		internal Drawing Drawing;
		internal Input Input;
		internal Pc Pc;

		internal int diggerx = 0;
		internal int diggery = 0;
		internal int diggerh = 0;
		internal int diggerv = 0;
		internal int diggerrx = 0;
		internal int diggerry = 0;
		internal int digmdir = 0;
		internal int digdir = 0;
		internal int digtime = 0;
		internal int rechargetime = 0;
		internal int firex = 0;
		internal int firey = 0;
		internal int firedir = 0;
		internal int expsn = 0;
		internal int deathstage = 0;
		internal int deathbag = 0;
		internal int deathani = 0;
		internal int deathtime = 0;
		internal int startbonustimeleft = 0;
		internal int bonustimeleft = 0;
		internal int eatmsc = 0;
		internal int emocttime = 0;
		int emmask = 0;

		/// <summary>
		/// 150
		/// </summary>
		byte[] emfield =
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
		};

		internal bool digonscr = false;
		internal bool notfiring = false;
		internal bool bonusvisible = false;
		internal bool bonusmode = false;
		internal bool diggervisible = false;
		internal long time;
		internal long ftime = 50;

		/// <summary>
		/// 8
		/// </summary>
		int[] embox = { 8, 12, 12, 9, 16, 12, 6, 9 };

		/// <summary>
		/// 7
		/// </summary>
		int[] deatharc = { 3, 5, 6, 6, 5, 3, 0 };

		public Digger(IFactory factory)
		{
			_factory = factory;
			Bags = new Bags(this);
			Main = new Main(this);
			Sound = new Sound(this);
			Monster = new Monsters(this);
			Scores = new Scores(this);
			Sprite = new Sprite(this);
			Drawing = new Drawing(this);
			Input = new Input(this);
			Pc = new Pc(this);
		}

		internal bool checkdiggerunderbag(int h, int v)
		{
			if (digmdir == 2 || digmdir == 6)
				if ((diggerx - 12) / 20 == h)
					if ((diggery - 18) / 18 == v || (diggery - 18) / 18 + 1 == v)
						return true;
			return false;
		}

		internal int countem()
		{
			int x, y, n = 0;
			for (x = 0; x < 15; x++)
			for (y = 0; y < 10; y++)
				if ((emfield[y * 15 + x] & emmask) != 0)
					n++;
			return n;
		}

		internal void createbonus()
		{
			bonusvisible = true;
			Drawing.drawbonus(292, 18);
		}

		public void destroy()
		{
			if (gamethread != null)
				gamethread.Abort();
		}

		void diggerdie()
		{
			int clbits;
			switch (deathstage)
			{
				case 1:
					if (Bags.bagy(deathbag) + 6 > diggery)
						diggery = Bags.bagy(deathbag) + 6;
					Drawing.drawdigger(15, diggerx, diggery, false);
					Main.incpenalty();
					if (Bags.getbagdir(deathbag) + 1 == 0)
					{
						Sound.soundddie();
						deathtime = 5;
						deathstage = 2;
						deathani = 0;
						diggery -= 6;
					}
					break;
				case 2:
					if (deathtime != 0)
					{
						deathtime--;
						break;
					}
					if (deathani == 0)
						Sound.music(2);
					clbits = Drawing.drawdigger(14 - deathani, diggerx, diggery, false);
					Main.incpenalty();
					if (deathani == 0 && ((clbits & 0x3f00) != 0))
						Monster.killmonsters(clbits);
					if (deathani < 4)
					{
						deathani++;
						deathtime = 2;
					}
					else
					{
						deathstage = 4;
						if (Sound.musicflag)
							deathtime = 60;
						else
							deathtime = 10;
					}
					break;
				case 3:
					deathstage = 5;
					deathani = 0;
					deathtime = 0;
					break;
				case 5:
					if (deathani >= 0 && deathani <= 6)
					{
						Drawing.drawdigger(15, diggerx, diggery - deatharc[deathani], false);
						if (deathani == 6)
							Sound.musicoff();
						Main.incpenalty();
						deathani++;
						if (deathani == 1)
							Sound.soundddie();
						if (deathani == 7)
						{
							deathtime = 5;
							deathani = 0;
							deathstage = 2;
						}
					}
					break;
				case 4:
					if (deathtime != 0)
						deathtime--;
					else
						Main.setdead(true);
					break;
			}
		}

		internal void dodigger()
		{
			newframe();
			if (expsn != 0)
				drawexplosion();
			else
				updatefire();
			if (diggervisible)
				if (digonscr)
					if (digtime != 0)
					{
						Drawing.drawdigger(digmdir, diggerx, diggery, notfiring && rechargetime == 0);
						Main.incpenalty();
						digtime--;
					}
					else
						updatedigger();
				else
					diggerdie();
			if (bonusmode && digonscr)
			{
				if (bonustimeleft != 0)
				{
					bonustimeleft--;
					if (startbonustimeleft != 0 || bonustimeleft < 20)
					{
						startbonustimeleft--;
						if ((bonustimeleft & 1) != 0)
						{
							Pc.ginten(0);
							Sound.soundbonus();
						}
						else
						{
							Pc.ginten(1);
							Sound.soundbonus();
						}
						if (startbonustimeleft == 0)
						{
							Sound.music(0);
							Sound.soundbonusoff();
							Pc.ginten(1);
						}
					}
				}
				else
				{
					endbonusmode();
					Sound.soundbonusoff();
					Sound.music(1);
				}
			}
			if (bonusmode && !digonscr)
			{
				endbonusmode();
				Sound.soundbonusoff();
				Sound.music(1);
			}
			if (emocttime > 0)
				emocttime--;
		}

		internal void drawemeralds()
		{
			int x, y;
			emmask = 1 << Main.getcplayer();
			for (x = 0; x < 15; x++)
			for (y = 0; y < 10; y++)
				if ((emfield[y * 15 + x] & emmask) != 0)
					Drawing.drawemerald(x * 20 + 12, y * 18 + 21);
		}

		void drawexplosion()
		{
			switch (expsn)
			{
				case 1:
					Sound.soundexplode();
					goto case 2;
				case 2:
				case 3:
					Drawing.drawfire(firex, firey, expsn);
					Main.incpenalty();
					expsn++;
					break;
				default:
					killfire();
					expsn = 0;
					break;
			}
		}

		void endbonusmode()
		{
			bonusmode = false;
			Pc.ginten(0);
		}

		internal void erasebonus()
		{
			if (bonusvisible)
			{
				bonusvisible = false;
				Sprite.erasespr(14);
			}
			Pc.ginten(0);
		}

		internal void erasedigger()
		{
			Sprite.erasespr(0);
			diggervisible = false;
		}

		public string getAppletInfo()
		{
			return "The Digger Remastered -- http://www.digger.org, Copyright (c) Andrew Jenner & Marek Futrega / MAF";
		}

		bool getfirepflag()
		{
			return Input.firepflag;
		}

		internal bool hitemerald(int x, int y, int rx, int ry, int dir)
		{
			bool hit = false;
			int r;
			if (dir < 0 || dir > 6 || ((dir & 1) != 0))
				return hit;
			if (dir == 0 && rx != 0)
				x++;
			if (dir == 6 && ry != 0)
				y++;
			if (dir == 0 || dir == 4)
				r = rx;
			else
				r = ry;
			if ((emfield[y * 15 + x] & emmask) != 0)
			{
				if (r == embox[dir])
				{
					Drawing.drawemerald(x * 20 + 12, y * 18 + 21);
					Main.incpenalty();
				}
				if (r == embox[dir + 1])
				{
					Drawing.eraseemerald(x * 20 + 12, y * 18 + 21);
					Main.incpenalty();
					hit = true;
					emfield[y * 15 + x] = (byte)(emfield[y * 15 + x] & ~emmask);
				}
			}
			return hit;
		}

		public void Init()
		{
			if (gamethread != null)
				gamethread.Abort();

			subaddr = _factory.GetSubmitParameter();

			try
			{
				frametime = _factory.GetSpeedParameter();
				if (frametime > MAX_RATE)
					frametime = MAX_RATE;
				else if (frametime < MIN_RATE)
					frametime = MIN_RATE;
			}
			catch (System.Exception e)
			{
			}

			Pc.pixels = new int[65536];

			for (int i = 0; i < 2; i++)
			{
				// 8, 4
				var model = new ColorModel(Pc.pal[i][0], Pc.pal[i][1], Pc.pal[i][2]);
				Pc.source[i] = _factory.CreateRefresher(this, model);
				Pc.source[i].NewPixels();
			}

			Pc.currentSource = Pc.source[0];

			gamethread = new Thread(this.run);
			gamethread.Start();
		}

		void initbonusmode()
		{
			bonusmode = true;
			erasebonus();
			Pc.ginten(1);
			bonustimeleft = 250 - Main.levof10() * 20;
			startbonustimeleft = 20;
			eatmsc = 1;
		}

		internal void initdigger()
		{
			diggerv = 9;
			digmdir = 4;
			diggerh = 7;
			diggerx = diggerh * 20 + 12;
			digdir = 0;
			diggerrx = 0;
			diggerry = 0;
			digtime = 0;
			digonscr = true;
			deathstage = 1;
			diggervisible = true;
			diggery = diggerv * 18 + 18;
			Sprite.movedrawspr(0, diggerx, diggery);
			notfiring = true;
			emocttime = 0;
			bonusvisible = bonusmode = false;
			Input.firepressed = false;
			expsn = 0;
			rechargetime = 0;
		}

		public IPc GetPc() => Pc;

		public bool KeyDown(int key)
		{
			switch (key)
			{
				case 1006:
					Input.processkey(0x4b);
					break;
				case 1007:
					Input.processkey(0x4d);
					break;
				case 1004:
					Input.processkey(0x48);
					break;
				case 1005:
					Input.processkey(0x50);
					break;
				case 1008:
					Input.processkey(0x3b);
					break;
				case 1021:
					Input.processkey(0x78);
					break;
				case 1031:
					Input.processkey(0x2b);
					break;
				case 1032:
					Input.processkey(0x2d);
					break;
				default:
					key &= 0x7f;
					if ((key >= 65) && (key <= 90))
						key += (97 - 65);
					Input.processkey(key);
					break;
			}
			return true;
		}

		public bool KeyUp(int key)
		{
			switch (key)
			{
				case 1006:
					Input.processkey(0xcb);
					break;
				case 1007:
					Input.processkey(0xcd);
					break;
				case 1004:
					Input.processkey(0xc8);
					break;
				case 1005:
					Input.processkey(0xd0);
					break;
				case 1008:
					Input.processkey(0xbb);
					break;
				case 1021:
					Input.processkey(0xf8);
					break;
				case 1031:
					Input.processkey(0xab);
					break;
				case 1032:
					Input.processkey(0xad);
					break;
				default:
					key &= 0x7f;
					if ((key >= 65) && (key <= 90))
						key += (97 - 65);
					Input.processkey(0x80 | key);
					break;
			}
			return true;
		}

		internal void killdigger(int stage, int bag)
		{
			if (deathstage < 2 || deathstage > 4)
			{
				digonscr = false;
				deathstage = stage;
				deathbag = bag;
			}
		}

		internal void killemerald(int x, int y)
		{
			if ((emfield[y * 15 + x + 15] & emmask) != 0)
			{
				emfield[y * 15 + x + 15] = (byte)(emfield[y * 15 + x + 15] & ~emmask);
				Drawing.eraseemerald(x * 20 + 12, (y + 1) * 18 + 21);
			}
		}

		internal void killfire()
		{
			if (!notfiring)
			{
				notfiring = true;
				Sprite.erasespr(15);
				Sound.soundfireoff();
			}
		}

		internal void makeemfield()
		{
			int x, y;
			emmask = 1 << Main.getcplayer();
			for (x = 0; x < 15; x++)
			for (y = 0; y < 10; y++)
				if (Main.getlevch(x, y, Main.levplan()) == 'C')
					emfield[y * 15 + x] = (byte)(emfield[y * 15 + x] | emmask);
				else
					emfield[y * 15 + x] = (byte)(emfield[y * 15 + x] & ~emmask);
		}

		internal void newframe()
		{
			Input.checkkeyb();
			time += frametime;
			long l = time - Pc.gethrt();
			if (l > 0)
			{
				try
				{
					Thread.Sleep((int)l);
				}
				catch (System.Exception e)
				{
				}
			}
			Pc.currentSource.NewPixels();
		}

		internal int reversedir(int dir)
		{
			switch (dir)
			{
				case 0: return 4;
				case 4: return 0;
				case 2: return 6;
				case 6: return 2;
			}
			return dir;
		}

		public void run()
		{
			Main.main();
		}

		public void Start()
		{
			_factory.RequestFocus();
		}

		void updatedigger()
		{
			int dir, ddir, clbits, diggerox, diggeroy, nmon;
			bool push = false;
			Input.readdir();
			dir = Input.getdir();
			if (dir == 0 || dir == 2 || dir == 4 || dir == 6)
				ddir = dir;
			else
				ddir = -1;
			if (diggerrx == 0 && (ddir == 2 || ddir == 6))
				digdir = digmdir = ddir;
			if (diggerry == 0 && (ddir == 4 || ddir == 0))
				digdir = digmdir = ddir;
			if (dir == -1)
				digmdir = -1;
			else
				digmdir = digdir;
			if ((diggerx == 292 && digmdir == 0) || (diggerx == 12 && digmdir == 4) ||
			    (diggery == 180 && digmdir == 6) || (diggery == 18 && digmdir == 2))
				digmdir = -1;
			diggerox = diggerx;
			diggeroy = diggery;
			if (digmdir != -1)
				Drawing.eatfield(diggerox, diggeroy, digmdir);
			switch (digmdir)
			{
				case 0:
					Drawing.drawrightblob(diggerx, diggery);
					diggerx += 4;
					break;
				case 4:
					Drawing.drawleftblob(diggerx, diggery);
					diggerx -= 4;
					break;
				case 2:
					Drawing.drawtopblob(diggerx, diggery);
					diggery -= 3;
					break;
				case 6:
					Drawing.drawbottomblob(diggerx, diggery);
					diggery += 3;
					break;
			}
			if (hitemerald((diggerx - 12) / 20, (diggery - 18) / 18, (diggerx - 12) % 20,
				    (diggery - 18) % 18, digmdir))
			{
				Scores.scoreemerald();
				Sound.soundem();
				Sound.soundemerald(emocttime);
				emocttime = 9;
			}
			clbits = Drawing.drawdigger(digdir, diggerx, diggery, notfiring && rechargetime == 0);
			Main.incpenalty();
			if ((Bags.bagbits() & clbits) != 0)
			{
				if (digmdir == 0 || digmdir == 4)
				{
					push = Bags.pushbags(digmdir, clbits);
					digtime++;
				}
				else if (!Bags.pushudbags(clbits))
					push = false;
				if (!push)
				{
					/* Strange, push not completely defined */
					diggerx = diggerox;
					diggery = diggeroy;
					Drawing.drawdigger(digmdir, diggerx, diggery, notfiring && rechargetime == 0);
					Main.incpenalty();
					digdir = reversedir(digmdir);
				}
			}
			if (((clbits & 0x3f00) != 0) && bonusmode)
				for (nmon = Monster.killmonsters(clbits); nmon != 0; nmon--)
				{
					Sound.soundeatm();
					Scores.scoreeatm();
				}
			if ((clbits & 0x4000) != 0)
			{
				Scores.scorebonus();
				initbonusmode();
			}
			diggerh = (diggerx - 12) / 20;
			diggerrx = (diggerx - 12) % 20;
			diggerv = (diggery - 18) / 18;
			diggerry = (diggery - 18) % 18;
		}

		void updatefire()
		{
			int clbits, b, mon, pix = 0;
			if (notfiring)
			{
				if (rechargetime != 0)
					rechargetime--;
				else if (getfirepflag())
					if (digonscr)
					{
						rechargetime = Main.levof10() * 3 + 60;
						notfiring = false;
						switch (digdir)
						{
							case 0:
								firex = diggerx + 8;
								firey = diggery + 4;
								break;
							case 4:
								firex = diggerx;
								firey = diggery + 4;
								break;
							case 2:
								firex = diggerx + 4;
								firey = diggery;
								break;
							case 6:
								firex = diggerx + 4;
								firey = diggery + 8;
								break;
						}
						firedir = digdir;
						Sprite.movedrawspr(15, firex, firey);
						Sound.soundfire();
					}
			}
			else
			{
				switch (firedir)
				{
					case 0:
						firex += 8;
						pix = Pc.ggetpix(firex, firey + 4) | Pc.ggetpix(firex + 4, firey + 4);
						break;
					case 4:
						firex -= 8;
						pix = Pc.ggetpix(firex, firey + 4) | Pc.ggetpix(firex + 4, firey + 4);
						break;
					case 2:
						firey -= 7;
						pix = (Pc.ggetpix(firex + 4, firey) | Pc.ggetpix(firex + 4, firey + 1) |
						       Pc.ggetpix(firex + 4, firey + 2) | Pc.ggetpix(firex + 4, firey + 3) |
						       Pc.ggetpix(firex + 4, firey + 4) | Pc.ggetpix(firex + 4, firey + 5) |
						       Pc.ggetpix(firex + 4, firey + 6)) & 0xc0;
						break;
					case 6:
						firey += 7;
						pix = (Pc.ggetpix(firex, firey) | Pc.ggetpix(firex, firey + 1) |
						       Pc.ggetpix(firex, firey + 2) | Pc.ggetpix(firex, firey + 3) |
						       Pc.ggetpix(firex, firey + 4) | Pc.ggetpix(firex, firey + 5) |
						       Pc.ggetpix(firex, firey + 6)) & 3;
						break;
				}
				clbits = Drawing.drawfire(firex, firey, 0);
				Main.incpenalty();
				if ((clbits & 0x3f00) != 0)
					for (mon = 0, b = 256; mon < 6; mon++, b <<= 1)
						if ((clbits & b) != 0)
						{
							Monster.killmon(mon);
							Scores.scorekill();
							expsn = 1;
						}
				if ((clbits & 0x40fe) != 0)
					expsn = 1;
				switch (firedir)
				{
					case 0:
						if (firex > 296)
							expsn = 1;
						else if (pix != 0 && clbits == 0)
						{
							expsn = 1;
							firex -= 8;
							Drawing.drawfire(firex, firey, 0);
						}
						break;
					case 4:
						if (firex < 16)
							expsn = 1;
						else if (pix != 0 && clbits == 0)
						{
							expsn = 1;
							firex += 8;
							Drawing.drawfire(firex, firey, 0);
						}
						break;
					case 2:
						if (firey < 15)
							expsn = 1;
						else if (pix != 0 && clbits == 0)
						{
							expsn = 1;
							firey += 7;
							Drawing.drawfire(firex, firey, 0);
						}
						break;
					case 6:
						if (firey > 183)
							expsn = 1;
						else if (pix != 0 && clbits == 0)
						{
							expsn = 1;
							firey -= 7;
							Drawing.drawfire(firex, firey, 0);
						}
						break;
				}
			}
		}
	}
}