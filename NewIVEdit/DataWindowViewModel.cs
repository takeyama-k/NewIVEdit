using System;
using System.Windows;
using System.Windows.Forms;
using ExcelDataReader;
using System.IO;
using NewIVEdit;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text.RegularExpressions;
using OCVS = OpenCvSharp;
using System.Windows.Media.Imaging;

public class DataWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    private string _totalNetWeight = "";

    public string TotalNetWeight
    {
        set
        {
            _totalNetWeight = value;
            NotifyPropertyChanged("TotalNetWeight");
        }
        get
        {
            return _totalNetWeight;
        }
    }

    private string _totalQuantity = "";

    public string TotalQuantity
    {
        set
        {
            _totalQuantity = value;
            NotifyPropertyChanged("TotalQuantity");
        }
        get
        {
            return _totalQuantity;
        }
    }


    public bool IsFaceValueValid = false;
    private string _faceValueWarningMessage = "";
    public string FaceValueWarningMessage
    {
        set
        {
            _faceValueWarningMessage = value;
            NotifyPropertyChanged("FaceValueWarningMessage");
        }
        get
        {
            return _faceValueWarningMessage;
        }
    }
    private string _faceValue = "";
    public string FaceValue
    {
        set
        {
            _faceValue = value;
            NotifyPropertyChanged("FaceValue");
            double tempFace = 0.0;
            if (!double.TryParse(value, out tempFace))
            {
                FaceValueWarningMessage = "数字として読み取れません。";
                IsFaceValueValid = false;
                return;
            }
            if (tempFace < 0)
            {
                FaceValueWarningMessage = "マイナスの数字を入力しないでください";
                IsFaceValueValid = false;
                return;
            }
            _faceValue = string.Format("{0:0.000}", tempFace);
            _faceValue = _faceValue.Length > 1 ? _faceValue.Substring(0, _faceValue.Length - 1) : _faceValue;
            NotifyPropertyChanged("FaceValue");
            double tempFob = 0.0;
            if (App.MainView.CurrentTradeterm != null)
            {
                if (double.TryParse(FOBValue, out tempFob))
                {
                    if (App.MainView.CurrentTradeterm.Tradeterm == "FCA")
                    {
                        if ((long)(tempFob * ConstValue.InternalValueFactor) != (long)(tempFace * ConstValue.InternalValueFactor))
                        {
                            FaceValueWarningMessage = "FCA価格とFOB価格が異なる値です";
                            IsFaceValueValid = false;
                            return;
                        }
                    }
                    else if (App.MainView.CurrentTradeterm.Tradeterm == "EXW")
                    {
                        if ((long)(tempFob * ConstValue.InternalValueFactor) <= (long)(tempFace * ConstValue.InternalValueFactor))
                        {
                            FaceValueWarningMessage = "FOB価格がEXW価格以下です。";
                            IsFaceValueValid = false;
                            return;
                        }
                    }
                    else if (App.MainView.CurrentTradeterm.Tradeterm != "FOB")
                    {
                        if ((long)(tempFob * ConstValue.InternalValueFactor) >= (long)(tempFace * ConstValue.InternalValueFactor))
                        {
                            FaceValueWarningMessage = "FOB価格が" + App.MainView.CurrentTradeterm.Tradeterm + "価格以上です。";
                            IsFaceValueValid = false;
                            return;
                        }
                    }
                }
                FOBValueWarningMessage = "";
                IsFOBValueValid = true;
                FaceValueWarningMessage = "";
                IsFaceValueValid = true;
            }
            else
            {
                FaceValueWarningMessage = "トレードタームを選択してください";
                IsFaceValueValid = false;
            }

        }
        get
        {
            return _faceValue;
        }
    }

    public bool IsFOBValueValid = false;
    private string _fobValueWarningMessage = "";
    public string FOBValueWarningMessage
    {
        set
        {
            _fobValueWarningMessage = value;
            NotifyPropertyChanged("FOBValueWarningMessage");
        }
        get
        {
            return _fobValueWarningMessage;
        }
    }
    private string _fobValue = "";
    public string FOBValue
    {
        set
        {
            _fobValue = value;
            NotifyPropertyChanged("FOBValue");
            double tempFob = 0.0;
            if (!double.TryParse(value, out tempFob))
            {
                FOBValueWarningMessage = "数字として読み取れません。";
                IsFOBValueValid = false;
                return;
            }
            if (tempFob < 0)
            {
                FOBValueWarningMessage = "マイナスの数字を入力しないでください";
                IsFOBValueValid = false;
                return;
            }
            _fobValue = string.Format("{0:0.000}", tempFob);
            _fobValue = _fobValue.Length > 1 ? _fobValue.Substring(0, _fobValue.Length - 1) : _fobValue;
            NotifyPropertyChanged("FOBValue");
            double tempFace = 0.0;
            if (App.MainView.CurrentTradeterm != null)
            {
                if (double.TryParse(FaceValue, out tempFace))
                {
                    if (App.MainView.CurrentTradeterm.Tradeterm == "FCA")
                    {
                        if ((long)(tempFace * ConstValue.InternalValueFactor) != (long)(tempFob * ConstValue.InternalValueFactor))
                        {
                            FOBValueWarningMessage = "FCA価格とFOB価格が異なる値です";
                            IsFOBValueValid = false;
                            return;
                        }
                    }
                    else if (App.MainView.CurrentTradeterm.Tradeterm == "EXW")
                    {
                        if ((long)(tempFace * ConstValue.InternalValueFactor) >= (long)(tempFob * ConstValue.InternalValueFactor))
                        {
                            FOBValueWarningMessage = "FOB価格がEXW価格以下です。";
                            IsFOBValueValid = false;
                            return;
                        }
                    }
                    else if (App.MainView.CurrentTradeterm.Tradeterm != "FOB")
                    {
                        if ((long)(tempFace * ConstValue.InternalValueFactor) <= (long)(tempFob * ConstValue.InternalValueFactor))
                        {
                            FOBValueWarningMessage = "FOB価格が" + App.MainView.CurrentTradeterm.Tradeterm + "価格以上です。";
                            IsFOBValueValid = false;
                            return;
                        }
                    }
                }
                FOBValueWarningMessage = "";
                IsFOBValueValid = true;
                FaceValueWarningMessage = "";
                IsFaceValueValid = true;
            }
            FOBValueWarningMessage = "";
            IsFOBValueValid = true;
        }
        get
        {
            return _fobValue;
        }
    }

    private string _searchWord = "";
    public string SearchWord
    {
        set
        {
            _searchWord = value;
            if(App.MainView.SubWindowController.DataWindow != null)
            {
                App.MainView.SubWindowController.DataWindow.InvoiceNoCombobox.SelectedItem = App.MainView.InvoiceIndex.FindLast(new InvoiceIndex { Key = value });
            }
            NotifyPropertyChanged("SearchWord");
        }
        get
        {
            return _searchWord;
        }
    }
    public DataWindowViewModel()
	{
	}
	public void OpenExcelFile(bool isPackingList = false)
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filter = "xlsxファイル|*.xlsx|xlsmファイル|*.xlsm|xlsファイル|*.xls|csvファイル|*.csv";
        string filepath = "";

        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            filepath = dialog.FileName;
        }
        else
        {
            return;
        }
        var curSubtype = App.MainView.CurrentCompanyProfile;
        if (curSubtype == null) return;

        string existkey = "";
        string existsecondkey = "";
        string invnokey = "";
        string amtkey = "";
        string numberkey = "";
        string netwtkey = "";
        string orkey = "";
        string desckey = "";
        string hskey = "";
        string hssuggestkey = "";
        string curkey = "";
        if (isPackingList)
        {
            existkey = curSubtype.packing_existby ?? "";
            existsecondkey = curSubtype.packing_existby_secondary ?? "";
            invnokey = curSubtype.packing_invnoby ?? "";
            amtkey = curSubtype.packing_amtby ?? "";
            numberkey = curSubtype.packing_numberby ?? "";
            netwtkey = curSubtype.packing_netwtby ?? "";
            orkey = curSubtype.packing_orby ?? "";
            desckey = curSubtype.packing_descby ?? "";
            hskey = curSubtype.packing_hsby ?? "";
            hssuggestkey = curSubtype.packing_hssuggestby ?? "";
            curkey = curSubtype.packing_curby ?? "";
        }
        else
        {
            existkey = curSubtype.existby ?? "";
            existsecondkey = curSubtype.existby_secondary ?? "";
            invnokey = curSubtype.invnoby ?? "";
            amtkey = curSubtype.amtby ?? "";
            numberkey = curSubtype.numberby ?? "";
            netwtkey = curSubtype.netwtby ?? "";
            orkey = curSubtype.orby ?? "";
            desckey = curSubtype.descby ?? "";
            hskey = curSubtype.hsby ?? "";
            hssuggestkey = curSubtype.hssuggestby ?? "";
            curkey = curSubtype.curby ?? "";
        }

        var rawData = new Dictionary<string, string[]>();
        int ttlRows = 0;
        int ttlDataElems = 0;
        if (curSubtype.is_serialized)
        {
            try
            {
                using (var stream = File.Open(filepath, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader reader;
                    if (filepath.Substring(filepath.Length - 3, 3) == "xls" || filepath.Substring(filepath.Length - 4, 4) == "xlsx" || filepath.Substring(filepath.Length - 4, 4) == "xlsm")
                    {
                        using (reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            ttlRows = reader.RowCount;
                            IVEditElasticClient.DataCoord existDatacoord = null;
                            foreach (var coord in App.MainView.CurrentCompanyProfile.datacoord)
                            {
                                if (!coord.isexcel) continue;
                                if (existkey == coord.label)
                                {
                                    existDatacoord = coord;
                                    break;
                                }
                            }
                            if (existDatacoord == null) return;
                            int[] elemsBypage;
                            if (ttlRows > 0) elemsBypage = new int[ttlRows];
                            else return;
                            var existPattern = UtilityFunc.ParseRegexModoki(existDatacoord.pattern, existDatacoord.prefix_pattern, existDatacoord.postfix_pattern, existDatacoord.is_completematch, existDatacoord.fuzzy_level);
                            string existStartPatternStr = existDatacoord.start_pattern != null ? existDatacoord.start_pattern + "(?<nokori>.*)$" : "";
                            var existStartPattern = existDatacoord.start_pattern != null ? new Regex(existStartPatternStr, RegexOptions.Compiled) : null;
                            do
                            {
                                int rowNo = 0;
                                while (reader.Read())
                                {
                                    var data = reader.GetValue((int)existDatacoord.from);
                                    if (data == null)
                                    {
                                        elemsBypage[rowNo++] = 0;
                                        continue;
                                    }

                                    string sdata = data.ToString();
                                    if (existStartPattern != null)
                                    {
                                        sdata = existStartPattern.Match(data.ToString()).Groups["nokori"].Value;
                                        if (sdata == null)
                                        {
                                            elemsBypage[rowNo++] = 0;
                                            continue;
                                        }
                                    }
                                    var matches = existPattern.Matches(sdata);
                                    elemsBypage[rowNo++] = matches.Count;
                                    ttlDataElems += matches.Count;
                                }
                            } while (reader.NextResult());
                            reader.Reset();
                            if (ttlDataElems == 0) return;
                            foreach (var coord in App.MainView.CurrentCompanyProfile.datacoord)
                            {
                                if (!coord.isexcel) continue;
                                var fieldname = coord.label;
                                if (fieldname == "") continue;
                                if (rawData.ContainsKey(fieldname)) continue;
                                int colNo = (int)coord.from;
                                rawData.Add(fieldname, new string[ttlDataElems]);
                                var pattern = UtilityFunc.ParseRegexModoki(coord.pattern, coord.prefix_pattern, coord.postfix_pattern, coord.is_completematch, coord.fuzzy_level);
                                string startPatternStr = coord.start_pattern != null ? coord.start_pattern + "(?<nokori>.*)$" : "";
                                var startPattern = coord.start_pattern != null ? UtilityFunc.ParseRegexModoki(startPatternStr, null, null, false, 0) : null;
                                do
                                {
                                    int rowNo = 0;
                                    int elemNo = 0;

                                    while (reader.Read())
                                    {
                                        if (reader.FieldCount - 1 < colNo) continue;
                                        var data = reader.GetValue(colNo);
                                        if (data == null)
                                        {
                                            elemNo += elemsBypage[rowNo++];
                                            continue;
                                        }

                                        if (data != null)
                                        {
                                            string sdata = data.ToString();
                                            if (existStartPattern != null)
                                            {
                                                sdata = startPattern.Match(data.ToString()).Groups["nokori"].Value;
                                                if (sdata == null)
                                                {
                                                    elemNo += elemsBypage[rowNo++] = 0;
                                                    continue;
                                                }
                                            }
                                            var matches = pattern.Matches(sdata);
                                            for (int i = 0; i < elemsBypage[rowNo]; i++)
                                            {
                                                var match = i < matches.Count ? matches[i] : null;
                                                rawData[fieldname][elemNo + i] = match != null ? match.Groups[1].Value : "";
                                            }
                                            elemNo += elemsBypage[rowNo];
                                        }
                                        rowNo++;
                                    }
                                } while (reader.NextResult());
                                reader.Reset();
                            }
                        }
                    }
                    else if (filepath.Substring(filepath.Length - 3, 3) == "csv")
                    {
                        using (reader = ExcelReaderFactory.CreateCsvReader(stream))
                        {
                            ttlRows = reader.RowCount;
                            IVEditElasticClient.DataCoord existDatacoord = null;
                            foreach (var coord in App.MainView.CurrentCompanyProfile.datacoord)
                            {
                                if (!coord.isexcel) continue;
                                if (existkey == coord.label)
                                {
                                    existDatacoord = coord;
                                    break;
                                }
                            }
                            if (existDatacoord == null) return;
                            int[] elemsBypage;
                            if (ttlRows > 0) elemsBypage = new int[ttlRows];
                            else return;
                            var existPattern = UtilityFunc.ParseRegexModoki(existDatacoord.pattern, existDatacoord.prefix_pattern, existDatacoord.postfix_pattern, existDatacoord.is_completematch, existDatacoord.fuzzy_level);
                            string existStartPatternStr = existDatacoord.start_pattern != null ? existDatacoord.start_pattern + "(?<nokori>.*)$" : "";
                            var existStartPattern = existDatacoord.start_pattern != null ? new Regex(existStartPatternStr, RegexOptions.Compiled) : null;
                            do
                            {
                                int rowNo = 0;
                                while (reader.Read())
                                {
                                    var data = reader.GetValue((int)existDatacoord.from);
                                    if (data == null)
                                    {
                                        elemsBypage[rowNo++] = 0;
                                        continue;
                                    }

                                    string sdata = data.ToString();
                                    if (existStartPattern != null)
                                    {
                                        sdata = existStartPattern.Match(data.ToString()).Groups["nokori"].Value;
                                        if (sdata == null)
                                        {
                                            elemsBypage[rowNo++] = 0;
                                            continue;
                                        }
                                    }
                                    var matches = existPattern.Matches(sdata);
                                    elemsBypage[rowNo++] = matches.Count;
                                    ttlDataElems += matches.Count;
                                }
                            } while (reader.NextResult());
                            reader.Reset();
                            if (ttlDataElems == 0) return;
                            foreach (var coord in App.MainView.CurrentCompanyProfile.datacoord)
                            {
                                if (!coord.isexcel) continue;
                                var fieldname = coord.label;
                                if (fieldname == "") continue;
                                if (rawData.ContainsKey(fieldname)) continue;
                                int colNo = (int)coord.from;
                                rawData.Add(fieldname, new string[ttlDataElems]);
                                var pattern = UtilityFunc.ParseRegexModoki(coord.pattern, coord.prefix_pattern, coord.postfix_pattern, coord.is_completematch, coord.fuzzy_level);
                                string startPatternStr = coord.start_pattern != null ? coord.start_pattern + @"(?<nokori>.*)$" : "";
                                var startPattern = coord.start_pattern != null ? new Regex(startPatternStr, RegexOptions.Compiled) : null;
                                do
                                {
                                    int rowNo = 0;
                                    int elemNo = 0;
                                    while (reader.Read())
                                    {
                                        if (reader.FieldCount - 1 < colNo) continue;
                                        var data = reader.GetValue(colNo);
                                        if (data == null)
                                        {
                                            elemNo += elemsBypage[rowNo++];
                                            continue;
                                        }

                                        if (data != null)
                                        {
                                            string sdata = data.ToString();
                                            if (startPattern != null)
                                            {
                                                sdata = startPattern.Match(data.ToString()).Groups["nokori"].Value;
                                                if (sdata == null)
                                                {
                                                    elemNo += elemsBypage[rowNo++] = 0;
                                                    continue;
                                                }
                                            }
                                            var matches = pattern.Matches(sdata);
                                            for (int i = 0; i < elemsBypage[rowNo]; i++)
                                            {
                                                var match = i < matches.Count ? matches[i] : null;
                                                rawData[fieldname][elemNo + i] = match != null ? match.Groups[1].Value : "";
                                            }
                                            elemNo += elemsBypage[rowNo];
                                        }
                                        rowNo++;
                                    }
                                } while (reader.NextResult());
                                reader.Reset();
                            }
                        }
                    }
                }
            }catch(IOException ioe)
            {
                System.Windows.MessageBox.Show("ファイルがほかのアプリにより開かれています。");
                return;
            }
        }
        else
        {
            try
            {
                using (var stream = File.Open(filepath, FileMode.Open, FileAccess.Read))
                {
                    if (filepath.Substring(filepath.Length - 3, 3) == "xls" || filepath.Substring(filepath.Length - 3, 3) == "XLS" || filepath.Substring(filepath.Length - 4, 4) == "xlsx" || filepath.Substring(filepath.Length - 4, 4) == "XLSX" || filepath.Substring(filepath.Length - 4, 4) == "xlsm" || filepath.Substring(filepath.Length - 4, 4) == "XLSM")
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet(
                                    new ExcelDataSetConfiguration()
                                    {
                                        UseColumnDataType = true,
                                    }
                                );
                            for (int tableIndex = 0; tableIndex < (curSubtype.multiple_excel_sheets ? result.Tables.Count : 1); tableIndex++)
                            {
                                ttlRows += result.Tables[tableIndex].Rows.Count;
                            }
                            foreach (var coord in App.MainView.CurrentCompanyProfile.datacoord)
                            {
                                int rowNo = 0;
                                if (!coord.isexcel) continue;
                                var fieldname = coord.label;
                                if (fieldname == "") continue;
                                if (rawData.ContainsKey(fieldname)) continue;
                                int colNo = (int)coord.from;
                                rawData.Add(fieldname, new string[ttlRows]);
                                int offset = coord.offset;
                                for (int tableIndex = 0; tableIndex < (curSubtype.multiple_excel_sheets ? result.Tables.Count : 1); tableIndex++)
                                {
                                    var table = result.Tables[tableIndex];
                                    foreach (System.Data.DataRow row in table.Rows)
                                    {
                                        if (offset > 0)
                                        {
                                            offset--;
                                            continue;
                                        }
                                        string data = "";
                                        if (row.ItemArray.Length > colNo)
                                        {
                                            data = row.ItemArray[colNo].ToString();
                                        }

                                        rawData[fieldname][rowNo++] = data;
                                    }
                                }
                            }

                        }
                    }
                    else if (filepath.Substring(filepath.Length - 3, 3) == "csv" || filepath.Substring(filepath.Length - 3, 3) == "CSV")
                    {
                        using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
                        {
                            ttlRows = reader.RowCount;
                            foreach (var coord in App.MainView.CurrentCompanyProfile.datacoord)
                            {
                                if (!coord.isexcel) continue;
                                var fieldname = coord.label;
                                if (fieldname == "") continue;
                                if (rawData.ContainsKey(fieldname)) continue;
                                int colNo = (int)coord.from;
                                rawData.Add(fieldname, new string[ttlRows]);
                                do
                                {
                                    int rowNo = 0;
                                    while (reader.Read())
                                    {
                                        if (reader.FieldCount - 1 < colNo) continue;
                                        var data = reader.GetValue(colNo);
                                        if (data != null)
                                        {
                                            rawData[fieldname][rowNo] = data.ToString();
                                        }
                                        else
                                        {
                                            rawData[fieldname][rowNo] = "";
                                        }
                                        rowNo++;
                                    }
                                } while (reader.NextResult());
                                reader.Reset();
                            }
                        }
                    }
                }
            }catch(IOException e)
            {
                System.Windows.MessageBox.Show("ファイルがほかのアプリにより開かれています。");
                return;
            }
        }
        if (curSubtype.enable_knockout)
        {
            var tempRaw = new Dictionary<string, string[]>();
            foreach (var key in rawData.Keys)
            {
                tempRaw.Add(key, Enumerable.Repeat("", curSubtype.is_serialized ? ttlDataElems : ttlRows).ToArray());
            }
            Regex berfore_pattern = null;
            Regex after_pattern = null;
            RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase;
            if (curSubtype.knockout_before != null) berfore_pattern = new Regex(curSubtype.knockout_before,options);
            if (curSubtype.knockout_after != null) after_pattern = new Regex(curSubtype.knockout_after, options);

            int dstidx = 0;
            bool valid = false;
            for(int srcidx = 0; srcidx < (curSubtype.is_serialized ? ttlDataElems : ttlRows); srcidx++)
            {
                
                string text = "";
                foreach(var key in rawData.Keys)
                {
                    text += rawData[key][srcidx] ?? "";
                }
                if (valid == false && berfore_pattern != null && berfore_pattern.IsMatch(text))
                {
                    valid = true;
                    continue;
                }
                else if (valid == true && after_pattern != null && after_pattern.IsMatch(text)) valid = false;
                if(valid == true)
                {
                    foreach (var key in rawData.Keys)
                    {
                        tempRaw[key][dstidx] = rawData[key][srcidx] ?? "";
                    }
                    dstidx++;
                }
                
            }
            if (curSubtype.is_serialized) ttlDataElems = dstidx;
            else ttlRows = dstidx;
            rawData = tempRaw;
        }

        int rowsCount = curSubtype.is_serialized ? ttlDataElems : ttlRows;

        for(int i = 0; i < rowsCount; i++)
        {
            var newData = new InvoiceRawData()
            {
                Exist = rawData.ContainsKey(existkey) ? rawData[existkey][i] : "",
                ExistSecondary = rawData.ContainsKey(existsecondkey) ? rawData[existsecondkey][i] : "",
                InvoiceNo = rawData.ContainsKey(invnokey) ? rawData[invnokey][i] : App.MainView.CurrentInvoiceNoCntr.ToString(),
                Amount = rawData.ContainsKey(amtkey) ? rawData[amtkey][i] : "",
                Number = rawData.ContainsKey(numberkey) ? rawData[numberkey][i] : "",
                NetWeight = rawData.ContainsKey(netwtkey) ? rawData[netwtkey][i] : "",
                Origin = rawData.ContainsKey(orkey) ? rawData[orkey][i] : "",
                Description = rawData.ContainsKey(desckey) ? rawData[desckey][i] : "",
                HSCodeKey = rawData.ContainsKey(hskey) ? rawData[hskey][i] : "",
                HSCodeClue = rawData.ContainsKey(hssuggestkey) ? rawData[hssuggestkey][i] : "",
                Currency = rawData.ContainsKey(curkey) ? rawData[curkey][i] : "",
                IndexNo = int.MaxValue,
            };
            if (isPackingList) App.MainView.PackingListRawData.Add(newData);
            else App.MainView.InvoiceRawData.Add(newData);
            
        }
        if (isPackingList)
        {
            App.MainView.PackingListIndex.Clear();
            if (App.MainView.PackingListRawData.Count == 0) return;
            App.MainView.PackingListIndex.Add(
                new InvoiceIndex()
                {
                    Index = -1,
                    Key = "",
                    IsAny = true,
                }
                );
            App.MainView.PackingListIndex.Add(
                new InvoiceIndex()
                {
                    Index = 0,
                    Key = App.MainView.PackingListRawData[0].InvoiceNo,
                }
                );
            for (int i = 1; i < App.MainView.PackingListRawData.Count; i++)
            {
                if (String.Compare(App.MainView.PackingListRawData[i].InvoiceNo, App.MainView.PackingListRawData[i - 1].InvoiceNo) != 0)
                {
                    App.MainView.PackingListIndex.Add(
                        new InvoiceIndex()
                        {
                            Index = App.MainView.PackingListRawData[i].IndexNo,
                            Key = App.MainView.PackingListRawData[i].InvoiceNo,
                        }
                    );
                }
            }
        }
        else
        {
            App.MainView.InvoiceIndex.Clear();
            if (App.MainView.InvoiceRawData.Count == 0) return;
            App.MainView.InvoiceIndex.Add(
                new InvoiceIndex()
                {
                    Index = -1,
                    Key = "",
                    IsAny = true,
                }
                );
            App.MainView.InvoiceIndex.Add(
                new InvoiceIndex()
                {
                    Index = 0,
                    Key = App.MainView.InvoiceRawData[0].InvoiceNo,
                }
                );
            for (int i = 1; i < App.MainView.InvoiceRawData.Count; i++)
            {
                if (String.Compare(App.MainView.InvoiceRawData[i].InvoiceNo, App.MainView.InvoiceRawData[i - 1].InvoiceNo) != 0)
                {
                    App.MainView.InvoiceIndex.Add(
                        new InvoiceIndex()
                        {
                            Index = App.MainView.InvoiceRawData[i].IndexNo,
                            Key = App.MainView.InvoiceRawData[i].InvoiceNo,
                        }
                    );
                }
            }
        }

    }

    public void ReadDataFromPdf(bool isPackingList = false)
    {
        
        var curSubtype = App.MainView.CurrentCompanyProfile;
        if (curSubtype == null) return;
        //
        string[] dataColumnKeys = null;
        if (isPackingList)
        {
            dataColumnKeys = new string[9]
            {
            curSubtype.packing_existby ?? "",
            curSubtype.packing_existby_secondary ?? "",
            curSubtype.packing_invnoby ?? "",
            curSubtype.packing_numberby ?? "",
            curSubtype.packing_netwtby ?? "",
            curSubtype.packing_orby ?? "",
            curSubtype.packing_descby ?? "",
            curSubtype.packing_hsby ?? "",
            curSubtype.packing_hssuggestby ?? ""
            };
        }
        else
        {
            dataColumnKeys = new string[11]
            {
            curSubtype.existby ?? "",
            curSubtype.existby_secondary ?? "",
            curSubtype.invnoby ?? "",
            curSubtype.amtby ?? "",
            curSubtype.numberby ?? "",
            curSubtype.netwtby ?? "",
            curSubtype.orby ?? "",
            curSubtype.descby ?? "",
            curSubtype.hsby ?? "",
            curSubtype.hssuggestby ?? "",
            curSubtype.curby ?? "",
            };
        }

        PdfLoader p = new PdfLoader();
        if (curSubtype.clue_profiles != null && curSubtype.clue_profiles.Length != 0)
        {
            foreach (var clueProf in curSubtype.clue_profiles) {
                if (clueProf.is_excel) continue;
                if (clueProf.label == null || clueProf.mode == null || clueProf.pattern == null) continue;
                if (!App.MainView.OcrClue.ContainsKey(clueProf.label))
                {
                    App.MainView.OcrClue.Add(clueProf.label, new ObservableSortedCollection<OcrClueConatiner>());
                }
                Regex pattern = UtilityFunc.ParseRegexModoki(clueProf.pattern,clueProf.prefix_pattern,clueProf.postfix_pattern,clueProf.is_completematch, clueProf.fuzzy_level,clueProf.is_excel);
                foreach (var page in App.MainView.Pages)
                {
                    if (page.IsGenerated || page.IsInputPaper) continue;
                    string filename = page.OriginFile;
                    int pageNum = page.OriginPage;
                    if (!File.Exists(filename)) continue;
                    var text = p.GetTextWithinBound(filename, clueProf.x, clueProf.y, clueProf.w, clueProf.h, pageNum);
                    text = ConstValue.Re_Delete_period.Replace(text, "");
                    text = ConstValue.Re_Delete_comma.Replace(text, "");
                    string res = "";
                    if(pattern != null)
                    {
                        res = pattern.Match(text).Groups[1].Value ?? "";
                    }
                    App.MainView.OcrClue[clueProf.label].Add(new OcrClueConatiner()
                    {
                       Page = page.PageNo,
                       Value = res,
                    });
                }
            }
        }

        var isReOcr = new Dictionary<string, bool>();
        var readMode = new Dictionary<string, string>();
        var readRange = new Dictionary<string, Tuple<float,float>>();
        var rawData = new Dictionary<string, List<string>>();
        var dataCoord = new Dictionary<string, List<Tuple<int,Point>>>();
        var regexPattern = new Dictionary<string, Regex>();
        var searchRegexPattern = new Dictionary<string, Regex>();


        foreach (var coord in curSubtype.datacoord)
        {

            var fieldname = coord.label;
            if (fieldname == "") continue;
            if (rawData.ContainsKey(fieldname)) continue;
            if (coord.isexcel)
            {
                readMode.Add(fieldname, "");
                continue;
            }
            if (coord.mode == null) 
            {
                readMode.Add(fieldname, "");
                continue;
            }
            if (coord.mode == "FROM_CLUE" && coord.ref_clue == null) 
            {
                readMode.Add(fieldname, "");
                continue;
            }
            isReOcr.Add(fieldname, coord.re_ocr);
            readMode.Add(fieldname, coord.mode);
            readRange.Add(fieldname, new Tuple<float, float>(coord.from,coord.to));
            rawData.Add(fieldname, new List<string>());
            dataCoord.Add(fieldname, new List<Tuple<int,Point>>());
            regexPattern.Add(fieldname, UtilityFunc.ParseRegexModoki(coord.pattern, coord.prefix_pattern, coord.postfix_pattern, coord.is_completematch, coord.fuzzy_level,coord.isexcel));
            searchRegexPattern.Add(fieldname, UtilityFunc.ParseRegexModoki(coord.pattern, coord.prefix_pattern, coord.postfix_pattern, coord.is_completematch, coord.fuzzy_level, true));
        }


        if (dataColumnKeys[0] == "" || !rawData.ContainsKey(dataColumnKeys[0])) return;
        if (App.MainView.Pages == null || App.MainView.Pages.Count == 0 || regexPattern[dataColumnKeys[0]] == null) return;
        foreach (var page in App.MainView.Pages)
        {
            if (page.IsGenerated || page.IsInputPaper) continue;
            List<InvoiceOcrDataContainer> existOcrrawdata = readOcrData(p, searchRegexPattern[dataColumnKeys[0]],regexPattern[dataColumnKeys[0]], page.OriginFile, page.OriginPage, readRange[dataColumnKeys[0]].Item1, readRange[dataColumnKeys[0]].Item2, 0, 847);
            
            if (existOcrrawdata == null || existOcrrawdata.Count == 0) continue;
            calcTilt(ref existOcrrawdata, page);

            List<InvoiceOcrDataContainer> existsecondOcrData = new List<InvoiceOcrDataContainer>();
            if (dataColumnKeys[1] != "" && rawData.ContainsKey(dataColumnKeys[1])) {
                existsecondOcrData = readOcrData(p, searchRegexPattern[dataColumnKeys[1]], regexPattern[dataColumnKeys[1]], page.OriginFile, page.OriginPage, readRange[dataColumnKeys[1]].Item1, readRange[dataColumnKeys[1]].Item2, 0, 847)
                                        ?? new List<InvoiceOcrDataContainer>();
            }
            deleteSameData(ref existOcrrawdata);
            deleteSameData(ref existsecondOcrData);
            if(existOcrrawdata.Count > 0 && existsecondOcrData.Count > 0)
            {
                bool[] isOk = fastenOcrData(existOcrrawdata, existsecondOcrData).Item1;
                for(int idx = existOcrrawdata.Count-1;idx >=0; idx--)
                {
                    if (!isOk[idx]) existOcrrawdata.RemoveAt(idx);
                }
            }
            else if(existsecondOcrData.Count > 0)
            {
                continue;
            }
            var tempIVData = new List<InvoiceRawData>(existOcrrawdata.Count);
            var tempPLData = new List<InvoiceRawData>(existOcrrawdata.Count);
            //
            if (isPackingList)
            {
                foreach (var ocrrawData in existOcrrawdata)
                {
                    tempPLData.Add(new InvoiceRawData()
                    {
                        Exist = ocrrawData.Text,
                        ExistSecondary = "",
                        InvoiceNo = "",
                        Amount = "",
                        Number = "",
                        NetWeight = "",
                        Origin = "",
                        Currency = "",
                        Description = "",
                        HSCodeKey = "",
                        HSCodeClue = "",
                    });
                }
            }
            else
            {
                foreach (var ocrrawData in existOcrrawdata)
                {
                    tempIVData.Add(new InvoiceRawData()
                    {
                        Exist = ocrrawData.Text,
                        ExistSecondary = "",
                        InvoiceNo = "",
                        Amount = "",
                        Number = "",
                        NetWeight = "",
                        Origin = "",
                        Currency = "",
                        Description = "",
                        HSCodeKey = "",
                        HSCodeClue = "",
                    });
                }
            }
            //
            for(int i = 1; i < dataColumnKeys.Length; i++)
            {
                if (!readMode.ContainsKey(dataColumnKeys[i]) || readMode[dataColumnKeys[i]] == "" || readMode[dataColumnKeys[i]] == "FROM_CLUE") continue;
                var secondOcrData = readOcrData(p, searchRegexPattern[dataColumnKeys[i]], regexPattern[dataColumnKeys[i]], page.OriginFile, page.OriginPage, readRange[dataColumnKeys[i]].Item1, readRange[dataColumnKeys[i]].Item2, 0, 847);
                if (secondOcrData == null || secondOcrData.Count == 0) continue;
                deleteSameData(ref secondOcrData);
                var isOK = fastenOcrData(secondOcrData, existOcrrawdata, true).Item1;
                if (isReOcr[dataColumnKeys[i]])
                {
                    reOcr(ref secondOcrData, page, regexPattern[dataColumnKeys[i]], isOK);
                }
                //
                if (isPackingList)
                {
                    mergePackingListData(ref tempPLData, existOcrrawdata, secondOcrData, i);
                }
                else
                {
                    mergeInvoiceData(ref tempIVData, existOcrrawdata, secondOcrData, i);
                }
                //
            }
            if (isPackingList)
            {
                if (tempPLData != null && tempPLData.Count > 0)
                {
                    foreach (var plraw in tempPLData)
                    {
                        plraw.InvoiceNo = App.MainView.CurrentInvoiceNoCntr.ToString();
                        plraw.IndexNo = int.MaxValue;
                        App.MainView.PackingListRawData.Add(plraw);
                    }
                }
            }
            else
            {
                if (tempIVData != null && tempIVData.Count > 0)
                {
                    foreach (var ivraw in tempIVData)
                    {
                        ivraw.InvoiceNo = App.MainView.CurrentInvoiceNoCntr.ToString();
                        ivraw.IndexNo = int.MaxValue;
                        App.MainView.InvoiceRawData.Add(ivraw);
                    }
                }
            }
            //
        }
        //
        if (isPackingList)
        {
            App.MainView.PackingListIndex.Clear();
            if (App.MainView.PackingListRawData.Count == 0) return;
            App.MainView.PackingListIndex.Add(
                new InvoiceIndex()
                {
                    Index = -1,
                    Key = "",
                    IsAny = true,
                }
                );
            App.MainView.PackingListIndex.Add(
                new InvoiceIndex()
                {
                    Index = 0,
                    Key = App.MainView.PackingListRawData[0].InvoiceNo,
                }
                );
            for (int i = 1; i < App.MainView.PackingListRawData.Count; i++)
            {
                if (String.Compare(App.MainView.PackingListRawData[i].InvoiceNo, App.MainView.PackingListRawData[i - 1].InvoiceNo) != 0)
                {
                    App.MainView.PackingListIndex.Add(
                        new InvoiceIndex()
                        {
                            Index = App.MainView.PackingListRawData[i].IndexNo,
                            Key = App.MainView.PackingListRawData[i].InvoiceNo,
                        }
                    );
                }
            }
        }
        else
        {
            App.MainView.InvoiceIndex.Clear();
            if (App.MainView.InvoiceRawData.Count == 0) return;
            App.MainView.InvoiceIndex.Add(
                new InvoiceIndex()
                {
                    Index = -1,
                    Key = "",
                    IsAny = true,
                }
                );
            App.MainView.InvoiceIndex.Add(
                new InvoiceIndex()
                {
                    Index = 0,
                    Key = App.MainView.InvoiceRawData[0].InvoiceNo,
                }
                );
            for (int i = 1; i < App.MainView.InvoiceRawData.Count; i++)
            {
                if (String.Compare(App.MainView.InvoiceRawData[i].InvoiceNo, App.MainView.InvoiceRawData[i - 1].InvoiceNo) != 0)
                {
                    App.MainView.InvoiceIndex.Add(
                        new InvoiceIndex()
                        {
                            Index = App.MainView.InvoiceRawData[i].IndexNo,
                            Key = App.MainView.InvoiceRawData[i].InvoiceNo,
                        }
                    );
                }
            }
        }
        //
    }

    private List<InvoiceOcrDataContainer> readOcrData(PdfLoader p,Regex searchRegex, Regex extRegex, string filepath,int pageNum,float rangeXfrom, float rangeXto, float rangeYfrom, float rangeYto)
    {
        if (!File.Exists(filepath)) return null;
        float width = rangeXto - rangeXfrom;
        var text = p.GetTextWithinBound(filepath, rangeXfrom, rangeYfrom, width, rangeYto - rangeYfrom, pageNum);
        var searchMatches = searchRegex.Matches(text);
        var matches = extRegex.Matches(text);
        if (searchMatches.Count == 0) return null;
        string[] searchPatterns = new string[searchMatches.Count];
        string[] matchedTexts = new string[matches.Count];
        int idx = 0;
        foreach (Match m in searchMatches)
        {
            searchPatterns[idx++] = m.Groups[1].Value;
        }
        idx = 0;
        foreach (Match m in matches)
        {
            matchedTexts[idx++] = m.Groups[0].Value;
        }
        return p.SearchLocsByText(filepath, pageNum, searchPatterns,matchedTexts, rangeYto, rangeYfrom, rangeXfrom, rangeXto);
    }
    private void deleteSameData(ref List<InvoiceOcrDataContainer> ocrData)
    {
        if (ocrData.Count > 0)
        {
            int idx = ocrData.Count - 1;
            for (; idx > 0; idx--)
            {
                if (ocrData[idx].CompareTo(ocrData[idx - 1]) == 0)
                {
                    ocrData.RemoveAt(idx);
                }
            }
        }
    }


    private void calcTilt(ref List<InvoiceOcrDataContainer> ocrData, Page page)
    {
        foreach (var data in ocrData)
        {
            int textStartX = Math.Min(page.Width -1, (int)data.Coord.X - FundamentalSettings.OcrMarginPx);
            textStartX = textStartX < 0 ? 0 : textStartX;
            int textStartY = Math.Min(page.Height-1 , (int)data.Coord.Y - FundamentalSettings.OcrMarginPx);
            textStartY = textStartY < 0 ? 0 : textStartY;
            int textWidth = Math.Min(page.Width - ((int)data.Coord.X + FundamentalSettings.OcrMarginPx * 2 + 1), (int)data.Width + FundamentalSettings.OcrMarginPx * 2);
            textWidth = textWidth < 1 ? 1 : textWidth;
            int textHeight = Math.Min(page.Height - ((int)data.Coord.Y + FundamentalSettings.OcrMarginPx * 2 + 1), (int)data.Height + FundamentalSettings.OcrMarginPx * 2);
            textHeight = textHeight < 1 ? 1 : textHeight;
            var target = new CroppedBitmap(page.BackgroundImage, new Int32Rect(textStartX, textStartY, textWidth, textHeight));
            var mat = OCVS.WpfExtensions.BitmapSourceConverter.ToMat(target);
            var tilt = UtilityFunc.CalcTilt(mat);
            if (tilt > 45) tilt -= 90;
            data.Tilt = tilt;
        }
    }

    private void reOcr(ref List<InvoiceOcrDataContainer> ocrData, Page page, Regex pattern, bool[] isOK = null)
    {
        int idx = 0;
        foreach (var data in ocrData)
        {
            if (isOK != null && ocrData.Count == isOK.Length && !isOK[idx])
            {
                idx++;
                continue;
            }
            int textStartX = Math.Max(0, (int)data.Coord.X - FundamentalSettings.OcrMarginPx);
            int textStartY = Math.Max(0, (int)data.Coord.Y - FundamentalSettings.OcrMarginPx);
            int textWidth = Math.Max(1, Math.Min(page.Width - ((int)data.Width + FundamentalSettings.OcrMarginPx * 2), ((int)data.Width + FundamentalSettings.OcrMarginPx * 2)));
            int textHeight = Math.Max(1, Math.Min(page.Height - ((int)data.Height + FundamentalSettings.OcrMarginPx * 2), ((int)data.Height + FundamentalSettings.OcrMarginPx * 2)));

            var target = new CroppedBitmap(page.BackgroundImage, new Int32Rect(textStartX, textStartY, textWidth, textHeight));
            var mat = OCVS.WpfExtensions.BitmapSourceConverter.ToMat(target);
            var range = UtilityFunc.DetectRange(mat);
            int trueTextStartX = textStartX + (int)range.Item1.X;
            int trueTextStartY = textStartY + (int)range.Item1.Y;
            int trueWidth = range.Item2.Item1;
            int trueHeight = range.Item2.Item2;

            trueTextStartX = Math.Max(0, trueTextStartX - FundamentalSettings.ReOcrMarginPx);
            trueTextStartY = Math.Max(0, trueTextStartY - FundamentalSettings.ReOcrMarginPx);
            trueWidth = Math.Max(1, Math.Min(page.Width - (trueWidth + FundamentalSettings.ReOcrMarginPx * 2), (trueWidth + FundamentalSettings.ReOcrMarginPx * 2)));
            trueHeight = Math.Max(1, Math.Min(page.Height - (trueHeight + FundamentalSettings.ReOcrMarginPx * 2), (trueHeight + FundamentalSettings.ReOcrMarginPx * 2)));
            target = new CroppedBitmap(page.BackgroundImage, new Int32Rect(trueTextStartX, trueTextStartY, trueWidth, trueHeight));
            using (var targetBitmap = UtilityFunc.BitmapSourceToBitmap(target))
            {
                string text = NewIVEdit.Tess.ProcessBitmap(targetBitmap);
                text = ConstValue.Re_Delete_space.Replace(text, "");
                if (pattern.IsMatch(text))
                {
                    data.Text = pattern.Match(text).Groups[1].Value;
                }
            }
            idx++;
        }
    }

    private void mergeInvoiceData(ref List<InvoiceRawData> ivData, List<InvoiceOcrDataContainer> primeOcrData, List<InvoiceOcrDataContainer> secondOcrData, int colmnIdx)
    {
        if (ivData == null || primeOcrData == null || secondOcrData == null) return;
        if (ivData.Count != primeOcrData.Count) return;
        if (colmnIdx < 0) return;
        bool[] isOk = fastenOcrData(primeOcrData, secondOcrData).Item1;
        int[] indecies = fastenOcrData(primeOcrData, secondOcrData).Item2;
        
        for(int idx = primeOcrData.Count-1; idx >= 0; idx--)
        {
            if (isOk[idx])
            {
                switch (colmnIdx) {
                    case 0:
                        ivData[idx].Exist = secondOcrData[indecies[idx]].Text;
                        break;
                    case 1:
                        ivData[idx].ExistSecondary = secondOcrData[indecies[idx]].Text;
                        break;
                    case 2:
                        ivData[idx].InvoiceNo = secondOcrData[indecies[idx]].Text;
                        break;
                    case 3:
                        ivData[idx].Amount = secondOcrData[indecies[idx]].Text;
                        break;
                    case 4:
                        ivData[idx].Number = secondOcrData[indecies[idx]].Text;
                        break;
                    case 5:
                        ivData[idx].NetWeight = secondOcrData[indecies[idx]].Text;
                        break;
                    case 6:
                        ivData[idx].Origin = secondOcrData[indecies[idx]].Text;
                        break;
                    case 7:
                        ivData[idx].Description = secondOcrData[indecies[idx]].Text;
                        break;
                    case 8:
                        ivData[idx].HSCodeKey = secondOcrData[indecies[idx]].Text;
                        break;
                    case 9:
                        ivData[idx].HSCodeClue = secondOcrData[indecies[idx]].Text;
                        break;
                    case 10:
                        ivData[idx].Currency = secondOcrData[indecies[idx]].Text;
                        break;
                    default:
                        break;

                }
            }
        }
    }

    private void mergePackingListData(ref List<InvoiceRawData> plData, List<InvoiceOcrDataContainer> primeOcrData, List<InvoiceOcrDataContainer> secondOcrData, int colmnIdx)
    {
        if (plData == null || primeOcrData == null || secondOcrData == null) return;
        if (plData.Count != primeOcrData.Count) return;
        if (colmnIdx < 0) return;
        bool[] isOk = fastenOcrData(primeOcrData, secondOcrData).Item1;
        int[] indecies = fastenOcrData(primeOcrData, secondOcrData).Item2;

        for (int idx = primeOcrData.Count - 1; idx >= 0; idx--)
        {
            if (isOk[idx])
            {
                switch (colmnIdx)
                {
                    case 0:
                        plData[idx].Exist = secondOcrData[indecies[idx]].Text;
                        break;
                    case 1:
                        plData[idx].ExistSecondary = secondOcrData[indecies[idx]].Text;
                        break;
                    case 2:
                        plData[idx].InvoiceNo = secondOcrData[indecies[idx]].Text;
                        break;
                    case 3:
                        plData[idx].Number = secondOcrData[indecies[idx]].Text;
                        break;
                    case 4:
                        plData[idx].NetWeight = secondOcrData[indecies[idx]].Text;
                        break;
                    case 5:
                        plData[idx].Origin = secondOcrData[indecies[idx]].Text;
                        break;
                    case 6:
                        plData[idx].Description = secondOcrData[indecies[idx]].Text;
                        break;
                    case 7:
                        plData[idx].HSCodeKey = secondOcrData[indecies[idx]].Text;
                        break;
                    case 8:
                        plData[idx].HSCodeClue = secondOcrData[indecies[idx]].Text;
                        break;
                    default:
                        break;

                }
            }
        }
    }
    private Tuple<bool[], int[]> fastenOcrData(List<InvoiceOcrDataContainer> primeOcrData, List<InvoiceOcrDataContainer> secondOcrData, bool isOuter = false) {
        bool[] isOk = Enumerable.Repeat(false, primeOcrData.Count).ToArray();
        int[] indecies = Enumerable.Repeat(-1, primeOcrData.Count).ToArray();
        
        //2本指のアルゴリズム
        //上から
        int idxj = secondOcrData.Count - 1;
        int idx = primeOcrData.Count - 1;
        for (; idx >= 0; idx--)
        {
            var prime = primeOcrData[idx];
            double primeCx = prime.Coord.X + prime.Width / 2;
            double primeCy = prime.Coord.Y + prime.Height / 2;
            bool isBent = false;
            while (idxj >= 0)
            {
                var second = secondOcrData[idxj];
                double secondCx = second.Coord.X + second.Width / 2;
                double secondCy = second.Coord.Y + second.Height / 2;
                double tilt = UtilityFunc.CalcTiltByPoint(primeCx, primeCy, secondCx, secondCy);
                double tiltDiff = 0.0;
                if (isOuter)
                {
                    tiltDiff = Math.Abs(second.Tilt - tilt);
                }
                else
                {
                    tiltDiff = Math.Abs(prime.Tilt - tilt);
                }
                    if (primeCy > secondCy) isBent = true;
                if (tiltDiff < 3)
                {
                    isOk[idx] = true;
                    indecies[idx] = idxj;
                    break;
                }
                if (isBent)
                {
                    break;
                }
                idxj--;
            }
        }
        //2本指のアルゴリズム
        //下から
        idxj = 0;
        idx = 0;
        for (; idx < primeOcrData.Count; idx++)
        {
            var prime = primeOcrData[idx];
            double primeCx = prime.Coord.X + prime.Width / 2;
            double primeCy = prime.Coord.Y + prime.Height / 2;
            bool isBent = false;
            while (idxj < secondOcrData.Count)
            {
                var second = secondOcrData[idxj];
                double secondCx = second.Coord.X + second.Width / 2;
                double secondCy = second.Coord.Y + second.Height / 2;
                double tilt = UtilityFunc.CalcTiltByPoint(primeCx, primeCy, secondCx, secondCy);
                double tiltDiff = 0.0;
                if (!isOuter)
                {
                    tiltDiff = Math.Abs(prime.Tilt - tilt);
                }
                else
                {
                    tiltDiff = Math.Abs(second.Tilt - tilt);
                }
                if (primeCy < secondCy) isBent = true;
                if (tiltDiff < 3)
                {
                    isOk[idx] = true;
                    indecies[idx] = idxj;
                    break;
                }
                if (isBent)
                {
                    break;
                }
                idxj++;
            }
        }

        return Tuple.Create(isOk, indecies);
    }
    public void ExtractData(InvoiceIndex idx, bool isPackingList = false)
    {
        int start, end;
        if (idx.IsAny) {
            start = 0;
            end = App.MainView.InvoiceRawData.Count;
        }
        else
        {
            end = start = idx.Index;
            if(isPackingList) while (end < App.MainView.PackingListRawData.Count && String.Compare(idx.Key, App.MainView.PackingListRawData[end].InvoiceNo) == 0) end++;
            else while (end < App.MainView.InvoiceRawData.Count && String.Compare(idx.Key, App.MainView.InvoiceRawData[end].InvoiceNo) == 0) end++;
        }
        var regexPattern = isPackingList ? App.MainView.PackingListRegexPattern : App.MainView.InvoiceRegexPattern;
        if (regexPattern.ExistPattern == null)
        {
            System.Windows.MessageBox.Show("存在判定パターンがありません。");
            return;
        }
        for (int i = start; i < end; i++) {
            var ivRaw = isPackingList ? App.MainView.PackingListRawData[i] :  App.MainView.InvoiceRawData[i];
            InvoiceDataElement newData = extractIvData(ivRaw, regexPattern, App.MainView.CurrentCompanyProfile.is_serialized);
            if (newData == null) continue;
            if (isPackingList)
            {
                newData.Amount = "0";
                newData.Currency = "";
                newData.Origin = "";
                newData.DeclValid |= InvoiceDataElement.DeclearValidFlag.Amount;
                if (newData.Number == "") newData.Number = "0";
            }
            if (isPackingList) App.MainView.PackingListData.Add(newData);
            else App.MainView.InvoiceData.Add(newData);
        }


    }

    private InvoiceDataElement extractIvData(InvoiceRawData ivRaw, InvoiceRegexPattern invRegex, bool isSerialized)
    {
        if (ivRaw.Exist == null) return null;
        InvoiceDataElement newData = new InvoiceDataElement() { IndexNo = int.MaxValue };
        Match match = null;
        if (isSerialized)
        {
            if (ivRaw.Exist == "") return null;
            else if (invRegex.ExistSecondPattern != null && ivRaw.ExistSecondary == "") return null;
            else
            {
                newData.Exist = ivRaw.Exist;
                newData.InvoiceNo = ivRaw.InvoiceNo;
                newData.Amount = ivRaw.Amount;
                newData.Number = ivRaw.Number == "" ? "0" : ivRaw.Number;
                newData.NetWeight = ivRaw.NetWeight == "" ? "0" : ivRaw.NetWeight;
                newData.HSCodeKey = ivRaw.HSCodeKey;
                newData.HSCodeClue = ivRaw.HSCodeClue;
                newData.Origin = ivRaw.Origin;
                newData.Currency = ivRaw.Currency;
                newData.Description = ivRaw.Description;
            }
        }
        else
        {
            match = invRegex.Ext_ExistPattern.Match(ivRaw.Exist);

            if (!match.Success) return null;

            if (invRegex.Ext_ExistSecondPattern != null && !invRegex.Ext_ExistSecondPattern.Match(ivRaw.ExistSecondary).Success) return null;
            else newData.Exist = match.Groups[1].Value ?? "";

            if (invRegex.Ext_InvoiceNoPattern != null)
            {

                match = invRegex.Ext_InvoiceNoPattern.Match(ivRaw.InvoiceNo);
                if (match.Success)
                {
                    if (invRegex.IsInvoiceNoBackward)
                    {
                        int idx = match.Groups.Count - 1;
                        newData.InvoiceNo = match.Groups[idx].Value ?? "";
                    }
                    else
                    {
                        newData.InvoiceNo = match.Groups[1].Value ?? "";
                    }
                }
            }
            if (invRegex.Ext_AmountPattern != null)
            {
                match = isSerialized ? invRegex.AmountPattern.Match(ivRaw.Amount) : invRegex.Ext_AmountPattern.Match(ivRaw.Amount);
                if (match.Success)
                {
                    if (invRegex.IsAmountBackward)
                    {
                        int idx = match.Groups.Count - 1;
                        newData.Amount = match.Groups[idx].Value ?? "";
                    }
                    else
                    {
                        newData.Amount = match.Groups[1].Value ?? "";
                    }
                }
            }
            if (invRegex.Ext_NumberPattern != null)
            {
                match = invRegex.Ext_NumberPattern.Match(ivRaw.Number);
                if (match.Success)
                {
                    if (invRegex.IsNumberBackward)
                    {
                        int idx = match.Groups.Count - 1;
                        newData.Number = match.Groups[idx].Value ?? "0";
                    }
                    else
                    {
                        newData.Number = match.Groups[1].Value ?? "0";
                    }
                }
                else newData.Number = "0";
            }
            else
            {
                newData.Number = "0";
            }
            if (invRegex.Ext_NetWeightPattern != null)
            {
                match = invRegex.Ext_NetWeightPattern.Match(ivRaw.NetWeight);
                if (match.Success)
                {
                    if (invRegex.IsNetWeightBackward)
                    {
                        int idx = match.Groups.Count - 1;
                        newData.NetWeight = match.Groups[idx].Value ?? "0";
                    }
                    else
                    {
                        newData.NetWeight = match.Groups[1].Value ?? "0";
                    }
                }
                else newData.NetWeight = "0";

            }
            else
            {
                newData.NetWeight = "0";
            }
            if (invRegex.Ext_OriginPattern != null)
            {
                match = invRegex.Ext_OriginPattern.Match(ivRaw.Origin);
                if (match.Success)
                {
                    if (invRegex.IsOriginBackward)
                    {
                        int idx = match.Groups.Count - 1;
                        newData.Origin = match.Groups[idx].Value ?? "";
                    }
                    else
                    {
                        newData.Origin = match.Groups[1].Value ?? "";
                    }
                }
            }
            if (invRegex.Ext_HSCodeKeyPattern != null)
            {
                match = invRegex.Ext_HSCodeKeyPattern.Match(ivRaw.HSCodeKey);
                if (match.Success)
                {
                    if (invRegex.IsHSCodeKeyBackward)
                    {
                        int idx = match.Groups.Count - 1;
                        newData.HSCodeKey = match.Groups[idx].Value ?? "";
                    }
                    else
                    {
                        newData.HSCodeKey = match.Groups[1].Value ?? "";
                    }
                }
            }
            if (invRegex.Ext_HSCodeCluePattern != null)
            {
                match = invRegex.Ext_HSCodeCluePattern.Match(ivRaw.HSCodeClue);
                if (match.Success)
                {
                    if (invRegex.IsHSCodeClueBackward)
                    {
                        var matches = invRegex.Ext_HSCodeCluePattern.Matches(ivRaw.HSCodeClue);
                        int idx = matches[matches.Count - 1].Groups.Count - 1;
                        newData.HSCodeClue = matches[matches.Count - 1].Groups[idx].Value ?? "";
                    }
                    else
                    {
                        newData.HSCodeClue = match.Groups[1].Value ?? "";
                    }
                }
            }
            if (invRegex.Ext_CurrencyPattern != null)
            {
                match = invRegex.Ext_CurrencyPattern.Match(ivRaw.Currency);
                if (match.Success)
                {
                    if (invRegex.IsCurrencyBackward)
                    {
                        int idx = match.Groups.Count - 1;
                        newData.Currency = match.Groups[idx].Value ?? "";
                    }
                    else
                    {
                        newData.Currency = match.Groups[1].Value ?? "";
                    }
                }
            }
            if (invRegex.Ext_DescriptionPattern != null)
            {
                match = invRegex.Ext_DescriptionPattern.Match(ivRaw.Description);
                if (match.Success)
                {
                    if (invRegex.IsDescriptionBackward)
                    {
                        int idx = match.Groups.Count - 1;
                        newData.Description = match.Groups[idx].Value ?? "";
                    }
                    else
                    {
                        newData.Description = match.Groups[1].Value ?? "";
                    }
                }
            }
        }
        switch (App.MainView.CurrentCompanyProfile.hssuggesttype)
        {
            case "DIRECT":
                newData.HSCodeSuggestion = newData.HSCodeClue;
                break;
            default:
                break;
        }
        //newData.HSCode = "";
        int newIndex = App.MainView.InvoiceData.Count;
        if (App.MainView.CurrentCompanyProfile.converter != null)
        {
            foreach (var conv in App.MainView.CurrentCompanyProfile.converter)
            {
                if (conv.condition_type == null || conv.action_operator == null || conv.condition_field == null || conv.result_field == null) continue;
                if (!ConstValue.ConverterConditionDelegates.ContainsKey(conv.condition_type)) continue;
                var conditionDelegate = ConstValue.ConverterConditionDelegates[conv.condition_type];
                if (!ConstValue.ConverterActionDelegates.ContainsKey(conv.action_operator)) continue;
                var actionDelegate = ConstValue.ConverterActionDelegates[conv.action_operator];
                string cond_data = "";
                switch (conv.condition_field)
                {
                    case "INVOICENO":
                        cond_data = newData.InvoiceNo;
                        break;
                    case "AMOUNT":
                        cond_data = newData.Amount;
                        break;
                    case "NUMBER":
                        cond_data = newData.Number;
                        break;
                    case "NETWEIGHT":
                        cond_data = newData.NetWeight;
                        break;
                    case "ORIGIN":
                        cond_data = newData.Origin;
                        break;
                    case "HSCODEKEY":
                        cond_data = newData.HSCodeKey;
                        break;
                    case "HSCODECLUE":
                        cond_data = newData.HSCodeClue;
                        break;
                    case "CURRENCY":
                        cond_data = newData.Currency;
                        break;
                    case "DESCRIPTION":
                        cond_data = newData.Description;
                        break;
                    default:
                        continue;
                }
                string cond_const = conv.condition_right_operand_factor;
                if (conditionDelegate(cond_data, cond_const))
                {
                    string action_left_data = "";
                    if (conv.action_left_operand_field != null)
                    {
                        if (App.MainView.DataRegister.DataRegisterColumn.ContainsKey(conv.action_left_operand_field))
                        {
                            object reg = App.MainView.DataRegister.DataRegisterColumn[conv.action_left_operand_field];
                            if (reg.GetType() == typeof(List<int>))
                            {
                                if ((reg as List<int>).Count - 1 < newIndex) continue;
                                action_left_data = (reg as List<int>).ElementAt(newIndex).ToString();
                            }
                            else if (reg.GetType() == typeof(List<double>))
                            {
                                if ((reg as List<double>).Count - 1 < newIndex) continue;
                                action_left_data = (reg as List<double>).ElementAt(newIndex).ToString();
                            }
                            else
                            {
                                if ((reg as List<string>).Count - 1 < newIndex) continue;
                                action_left_data = (reg as List<string>).ElementAt(newIndex);
                            }
                        }
                        else
                        {
                            switch (conv.action_left_operand_field)
                            {
                                case "INVOICENO":
                                    action_left_data = newData.InvoiceNo;
                                    break;
                                case "AMOUNT":
                                    action_left_data = newData.Amount;
                                    break;
                                case "NUMBER":
                                    action_left_data = newData.Number;
                                    break;
                                case "NETWEIGHT":
                                    action_left_data = newData.NetWeight;
                                    break;
                                case "ORIGIN":
                                    action_left_data = newData.Origin;
                                    break;
                                case "HSCODEKEY":
                                    action_left_data = newData.HSCodeKey;
                                    break;
                                case "HSCODECLUE":
                                    action_left_data = newData.HSCodeClue;
                                    break;
                                case "CURRENCY":
                                    action_left_data = newData.Currency;
                                    break;
                                case "DESCRIPTION":
                                    action_left_data = newData.Description;
                                    break;
                                default:
                                    continue;
                            }
                        }
                    }
                    else
                    {
                        if (conv.action_left_operand_factor == null) continue;
                        action_left_data = conv.action_left_operand_factor;
                    }
                    string action_right_data = "";
                    
                    if (conv.action_right_operand_field != null)
                    {
                        if (App.MainView.DataRegister.DataRegisterColumn.ContainsKey(conv.action_right_operand_field))
                        {
                            object reg = App.MainView.DataRegister.DataRegisterColumn[conv.action_right_operand_field];
                            if (reg.GetType() == typeof(List<int>))
                            {
                                if ((reg as List<int>).Count - 1 < newIndex) continue;
                                action_right_data = (reg as List<int>).ElementAt(newIndex).ToString();
                            }
                            else if (reg.GetType() == typeof(List<double>))
                            {
                                if ((reg as List<double>).Count - 1 < newIndex) continue;
                                action_right_data = (reg as List<double>).ElementAt(newIndex).ToString();
                            }
                            else
                            {
                                if ((reg as List<string>).Count - 1 < newIndex) continue;
                                action_right_data = (reg as List<string>).ElementAt(newIndex);
                            }
                        }
                        else
                        {
                            switch (conv.action_right_operand_field)
                            {
                                case "INVOICENO":
                                    action_right_data = newData.InvoiceNo;
                                    break;
                                case "AMOUNT":
                                    action_right_data = newData.Amount;
                                    break;
                                case "NUMBER":
                                    action_right_data = newData.Number;
                                    break;
                                case "NETWEIGHT":
                                    action_right_data = newData.NetWeight;
                                    break;
                                case "ORIGIN":
                                    action_right_data = newData.Origin;
                                    break;
                                case "HSCODEKEY":
                                    action_right_data = newData.HSCodeKey;
                                    break;
                                case "HSCODECLUE":
                                    action_right_data = newData.HSCodeClue;
                                    break;
                                case "CURRENCY":
                                    action_right_data = newData.Currency;
                                    break;
                                case "DESCRIPTION":
                                    action_right_data = newData.Description;
                                    break;
                                default:
                                    continue;
                            }
                        }
                    }
                    else
                    {
                        if (conv.action_right_operand_factor == null) continue;
                        action_right_data = conv.action_right_operand_factor;
                    }
                    string res = actionDelegate(action_left_data, action_right_data);
                    double resValue;
                    if (App.MainView.DataRegister.DataRegisterColumn.ContainsKey(conv.result_field) && double.TryParse(res, out resValue))
                    {

                        object reg = App.MainView.DataRegister.DataRegisterColumn[conv.result_field];
                        if (reg.GetType() == typeof(List<int>))
                        {
                            if ((reg as List<int>).Count < App.MainView.InvoiceData.Count * 2)
                            {
                                App.MainView.DataRegister.Length = App.MainView.InvoiceData.Count * 2;
                                var newreg = new List<int>(Enumerable.Repeat(0, App.MainView.InvoiceData.Count * 2));
                                int regcntr = 0;
                                foreach (var oldval in (reg as List<int>))
                                {
                                    newreg[regcntr++] = oldval;
                                }
                                App.MainView.DataRegister.DataRegisterColumn[conv.result_field] = newreg;
                            }
                            (App.MainView.DataRegister.DataRegisterColumn[conv.result_field] as List<int>)[newIndex] = (int)resValue;
                        }
                        else if (reg.GetType() == typeof(List<double>) && double.TryParse(res, out resValue))
                        {
                            if ((reg as List<double>).Count < App.MainView.InvoiceData.Count * 2)
                            {
                                App.MainView.DataRegister.Length = App.MainView.InvoiceData.Count * 2;
                                var newreg = new List<double>(Enumerable.Repeat(0.0, App.MainView.InvoiceData.Count * 2));
                                int regcntr = 0;
                                foreach (var oldval in (reg as List<double>))
                                {
                                    newreg[regcntr++] = oldval;
                                }
                                App.MainView.DataRegister.DataRegisterColumn[conv.result_field] = newreg;
                            }
                            (App.MainView.DataRegister.DataRegisterColumn[conv.result_field] as List<double>)[newIndex] = (double)resValue;
                        }
                        else if (reg.GetType() == typeof(List<string>))
                        {
                            if ((reg as List<string>).Count < App.MainView.InvoiceData.Count * 2)
                            {
                                App.MainView.DataRegister.Length = App.MainView.InvoiceData.Count * 2;
                                var newreg = new List<string>(Enumerable.Repeat("", App.MainView.InvoiceData.Count * 2));
                                int regcntr = 0;
                                foreach (var oldval in (reg as List<string>))
                                {
                                    newreg[regcntr++] = oldval;
                                }
                                App.MainView.DataRegister.DataRegisterColumn[conv.result_field] = newreg;
                            }
                           (App.MainView.DataRegister.DataRegisterColumn[conv.result_field] as List<string>)[newIndex] = res;
                        }
                    }
                    else
                    {
                        switch (conv.result_field)
                        {
                            case "INVOICENO":
                                newData.InvoiceNo = res;
                                break;
                            case "AMOUNT":
                                newData.Amount = res;
                                break;
                            case "NUMBER":
                                newData.Number = res;
                                break;
                            case "NETWEIGHT":
                                newData.NetWeight = res;
                                break;
                            case "ORIGIN":
                                newData.Origin = res;
                                break;
                            case "HSCODEKEY":
                                newData.HSCodeKey = res;
                                break;
                            case "HSCODECLUE":
                                newData.HSCodeClue = res;
                                break;
                            case "CURRENCY":
                                newData.Currency = res;
                                break;
                            case "DESCRIPTION":
                                newData.Description = res;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        return newData;
    }

    public void AddInvoiceRow(int index)
    {
        if (index > App.MainView.InvoiceData.Count || index < 0) return;
        App.MainView.InvoiceData.Add(
            new InvoiceDataElement() {
                IndexNo = index,
                HSCode = "",
                Currency = "",
                Origin = "",
                Amount = "0",
                NetWeight = "0",
                Number = "0"
            }
            );
    }
    public void DeleteInvoiceRow(int index)
    {
        if (index > App.MainView.InvoiceData.Count - 1 || index < 0) return;
        App.MainView.InvoiceData.Remove(new InvoiceDataElement() { IndexNo = index });
    }

    public void AddPackingListRow(int index)
    {
        if (index > App.MainView.PackingListData.Count || index < 0) return;
        App.MainView.PackingListData.Add(
            new InvoiceDataElement()
            {
                IndexNo = index,
                HSCode = "",
                Currency = "",
                Origin = "",
                Amount = "0",
                NetWeight = "0",
                Number = "0"
            }
            );
    }
    public void DeletePackingListRow(int index)
    {
        if (index > App.MainView.PackingListData.Count - 1 || index < 0) return;
        App.MainView.PackingListData.Remove(new InvoiceDataElement() { IndexNo = index });
    }

    public void CopyInvoiceSuggestion()
    {
        foreach(var invData in App.MainView.InvoiceData)
        {
            if (invData.HSCode != "") continue;
            invData.HSCode = invData.HSCodeSuggestion;
        }
    }
    public void CopyInvoiceSuggestion(int index)
    {
        if (App.MainView.InvoiceData.Count - 1 < index|| index < 0) return;
        if (App.MainView.InvoiceData[index].HSCode != "") return;
        App.MainView.InvoiceData[index].HSCode = App.MainView.InvoiceData[index].HSCodeSuggestion;
    }

    public void DuplicateCurrency(bool isPackingList = false)
    {
        string primeCurrency = "";
        foreach(var cur in App.MainView.CurrencyProfiles)
        {
            if (cur.IsPrime)
            {
                primeCurrency = cur.Code;
                break;
            }
        }
        if (isPackingList)
        {
            string prev = "";
            foreach (var plData in App.MainView.PackingListData)
            {
                if (plData.Currency != "")
                {
                    prev = plData.Currency;
                    continue;
                }
                if(prev == "")
                {
                    plData.Currency = primeCurrency;
                }
                else
                {
                    plData.Currency = prev;
                }
                prev = plData.Currency;
            }
        }
        else
        {
            string prev = "";
            foreach (var invData in App.MainView.InvoiceData)
            {
                if (invData.Currency != "")
                {
                    prev = invData.Currency;
                    continue;
                }
                if (prev == "")
                {
                    invData.Currency = primeCurrency;
                }
                else
                {
                    invData.Currency = prev;
                }
                prev = invData.Currency;
            }
        }
    }

    public void DuplicateOrigin(bool isPackingList = false)
    {
        string primeOrigin = "JP";
        if (isPackingList)
        {
            string prev = "";
            foreach (var plData in App.MainView.PackingListData)
            {
                if (plData.Origin != "")
                {
                    prev = plData.Origin;
                    continue;
                }
                if (prev == "")
                {
                    plData.Origin = primeOrigin;
                }
                else
                {
                    plData.Origin = prev;
                }
                prev = plData.Origin;
            }
        }
        else
        {
            string prev = "";
            foreach (var invData in App.MainView.InvoiceData)
            {
                if (invData.Origin != "")
                {
                    prev = invData.Origin;
                    continue;
                }
                if (prev == "")
                {
                    invData.Origin = primeOrigin;
                }
                else
                {
                    invData.Origin = prev;
                }
                prev = invData.Origin;
            }
        }
    }

    public void CopyPackingListSuggestion()
    {
        foreach (var invData in App.MainView.PackingListData)
        {
            if (invData.HSCode != "") continue;
            invData.HSCode = invData.HSCodeSuggestion;
        }
    }
    public void CopyPackingListSuggestion(int index)
    {
        if (App.MainView.PackingListData.Count - 1 < index || index < 0) return;
        if (App.MainView.PackingListData[index].HSCode != "") return;
        App.MainView.PackingListData[index].HSCode = App.MainView.PackingListData[index].HSCodeSuggestion;
    }

    public bool CheckCurrencyIsValid()
    {
        bool flag = true;
        foreach(var accum in App.MainView.DeclearationAccum.DeclAccumDictionary)
        {
            string cur = accum.Value.Currency;
            var profile = App.MainView.CurrencyProfiles.Find(new CurrencyProfile() { Code = cur });
            if (profile == null)
            {
                profile = new CurrencyProfile() { Code = cur,IsRateValid = false};
                App.MainView.CurrencyProfiles.Add(profile) ;
                flag = false;
            }
            else
            {
                if (!profile.IsRateValid) flag = false;
            }
        }
        return flag;
        
    }

    public bool CheckDataIsValid()
    {
        foreach(var data in App.MainView.InvoiceData)
        {
            if (data.DeclValid != InvoiceDataElement.DeclearValidFlag.ALL) return false;
        }
        return true;
    }

    public bool CheckValueValid()
    {
        return IsFaceValueValid && IsFOBValueValid;
    }
    public bool CheckCurrencyIsSingle()
    {
        bool flag = true;
        string cur = App.MainView.InvoiceData[0].Currency;
        foreach(var data in App.MainView.InvoiceData)
        {
            if (data.Currency != cur)
            {
                flag = false;
            }
        }
        return flag;
    }

    public void Calc()
    {
        double tempFaceValue = 0.0;
        double.TryParse(FaceValue, out tempFaceValue);
        double tempFOBValue = 0.0;
        double.TryParse(FOBValue, out tempFOBValue);
        double factor = 1.0;
        SortedList<string, int> majorCntrs = new SortedList<string, int>();
        SortedList<string, int> majorYCntrs = new SortedList<string, int>();
        SortedList<string, int> minorCntrs = new SortedList<string, int>();
        SortedList<string, int> majorYIndicies = new SortedList<string, int>();
        SortedList<string, int> minorIndicies = new SortedList<string, int>();
        SortedList<string, double> ttlFOBJpyValue = new SortedList<string, double>();
        SortedList<string, CurrencyProfile> currencies = new SortedList<string, CurrencyProfile>();
        SortedList<string, double> rates = new SortedList<string, double>();

        foreach (var cur in App.MainView.CurrencyProfiles)
        {
            currencies.Add(cur.Code, cur);
        }
        string primeCurrency = "";
        foreach (var currencyProfile in App.MainView.CurrencyProfiles)
        {
            if (currencyProfile.IsPrime)
            {
                primeCurrency = currencyProfile.Code;
                break;
            }
        }

        bool isWholeAnotherCurrency = true;
        foreach (var accumDic in App.MainView.DeclearationAccum.DeclAccumDictionary)
        {
            if (accumDic.Value.Currency == primeCurrency)
            {
                isWholeAnotherCurrency = false;
                break;
            }
        }
        string anotherCurrency = "";
        if (isWholeAnotherCurrency)
        {
            anotherCurrency = primeCurrency;
            primeCurrency = App.MainView.DecidePrimeCurrency();
            double ttlBPR = 0.0;
            ulong ttlBPRInternal = 0L;
            foreach (var accumDic in App.MainView.DeclearationAccum.DeclAccumDictionary)
            {
                if (accumDic.Value.Currency == primeCurrency) ttlBPRInternal += (ulong)accumDic.Value.AmountInternal;
            }
            ttlBPR = (double)ttlBPRInternal / (double)ConstValue.InternalValueFactor;
            double rate = 1.0;
            double.TryParse(currencies[anotherCurrency].Rate, out rate);
            factor = tempFOBValue * rate / ttlBPR;
        }
        else
        {
            double ttlBPR = 0.0;
            ulong ttlBPRInternal = 0L;
            foreach (var accumDic in App.MainView.DeclearationAccum.DeclAccumDictionary)
            {
                if (accumDic.Value.Currency == primeCurrency) ttlBPRInternal += (ulong)accumDic.Value.AmountInternal;
            }
            ttlBPR = (double)ttlBPRInternal / (double)ConstValue.InternalValueFactor;
            double rate = 1.0;
            double.TryParse(currencies[primeCurrency].Rate, out rate);
            factor = tempFOBValue * rate / ttlBPR;
            if (factor > 0.999999 && factor < 1.000001) factor = 1;
        }

        foreach (var accumDic in App.MainView.DeclearationAccum.DeclAccumDictionary)
        {
            double tempRate = 0.0;
            bool isPrimeCurrency = currencies[accumDic.Value.Currency].IsPrime; 
            if (!rates.ContainsKey(accumDic.Value.Currency)) {
                double.TryParse(currencies[accumDic.Value.Currency].Rate, out tempRate);
                rates.Add(accumDic.Value.Currency, tempRate);
            }
            else
            {
                tempRate = rates[accumDic.Value.Currency];
            }
            double tempValue = isPrimeCurrency ? (double)accumDic.Value.AmountInternal  * factor : (double)accumDic.Value.AmountInternal * tempRate;
            if (!ttlFOBJpyValue.ContainsKey(accumDic.Value.HashWOCurrency()))
            {
                ttlFOBJpyValue.Add(accumDic.Value.HashWOCurrency(), tempValue);
            }
            else
            {
                ttlFOBJpyValue[accumDic.Value.HashWOCurrency()] += tempValue;
            }
        }

        foreach (var accumDic in App.MainView.DeclearationAccum.DeclAccumDictionary)
        {
            if (accumDic.Value.Postfix != "Y" && accumDic.Value.Postfix != "E" && ttlFOBJpyValue[accumDic.Value.HashWOCurrency()] >= ConstValue.GOLD)
            {
                int idx = RegsiterDeclElement(accumDic.Value,isWholeAnotherCurrency,primeCurrency);
                App.MainView.DeclearationAccum.DeclAccumDictionary[accumDic.Key].DeclIndex = idx;
                if (!majorCntrs.ContainsKey(accumDic.Value.Currency)) majorCntrs.Add(accumDic.Value.Currency, 1);
                else majorCntrs[accumDic.Value.Currency]++;
            }
        }
        var majorYbuckets = new SortedList<string, DeclAccumElem>();
        var maxHSCodes = new SortedList<string, string>();
        var prevAmounts = new SortedList<string, long>();
        foreach (var accumDic in App.MainView.DeclearationAccum.DeclAccumDictionary)
        {
            if (accumDic.Value.Postfix == "Y"  && ttlFOBJpyValue[accumDic.Value.HashWOCurrency()] >= ConstValue.GOLD)
            {
                if (!majorYbuckets.ContainsKey(accumDic.Value.Currency))
                {
                    majorYbuckets.Add(accumDic.Value.Currency, new DeclAccumElem()
                    {
                        AmountInternal = accumDic.Value.AmountInternal,
                        NetWeightInternal = accumDic.Value.NetWeightInternal,
                        Postfix = "Y",
                        Currency = accumDic.Value.Currency,
                    }) ;
                    maxHSCodes.Add(accumDic.Value.Currency, accumDic.Value.HSCode);
                    prevAmounts.Add(accumDic.Value.Currency, accumDic.Value.AmountInternal);
                    majorYCntrs.Add(accumDic.Value.Currency, 1);
                }
                else
                {
                    majorYbuckets[accumDic.Value.Currency].AmountInternal += accumDic.Value.AmountInternal;
                    majorYbuckets[accumDic.Value.Currency].NetWeightInternal += accumDic.Value.NetWeightInternal;
                    if (prevAmounts[accumDic.Value.Currency] < accumDic.Value.AmountInternal) maxHSCodes[accumDic.Value.Currency] = accumDic.Value.HSCode;
                    majorYCntrs[accumDic.Value.Currency]++;
                }
            }

        }
        foreach(var majY in majorYbuckets)
        {
            majY.Value.HSCode = maxHSCodes[majY.Key];
            int idx = RegsiterDeclElement(majY.Value,isWholeAnotherCurrency,primeCurrency);
            majorYIndicies.Add(majY.Key, idx);
        }
        var minorBuckets = new SortedList<string, DeclAccumElem>();
        maxHSCodes = new SortedList<string, string>();
        prevAmounts = new SortedList<string, long>();
        var minorBucket = new DeclAccumElem();
        foreach (var accumDic in App.MainView.DeclearationAccum.DeclAccumDictionary)
        {
            
            if (accumDic.Value.Postfix == "E" || ttlFOBJpyValue[accumDic.Value.HashWOCurrency()] < ConstValue.GOLD)
            {
                if (!minorBuckets.ContainsKey(accumDic.Value.Currency))
                {
                    minorBuckets.Add(accumDic.Value.Currency, new DeclAccumElem()
                    {
                        AmountInternal = accumDic.Value.AmountInternal,
                        Currency = accumDic.Value.Currency,
                    });
                    maxHSCodes.Add(accumDic.Value.Currency, accumDic.Value.HSCode);
                    prevAmounts.Add(accumDic.Value.Currency, accumDic.Value.AmountInternal);
                    minorCntrs.Add(accumDic.Value.Currency, 1);
                }
                else
                {
                    minorBuckets[accumDic.Value.Currency].AmountInternal += accumDic.Value.AmountInternal;
                    if (prevAmounts[accumDic.Value.Currency] < accumDic.Value.AmountInternal) maxHSCodes[accumDic.Value.Currency] = accumDic.Value.HSCode;
                    minorCntrs[accumDic.Value.Currency]++;
                }
            }
        }
        foreach(var minorElem in minorBuckets)
        {
            int minorIdx = -1;
            minorElem.Value.Postfix = minorCntrs[minorElem.Value.Currency] > 1 ? "X" : "E";
            minorElem.Value.HSCode = maxHSCodes[minorElem.Value.Currency];
            minorIdx = RegsiterDeclElement(minorElem.Value,isWholeAnotherCurrency,primeCurrency);
            minorIndicies.Add(minorElem.Value.Currency, minorIdx);
        }
        var selectedTerm = App.MainView.SubWindowController.DataWindow.TradetermCombobox.SelectedItem as TradetermProfile;
        if (selectedTerm != null)
        {
            App.MainView.DeclArtcile.TradeTerm = selectedTerm.Tradeterm;
        }
        if (selectedTerm != null && selectedTerm.Tradeterm != "FOB")
        {
            App.MainView.DeclArtcile.TermAmount = FaceValue;
            App.MainView.DeclArtcile.FOBAmount = FOBValue;
        }
        else
        {
            App.MainView.DeclArtcile.TermAmount = FOBValue;
            App.MainView.DeclArtcile.FOBAmount = "";
        }
        if (isWholeAnotherCurrency)
        {
            App.MainView.DeclArtcile.Currency = anotherCurrency;
        }
        else {
            App.MainView.DeclArtcile.Currency = primeCurrency;
        }
        //少額の場合
        if (majorCntrs.Count == 0 && majorYCntrs.Count == 0)
        {
            App.MainView.DeclArtcile.BouekiType = "";
            var maxAccum = ttlFOBJpyValue.OrderByDescending(elem => elem.Value).FirstOrDefault();
            App.MainView.DeclArtcile.Daisyou = "S";
            App.MainView.DeclArtcile.DeclSubmiss = "1H";
            int comp = maxAccum.Key.Substring(0, 2).CompareTo("50");
            if (comp > -1) App.MainView.DeclArtcile.DeclSubmissBumon = "02";
            else App.MainView.DeclArtcile.DeclSubmissBumon = "01";
            App.MainView.DeclArtcile.DeclElements.Clear();
            App.MainView.DeclArtcile.IsMinor = true;
            //複数通貨混在（少額）
            if (minorCntrs.Count > 1)
            {
                //全てJPY換算で計算する。FaceValue = Prime通貨(Term) + 強制通貨(FOB) FOBValue =  Prime通貨(FOB) + 強制通貨(FOB)
                double faceValue = 0.0;
                double fobValue = 0.0;
                foreach (var fob in ttlFOBJpyValue)
                {
                    fobValue += fob.Value;
                }
                faceValue += tempFaceValue * rates[primeCurrency] * ConstValue.InternalValueFactor;
                foreach(var acuum in App.MainView.DeclearationAccum.DeclAccumDictionary)
                {
                    if(acuum.Value.Currency != primeCurrency)
                    {
                        faceValue += (double)acuum.Value.AmountInternal * rates[acuum.Value.Currency];
                    }
                }
                fobValue = fobValue > 0 ? fobValue / ConstValue.InternalValueFactor : 0;
                faceValue = faceValue > 0 ? faceValue / ConstValue.InternalValueFactor : 0;
                App.MainView.DeclArtcile.Currency = "JPY";
                if (App.MainView.DeclArtcile.TradeTerm != "FOB")
                {
                    App.MainView.DeclArtcile.TermAmount = string.Format("{0:0.0}", faceValue);
                    App.MainView.DeclArtcile.TermAmount = App.MainView.DeclArtcile.TermAmount.Length > 1 ? App.MainView.DeclArtcile.TermAmount.Substring(0, App.MainView.DeclArtcile.TermAmount.Length - 1) : App.MainView.DeclArtcile.TermAmount;
                    App.MainView.DeclArtcile.FOBAmount = string.Format("{0:0.0}", fobValue);
                    App.MainView.DeclArtcile.FOBAmount = App.MainView.DeclArtcile.FOBAmount.Length > 1 ? App.MainView.DeclArtcile.FOBAmount.Substring(0, App.MainView.DeclArtcile.FOBAmount.Length - 1) : App.MainView.DeclArtcile.FOBAmount;
                }
                else
                {
                    App.MainView.DeclArtcile.TermAmount = string.Format("{0:0.0}", fobValue);
                    App.MainView.DeclArtcile.TermAmount = App.MainView.DeclArtcile.FOBAmount.Length > 1 ? App.MainView.DeclArtcile.FOBAmount.Substring(0, App.MainView.DeclArtcile.FOBAmount.Length - 1) : App.MainView.DeclArtcile.FOBAmount;
                }
            }
        }
        else
        {
            App.MainView.DeclArtcile.BouekiType = "118";
            App.MainView.DeclArtcile.Daisyou = "L";
            App.MainView.DeclArtcile.DeclSubmiss = "";
            App.MainView.DeclArtcile.DeclSubmissBumon = "";

            //1欄(主要通貨)+(通貨強制(含む少額))?
            if (((majorCntrs.ContainsKey(primeCurrency) && majorCntrs[primeCurrency] == 1 && !majorYCntrs.ContainsKey(primeCurrency) && !minorCntrs.ContainsKey(primeCurrency)) ||(majorYCntrs.ContainsKey(primeCurrency) && !majorCntrs.ContainsKey(primeCurrency) && !minorCntrs.ContainsKey(primeCurrency))))
            {
                App.MainView.DeclArtcile.DeclElements[0].BasicPrice = "";
            }
        }

        foreach (var accumDic in App.MainView.DeclearationAccum.DeclAccumDictionary)
        {
            if (accumDic.Value.Postfix == "Y" && ttlFOBJpyValue[accumDic.Value.HashWOCurrency()] >= ConstValue.GOLD)
            {
                App.MainView.DeclearationAccum.DeclAccumDictionary[accumDic.Key].DeclIndex = majorYIndicies[accumDic.Value.Currency];
            }
        }
        foreach (var accumDic in App.MainView.DeclearationAccum.DeclAccumDictionary)
        {
            if (accumDic.Value.Postfix == "E" || ttlFOBJpyValue[accumDic.Value.HashWOCurrency()] < ConstValue.GOLD)
            {
                App.MainView.DeclearationAccum.DeclAccumDictionary[accumDic.Key].DeclIndex = minorIndicies[accumDic.Value.Currency];
            }
        }
    }
    private int RegsiterDeclElement(DeclAccumElem acuum, bool isWholeAnotherCurrency = false, string tempPrimeCurrency = "")
    {
        string BPR = string.Format("{0:0.000}", (double)acuum.AmountInternal / (double)ConstValue.InternalValueFactor);
        BPR = BPR.Substring(0, BPR.Length - 1); //小数点3桁目を切り捨てる。
        string NW = string.Format("{0:0.000}", (double)acuum.NetWeightInternal / (double)ConstValue.InternalValueFactor);
        string NO = string.Format("{0:0.000}", (double)acuum.NumberInternal / (double)ConstValue.InternalValueFactor);
        string HSCODE = acuum.Postfix == "A" ? acuum.HSCode : acuum.HSCode.Substring(0, 9) + acuum.Postfix;
        string CURRENCY = "";
        if (isWholeAnotherCurrency)
        {
            CURRENCY = acuum.Currency == tempPrimeCurrency ?  "" : acuum.Currency;
        }
        else
        {
            CURRENCY = App.MainView.CurrencyProfiles.Find(new CurrencyProfile { Code = acuum.Currency }).IsPrime ? "" : acuum.Currency;
        }
        string UNIT1 = "";
        string UNIT2 = "";
        string QTY1 = "";
        string QTY2 = "";
        if (acuum.Postfix == "A")
        {
            if (HSCode.HSCodeDictionary.ContainsKey(acuum.HSCode.Substring(0, 9)))
            {
                UNIT1 = HSCode.HSCodeDictionary[acuum.HSCode.Substring(0, 9)].Unit1 == "" ? HSCode.HSCodeDictionary[acuum.HSCode.Substring(0, 9)].Unit2 : HSCode.HSCodeDictionary[acuum.HSCode.Substring(0, 9)].Unit1;
                UNIT2 = HSCode.HSCodeDictionary[acuum.HSCode.Substring(0, 9)].Unit1 == "" ? "" : HSCode.HSCodeDictionary[acuum.HSCode.Substring(0, 9)].Unit2;
            }
            if (UNIT1 == "NO")
            {
                QTY1 = NO;
            }
            else if (UNIT1 == "KG")
            {
                QTY1 = NW;
            }
            else
            {
                QTY1 = "";
            }
            if (UNIT2 == "NO")
            {
                QTY2 = NO;
            }
            else if (UNIT2 == "KG")
            {
                QTY2 = NW;
            }
            else
            {
                QTY2 = "";
            }
        }
        else if (acuum.Postfix == "Y")
        {
            UNIT1 = "KG";
            QTY1 = NW;
            UNIT2 = "";
            QTY2 = "";
        }
        else
        {
            UNIT1 = "";
            UNIT2 = "";
            QTY1 = "";
            QTY2 = "";
        }

        var newElem = new DeclArticle.DeclElement()
        {
            HSCode = HSCODE,
            Unit1 = UNIT1,
            Quantity1 = QTY1,
            Unit2 = UNIT2,
            Quantity2 = QTY2,
            BasicPrice = BPR,
            Currency = CURRENCY,
        };

        App.MainView.DeclArtcile.DeclElements.Add(newElem);
        App.MainView.DeclArtcile.IVNetWeight = "";
        App.MainView.DeclArtcile.ValidateAnbunNetWeight();
        App.MainView.DeclArtcile.ValidateQuantity();
        return App.MainView.DeclArtcile.DeclElements.Count;

    }

    public void UpdateHSCode()
    {
        if (App.MainView.CurrentCompanyProfile == null) return;
        if (App.MainView.CurrentCompanyProfile.hs_readonly || !App.MainView.IsHSMasterValid) return;
        foreach(var hs in App.MainView.ImmediateHSMaster)
        {
            if (hs.Value.ForceUpdate)
            {
                if (hs.Value.OldHSCode == null && hs.Value.NewHSCode == null) continue;
                string validHSCode = hs.Value.NewHSCode == null ? hs.Value.OldHSCode : hs.Value.NewHSCode;
                if (!HSCode.HSCodeDictionary.ContainsKey(validHSCode)) continue;
                IVEditElasticClient.PostHSCode(App.MainView.HSMasterIndex, hs.Key, validHSCode, hs.Value.HSClue, DateTime.Now);
            }
            else
            {
                if (hs.Value.NewHSCode == null || !HSCode.HSCodeDictionary.ContainsKey(hs.Value.NewHSCode)) continue;
                IVEditElasticClient.PostHSCode(App.MainView.HSMasterIndex, hs.Key, hs.Value.NewHSCode, hs.Value.HSClue, DateTime.Now);
            }
        }
    } 
    public static bool GenerateInput(object sender, RoutedEventArgs e)
    {
        if(App.MainView.SubWindowController.DataWindow == null)
        {
            App.MainView.SubWindowController.OpenDataWindow(sender, e);
        }
        var dataWindow = App.MainView.SubWindowController.DataWindow;
        if (!dataWindow.View.CheckCurrencyIsValid())
        {
            App.MainView.OpenCurrencyWindow(sender, e);
            return false;
        }
        if (App.MainView.InvoiceData == null || App.MainView.InvoiceData.Count == 0)
        {
            System.Windows.MessageBox.Show("データが空です");
            return false;
        }
        if (!dataWindow.View.CheckDataIsValid())
        {
            System.Windows.MessageBox.Show("正しく入力されていないデータがあります");
            return false;
        }
        if (!dataWindow.View.CheckValueValid())
        {
            System.Windows.MessageBox.Show("トレードタームの価格及びFOB価格を正しく入力してください");
            return false;
        }
        App.MainView.DeclArtcile = new DeclArticle();
        dataWindow.View.Calc();
        dataWindow.View.UpdateHSCode();
        if (App.MainView.SubWindowController.InputWindow != null)
        {
            App.MainView.SubWindowController.InputWindow.Close();
            App.MainView.SubWindowController.OpenInputWindow(sender, e);
        }
        else
        {
            App.MainView.SubWindowController.OpenInputWindow(sender, e);
        }
        return true;
    }

    public static void DeleteInvoiceCell(object sender, RoutedEventArgs e, bool isPackingList = false)
    {
        var menuitem = sender as System.Windows.Controls.MenuItem;
        if (menuitem == null || menuitem.Tag == null) return;
        string[] tags = menuitem.Tag.ToString().Split(',');
        int index = 0;
        ObservableSortedCollection<InvoiceDataElement> inv = null;
        if (isPackingList)
        {
            inv = App.MainView.PackingListData;
        }
        else
        {
            inv = App.MainView.InvoiceData;
        }
        if (!int.TryParse(tags[1], out index)|| index > inv.Count-1 || index < 0) return;
        for(int i = index; i < inv.Count-1; i++)
        {
            switch (tags[0])
            {
                case "HSKEY":
                    inv[i].HSCode = "";
                    inv[i].HSCodeKey = inv[i+1].HSCodeKey;
                    break;
                case "HSCLUE":
                    inv[i].HSCodeClue = inv[i+1].HSCodeClue;
                    break;
                case "NETWEIGHT":
                    inv[i].NetWeight = inv[i+1].NetWeight;
                    break;
                case "NUMBER":
                    inv[i].Number = inv[i+1].Number;
                    break;
                case "AMOUNT":
                    inv[i].Amount = inv[i+1].Amount;
                    break;
                case "CURRENCY":
                    inv[i].Currency = inv[i+1].Currency;
                    break;
                case "ORIGIN":
                    inv[i].Origin = inv[i+1].Origin;
                    break;
                case "DESCRIPTION":
                    inv[i].Description = inv[i+1].Description;
                    break;
                default:
                    break;
            }
        }
        switch (tags[0])
        {
            case "HSKEY":
                inv[inv.Count - 1].HSCode = "";
                inv[inv.Count-1].HSCodeKey = "";
                break;
            case "HSCLUE":
                inv[inv.Count - 1].HSCodeClue = "";
                break;
            case "NETWEIGHT":
                inv[inv.Count - 1].NetWeight = "";
                break;
            case "NUMBER":
                inv[inv.Count - 1].Number = "";
                break;
            case "AMOUNT":
                inv[inv.Count - 1].Amount = "";
                break;
            case "CURRENCY":
                inv[inv.Count - 1].Currency = "";
                break;
            case "ORIGIN":
                inv[inv.Count - 1].Origin = "";
                break;
            case "DESCRIPTION":
                inv[inv.Count - 1].Description = "";
                break;
            default:
                break;
        }
    }

    public static void InsertInvoiceCell(object sender, RoutedEventArgs e, bool isToNextblanc = false, bool isPackingList = false)
    {
        ObservableSortedCollection<InvoiceDataElement> inv = null;
        if (isPackingList)
        {
            inv = App.MainView.PackingListData;
        }
        else
        {
            inv = App.MainView.InvoiceData;
        }
        var menuitem = sender as System.Windows.Controls.MenuItem;
        if (menuitem == null || menuitem.Tag == null) return;
        string[] tags = menuitem.Tag.ToString().Split(',');
        if (tags.Length < 2) return;
        int index = 0;
        if (!int.TryParse(tags[1], out index) || index < 0) return;
        int nextblanc = 0;
        bool isContainBlanc = false;
        if (isToNextblanc) {
            for (int i = index; i < inv.Count; i++)
            {
                switch (tags[0])
                {
                    case "HSKEY":
                        if (inv[i].HSCodeKey == "")
                        {
                            nextblanc = i;
                            isContainBlanc = true;
                        }
                        break;
                    case "HSCLUE":
                        if (inv[i].HSCodeClue == "")
                        {
                            nextblanc = i;
                            isContainBlanc = true;
                        }
                        break;
                    case "NETWEIGHT":
                        if (inv[i].NetWeight == "")
                        {
                            nextblanc = i;
                            isContainBlanc = true;
                        }
                        break;
                    case "NUMBER":
                        if (inv[i].Number == "")
                        {
                            nextblanc = i;
                            isContainBlanc = true;
                        }
                        break;
                    case "AMOUNT":
                        if (inv[i].Amount == "")
                        {
                            nextblanc = i;
                            isContainBlanc = true;
                        }
                        break;
                    case "CURRENCY":
                        if (inv[i].Currency == "")
                        {
                            nextblanc = i;
                            isContainBlanc = true;
                        }
                        break;
                    case "ORIGIN":
                        if (inv[i].Origin == "")
                        {
                            nextblanc = i;
                            isContainBlanc = true;
                        }
                        break;
                    case "DESCRIPTION":
                        if (inv[i].Description == "")
                        {
                            nextblanc = i;
                            isContainBlanc = true;
                        }
                        break;
                    default:
                        break;
                }
                if (isContainBlanc) break;
            }
        }
        string previnvno = inv[inv.Count - 1].InvoiceNo;
        if (!isContainBlanc)
        {
            inv.Add(new InvoiceDataElement() { IndexNo = int.MaxValue, InvoiceNo = previnvno });
            nextblanc = inv.Count - 1;
        }
        for (int i = nextblanc; i > index; i--)
        {
            switch (tags[0])
            {
                case "HSKEY":
                    inv[i].HSCode = "";
                    inv[i].HSCodeKey = inv[i - 1].HSCodeKey;
                    break;
                case "HSCLUE":
                    inv[i].HSCodeClue = inv[i - 1].HSCodeClue;
                    break;
                case "NETWEIGHT":
                    inv[i].NetWeight = inv[i - 1].NetWeight;
                    break;
                case "NUMBER":
                    inv[i].Number = inv[i - 1].Number;
                    break;
                case "AMOUNT":
                    inv[i].Amount = inv[i - 1].Amount;
                    break;
                case "CURRENCY":
                    inv[i].Currency = inv[i - 1].Currency;
                    break;
                case "ORIGIN":
                    inv[i].Origin = inv[i - 1].Origin;
                    break;
                case "DESCRIPTION":
                    inv[i].Description = inv[i - 1].Description;
                    break;
                default:
                    break;
            }
        }
        switch (tags[0])
        {
            case "HSKEY":
                inv[index].HSCode = "";
                inv[index].HSCodeKey = "";
                break;
            case "HSCLUE":
                inv[index].HSCodeClue = "";
                break;
            case "NETWEIGHT":
                inv[index].NetWeight = "";
                break;
            case "NUMBER":
                inv[index].Number = "";
                break;
            case "AMOUNT":
                inv[index].Amount = "";
                break;
            case "CURRENCY":
                inv[index].Currency = "";
                break;
            case "ORIGIN":
                inv[index].Origin = "";
                break;
            case "DESCRIPTION":
                inv[index].Description = "";
                break;
            default:
                break;
        }
    }

    public static void Reset()
    {
        App.MainView.RemoveInvoiceData();
        App.MainView.RemovePackingListData();
        App.MainView.RemoveBucketData();
        App.MainView.RemoveQrCodePage();
        App.MainView.RemoveDeclArticle();
        App.MainView.RemoveInputPaper();
        App.MainView.RemoveDeclAccum();
        App.MainView.ResetBLNo();
        App.MainView.ResetDocNo();
        App.MainView.ResetDataRegister();
        App.MainView.ResetIsPrimeCurrencyForced();
        if(App.MainView.SubWindowController.DataWindow != null)
        {
            App.MainView.SubWindowController.DataWindow.View.FOBValue = "";
            App.MainView.SubWindowController.DataWindow.View.FaceValue = "";
            App.MainView.SubWindowController.DataWindow.TradetermCombobox.SelectedItem = null;
        }
        if (App.MainView.Pages.Count > 0)
        {
            App.MainView.CurrentPage = App.MainView.Pages[0];
            App.MainView.CurrentAnnotaion.Clear();
            foreach (var annot in App.MainView.Pages[0].Annotaion)
            {
                App.MainView.CurrentAnnotaion.Add(annot);
            }
        }
        else
        {
            App.MainView.CurrentPage = new Page();
            App.MainView.CurrentAnnotaion.Clear();
        }
    }
}

