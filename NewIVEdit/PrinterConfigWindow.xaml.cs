using System.Windows;

namespace NewIVEdit
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class PrinterConfigWindow : Window
    {
        public PrinterConfigWindowViewModel View { set; get; }
        public PrinterConfigWindow()
        {
            InitializeComponent();
        }
        private void OnClickRegister(object sender, RoutedEventArgs e)
        {
            View.Register(sender, e);
            this.Close();
        }
    }
}