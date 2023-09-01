using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using DiggerClassic.Util;

[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]

namespace Digger.Wpf
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			var frm = new WpfDigger(null);
			var game = new DiggerClassic.Core.Digger(frm);
			frm.SetFocusable(true);
			game.Init();
			game.Start();

			var timer = new Thread(async _ =>
			{
				await game.runAsync();
			});
			timer.Start();

			var frame = new Window { Title = "Digger Remastered" };
			frame.Closed += delegate
			{
				game.CaTok.Cancel(true);
				var app = Application.Current;
				app.Shutdown();
				Environment.Exit(0);
			};
			var size = new Size((int)(game.width * 4.03), (int)(game.height * 4.15));
			frame.Width = size.Width;
			frame.Height = size.Height;
			frame.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			frame.Background = Brushes.Black;

			var icon = Resources.FindResource("/icons/digger.png");
			frame.Icon = WpfResources.LoadImage(icon);

			frm._digger = game;
			frame.Content = frm;
			frame.Visibility = Visibility.Visible;

			var app = new Application();
			app.Run(frame);
		}
	}
}