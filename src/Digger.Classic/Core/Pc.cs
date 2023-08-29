using System;
using DiggerAPI;
using DiggerClassic.Graphics;

namespace DiggerClassic.Core
{
	internal sealed class Pc : IPc
	{
		internal IRefresher[] source = new IRefresher[2];
		internal IRefresher currentSource;

		internal const int width = 320;
		internal const int height = 200;
		internal int size = width * height;

		internal int[] pixels;

		internal byte[][][] pal =
		{
			new[]
			{
				new byte[] { 0, 0x00, 0xAA, 0xAA },
				new byte[] { 0, 0xAA, 0x00, 0x54 },
				new byte[] { 0, 0x00, 0x00, 0x00 }
			},
			new[]
			{
				new byte[] { 0, 0x54, 0xFF, 0xFF },
				new byte[] { 0, 0xFF, 0x54, 0xFF },
				new byte[] { 0, 0x54, 0x54, 0x54 }
			}
		};

		Digger dig;

		internal Pc(Digger d)
		{
			dig = d;
		}

		internal void gclear()
		{
			for (var i = 0; i < size; i++)
				pixels[i] = 0;
			currentSource.NewPixels();
		}

		internal long gethrt()
		{
			return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		}

		internal int getkips()
		{
			return 0;
		}

		internal void ggeti(int x, int y, short[] p, int w, int h)
		{
			var src = 0;
			var dest = y * width + (x & 0xfffc);
			for (var i = 0; i < h; i++)
			{
				var d = dest;
				for (var j = 0; j < w; j++)
				{
					p[src++] = (short)((((((pixels[d] << 2) | pixels[d + 1]) << 2) | pixels[d + 2]) << 2) |
					                   pixels[d + 3]);
					d += 4;
					if (src == p.Length)
						return;
				}
				dest += width;
			}
		}

		internal int ggetpix(int x, int y)
		{
			var ofs = width * y + x & 0xfffc;
			return (((((pixels[ofs] << 2) | pixels[ofs + 1]) << 2) | pixels[ofs + 2]) << 2) | pixels[ofs + 3];
		}

		internal void ginit()
		{
		}

		internal void ginten(int inten)
		{
			currentSource = source[inten & 1];
			currentSource.NewPixels();
		}

		internal void gpal(int pal)
		{
		}

		internal void gputi(int x, int y, short[] p, int w, int h)
		{
			gputi(x, y, p, w, h, true);
		}

		internal void gputi(int x, int y, short[] p, int w, int h, bool b)
		{
			var src = 0;
			var dest = y * width + (x & 0xfffc);
			for (var i = 0; i < h; i++)
			{
				var d = dest;
				for (var j = 0; j < w; j++)
				{
					var px = p[src++];
					pixels[d + 3] = px & 3;
					px >>= 2;
					pixels[d + 2] = px & 3;
					px >>= 2;
					pixels[d + 1] = px & 3;
					px >>= 2;
					pixels[d] = px & 3;
					d += 4;
					if (src == p.Length)
						return;
				}
				dest += width;
			}
		}

		internal void gputim(int x, int y, int ch, int w, int h)
		{
			var spr = CgaGrafx.cgatable[ch * 2];
			var msk = CgaGrafx.cgatable[ch * 2 + 1];
			var src = 0;
			var dest = y * width + (x & 0xfffc);
			for (var i = 0; i < h; i++)
			{
				var d = dest;
				for (var j = 0; j < w; j++)
				{
					var px = spr[src];
					var mx = msk[src];
					src++;
					if ((mx & 3) == 0)
						pixels[d + 3] = px & 3;
					px >>= 2;
					if ((mx & (3 << 2)) == 0)
						pixels[d + 2] = px & 3;
					px >>= 2;
					if ((mx & (3 << 4)) == 0)
						pixels[d + 1] = px & 3;
					px >>= 2;
					if ((mx & (3 << 6)) == 0)
						pixels[d] = px & 3;
					d += 4;
					if (src == spr.Length || src == msk.Length)
					{
						return;
					}
				}
				dest += width;
			}
		}

		internal void gtitle()
		{
			int src = 0, dest = 0, plus = 0;
			while (true)
			{
				if (src >= CgaGrafx.cgatitledat.Length)
					break;
				int b = CgaGrafx.cgatitledat[src++], l, c;
				if (b == 0xfe)
				{
					l = CgaGrafx.cgatitledat[src++];
					if (l == 0)
						l = 256;
					c = CgaGrafx.cgatitledat[src++];
				}
				else
				{
					l = 1;
					c = b;
				}
				for (var i = 0; i < l; i++)
				{
					int px = c, adst = 0;
					if (dest < 32768)
						adst = (dest / 320) * 640 + dest % 320;
					else
						adst = 320 + ((dest - 32768) / 320) * 640 + (dest - 32768) % 320;
					pixels[adst + 3] = px & 3;
					px >>= 2;
					pixels[adst + 2] = px & 3;
					px >>= 2;
					pixels[adst + 1] = px & 3;
					px >>= 2;
					pixels[adst + 0] = px & 3;
					dest += 4;
					if (dest >= 65535)
						break;
				}
				if (dest >= 65535)
					break;
			}
		}

		internal void gwrite(int x, int y, int ch, int c)
		{
			gwrite(x, y, ch, c, false);
		}

		internal void gwrite(int x, int y, int ch, int c, bool upd)
		{
			int dest = x + y * width, ofs = 0, color = c & 3;
			ch -= 32;
			if ((ch < 0) || (ch > 0x5f))
				return;
			var chartab = Alpha.ascii2cga[ch];
			if (chartab == null)
				return;
			for (var i = 0; i < 12; i++)
			{
				var d = dest;
				for (var j = 0; j < 3; j++)
				{
					int px = chartab[ofs++];
					pixels[d + 3] = px & color;
					px >>= 2;
					pixels[d + 2] = px & color;
					px >>= 2;
					pixels[d + 1] = px & color;
					px >>= 2;
					pixels[d] = px & color;
					d += 4;
				}
				dest += width;
			}
			if (upd)
			{
				// Force complete update for high score
				currentSource.NewPixels( /* x, y, 12, 12 */);
			}
		}

		public int GetWidth() => width;
		public int GetHeight() => height;
		public int[] GetPixels() => pixels;
		public IRefresher GetCurrentSource() => currentSource;
	}
}