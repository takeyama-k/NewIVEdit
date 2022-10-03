using System;
using System.Text.RegularExpressions;
using NewIVEdit;
using System.Collections.Generic;

public class InvoiceRegexPattern
{
	public InvoiceRegexPattern()
	{
	}
	public Regex ExistPattern = null;
	public bool IsExistBackward = false;
	public Regex ExistSecondPattern = null;
	public bool IsExistSecondBackward = false;
	public Regex InvoiceNoPattern = null;
	public bool IsInvoiceNoBackward = false;
	public Regex AmountPattern = null;
	public bool IsAmountBackward = false;
	public Regex NumberPattern = null;
	public bool IsNumberBackward = false;
	public Regex NetWeightPattern = null;
	public bool IsNetWeightBackward = false;
	public Regex OriginPattern = null;
	public bool IsOriginBackward = false;
	public Regex HSCodeKeyPattern = null;
	public bool IsHSCodeKeyBackward = false;
	public Regex HSCodeCluePattern = null;
	public bool IsHSCodeClueBackward = false;
	public Regex CurrencyPattern = null;
	public bool IsCurrencyBackward = false;
	public Regex DescriptionPattern = null;
	public bool IsDescriptionBackward = false;
	public Regex Ext_ExistPattern = null;
	public Regex Ext_ExistSecondPattern = null;
	public Regex Ext_InvoiceNoPattern = null;
	public Regex Ext_AmountPattern = null;
	public Regex Ext_NumberPattern = null;
	public Regex Ext_NetWeightPattern = null;
	public Regex Ext_OriginPattern = null;
	public Regex Ext_HSCodeKeyPattern = null;
	public Regex Ext_HSCodeCluePattern = null;
	public Regex Ext_CurrencyPattern = null;
	public Regex Ext_DescriptionPattern = null;
	public void Load(bool isPackingList = false)
	{
		Dictionary<string, string> invoicePattern = new Dictionary<string, string>();
		Dictionary<string, int> fuzzyLevel = new Dictionary<string, int>();
		Dictionary<string, bool> isCompMatch = new Dictionary<string, bool>();
		Dictionary<string, string> invoicePrefixPattern = new Dictionary<string, string>();
		Dictionary<string, string> invoicePostfixPattern = new Dictionary<string, string>();
		Dictionary<string, bool> isBackward = new Dictionary<string, bool>();
		var curSubtype = App.MainView.CurrentCompanyProfile;
		if (curSubtype == null || curSubtype.datacoord == null) return;
		foreach (var coord in curSubtype.datacoord)
		{
			invoicePattern.Add(coord.label, coord.pattern);
			invoicePrefixPattern.Add(coord.label, coord.prefix_pattern);
			invoicePostfixPattern.Add(coord.label, coord.postfix_pattern);
			fuzzyLevel.Add(coord.label, coord.fuzzy_level);
			isCompMatch.Add(coord.label, coord.is_completematch);
			isBackward.Add(coord.label, coord.is_backward);
		}
		string existkey = "";
		string existsecondkey = "";
		string invnokey = "";
		string amtkey = "";
		string numberkey = "";
		string netwtkey = "";
		string orkey = "";
		string desckey = "";
		string hskey = "";
		string hssuggestkey = "";
		string curkey = "";
		if (isPackingList)
		{
			existkey = curSubtype.packing_existby ?? "";
			existsecondkey = curSubtype.packing_existby_secondary ?? "";
			invnokey = curSubtype.packing_invnoby ?? "";
			amtkey = curSubtype.packing_amtby ?? "";
			numberkey = curSubtype.packing_numberby ?? "";
			netwtkey = curSubtype.packing_netwtby ?? "";
			orkey = curSubtype.packing_orby ?? "";
			desckey = curSubtype.packing_descby ?? "";
			hskey = curSubtype.packing_hsby ?? "";
			hssuggestkey = curSubtype.packing_hssuggestby ?? "";
			curkey = curSubtype.packing_curby ?? "";
		}
		else
		{
			existkey = curSubtype.existby ?? "";
			existsecondkey = curSubtype.existby_secondary ?? "";
			invnokey = curSubtype.invnoby ?? "";
			amtkey = curSubtype.amtby ?? "";
			numberkey = curSubtype.numberby ?? "";
			netwtkey = curSubtype.netwtby ?? "";
			orkey = curSubtype.orby ?? "";
			desckey = curSubtype.descby ?? "";
			hskey = curSubtype.hsby ?? "";
			hssuggestkey = curSubtype.hssuggestby ?? "";
			curkey = curSubtype.curby ?? "";
		}
		if (curSubtype == null) return;
		if (curSubtype.valid_patterns == null)
		{
			
			ExistPattern = invoicePattern.ContainsKey(existkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[existkey], invoicePrefixPattern[existkey], invoicePostfixPattern[existkey], isCompMatch[existkey], fuzzyLevel[existkey]) : null;
			ExistSecondPattern = invoicePattern.ContainsKey(existsecondkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[existsecondkey], invoicePrefixPattern[existsecondkey], invoicePostfixPattern[existsecondkey], isCompMatch[existsecondkey], fuzzyLevel[existsecondkey]) : null;
			InvoiceNoPattern = invoicePattern.ContainsKey(invnokey) ? UtilityFunc.ParseRegexModoki(invoicePattern[invnokey], invoicePrefixPattern[invnokey], invoicePostfixPattern[invnokey], isCompMatch[invnokey], fuzzyLevel[invnokey]) : null;
			AmountPattern = invoicePattern.ContainsKey(amtkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[amtkey], invoicePrefixPattern[amtkey], invoicePostfixPattern[amtkey], isCompMatch[amtkey], fuzzyLevel[amtkey]) : null;
			NumberPattern = invoicePattern.ContainsKey(numberkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[numberkey], invoicePrefixPattern[numberkey], invoicePostfixPattern[numberkey], isCompMatch[numberkey], fuzzyLevel[numberkey]) : null;
			NetWeightPattern = invoicePattern.ContainsKey(netwtkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[netwtkey], invoicePrefixPattern[netwtkey], invoicePostfixPattern[netwtkey], isCompMatch[netwtkey], fuzzyLevel[netwtkey]) : null;
			OriginPattern = invoicePattern.ContainsKey(orkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[orkey], invoicePrefixPattern[orkey], invoicePostfixPattern[orkey], isCompMatch[orkey], fuzzyLevel[orkey]) : null;
			HSCodeKeyPattern = invoicePattern.ContainsKey(hskey) ? UtilityFunc.ParseRegexModoki(invoicePattern[hskey], invoicePrefixPattern[hskey], invoicePostfixPattern[hskey], isCompMatch[hskey], fuzzyLevel[hskey]) : null;
			HSCodeCluePattern = invoicePattern.ContainsKey(hssuggestkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[hssuggestkey], invoicePrefixPattern[hssuggestkey], invoicePostfixPattern[hssuggestkey], isCompMatch[hssuggestkey], fuzzyLevel[hssuggestkey]) : null;
			CurrencyPattern = invoicePattern.ContainsKey(curkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[curkey], invoicePrefixPattern[curkey], invoicePostfixPattern[curkey], isCompMatch[curkey], fuzzyLevel[curkey]) : null;
			DescriptionPattern = invoicePattern.ContainsKey(desckey) ? UtilityFunc.ParseRegexModoki(invoicePattern[desckey], invoicePrefixPattern[desckey], invoicePostfixPattern[desckey], isCompMatch[desckey], fuzzyLevel[desckey]) : null;
			Ext_ExistPattern = invoicePattern.ContainsKey(existkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[existkey], invoicePrefixPattern[existkey], invoicePostfixPattern[existkey], isCompMatch[existkey], fuzzyLevel[existkey]) : null;
			Ext_ExistSecondPattern = invoicePattern.ContainsKey(existsecondkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[existsecondkey], invoicePrefixPattern[existsecondkey], invoicePostfixPattern[existsecondkey], isCompMatch[existsecondkey], fuzzyLevel[existsecondkey]) : null;
			Ext_InvoiceNoPattern = invoicePattern.ContainsKey(invnokey) ? UtilityFunc.ParseRegexModoki(invoicePattern[invnokey], invoicePrefixPattern[invnokey], invoicePostfixPattern[invnokey], isCompMatch[invnokey], fuzzyLevel[invnokey]) : null;
			Ext_AmountPattern = invoicePattern.ContainsKey(amtkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[amtkey], invoicePrefixPattern[amtkey], invoicePostfixPattern[amtkey], isCompMatch[amtkey], fuzzyLevel[amtkey]) : null;
			Ext_NumberPattern = invoicePattern.ContainsKey(numberkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[numberkey], invoicePrefixPattern[numberkey], invoicePostfixPattern[numberkey], isCompMatch[numberkey], fuzzyLevel[numberkey]) : null;
			Ext_NetWeightPattern = invoicePattern.ContainsKey(netwtkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[netwtkey], invoicePrefixPattern[netwtkey], invoicePostfixPattern[netwtkey], isCompMatch[netwtkey], fuzzyLevel[netwtkey]) : null;
			Ext_OriginPattern = invoicePattern.ContainsKey(orkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[orkey], invoicePrefixPattern[orkey], invoicePostfixPattern[orkey], isCompMatch[orkey], fuzzyLevel[orkey]) : null;
			Ext_HSCodeKeyPattern = invoicePattern.ContainsKey(hskey) ? UtilityFunc.ParseRegexModoki(invoicePattern[hskey], invoicePrefixPattern[hskey], invoicePostfixPattern[hskey], isCompMatch[hskey], fuzzyLevel[hskey]) : null;
			Ext_HSCodeCluePattern = invoicePattern.ContainsKey(hssuggestkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[hssuggestkey], invoicePrefixPattern[hssuggestkey], invoicePostfixPattern[hssuggestkey], isCompMatch[hssuggestkey], fuzzyLevel[hssuggestkey]) : null;
			Ext_CurrencyPattern = invoicePattern.ContainsKey(curkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[curkey], invoicePrefixPattern[curkey], invoicePostfixPattern[curkey], isCompMatch[curkey], fuzzyLevel[curkey]) : null;
			Ext_DescriptionPattern = invoicePattern.ContainsKey(desckey) ? UtilityFunc.ParseRegexModoki(invoicePattern[desckey], invoicePrefixPattern[desckey], invoicePostfixPattern[desckey], isCompMatch[desckey], fuzzyLevel[desckey]) : null;
			IsExistBackward = isBackward.ContainsKey(existkey) ? isBackward[existkey]: false;
			IsExistSecondBackward = isBackward.ContainsKey(existsecondkey) ? isBackward[existsecondkey]: false;
			IsInvoiceNoBackward = isBackward.ContainsKey(invnokey) ? isBackward[invnokey]: false;
			IsAmountBackward = isBackward.ContainsKey(amtkey) ? isBackward[amtkey]: false;
			IsNumberBackward = isBackward.ContainsKey(numberkey) ? isBackward[numberkey]: false;
			IsNetWeightBackward = isBackward.ContainsKey(netwtkey) ? isBackward[netwtkey]: false;
			IsOriginBackward = isBackward.ContainsKey(orkey) ? isBackward[orkey] : false;
			IsHSCodeKeyBackward = isBackward.ContainsKey(hskey) ? isBackward[hskey] : false;
			IsHSCodeClueBackward = isBackward.ContainsKey(hssuggestkey) ? isBackward[hssuggestkey]  : false;
			IsCurrencyBackward = isBackward.ContainsKey(curkey) ? isBackward[curkey]  : false;
			IsDescriptionBackward = isBackward.ContainsKey(desckey) ? isBackward[desckey] : false;
		}
		else
        {
			ExistPattern = curSubtype.valid_patterns.exist != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.exist,null,null,true,0) : invoicePattern.ContainsKey(existkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[existkey], null, null, isCompMatch[existkey], fuzzyLevel[existkey]) : null;
			ExistSecondPattern = curSubtype.valid_patterns.exist_secondary != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.exist_secondary, null, null, true, 0) : invoicePattern.ContainsKey(existsecondkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[existsecondkey], null, null, isCompMatch[existsecondkey], fuzzyLevel[existsecondkey]) : null;
			InvoiceNoPattern = curSubtype.valid_patterns.invn != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.invn, null, null, true, 0) : invoicePattern.ContainsKey(invnokey) ? UtilityFunc.ParseRegexModoki(invoicePattern[invnokey], null, null, isCompMatch[invnokey], fuzzyLevel[invnokey]) : null;
			AmountPattern = curSubtype.valid_patterns.amount != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.amount, null, null, true, 0) : invoicePattern.ContainsKey(amtkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[amtkey], null, null, isCompMatch[amtkey], fuzzyLevel[amtkey]) : null;
			NumberPattern = curSubtype.valid_patterns.number != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.number, null, null, true, 0) : invoicePattern.ContainsKey(numberkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[numberkey], null, null, isCompMatch[numberkey], fuzzyLevel[numberkey]) : null; ;
			NetWeightPattern = curSubtype.valid_patterns.netweight != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.netweight, null, null, true, 0) : invoicePattern.ContainsKey(netwtkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[netwtkey], null, null, isCompMatch[netwtkey], fuzzyLevel[netwtkey]) : null; ;
			OriginPattern = curSubtype.valid_patterns.origin != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.origin, null, null, true, 0) : invoicePattern.ContainsKey(orkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[orkey], null, null, isCompMatch[orkey], fuzzyLevel[orkey]) : null; ;
			HSCodeKeyPattern = curSubtype.valid_patterns.hskey != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.hskey, null, null, true, 0) : invoicePattern.ContainsKey(hskey) ? UtilityFunc.ParseRegexModoki(invoicePattern[hskey], null, null, isCompMatch[hskey], fuzzyLevel[hskey]) : null; ;
			HSCodeCluePattern = curSubtype.valid_patterns.hsclue != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.hsclue, null, null, true, 0) : invoicePattern.ContainsKey(hssuggestkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[hssuggestkey], null, null, isCompMatch[hssuggestkey], fuzzyLevel[hssuggestkey]) : null;
			CurrencyPattern = curSubtype.valid_patterns.currency != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.currency, null, null, true, 0) : invoicePattern.ContainsKey(curkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[curkey], null, null, isCompMatch[curkey], fuzzyLevel[curkey]) : null;
			DescriptionPattern = curSubtype.valid_patterns.description != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.description, null, null, true, 0) : invoicePattern.ContainsKey(desckey) ? UtilityFunc.ParseRegexModoki(invoicePattern[desckey], null, null, isCompMatch[desckey], fuzzyLevel[desckey]) : null;
			Ext_ExistPattern = invoicePattern.ContainsKey(existkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[existkey], invoicePrefixPattern[existkey], invoicePostfixPattern[existkey], isCompMatch[existkey], fuzzyLevel[existkey]) :curSubtype.valid_patterns.exist != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.exist, null, null, true, 0) :  null;
			Ext_ExistSecondPattern =  invoicePattern.ContainsKey(existsecondkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[existsecondkey], invoicePrefixPattern[existsecondkey], invoicePostfixPattern[existsecondkey], isCompMatch[existsecondkey], fuzzyLevel[existsecondkey]):curSubtype.valid_patterns.exist_secondary != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.exist_secondary, null, null, true, 0) :  null;
			Ext_InvoiceNoPattern = invoicePattern.ContainsKey(invnokey) ? UtilityFunc.ParseRegexModoki(invoicePattern[invnokey], invoicePrefixPattern[invnokey], invoicePostfixPattern[invnokey], isCompMatch[invnokey], fuzzyLevel[invnokey]) :curSubtype.valid_patterns.invn != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.invn, null, null, true, 0) :  null;
			Ext_AmountPattern = invoicePattern.ContainsKey(amtkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[amtkey], invoicePrefixPattern[amtkey], invoicePostfixPattern[amtkey], isCompMatch[amtkey], fuzzyLevel[amtkey]):curSubtype.valid_patterns.amount != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.amount, null, null, true, 0) : null;
			Ext_NumberPattern = invoicePattern.ContainsKey(numberkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[numberkey], invoicePrefixPattern[numberkey], invoicePostfixPattern[numberkey], isCompMatch[numberkey], fuzzyLevel[numberkey]) : curSubtype.valid_patterns.number != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.number, null, null, true, 0) :null; 
			Ext_NetWeightPattern = invoicePattern.ContainsKey(netwtkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[netwtkey], invoicePrefixPattern[netwtkey], invoicePostfixPattern[netwtkey], isCompMatch[netwtkey], fuzzyLevel[netwtkey]) : curSubtype.valid_patterns.netweight != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.netweight, null, null, true, 0) :  null; 
			Ext_OriginPattern = invoicePattern.ContainsKey(orkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[orkey], invoicePrefixPattern[orkey], invoicePostfixPattern[orkey], isCompMatch[orkey], fuzzyLevel[orkey]) : curSubtype.valid_patterns.origin != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.origin, null, null, true, 0) : null; 
			Ext_HSCodeKeyPattern = invoicePattern.ContainsKey(hskey) ? UtilityFunc.ParseRegexModoki(invoicePattern[hskey], invoicePrefixPattern[hskey], invoicePostfixPattern[hskey], isCompMatch[hskey], fuzzyLevel[hskey]) : curSubtype.valid_patterns.hskey != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.hskey, null, null, true, 0) : null; 
			Ext_HSCodeCluePattern = invoicePattern.ContainsKey(hssuggestkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[hssuggestkey], invoicePrefixPattern[hssuggestkey], invoicePostfixPattern[hssuggestkey], isCompMatch[hssuggestkey], fuzzyLevel[hssuggestkey]) : curSubtype.valid_patterns.hsclue != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.hsclue, null, null, true, 0) : null;
			Ext_CurrencyPattern = invoicePattern.ContainsKey(curkey) ? UtilityFunc.ParseRegexModoki(invoicePattern[curkey], invoicePrefixPattern[curkey], invoicePostfixPattern[curkey], isCompMatch[curkey], fuzzyLevel[curkey]) : curSubtype.valid_patterns.currency != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.currency, null, null, true, 0) :null;
			Ext_DescriptionPattern = invoicePattern.ContainsKey(desckey) ? UtilityFunc.ParseRegexModoki(invoicePattern[desckey], invoicePrefixPattern[desckey], invoicePostfixPattern[desckey], isCompMatch[desckey], fuzzyLevel[desckey]) : curSubtype.valid_patterns.description != null ? UtilityFunc.ParseRegexModoki(curSubtype.valid_patterns.description, null, null, true, 0) :  null;
			IsExistBackward = isBackward.ContainsKey(existkey) ? isBackward[existkey] : false;
			IsExistSecondBackward = isBackward.ContainsKey(existsecondkey) ? isBackward[existsecondkey] : false;
			IsInvoiceNoBackward = isBackward.ContainsKey(invnokey) ? isBackward[invnokey] : false;
			IsAmountBackward = isBackward.ContainsKey(amtkey) ? isBackward[amtkey] : false;
			IsNumberBackward = isBackward.ContainsKey(numberkey) ? isBackward[numberkey] : false;
			IsNetWeightBackward = isBackward.ContainsKey(netwtkey) ? isBackward[netwtkey] : false;
			IsOriginBackward = isBackward.ContainsKey(orkey) ? isBackward[orkey] : false;
			IsHSCodeKeyBackward = isBackward.ContainsKey(hskey) ? isBackward[hskey] : false;
			IsHSCodeClueBackward = isBackward.ContainsKey(hssuggestkey) ? isBackward[hssuggestkey] : false;
			IsCurrencyBackward = isBackward.ContainsKey(curkey) ? isBackward[curkey] : false;
			IsDescriptionBackward = isBackward.ContainsKey(desckey) ? isBackward[desckey] : false;
		}
	}
}
