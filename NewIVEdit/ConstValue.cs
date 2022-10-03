using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace NewIVEdit
{
	public static class ConstValue
	{
		public static string HSCodeFolder = "HSCode";
		public static string HSCodeFile = "HSCode.xlsx";
		public static string PrinterSettingFile = "printer.ini";
		public static SortedList<string, SortedSet<string>> ZeikanBumon = new SortedList<string, SortedSet<string>>();
		public static SortedSet<string> Currency { set; get; } = new SortedSet<string>();
		public static List<string> Tradeterm { set; get; } = new List<string>();
		static ConstValue()
		{
			ZeikanBumon.Add("1H", new SortedSet<string>() { "01", "02", "21" });
			ZeikanBumon.Add("1M", new SortedSet<string>() { "12"});
			ZeikanBumon.Add("2A", new SortedSet<string>() { "01", "02", "88" });


			Currency.Add("JPY");
			Currency.Add("USD");
			Currency.Add("SEK");
			Currency.Add("EUR");
			Currency.Add("GBP");
			Currency.Add("CAD");
			Currency.Add("PLN");
			Currency.Add("DKK");
			Currency.Add("NOK");
			Currency.Add("AUD");
			Currency.Add("CHF");
			Currency.Add("CNY");
			Currency.Add("HKD");
			Currency.Add("INR");
			Currency.Add("KRW");
			Currency.Add("MYR");
			Currency.Add("MXN");
			Currency.Add("NZD");
			Currency.Add("PHP");
			Currency.Add("SGD");
			Currency.Add("TWD");
			Currency.Add("THB");
			Currency.Add("ZAR");
			Currency.Add("CZK");
			Currency.Add("IDR");
			Currency.Add("TRY");
			Currency.Add("RUB");
			Currency.Add("BRL");
			Currency.Add("HUF");
			Currency.Add("ARS");
			Currency.Add("COP");
			Currency.Add("CLP");
			Currency.Add("KWD");


			Tradeterm.Add("FOB");
			Tradeterm.Add("CIF");
			Tradeterm.Add("EXW");
			Tradeterm.Add("FCA");
			Tradeterm.Add("C&I");
			Tradeterm.Add("C&F");
			Tradeterm.Add("FAS");
			Tradeterm.Add("DAP");
			Tradeterm.Add("DAF");
			Tradeterm.Add("DES");
			Tradeterm.Add("DDU");
			Tradeterm.Add("DPU");
			Tradeterm.Add("DAT");
			Tradeterm.Add("DEQ");
			Tradeterm.Add("DDP");
			Tradeterm.Add("CFR");
			Tradeterm.Add("CPT");
			Tradeterm.Add("CIP");

			ConverterConditionDelegates.Add(ConverterConditionEnum.EQUAL.ToString(), (string _this, string that) => 
			{
				double tempThis, tempThat;

				if(double.TryParse(_this,out tempThis) && double.TryParse(that, out tempThat))
                {
					if (_this == that) return true;
					else return false;
                }
                else
                {
					int c = _this.CompareTo(that);
					if (c == 0) return true;
					else return false;
                }
			});
			ConverterConditionDelegates.Add(ConverterConditionEnum.NOT_EQUAL.ToString(), (string _this, string that) =>
			{
				double tempThis, tempThat;

				if (double.TryParse(_this, out tempThis) && double.TryParse(that, out tempThat))
				{
					if (_this != that) return true;
					else return false;
				}
				else
				{
					int c = _this.CompareTo(that);
					if (c == 1 || c == -1) return true;
					else return false;
				}
			});
			ConverterConditionDelegates.Add(ConverterConditionEnum.GREATER_THAN.ToString(), (string _this, string that) =>
			{
				double tempThis, tempThat;

				if (double.TryParse(_this, out tempThis) && double.TryParse(that, out tempThat))
				{
					return tempThis > tempThat;
				}
				else
				{
					int c = _this.CompareTo(that);
					if (c == 1) return true;
					else return false;
				}
			});

			ConverterConditionDelegates.Add(ConverterConditionEnum.EQUAL_GREATER_THAN.ToString(), (string _this, string that) =>
			{
				double tempThis, tempThat;

				if (double.TryParse(_this, out tempThis) && double.TryParse(that, out tempThat))
				{
					return tempThis >= tempThat;
				}
				else
				{
					int c = _this.CompareTo(that);
					if (c == 1 || c == 0) return true;
					else return false;
				}
			});

			ConverterConditionDelegates.Add(ConverterConditionEnum.LESS_THAN.ToString(), (string _this, string that) =>
			{
				double tempThis, tempThat;

				if (double.TryParse(_this, out tempThis) && double.TryParse(that, out tempThat))
				{
					return tempThis < tempThat;
				}
				else
				{
					int c = _this.CompareTo(that);
					if (c == -11) return true;
					else return false;
				}
			});

			ConverterConditionDelegates.Add(ConverterConditionEnum.EQUAL_LESS_THAN.ToString(), (string _this, string that) =>
			{
				double tempThis, tempThat;

				if (double.TryParse(_this, out tempThis) && double.TryParse(that, out tempThat))
				{
					return tempThis <= tempThat;
				}
				else
				{
					int c = _this.CompareTo(that);
					if (c == -11 || c == 0) return true;
					else return false;
				}
			});

			ConverterActionDelegates.Add(ConverterActionEnum.DIVIDE_INT.ToString(), (string _this, string that) =>
			{
				double tempThis;
				int tempThat;
				string res = "";

				if (double.TryParse(_this, out tempThis) && int.TryParse(that, out tempThat))
				{
					tempThis = tempThis * (double)(InternalValueFactor / tempThat) / (double)InternalValueFactor;
					//暫定的に、小数点2位まで。
					res = string.Format("{0:0.000}",tempThis);
					if (res.Length > 1) res = res.Substring(0, res.Length - 1);
					return res;
				}
				else if (double.TryParse(_this, out tempThis))
				{
					return _this;
				}
				else if (int.TryParse(that, out tempThat))
				{
					return that;
				}
				else
				{
					return "";
				}
			});
			ConverterActionDelegates.Add(ConverterActionEnum.DIVIDE_DOUBLE.ToString(), (string _this, string that) =>
			{
				double tempThis;
				double tempThat;
				string res = "";

				if (double.TryParse(_this, out tempThis) && double.TryParse(that, out tempThat))
				{
					if (tempThat == 0.0) return res;
					tempThis = tempThis  / tempThat;
					//暫定的に、小数点2位まで。
					res = string.Format("{0:0.000}", tempThis);
					if (res.Length > 1) res = res.Substring(0, res.Length - 1);
					return res;
				}
				else if (double.TryParse(_this, out tempThis))
				{
					return _this;
				}
				else if (double.TryParse(that, out tempThat))
				{
					return that;
				}
				else
				{
					return "";
				}
			});
			ConverterActionDelegates.Add(ConverterActionEnum.MULTIPLY_INT.ToString(), (string _this, string that) =>
			{
				double tempThis;
				int tempThat;
				string res = "";

				if (double.TryParse(_this, out tempThis) && int.TryParse(that, out tempThat))
				{
					tempThis = tempThis * tempThat;
					//暫定的に、小数点2位まで。
					res = string.Format("{0:0.000}", tempThis);
					if (res.Length > 1) res = res.Substring(0, res.Length - 1);
					return res;
				}
				else if (double.TryParse(_this, out tempThis))
				{
					return _this;
				}
				else if (int.TryParse(that, out tempThat))
				{
					return that;
				}
				else
				{
					return "";
				}
			});

			ConverterActionDelegates.Add(ConverterActionEnum.MULTIPLY_DOUBLE.ToString(), (string _this, string that) =>
			{
				double tempThis;
				double tempThat;
				string res = "";

				if (double.TryParse(_this, out tempThis) && double.TryParse(that, out tempThat))
				{
					tempThis = tempThis * tempThat;
					//暫定的に、小数点2位まで。
					res = string.Format("{0:0.000}", tempThis);
					if (res.Length > 1) res = res.Substring(0, res.Length - 1);
					return res;
				}
				else if (double.TryParse(_this, out tempThis))
				{
					return _this;
				}
				else if (double.TryParse(that, out tempThat))
				{
					return that;
				}
				else
				{
					return "";
				}
			});
			ConverterActionDelegates.Add(ConverterActionEnum.ADD.ToString(), (string _this, string that) =>
			{
				double tempThis;
				double tempThat;
				string res = "";

				if (double.TryParse(_this, out tempThis) && double.TryParse(that, out tempThat))
				{
					tempThis = tempThis + tempThat;
					//暫定的に、小数点2位まで。
					res = string.Format("{0:0.000}", tempThis);
					if (res.Length > 1) res = res.Substring(0, res.Length - 1);
					return res;
				}
				else if (double.TryParse(_this, out tempThis))
				{
					return _this;
				}
				else if (double.TryParse(that, out tempThat))
				{
					return that;
				}else
                {
					return "";
                }
			});

		}

		public const long GOLD = 2010000000;
		public static int InternalValueFactor = 10000;
		public enum EditingModeEnum
		{
			Ink,
			Hand,
			Ruler,
		}

		public enum HSGroupClass
        {
			MINOR_FORCED_SINGULAR,
			MINOR_FORCED_PLURAL,
			MINOR_SINGULAR,
			MINOR_PLURAL,
			MAJOR,
			MAJOR_FOREIGN_ORIGIN,
        }
		public delegate bool ConverterConditionDelegate(string _this,string that);
		public delegate string ConverterActionDelegate(string _this, string that);
		public static SortedList<string, ConverterConditionDelegate> ConverterConditionDelegates = new SortedList<string, ConverterConditionDelegate>();

		public static SortedList<string, ConverterActionDelegate> ConverterActionDelegates = new SortedList<string, ConverterActionDelegate>();
		public enum ConverterConditionEnum
        {
			EQUAL,
			NOT_EQUAL,
			LESS_THAN,
			GREATER_THAN,
			EQUAL_LESS_THAN,
			EQUAL_GREATER_THAN,
			SMAE,
			MATCH,
        }

		public enum ConverterActionEnum
        {
			ADD,
			SUBTRACT,
			MULTIPLY_DOUBLE,
			MULTIPLY_INT,
			DIVIDE_DOUBLE,
			DIVIDE_INT,
		}

		public enum FieldNameEnum
        {
			HSCODE,
			CURRENCY,
			ORIGIN,
			AMOUNT,
			NUMBER,
			NETWEIGHT,
			EXIST,
			INVOICENO,
			HSCODEKEY,
			HSCODECLUE,
			HSCODESUGGESTION,
			DESCRIPTION,
		}
		public enum InvoiceDataModeEnum
        {
			DIRECT,
			FROM_CLUE,
			DIC_CURRENCY,
			DIC_ORIGIN,
        }

		public static Regex Re_ContainsExNumber = new Regex(@"[^0-9]", RegexOptions.Compiled);
		public static Regex Re_ContainsExNumberCommaPeriod = new Regex(@"[^0-9,.]", RegexOptions.Compiled);
		public static Regex Re_ContainsExAlphabet = new Regex(@"[^A-Z]", RegexOptions.Compiled);
		public static Regex Re_ContainsExNumberAlphabet = new Regex(@"[^0-9A-Z]", RegexOptions.Compiled);
		public static Regex Re_Is9Numbers = new Regex(@"^[0-9]{9}$",RegexOptions.Compiled);
		public static Regex Re_Is10Numbers = new Regex(@"^[0-9]{10}$", RegexOptions.Compiled);
		public static Regex Re_Is3Letters = new Regex(@"^[A-Z]{3}$", RegexOptions.Compiled);
		public static Regex Re_Is2Letters = new Regex(@"^[A-Z]{2}$", RegexOptions.Compiled);
		

		public static string QrDataDelimiter = "%";
		public static Regex Re_Delete_delim = new Regex(@"[" + QrDataDelimiter + @"]", RegexOptions.Compiled);
		public static Regex Re_Delete_exUse = new Regex(@"[^0-9A-Z.:/$%+*\-,\&\s]", RegexOptions.Compiled);
		public static Regex Re_Delete_Alpha = new Regex(@"[^0-9.:/$%+*\-,\&\s]", RegexOptions.Compiled);
		public static Regex Re_Delete_comma = new Regex(",", RegexOptions.Compiled);
		public static Regex Re_Delete_period = new Regex(@"\.", RegexOptions.Compiled);
		public static Regex Re_Delete_space = new Regex(@"\s", RegexOptions.Compiled);
		public static Regex Re_PriceQty = new Regex(@"(?<upper>[0-9]*)(\.(?<lower>[0-9]{2,}))?", RegexOptions.Compiled);
		public static Regex Re_HSCode = new Regex(@"(?<nine>[0-9]{1,9})(?<postfix>[a-zA-Z0-9]{1})?", RegexOptions.Compiled);
		public static Regex Re_Gaitame = new Regex(@"(?<code>[0-9]{5})", RegexOptions.Compiled);
		public static Regex Re_DeleteExDigitAndPeriod = new Regex(@"[^0-9.]", RegexOptions.Compiled);

		public static Regex Re_CovertHyphenLikeCharacter = new Regex(@"[_=₋―ー‐－～￣—ｰ]");
		public static Regex Re_DeleteExDigit = new Regex(@"[^0-9]");
		public static Regex Re_DeleteExAlphaDigitHyphen = new Regex(@"[^A-Z0-9\-]");
	}
}


