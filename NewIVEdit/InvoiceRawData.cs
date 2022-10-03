using System;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class InvoiceRawData : IIndexed, INotifyPropertyChanged, IComparable<InvoiceRawData>
{
	public event PropertyChangedEventHandler PropertyChanged;

	private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
	{
		if (PropertyChanged != null)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
	public int IndexNo { set; get; }
	public int CompareTo(InvoiceRawData that)
	{
		return Compare(this, that);
	}

	public int Compare(InvoiceRawData _this, InvoiceRawData that)
	{
		int comp = String.Compare(that.InvoiceNo, _this.InvoiceNo);
		if (comp != 0) return comp;
        else
        {
			if (_this.IndexNo < that.IndexNo) return 1;
			else if (_this.IndexNo > that.IndexNo) return -1;
			else return 0;
        }
	}
	public InvoiceRawData()
	{
	}

	public string Exist { set; get; }

	public string ExistSecondary { set; get; }
	public string InvoiceNo { set; get; }
	public string Amount { set; get; }
	public string Number { set; get; }
	public string NetWeight { set; get; }
	public string Origin { set; get; }
	public string HSCodeKey { set; get; }
	public string HSCodeClue { set; get; }
	public string Currency { set; get; }
	public string Description { set; get; }

}

public class InvoiceIndex : IIndexed, INotifyPropertyChanged, IComparable<InvoiceIndex>
{
	public event PropertyChangedEventHandler PropertyChanged;

	private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
	{
		if (PropertyChanged != null)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
	public int IndexNo { set; get; }

	public int CompareTo(InvoiceIndex that)
    {
		return Compare(this, that);
    }
	public int Compare(InvoiceIndex _this, InvoiceIndex that)
    {
		return String.Compare(that.Key, _this.Key);
    }
	public InvoiceIndex()
	{
	}
	public string Key { set; get; }
	public bool IsAny { set; get; } = false;
	public int Index { set; get; }
}
