using System;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class OcrClueConatiner : IIndexed, INotifyPropertyChanged, IComparable<OcrClueConatiner>
{
	public event PropertyChangedEventHandler PropertyChanged;
	public int IndexNo { set; get; }

	private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
	{
		if (PropertyChanged != null)
		{

			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
	private int _page;
	public int Page
	{
		set
		{
			_page = value;
		}
		get
		{
			return _page;
		}
	}
	public int CompareTo(OcrClueConatiner that)
	{
		return Compare(this, that);
	}

	public int Compare(OcrClueConatiner _this, OcrClueConatiner that)
	{
		if (_this.Page < that.Page)
		{
			return 1;
		}
		else if (_this.Page > that.Page)
		{
			return -1;
		}
		else
		{
			return 0;
		}
	}
	public Regex Pattern = new Regex("");
	public string Value = "";

	public OcrClueConatiner()
	{
	}
}
