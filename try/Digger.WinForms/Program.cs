using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DiggerClassic;

namespace Digger.WinForms
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var frm = new FormsDigger(null);
			var game = new DiggerClassic.Digger(frm);
			frm.SetFocusable(true);
			game.Init();
			game.Start();

			var timer = new Thread(async _ =>
			{
				await game.runAsync();
			});
			timer.Start();

			var frame = new Form { Text = "Digger Remastered" };
			frame.FormClosed += delegate
			{
				game.CaTok.Cancel(true);
				Application.Exit();
				Environment.Exit(0);
			};
			frame.Size = new Size((int)(game.width * 4.08), (int)(game.height * 4.28));
			frame.StartPosition = FormStartPosition.CenterScreen;

			var icon = Resources.FindResource("/icons/digger.png");
			frame.Icon = FormsResources.LoadImage(icon);

			frm._digger = game;
			frame.Controls.Add(frm);
			frame.Visible = true;

			Application.Run(frame);
		}
	}
}