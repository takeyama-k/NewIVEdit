using System;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NewIVEdit;

public class CurrencyProfile : IIndexed, INotifyPropertyChanged, IComparable<CurrencyProfile>
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
	public int CompareTo(CurrencyProfile that)
	{
		return Compare(this, that);
	}

	public int Compare(CurrencyProfile _this, CurrencyProfile that)
	{
		return String.Compare(that.Code, _this.Code);
	}
	public string Code { set; get; } = "";

	private bool _isRateValid = false;

	public bool IsRateValid
    {
		set
        {
			_isRateValid = value;
			NofityPropertyChanged("IsRatevalid");
        }
		get
        {
			return _isRateValid;
        }
    }

	private string _rateWarning = "有効な数字を入力してください";
	public string RateWarningMessage
    {
		set
        {
			_rateWarning = value;
			NofityPropertyChanged("RateWarningMessage");
        }
        get
        {
			return _rateWarning;
        }
    }
	private string _rate = "0.00";
	public string Rate {
		set
		{
			double tempRate = 0.0;
			if(!double.TryParse(value,out tempRate))
            {
				RateWarningMessage = "数値として読み取れません";
				IsRateValid = false;
				return;
            }
			if(tempRate < 0)
            {
				RateWarningMessage = "マイナスの数字を入れないでください";
				IsRateValid = false;
				return;
			}
			_rate = (((double)((int)(tempRate * 10000)))/(double)10000).ToString("0.0000");
			if (_rate == "0.00")
            {
				RateWarningMessage = "有効な数字を入力してください";
			}
			RateWarningMessage = "";
			IsRateValid = true;
		}
		get 
		{
			return _rate;
		} 
	}
	private bool _isPrime = false;
	public bool IsPrime {
		set
		{
			_isPrime = value;
			App.MainView.IsPrimeCurrencyForced = true;
			App.MainView.DeclearationAccum.SumUP(true);
			if(App.MainView.SubWindowController.DataWindow != null)
            {
				var selected = App.MainView.SubWindowController.DataWindow.PrimeCurrencyCombobox.SelectedItem;
				var newprime = App.MainView.FindPrimeCurrency();
				App.MainView.DeclearationAccum.SumUP(true);
				if (newprime != null){
					if(selected == null)
                    {
						App.MainView.SubWindowController.DataWindow.PrimeCurrencyCombobox.SelectedItem = newprime;

					}else if(selected != null && (selected as CurrencyProfile).Code != newprime.Code)
                    {
						App.MainView.SubWindowController.DataWindow.PrimeCurrencyCombobox.SelectedItem = newprime;
					}
                }
			}
			NofityPropertyChanged("IsPrime");
		}
		get
		{
			return _isPrime;
		}
	}

	public void SetPrime(bool value)
    {
		_isPrime = value;
		var selected = App.MainView.SubWindowController.DataWindow.PrimeCurrencyCombobox.SelectedItem;
		var newprime = App.MainView.FindPrimeCurrency();
		if (newprime != null)
		{
			if (selected == null)
			{
				App.MainView.SubWindowController.DataWindow.PrimeCurrencyCombobox.SelectedItem = newprime;

			}
			else if (selected != null && (selected as CurrencyProfile).Code != newprime.Code)
			{
				App.MainView.SubWindowController.DataWindow.PrimeCurrencyCombobox.SelectedItem = newprime;
			}
		}
	}
	public CurrencyProfile()
	{
	}
	
}
