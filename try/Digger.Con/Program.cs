using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using DiggerSkia.Render;
using SkiaSharp;

namespace Digger.Con
{
	internal static class Program
	{
		private static void Main()
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
			var outDir = Directory.CreateDirectory("out").FullName;
			var watch = new Stopwatch();
			var imageIdx = 0;
			var minMs = new[] { long.MaxValue, long.MaxValue, long.MaxValue };
			var maxMs = new[] { long.MinValue, long.MinValue, long.MinValue };
			var avgMs = new[] { 0d, 0d, 0d };
			var sumMs = new[] { 0L, 0L, 0L };

			var dd = frm._digger;
			var lines = new IScanLine[] { new DrawScanLine(dd), new InstScanLine(dd), new SafeScanLine(dd) };

			var timer = new Timer(_ =>
			{
				imageIdx++;
				for (var i = 0; i < lines.Length; i++)
				{
					var line = lines[i];
					watch.Restart();
					line.Paint(canvas, info);
					var dur = watch.ElapsedMilliseconds;

					minMs[i] = Math.Min(dur, minMs[i]);
					maxMs[i] = Math.Max(dur, maxMs[i]);
					sumMs[i] += dur;
					avgMs[i] = sumMs[i] * 1d / imageIdx;
					var pre = $"{imageIdx:D6}_{i:D1}";
					Console.WriteLine($" {pre} -> {dur} ms (min={minMs[i]} | avg={avgMs[i]:F2} | max={maxMs[i]})");

					using var data = bitmap.Encode(SKEncodedImageFormat.Png, 100);
					var fileName = Path.Combine(outDir, $"img_{pre}.png");
					using var file = File.Create(fileName);
					data.SaveTo(file);
					file.Flush(flushToDisk: true);
				}
			});
			var period = TimeSpan.FromSeconds(2);
			timer.Change(period, period);

			Console.ReadLine();
			Console.WriteLine("Done.");
			frame.Close();
		}
	}
}