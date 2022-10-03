using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class TradetermProfile: IIndexed, INotifyPropertyChanged, IComparable<TradetermProfile>
{
	public event PropertyChangedEventHandler PropertyChanged;

	private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
	{
		if (PropertyChanged != null)
		{

			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
	private int _indexNo;
	public int IndexNo
	{
		set
		{
			_indexNo = value;
		}
		get
		{
			return _indexNo;
		}
	}
	public int CompareTo(TradetermProfile that)
	{
		return Compare(this, that);
	}

	public int Compare(TradetermProfile _this, TradetermProfile that)
	{
		if (_this.IndexNo < that.IndexNo) return 1;
		else if (_this.IndexNo > that.IndexNo) return -1;
		else return 0;
	}
	public string Tradeterm { set; get; }
	public TradetermProfile()
	{
	}
}
