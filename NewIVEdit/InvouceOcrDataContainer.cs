using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
public class InvoiceOcrDataContainer : IComparable<InvoiceOcrDataContainer>, IIndexed , INotifyPropertyChanged 
{
	public event PropertyChangedEventHandler PropertyChanged;

	public int PageNo;
	public Point Coord;
	public float Width;
	public float Height;
	public string Text;
	public double Tilt = 0.0d;
	private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
	{
		if (PropertyChanged != null)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public int IndexNo { set; get; }
	public int CompareTo(InvoiceOcrDataContainer that)
	{
		return Compare(this, that);
	}

	public int Compare(InvoiceOcrDataContainer _this, InvoiceOcrDataContainer that)
	{
		int c = _compare(this.PageNo, that.PageNo);
		if (c != 0) return c;
		else
        {
			c = _compare(_this.Coord.Y,that.Coord.Y);
			return c;
        }
	}
	private int _compare(int _this, int that)
    {
		if (_this < that) return 1;
		else if (_this > that) return -1;
		else return 0;
	}
	private int _compare(double _this, double that)
    {
		if (_this < that) return 1;
		else if (_this > that) return -1;
		else return 0;
    }
}