using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NewIVEdit;
public class Accumlator
{
	public string BaseCurrency { set; get; }
	//if exchange rate is Default(double) (0) Converting Accum → IntegratedAccum does not work
	public double ExchangeRate { set; get; } = 1;
	public SortedList<string, AccumlatedResult> Accum { set; get; }
	public ObservableSortedCollection<AccumlatedResult> IntegratedAccum { set; get; }
	public Accumlator()
	{
	}

	public void UpdateMinor()
    {
		foreach(var key in Accum.Keys)
        {
			var acc = Accum[key];
			if (acc.IsMinorForced) continue;
			if (acc.Value >= ConstValue.GOLD)
            {
				acc.IsMinor = false;
            } 
        }
    }
	public void UpdateForeign()
	{

	}
	public void Add(AccumlatedResult value)
    {
		if (Accum.ContainsKey(value.HSCode + "|" + value.Currency))
        {
			var subject = Accum[value.HSCode + "|" + value.Currency];
			subject.Value += value.Value;
        }
        else
        {
			Accum.Add(value.HSCode + "|" + value.Currency, value);
        }
    }

	public void Subtract(AccumlatedResult value)
    {
		if (Accum.ContainsKey(value.HSCode + "|" + value.Currency))
		{
			var subject = Accum[value.HSCode + "|" + value.Currency];
			subject.Value -= value.Value;
		}
	}

	public class AccumlatedResult : IComparable<AccumlatedResult>, IIndexed, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public int Compare(AccumlatedResult _this, AccumlatedResult that)
        {
			if (_this.Value < that.Value) return 1;
			else if (_this.Value > that.Value) return -1;
			else return 0;
        }
		public int CompareTo(AccumlatedResult that)
        {
			return Compare(this, that);
        }
		public string HSCode { set; get; }
		public string Currency { set; get; }
		public bool IsForeignOrigin { set; get; } = false;
		private bool _isMinor = false;
		public bool IsMinor {
			set
			{
				bool prev = _isMinor;
				if(prev != value)
                {
					if (value == true)
					{
						_isMinor = value;
						App.MainView.SOA.MinToMaj(this.HSCode,this.Currency, true);
                    }
                    else
                    {
						_isMinor = false;
						App.MainView.SOA.MajToMin(this.HSCode,this.Currency);
					}
				}
			}
			get
            {
				return _isMinor;
            }
		}
		public bool IsMinorForced { set; get; } = false;
		public int IndexNo { set; get; }
		public int DelcColmnNo { set; get; }
		public long Value { set; get; }
		public long Qty1 { set; get; }
		public long Qty2 { set; get; }
		public AccumlatedResult()
        {

        }
    }
}
