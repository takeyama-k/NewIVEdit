using NewIVEdit;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
public class DeclArticle : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
	{
		if (PropertyChanged != null)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

		}
	}
	private bool _isMinor = false;
	public bool IsMinor {
		set
		{
			_isMinor = value;
			ValidateAll();
			GoodsName = GoodsName;
		}
		get
		{
			return _isMinor;
		}
	}
	public bool IsGaitou { set; get; } = false;
	public bool IsOtherLaw { set; get; } = false;
	private bool _isAnbun = false;
	public bool IsAnbun {
		set
		{
			_isAnbun = value;
			if(ValidateAnbunNetWeight())
            {
				CommonValid |= CommonValidFlag.NetWeight;
            }
            else
            {
				CommonValid &= ~CommonValidFlag.NetWeight;
			}
		}
		get
		{
			return _isAnbun;
		}
	}
	public bool IsShprOverride { set; get; } = false;
	public bool IsCneeOverride { set; get; } = false;

	[Flags]
	public enum CommonValidFlag
	{
		None = 0,
		Payment = 1 << 0,
		TradeTerm = 1 << 1,
		Currency = 1 << 2,
		TermAmount = 1 << 3,
		FOBAmount = 1 << 4,
		DeclSubmiss = 1 << 5,
		DeclSubmissBumon = 1 << 6,
		DayOfDept = 1 << 7,
		BouekiType = 1 << 8,
		NetWeight = 1 << 9,
		QtyExNW = 1 << 10,
		BPR =  1 << 11,
		Daisyou = 1 << 12,
		GDname = 1 << 13,
		All = ~(-1 << 14),
	}
	public CommonValidFlag CommonValid = CommonValidFlag.DayOfDept;
	private string _daisyouWarning = "SかLを入力してください";
	public string DaisyouWarning
	{
		set
		{
			_daisyouWarning = value;
			NofityPropertyChanged("DaisyouWarning");
		}
		get
		{
			return _daisyouWarning;
		}
	}
	private string _daisyou = "";
	public string Daisyou {
		set
		{
			_daisyou = value;
			NofityPropertyChanged("Daisyou");
			if(_daisyou != "S" && _daisyou != "L")
            {
				DaisyouWarning = "SかLを入力してください";
				CommonValid &= ~CommonValidFlag.Daisyou;
				return;
            }
			DaisyouWarning = "";
			CommonValid |= CommonValidFlag.Daisyou;
			if (_daisyou == "S") IsMinor = true;
			else IsMinor = false;
		}
		get
		{
			return _daisyou;
		} 
	}
	private string _declType = "";
	public TextBoxAnnotation DeclTypeAnnot { set; get; }
	public string DeclType {
		set 
		{
			_declType = value;
			if (DeclTypeAnnot != null) DeclTypeAnnot.SetText(value);
			NofityPropertyChanged("DeclType");
		}
		get 
		{
			return _declType;
		}
	}
	private string _declSubmissWarning = "";
	public string DeclSubmissWarning
	{
		set
		{
			_declSubmissWarning = value;
			NofityPropertyChanged("DeclSubmissWarning");
		}
		get
		{
			return _declSubmissWarning;
		}
	}
	public TextBoxAnnotation DeclSubmissAnnot { set; get; }
	private string _declSubmiss = "";
	public string DeclSubmiss
	{
		set
		{
			_declSubmiss = value;
			if (DeclSubmissAnnot != null) DeclSubmissAnnot.SetText(value);
			NofityPropertyChanged("DeclSubmiss");
			if (IsMinor)
			{
				if (_declSubmiss.Length != 2)
				{
					DeclSubmissWarning = "2文字の税関コードを入力してください(空欄不可)";
					CommonValid &= ~CommonValidFlag.DeclSubmiss;
					return;
				}
				if (!ConstValue.ZeikanBumon.ContainsKey(_declSubmiss))
				{
					DeclSubmissWarning = "存在しない税関コードです。";
					CommonValid &= ~CommonValidFlag.DeclSubmiss;
					return;
				}
            }
            else
            {
				if (_declSubmiss.Length != 0 && _declSubmiss.Length != 2)
				{
					DeclSubmissWarning = "2文字の税関コードを入力してください(空欄可)";
					CommonValid &= ~CommonValidFlag.DeclSubmiss;
					return;
				}
				if (_declSubmiss.Length == 2 && !ConstValue.ZeikanBumon.ContainsKey(_declSubmiss))
				{
					DeclSubmissWarning = "存在しない税関コードです。";
					CommonValid &= ~CommonValidFlag.DeclSubmiss;
					return;
				}
			}
			DeclSubmissWarning = "";
			CommonValid |= CommonValidFlag.DeclSubmiss;
		}
		get
		{
			return _declSubmiss;
		}
	}
	private string _declSubmissBumonWarning = "";
	public string DeclSubmissBumonWarning
	{
		set
		{
			_declSubmissBumonWarning = value;
			NofityPropertyChanged("DeclSubmissBumonWarning");
		}
		get
		{
			return _declSubmissBumonWarning;
		}
	}
	public TextBoxAnnotation DeclSubmissBumonAnnot { set; get; } = null;
	private string _declSubmissBumon = "";
	public string DeclSubmissBumon {
		set
		{
			_declSubmissBumon = value;
			if (DeclSubmissBumonAnnot != null) DeclSubmissBumonAnnot.SetText(value);
			NofityPropertyChanged("DeclSubmissBumon");
            if (IsMinor)
            {
				if((int)(CommonValid & CommonValidFlag.DeclSubmiss) == 0)
                {
					DeclSubmissBumonWarning = "先に税関コードを正しく入力してください";
					CommonValid &= ~CommonValidFlag.DeclSubmissBumon;
					return;
				}
				if(_declSubmissBumon.Length != 2)
                {
					DeclSubmissBumonWarning = "2文字の部門コードを入力してください";
					CommonValid &= ~CommonValidFlag.DeclSubmissBumon;
					return;
				}
				if (!ConstValue.ZeikanBumon[_declSubmiss].Contains(_declSubmissBumon))
				{
					DeclSubmissBumonWarning = "指定の税関には存在しない部門コードです";
					CommonValid &= ~CommonValidFlag.DeclSubmissBumon;
					return;
				}
            }
            else
            {
				if (_declSubmiss.Length != 0 && ((int)(CommonValid & CommonValidFlag.DeclSubmiss) == 0))
				{
					DeclSubmissBumonWarning = "先に税関コードを正しく入力してください";
					CommonValid &= ~CommonValidFlag.DeclSubmissBumon;
					return;
				}
				if (_declSubmissBumon.Length != 0 && _declSubmissBumon.Length != 2)
				{
					DeclSubmissBumonWarning = "2文字の部門コードを入力してください(空欄可)";
					CommonValid &= ~CommonValidFlag.DeclSubmissBumon;
					return;
				}
				if (_declSubmiss.Length != 0 && !ConstValue.ZeikanBumon[_declSubmiss].Contains(_declSubmissBumon))
				{
					DeclSubmissBumonWarning = "指定の税関には存在しない部門コードです";
					CommonValid &= ~CommonValidFlag.DeclSubmissBumon;
					return;
				}
			}
			DeclSubmissBumonWarning = "";
			CommonValid |= CommonValidFlag.DeclSubmissBumon;
		}
		get
		{
			return _declSubmissBumon;
		}
	}
	private string _bouekiTypeWarning = "";
	public string BouekiTypeWarning
	{
		set
		{
			_bouekiTypeWarning = value;
			NofityPropertyChanged("BouekiTypeWarning");
		}
		get
		{
			return _bouekiTypeWarning;
		}
	}
	public TextBoxAnnotation BouekiTypeAnnot { set; get; } = null;
	private string _bouekiType = "";
	public string BouekiType {
		set
		{
			_bouekiType = value;
			if (BouekiTypeAnnot != null) BouekiTypeAnnot.SetText(value);
			NofityPropertyChanged("BouekiType");
            if (IsMinor)
            {
				if(_bouekiType != "")
                {
					BouekiTypeWarning = "少額のため空欄にしてください。";
					CommonValid &= ~CommonValidFlag.BouekiType;
					return;
                }
            }
            else
            {
				if(_bouekiType.Length != 0 && _bouekiType.Length != 3)
                {
					BouekiTypeWarning = "3文字で貿易形態符号を入力してください。(空欄可)";
					CommonValid &= ~CommonValidFlag.BouekiType;
					return;
				}
            }
			BouekiTypeWarning = "";
			CommonValid |= CommonValidFlag.BouekiType;
		}
		get
		{
			return _bouekiType;
		}
	}
	public TextBoxAnnotation PortOfDeptAnnot { set; get; } = null;
	private string _portOfDept = "";
	public string PortOfDept {
		set
		{
			_portOfDept = value;
			if (PortOfDeptAnnot != null) PortOfDeptAnnot.SetText(value);
			NofityPropertyChanged("PortOfDept");
		}
		get
		{
			return _portOfDept;
		}
	}
	public TextBoxAnnotation Iata3Annot { set; get; } = null;
	private string _iata3 = "";
	public string Iata3 {
		set
		{
			_iata3 = value;
			if (Iata3Annot != null) Iata3Annot.SetText(value);
			NofityPropertyChanged("Iata3");
		}
		get
		{
			return _iata3;
		} 
	}
	public TextBoxAnnotation DestinationAnnot { set; get; } = null;

	private string _destination = "";
	public string Destination {
		set 
		{
			_destination = value;
			if (DestinationAnnot != null) DestinationAnnot.SetText(value);
			NofityPropertyChanged("Destination");
		}
		get 
		{
			return _destination;
		}
	}
	public TextBoxAnnotation DestinationNameAnnot { set; get; } = null;
	private string _destinationName = "";
	public string DestinationName {
		set 
		{
			_destinationName = value;
			if (DestinationNameAnnot != null) DestinationNameAnnot.SetText(value);
			NofityPropertyChanged("DestionationName");
		}
		get 
		{
			return _destinationName;
		} 
	}
	public TextBoxAnnotation GaitameCodeAnnot { set; get; } = null;
	private string _gaitameCode = "";
	public string GaitameCode {
		set
		{
			_gaitameCode = value;
			if (GaitameCodeAnnot != null) GaitameCodeAnnot.SetText(value);
			NofityPropertyChanged("GaitameCode");
		}
		get 
		{
			return _gaitameCode;
		}
	}
	public TextBoxAnnotation YoutoukakuAnnot { set; get; } = null;
	private string _youtoukaku = "";
	public string Youtoukaku {
		set
		{
			_youtoukaku = value;
			if (YoutoukakuAnnot != null) YoutoukakuAnnot.SetText(value);
			NofityPropertyChanged("Youtoukaku");
		}
		get 
		{
			return _youtoukaku;
		}
	}
	public ObservableCollection<LicenceElement> Licences { set; get; } = new ObservableCollection<LicenceElement>();

	private string _paymentCodeWarning = "AかBかCを入力してください";
	public string PaymentCodeWarning
    {
        set
        {
			_paymentCodeWarning = value;
			NofityPropertyChanged("PaymentCodeWarning");
        }
        get
        {
			return _paymentCodeWarning;
        }
    }
	public TextBoxAnnotation PaymentCodeAnnot { set; get; } = null;

	private string _paymentCode = "";
	public string PaymentCode {
		set 
		{
			_paymentCode = value;
			NofityPropertyChanged("PaymentCode");
			if (PaymentCodeAnnot != null) PaymentCodeAnnot.SetText(value);
			if (_paymentCode != "A" && _paymentCode != "B" && _paymentCode != "C")
            {
				PaymentCodeWarning = "AかBかCを入力してください";
				CommonValid &= ~CommonValidFlag.Payment;
				return;
			}
			CommonValid |= CommonValidFlag.Payment;
			PaymentCodeWarning = "";
		}
		get 
		{
			return _paymentCode;
		}
	}
	private string _tradeTermWarning = "トレードタームを入力してください";
	public string TradeTermWarning
	{
		set
		{
			_tradeTermWarning = value;
			NofityPropertyChanged("TradeTermWarning");
		}
		get
		{
			return _tradeTermWarning;
		}
	}
	public TextBoxAnnotation TradeTermAnnot { set; get; } = null;
	private string _tradeTerm = "";
	public string TradeTerm {
		set
		{
			_tradeTerm = value;
			if (TradeTermAnnot != null) TradeTermAnnot.SetText(value);
			NofityPropertyChanged("TradeTerm");
			if (!ConstValue.Tradeterm.Contains(_tradeTerm))
            {
				TradeTermWarning = "存在しないトレードタームです";
				CommonValid &= ~CommonValidFlag.TradeTerm;
				return;
            }
			TradeTermWarning = "";
			CommonValid |= CommonValidFlag.TradeTerm;
		}
		get 
		{
			return _tradeTerm;
		} 
	}

	public TextBoxAnnotation CurrencyAnnot { set; get; } = null;
	public TextBoxAnnotation CurrencyAnnot2 { set; get; } = null;
	private string _currencyWarning = "通貨を入力してください";
	public string CurrencyWarning
	{
		set
		{
			_currencyWarning = value;
			NofityPropertyChanged("CurrencyWarning");
		}
		get
		{
			return _currencyWarning;
		}
	}
	private string _currency = "";
	public string Currency
	{
		set
		{
			_currency = value;
			if (CurrencyAnnot != null) CurrencyAnnot.SetText(value);
			if (CurrencyAnnot2 != null) CurrencyAnnot2.SetText(value);
			NofityPropertyChanged("Currency");
            if (!ConstValue.Currency.Contains(_currency))
            {
				CurrencyWarning = "存在しない通貨です";
				CommonValid &= ~CommonValidFlag.Currency;
				return;
            }
			CommonValid |= CommonValidFlag.Currency;
			CurrencyWarning = "";
		}
		get
		{
			return _currency;
		}
	}
	private string _termAmountWarning = "価格を入力してください";
	public string TermAmountWarning
	{
		set
		{
			_termAmountWarning = value;
			NofityPropertyChanged("TermAmountWarning");
		}
		get
		{
			return _termAmountWarning;
		}
	}
	public TextBoxAnnotation TermAmountAnnot { set; get; } = null;
	private string _termAmount = "";
	public string TermAmount
	{
		set
		{
			double dummy = 0.0;
			_termAmount = value;
			if (TermAmountAnnot != null) TermAmountAnnot.SetText(value);
			NofityPropertyChanged("TermAmount");
			if(!double.TryParse(_termAmount, out dummy))
            {
				TermAmountWarning = "数字として読み取れません";
				CommonValid &= ~CommonValidFlag.TermAmount;
				return;
            }
			if(dummy <= 0)
            {
				TermAmountWarning = "0やマイナスの数字を入れれないでください";
				CommonValid &= ~CommonValidFlag.TermAmount;
				return;
			}
			TermAmountWarning = "";
			CommonValid |= CommonValidFlag.TermAmount;
		}
		get
		{
			return _termAmount;
		}
	}
	private string _fobAmountWarning = "価格を入力してください";
	public string FOBAmountWarning
	{
		set
		{
			_fobAmountWarning = value;
			NofityPropertyChanged("FOBAmountWarning");
		}
		get
		{
			return _fobAmountWarning;
		}
	}
	public TextBoxAnnotation FOBAmountAnnot { set; get; } = null;
	private string _fobAmount = "";
	public string FOBAmount
	{
		set
		{
			double dummy = 0.0;
			_fobAmount = value;
			if (FOBAmountAnnot != null) FOBAmountAnnot.SetText(value);
			NofityPropertyChanged("FOBAmount");
			if (TradeTerm != "FOB")
			{
				if (!double.TryParse(_fobAmount, out dummy))
				{
					FOBAmountWarning = "数字として読み取れません";
					CommonValid &= ~CommonValidFlag.FOBAmount;
					return;
				}
				if (dummy <= 0)
				{
					FOBAmountWarning = "0やマイナスの数字を入れれないでください";
					CommonValid &= ~CommonValidFlag.FOBAmount;
					return;
				}
            }
            else
            {
				if(_fobAmount != "")
                {
					FOBAmountWarning = "入力できません";
					CommonValid &= ~CommonValidFlag.FOBAmount;
					return;
                }
                else
                {
					FOBAmountWarning = "";
					CommonValid |= CommonValidFlag.FOBAmount;
					return;
				}
            }
			FOBAmountWarning = "";
			CommonValid |= CommonValidFlag.FOBAmount;
		}
		get
		{
			return _fobAmount;
		}
	}

	private string _declDateWarning = "";
	public string DeclDateWarning
	{
		set
		{
			_declDateWarning = value;
			NofityPropertyChanged("DeclDateWarning");
		}
		get
		{
			return _declDateWarning;
		}
	}

	private string _declDate = "";
	public string DeclDate
	{
		set
		{
			long dummy = 0;
			if(!long.TryParse(value, out dummy))
            {
				_declDate = "";
				return;
            }
			_declDate = value;
			NofityPropertyChanged("DeclDate");
		}
		get
		{
			return _declDate;
		}
	}
	public TextBoxAnnotation IVNoToInputAnnot { set; get; } = null;
	private string _ivnoToInput = "";
	public string IVNoToInput
	{
		set
		{
			_ivnoToInput = value;
			if (IVNoToInputAnnot != null) IVNoToInputAnnot.SetText(value);
			NofityPropertyChanged("IVNoToInput");
		}
		get
		{
			return _ivnoToInput;
		}
	}
	public TextBoxAnnotation ZeikanArticle1Annot { set; get; } = null;
	private string _zeikanArticle1 = "";
	public string ZeikanArticle1
	{
		set
		{
			_zeikanArticle1 = value;
			if (ZeikanArticle1Annot != null) ZeikanArticle1Annot.SetText(value);
			NofityPropertyChanged("ZeikanArticle1");
		}
		get
		{
			return _zeikanArticle1;
		}
	}
	public TextBoxAnnotation ZeikanArticle2Annot { set; get; } = null;
	private string _zeikanArticle2 = "";
	public string ZeikanArticle2
	{
		set
		{
			_zeikanArticle2 = value;
			if (ZeikanArticle2Annot != null) ZeikanArticle2Annot.SetText(value);
			NofityPropertyChanged("ZeikanArticle2");
		}
		get
		{
			return _zeikanArticle2;
		}
	}
	public TextBoxAnnotation TuukanArticleAnnot { set; get; } = null;
	private string _tuukanArticle = "";
	public string TuukanArticle
	{
		set
		{
			_tuukanArticle = value;
			if (TuukanArticleAnnot != null) TuukanArticleAnnot.SetText(value);
			NofityPropertyChanged("TuukanArticle");
		}
		get
		{
			return _tuukanArticle;
		}
	}
	public TextBoxAnnotation NinushiArticleAnnot { set; get; } = null;
	private string _ninushiArticle = "";
	public string NinushiArticle
	{
		set
		{
			_ninushiArticle = value;
			if (NinushiArticleAnnot != null) NinushiArticleAnnot.SetText(value);
			NofityPropertyChanged("NinushiArticle");
		}
		get
		{
			return _ninushiArticle;
		}
	}
	public TextBoxAnnotation NinushiSecArticleAnnot { set; get; } = null;
	private string _ninushiSecArticle = "";
	public string NinushiSecArticle
	{
		set
		{
			_ninushiSecArticle = value;
			if (NinushiSecArticleAnnot != null) NinushiSecArticleAnnot.SetText(value);
			NofityPropertyChanged("NinushiSecArticle");
		}
		get
		{
			return _ninushiSecArticle;
		}
	}
	public TextBoxAnnotation NinushiRefArticleAnnot { set; get; } = null;
	private string _ninushiRefArticle = "";
	public string NinushiRefArticle
	{
		set
		{
			_ninushiRefArticle = value;
			if (NinushiRefArticleAnnot != null) NinushiRefArticleAnnot.SetText(value);
			NofityPropertyChanged("NinushiRefArticle");
		}
		get
		{
			return _ninushiRefArticle;
		}
	}
	private string _ivNetWeightWarning = "";
	public string IVNetWeightWarning
	{
		set
		{
			_ivNetWeightWarning = value;
			NofityPropertyChanged("IVNetWeightWarning");
		}
		get
		{
			return _ivNetWeightWarning;
		}
	}
	public TextBoxAnnotation IVNetWeightAnnot { set; get; } = null;
	private string _ivNetWeight = "";
	public string IVNetWeight
	{
		set
		{
			_ivNetWeight = value;
			if (IVNetWeightAnnot != null) IVNetWeightAnnot.SetText(value);
			NofityPropertyChanged("IVNetWeight");
			if (IsMinor)
            {
				if (_ivNetWeight != "")
				{
					IVNetWeightWarning = "少額のため按分重量は入力できません";
					IsAnbun = false;
					CommonValid &= ~CommonValidFlag.NetWeight;
					return;
                }
                else
                {
					IVNetWeightWarning = "";
					IsAnbun = false;
					CommonValid |= CommonValidFlag.NetWeight;
					return;
				}
			}
			else
			{
				if (_ivNetWeight != "" && ConstValue.Re_ContainsExNumberCommaPeriod.IsMatch(_ivNetWeight))
				{
					IVNetWeightWarning = "数字カンマピリオド以外は入力できません。";
					IsAnbun = false;
					CommonValid &= ~CommonValidFlag.NetWeight;
					return;
				}
				double tempIvNetWt = 0.0d;
				if (_ivNetWeight != "" && !double.TryParse(_ivNetWeight, out tempIvNetWt))
				{
					IsAnbun = false;
					IVNetWeightWarning = "数字として読み取れません。";
					CommonValid &= ~CommonValidFlag.NetWeight;
					return;
				}
				if (_ivNetWeight != "" && tempIvNetWt <= 0.0)
				{
					IsAnbun = false;
					IVNetWeightWarning = "マイナス又は0の数字は入力できません";
					CommonValid &= ~CommonValidFlag.NetWeight;
					return;
				}
				IsAnbun = _ivNetWeight == "" ? false : true;
				if (_ivNetWeight != "")
				{
					_ivNetWeight = string.Format("{0:0.000}", tempIvNetWt);
					_ivNetWeight = _ivNetWeight.Length > 1 ? _ivNetWeight.Substring(0, _ivNetWeight.Length - 1) : _ivNetWeight;
				}
				if (!ValidateAnbunNetWeight())
				{
					CommonValid &= ~CommonValidFlag.NetWeight;
				}
				else
				{
					CommonValid |= CommonValidFlag.NetWeight;
				}
				IVNetWeightWarning = "";
				NofityPropertyChanged("IVNetWeight");
			}

		}
		get
		{
			return _ivNetWeight;
		}
	}
	public TextBoxAnnotation NetWeightAnbunAnnot { set; get; } = null;
	private string _netWeightAnbun = "按分：";
	public string NetWeightAnbun
	{
		set
		{
			_netWeightAnbun = value;
			if (NetWeightAnbunAnnot != null) NetWeightAnbunAnnot.SetText(value);
			NofityPropertyChanged("NetWeightAnbun");
		}
		get
		{
			return _netWeightAnbun;
		}
	}
	public TextBoxAnnotation InvNoAnnot { set; get; } = null;
	private string _invNo = "";
	public string InvNo
	{
		set
		{
			_invNo = value;
			if (InvNoAnnot != null) InvNoAnnot.SetText(value);
			NofityPropertyChanged("InvNo");
		}
		get
		{
			return _invNo;
		}
	}
	private string _goodsNameWarning = "";
	public string GoodsNameWarning
	{
		set
		{
			_goodsNameWarning = value;
			NofityPropertyChanged("GoodsNameWarning");
		}
		get
		{
			return _goodsNameWarning;
		}
	}
	public TextBoxAnnotation GoodsNameAnnot { set; get; } = null;
	private string _goodsName = "";
	public string GoodsName
	{
		set
		{
			_goodsName = value;
			if (GoodsNameAnnot != null) GoodsNameAnnot.SetText(value);
			NofityPropertyChanged("GoodsName");
            if (IsMinor)
            {
				if(_goodsName == "")
                {
					GoodsNameWarning = "商品名を入力してください";
					CommonValid &= ~CommonValidFlag.GDname;
					return;
                }
                if (ConstValue.Re_Delete_exUse.IsMatch(_goodsName) || ConstValue.Re_Delete_delim.IsMatch(_goodsName))
                {
					GoodsNameWarning = "使用不可能な文字が含まれます。(英大文字数字,.:+*-/$ (スペース)以外使用不可)";
					CommonValid &= ~CommonValidFlag.GDname;
					return;
				}
				if (ConstValue.Re_Delete_delim.IsMatch(_goodsName))
				{
					GoodsNameWarning = "使用不可能な文字が含まれます。(英大文字数字,.:+*-/$ (スペース)以外使用不可)";
					CommonValid &= ~CommonValidFlag.GDname;
					return;
				}
				if (_goodsName.Length > 40)
				{
					GoodsNameWarning = "長すぎます 40文字以内で入力してください。";
					CommonValid &= ~CommonValidFlag.GDname;
					return;
				}
				CommonValid |= CommonValidFlag.GDname;
				GoodsNameWarning = "";
            }
            else
            {
				CommonValid |= CommonValidFlag.GDname;
				GoodsNameWarning = "";
			}
		}
		get
		{
			return _goodsName;
		}
	}

	public TextBoxAnnotation MinorBeppyouAnnot { set; get; } = null;
	private string _minorBeppyou = "";
	public string MinorBeppyou
	{
		set
		{
			_minorBeppyou = value;
			if (MinorBeppyouAnnot != null) MinorBeppyouAnnot.SetText(value);
			NofityPropertyChanged("MinorBeppyou");
		}
		get
		{
			return _minorBeppyou;
		}
	}

	public TextBoxAnnotation MinorLicenceClassCodeAnnot { set; get; } = null;
	private string _minorLicenceClassCode = "";
	public string MinorLicenceClassCode
	{
		set
		{
			_minorLicenceClassCode = value;
			if (MinorLicenceClassCodeAnnot != null) MinorLicenceClassCodeAnnot.SetText(value);
			NofityPropertyChanged("MinorLicenceClassCode");
		}
		get
		{
			return _minorLicenceClassCode;
		}
	}

	public TextBoxAnnotation MinorOLAnnot { set; get; } = null;
	private string _minorOL = "";
	public string MinorOL
	{
		set
		{
			_minorOL = value;
			if (MinorOLAnnot != null) MinorOLAnnot.SetText(value);
			NofityPropertyChanged("MinorOL");
		}
		get
		{
			return _minorOL;
		}
	}


	public string Miscemiscellaneous { set; get; } = "";

	public string Zouti { set; get; } = "";
	public string shprHoujinCode13 { set; get; } = "";
	public string shprHoujinCode4 { set; get; } = "";
	public string shprDiscCode { set; get; } = "";
	private string _shprName = "";
	public string ShprName {
		set 
		{
			_shprName = value;
			NofityPropertyChanged("ShprName");
			if (CheckShprEdited()) IsShprOverride = true;
		}
		get 
		{
			return _shprName;
		}
	}
	private string _shoprZipCode = "";
	public string ShprZipCode {
		set
		{
			_shoprZipCode = value;
			NofityPropertyChanged("ShprZipCode");
			if (CheckShprEdited()) IsShprOverride = true;
		}
		get
		{
			return _shoprZipCode;
		}		
	}
	private string _shprPref = "";
	public string ShprPref {
		set
		{
			_shprPref = value;
			NofityPropertyChanged("ShprPref");
			if (CheckShprEdited()) IsShprOverride = true;
		}
		get
		{
			return _shprPref;
		}
			
	}
	private string _shprMuni = "";
	public string ShprMuni {
		set 
		{
			_shprMuni = value;
			NofityPropertyChanged("ShprMuni");
			if (CheckShprEdited()) IsShprOverride = true;
		}
		get
		{
			return _shprMuni;
		}		
	}
	private string _shprAddr1 = "";
	public string ShprAddr1 {
		set 
		{
			_shprAddr1 = value;
			NofityPropertyChanged("ShprAddr1");
			if (CheckShprEdited()) IsShprOverride = true;
		}
		get
		{
			return _shprAddr1;
		}		
	}
	private string _shprAddr2 = "";
	public string ShprAddr2 {
		set
		{
			_shprAddr2 = value;
			NofityPropertyChanged("ShprAddr2");
			if (CheckShprEdited()) IsShprOverride = true;
		}
		get 
		{
			return _shprAddr2;
		} 
	}
	private string _shprTelephone = "";
	public string ShprTelephone {
		set
		{
			_shprTelephone = value;
			NofityPropertyChanged("ShprTelephone");
			if (CheckShprEdited()) IsShprOverride = true;
		}
		get 
		{
			return _shprTelephone;
		}
	}
	public string cineeCode { set; get; } = "";
	public string cineeName { set; get; } = "";
	public string cineeAddr1 { set; get; } = "";
	public string cineeAddr2 { set; get; } = "";
	public string cineeAddr3 { set; get; } = "";
	public string cineeAddr4 { set; get; } = "";
	public string cineeZipCode { set; get; } = "";
	public string cineeCountryCode { set; get; } = "";

	public ObservableCollection<DeclElement> DeclElements { set; get; } = new ObservableCollection<DeclElement>();
	public void ValidateAll()
    {
        if (ValidateAnbunNetWeight())
        {
			CommonValid |= CommonValidFlag.NetWeight;
        }
        else
        {
			CommonValid &= ~CommonValidFlag.NetWeight;
        }
		if (ValidateQuantity())
		{
			CommonValid |= CommonValidFlag.QtyExNW;
		}
		else
		{
			CommonValid &= ~CommonValidFlag.QtyExNW;
		}
		if (ValidateBPR())
		{
			CommonValid |= CommonValidFlag.BPR;
		}
		else
		{
			CommonValid &= ~CommonValidFlag.BPR;
		}

	}
	public bool ValidateAnbunNetWeight()
    {
		if (IsMinor) {
            if (IsAnbun) //按分NetWeightが0以上の有効な数字が入っている。
			{
				return false;
            }
			return true;
		}
        else
        {
			if (IsAnbun) { //按分NetWeightが0以上の有効な数字が入っている。
				bool flag = true;
				foreach (var element in DeclElements) {
					if (element.NetWeightValid == (DeclElement.NetWeightValidFlag.All))
					{
						flag = false;
						if (element.Unit1 == "KG") element.Quantity1Warning = "按分NWが入力されています。";
						if (element.Unit2 == "KG") element.Quantity2Warning = "按分NWが入力されています。";
                    }
					else if(element.NetWeightValid == (DeclElement.NetWeightValidFlag.Needed))
					{

						if (element.Unit1 == "KG") element.Quantity1Warning = "";
						if (element.Unit2 == "KG") element.Quantity2Warning = "";
					}
					//NetWeightValidFlag.Needed →単位がKG
					//NetWeightValidFlag.IsNotEmptyOrZero→0以外の数字が入っている。
					//申告欄中に一つでもKG単位かつ0"以上"若しくは空欄"以外"の欄があれば、false;
				}
				return flag;
            }
            else
            {
				bool flag = true;
				foreach (var element in DeclElements)
				{
					if (element.NetWeightValid == (DeclElement.NetWeightValidFlag.All))
					{
						if (element.Unit1 == "KG") element.Quantity1Warning = "";
						if (element.Unit2 == "KG") element.Quantity2Warning = "";
					}
					else if (element.NetWeightValid == (DeclElement.NetWeightValidFlag.Needed))
					{
						flag = false;
						if (element.Unit1 == "KG") element.Quantity1Warning = "NWを入力してください";
						if (element.Unit2 == "KG") element.Quantity2Warning = "NWを入力してください";
					}
				}
				return flag;
			}
        }
    }

	public bool ValidateQuantity()
	{
		if (IsMinor)
		{
			return true;
		}
		else
		{
			bool flag = true;
			foreach (var element in DeclElements)
			{
				if (element.Quantity1Valid == DeclElement.Quantity1ValidFlag.NotZeroOrEmpty || element.Quantity1Valid == DeclElement.Quantity1ValidFlag.Needed)
				{
					flag = false;
					element.Quantity1 = element.Quantity1;
				}
				if (element.Quantity2Valid == DeclElement.Quantity2ValidFlag.NotZeroOrEmpty || element.Quantity2Valid == DeclElement.Quantity2ValidFlag.Needed)
				{
					flag = false;
					element.Quantity2 = element.Quantity2;
				}
			}
			return flag;
		}
	}

	public bool ValidateBPR()
    {
		if (IsMinor) return true;
		else
		{
			bool flag = true;
			foreach (var element in DeclElements)
			{
				if ((int)element.BPRValid == 0)
				{
					flag = false;
					break;
				}
			}
			return flag;
		}
	}

	private bool CheckShprEdited()
    {
		return (_shprName == "" && _shprMuni == "" && _shprMuni == "" && _shprAddr1 == "" && _shprAddr2 == "" && _shoprZipCode == "" && _shprTelephone == "") ? false : true ;
    }
	//申告欄
	public class DeclElement : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

			}
		}
		[Flags]
		public enum NetWeightValidFlag
        {
			Needed = 1,
			NotZeroOrEmpty = 2,
			All = ~(-1 << 2),
        }

		public NetWeightValidFlag NetWeightValid;

		[Flags]
		public enum Quantity1ValidFlag
		{
			Needed = 1,
			NotZeroOrEmpty = 2,
			All = ~(-1 << 2),
		}
		public Quantity1ValidFlag Quantity1Valid;

		[Flags]
		public enum Quantity2ValidFlag
		{
			Needed = 1,
			NotZeroOrEmpty = 2,
			All = ~(-1 << 2),
		}

		public Quantity2ValidFlag Quantity2Valid;
		[Flags]
		public enum BPRValidFlag
		{
			NotZeroOrEmpty = 1,
		}

		public BPRValidFlag BPRValid;
		public TextBoxAnnotation HSCodeAnnot { set; get; } = null;
		private string _hsCode = "";
		public string HSCode
		{
			set
			{
				_hsCode = value;
				if (HSCodeAnnot != null) HSCodeAnnot.SetText(value);
				NofityPropertyChanged("HSCode");
			}
			get
			{
				return _hsCode;
			}
		}
		public TextBoxAnnotation Unit1Annot { set; get; } = null;
		private string _unit1 = "";
		public string Unit1
		{
			set
			{
				_unit1 = value;
				if (Unit1Annot != null) Unit1Annot.SetText(value);
				NofityPropertyChanged("Unit1");
				if (_unit1 == "KG")
				{
					NetWeightValid |= NetWeightValidFlag.Needed;
				}else if(_unit1 != "" && _unit1 != "KG")
                {
					Quantity1Valid |= Quantity1ValidFlag.Needed;
                }
				else
				{
					Quantity1Valid &= ~Quantity1ValidFlag.Needed;
				}
				if (_unit1 != "KG" && _unit2 != "KG")
                {
					NetWeightValid &= ~NetWeightValidFlag.Needed;
				}

				App.MainView.DeclArtcile.ValidateQuantity();

			}
			get
			{
				return _unit1;
			}
		}
		public TextBoxAnnotation Unit2Annot { set; get; } = null;
		private string _unit2 = "";
		public string Unit2
		{
			set
			{
				_unit2 = value;
				if (Unit2Annot != null) Unit2Annot.SetText(value);
				NofityPropertyChanged("Unit2");
				if (_unit2 == "KG")
				{
					NetWeightValid |= NetWeightValidFlag.Needed;
				}
				else if (_unit2 != "" && _unit2 != "KG")
				{
					Quantity2Valid |= Quantity2ValidFlag.Needed;
                }
                else
                {
					Quantity2Valid &= ~Quantity2ValidFlag.Needed;
				}
				if (_unit1 != "KG" && _unit2 != "KG")
				{
					NetWeightValid &= ~NetWeightValidFlag.Needed;
				}
				App.MainView.DeclArtcile.ValidateQuantity();
			}
			get
			{
				return _unit2;
			}
		}
		private string _quantity1Warning = "";
		public string Quantity1Warning
		{
			set
			{
				_quantity1Warning = value;
				NofityPropertyChanged("Quantity1Warning");
			}
			get
			{
				return _quantity1Warning;
			}
		}
		public TextBoxAnnotation Quantity1Annot { set; get; } = null;
		private string _quantity1 = "";
		public string Quantity1
		{
			set
			{
				_quantity1 = value;
				if (Quantity1Annot != null) Quantity1Annot.SetText(value);
				NofityPropertyChanged("Quantity1");
				if (_unit1 != "")
				{
					if (ConstValue.Re_ContainsExNumberCommaPeriod.IsMatch(_quantity1))
					{
						Quantity1Warning = "数字カンマピリオド以外入力できません";
						if (_unit1 == "KG")
						{
							NetWeightValid &= ~NetWeightValidFlag.NotZeroOrEmpty;
                        }
                        else
                        {
							Quantity1Valid &= ~Quantity1ValidFlag.NotZeroOrEmpty;
                        }
						return;
					}
					double tempQuantity = 0.0;
					if (!double.TryParse(_quantity1, out tempQuantity))
					{
						Quantity1Warning = "数値として読み取れません。";
						if (_unit1 == "KG")
						{
							NetWeightValid &= ~NetWeightValidFlag.NotZeroOrEmpty;
						}
						else
						{
							Quantity1Valid &= ~Quantity1ValidFlag.NotZeroOrEmpty;
						}
						return;
					}
					if (tempQuantity < 0.0)
					{
						Quantity1Warning = "マイナスの値は入力できません。";
						if (_unit1 == "KG")
						{
							NetWeightValid &= ~NetWeightValidFlag.NotZeroOrEmpty;
						}
						else
						{
							Quantity1Valid &= ~Quantity1ValidFlag.NotZeroOrEmpty;
						}
						return;
					}
					Quantity1Warning = "";
					_quantity1 = string.Format("{0:0.000}", tempQuantity);
					_quantity1 = _quantity1.Substring(0, _quantity1.Length - 1);
					if (_unit1 == "KG")
					{
						if (tempQuantity == 0.0) NetWeightValid &= ~NetWeightValidFlag.NotZeroOrEmpty;
						else NetWeightValid |= NetWeightValidFlag.NotZeroOrEmpty;
                    }
                    else
                    {
						if (tempQuantity == 0.0)
						{
							Quantity1Valid &= ~Quantity1ValidFlag.NotZeroOrEmpty;
							Quantity1Warning = "KG以外の単位で0を入力できません";
						}
						else Quantity1Valid |= Quantity1ValidFlag.NotZeroOrEmpty;
					}
					NofityPropertyChanged("Quantity1");
					App.MainView.DeclArtcile.ValidateAnbunNetWeight();
                }
                else
                {
					if (_quantity1 != "")
					{
						if (ConstValue.Re_ContainsExNumberCommaPeriod.IsMatch(_quantity1))
						{
							Quantity1Warning = "数字カンマピリオド以外入力できません";
							NetWeightValid &= ~NetWeightValidFlag.NotZeroOrEmpty;
							Quantity1Valid &= ~Quantity1ValidFlag.NotZeroOrEmpty;
							return;
						}
						double tempQuantity = 0.0;
						if (!double.TryParse(_quantity1, out tempQuantity))
						{
							Quantity1Warning = "数値として読み取れません。";
							NetWeightValid &= ~NetWeightValidFlag.NotZeroOrEmpty;
							Quantity1Valid &= ~Quantity1ValidFlag.NotZeroOrEmpty;
							return;
						}
						if (tempQuantity < 0.0)
						{
							Quantity1Warning = "マイナスの値は入力できません。";
							NetWeightValid &= ~NetWeightValidFlag.NotZeroOrEmpty;
							Quantity1Valid &= ~Quantity1ValidFlag.NotZeroOrEmpty;
							return;
						}
						NetWeightValid |= NetWeightValidFlag.NotZeroOrEmpty;
						Quantity1Valid |= Quantity1ValidFlag.NotZeroOrEmpty;
						Quantity1Warning = "単位を入力してください";
					}
					else
					{
						if (_unit1 == "KG")
						{
							NetWeightValid &= ~NetWeightValidFlag.NotZeroOrEmpty;
						}
						Quantity1Valid &= ~Quantity1ValidFlag.NotZeroOrEmpty;
						Quantity1Warning = "";
						NofityPropertyChanged("Quantity1");
					}
				}
			}
			get
			{
				return _quantity1;
			}
		}
		private string _quantity2Warning = "";
		public string Quantity2Warning
		{
			set
			{
				_quantity2Warning = value;
				NofityPropertyChanged("Quantity2Warning");
			}
			get
			{
				return _quantity2Warning;
			}
		}
		public TextBoxAnnotation Quantity2Annot { set; get; } = null;
		private string _quantity2 = "";
		public string Quantity2
		{
			set
			{
				_quantity2 = value;
				if (Quantity2Annot != null) Quantity2Annot.SetText(value);
				NofityPropertyChanged("Quantity2");
				if(_unit2 != "") {
					if (ConstValue.Re_ContainsExNumberCommaPeriod.IsMatch(_quantity2))
					{
						Quantity2Warning = "数字カンマピリオド以外入力できません";
						if (_unit2 == "KG")
						{
							NetWeightValid &= ~NetWeightValidFlag.NotZeroOrEmpty;
						}
						else
						{
							Quantity2Valid &= ~Quantity2ValidFlag.NotZeroOrEmpty;
						}
						return;
					}
					double tempQuantity = 0.0;
					if (!double.TryParse(_quantity2, out tempQuantity))
					{
						Quantity2Warning = "数値として読み取れません。";
						if (_unit2 == "KG")
						{
							NetWeightValid &= ~NetWeightValidFlag.NotZeroOrEmpty;
						}
						else
						{
							Quantity2Valid &= ~Quantity2ValidFlag.NotZeroOrEmpty;
						}
						return;
					}
					if (tempQuantity < 0.0)
					{
						Quantity2Warning = "マイナスの値は入力できません。";
						if (_unit2 == "KG")
						{
							NetWeightValid &= ~NetWeightValidFlag.NotZeroOrEmpty;
						}
						else
						{
							Quantity2Valid &= ~Quantity2ValidFlag.NotZeroOrEmpty;
						}
						return;
					}
					Quantity2Warning = "";
					_quantity2 = string.Format("{0:0.000}", tempQuantity);
					_quantity2 = _quantity2.Substring(0, _quantity2.Length - 1);
					if (_unit2 == "KG")
					{
						if (tempQuantity == 0.0) NetWeightValid &= ~NetWeightValidFlag.NotZeroOrEmpty;
						else NetWeightValid |= NetWeightValidFlag.NotZeroOrEmpty;
                    }
                    else
                    {
						if (tempQuantity == 0.0)
						{
							Quantity2Valid &= ~Quantity2ValidFlag.NotZeroOrEmpty;
							Quantity2Warning = "KG以外の単位で0を入力できません";
						}
						else Quantity2Valid |= Quantity2ValidFlag.NotZeroOrEmpty;
					}
					NofityPropertyChanged("Quantity2");
					App.MainView.DeclArtcile.ValidateAnbunNetWeight();
                }
                else
                {
					if (_quantity2 != "")
					{
						if (ConstValue.Re_ContainsExNumberCommaPeriod.IsMatch(_quantity2))
						{
							Quantity2Warning = "数字カンマピリオド以外入力できません";
							NetWeightValid &= ~NetWeightValidFlag.NotZeroOrEmpty;
							Quantity2Valid &= ~Quantity2ValidFlag.NotZeroOrEmpty;
							return;
						}
						double tempQuantity = 0.0;
						if (!double.TryParse(_quantity2, out tempQuantity))
						{
							Quantity2Warning = "数値として読み取れません。";
							NetWeightValid &= ~NetWeightValidFlag.NotZeroOrEmpty;
							Quantity2Valid &= ~Quantity2ValidFlag.NotZeroOrEmpty;
							return;
						}
						if (tempQuantity < 0.0)
						{
							Quantity2Warning = "マイナスの値は入力できません。";
							NetWeightValid &= ~NetWeightValidFlag.NotZeroOrEmpty;
							Quantity2Valid &= ~Quantity2ValidFlag.NotZeroOrEmpty;
							return;
						}
						NetWeightValid |= NetWeightValidFlag.NotZeroOrEmpty;
						Quantity2Valid |= Quantity2ValidFlag.NotZeroOrEmpty;
						Quantity2Warning = "単位を入力してください";
					}
					else
					{
						if (_unit2 == "KG")
						{
							NetWeightValid &= ~NetWeightValidFlag.NotZeroOrEmpty;
						}
						Quantity2Valid &= ~Quantity2ValidFlag.NotZeroOrEmpty;
						Quantity2Warning = "";
						NofityPropertyChanged("Quantity2");
					}
				}
			}
			get
			{
				return _quantity2;
			}
		}

		public TextBoxAnnotation BeppyouCodeAnnot { set; get; } = null;
		private string _beppyouCode = "";
		public string BeppyouCode
		{
			set
			{
				_beppyouCode = value;
				if (BeppyouCodeAnnot != null) BeppyouCodeAnnot.SetText(value);
				NofityPropertyChanged("BeppyouCode");
			}
			get
			{
				return _beppyouCode;
			}
		}
		public TextBoxAnnotation LicenceClassCodeAnnot { set; get; } = null;
		private string _licenceClassCode = "";
		public string LicenceClassCode
		{
			set
			{
				_licenceClassCode = value;
				if (LicenceClassCodeAnnot != null) LicenceClassCodeAnnot.SetText(value);
				NofityPropertyChanged("LicenceClassCode");
			}
			get
			{
				return _licenceClassCode;
			}
		}

		public TextBoxAnnotation CurrencyAnnot { set; get; } = null;
		private string _currency = "";
		public string Currency
		{
			set
			{
				_currency = value;
				if (CurrencyAnnot != null) CurrencyAnnot.SetText(value);
				NofityPropertyChanged("Currency");
			}
			get
			{
				return _currency;
			}
		}
		private string _basicPriceWarning = "";
		public string BasicPriceWarning
		{
			set
			{
				_basicPriceWarning = value;
				NofityPropertyChanged("BasicPriceWarning");
			}
			get
			{
				return _basicPriceWarning;
			}
		}
		public TextBoxAnnotation BasicPriceAnnot { set; get; } = null;
		private string _basicPrice = "";
		public string BasicPrice
		{
			set
			{
				_basicPrice = value;
				if (BasicPriceAnnot != null) BasicPriceAnnot.SetText(value);
				NofityPropertyChanged("BasicPrice");
				if (App.MainView.DeclArtcile.DeclElements.Count(item => item.Currency == App.MainView.DeclArtcile.DeclElements[0].Currency) != 1 || _basicPrice != "")
				{
					if (ConstValue.Re_ContainsExNumberCommaPeriod.IsMatch(_basicPrice))
					{
						BPRValid &= ~BPRValidFlag.NotZeroOrEmpty;
						BasicPriceWarning = "数字カンマピリオド以外入力できません";
						return;
					}
					double tempBPR = 0.0;
					if (!double.TryParse(_basicPrice, out tempBPR))
					{
						BPRValid &= ~BPRValidFlag.NotZeroOrEmpty;
						BasicPriceWarning = "数値として読み取れません。";
						return;
					}
					if (tempBPR <= 0.0)
					{
						BPRValid &= ~BPRValidFlag.NotZeroOrEmpty;
						BasicPriceWarning = "マイナスの値は入力できません。";
						return;
					}
				}
				BasicPriceWarning = "";
				BPRValid |= BPRValidFlag.NotZeroOrEmpty;

			}
			get
			{
				return _basicPrice;
			}
		}

		public TextBoxAnnotation TaxCodeAnnot { set; get; } = null;
		private string _taxCode = "";
		public string TaxCode
		{
			set
			{
				_taxCode = value;
				if (TaxCodeAnnot != null) TaxCodeAnnot.SetText(value);
				NofityPropertyChanged("TaxCode");
			}
			get
			{
				return _taxCode;
			}
		}
		public TextBoxAnnotation OtherLawCodeAnnot { set; get; } = null;
		private string _otherLawCode = "";
		public string OtherLawCode
		{
			set
			{
				_otherLawCode = value;
				if (OtherLawCodeAnnot != null) OtherLawCodeAnnot.SetText(value);
				NofityPropertyChanged("OtherLawCode");
			}
			get
			{
				return _otherLawCode;
			}
		}

		public TextBoxAnnotation TaxArticleAnnot { set; get; } = null;
		private string _taxArticle = "";
		public string TaxArticle
		{
			set
			{
				_taxArticle = value;
				if (TaxArticleAnnot != null) TaxArticleAnnot.SetText(value);
				NofityPropertyChanged("TaxArticle");
			}
			get
			{
				return _taxArticle;
			}
		}
	}
	public class LicenceElement : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

			}
		}
		public TextBoxAnnotation LicenceTypeCodeAnnot { set; get; } = null;
		private string _licenceTypeCode = "";
		public string LicenceTypeCode
		{
			set
			{
				_licenceTypeCode = value;
				if (LicenceTypeCodeAnnot != null) LicenceTypeCodeAnnot.SetText(value);
				NofityPropertyChanged("LicenceTypeCode");
			}
			get
			{
				return _licenceTypeCode;
			}
		}

		public TextBoxAnnotation LicenceNoAnnot { set; get; } = null;
		private string _licenceNo = "";
		public string LicenceNo
		{
			set
			{
				_licenceNo = value;
				if (LicenceNoAnnot != null) LicenceNoAnnot.SetText(value);
				NofityPropertyChanged("LicenceNo");
			}
			get
			{
				return _licenceNo;
			}
		}

	}
}
