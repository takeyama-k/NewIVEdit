using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;


public static class ImageFunc
{
	static ImageFunc()
	{
	}
	public static BitmapSource BitmapToBitmapSource(Bitmap bmp)
	{
		BitmapSource res = null;
		using (var ms = new System.IO.MemoryStream())
		{
			bmp.Save(ms, ImageFormat.Png);
			ms.Seek(0, System.IO.SeekOrigin.Begin);

			res =
				System.Windows.Media.Imaging.BitmapFrame.Create(
					ms,
					System.Windows.Media.Imaging.BitmapCreateOptions.None,
					System.Windows.Media.Imaging.BitmapCacheOption.OnLoad);
		}
		return res;
	}
}
