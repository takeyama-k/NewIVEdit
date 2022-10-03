using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NewIVEdit;


public class Annotation : IComparable<Annotation>, IIndexed, INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	protected void NofityPropertyChanged([CallerMemberName] String propertyName = "")
	{
		if (PropertyChanged != null)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
	private double _x = 0.0;
	public double X {
		set
		{
			_x = value;
			NofityPropertyChanged("X");
		}
		get
        {
			return _x;
        }
	}
	private double _y = 0.0;
	public double Y {
		set
		{
			_y = value;
			NofityPropertyChanged("Y");
		}
		get
		{
			return _y;
		}
	}

	private double _width = 0.0;
	public double Width {
		set
		{
			_width = value;
			NofityPropertyChanged("Width");
		}
		get
		{
			return _width;
		}		
	}
	private double _height = 0.0;
	public double Height {
		set 
		{
			_height = value;
			NofityPropertyChanged("Height");

		}
		get
		{
			return _height;
		}
	}
	public int IndexNo { set; get; }
	public int PageNo { set; get; }

	public DataElement<ulong> Data{set;get;}

	public int Compare(Annotation _this, Annotation that)
    {
		if (_this.PageNo < that.PageNo) return 1;
		else if (_this.PageNo > that.PageNo) return -1;
		else
		{
			if (_this.Y < that.Y) return 1;
			else if (_this.Y > that.Y) return -1;
			else return 0;
		}
	}
	public int CompareTo(Annotation that)
    {
		return Compare(this, that);
    }
	public Annotation()
	{
	}

}

