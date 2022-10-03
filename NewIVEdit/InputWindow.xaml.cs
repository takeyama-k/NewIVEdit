using System.Windows;


namespace NewIVEdit
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class InputWindow : Window
    {
        public InputWindowViewModel View { set; get; } = new InputWindowViewModel();
        public InputWindow()
        {
            InitializeComponent();
        }
        private void OnClickAddLicence(object sender, RoutedEventArgs e)
        {
            App.MainView.DeclArtcile.Licences.Add(new DeclArticle.LicenceElement());
        }
        private void OnClickDeleteLicence(object sender, RoutedEventArgs e)
        {
            if (App.MainView.DeclArtcile.Licences.Count == 0) return;
            else App.MainView.DeclArtcile.Licences.RemoveAt(App.MainView.DeclArtcile.Licences.Count - 1);
        }

        private void OnClickGenerateInputPaper(object sender, RoutedEventArgs e)
        {
            InputWindowViewModel.GenerateInput(sender, e);
        }
        private void OnClickLoadShpr(object sender, RoutedEventArgs e)
        {
            InputWindowViewModel.LoadShpr(sender, e);
        }

        private void OnClickForceAnbun(object sender, RoutedEventArgs e)
        {
            InputWindowViewModel.ForceAnbun(sender, e);
        }

    }

}