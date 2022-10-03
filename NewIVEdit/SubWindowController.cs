using System;
using System.Windows;
using System.Windows.Controls;
using NewIVEdit;
using System.Text.RegularExpressions;
public class SubWindowController
{
	public ElasticConfigWindow ElasticConfigWindow = null;
    public PrinterConfigWindow PrinterConfigWindow = null;
    public DataWindow DataWindow = null;
    public CurrencyWindow CurrencyWindow = null;
    public InputWindow InputWindow = null;
    public DocumentNoWindow DocunoWindow = null;
    public SideChannelWindow SideChannelWindow = null;
    public SubWindowController()
	{
	}

    public void OpenSideChannelWindow(object sender, RoutedEventArgs e)
    {
        if(SideChannelWindow == null)
        {
            SideChannelWindow = new SideChannelWindow();
            SideChannelWindow.Closed += (s, ev) => { SideChannelWindow = null; };
            SideChannelDataViewModel.Init();
            SideChannelWindow.Show();
        }
        else
        {
            SideChannelWindow = null;
        }
    }

	public void OpenElasticConfigWindew(object  sender, RoutedEventArgs e)
    {
		if(ElasticConfigWindow != null)
        {
			ElasticConfigWindow.Activate();
        }
        else
        {
            ElasticConfigWindow = new ElasticConfigWindow()
            {
                View = new ElasticConfigWindowViewModel()
            };
            ElasticConfigWindow.DataContext = ElasticConfigWindow.View;
            ElasticConfigWindow.View.ElasticHostUri = Regex.Replace(App.MainView.ElasticClient.ElasticServiceHost.AbsoluteUri, ":[0-9]+(/)?", "");
            ElasticConfigWindow.View.ElasticHostPortNo = App.MainView.ElasticClient.ElasticServiceHost.Port.ToString();
            ElasticConfigWindow.Closed += (object closesender, EventArgs closeeventargs) =>
            {
                this.ElasticConfigWindow = null;
            };
            ElasticConfigWindow.Show();
        }
    }
    public void OpenPrinterSettingWindew(object sender, RoutedEventArgs e)
    {
        if (PrinterConfigWindow != null)
        {
            PrinterConfigWindow.Activate();
        }
        else
        {
            PrinterConfigWindow = new PrinterConfigWindow()
            {
                View = new PrinterConfigWindowViewModel()
            };
            PrinterConfigWindow.View.PrinterName = App.MainView.CurrentPrinterName;
            PrinterConfigWindow.DataContext = PrinterConfigWindow.View;
            PrinterConfigWindow.Closed += (object closesender, EventArgs closeeventargs) =>
            {
                this.PrinterConfigWindow = null;
            };
            PrinterConfigWindow.Show();
        }
    }
    public void OpenDocnoWindew(object sender, RoutedEventArgs e)
    {
        if (DocunoWindow != null)
        {
            if((sender as Button) != null && (sender as Button).Tag != null && (sender as Button).Tag.ToString() == "GenerateButton")
            {
                DocunoWindow.GenerateButton.Visibility = Visibility.Visible;
            }
            else
            {
                DocunoWindow.GenerateButton.Visibility = Visibility.Hidden;
            }
            DocunoWindow.Activate();
        }
        else
        {
            DocunoWindow = new DocumentNoWindow()
            {
                DataContext = App.MainView,
            };
            DocunoWindow.Closed += (object closesender, EventArgs closeeventargs) =>
            {
                this.DocunoWindow = null;
            };
            if ((sender as Button) != null && (sender as Button).Tag != null && (sender as Button).Tag.ToString() == "GenerateButton")
            {
                DocunoWindow.GenerateButton.Visibility = Visibility.Visible;
            }
            else
            {
                DocunoWindow.GenerateButton.Visibility = Visibility.Hidden;
            }
            DocunoWindow.Show();
        }
    }
    public void OpenCurrencyWindow(object sender, RoutedEventArgs eventargs)
    {
        if (CurrencyWindow != null)
        {
            if ((sender as Button) != null && (sender as Button).Tag != null && (sender as Button).Tag.ToString() == "CalculateButton")
            {
                CurrencyWindow.CalculateButton.Visibility = Visibility.Visible;
            }
            else
            {
                CurrencyWindow.CalculateButton.Visibility = Visibility.Hidden;
            }
            CurrencyWindow.Activate();
        }
        else
        {
            CurrencyWindow = new CurrencyWindow();
            if ((sender as Button) != null && (sender as Button).Tag != null && (sender as Button).Tag.ToString() == "CalculateButton")
            {
                CurrencyWindow.CalculateButton.Visibility = Visibility.Visible;
            }
            else
            {
                CurrencyWindow.CalculateButton.Visibility = Visibility.Hidden;
            }
            CurrencyWindow.CurrencyProfilesControl.ItemsSource = App.MainView.CurrencyProfiles;
            CurrencyWindow.Closed += (s, e) =>
            {
                CurrencyWindow = null;
            };
            CurrencyWindow.Show();
        }
    }

    public void OpenInputWindow(object sender, RoutedEventArgs eventargs)
    {
        if (InputWindow != null)
        {
            InputWindow.Activate();
            return;
        }
        InputWindow = new InputWindow();
        InputWindow.DataContext = App.MainView.DeclArtcile;
        InputWindow.Closed += (object s, EventArgs e) =>
        {
            InputWindow = null;
        };
        InputWindow.Show();
    }
    public void OpenDataWindow(object sender, RoutedEventArgs e)
    {
        if(DataWindow != null)
        {
            DataWindow.Activate();
        }
        else 
        {
            DataWindow = new DataWindow();
            DataWindow.DataContext = DataWindow.View;
            DataWindow.CompanyCombobox.ItemsSource = App.MainView.CompanyIndicies;
            DataWindow.CompanySubTypeCombobox.ItemsSource = App.MainView.CompanySubtypes;
            DataWindow.InvoiceNoCombobox.ItemsSource = App.MainView.InvoiceIndex;
            DataWindow.InvoiceDataGrid.ItemsSource = App.MainView.InvoiceData;
            DataWindow.PackingListDataGrid.ItemsSource = App.MainView.PackingListData;
            DataWindow.TradetermCombobox.ItemsSource = App.MainView.TradetermProfile;
            DataWindow.PrimeCurrencyCombobox.ItemsSource = App.MainView.CurrencyProfiles;
            DataWindow.PrimeCurrencyCombobox.SelectedItem = App.MainView.FindPrimeCurrency();
            DataWindow.PrimeCurrencyCombobox.SelectionChanged += (object _sender, SelectionChangedEventArgs _e) =>
            {
                var selected = (CurrencyProfile)DataWindow.PrimeCurrencyCombobox.SelectedItem;
                App.MainView.UpdatePrimeCurrency(selected);
            };
            DataWindow.CompanyCombobox.SelectionChanged += (object _sender, SelectionChangedEventArgs _e) =>
            {
                if (DataWindow != null)
                {
                    IVEditElasticClient.CompanyIndex selected = (IVEditElasticClient.CompanyIndex)DataWindow.CompanyCombobox.SelectedItem;
                    if (selected == null || selected.subtype == null || selected.subtype.Count == 0) {
                        App.MainView.CompanySubtypes.Clear();
                        App.MainView.CompanyProfiles.Clear();
                        return;
                    }
                    var nextSubtype = selected.subtype;
                    App.MainView.CompanySubtypes.Clear();
                    App.MainView.CompanyProfiles.Clear();
                    foreach (var subtype in nextSubtype)
                    {
                        var prof = IVEditElasticClient.GetCompanyProfile(subtype.ref_id);
                        if (prof == null) continue;
                        App.MainView.CompanyProfiles.Add(prof);
                        App.MainView.CompanySubtypes.Add(new IVEditElasticClient.CompanyIndex.Subtype()
                        {
                            ref_id = subtype.ref_id,
                            key = subtype.key,
                            description = subtype.description,
                            hsmaster = subtype.hsmaster,
                            ref_prof = prof,
                        }) ;
                    }
                    App.MainView.CurrentCompanyIndex = selected;
                    if (App.MainView.CompanySubtypes.Count > 0)
                    {
                        DataWindow.CompanySubTypeCombobox.SelectedItem = App.MainView.CompanySubtypes[0];
                        App.MainView.CurrentCompanyProfile = App.MainView.CompanySubtypes[0].ref_prof;
                        App.MainView.CurrentCompanySubtype = App.MainView.CompanySubtypes[0];
                    }
                    else
                    {
                        DataWindow.CompanySubTypeCombobox.SelectedItem = null;
                        App.MainView.CurrentCompanySubtype = null;
                    }
                }
                else
                {
                    App.MainView.CurrentCompanyIndex = null;
                }

            };
            DataWindow.CompanySubTypeCombobox.SelectionChanged += (object _sender, SelectionChangedEventArgs _e) =>
            {
                string prevHsIndex = App.MainView.HSMasterIndex;
                if (_e.AddedItems == null || _e.AddedItems.Count == 0) return;
                IVEditElasticClient.CompanyIndex.Subtype selected = (IVEditElasticClient.CompanyIndex.Subtype)DataWindow.CompanySubTypeCombobox.SelectedItem;
                if(selected != null)
                {
                    App.MainView.CurrentCompanySubtype = selected;
                    App.MainView.CurrentCompanyProfile = selected.ref_prof;
                }
                else
                {
                    App.MainView.CurrentCompanySubtype = null;
                    App.MainView.CurrentCompanyProfile = null;
                    App.MainView.ImmediateHSMaster.Clear();
                    return;
                }
                App.MainView.InvoiceBucketData.SetUp();
                App.MainView.DataRegister.SetUp();
                App.MainView.InvoiceRegexPattern.Load();
                App.MainView.PackingListRegexPattern.Load(true);
                if (App.MainView.CurrentCompanyProfile.defaulttradeterm != null)
                {
                    string tt = App.MainView.CurrentCompanyProfile.defaulttradeterm;
                    var found = App.MainView.TradetermProfile.FindLast(new TradetermProfile() { Tradeterm = tt });
                    if (found.Tradeterm == tt) DataWindow.TradetermCombobox.SelectedItem = found;
                }
                if (!IVEditElasticClient.CheckIfClientValid()) {
                    App.MainView.IsHSMasterValid = false;
                    App.MainView.HSMasterIndex = null;
                    return;
                }
                string indexname = "";
                if (selected.hsmaster == null || selected.hsmaster == "")
                {
                    string orgNo = ((IVEditElasticClient.CompanyIndex)DataWindow.CompanyCombobox.SelectedItem).organizationno;
                    if (orgNo == null || orgNo == "") return;
                    indexname = "hsmaster_" + orgNo + "_1";
                }
                else
                {
                    indexname = selected.hsmaster;
                }
                if(prevHsIndex != null && prevHsIndex != indexname)
                {
                    App.MainView.ImmediateHSMaster.Clear();
                }
                if (IVEditElasticClient.CheckIfIndexExist(indexname))
                {
                    if (selected.hsmaster == null || selected.hsmaster == "")
                    {
                        App.MainView.HSMasterIndex = indexname;
                        App.MainView.IsHSMasterValid = true;
                        App.MainView.CurrentCompanySubtype.hsmaster = indexname;
                        IVEditElasticClient.UpdateCompanyIndex(App.MainView.CurrentCompanyIndex);
                    }
                    else
                    {
                        App.MainView.HSMasterIndex = indexname;
                        App.MainView.IsHSMasterValid = true;
                        App.MainView.CurrentCompanySubtype.hsmaster = indexname;
                    }
                }
                else
                {
                    if (IVEditElasticClient.CreateIndex(indexname))
                    {
                        App.MainView.HSMasterIndex = indexname;
                        App.MainView.IsHSMasterValid = true;
                        App.MainView.CurrentCompanySubtype.hsmaster = indexname;
                        IVEditElasticClient.UpdateCompanyIndex(App.MainView.CurrentCompanyIndex);
                        return;
                    }
                    else
                    {
                        App.MainView.HSMasterIndex = null;
                        App.MainView.IsHSMasterValid = false;
                    }
                }
                
            };
            DataWindow.TradetermCombobox.SelectionChanged += (object _sender, SelectionChangedEventArgs _e) =>
            {
                TradetermProfile selected = (TradetermProfile)DataWindow.TradetermCombobox.SelectedItem;
                App.MainView.CurrentTradeterm = selected;
                if (DataWindow != null)
                {
                    DataWindow.View.FaceValue = DataWindow.View.FaceValue ?? "";
                    DataWindow.View.FOBValue = DataWindow.View.FOBValue ?? ""; 
                }
            };

            if (App.MainView.CurrentCompanyIndex != null)
            {
                DataWindow.CompanyCombobox.SelectedItem = App.MainView.CurrentCompanyIndex;
            }
            DataWindow.Closed += (object closesender, EventArgs closeeventargs) =>
            {
                this.DataWindow.InvoiceDataGrid.CancelEdit();
                this.DataWindow.InvoiceDataGrid.CancelEdit();
                this.DataWindow = null;
            };
            DataWindow.TradetermCombobox.SelectedItem = App.MainView.CurrentTradeterm;
            App.MainView.DeclearationAccum.SumUP(App.MainView.IsPrimeCurrencyForced);
            DataWindow.Show();
        }
    }

}
