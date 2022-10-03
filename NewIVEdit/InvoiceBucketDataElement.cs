using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NewIVEdit;

public class InvoiceBucketDataElement : IIndexed, INotifyPropertyChanged, IComparable<InvoiceBucketDataElement>
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
	public InvoiceBucketDataElement()
	{
	}

	public int Compare(InvoiceBucketDataElement _this, InvoiceBucketDataElement that)
    {
		return String.Compare(that.Key, _this.Key);
    }
	public int CompareTo(InvoiceBucketDataElement that)
    {
		return Compare(this, that);
    }
	public string Key { set; get; }
	private long _valueInternal = 0;
	public long ValueInternal {
		set 
		{
			_valueInternal = value;
			Value = (double)_valueInternal / (double)ConstValue.InternalValueFactor;
		}
		get
		{
			return _valueInternal;
		}
	}

	public double Value { set; get; }

	public int Count { set; get; } = 0;
}
