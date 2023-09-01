namespace DiggerDemo.Core
{
	internal abstract class AppletCompat
	{
		public void Focus()
		{
			// NO-OP!
		}

		public void SetFocusable() => Focus();

		public string GetSubmitParameter() => null;

		public int GetSpeedParameter() => 66;

		public void RequestFocus() => Focus();

		public abstract bool KeyUp(int key);
		public abstract bool KeyDown(int key);

		public const int Key_Left = 1006;
		public const int Key_Right = 1007;
		public const int Key_Up = 1004;
		public const int Key_Down = 1005;
		public const int Key_F1 = 1008;
		public const int Key_F10 = 1021;
		public const int Key_Plus = 1031;
		public const int Key_Minus = 1032;
	}
}