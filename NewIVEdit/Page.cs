using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Ink;

public class Page : INotifyPropertyChanged, IIndexed, IComparable<Page>
{
	public static int ThumbneilLongerPixel = 300;
	public event PropertyChangedEventHandler PropertyChanged;
	public string _pageNoString = "";
	public string OriginFile = "";
	public int OriginPage = 0;
	public string PageNoString
    {
        set
        {
			_pageNoString = value;
			NofityPropertyChanged("PageNoString");
        }
		get
        {
			return _pageNoString;
        }

    }
	public int _pageNo = 1;
	public int PageNo
    {
        set 
		{
			_pageNo = value;
			PageNoString = _pageNo.ToString();
		}
		get
        {
			return _pageNo;
        }
    }
	public int _indexNo = 0;
	public int IndexNo {
		set
		{
			_indexNo = value;
			PageNo = _indexNo + 1;
		}
		get
		{
			return _indexNo;
		}
	}
	protected void NofityPropertyChanged([CallerMemberName] String propertyName = "")
	{
		if (PropertyChanged != null)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
	public int CompareTo(Page that)
    {
		return Comapre(this, that);
    }
	public int Comapre(Page _this, Page that)
    {
		if (!_this.IsGenerated && that.IsGenerated) return 1;
		else if (_this.IsGenerated && !that.IsGenerated) return -1;
		else
		{
			if (_this.PageNo < that.PageNo) return 1;
			else if (_this.PageNo > that.PageNo) return -1;
			else return 0;
		}
    }

	private PageThumbneil _pageThumbneil = null;
	public PageThumbneil PageThumbneil
	{
		set
		{
			_pageThumbneil = value;
			NofityPropertyChanged("Thumbneil");
		}
		get
		{
			return _pageThumbneil;
		}
	}

	private string _caption = "";
	public string Caption
	{
		set
		{
			_caption = value;
			NofityPropertyChanged("Caption");
		}
		get
		{
			return _caption;
		}
	}
	public BitmapSource BackgroundImage {
		set
		{
			_bgimage = value;
			NofityPropertyChanged();
		}
		get
		{
			return _bgimage;
		}
	}
	private BitmapSource _bgimage;
	public int Width {
		set
		{
			_width = value;
			NofityPropertyChanged();
		}
		get
		{
			return _width;
		}
	}
	private int _width;
	public int Height {
		set
		{
			_height = value;
			NofityPropertyChanged();
		}
		get
		{
			return _height;
		}
	}
	private int _height;
	public int CanvasWidth {
		set
		{
			_cwidth = value;
			NofityPropertyChanged();
		}
		get
		{
			return _cwidth;
		}
	}
	private int _cwidth;
	public int CanvasHeight
	{
		set
		{
			_cheight = value;
			NofityPropertyChanged();
		}
		get
		{
			return _cheight;
		}
	}
	private int _cheight;
	public int CurrentRot { set; get; } = 0;
	public double CurrentScale { set; get; } = 1.0;
	public Matrix CurrentMatrix { set; get; }
	public bool IsGenerated { set; get; } = false;
	public bool IsInputPaper { set; get; } = false;

	public StrokeCollection Stroke { set; get; } = new StrokeCollection();

	public ObservableSortedCollection<Annotation> Annotaion { set; get; } = new ObservableSortedCollection<Annotation>(); 

	public Page()
	{
	}
}
