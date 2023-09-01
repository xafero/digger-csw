using System;
using System.Drawing;
using System.Threading;

namespace Digger.Con
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Starting...");

			var frm = new ConDigger(null);
			var game = new DiggerClassic.Core.Digger(frm);
			frm.SetFocusable(true);
			game.Init();
			game.Start();

			var timer = new Thread(async _ => { await game.RunAsync(); });
			timer.Start();

			var frame = new ConForm { Text = "Digger Remastered" };
			frame.FormClosed += delegate
			{
				game.CaTok.Cancel(true);
				Environment.Exit(0);
			};
			frame.Size = new Size((int)(game.width * 4.08), (int)(game.height * 4.28));

			frm._digger = game;
			frame.AddControl(frm);
			frame.Visible = true;

			Console.ReadLine();
			Console.WriteLine("Done.");
		}
	}
}