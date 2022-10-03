using System;
using System.Linq;
using System.Collections.Generic;
using NewIVEdit;


public class DataRegister
{
	public DataRegister()
	{
	}
	public SortedList<string, object> DataRegisterColumn = new SortedList<string, object>();
	public int Length = 0;
	public void SetUp()
	{
		DataRegisterColumn.Clear();
		if (App.MainView.CurrentCompanyProfile == null || App.MainView.CurrentCompanyProfile.data_register == null || App.MainView.CurrentCompanyProfile.data_register.Length == 0) return;
		Length = App.MainView.InvoiceData.Count > 0 ? App.MainView.InvoiceData.Count * 2 : 1;
		foreach (var regProf in App.MainView.CurrentCompanyProfile.data_register)
		{
			if (regProf.label == null || regProf.type == null) continue;
			if (!DataRegisterColumn.ContainsKey(regProf.label))
			{
                switch (regProf.type)
                {
					case "DOUBLE":
						DataRegisterColumn.Add(regProf.label, new List<double>(Enumerable.Repeat(0.0, Length)));
						break;
					case "INT":
						DataRegisterColumn.Add(regProf.label, new List<int>(Enumerable.Repeat(0, Length)));
						break;
					case "STRING":
						DataRegisterColumn.Add(regProf.label, new List<string>(Enumerable.Repeat("", Length)));
						break;

				}
			}
		}
	}
}