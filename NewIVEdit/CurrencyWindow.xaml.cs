using System.Windows;

namespace NewIVEdit
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class CurrencyWindow : Window
    {
        public CurrencyWindow()
        {
            InitializeComponent();

        }
        private void OnClickCalc(object sender, RoutedEventArgs e) 
        {
            if (DataWindowViewModel.GenerateInput(sender, e))
            {
                this.Close();
            }
        }
    }
    

}