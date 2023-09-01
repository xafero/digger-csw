namespace DiggerClassic
{
	public sealed class LoopData
	{
		public int frame;
		public int t;
		public int x = 0;
		public bool start;

		public bool LoopStarted;
		public bool TitleStarted;

		public override string ToString()
			=> $"{nameof(frame)}: {frame}, {nameof(t)}: {t}, {nameof(x)}: {x}, {nameof(start)}: {start}";
	}
}