using System;
using System.Drawing;

namespace Digger.Con
{
	internal sealed class ConForm
	{
		public string Text { get; set; }
		public Size Size { get; set; }
		public bool Visible { get; set; }

		public event Action FormClosed;

		public void AddControl(ConDigger frm)
		{
			Form = frm;
		}

		public ConDigger Form { get; set; }

		public void Close()
		{
			FormClosed?.Invoke();
		}
	}
}