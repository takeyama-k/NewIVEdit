using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public class PageThumbneil : Page
{
	private BitmapSource _thumbneilImage = null;
	public BitmapSource ThumbneilImage
	{
		set
		{
			_thumbneilImage = value;
			NofityPropertyChanged("ThumbneilImage");
		}
		get
		{
			return _thumbneilImage;
		}
	}
	private bool _isMouseOver = false;
	public bool IsMouseOver
	{
		set
		{
			_isMouseOver = value;
			NofityPropertyChanged("IsMouseOver");
		}
		get
		{
			return _isMouseOver;
		}
	}
}

public class PageThumbneilHedge : PageThumbneil
{

}
