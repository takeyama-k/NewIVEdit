using System.Windows;

namespace NewIVEdit
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ElasticConfigWindow : Window
    {
        public ElasticConfigWindowViewModel View { set; get; }
        public ElasticConfigWindow()
        {
            InitializeComponent();
        }
        private void OnClickRegister(object sender, RoutedEventArgs e){
            View.Register(sender, e);
            App.MainView.ResetCompanyProfiles();
            this.Close();
        }

        
    }
}