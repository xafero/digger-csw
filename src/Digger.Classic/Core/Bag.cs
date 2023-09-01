namespace DiggerClassic.Core
{
	internal sealed class Bag
	{
		internal int x;
		internal int y;
		internal int h;
		internal int v;
		internal int xr;
		internal int yr;
		internal int dir;
		internal int wt;
		internal int gt;
		internal int fallh;
		internal bool wobbling;
		internal bool unfallen;
		internal bool exist;

		internal void copyFrom(Bag t)
		{
			x = t.x;
			y = t.y;
			h = t.h;
			v = t.v;
			xr = t.xr;
			yr = t.yr;
			dir = t.dir;
			wt = t.wt;
			gt = t.gt;
			fallh = t.fallh;
			wobbling = t.wobbling;
			unfallen = t.unfallen;
			exist = t.exist;
		}
	}
}