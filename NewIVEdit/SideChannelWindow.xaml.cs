using System;
using System.Windows;

namespace NewIVEdit
{
    public partial class SideChannelWindow : Window
    {
        public SideChannelWindow()
        {
            InitializeComponent();
        }
        private void OnClickCalcButton(object sender, RoutedEventArgs e)
        {
            if(SideChannelDataViewModel.SideChannelConverters == null || SideChannelDataViewModel.SideChannelConverters.Count == 0)
            {
                MessageBox.Show("サイドチャンネルデータの計算式が定義されていません");
                return;
            }
            SideChannelDataViewModel.Calc();
        }
    }
}