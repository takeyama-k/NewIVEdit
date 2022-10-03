using System;
using System.Collections.Generic;
using NewIVEdit;


public class InvoiceBucketData
{
	public InvoiceBucketData()
	{
	}
	public SortedList<string, InvoiceBucketDataColumn> BucketDataColumn = new SortedList<string, InvoiceBucketDataColumn>();

	public void SetUp()
	{
		if (App.MainView.CurrentCompanySubtype == null || App.MainView.CurrentCompanySubtype.data_bucket == null || App.MainView.CurrentCompanySubtype.data_bucket.Length == 0) return;
		BucketDataColumn.Clear();
		foreach (var bucketProf in App.MainView.CurrentCompanySubtype.data_bucket)
		{
			if (bucketProf.label == null || bucketProf.type == null || bucketProf.target == null) continue;
			if (!BucketDataColumn.ContainsKey(bucketProf.label))
			{
				var col = new InvoiceBucketDataColumn();
				col.PartitionedBy = bucketProf.partitioned_by;
				col.AddDelegate = (InvoiceDataElement oldItem, InvoiceDataElement newItem) =>
				{
					string oldkey = "";
					string newkey = "";
					bool oldflag = false;
					bool newflag = false;
					if (newItem == null)
					{
						switch (bucketProf.partitioned_by)
						{
							case "INVOICENO":
								oldkey = oldItem.InvoiceNo;
								oldflag = (oldItem.DataValid & InvoiceDataElement.DatabaseValidFlag.InvoiceNo) == InvoiceDataElement.DatabaseValidFlag.InvoiceNo ? true : false;
								break;
							case "ORIGIN":
								oldkey = oldItem.Origin;
								oldflag = (oldItem.DeclValid & InvoiceDataElement.DeclearValidFlag.Origin) == InvoiceDataElement.DeclearValidFlag.Origin ? true : false;
								break;
							case "HSCODEKEY":
								oldkey = oldItem.HSCodeKey;
								oldflag = (oldItem.DataValid & InvoiceDataElement.DatabaseValidFlag.HSCodeKey) == InvoiceDataElement.DatabaseValidFlag.HSCodeKey ? true : false;
								break;
							case "HSCODECLUE":
								oldkey = oldItem.HSCodeClue;
								oldflag = (oldItem.DataValid & InvoiceDataElement.DatabaseValidFlag.HSCodeClue) == InvoiceDataElement.DatabaseValidFlag.HSCodeClue ? true : false;
								break;
							case "CURRENCY":
								oldkey = oldItem.Currency;
								oldflag = (oldItem.DeclValid & InvoiceDataElement.DeclearValidFlag.Currency) == InvoiceDataElement.DeclearValidFlag.Currency ? true : false;
								break;
							case "DESCRIPTION":
								oldkey = oldItem.Description;
								oldflag = (oldItem.DataValid & InvoiceDataElement.DatabaseValidFlag.Description) == InvoiceDataElement.DatabaseValidFlag.Description ? true : false;
								break;
							case "HSCODE":
								oldkey = oldItem.HSCode;
								oldflag = (oldItem.DeclValid & InvoiceDataElement.DeclearValidFlag.HSCode) == InvoiceDataElement.DeclearValidFlag.HSCode ? true : false;
								break;
							default:
								break;
						}
						long _deltaInternal = 0;
						switch (bucketProf.target)
						{
							case "AMOUNT":
								if ((oldItem.DeclValid & InvoiceDataElement.DeclearValidFlag.Amount) != InvoiceDataElement.DeclearValidFlag.Amount) return;
								if (oldflag) _deltaInternal -= oldItem.AmountInternal;
								else return;
								break;
							case "NETWEIGHT":
								if ((oldItem.DeclValid & InvoiceDataElement.DeclearValidFlag.NetWeight) != InvoiceDataElement.DeclearValidFlag.NetWeight) return;
								if (oldflag) _deltaInternal -= oldItem.NetWeightInternal;
								else return;
								break;
							case "NUMBER":
								if ((oldItem.DeclValid & InvoiceDataElement.DeclearValidFlag.Number) != InvoiceDataElement.DeclearValidFlag.Number) return;
								if (oldflag) _deltaInternal -= newItem.NumberInternal;
								else return;
								break;
							default:
								break;
						}

						if (col.BucketData.ContainsKey(oldkey))
						{
							col.BucketData[oldkey].ValueInternal += _deltaInternal;
						}

						return;
					}


					switch (bucketProf.partitioned_by)
					{
						case "INVOICENO":
							oldkey = oldItem.InvoiceNo;
							newkey = newItem.InvoiceNo;
							oldflag = (oldItem.DataValid & InvoiceDataElement.DatabaseValidFlag.InvoiceNo) == InvoiceDataElement.DatabaseValidFlag.InvoiceNo ? true : false;
							newflag = (newItem.DataValid & InvoiceDataElement.DatabaseValidFlag.InvoiceNo) == InvoiceDataElement.DatabaseValidFlag.InvoiceNo ? true : false;
							break;
						case "ORIGIN":
							oldkey = oldItem.Origin;
							newkey = newItem.Origin;
							oldflag = (oldItem.DeclValid & InvoiceDataElement.DeclearValidFlag.Origin) == InvoiceDataElement.DeclearValidFlag.Origin ? true : false;
							newflag = (newItem.DeclValid & InvoiceDataElement.DeclearValidFlag.Origin) == InvoiceDataElement.DeclearValidFlag.Origin ? true : false;
							break;
						case "HSCODEKEY":
							oldkey = oldItem.HSCodeKey;
							newkey = newItem.HSCodeKey;
							oldflag = (oldItem.DataValid & InvoiceDataElement.DatabaseValidFlag.HSCodeKey) == InvoiceDataElement.DatabaseValidFlag.HSCodeKey ? true : false;
							newflag = (newItem.DataValid & InvoiceDataElement.DatabaseValidFlag.HSCodeKey) == InvoiceDataElement.DatabaseValidFlag.HSCodeKey ? true : false;
							break;
						case "HSCODECLUE":
							oldkey = oldItem.HSCodeClue;
							newkey = newItem.HSCodeClue;
							oldflag = (oldItem.DataValid & InvoiceDataElement.DatabaseValidFlag.HSCodeClue) == InvoiceDataElement.DatabaseValidFlag.HSCodeClue ? true : false;
							newflag = (newItem.DataValid & InvoiceDataElement.DatabaseValidFlag.HSCodeClue) == InvoiceDataElement.DatabaseValidFlag.HSCodeClue ? true : false;
							break;
						case "CURRENCY":
							oldkey = oldItem.Currency;
							newkey = newItem.Currency;
							oldflag = (oldItem.DeclValid & InvoiceDataElement.DeclearValidFlag.Currency) == InvoiceDataElement.DeclearValidFlag.Currency ? true : false;
							newflag = (newItem.DeclValid & InvoiceDataElement.DeclearValidFlag.Currency) == InvoiceDataElement.DeclearValidFlag.Currency ? true : false;
							break;
						case "DESCRIPTION":
							oldkey = oldItem.Description;
							newkey = newItem.Description;
							oldflag = (oldItem.DataValid & InvoiceDataElement.DatabaseValidFlag.Description) == InvoiceDataElement.DatabaseValidFlag.Description ? true : false;
							newflag = (newItem.DataValid & InvoiceDataElement.DatabaseValidFlag.Description) == InvoiceDataElement.DatabaseValidFlag.Description ? true : false;
							break;
						case "HSCODE":
							oldkey = oldItem.HSCode;
							newkey = newItem.HSCode;
							oldflag = (oldItem.DeclValid & InvoiceDataElement.DeclearValidFlag.HSCode) == InvoiceDataElement.DeclearValidFlag.HSCode ? true : false;
							newflag = (newItem.DeclValid & InvoiceDataElement.DeclearValidFlag.HSCode) == InvoiceDataElement.DeclearValidFlag.HSCode ? true : false;
							break;
						default:
							break;
					}
					long deltaInternal = 0;
					switch (bucketProf.target)
					{
						case "AMOUNT":
							if ((newItem.DeclValid & InvoiceDataElement.DeclearValidFlag.Amount) != InvoiceDataElement.DeclearValidFlag.Amount) return;
							if (!oldflag && newflag) deltaInternal = newItem.AmountInternal;
							else if (oldflag && !newflag) deltaInternal -= newItem.AmountInternal;
							else if (oldflag && newflag)
							{
								if (oldkey == newkey)
								{
									deltaInternal = newItem.AmountInternal - oldItem.AmountInternal;
								}
								else
								{
									deltaInternal = oldItem.AmountInternal;
								}
							}

							else return;
							break;
						case "NETWEIGHT":
							if ((newItem.DeclValid & InvoiceDataElement.DeclearValidFlag.NetWeight) != InvoiceDataElement.DeclearValidFlag.NetWeight) return;
							if (!oldflag && newflag) deltaInternal = newItem.NetWeightInternal;
							else if (oldflag && !newflag) deltaInternal -= newItem.NetWeightInternal;
							else if (oldflag && newflag)
							{
								if (oldkey == newkey)
								{
									deltaInternal = newItem.NetWeightInternal - oldItem.NetWeightInternal;
								}
								else
								{
									deltaInternal = oldItem.NetWeightInternal;
								}
							}
							else return;
							break;
						case "NUMBER":
							if ((newItem.DeclValid & InvoiceDataElement.DeclearValidFlag.Number) != InvoiceDataElement.DeclearValidFlag.Number) return;
							if (!oldflag && newflag) deltaInternal = newItem.NumberInternal;
							else if (oldflag && !newflag) deltaInternal -= newItem.NumberInternal;
							else if (oldflag && newflag)
							{
								if (oldkey == newkey)
								{
									deltaInternal = newItem.NumberInternal - oldItem.NumberInternal;
								}
								else
								{
									deltaInternal = oldItem.NumberInternal;
								}
							}
							else return;
							break;
						default:
							break;
					}

					if (!oldflag && newflag)
					{
						if (!col.BucketData.ContainsKey(newkey)) return;
						col.BucketData[newkey].ValueInternal += deltaInternal;
					}
					else if (oldflag && !newflag)
					{
						if (!col.BucketData.ContainsKey(oldkey)) return;
						col.BucketData[oldkey].ValueInternal += deltaInternal;
					}
					else if (oldflag && newflag)
					{
						if (oldkey != newkey)
						{
							if (col.BucketData.ContainsKey(oldkey))
							{
								col.BucketData[oldkey].ValueInternal -= deltaInternal;
							}
							if (col.BucketData.ContainsKey(newkey))
							{
								col.BucketData[newkey].ValueInternal += deltaInternal;
							}
						}
						else
						{
							if (col.BucketData.ContainsKey(newkey))
							{
								col.BucketData[newkey].ValueInternal += deltaInternal;
							}
						}
					}
					else return;

				};
				BucketDataColumn.Add(bucketProf.label, col);
			}
			else
			{
				return;
			}
		}
		foreach (var col in BucketDataColumn.Values)
		{
			col.CountDelegate = (InvoiceDataElement oldItem, InvoiceDataElement newItem) =>
			{

				string oldkey = "";
				string newkey = "";
				bool oldflag = false;
				bool newflag = false;
				if (newItem == null)
				{
					switch (col.PartitionedBy)
					{
						case "INVOICENO":
							oldkey = oldItem.InvoiceNo;
							oldflag = (oldItem.DataValid & InvoiceDataElement.DatabaseValidFlag.InvoiceNo) == InvoiceDataElement.DatabaseValidFlag.InvoiceNo ? true : false;
							break;
						case "ORIGIN":
							oldkey = oldItem.Origin;
							oldflag = (oldItem.DeclValid & InvoiceDataElement.DeclearValidFlag.Origin) == InvoiceDataElement.DeclearValidFlag.Origin ? true : false;
							break;
						case "HSCODEKEY":
							oldkey = oldItem.HSCodeKey;
							oldflag = (oldItem.DataValid & InvoiceDataElement.DatabaseValidFlag.HSCodeKey) == InvoiceDataElement.DatabaseValidFlag.HSCodeKey ? true : false;
							break;
						case "HSCODECLUE":
							oldkey = oldItem.HSCodeClue;
							oldflag = (oldItem.DataValid & InvoiceDataElement.DatabaseValidFlag.HSCodeClue) == InvoiceDataElement.DatabaseValidFlag.HSCodeClue ? true : false;
							break;
						case "CURRENCY":
							oldkey = oldItem.Currency;
							oldflag = (oldItem.DeclValid & InvoiceDataElement.DeclearValidFlag.Currency) == InvoiceDataElement.DeclearValidFlag.Currency ? true : false;
							break;
						case "DESCRIPTION":
							oldkey = oldItem.Description;
							oldflag = (oldItem.DataValid & InvoiceDataElement.DatabaseValidFlag.Description) == InvoiceDataElement.DatabaseValidFlag.Description ? true : false;
							break;
						case "HSCODE":
							oldkey = oldItem.HSCode;
							oldflag = (oldItem.DeclValid & InvoiceDataElement.DeclearValidFlag.HSCode) == InvoiceDataElement.DeclearValidFlag.HSCode ? true : false;
							break;
						default:
							break;
					}
					if (oldflag)
					{
						if (col.BucketData.ContainsKey(oldkey))
						{
							col.BucketData[oldkey].Count--;
							if (col.BucketData[oldkey].Count == 0)
							{
								col.BucketData.Remove(oldkey);
							}
						}

					}

					return;
				}
				switch (col.PartitionedBy)
				{
					case "INVOICENO":
						oldkey = oldItem.InvoiceNo;
						newkey = newItem.InvoiceNo;
						oldflag = (oldItem.DataValid & InvoiceDataElement.DatabaseValidFlag.InvoiceNo) == InvoiceDataElement.DatabaseValidFlag.InvoiceNo ? true : false;
						newflag = (newItem.DataValid & InvoiceDataElement.DatabaseValidFlag.InvoiceNo) == InvoiceDataElement.DatabaseValidFlag.InvoiceNo ? true : false;
						break;
					case "ORIGIN":
						oldkey = oldItem.Origin;
						newkey = newItem.Origin;
						oldflag = (oldItem.DeclValid & InvoiceDataElement.DeclearValidFlag.Origin) == InvoiceDataElement.DeclearValidFlag.Origin ? true : false;
						newflag = (newItem.DeclValid & InvoiceDataElement.DeclearValidFlag.Origin) == InvoiceDataElement.DeclearValidFlag.Origin ? true : false;
						break;
					case "HSCODEKEY":
						oldkey = oldItem.HSCodeKey;
						newkey = newItem.HSCodeKey;
						oldflag = (oldItem.DataValid & InvoiceDataElement.DatabaseValidFlag.HSCodeKey) == InvoiceDataElement.DatabaseValidFlag.HSCodeKey ? true : false;
						newflag = (newItem.DataValid & InvoiceDataElement.DatabaseValidFlag.HSCodeKey) == InvoiceDataElement.DatabaseValidFlag.HSCodeKey ? true : false;
						break;
					case "HSCODECLUE":
						oldkey = oldItem.HSCodeClue;
						newkey = newItem.HSCodeClue;
						oldflag = (oldItem.DataValid & InvoiceDataElement.DatabaseValidFlag.HSCodeClue) == InvoiceDataElement.DatabaseValidFlag.HSCodeClue ? true : false;
						newflag = (newItem.DataValid & InvoiceDataElement.DatabaseValidFlag.HSCodeClue) == InvoiceDataElement.DatabaseValidFlag.HSCodeClue ? true : false;
						break;
					case "CURRENCY":
						oldkey = oldItem.Currency;
						newkey = newItem.Currency;
						oldflag = (oldItem.DeclValid & InvoiceDataElement.DeclearValidFlag.Currency) == InvoiceDataElement.DeclearValidFlag.Currency ? true : false;
						newflag = (newItem.DeclValid & InvoiceDataElement.DeclearValidFlag.Currency) == InvoiceDataElement.DeclearValidFlag.Currency ? true : false;
						break;
					case "DESCRIPTION":
						oldkey = oldItem.Description;
						newkey = newItem.Description;
						oldflag = (oldItem.DataValid & InvoiceDataElement.DatabaseValidFlag.Description) == InvoiceDataElement.DatabaseValidFlag.Description ? true : false;
						newflag = (newItem.DataValid & InvoiceDataElement.DatabaseValidFlag.Description) == InvoiceDataElement.DatabaseValidFlag.Description ? true : false;
						break;
					case "HSCODE":
						oldkey = oldItem.HSCode;
						newkey = newItem.HSCode;
						oldflag = (oldItem.DeclValid & InvoiceDataElement.DeclearValidFlag.HSCode) == InvoiceDataElement.DeclearValidFlag.HSCode ? true : false;
						newflag = (newItem.DeclValid & InvoiceDataElement.DeclearValidFlag.HSCode) == InvoiceDataElement.DeclearValidFlag.HSCode ? true : false;
						break;
					default:
						break;
				}
				if (!oldflag && newflag)
				{
					if (col.BucketData.ContainsKey(newkey))
					{
						col.BucketData[newkey].Count++;
					}
					else
					{
						col.BucketData.Add(newkey, new InvoiceBucketDataElement() { Count = 1 });
					}
				}
				else if (oldflag && !newflag)
				{
					if (col.BucketData.ContainsKey(oldkey))
					{
						col.BucketData[oldkey].Count--;
						if (col.BucketData[oldkey].Count == 0) col.BucketData.Remove(oldkey);
					}
				}
				else if (oldflag && newflag)
				{
					if (oldkey != newkey)
					{
						if (col.BucketData.ContainsKey(oldkey))
						{
							col.BucketData[oldkey].Count--;
							if (col.BucketData[oldkey].Count == 0) col.BucketData.Remove(oldkey);
						}
						if (col.BucketData.ContainsKey(newkey))
						{
							col.BucketData[newkey].Count++;
						}
						else
						{
							col.BucketData.Add(newkey, new InvoiceBucketDataElement() { Count = 1 });
						}
					}
				}
				else
				{
					return;
				}

			};
		}
		foreach (var col in BucketDataColumn.Values)
		{
			InvoiceDataElement.AddDelegate += col.AddDelegate;
			InvoiceDataElement.CountDelegate += col.CountDelegate;
		}
	}
}

public class InvoiceBucketDataColumn
{
	public string PartitionedBy = "";
	public SortedList<string, InvoiceBucketDataElement> BucketData = new SortedList<string, InvoiceBucketDataElement>();
	public InvoiceDataElement.AddValueDelegate AddDelegate;
	public InvoiceDataElement.CountElementDelegate CountDelegate;

}
