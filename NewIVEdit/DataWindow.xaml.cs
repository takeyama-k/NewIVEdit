using System.Windows;
using System.Windows.Controls;

namespace NewIVEdit
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DataWindow : Window
    {
        public DataWindowViewModel View { set; get; } = new DataWindowViewModel();
        public DataWindow()
        {
            InitializeComponent();
            InvoiceDataGrid.MouseRightButtonUp += datgrid_invoice_OnMouseRightButtonUp;
            PackingListDataGrid.MouseRightButtonUp += datgrid_packinglist_OnMouseRightButtonUp;
        }
        private void Menu_ReadDataFromPdf(object sender, RoutedEventArgs e)
        {
            if (App.MainView.CurrentCompanyProfile == null)
            {
                MessageBox.Show("会社が選択されていません!");
                return;
            }
            App.MainView.CurrentInvoiceNoCntr++;
            View.ReadDataFromPdf();
            if(App.MainView.CurrentCompanyProfile.packing_existby != null && App.MainView.CurrentCompanyProfile.packing_existby_secondary != null) View.ReadDataFromPdf(true);
        }
        private void Menu_OpenInvoiceFile(object sender, RoutedEventArgs e)
        {
            if(App.MainView.CurrentCompanyProfile == null)
            {
                MessageBox.Show("会社が選択されていません!");
                return;
            }
            App.MainView.CurrentInvoiceNoCntr++;
            View.OpenExcelFile();
        }
        private void Menu_OpenPackingListFile(object sender, RoutedEventArgs e)
        {
            if (App.MainView.CurrentCompanyProfile == null)
            {
                MessageBox.Show("会社が選択されていません!");
                return;
            }
            View.OpenExcelFile(true);
        }

        private void Menu_OpenSideChannel(object sender, RoutedEventArgs e)
        {
            App.MainView.SubWindowController.OpenSideChannelWindow(sender,e);
        }

        public void OnClickExtract(object sender, RoutedEventArgs e)
        {
            InvoiceIndex idx = (InvoiceIndex)InvoiceNoCombobox.SelectedItem;
            if(idx == null)
            {
                MessageBox.Show("インボイスNOが選択されていません");
                return;
            }
            View.ExtractData(idx);
            if (App.MainView.CurrentCompanyProfile.packing_existby != null) View.ExtractData(idx,true);
        }

        private void OnClickReset(object sender ,RoutedEventArgs e)
        {
            DataWindowViewModel.Reset();
        }
        public void OnClickCalc(object sender, RoutedEventArgs e)
        {
            DataWindowViewModel.GenerateInput(sender, e);
        }

        public void OnClickIVPlus(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button == null || button.Tag == null) return;
            int index = 0;
            if (button.Tag.ToString() == "IVHeader")
            {
                index = App.MainView.InvoiceData.Count;
            }
            else
            {
                if (!int.TryParse(button.Tag.ToString(), out index)) return;
            }
            View.AddInvoiceRow(index);

        }
        public void OnClickIVMinus(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button == null || button.Tag == null) return;
            int index = 0;
            if (button.Tag.ToString() == "IVHeader")
            {
                index = App.MainView.InvoiceData.Count - 1;
            }
            else
            {
                if (!int.TryParse(button.Tag.ToString(), out index)) return;
            }
            View.DeleteInvoiceRow(index);
        }
        public void OnClickPLPlus(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button == null || button.Tag == null) return;
            int index = 0;
            if (button.Tag.ToString() == "PLHeader")
            {
                index = App.MainView.PackingListData.Count;
            }
            else
            {
                if (!int.TryParse(button.Tag.ToString(), out index)) return;
            }
            View.AddPackingListRow(index);

        }
        public void OnClickPLMinus(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button == null || button.Tag == null) return;
            int index = 0;
            if (button.Tag.ToString() == "PLHeader")
            {
                index = App.MainView.PackingListData.Count - 1;
            }
            else
            {
                if (!int.TryParse(button.Tag.ToString(), out index)) return;
            }
            View.DeletePackingListRow(index);
        }
        public void OnClickInvoiceRightArrow(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button == null || button.Tag == null) return;
            int index = 0;
            if (button.Tag.ToString() == "ALL")
            {
                View.CopyInvoiceSuggestion();
            }if(int.TryParse(button.Tag.ToString(), out index))
            {
                View.CopyInvoiceSuggestion(index);
            }
        }

        public void OnClickInvoiceDownArrow(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button == null || button.Tag == null) return;
            int index = 0;
            if (button.Tag.ToString() == "CURRENCY")
            {
                View.DuplicateCurrency();
            }else if(button.Tag.ToString() == "ORIGIN")
            {
                View.DuplicateOrigin();
            }

        }
        public void OnClickPackingListDownArrow(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button == null || button.Tag == null) return;
            int index = 0;
            if (button.Tag.ToString() == "CURRENCY")
            {
                View.DuplicateCurrency(true);
            }
            else if (button.Tag.ToString() == "ORIGIN")
            {
                View.DuplicateOrigin(true);
            }

        }

        public void OnClickPackingListRightArrow(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button == null || button.Tag == null) return;
            int index = 0;
            if (button.Tag.ToString() == "ALL")
            {
                View.CopyInvoiceSuggestion();
            }
            if (int.TryParse(button.Tag.ToString(), out index))
            {
                View.CopyInvoiceSuggestion(index);
            }
        }

        private void datgrid_invoice_OnMouseRightButtonUp(object sender , System.Windows.Input.MouseButtonEventArgs e)
        {
            var datagrid = InvoiceDataGrid;
            if (datagrid.SelectedCells.Count < 1) return;
            string headr = datagrid.SelectedCells[0].Column.Header.ToString();
            int colindex = datagrid.SelectedCells[0].Column.DisplayIndex;
            var item = datagrid.SelectedCells[0].Item as InvoiceDataElement;
            int rowindex = item.IndexNo;
            MenuItem deletemenuitem = new MenuItem() { Header="セルを削除して上詰め"};
            MenuItem inserttonextblancitem = new MenuItem() { Header="空白セルを挿入(次の空白まで下詰め)"};
            MenuItem addblancitem = new MenuItem() { Header = "空白セルを挿入(以降全体を下詰め)"};
            string messeage = "";
            string menuCaption = "";
            switch (colindex) {
                case 2:
                    messeage = "HSKEY" + "," + rowindex.ToString();
                    menuCaption = "商品コード";
                    break;
                case 3:
                    messeage = "HSCLUE" + "," + rowindex.ToString();
                    menuCaption = "HS ヒント";
                    break;
                case 4:
                    messeage = "NETWEIGHT" + "," + rowindex.ToString();
                    menuCaption = "Net Weight";
                    break;
                case 5:
                    messeage = "NUMBER" + "," + rowindex.ToString();
                    menuCaption = "数量";
                    break;
                case 6:
                    messeage = "AMOUNT" + "," + rowindex.ToString();
                    menuCaption = "金額";
                    break;
                case 7:
                    messeage = "CURRENCY" + "," + rowindex.ToString();
                    menuCaption = "通貨";
                    break;
                case 8:
                    messeage = "ORIGIN" + "," + rowindex.ToString();
                    menuCaption = "原産国";
                    break;
                case 9:
                    messeage = "DESCRIPTION" + "," + rowindex.ToString();
                    menuCaption = "説明";
                    break;
                default:
                    return;
                    break;
            }
            deletemenuitem.Tag = messeage;
            deletemenuitem.Click += datagrid_invoice_contextmenu_deletecell;
            inserttonextblancitem.Tag = messeage;
            inserttonextblancitem.Click += datagrid_invoice_contextmenu_inserttonextblanc;
            addblancitem.Tag = messeage;
            addblancitem.Click += datagrid_invoice_contextmenu_insertblanc;
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.Items.Add(new MenuItem() { Header = menuCaption, IsEnabled = false });
            contextMenu.Items.Add(deletemenuitem);
            contextMenu.Items.Add(inserttonextblancitem);
            contextMenu.Items.Add(addblancitem);
            InvoiceDataGrid.ContextMenu = contextMenu;
        }

        private void datgrid_packinglist_OnMouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var datagrid = PackingListDataGrid;
            if (datagrid.SelectedCells.Count < 1) return;
            string headr = datagrid.SelectedCells[0].Column.Header.ToString();
            int colindex = datagrid.SelectedCells[0].Column.DisplayIndex;
            var item = datagrid.SelectedCells[0].Item as InvoiceDataElement;
            int rowindex = item.IndexNo;
            MenuItem deletemenuitem = new MenuItem() { Header = "セルを削除して上詰め" };
            MenuItem inserttonextblancitem = new MenuItem() { Header = "空白セルを挿入(次の空白まで下詰め)" };
            MenuItem addblancitem = new MenuItem() { Header = "空白セルを挿入(以降全体を下詰め)" };
            string messeage = "";
            string menuCaption = "";
            switch (colindex)
            {
                case 2:
                    messeage = "HSKEY" + "," + rowindex.ToString();
                    menuCaption = "商品コード";
                    break;
                case 3:
                    messeage = "HSCLUE" + "," + rowindex.ToString();
                    menuCaption = "HS ヒント";
                    break;
                case 4:
                    messeage = "NETWEIGHT" + "," + rowindex.ToString();
                    menuCaption = "Net Weight";
                    break;
                case 5:
                    messeage = "NUMBER" + "," + rowindex.ToString();
                    menuCaption = "数量";
                    break;
                case 6:
                    messeage = "ORIGIN" + "," + rowindex.ToString();
                    menuCaption = "原産国";
                    break;
                case 7:
                    messeage = "DESCRIPTION" + "," + rowindex.ToString();
                    menuCaption = "説明";
                    break;
                case 14:
                    messeage = "CURRENCY" + "," + rowindex.ToString();
                    menuCaption = "通貨";
                    break;
                default:
                    return;
                    break;
            }
            deletemenuitem.Tag = messeage;
            deletemenuitem.Click += datagrid_packinglist_contextmenu_deletecell;
            inserttonextblancitem.Tag = messeage;
            inserttonextblancitem.Click += datagrid_packinglist_contextmenu_inserttonextblanc;
            addblancitem.Tag = messeage;
            addblancitem.Click += datagrid_packinglist_contextmenu_insertblanc;
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.Items.Add(new MenuItem() { Header = menuCaption, IsEnabled = false });
            contextMenu.Items.Add(deletemenuitem);
            contextMenu.Items.Add(inserttonextblancitem);
            contextMenu.Items.Add(addblancitem);
            PackingListDataGrid.ContextMenu = contextMenu;
        }

        private void datagrid_invoice_contextmenu_deletecell(object sender, RoutedEventArgs e)
        {
            DataWindowViewModel.DeleteInvoiceCell(sender, e);
        }
        private void datagrid_invoice_contextmenu_inserttonextblanc(object sender, RoutedEventArgs e)
        {
            DataWindowViewModel.InsertInvoiceCell(sender, e, true);
        }
        private void datagrid_invoice_contextmenu_insertblanc(object sender, RoutedEventArgs e)
        {
            DataWindowViewModel.InsertInvoiceCell(sender, e);
        }
        private void datagrid_packinglist_contextmenu_deletecell(object sender, RoutedEventArgs e)
        {
            DataWindowViewModel.DeleteInvoiceCell(sender, e,true);
        }
        private void datagrid_packinglist_contextmenu_inserttonextblanc(object sender, RoutedEventArgs e)
        {
            DataWindowViewModel.InsertInvoiceCell(sender, e, true,true);
        }
        private void datagrid_packinglist_contextmenu_insertblanc(object sender, RoutedEventArgs e)
        {
            DataWindowViewModel.InsertInvoiceCell(sender, e,false,true);
        }
    }
}