using System;
using System.Collections.Generic;
using NewIVEdit;
public class DeclAccum
{
	public Dictionary<string, DeclAccumElem> DeclAccumDictionary { set; get; } = new Dictionary<string, DeclAccumElem>();
	public Dictionary<string, int> DeclElementCounter { set; get; } = new Dictionary<string, int>();

	public DeclAccum()
	{
	}
	public void Inclement(string hash)
    {
        if (DeclElementCounter.ContainsKey(hash))
        {
			DeclElementCounter[hash]++;
        }
        else
        {
			DeclElementCounter.Add(hash, 1);
        }
    }
	public void Inclement(DeclAccumElem item)
    {
		string hash = item.Hash();
		Inclement(hash);
    }
	public void Declement(string hash)
	{
		if (DeclElementCounter.ContainsKey(hash))
		{
			DeclElementCounter[hash]--;
		}
		else
		{
			return;
		}
	}
	public void Declement(DeclAccumElem item)
	{
		string hash = item.Hash();
		Declement(hash);
	}
	public void Add(DeclAccumElem delta)
    {
		string hash = delta.Hash();
		if (!DeclAccumDictionary.ContainsKey(hash))
		{
			DeclAccumDictionary.Add(hash, delta);
		}
		else
		{
			var target = DeclAccumDictionary[hash];
			target.AmountInternal += delta.AmountInternal;
			target.NetWeightInternal += delta.NetWeightInternal;
			target.NumberInternal += delta.NumberInternal;
		}

        if (DeclElementCounter.ContainsKey(hash) && DeclElementCounter[hash] == 0 && DeclAccumDictionary.ContainsKey(hash))
        {
			if(DeclAccumDictionary[hash].AmountInternal == 0 && DeclAccumDictionary[hash].NetWeightInternal == 0 && DeclAccumDictionary[hash].NumberInternal == 0)
            {
				DeclAccumDictionary.Remove(hash);
				DeclElementCounter.Remove(hash);
            }
        }
		SumUP(App.MainView.IsPrimeCurrencyForced);
	}

	public void SumUP(bool primeCurrencyFixed = false)
    {
		
		long amount = 0L;
		long netweight = 0L;
		long quantity = 0L;
		foreach(var accum in App.MainView.DeclearationAccum.DeclAccumDictionary)
        {
			App.MainView.UpdateCurrencyProfile(accum.Value.Currency);
        }
        if (!primeCurrencyFixed)
        {
			var prime = App.MainView.DecidePrimeCurrency();
			if (prime != null)
            {
				foreach (var cur in App.MainView.CurrencyProfiles)
				{
					if (cur.Code == prime) cur.SetPrime(true);
					else cur.SetPrime(false);
				}
			}
			App.MainView.IsPrimeCurrencyForced = false;
        }
		CurrencyProfile primeCurrency =App.MainView.FindPrimeCurrency();
		
		foreach (var accum in DeclAccumDictionary)
        {
			if (primeCurrency != null && primeCurrency.Code == accum.Value.Currency)
			{
				amount += accum.Value.AmountInternal;
			}
			netweight += accum.Value.NetWeightInternal;
			quantity += accum.Value.NumberInternal;
        }
		double realAmount = 0.0D;
		double realNetWeight = 0.0D;
		double realQuantity = 0.0D;
		realAmount = (double)amount / (double)NewIVEdit.ConstValue.InternalValueFactor;
		realNetWeight = (double)netweight / (double)NewIVEdit.ConstValue.InternalValueFactor;
		realQuantity = (double)quantity / (double)NewIVEdit.ConstValue.InternalValueFactor;
		if (App.MainView.SubWindowController.DataWindow != null)
		{
			App.MainView.SubWindowController.DataWindow.View.FaceValue = string.Format("{0:0.00}",realAmount);
			App.MainView.SubWindowController.DataWindow.View.FOBValue = string.Format("{0:0.00}", realAmount);
			App.MainView.SubWindowController.DataWindow.View.TotalNetWeight = string.Format("{0:0.000}", realNetWeight);
			App.MainView.SubWindowController.DataWindow.View.TotalQuantity = string.Format("{0:0}", realQuantity);
		}
	}

	public void Clear()
    {
		DeclAccumDictionary.Clear();
		DeclElementCounter.Clear();
    }

}

public class DeclAccumElem
{
	public string InvoiceNo { set; get; }
	public string HSCode { set; get; }
	public string Postfix { set; get; }
	public string Currency { set; get; }
	public long AmountInternal{ set; get; }
	public long NetWeightInternal { set; get; }
	public long NumberInternal { set; get; }
	public long AmountInternalJPY = 0L;
	public int DeclIndex { set; get; } = -1;
	public string Origin { set; get; }
	public string HSCodeKey { set; get; }
	public string HSCodeClue { set; get; }
	public string Description { set; get; }
	public string Hash()
    {
		return HSCode + "|" + Postfix + "|" + Currency;
    }

	public string HashWOCurrency()
    {
		return HSCode + "|" + Postfix;
	}

}
