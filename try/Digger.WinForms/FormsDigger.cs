using System;
using System.Drawing;
using System.Windows.Forms;
using DiggerAPI;

namespace Digger.WinForms
{
	internal class FormsDigger : AppletCompat, IFactory
	{
		public IDigger _digger;
		private bool _setup;

		public FormsDigger(IDigger digger)
		{
			_digger = digger;
			DoubleBuffered = true;
			Dock = DockStyle.Fill;
			_setup = false;
			VisibleChanged += OnShow;
		}

		private void OnShow(object sender, EventArgs e)
		{
			if (_setup)
				return;
			_setup = true;
			var ctrl = (FormsDigger)sender;
			var form = ctrl.FindForm();
			form.KeyDown += HandleKeyDown;
			form.KeyUp += HandleKeyUp;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var g = e.Graphics;

			g.ScaleTransform(4, 4);

			var pc = _digger.GetPc();

			var w = pc.GetWidth();
			var h = pc.GetHeight();
			var data = pc.GetPixels();

			const int shift = 1;

			for (var x = 0; x < w; x++)
			{
				for (var y = 0; y < h; y++)
				{
					var arrayIndex = y * w + x;
					var (sr, sg, sb) = pc.GetCurrentSource().Model.GetColor(data[arrayIndex]);
					var color = Color.FromArgb(sr, sg, sb);
					var brush = new SolidBrush(color);
					g.FillRectangle(brush, x + shift, y + shift, 1, 1);
				}
			}
		}

		protected override bool DoKeyUp(int key) => _digger.KeyUp(key);
		protected override bool DoKeyDown(int key) => _digger.KeyDown(key);

		public IRefresher CreateRefresher(IDigger digger, IColorModel model)
			=> new FormsRefresher(this, model);
	}
}