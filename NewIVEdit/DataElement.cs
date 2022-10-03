using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;

public class DataElement<T> : IComparable<DataElement<T>>, IIndexed, INotifyPropertyChanged
    where T : IComparable<T>
{
    public event PropertyChangedEventHandler PropertyChanged;
    public int IndexNo { set; get; }
    public T Value { set; get; }
    private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public int Compare(DataElement<T> _this, DataElement<T> that)
    {
        return _this.Value.CompareTo(that.Value);
    }

    public int CompareTo(DataElement<T> that)
    {
        return Compare(this, that);
    }
	public DataElement() 
    {
        
	}
}

public class AmtDataElem : DataElement<ulong>
{
    public string HSCode { set; get; }
    public string Currency { set; get; }
    public AmtDataElem() : base(){

    }
}

public class Qty1DataElem : DataElement<ulong>
{
    public string HSCode { set; get; }
    public string Currency { set; get; }
    public Qty1DataElem() : base()
    {

    }
}

public class Qty2DataElem : DataElement<ulong>
{
    public string HSCode { set; get; }
    public string Currency { set; get; }
    public Qty2DataElem() : base()
    {

    }
}

public class ProductDataElem : DataElement<ulong>
{
    public string Keyword { set; get; }
    public DataElement<ulong> DataElementLinked { set; get; }
    public ProductDataElem() : base()
    {

    }
}

