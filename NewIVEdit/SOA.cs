using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NewIVEdit;
//Statement Of Account
public class SOA
{
	public ObservableSortedCollection<HSGroup> HSGroups { set; get; }
	public SOA()
	{
	}

	public void MinToMaj(string hscode, string currency, bool isForced)
    {
		HSGroup target = null;
        if (isForced)
        {
			target = HSGroups.Find(new HSGroup { HSGroupClass = ConstValue.HSGroupClass.MINOR_FORCED_SINGULAR, Currency = currency, HSCode = hscode });
			if (target == null) target = HSGroups.FindLast(new HSGroup { HSGroupClass = ConstValue.HSGroupClass.MINOR_FORCED_PLURAL, Currency = currency ,HSCode = "Z"});
			if (target == null) return;
        }
        else
        {
			target = HSGroups.Find(new HSGroup { HSGroupClass = ConstValue.HSGroupClass.MINOR_SINGULAR, Currency = currency, HSCode = hscode });
			if (target == null) target = HSGroups.FindLast(new HSGroup { HSGroupClass = ConstValue.HSGroupClass.MINOR_PLURAL, Currency = currency, HSCode = "Z" });
			if (target.HSGroupClass == ConstValue.HSGroupClass.MINOR_FORCED_SINGULAR || target.HSGroupClass == ConstValue.HSGroupClass.MINOR_FORCED_PLURAL || target == null) return;
		}
		Subtract(target);
		target.IsMinor = false;
		Add(target);
    }

	public void MajToMin(string hscode, string currency)
	{
		var target = HSGroups.Find(new HSGroup {IsMinor = false, HSCode = hscode, Currency = currency });
		if (target == null) return;
		Subtract(target);
		target.IsMinor = false;
		Add(target);
	}

	public void Subtract(HSGroup hsg)
    {
		var currentHsg = HSGroups.Find(hsg);
		if (currentHsg.IsMinor == false)
		{
			if (currentHsg != null)
			{
				foreach (var line in hsg.Lines)
				{
					var currentLine = currentHsg.Lines.Find(line);
					if (currentLine != null)
					{
						currentLine.Value -= line.Value;
						currentLine.Qty1 -= line.Qty1;
						currentLine.Qty2 -= line.Qty2;
					}
					if (currentLine.Value == 0 && currentLine.Qty1 == 0 && currentLine.Qty2 == 0)
					{
						currentHsg.Lines.Remove(currentLine);
					}
				}
			}
		}
    }

	public void Add(HSGroup hsg)
	{

	}

	public void UpdateForeign()
	{

	}

	public class HSGroup: IComparable<HSGroup>, IIndexed, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public int CompareTo(HSGroup that)
        {
			return Compare(this, that);
        }
		public int Compare(HSGroup _this, HSGroup that)
		{
			if (_this.HSGroupClass < that.HSGroupClass) return 1;
			else if (_this.HSGroupClass > that.HSGroupClass) return -1;
            else if (0 != String.Compare(that.Currency, _this.Currency)) return String.Compare(that.Currency, _this.Currency);
			else return String.Compare(that.HSCode, _this.HSCode);
		}
		public int IndexNo { set; get; }
		public string HSCode { set; get; }
		public string Currency { set; get; }
		public long Value { set; get; }
		public long Qty1 { set; get; }
		public long Qty2 { set; get; }

		public ConstValue.HSGroupClass HSGroupClass;
		public bool IsMinor { set; get; } = false;
		public bool IsMinorForced { set; get; } = false;
		public bool IsForeignOrigin { set; get; } = false;
		public ObservableSortedCollection<SOALine> Lines { set; get; }
		public HSGroup()
        {
        }

	}
	public class SOALine : IComparable<SOALine>, IIndexed, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public string Label { set; get; }
		public int IndexNo { set; get; }
		public string HSCode { set; get; }
		public string Currency { set; get; }
		public string LineNo { set; get; }
		public long Value { set; get; }
		public long Qty1 { set; get; }
		public long Qty2 { set; get; }

		public SOALine(){
		}
		public int Compare(SOALine _this, SOALine that)
		{
			return String.Compare(that.LineNo, _this.LineNo);
		}
		public int CompareTo(SOALine that)
        {
			return Compare(this, that);
        }
    }
}
