using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;
using NewIVEdit;

public class SideChannelDataViewModel
{
	public SideChannelDataViewModel()
	{
	}
	public delegate void SideChannelConverterDelegate(InvoiceDataElement ivElem);
	public static List<SideChannelConverterDelegate> SideChannelConverters = new List<SideChannelConverterDelegate>();
	public static SortedList<string, string[]> SideChannelDataProfs = new SortedList<string, string[]>();
	public static void Init()
	{
		App.MainView.SideChannelData.Clear();
		SideChannelDataProfs.Clear();
		SideChannelConverters.Clear();
		var sideChanProfs = getSideChannel();
		if (sideChanProfs == null) return;
		foreach (var sideChanProf in sideChanProfs)
		{
			var fields = sideChanProf.field;
			string[] colNames = new string[fields.Length];
			SideChannelDataProfs.Add(sideChanProf.label, colNames);
			int cntr = 0;
			foreach (var field in fields)
			{
				SideChannelDataProfs[sideChanProf.label][cntr++] = field.label;
			}
			var convs = sideChanProf.converter;
			foreach (var conv in convs)
			{
				string key_field = sideChanProf.key_field;
				if (!UtilityFunc.ExistsField(key_field)) continue;
				SideChannelConverters.Add(new SideChannelConverterDelegate((ivElem) =>
				{
					string key = null;
					switch (key_field)
					{
						case "HSCODE":
							key = ivElem.HSCode;
							break;
						case "CURRENCY":
							key = ivElem.Currency;
							break;
						case "ORIGIN":
							key = ivElem.Origin;
							break;
						case "AMOUNT":
							key = ivElem.Amount;
							break;
						case "NUMBER":
							key = ivElem.Number;
							break;
						case "NETWEIGHT":
							key = ivElem.NetWeight;
							break;
						case "EXIST":
							key = ivElem.Exist;
							break;
						case "INVOICENO":
							key = ivElem.InvoiceNo;
							break;
						case "HSCODEKEY":
							key = ivElem.HSCodeKey;
							break;
						case "HSCODECLUE":
							key = ivElem.HSCodeClue;
							break;
						case "HSCODESUGGESTION":
							key = ivElem.HSCodeSuggestion;
							break;
						case "DESCRIPTION":
							key = ivElem.Description;
							break;
						default:
							break;
					}
					if (key == null) return;
					bool condValidFlag = true;
					if (!(conv.condition_type == null || conv.condition_sidechannel_field == null || conv.condition_right_operand_field == null || !Enum.IsDefined(typeof(ConstValue.ConverterConditionEnum), conv.condition_type)))
					{
						string cond_table_name = conv.condition_sidechannel_name;
						string cond_field = conv.condition_sidechannel_field;
						string cond_type = conv.condition_type;
						string cond_right = conv.condition_right_operand_field;
						if (App.MainView.SideChannelData.ContainsKey(cond_field) && Enum.IsDefined(typeof(ConstValue.ConverterActionEnum), cond_type) && UtilityFunc.ExistsField(cond_right))
						{

							string cond_left_value = null;
							string cond_right_value = null;
							if (key != null)
							{
								cond_left_value = getSideChannelData(cond_table_name,cond_field,key);
								cond_right_value = getFieldData(cond_right, key, ivElem);
								int c = int.MinValue;
								int sc = int.MinValue;
								if (cond_right_value == null || cond_left_value == null) condValidFlag = false;
								else
								{
									c = UtilityFunc.ComapreValue(cond_left_value, cond_right_value);
									sc = String.Compare(cond_right_value, cond_left_value);
									switch (cond_type)
									{
										case "EQUAL":
											if (c == 0) condValidFlag = true;
											else condValidFlag = false;
											break;
										case "NOT_EQUAL":
											if (c != 0) condValidFlag = true;
											else condValidFlag = false;
											break;
										case "LESS_THAN":
											if (c == 1) condValidFlag = true;
											else condValidFlag = false;
											break;
										case "GREATER_THAN":
											if (c == -1) condValidFlag = true;
											else condValidFlag = false;
											break;
										case "EQUAL_LESS_THAN":
											if (c == 0 || c == 1) condValidFlag = true;
											else condValidFlag = false;
											break;
										case "EQUAL_GREATER_THAN":
											if (c == 0 || c == -1) condValidFlag = true;
											else condValidFlag = false;
											break;
										case "SAME":
											if (sc == 0) condValidFlag = true;
											else condValidFlag = false;
											break;
										case "MATCH":
											if (sc == 0 || Regex.Match(cond_right_value, cond_left_value).Success) condValidFlag = true;
											else condValidFlag = false;
											break;
										default:
											break;
									}
								}
							}
						}
					}
					if (!condValidFlag) return;
					else
					{
						if (conv.condition_sidechannel_name == null || conv.action_right_operand_field == null || conv.action_left_operand_field == null || 
						conv.action_operator == null || !Enum.IsDefined(typeof(ConstValue.ConverterActionEnum), conv.action_operator) ||
						conv.result_field == null || (!Enum.IsDefined(typeof(ConstValue.FieldNameEnum), conv.result_field) && !App.MainView.DataRegister.DataRegisterColumn.ContainsKey(conv.result_field))) return;
						string tableName = conv.condition_sidechannel_name;
						string rightField = conv.action_right_operand_field;
						string leftField = conv.action_left_operand_field;
						string rightValue =  getRegisterData(rightField,ivElem) ??getSideChannelData(tableName,rightField,key) ?? getFieldData(rightField,key,ivElem);
						if (rightValue == null) return;
						string leftValue = getRegisterData(leftField, ivElem) ?? getSideChannelData(tableName, leftField,key) ?? getFieldData(leftField, key, ivElem);
						if (leftValue == null) return;
						double tempRight = 0.0;
						double tempLeft = 0.0;
						double resValue = 0.0;
						if (!(double.TryParse(rightValue, out tempRight) && double.TryParse(leftValue, out tempLeft))) return;
                        switch (conv.action_operator) 
						{
							case "ADD":
								resValue = tempLeft + tempRight;
								break;
							case "SUBTRACT":
								resValue = tempLeft - tempRight;
								break;
							case "MULTIPLY_DOUBLE":
								resValue = tempLeft * tempRight;
								break;
							case "MULTIPLY_INT":
								resValue = tempLeft * tempRight;
								break;
							case "DIVIDE_DOUBLE":
								resValue = tempRight != 0.0 ? tempLeft / tempRight : 0.0;
								break;
							case "DIVIDE_INT":
								resValue = tempRight != 0.0 ? tempLeft / tempRight : 0.0;
								break;
							default:
								break;
						}
						if (App.MainView.DataRegister.DataRegisterColumn.ContainsKey(conv.result_field))
						{
							object reg = App.MainView.DataRegister.DataRegisterColumn[conv.result_field];
							if(reg.GetType() == typeof(List<int>))
                            {
								if ((reg as List<int>).Count < App.MainView.InvoiceData.Count * 2)
								{
									App.MainView.DataRegister.Length = App.MainView.InvoiceData.Count * 2;
									var newreg = new List<int>(Enumerable.Repeat(0, App.MainView.InvoiceData.Count * 2));
									int regcntr = 0;
									foreach(var oldval in (reg as List<int>))
                                    {
										newreg[regcntr++] = oldval;
                                    }
									App.MainView.DataRegister.DataRegisterColumn[conv.result_field] = newreg;
								}
								(App.MainView.DataRegister.DataRegisterColumn[conv.result_field] as List<int>)[ivElem.IndexNo] = (int)resValue;
                            }
							else if (reg.GetType() == typeof(List<double>))
							{
								if ((reg as List<double>).Count < App.MainView.InvoiceData.Count * 2)
								{
									App.MainView.DataRegister.Length = App.MainView.InvoiceData.Count * 2;
									var newreg = new List<double>(Enumerable.Repeat(0.0, App.MainView.InvoiceData.Count * 2));
									int regcntr = 0;
									foreach (var oldval in (reg as List<double>))
									{
										newreg[regcntr++] = oldval;
									}
									App.MainView.DataRegister.DataRegisterColumn[conv.result_field] = newreg;
								}
								(App.MainView.DataRegister.DataRegisterColumn[conv.result_field] as List<double>)[ivElem.IndexNo] = (double)resValue;
							}
							else if (reg.GetType() == typeof(List<string>))
							{
								if ((reg as List<string>).Count < App.MainView.InvoiceData.Count * 2)
								{
									App.MainView.DataRegister.Length = App.MainView.InvoiceData.Count * 2;
									var newreg = new List<string>(Enumerable.Repeat("", App.MainView.InvoiceData.Count * 2));
									int regcntr = 0;
									foreach (var oldval in (reg as List<string>))
									{
										newreg[regcntr++] = oldval;
									}
									App.MainView.DataRegister.DataRegisterColumn[conv.result_field] = newreg;
								}
							   (App.MainView.DataRegister.DataRegisterColumn[conv.result_field] as List<string>)[ivElem.IndexNo] = resValue.ToString();
							}
						}
						else
						{
							switch (conv.result_field)
							{
								case "HSCODE":
									ivElem.HSCode = resValue.ToString();
									break;
								case "CURRENCY":
									ivElem.Currency = resValue.ToString();
									break;
								case "ORIGIN":
									ivElem.Origin = resValue.ToString();
									break;
								case "AMOUNT":
									ivElem.Amount = String.Format("{0:0.000}", resValue);
									ivElem.Amount = ivElem.Amount.Length > 1 ? ivElem.Amount.Substring(0, ivElem.Amount.Length - 1) : ivElem.Amount;
									break;
								case "NUMBER":
									ivElem.Number = String.Format("{0:0.000}", resValue);
									ivElem.Number = ivElem.Number.Length > 1 ? ivElem.Number.Substring(0, ivElem.Number.Length - 1) : ivElem.Number;
									break;
								case "NETWEIGHT":
									ivElem.NetWeight = String.Format("{0:0.000}", resValue);
									ivElem.NetWeight = ivElem.NetWeight.Length > 1 ? ivElem.NetWeight.Substring(0, ivElem.NetWeight.Length - 1) : ivElem.NetWeight;
									break;
								case "EXIST":
									ivElem.Exist = resValue.ToString();
									break;
								case "INVOICENO":
									ivElem.InvoiceNo = resValue.ToString();
									break;
								case "HSCODEKEY":
									ivElem.HSCodeKey = resValue.ToString();
									break;
								case "HSCODECLUE":
									ivElem.HSCodeClue = resValue.ToString();
									break;
								case "HSCODESUGGESTION":
									ivElem.HSCodeSuggestion = resValue.ToString();
									break;
								case "DESCRIPTION":
									ivElem.Description = resValue.ToString();
									break;
								default:
									break;
							}
						}

					}
				}));
			}

			App.MainView.SideChannelData.Add(sideChanProf.label, new ObservableSortedCollection<SideChannelDataContainer>());
		}
		var gridList = renderCols(sideChanProfs);
		if (gridList.Count == 0) return;
		populate(sideChanProfs, gridList);
	}

	private static string getRegisterData(string regName, InvoiceDataElement ivElem)
	{
		if (regName == null || ivElem == null) return null;
		string res = null;
		if (App.MainView.DataRegister.DataRegisterColumn.ContainsKey(regName))
		{
			object reg = App.MainView.DataRegister.DataRegisterColumn[regName];
			if (reg.GetType() == typeof(List<int>))
			{
				if ((reg as List<int>).Count - 1 < ivElem.IndexNo) return null;
				else return (reg as List<int>)[ivElem.IndexNo].ToString();
			}
			else if (reg.GetType() == typeof(List<double>))
			{
				if ((reg as List<double>).Count - 1 < ivElem.IndexNo) return null;
				else return (reg as List<double>)[ivElem.IndexNo].ToString();
			}
			else if (reg.GetType() == typeof(List<string>))
			{
				if ((reg as List<string>).Count - 1 < ivElem.IndexNo) return null;
				else return (reg as List<string>)[ivElem.IndexNo];
			}

		}
		return res;

	}

	private static string getSideChannelData(string tableName,string fieldName, string key)
    {
		if (tableName == null || fieldName == null || key == null) return null;
		string res = null;
		if (App.MainView.SideChannelData.ContainsKey(tableName))
		{
			var sideChannelData = App.MainView.SideChannelData[tableName].Find(new SideChannelDataContainer() { Key = key });
			if (sideChannelData != null && sideChannelData.ExtraProperties.ContainsKey(fieldName))
			{
				res = sideChannelData.ExtraProperties[fieldName].Value;
			}
		}
		return res;
		
	}

	private static string getFieldData(string fieldName ,string key, InvoiceDataElement ivElem)
    {
		if (fieldName == null || key == null || ivElem == null) return null;
		string res = null;
		if (Enum.IsDefined(typeof(ConstValue.FieldNameEnum), fieldName))
		{
			switch (fieldName)
			{
				case "HSCODE":
					res = ivElem.HSCode;
					break;
				case "CURRENCY":
					res = ivElem.Currency;
					break;
				case "ORIGIN":
					res = ivElem.Origin;
					break;
				case "AMOUNT":
					res = ivElem.Amount;
					break;
				case "NUMBER":
					res = ivElem.Number;
					break;
				case "NETWEIGHT":
					res = ivElem.NetWeight;
					break;
				case "EXIST":
					res = ivElem.Exist;
					break;
				case "INVOICENO":
					res = ivElem.InvoiceNo;
					break;
				case "HSCODEKEY":
					res = ivElem.HSCodeKey;
					break;
				case "HSCODECLUE":
					res = ivElem.HSCodeClue;
					break;
				case "HSCODESUGGESTION":
					res = ivElem.HSCodeSuggestion;
					break;
				case "DESCRIPTION":
					res = ivElem.Description;
					break;
				default:
					break;
			}
		}
		else if (App.MainView.InvoiceBucketData.BucketDataColumn.ContainsKey(fieldName)
				&& App.MainView.InvoiceBucketData.BucketDataColumn[fieldName].BucketData.ContainsKey(key))
		{
			res = String.Format("{0:0.000}", App.MainView.InvoiceBucketData.BucketDataColumn[fieldName].BucketData[key].Value);
			res = res.Length > 1 ? res.Substring(0, res.Length - 1) : res;
		}
		return res;
	}

	public static void Calc()
	{
		foreach (var ivElem in App.MainView.InvoiceData)
		{
			foreach(var sideChanConv in SideChannelConverters)
            {
				sideChanConv(ivElem);
            }
		}		
	}
	private static IVEditElasticClient.Side_Channel[] getSideChannel()
    {
		if (App.MainView.CurrentCompanyProfile == null || App.MainView.CurrentCompanyProfile.side_channel == null || App.MainView.CurrentCompanyProfile.side_channel.Length == 0) return null;
		else return App.MainView.CurrentCompanyProfile.side_channel;

	}

	private static SortedList<string,DataGrid> renderCols(IVEditElasticClient.Side_Channel[] profs)
    {
		SortedList<string, DataGrid> res = new SortedList<string, DataGrid>();
		var tab = App.MainView.SubWindowController.SideChannelWindow.SideChannelTab;
		foreach (var prof in profs) {
			var tabItem = new TabItem() { Header = prof.label };
			var stackPanel = new StackPanel() { Orientation = Orientation.Vertical };
			var grid = new DataGrid() { Margin = new System.Windows.Thickness(0, 0, 0, 0), AutoGenerateColumns = false, CanUserAddRows = false };
			grid.Columns.Add(new DataGridTextColumn
			{
				// bind to a dictionary property
				Binding = new Binding("Key"),
				Header = prof.key_field
			}) ;
			foreach (var colName in SideChannelDataProfs[prof.label])
			{
				grid.Columns.Add(new DataGridTextColumn
				{
					// bind to a dictionary property
					Binding = new Binding("ExtraProperties[" + colName + "].Value"),
					Header = colName
				});
			}
			stackPanel.Children.Add(grid);
			tabItem.Content = stackPanel;
			tab.Items.Add(tabItem);
			res.Add(prof.label, grid);
		}
		return res;
	}
	private static void populate(IVEditElasticClient.Side_Channel[] profs, SortedList<string,DataGrid> grids)
    {
		foreach (var prof in profs)
		{
			string profLabel = prof.label;
			if (prof.is_autogenerate)
			{
				var invData = App.MainView.InvoiceData;
				IEnumerable<IGrouping<string, InvoiceDataElement>> tempData = null;
				switch (prof.key_field) {
					case "INVOICENO":
						tempData = invData.GroupBy(elem => elem.InvoiceNo);
						break;
					case "ORIGIN":
						tempData = invData.GroupBy(elem =>  elem.Origin);
						break;
					case "HSCODEKEY":
						tempData = invData.GroupBy(elem => elem.HSCodeKey);
						break;
					case "HSCODECLUE":
						tempData = invData.GroupBy(elem => elem.HSCodeClue);
						break;
					case "CURRENCY":
						tempData = invData.GroupBy(elem => elem.Currency);
						break;
					case "DESCRIPTION":
						tempData = invData.GroupBy(elem => elem.Description);
						break;
					case "HSCODE":
						tempData = invData.GroupBy(elem => elem.HSCode);
						break;
					default:
						continue;
				}
				foreach (var tempElem in tempData)
                {
					var dataCon = new SideChannelDataContainer() { Key = tempElem.Key };
					foreach (var field in prof.field)
                    {
						dataCon.ExtraProperties.Add(field.label, new SideChannelDataContainer.ExtraPropCont());
                    }
					App.MainView.SideChannelData[profLabel].Add(dataCon);
				}
				grids[profLabel].ItemsSource = App.MainView.SideChannelData[profLabel];

			}
		}
	}

}

public class SideChannelDataContainer : IIndexed, INotifyPropertyChanged, IComparable<SideChannelDataContainer>
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

	public string Key { set; get; }
	public SortedList<string, ExtraPropCont> ExtraProperties { set; get; } = new SortedList<string, ExtraPropCont>();
		public class ExtraPropCont:INotifyPropertyChanged
		{
			public event PropertyChangedEventHandler PropertyChanged;

			private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
			{
				if (PropertyChanged != null)
				{

					PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
				}
			}

			private string _warning = "";

			public string Warning 
			{
				set 
				{
					_warning = value;
					NofityPropertyChanged("Warning");
				}
				get
				{
					return _warning;
				}

			}
    
			private string _value;
			public string Value 
			{
				set 
				{
					_value = value;
					NofityPropertyChanged("Value");
				}
				get
				{
					return _value;
				}
			}
		}

	public int Compare(SideChannelDataContainer _this, SideChannelDataContainer that)
    {
		return String.Compare(that.Key, _this.Key);
    }

	public int CompareTo(SideChannelDataContainer that)
    {
		return Compare(this, that);
    }
	
}