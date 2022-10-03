using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NewIVEdit
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OnClickLoad(object sender, RoutedEventArgs e)
        {
            App.MainView.LoadPdf();
        }

        private void OnClickPen(object sender, RoutedEventArgs e)
        {
            if (App.MainView.RenderController.CurrentEditingMode != ConstValue.EditingModeEnum.Ink)
            {
                App.MainView.ChangeMode(ConstValue.EditingModeEnum.Ink);
            }
            else
            {
                App.MainView.ChangeMode(ConstValue.EditingModeEnum.Hand);
            }
        }

        private void OnClickData(object sender, RoutedEventArgs e)
        {
            App.MainView.OpenDataWindow(sender, e);
        }

        private void OnClickCurrency(object sender, RoutedEventArgs e)
        {
            App.MainView.OpenCurrencyWindow(sender, e);
        }

        private void OnClickInput(object sender, RoutedEventArgs e)
        {
            App.MainView.OpenInputWindow(sender, e);
        }
        private void OnClickDocno(object sender, RoutedEventArgs e)
        {
            App.MainView.OpenDocnoWindow(sender, e);
        }
        private void OnClickSavePdf(object sender, RoutedEventArgs e)
        {
            if(App.MainView.Pages.Count == 0)
            {
                MessageBox.Show("保存するページがありません");
                return;
            }
            App.MainView.SavePdf();
        }

        private void OnClickThumbneil(object sender, RoutedEventArgs e)
        {
            if(App.MainView.SubWindowController.DataWindow != null)
            {
                App.MainView.SubWindowController.DataWindow.InvoiceDataGrid.CancelEdit();
                App.MainView.SubWindowController.DataWindow.InvoiceDataGrid.CancelEdit();
            }
            int pageNo = 0;
            var thubneilButton = sender as Button;
            if (thubneilButton == null || thubneilButton.Tag == null) return;
            if (!int.TryParse(thubneilButton.Tag.ToString(), out pageNo)) return;
            if (pageNo < 1 || pageNo > App.MainView.Pages.Count) return;
            App.MainView.PageNavigate(pageNo);
        }
        private void OnClickTrashcan(object sender, RoutedEventArgs e)
        {
            if (App.MainView.SubWindowController.DataWindow != null)
            {
                App.MainView.SubWindowController.DataWindow.InvoiceDataGrid.CancelEdit();
                App.MainView.SubWindowController.DataWindow.InvoiceDataGrid.CancelEdit();
            }
            DataWindowViewModel.Reset();
            App.MainView.RemoveInvoicePdf();
        }

        private void Main_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            App.MainView.RenderController.ScaleMain(sender, e);
        }

        private void Main_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.MainView.RenderController.MainLeftButtonDown(sender, e);
        }
        private void InkCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.MainView.RenderController.MainLeftButtonDown(sender, e);
        }

        private void InkCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.MainView.RenderController.InkCanvasLeftButtonDown(sender, e);
        }
        private void InkCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            App.MainView.RenderController.InkCanvasLeftButtonUp(sender, e);
        }


        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            App.MainView.RenderController.DragMain(sender, e);
        }
        private void Main_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            App.MainView.RenderController.MainLeftButtonUp(sender, e);
        }

        private void InkCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            App.MainView.RenderController.MainLeftButtonUp(sender, e);
        }

        private void Menu_ElasticSetting(object sender, RoutedEventArgs e)
        {
            App.MainView.SubWindowController.OpenElasticConfigWindew(sender, e);
        }
        private void Menu_PrinterSetting(object sender, RoutedEventArgs e)
        {
            App.MainView.SubWindowController.OpenPrinterSettingWindew(sender, e);
        }

        private void Main_OnThumbneilMouseEnter(object sender ,RoutedEventArgs e)
        {
            App.MainView.SwitchThumbneilMouseOver(sender);
        }
        private void Main_OnThumbneilMouseLeave(object sender ,RoutedEventArgs e)
        {
            App.MainView.SwitchThumbneilMouseOver(sender);
        }

        private void Main_OnClickThumbneilDelete(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null || button.Tag == null) return;
            int page = 0;
            if (!int.TryParse(button.Tag.ToString(), out page)) ;
            App.MainView.DeletePage(page);
        }

    }
}
