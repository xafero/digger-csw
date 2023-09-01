using System.Windows.Controls;
using System.Windows.Input;

namespace Digger.Wpf
{
	internal abstract class AppletCompat : Image
	{
		public void SetFocusable(bool value) => Focusable = value;

		public string GetSubmitParameter() => null;

		public int GetSpeedParameter() => 66;

		public void RequestFocus() => Focus();

		protected void HandleKeyDown(object sender, KeyEventArgs e)
		{
			var num = ConvertToLegacy(e.Key);
			if (num >= 0)
			{
				DoKeyDown(num);
				return;
			}
			base.OnKeyDown(e);
		}

		protected void HandleKeyUp(object sender, KeyEventArgs e)
		{
			var num = ConvertToLegacy(e.Key);
			if (num >= 0)
			{
				DoKeyUp(num);
				return;
			}
			base.OnKeyUp(e);
		}

		protected abstract bool DoKeyUp(int key);

		protected abstract bool DoKeyDown(int key);

		private static int ConvertToLegacy(Key netCode)
		{
			switch (netCode)
			{
				case Key.Left:
					return 1006;
				case Key.Right:
					return 1007;
				case Key.Up:
					return 1004;
				case Key.Down:
					return 1005;
				case Key.F1:
					return 1008;
				case Key.F10:
					return 1021;
				case Key.PageUp:
				case Key.OemPlus:
					return 1031;
				case Key.PageDown:
				case Key.OemMinus:
					return 1032;
				default:
					var ascii = (int)netCode;
					return ascii;
			}
		}
	}
}