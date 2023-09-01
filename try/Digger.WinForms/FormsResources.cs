using System.Drawing;
using System.IO;

namespace Digger.WinForms
{
	internal static class FormsResources
	{
		internal static Icon LoadImage(Stream icon)
		{
			var bitmap = (Bitmap)Image.FromStream(icon);
			var iconHandle = bitmap.GetHicon();
			var img = Icon.FromHandle(iconHandle);
			return img;
		}
	}
}