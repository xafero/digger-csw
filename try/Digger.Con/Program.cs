using System;
using System.Drawing;
using System.IO;
using System.Threading;
using SkiaSharp;

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

			var thread = new Thread(async _ => await game.RunAsync());
			thread.Start();

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

			var info = new SKImageInfo(frame.Size.Width, frame.Size.Height);
			using var bitmap = new SKBitmap(info);
			using var canvas = new SKCanvas(bitmap);
			var imageIdx = 0;
			var outDir = Directory.CreateDirectory("out").FullName;
			var timer = new Timer(_ =>
			{
				frm.OnPaintSurface(canvas, info);
				imageIdx++;
				using var data = bitmap.Encode(SKEncodedImageFormat.Png, 100);
				var fileName = Path.Combine(outDir, $"image_{imageIdx:D8}.png");
				using var file = File.Create(fileName);
				data.SaveTo(file);
				file.Flush(flushToDisk: true);
			});
			var period = TimeSpan.FromSeconds(2);
			timer.Change(period, period);

			Console.ReadLine();
			Console.WriteLine("Done.");
			frame.Close();
		}
	}
}