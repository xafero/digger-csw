namespace DiggerClassic.Core
{
	internal sealed class Bag
	{
		internal int X;
		internal int Y;
		internal int H;
		internal int V;
		internal int Xr;
		internal int Yr;
		internal int Dir;
		internal int Wt;
		internal int Gt;
		internal int Fallh;
		internal bool Wobbling;
		internal bool Unfallen;
		internal bool Exist;

		public Bag()
		{
		}

		public void CopyFrom(Bag t)
		{
			X = t.X;
			Y = t.Y;
			H = t.H;
			V = t.V;
			Xr = t.Xr;
			Yr = t.Yr;
			Dir = t.Dir;
			Wt = t.Wt;
			Gt = t.Gt;
			Fallh = t.Fallh;
			Wobbling = t.Wobbling;
			Unfallen = t.Unfallen;
			Exist = t.Exist;
		}
	}
}