using System;
using System.Windows;

namespace NewIVEdit
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DocumentNoWindow : Window
    {
        public DocumentNoWindow()
        {
            InitializeComponent();
        }
        private void OnClickGenerateButton(object sender, RoutedEventArgs e)
        {
            if(InputWindowViewModel.GenerateInput(sender, e))
            {
                this.Close();
            }
        }
    }
    
}