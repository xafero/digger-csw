using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Digger.Wpf
{
	internal static class WpfResources
	{
		internal static ImageSource LoadImage(Stream icon)
		{
			var img = BitmapFrame.Create(icon);
			return img;
		}
	}
}