using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using NewIVEdit;
public class ElasticConfigWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

	public string ElasticHostUri { set; get; } = @"http://localhost";
    public string ElasticHostPortNo { set; get; } = "9200";
    public ElasticConfigWindowViewModel()
	{
    }
    public void Register(object sender, RoutedEventArgs e)
    {
        Uri elastic = null;
        try
        {
            elastic = new Uri(ElasticHostUri + ":" + ElasticHostPortNo + @"/");
        }
        catch (Exception ex)
        {
            if (typeof(UriFormatException) == ex.GetType())
            {
                MessageBox.Show("Urlの形式が不正です");
                return;
            }
        }
        App.MainView.ElasticClient.ElasticServiceHost = elastic;
    }
}
