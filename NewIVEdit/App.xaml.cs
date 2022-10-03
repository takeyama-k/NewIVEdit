using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NewIVEdit
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public static MainWindow IVEditMainWindow;
        public static MainWindowViewModel MainView;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (null == System.Windows.Application.Current)
            {
                new System.Windows.Application();
            }

            IVEditMainWindow = new MainWindow();
            MainView = new MainWindowViewModel();
            IVEditMainWindow.DataContext = MainView;

            App.MainView.LoadSettings();
            //loading company's specific setting
            App.MainView.ResetCompanyProfiles();

            foreach(var tt in NewIVEdit.ConstValue.Tradeterm)
            {
                MainView.TradetermProfile.Add(new TradetermProfile() {IndexNo = int.MaxValue, Tradeterm = tt });
            }
            IVEditMainWindow.AnnotationCanvas.ItemsSource = MainView.CurrentAnnotaion;
            IVEditMainWindow.PageNavigation.ItemsSource = MainView.PageThumbneils;
            IVEditMainWindow.Show();
        }
    }

    
}
