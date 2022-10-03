using System;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using NewIVEdit;

public class InvoiceDataElement : IIndexed, INotifyPropertyChanged, IComparable<InvoiceDataElement>
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
			IndexNoString = value.ToString();
		}
		get
		{
			return _indexNo;
		}
	}
	public int CompareTo(InvoiceDataElement that)
	{
		return Compare(this, that);
	}

	public int Compare(InvoiceDataElement _this, InvoiceDataElement that)
    {
		if(_this.IndexNo < that.IndexNo)
        {
			return 1;
        }else if(_this.IndexNo > that.IndexNo)
        {
			return -1;
        }
        else
        {
			return 0;
        }
    }

	public string Hash()
	{
		
		string hash = HSCode + "|" + Postfix + "|" + Currency;
		return hash;
	}

	public static void AccumDelta(InvoiceDataElement oldItem, InvoiceDataElement newItem)
	{
		CountDelegate(oldItem, newItem);
		AddDelegate(oldItem, newItem);
    }
	public delegate void AddValueDelegate(InvoiceDataElement _this, InvoiceDataElement that);
	public static AddValueDelegate AddDelegate = (InvoiceDataElement oldItem, InvoiceDataElement newItem) => {
		if(newItem == null)
        {
			if (oldItem.DeclValid == InvoiceDataElement.DeclearValidFlag.ALL)
			{

				var delta = new DeclAccumElem()
				{
					InvoiceNo = oldItem.InvoiceNo,
					Origin = oldItem.Origin,
					HSCodeClue = oldItem.HSCodeClue,
					HSCodeKey = oldItem.HSCode,
					Description = oldItem.Description,
					HSCode = oldItem.HSCode,
					Currency = oldItem.Currency,
					Postfix = oldItem.Postfix,
					AmountInternal = -oldItem.AmountInternal,
					NetWeightInternal = -oldItem.NetWeightInternal,
					NumberInternal = -oldItem.NumberInternal,
				};
				App.MainView.DeclearationAccum.Add(delta);
			}
			return;
		}
		DeclearValidFlag oldFlag = oldItem.DeclValid;
		DeclearValidFlag newFlag = newItem.DeclValid;
		if (oldFlag != DeclearValidFlag.ALL && newFlag == DeclearValidFlag.ALL)
		{
			DeclAccumElem delta = new DeclAccumElem()
			{
				InvoiceNo = newItem.InvoiceNo,
				Origin = newItem.Origin,
				HSCodeClue = newItem.HSCodeClue,
				HSCodeKey = newItem.HSCode,
				Description = newItem.Description,
				HSCode = newItem.HSCode,
				Currency = newItem.Currency,
				Postfix = newItem.Postfix,
				AmountInternal = newItem.AmountInternal,
				NetWeightInternal = newItem.NetWeightInternal,
				NumberInternal = newItem.NumberInternal,
			};
			App.MainView.DeclearationAccum.Add(delta);
		}
		else if (newFlag != DeclearValidFlag.ALL && oldFlag == DeclearValidFlag.ALL)
		{
			DeclAccumElem delta = new DeclAccumElem()
			{
				InvoiceNo = oldItem.InvoiceNo,
				Origin = oldItem.Origin,
				HSCodeClue = oldItem.HSCodeClue,
				HSCodeKey = oldItem.HSCode,
				Description = oldItem.Description,
				HSCode = oldItem.HSCode,
				Currency = oldItem.Currency,
				Postfix = oldItem.Postfix,
				AmountInternal = -oldItem.AmountInternal,
				NetWeightInternal = -oldItem.NetWeightInternal,
				NumberInternal = -oldItem.NumberInternal,
			};
			App.MainView.DeclearationAccum.Add(delta);
		}
		else if (oldFlag == DeclearValidFlag.ALL && newFlag == DeclearValidFlag.ALL)
		{
			if (oldItem.HSCode != newItem.HSCode || oldItem.Currency != newItem.Currency || newItem.Postfix != oldItem.Postfix)
			{
				DeclAccumElem delta = new DeclAccumElem()
				{
					InvoiceNo = oldItem.InvoiceNo,
					Origin = oldItem.Origin,
					HSCodeClue = oldItem.HSCodeClue,
					HSCodeKey = oldItem.HSCode,
					Description = oldItem.Description,
					HSCode = oldItem.HSCode,
					Currency = oldItem.Currency,
					Postfix = oldItem.Postfix,
					AmountInternal = -oldItem.AmountInternal,
					NetWeightInternal = -oldItem.NetWeightInternal,
					NumberInternal = -oldItem.NumberInternal,
				};
				App.MainView.DeclearationAccum.Add(delta);
				delta = new DeclAccumElem()
				{
					InvoiceNo = newItem.InvoiceNo,
					Origin = newItem.Origin,
					HSCodeClue = newItem.HSCodeClue,
					HSCodeKey = newItem.HSCode,
					Description = newItem.Description,
					HSCode = newItem.HSCode,
					Currency = newItem.Currency,
					Postfix = newItem.Postfix,
					AmountInternal = newItem.AmountInternal,
					NetWeightInternal = newItem.NetWeightInternal,
					NumberInternal = newItem.NumberInternal,
				};
				App.MainView.DeclearationAccum.Add(delta);
			}
			else
			{
				DeclAccumElem delta = new DeclAccumElem()
				{
					InvoiceNo = newItem.InvoiceNo,
					Origin = newItem.Origin,
					HSCodeClue = newItem.HSCodeClue,
					HSCodeKey = newItem.HSCode,
					Description = newItem.Description,
					HSCode = newItem.HSCode,
					Currency = newItem.Currency,
					Postfix = newItem.Postfix,
					AmountInternal = newItem.AmountInternal - oldItem.AmountInternal,
					NetWeightInternal = newItem.NetWeightInternal - oldItem.NetWeightInternal,
					NumberInternal = newItem.NumberInternal - oldItem.NumberInternal,
				};
				App.MainView.DeclearationAccum.Add(delta);
			}
		}//else Accumに増減なし
	};
	public delegate void CountElementDelegate(InvoiceDataElement _this,InvoiceDataElement that);
	public static CountElementDelegate CountDelegate = (InvoiceDataElement oldItem, InvoiceDataElement newItem) => 
	{
		if(newItem == null)
        {
			if ((oldItem.DeclValid & InvoiceDataElement.DeclearValidFlag.CountValid) == InvoiceDataElement.DeclearValidFlag.CountValid)
			{
				App.MainView.DeclearationAccum.Declement(oldItem.Hash());
			}
			return;
		}
		DeclearValidFlag oldFlag = oldItem.DeclValid;
		DeclearValidFlag newFlag = newItem.DeclValid;
		if ((oldFlag & DeclearValidFlag.CountValid) != DeclearValidFlag.CountValid && (newFlag & DeclearValidFlag.CountValid) == DeclearValidFlag.CountValid)
		{
			App.MainView.DeclearationAccum.Inclement(newItem.Hash());
		}
		else if ((newFlag & DeclearValidFlag.CountValid) != DeclearValidFlag.CountValid && (oldFlag & DeclearValidFlag.CountValid) == DeclearValidFlag.CountValid)
		{
			App.MainView.DeclearationAccum.Declement(oldItem.Hash());
		}
		else if ((oldFlag & DeclearValidFlag.CountValid) == DeclearValidFlag.CountValid && (newFlag & DeclearValidFlag.CountValid) == DeclearValidFlag.CountValid)
		{
			if (oldItem.HSCode != newItem.HSCode || oldItem.Currency != newItem.Currency || newItem.Postfix != oldItem.Postfix)
			{
				App.MainView.DeclearationAccum.Declement(oldItem.Hash());
				App.MainView.DeclearationAccum.Inclement(newItem.Hash());
			}
		}//else Element数に増減なし
	};

	public InvoiceDataElement()
	{
	}

	[Flags]
	public enum DeclearValidFlag
    {

		HSCode = 1,
		Currency = 2,
		Origin = 4,
		Amount = 8,
		Number = 16,
		NetWeight = 32,
		ALL = ~(-1 << 6),
		CountValid = HSCode | Currency | Origin,
	}
	[Flags]
	public enum DatabaseValidFlag
	{
		Exist = 1,
		InvoiceNo = 2,
		HSCodeKey = 4,
		HSCodeClue = 8,
		HSCodeSuggestion = 16,
		Description = 32,
		ALL = ~(-1 << 6),
	}

	private DeclearValidFlag _declValid = 0;
	private DatabaseValidFlag _dataValid = 0;

	public DeclearValidFlag DeclValid {
		set
		{
			_declValid = value;
			NofityPropertyChanged("DeclValid");
		}
		get 
		{
			return _declValid;
		}
	} 
	public DatabaseValidFlag DataValid
    {
		set
        {
			_dataValid = value;
			NofityPropertyChanged("DataValid");
		}
		get
        {
			return _dataValid;
        }
    }
	private string _indexNoString; 
	public string IndexNoString {
		set
		{
			_indexNoString = value;
			NofityPropertyChanged("IndexNoString");
		}
		get
		{
			return _indexNoString;
		}
	}
	private string _exist = "";
	public string Exist {
		set
		{
			_exist = value;
			if (!Validate(value, App.MainView.InvoiceRegexPattern.ExistPattern))
            {
				ExistWarningMessage = "存在判定項目のパターンに一致しません。";
				DataValid &= ~DatabaseValidFlag.Exist;
				return;
            }
			ExistWarningMessage = "";
			DataValid |= DatabaseValidFlag.Exist;
		}
		get 
		{
			return _exist;
		}
	}
	private string _existWarningMessage = "";
	public string ExistWarningMessage {
		set
		{
			_existWarningMessage = value;
			NofityPropertyChanged("ExistWarningMessage");

		}
		get 
		{
			return _existWarningMessage;
		}
	}
	private string _invoiceNo = "";
	public string InvoiceNo {
		set 
		{
			InvoiceDataElement oldItem = (InvoiceDataElement)MemberwiseClone();
			_invoiceNo = value;
			DataValid |= DatabaseValidFlag.InvoiceNo;
			AccumDelta(oldItem, this);
		} 
		get
		{
			return _invoiceNo;
		} 
	}

	private string _invoiceNoWarningMessage = "";
	public string InvoiceNoWarningMessage
	{
		set
		{
			_invoiceNoWarningMessage = value;
			NofityPropertyChanged("InvoiceNoWarningMessage");

		}
		get
		{
			return _invoiceNoWarningMessage;
		}
	}
	private string _amount = "";
	private long _amountInternal;
	private string _amountWarningMessage = "";
	public string AmountWarningMessage
	{
		set
		{
			_amountWarningMessage = value;
			NofityPropertyChanged("AmountWarningMessage");

		}
		get
		{
			return _amountWarningMessage;
		}
	}
	public string Amount
	{
		set
		{
			InvoiceDataElement oldItem = (InvoiceDataElement)MemberwiseClone();
			double tempAmount;
			_amount = value;
			NofityPropertyChanged("Amount");
			if (Validate(value, ConstValue.Re_ContainsExNumberCommaPeriod))
            {
				AmountWarningMessage = "数字、カンマ、ピリオド以外は使用できません。";
				DeclValid &= ~DeclearValidFlag.Amount;
				_amount = value;
				AccumDelta(oldItem, this);
				return;
            }
			if (!double.TryParse(value, out tempAmount))
			{
				AmountWarningMessage = "小数点付きの数字として読み取れませんでした。";
				DeclValid &= ~DeclearValidFlag.Amount;
				_amount = value;
				AccumDelta(oldItem, this);
				return;
			}
			AmountWarningMessage = "";
			DeclValid |= DeclearValidFlag.Amount;
			_amountInternal = UtilityFunc.LongRound((long)(tempAmount * (double)ConstValue.InternalValueFactor));
			AccumDelta(oldItem, this);
			
		}
        get
		{
			return _amount;
		}
	}

	public long AmountInternal
    {
        get
        {
			return _amountInternal;
        }
    }

	private string _numberWarningMessage = "";
	public string NumberWarningMessage
	{
		set
		{
			_numberWarningMessage = value;
			NofityPropertyChanged("NumberWarningMessage");

		}
		get
		{
			return _numberWarningMessage;
		}
	}
	private long _numberInternal;
	private string _number = "";
	public string Number
	{
		set
		{
			InvoiceDataElement oldItem = (InvoiceDataElement)MemberwiseClone();
			long tempNumber;
			double tempDoubleNumber;
			_number = value;
			NofityPropertyChanged("Number");
			if (Validate(value, ConstValue.Re_ContainsExNumberCommaPeriod))
			{
				NumberWarningMessage = "数字、カンマ、ピリオド以外は使用できません。";
				DeclValid &= ~DeclearValidFlag.Number;
				AccumDelta(oldItem, this);
				return;
			}
			if (!long.TryParse(value, out tempNumber))
			{
				if (!double.TryParse(value, out tempDoubleNumber))
				{
					NumberWarningMessage = "整数として読み取れませんでした。";
					DeclValid &= ~DeclearValidFlag.Number;
					AccumDelta(oldItem, this);
					return;
                }
                else
                {
					tempNumber = (long)tempDoubleNumber;
					_number = tempNumber.ToString();
                }
			}
			if(tempNumber < 0)
            {
				NumberWarningMessage = "マイナスの数字は入力できません";
				DeclValid &= ~DeclearValidFlag.Number;
				AccumDelta(oldItem, this);
				return;
			}
			NumberWarningMessage = "";
			DeclValid |= DeclearValidFlag.Number;
			_numberInternal = tempNumber * ConstValue.InternalValueFactor;
			AccumDelta(oldItem, this);
		}
		get
		{
			return _number;
		}
	}

	public long NumberInternal 
	{
        get 
		{
			return _numberInternal;
		}
	}
	private string _netWeightWarningMessage = "";
	public string NetWeightWarningMessage
	{
		set
		{
			_netWeightWarningMessage = value;
			NofityPropertyChanged("NetWeightWarningMessage");

		}
		get
		{
			return _netWeightWarningMessage;
		}
	}
	private long _netWeightInternal;
	private string _netWeight = "";
	public string NetWeight
	{
		set
		{
			InvoiceDataElement oldItem = (InvoiceDataElement)MemberwiseClone();
			double tempNetWeight;
			_netWeight = value;
			NofityPropertyChanged("NetWeight");
			if (Validate(value, ConstValue.Re_ContainsExNumberCommaPeriod))
			{
				NetWeightWarningMessage = "数字、カンマ、ピリオド以外は使用できません。";
				DeclValid &= ~DeclearValidFlag.NetWeight;
				AccumDelta(oldItem, this);
				return;
			}
			if (!double.TryParse(value, out tempNetWeight))
			{
				NetWeightWarningMessage = "数字として読み取れませんでした。";
				DeclValid &= ~DeclearValidFlag.NetWeight;
				AccumDelta(oldItem, this);
				return;
			}
			NetWeightWarningMessage = "";
			DeclValid |= DeclearValidFlag.NetWeight;
			_netWeight = value;
			NofityPropertyChanged("NetWeight");
			_netWeightInternal = (long)(tempNetWeight * ConstValue.InternalValueFactor);
			AccumDelta(oldItem, this);
		}
		get
		{
			return _netWeight;
		}
	}

	public long NetWeightInternal
    {
		get
        {
			return _netWeightInternal;
        }
    }

	private string _originWarningMessage = "";
	public string OriginWarningMessage
	{
		set
		{
			_originWarningMessage = value;
			NofityPropertyChanged("OriginWarningMessage");

		}
		get
		{
			return _originWarningMessage;
		}
	}
	private string _origin = "";
	public string Origin
	{
		set
		{
			InvoiceDataElement oldItem = (InvoiceDataElement)MemberwiseClone();
			_origin = value;
			NofityPropertyChanged("Origin");
			if (Validate(value, ConstValue.Re_ContainsExAlphabet))
			{
				OriginWarningMessage = "アルファベット大文字以外は使用できません。";
				DeclValid &= ~DeclearValidFlag.Origin;
				AccumDelta(oldItem, this);
				return;
			}
			if (!Validate(value,ConstValue.Re_Is2Letters))
			{
				OriginWarningMessage = "国コードはアルファベット2文字で入力してください";
				DeclValid &= ~DeclearValidFlag.Origin;
				AccumDelta(oldItem, this);
				return;
			}
			if (!IsMinorForced)
			{
				if (_origin != "JP")
				{
					IsDomestic = false;
					IsForeign = true;
					IsMinorForced = false;
				}
				else
				{
					IsDomestic = true;
					IsForeign = false;
					IsMinorForced = false;	
				}
			}
            OriginWarningMessage = "";
			DeclValid |= DeclearValidFlag.Origin;
			AccumDelta(oldItem, this);
		}
		get
		{
			return _origin;
		}
	}
	private string _hsCodeKeyWarningMessage = "";
	public string HSCodeKeyWarningMessage
	{
		set
		{
			_hsCodeKeyWarningMessage = value;
			NofityPropertyChanged("HSCodeKeyWarningMessage");

		}
		get
		{
			return _hsCodeKeyWarningMessage;
		}
	}
	private string _hsCodeKey ="";
	public string HSCodeKey {
		set
		{
			_hsCodeKey = value;
			NofityPropertyChanged("HSCodeKey");
			if (!Validate(value, App.MainView.InvoiceRegexPattern.HSCodeKeyPattern))
			{
				HSCodeKeyWarningMessage = "HSコードキーのパターンに一致しません。";
				DataValid &= ~DatabaseValidFlag.HSCodeKey;
				return;
			}
			HSCodeKeyWarningMessage = "";
			DataValid |= DatabaseValidFlag.HSCodeKey;
			if ((DeclValid & DeclearValidFlag.HSCode) != DeclearValidFlag.HSCode)
			{
				if (App.MainView.ImmediateHSMaster.ContainsKey(_hsCodeKey) && App.MainView.ImmediateHSMaster[_hsCodeKey].OldHSCode != null)
				{
					HSCode = App.MainView.ImmediateHSMaster[_hsCodeKey].NewHSCode != null ? App.MainView.ImmediateHSMaster[_hsCodeKey].NewHSCode : App.MainView.ImmediateHSMaster[_hsCodeKey].OldHSCode;
				}
				else if(App.MainView.IsHSMasterValid)
				{
					bool isBackWard = App.MainView.CurrentCompanyProfile != null ? App.MainView.CurrentCompanyProfile.hs_backward : false;
					bool isForWard = App.MainView.CurrentCompanyProfile != null ? App.MainView.CurrentCompanyProfile.hs_forward : false;
					bool isNormalize = App.MainView.CurrentCompanyProfile != null ? App.MainView.CurrentCompanyProfile.hs_isnormalize : false;
					HSCode = IVEditElasticClient.GetHSCode(App.MainView.HSMasterIndex, _hsCodeKey, isForWard, isBackWard,isNormalize);
					string regdHS = HSCode.Length > 9 ? HSCode.Substring(0, 9) : "";
					if (App.MainView.ImmediateHSMaster.ContainsKey(_hsCodeKey))
					{
						App.MainView.ImmediateHSMaster[_hsCodeKey].OldHSCode =  regdHS == "" ? null : regdHS;
                    }
                    else
                    {
						App.MainView.ImmediateHSMaster.Add(_hsCodeKey, new ImmediateHSMaster() { OldHSCode = regdHS == "" ? null : regdHS, NewHSCode = null,HSClue = (DataValid & DatabaseValidFlag.HSCodeClue) == DatabaseValidFlag.HSCodeClue ? _hsCodeClue : null}) ;
					}
				}
            }
            else
            {
				string newHS = HSCode.Length > 9 ? HSCode.Substring(0, 9) : HSCode;
				if (App.MainView.ImmediateHSMaster.ContainsKey(_hsCodeKey))
				{
					if (App.MainView.ImmediateHSMaster[_hsCodeKey].OldHSCode != newHS)
                    {
						App.MainView.ImmediateHSMaster[_hsCodeKey].NewHSCode = newHS;
					}
				}
				else if(App.MainView.IsHSMasterValid)
				{
					bool isBackWard = App.MainView.CurrentCompanyProfile != null ? App.MainView.CurrentCompanyProfile.hs_backward : false;
					bool isForWard = App.MainView.CurrentCompanyProfile != null ? App.MainView.CurrentCompanyProfile.hs_forward : false;
					bool isNormalize = App.MainView.CurrentCompanyProfile != null ? App.MainView.CurrentCompanyProfile.hs_isnormalize : false;
					var oldHS = IVEditElasticClient.GetHSCode(App.MainView.HSMasterIndex, _hsCodeKey, isForWard, isBackWard,isNormalize);
					App.MainView.ImmediateHSMaster.Add(_hsCodeKey, new ImmediateHSMaster() { OldHSCode = oldHS == "" ? null : oldHS, NewHSCode = newHS , HSClue = (DataValid & DatabaseValidFlag.HSCodeClue) == DatabaseValidFlag.HSCodeClue ? _hsCodeClue : null });
				}
			}
		}
		get
		{
			return _hsCodeKey;
		}
	}
	private string _hsCodeClueWarningMessage = "";
	public string HSCodeClueWarningMessage
	{
		set
		{
			_hsCodeClueWarningMessage = value;
			NofityPropertyChanged("HSCodeClueWarningMessage");

		}
		get
		{
			return _hsCodeClueWarningMessage;
		}
	}
	private string _hsCodeClue = "";
	public string HSCodeClue {
		set 
		{
			_hsCodeClue = value;
			NofityPropertyChanged("HSCodeClue");
			if (!Validate(value, App.MainView.InvoiceRegexPattern.HSCodeCluePattern))
			{
				HSCodeClueWarningMessage = "HSコードヒントのパターンに一致しません。";
				DataValid &= ~DatabaseValidFlag.HSCodeClue;
				return;
			}
			DataValid |= DatabaseValidFlag.HSCodeClue;
			if ((DataValid & DatabaseValidFlag.HSCodeSuggestion) != DatabaseValidFlag.HSCodeSuggestion)
			{
				if (App.MainView.IsHSMasterValid && App.MainView.CurrentCompanyProfile != null && App.MainView.CurrentCompanyProfile.hs_enablesuggestion)
				{
					HSCodeSuggestion = IVEditElasticClient.GetHSSuggestion(App.MainView.HSMasterIndex, _hsCodeClue);
				}
			}
		}
		get
		{
			return _hsCodeClue;
		}
	}
	private string _currencyWarningMessage = "";
	public string CurrencyWarningMessage
	{
		set
		{
			_currencyWarningMessage = value;
			NofityPropertyChanged("CurrencyWarningMessage");

		}
		get
		{
			return _currencyWarningMessage;
		}
	}
	private string _currency = "";
	public string Currency {
		set
		{
			InvoiceDataElement oldItem = (InvoiceDataElement)MemberwiseClone();
			_currency = value;
			NofityPropertyChanged("Currency");
			if (Validate(value, ConstValue.Re_ContainsExAlphabet))
			{
				CurrencyWarningMessage = "アルファベット大文字以外は使用できません。";
				DeclValid &= ~DeclearValidFlag.Currency;
				AccumDelta(oldItem, this);
				return;
			}
			if (!Validate(value, ConstValue.Re_Is3Letters))
			{
				CurrencyWarningMessage = "通貨コードはアルファベット3文字で入力してください";
				DeclValid &= ~DeclearValidFlag.Currency;
				AccumDelta(oldItem, this);
				return;
			}
			CurrencyWarningMessage = "";
			DeclValid |= DeclearValidFlag.Currency;
			AccumDelta(oldItem, this);
		}
		get
		{
			return _currency;
		}
	}
	private string _descriptionWarningMessage = "";
	public string DescriptionWarningMessage
	{
		set
		{
			_descriptionWarningMessage = value;
			NofityPropertyChanged("Description");
			NofityPropertyChanged("DescriptionWarningMessage");

		}
		get
		{
			return _descriptionWarningMessage;
		}
	}
	private string _description = "";
	public string Description {
		set 
		{
			_description = value;
			DataValid |= DatabaseValidFlag.Description;
		}
		get
		{
			return _description;
		}
	}
	private string _hsCodeWarningMessage = "";
	public string HSCodeWarningMessage
	{
		set
		{
			_hsCodeWarningMessage = value;
			NofityPropertyChanged("HSCodeWarningMessage");

		}
		get
		{
			return _hsCodeWarningMessage;
		}
	}
	private string _hsCode = "";
	public string HSCode
	{
		set
		{
			InvoiceDataElement oldItem = (InvoiceDataElement)MemberwiseClone();
			_hsCode = value;
			NofityPropertyChanged("HSCode");
			if (Validate(value, ConstValue.Re_ContainsExNumber))
			{
				HSCodeWarningMessage = "数字以外使用できません。";
				DeclValid &= ~DeclearValidFlag.HSCode;
				AccumDelta(oldItem, this);
				return;
			}
			if (Validate(value, ConstValue.Re_Is9Numbers))
			{
				string prev = oldItem.HSCode;
				if (prev == null || prev.Length != 10)
				{
					if (NewIVEdit.HSCode.HSCodeDictionary.ContainsKey(value))
					{
						value = NewIVEdit.HSCode.HSCodeDictionary[value].HSCode + NewIVEdit.HSCode.HSCodeDictionary[value].Checkthumb;
						_hsCode = value;
					}
					else
					{
						HSCodeWarningMessage = "存在しないHSコードです";
						DeclValid &= ~DeclearValidFlag.HSCode;
						AccumDelta(oldItem, this);
						return;
					}
				}
			}
			if (!Validate(value, ConstValue.Re_Is10Numbers))
			{
				HSCodeWarningMessage = "HSコードは数字10桁で入力してください";
				DeclValid &= ~DeclearValidFlag.HSCode;
				AccumDelta(oldItem, this);
				return;
			}
			//HSが10桁→実在するかのチェック+チェックサムがあっているかのチェック
			HSCodeWarningMessage = "";
			DeclValid |= DeclearValidFlag.HSCode;
			AccumDelta(oldItem, this);
			if((DataValid & DatabaseValidFlag.HSCodeKey) == DatabaseValidFlag.HSCodeKey)
            {
				string newHS = _hsCode.Length > 9 ? _hsCode.Substring(0, 9) : _hsCode;

				if (App.MainView.ImmediateHSMaster.ContainsKey(HSCodeKey))
                {
					
					if (App.MainView.ImmediateHSMaster[HSCodeKey].OldHSCode != newHS) {
						App.MainView.ImmediateHSMaster[HSCodeKey].NewHSCode = newHS;
						if((DataValid & DatabaseValidFlag.HSCodeClue) == DatabaseValidFlag.HSCodeClue)
                        {
							App.MainView.ImmediateHSMaster[HSCodeKey].HSClue = _hsCodeClue;
						}
					}
					if ((DataValid & DatabaseValidFlag.HSCodeClue) == DatabaseValidFlag.HSCodeClue &&
						((DeclValid & DeclearValidFlag.HSCode) == DeclearValidFlag.HSCode) &&
						(HSCode != HSCodeSuggestion))
					{
						App.MainView.ImmediateHSMaster[HSCodeKey].HSClue = _hsCodeClue;
						App.MainView.ImmediateHSMaster[HSCodeKey].ForceUpdate = true;
                    }
                    else
                    {
						App.MainView.ImmediateHSMaster[HSCodeKey].ForceUpdate = false;
					}

				}
            }
			NofityPropertyChanged("HSCode");
		}
		get
		{
			return _hsCode;
		}
	}
	private string _postfix = "A";
	public string Postfix
    {
		set
		{
			if (_postfix != value)
			{
				InvoiceDataElement oldItem = (InvoiceDataElement)MemberwiseClone();
				_postfix = value;
				AccumDelta(oldItem, this);
			}
		}
        get 
		{
			return _postfix;
		}
    }
	private string _hsCodeSuggestionWarningMessage = "";
	public string HSCodeSuggestionWarningMessage
	{
		set
		{
			_hsCodeSuggestionWarningMessage = value;
			NofityPropertyChanged("HSCodeSuggestionWarningMessage");

		}
		get
		{
			return _hsCodeSuggestionWarningMessage;
		}
	}
	private string _hsCodeSuggestion = "";
	public string HSCodeSuggestion
	{
		set
		{
			InvoiceDataElement oldItem = (InvoiceDataElement)MemberwiseClone();
			_hsCodeSuggestion = value;
			if ((DataValid & DatabaseValidFlag.HSCodeClue) == DatabaseValidFlag.HSCodeClue)
			{
				string newHS = _hsCodeSuggestion.Length > 9 ? _hsCodeSuggestion.Substring(0, 9) : _hsCodeSuggestion;

				if (App.MainView.ImmediateHSMaster.ContainsKey(HSCodeKey))
				{
					if ((DataValid & DatabaseValidFlag.HSCodeClue) == DatabaseValidFlag.HSCodeClue &&
						((DeclValid & DeclearValidFlag.HSCode) == DeclearValidFlag.HSCode) &&
						(HSCode != HSCodeSuggestion))
					{
						App.MainView.ImmediateHSMaster[HSCodeKey].HSClue = _hsCodeClue;
						App.MainView.ImmediateHSMaster[HSCodeKey].ForceUpdate = true;
					}
					else
					{
						App.MainView.ImmediateHSMaster[HSCodeKey].ForceUpdate = false;
					}

				}
			}
			NofityPropertyChanged("HSCodeSuggestion");
			if (Validate(value, ConstValue.Re_ContainsExNumber))
			{
				HSCodeSuggestionWarningMessage = "数字以外使用できません。";
				DataValid &= ~DatabaseValidFlag.HSCodeSuggestion;
				return;
			}
			if (Validate(value, ConstValue.Re_Is9Numbers))
			{
				string prev = oldItem.HSCodeSuggestion;
				if (prev == null || prev.Length != 10)
				{
					if (NewIVEdit.HSCode.HSCodeDictionary.ContainsKey(value))
					{
						value = NewIVEdit.HSCode.HSCodeDictionary[value].HSCode + NewIVEdit.HSCode.HSCodeDictionary[value].Checkthumb;
						_hsCodeSuggestion = value;
					}
					else
					{
						HSCodeSuggestionWarningMessage = "存在しないHSコードです";
						DataValid &= ~DatabaseValidFlag.HSCodeSuggestion;
						return;
					}
				}
			}
			if (!Validate(value, ConstValue.Re_Is10Numbers))
			{
				HSCodeSuggestionWarningMessage = "HSコードは数字10桁で入力してください";
				DataValid &= ~DatabaseValidFlag.HSCodeSuggestion;
				return;
			}
			HSCodeSuggestionWarningMessage = "";
			DataValid |= DatabaseValidFlag.HSCodeSuggestion;
			
		}
		get
		{
			return _hsCodeSuggestion;
		}
	}

	private bool Validate(string text, Regex regex)
    {
		if (text == null || regex == null) return false;
		if (!regex.Match(text).Success) return false;
		else return true;
    }

	bool _isDomestic = true;
	bool _isForeign = false;
	bool _isMinorForced = false;
	public bool IsDomestic {
		set
		{
			if (_isDomestic != value)
			{
				_isDomestic = value;
				NofityPropertyChanged("IsDomestic");
				Postfix = _isDomestic ? "A" : _postfix;
			}
		}
		get
		{
			return _isDomestic;
		}
	}
	public bool IsForeign {
		set
		{
			if (_isForeign != value)
			{
				_isForeign = value;
				NofityPropertyChanged("IsForeign");
				Postfix = _isForeign ? "Y" : _postfix;
			}
		}
		get
		{
			return _isForeign;
		}
	}
	public bool IsMinorForced{
		set
		{
			if (_isMinorForced != value)
			{
				_isMinorForced = value;
				NofityPropertyChanged("IsMinorForced");
				Postfix = _isMinorForced ? "E" : _postfix;
			}
		}
		get
		{
			return _isMinorForced;
		}
	}

	
};

	


