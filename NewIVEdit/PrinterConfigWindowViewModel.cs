using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.IO;
using NewIVEdit;
public class PrinterConfigWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public bool IsValid = false;

    private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    private string _printerNameWarning = "";
    public string PrinterNameWarning
    {
        set
        {
            _printerNameWarning = value;
            NofityPropertyChanged("PrinterNameWarning");
        }
        get 
        {
            return _printerNameWarning;
        }
    }
    private string _printerName = "";
    public string PrinterName {
        set
        {
            _printerName = value;
            NofityPropertyChanged("PrinterName");
            if (ConstValue.Re_ContainsExNumberAlphabet.Match(_printerName).Success)
            {
                IsValid = false;
                PrinterNameWarning = "プリンタ名に英大文字　数字以外の文字を使わないでください";
                return;
            }
            IsValid = true;
            PrinterNameWarning = "";
        }
        get
        {
            return _printerName;
        }
    }
    public PrinterConfigWindowViewModel()
    {
        if(App.MainView.CurrentPrinterName == "")
        {
            PrinterName = PrinterConfigWindowViewModel.LoadPrinterSetting();
            if (IsValid) App.MainView.CurrentPrinterName = PrinterName;
        }
        else
        {
            PrinterName = App.MainView.CurrentPrinterName;
        }
    }

    public static string LoadPrinterSetting()
    {
        string path = Path.Combine(System.Windows.Forms.Application.StartupPath, ConstValue.PrinterSettingFile);
        StreamWriter f = null;
        if (!File.Exists(path))
        {
            try
            {
                f = File.CreateText(path);
                f.Close();
            }
            catch (Exception e)
            {
                if (f != null) f.Close();
                MessageBox.Show("プリンタ名の設定ファイルが開けません");
                return "";
            }
        }
        using (var stream = new StreamReader(path))
        {
            string printer = stream.ReadLine();
            return printer;
        }
    }

    public void Register(object sender, RoutedEventArgs e)
    {
        if (IsValid) App.MainView.CurrentPrinterName = PrinterName;
        else
        {
            MessageBox.Show("登録できません");
            return;
        }
        string path = Path.Combine(System.Windows.Forms.Application.StartupPath, ConstValue.PrinterSettingFile);
        StreamWriter f = null;
        if (!File.Exists(path))
        {
            try
            {
                f = File.CreateText(path);
                f.Close();
            }
            catch (Exception ex)
            {
                if (f != null) f.Close();
                MessageBox.Show("プリンタ名の設定ファイルを開いていないか確認してください。");
                return;
            }
        }
        using (var stream = new FileStream(path, FileMode.Open))
        {
            stream.SetLength(0);
        }
        using (var stream = new StreamWriter(path))
        {
            stream.WriteLine(PrinterName);
        }
    }
}