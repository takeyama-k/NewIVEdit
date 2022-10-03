using System;
using System.Windows.Media.Imaging;
using System.Windows.Media;

public class QrImageAnnotation : Annotation
{
	private BitmapSource _image;
	public BitmapSource Image {
		set 
		{
			_image = value;
			NofityPropertyChanged("Image");
		} 
		get
		{
			return _image;
		} 
	}
	public QrImageAnnotation()
	{
	}
}

public class TextBlockAnnotation : Annotation
{
	private string _text = "";
	private float _fontsize = 16.0F;
	public string Text
    {
        set 
		{
			_text = value;
			NofityPropertyChanged("Text");
		}
		get
        {
			return _text;
        }
    }
	public float FontSize
	{
		set
		{
			_fontsize = value;
			NofityPropertyChanged("FontSize");
		}
		get
		{
			return _fontsize;
		}
	}

	public bool IsViewBox = false;


}

public class TextBoxAnnotation : Annotation
{
	private string _text = "";
	private float _fontsize = 16.0F;
	public delegate void BindDelegate(string value);
	public BindDelegate BindTo;
	public bool IsViewBox = false;
	public void SetText(string value)
    {
		_text = value;
		NofityPropertyChanged("Text");
	}
	public string Text
	{
		set
		{
			_text = value;
			if(BindTo != null)
            {
				BindTo(value);
            }
			NofityPropertyChanged("Text");
		}
		get
		{
			return _text;
		}
	}
	public float FontSize
	{
		set
		{
			_fontsize = value;
			NofityPropertyChanged("FontSize");
		}
		get
		{
			return _fontsize;
		}
	}

}

public class LineAnnotation : Annotation
{
	public double _startX;
	public double _endX;
	public double _startY;
	public double _endY;
	public double _pitch;
	public SolidColorBrush _color = new SolidColorBrush(Colors.Black);

	public double StartX
    {
		set
        {
			_startX = value;
			NofityPropertyChanged("StartX");
        }
		get
        {
			return _startX;
        }
    }
	public double EndX
	{
		set
		{
			_endX = value;
			NofityPropertyChanged("EndX");
		}
		get
		{
			return _endX;
		}
	}
	public double StartY
	{
		set
		{
			_startY = value;
			NofityPropertyChanged("StartY");
		}
		get
		{
			return _startY;
		}
	}
	public double EndY
	{
		set
		{
			_endY = value;
			NofityPropertyChanged("EndY");
		}
		get
		{
			return _endY;
		}
	}
	public double Pitch
	{
		set
		{
			_pitch = value;
			NofityPropertyChanged("Pitch");
		}
		get
		{
			return _pitch;
		}
	}
	public SolidColorBrush Color
	{
		set
		{
			_color = value;
			NofityPropertyChanged("Color");
		}
		get
		{
			return _color;
		}
	}



}
