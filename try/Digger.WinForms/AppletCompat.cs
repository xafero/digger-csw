using System.Windows.Forms;

namespace Digger.WinForms
{
	internal abstract class AppletCompat : Panel
	{
		public void SetFocusable(bool _) => Focus();

		public string GetSubmitParameter() => null;

		public int GetSpeedParameter() => 66;

		public void RequestFocus() => Focus();

		protected void HandleKeyDown(object s, KeyEventArgs e)
		{
			var num = ConvertToLegacy(e.KeyCode);
			if (num >= 0)
			{
				DoKeyDown(num);
				return;
			}
			base.OnKeyDown(e);
		}

		protected void HandleKeyUp(object s, KeyEventArgs e)
		{
			var num = ConvertToLegacy(e.KeyCode);
			if (num >= 0)
			{
				DoKeyUp(num);
				return;
			}
			base.OnKeyUp(e);
		}

		protected abstract bool DoKeyUp(int key);

		protected abstract bool DoKeyDown(int key);

		private static int ConvertToLegacy(Keys netCode)
		{
			switch (netCode)
			{
				case Keys.Left:
					return 1006;
				case Keys.Right:
					return 1007;
				case Keys.Up:
					return 1004;
				case Keys.Down:
					return 1005;
				case Keys.F1:
					return 1008;
				case Keys.F10:
					return 1021;
				case Keys.PageUp:
				case Keys.Oemplus:
					return 1031;
				case Keys.PageDown:
				case Keys.OemMinus:
					return 1032;
				default:
					var ascii = (int)netCode;
					return ascii;
			}
		}
	}
}