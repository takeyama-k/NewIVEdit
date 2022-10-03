using System;
using System.Windows;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Ink;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Geom;
using iText.IO.Image;
using ZXing;

namespace NewIVEdit
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public MainWindowViewModel()
        {
            PackingListData.CollectionChanged += _invoiceDataRemoved;
            InvoiceData.CollectionChanged += _invoiceDataRemoved;
            Pages.CollectionChanged += _pageAdded;
            Pages.CollectionChanged += _pageRemoved;
        }
        public ObservableSortedCollection<PageThumbneil> PageThumbneils { set; get; } = new ObservableSortedCollection<PageThumbneil>();
        public ObservableSortedCollection<Page> Pages { set; get; } = new ObservableSortedCollection<Page>();
        private Page _currentPage = new Page();
        public Page CurrentPage
        {
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    NofityPropertyChanged("CurrentPage");
                    CurrentAnnotaion.Clear();
                    foreach(var annot in CurrentPage.Annotaion)
                    {
                        CurrentAnnotaion.Add(annot);
                    }
                    CurrentInkStroke = CurrentPage.Stroke;
                }
            }
            get
            {
                return _currentPage;
            }
        }

        public List<IVEditElasticClient.CompanyProfile> CompanyProfiles { set; get; } = new List<IVEditElasticClient.CompanyProfile>();
        public ObservableSortedCollection<IVEditElasticClient.CompanyIndex> CompanyIndicies { set; get; } = new ObservableSortedCollection<IVEditElasticClient.CompanyIndex>();
        public ObservableSortedCollection<IVEditElasticClient.CompanyIndex.Subtype> CompanySubtypes { set; get; } = new ObservableSortedCollection<IVEditElasticClient.CompanyIndex.Subtype>();

        public RenderController RenderController = new RenderController();

        public SubWindowController SubWindowController = new SubWindowController();

        public InkCanvasController IinkCanvasController = new InkCanvasController();

        public IVEditElasticClient ElasticClient = new IVEditElasticClient();
        public ObservableSortedCollection<Annotation> CurrentAnnotaion { set; get; } = new ObservableSortedCollection<Annotation>();

        private StrokeCollection _currentInkStroke = new StrokeCollection();
        public StrokeCollection CurrentInkStroke {
            set
            {
                _currentInkStroke = value;
                NofityPropertyChanged("CurrentInkStroke");
            }
            get
            {
                return _currentInkStroke;
            } 
        }

        public IVEditElasticClient.CompanyIndex CurrentCompanyIndex { set; get; } = null;
        public IVEditElasticClient.CompanyIndex.Subtype CurrentCompanySubtype { set; get; } = null;
        public IVEditElasticClient.CompanyProfile CurrentCompanyProfile { set; get; } = null;

        public ObservableSortedCollection<InvoiceRawData> PackingListRawData { set; get; } = new ObservableSortedCollection<InvoiceRawData>();
        public ObservableSortedCollection<InvoiceDataElement> PackingListData { set; get; } = new ObservableSortedCollection<InvoiceDataElement>();
        public ObservableSortedCollection<InvoiceRawData> InvoiceRawData { set; get; } = new ObservableSortedCollection<InvoiceRawData>();

        public ObservableSortedCollection<InvoiceDataElement> InvoiceData { set; get; } = new ObservableSortedCollection<InvoiceDataElement>();

        public InvoiceBucketData InvoiceBucketData { set; get; } = new InvoiceBucketData();
        public SortedList<string,ObservableSortedCollection<SideChannelDataContainer>> SideChannelData = new SortedList<string, ObservableSortedCollection<SideChannelDataContainer>>();
        public DataRegister DataRegister = new DataRegister();
        public SortedList<string, ObservableSortedCollection<OcrClueConatiner>> OcrClue = new SortedList<string, ObservableSortedCollection<OcrClueConatiner>>();

        public ObservableSortedCollection<TradetermProfile> TradetermProfile { set; get; } = new ObservableSortedCollection<TradetermProfile>();
        public ObservableSortedCollection<CurrencyProfile> CurrencyProfiles { set; get; } = new ObservableSortedCollection<CurrencyProfile>();
        public DeclArticle DeclArtcile { set; get; } = new DeclArticle();
        public TradetermProfile CurrentTradeterm { set; get; } = null;

        public ObservableSortedCollection<InvoiceIndex> InvoiceIndex { set; get; } = new ObservableSortedCollection<InvoiceIndex>();

        public ObservableSortedCollection<InvoiceIndex> PackingListIndex { set; get; } = new ObservableSortedCollection<InvoiceIndex>();

        public DeclAccum DeclearationAccum { set; get; } = new DeclAccum();

        public InvoiceRegexPattern PackingListRegexPattern { set; get; } = new InvoiceRegexPattern();
        public InvoiceRegexPattern InvoiceRegexPattern { set; get; } = new InvoiceRegexPattern();
        public string HSMasterIndex = null;

        public bool IsHSMasterValid = false;

        public SortedList<string, ImmediateHSMaster> ImmediateHSMaster = new SortedList<string, ImmediateHSMaster>();
        public bool IsPrimeCurrencyForced { set; get; } = false;

        public string CurrentPrinterName = "";

        private int _currentInvoiceNoCntr = 0;
        public int CurrentInvoiceNoCntr {
            set
            {
                _currentInvoiceNoCntr = value;
            }
            get
            {
                return _currentInvoiceNoCntr;
            }
        }
        private string _documentNoWarning = "DocumentNoを入力してください";
        public string DocumentNoWarning 
        {
            set 
            {
                _documentNoWarning = value;
                NofityPropertyChanged("DocumentNoWarning");
            }
            get
            {
                return _documentNoWarning;
            }
        }

        private string _currentDocumentNo = "";
        public string CurrentDocumentNo
        {
            set
            {
                _currentDocumentNo = value;
                NofityPropertyChanged("CurrentDocumentNo");
                if (ConstValue.Re_ContainsExNumber.IsMatch(_currentDocumentNo))
                {
                    DocumentNoWarning = "数字のみ入力してください";
                    IsDocnoValid = false;
                    return;
                }
                if (!ConstValue.Re_Is10Numbers.IsMatch(_currentDocumentNo))
                {
                    DocumentNoWarning = "数字10桁で入力してください";
                    IsDocnoValid = false;
                    return;
                }
                DocumentNoWarning = "";
                IsDocnoValid = true;

            }
            get
            {
                return _currentDocumentNo;
            }
        }
        private string _blNoWarning = "何か入力してください(必須ではありません)";
        public string BLNoWarning
        {
            set
            {
                _blNoWarning = value;
                NofityPropertyChanged("BLNoWarning");
            }
            get
            {
                return _blNoWarning;
            }
        }
        public TextBoxAnnotation CurrentBLNoAnnot { set; get; } = null;
        private string _blNo = "";
        public string CurrentBLNo
        {
            set
            {
                _blNo = value;
                if (CurrentBLNoAnnot != null) CurrentBLNoAnnot.SetText(value);
                IsBLNoValid = true;
                if(_blNo == "")
                {
                    BLNoWarning = "何か入力してください(必須ではありません)";
                }
                else
                {
                    BLNoWarning = "";
                }
                NofityPropertyChanged("CurrentBLNo");
            }
            get
            {
                return _blNo;
            }
        }
        public bool IsDocnoValid { set; get; } = false;
        public bool IsBLNoValid { set; get; } = false;

        public SOA SOA { set; get; } = new SOA();

        public void ChangeMode(ConstValue.EditingModeEnum mode)
        {
            if (mode == ConstValue.EditingModeEnum.Ink)
            {
                Canvas.SetZIndex(App.IVEditMainWindow.InkCanvas, 2);
                App.IVEditMainWindow.InkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            }
            else
            {
                Canvas.SetZIndex(App.IVEditMainWindow.InkCanvas, -1);
                App.IVEditMainWindow.InkCanvas.EditingMode = InkCanvasEditingMode.None;
            }
            RenderController.CurrentEditingMode = mode;
        }
        private void _invoiceDataRemoved(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.OldItems == null)
            {
                return;
            }
            foreach(object item in e.OldItems)
            {
                InvoiceDataElement oldItem = item as InvoiceDataElement;
                if (oldItem == null) continue;
                InvoiceDataElement.AccumDelta(oldItem, null);
            }
        }

        private void _pageAdded(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null)
            {
                return;
            }
            foreach (object item in e.NewItems)
            {
                Page newItem = item as Page;
                if (newItem.BackgroundImage != null) {
                    int origLonger = Math.Max(newItem.BackgroundImage.PixelWidth, newItem.BackgroundImage.PixelHeight);
                    double factor = (double)Page.ThumbneilLongerPixel / (double)origLonger;
                    newItem.PageThumbneil = new PageThumbneil { PageNo = newItem.PageNo,IsInputPaper = newItem.IsInputPaper ,IsGenerated = newItem.IsGenerated,ThumbneilImage = new TransformedBitmap(newItem.BackgroundImage, new ScaleTransform(factor, factor)) ,Caption = newItem.Caption};
                }
                else
                {
                    newItem.PageThumbneil = new PageThumbneil { PageNo = newItem.PageNo, IsInputPaper = newItem.IsInputPaper, IsGenerated = newItem.IsGenerated, Caption = newItem.Caption };
                }
                PageThumbneils.Add(newItem.PageThumbneil);
            }
        }

        private void _pageRemoved(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems == null)
            {
                return;
            }
            foreach (object item in e.OldItems)
            {
                Page oldItem = item as Page;
                PageThumbneils.Remove(oldItem.PageThumbneil);
            }
        }

        public void OpenDataWindow(object sender, RoutedEventArgs e)
        {
            SubWindowController.OpenDataWindow(sender, e);
        }

        public void OpenCurrencyWindow(object sender, RoutedEventArgs e)
        {
            SubWindowController.OpenCurrencyWindow(sender, e);
        }
        public void OpenInputWindow(object sender, RoutedEventArgs e)
        {
            SubWindowController.OpenInputWindow(sender, e);
        }
        public void OpenDocnoWindow(object sender, RoutedEventArgs e)
        {
            SubWindowController.OpenDocnoWindew(sender, e);
        }
        public void LoadPdf()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "pdfファイル|*.pdf";
            string filename = "";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = dialog.FileName;
            }
            else
            {
                return;
            }
            PdfLoader p = new PdfLoader();
            var pagenum = p.GetPageNum(filename);
            int startpage = 0;
            while (startpage < Pages.Count && Pages[startpage].IsInputPaper) startpage++;
            for(int i = 0; i < pagenum; i++)
            {
                using (var bmp = p.RenderPdfBmpWithSize(filename, i, FundamentalSettings.ShorterPixels))
                {
                    int width = bmp.Width;
                    int height = bmp.Height;
                    var bmps = ImageFunc.BitmapToBitmapSource(bmp);
                    Pages.Add(new Page()
                    {
                        PageNo = startpage + i + 1,
                        OriginFile = filename,
                        OriginPage = i+1,
                        IsGenerated = false,
                        IsInputPaper = false,
                        Width = width,
                        Height = height,
                        CanvasWidth = width,
                        CanvasHeight = height,
                        BackgroundImage = bmps,
                    }) ;
                }
            }
            CurrentPage = Pages[0];
            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(FundamentalSettings.DocNoBarcodeX, FundamentalSettings.DocNoBarcodeY, FundamentalSettings.DocNoBcdWidth, FundamentalSettings.DocNoBcdHeight);

            var bcdReader = new BarcodeReader() {
                AutoRotate = true,
                TryInverted = true,
                Options =
                {
                    TryHarder = true,
                    PossibleFormats = new []{BarcodeFormat.CODE_39 }
                }
            };


            using (var firstPageBmp = p.RenderPdfBmpWithSize(filename, 0, FundamentalSettings.ShorterPixels))
            {
                using (var targetBmp = firstPageBmp.Clone(destRect, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    var result = bcdReader.Decode(targetBmp);
                    if (result != null) App.MainView.CurrentDocumentNo = result.Text;
                }
            }


            var text = p.GetTextWithinBound(filename, FundamentalSettings.HoujinStartXIn72DPI, FundamentalSettings.HoujinStartYIn72DPI, FundamentalSettings.HoujinWidthIn72DPI, FundamentalSettings.HoujinHeightIn72DPI, 1);

            var houjinPattern = new Regex(@"(?<houjin>[0-9]{13})(-[0-9]{4})?");
            var houjinMatch = houjinPattern.Match(text);
            var orgNr = houjinMatch.Groups["houjin"].Value;
            if(orgNr != "")
            {
                var company = CompanyIndicies.Find(new IVEditElasticClient.CompanyIndex { organizationno = orgNr });
                if (company != null)
                {
                    CurrentCompanyIndex = company;
                }

            }
        }
        public void SavePdf()
        {
            var sfd = new System.Windows.Forms.SaveFileDialog();
            if (App.MainView.CurrentBLNo == null || App.MainView.CurrentBLNo == "")
            {
                sfd.FileName = App.MainView.CurrentDocumentNo + ".pdf";
            }
            else
            {
                sfd.FileName = App.MainView.CurrentBLNo + ".pdf";
            }
            sfd.Filter = "pdfファイル|*.pdf";
            sfd.FilterIndex = 1;
            sfd.Title = "保存先ファイル名を指定してください";
            sfd.RestoreDirectory = true;
            string filepath = "";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filepath = sfd.FileName;
            }
            if (filepath == "")
            {
                return;
            }
            PdfWriter pdfWriter;
            try
            {
                pdfWriter = new PdfWriter(filepath);
            }catch(IOException ioe)
            {
                System.Windows.MessageBox.Show("Pdfファイルがほかのアプリによって開かれています。保存できません。");
                return;
            }
            PdfDocument pdfDocument = new PdfDocument(pdfWriter);
            pdfDocument.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);
            if (pdfDocument == null) return;
            foreach(var page in App.MainView.Pages)
            {
                AddPageAsBitmap(page, pdfDocument);
            }
            pdfDocument.Close();
        }

        private void AddPageAsBitmap(Page page, PdfDocument doc)
        {
            var canvas = new Canvas()
            {
                Width = page.Width,
                Height = page.Height,
                Background = new SolidColorBrush(Colors.White),
            };
            
            var backgroundImage = new System.Windows.Controls.Image()
            {
                Width = page.Width,
                Height = page.Height,
                Source = page.BackgroundImage,
            };
            canvas.Children.Add(backgroundImage);
            Canvas.SetLeft(backgroundImage, 0.0d);
            Canvas.SetTop(backgroundImage, 0.0d);
            Canvas.SetZIndex(backgroundImage, -1);

            var ink = new InkCanvas() {
                Width = page.Width,
                Height = page.Height,
                Strokes = page.Stroke,
                Background = new SolidColorBrush(Colors.Transparent),
            };
            canvas.Children.Add(ink);
            Canvas.SetLeft(ink, 0.0d);
            Canvas.SetTop(ink, 0.0d);
            Canvas.SetZIndex(ink, 2);

            foreach (var annot in page.Annotaion)
            {
                if (annot.GetType() == typeof(QrImageAnnotation))
                {
                    var qr = annot as QrImageAnnotation;
                    var qrImage = new System.Windows.Controls.Image()
                    {
                        Width = qr.Width,
                        Height = qr.Height,
                        Source = qr.Image,
                    };
                    canvas.Children.Add(qrImage);
                    Canvas.SetLeft(qrImage, qr.X);
                    Canvas.SetTop(qrImage, qr.Y);
                    Canvas.SetZIndex(qrImage, 0);
                }
                else if (annot.GetType() == typeof(TextBlockAnnotation))
                {
                    var tb = annot as TextBlockAnnotation;
                    if (tb.IsViewBox)
                    {
                        var viewBox = new System.Windows.Controls.Viewbox()
                        {
                            Width = tb.Width,
                            Height = tb.Height,
                        };
                        var childtextBlock = new System.Windows.Controls.TextBlock()
                        {
                            FontSize = tb.FontSize,
                            Text = tb.Text,
                        };
                        viewBox.Child = childtextBlock;
                        canvas.Children.Add(viewBox);
                        Canvas.SetLeft(viewBox, tb.X);
                        Canvas.SetTop(viewBox, tb.Y);
                        Canvas.SetZIndex(viewBox, 0);
                    }
                    else
                    {
                        var textBlock = new System.Windows.Controls.TextBlock()
                        {
                            Width = tb.Width,
                            Height = tb.Height,
                            FontSize = tb.FontSize,
                            Text = tb.Text,
                        };
                        canvas.Children.Add(textBlock);
                        Canvas.SetLeft(textBlock, tb.X);
                        Canvas.SetTop(textBlock, tb.Y);
                        Canvas.SetZIndex(textBlock, 0);
                    }
                }else if(annot.GetType() == typeof(TextBoxAnnotation))
                {
                    var tbox = annot as TextBoxAnnotation;
                    if (tbox.IsViewBox)
                    {
                        var viewBox = new System.Windows.Controls.Viewbox()
                        {
                            Width = tbox.Width,
                            Height = tbox.Height,
                        };
                        var childtextBlock = new System.Windows.Controls.TextBlock()
                        {
                            FontSize = tbox.FontSize,
                            Text = tbox.Text,
                        };
                        viewBox.Child = childtextBlock;

                        canvas.Children.Add(viewBox);
                        Canvas.SetLeft(viewBox, tbox.X);
                        Canvas.SetTop(viewBox, tbox.Y);
                        Canvas.SetZIndex(viewBox, 0);
                    }
                    else
                    {
                        var textBlock = new System.Windows.Controls.TextBlock()
                        {
                            Width = tbox.Width,
                            Height = tbox.Height,
                            FontSize = tbox.FontSize,
                            Text = tbox.Text,
                        };
                        canvas.Children.Add(textBlock);
                        Canvas.SetLeft(textBlock, tbox.X);
                        Canvas.SetTop(textBlock, tbox.Y);
                        Canvas.SetZIndex(textBlock, 0);
                    }
                }else if(annot.GetType() == typeof(LineAnnotation))
                {
                    var lineAnnot = annot as LineAnnotation;
                    var line = new System.Windows.Shapes.Line()
                    {
                        X1 = lineAnnot.StartX,
                        X2 = lineAnnot.EndX,
                        Y1 = lineAnnot.StartY,
                        Y2 = lineAnnot.EndY,
                        StrokeThickness = lineAnnot.Pitch,
                        Stroke = lineAnnot.Color,
                    };
                    canvas.Children.Add(line);
                    Canvas.SetLeft(line, lineAnnot.X);
                    Canvas.SetTop(line, lineAnnot.Y);
                    Canvas.SetZIndex(line, 0);
                }
            }
            var size = new System.Windows.Size(canvas.Width, canvas.Height);
            canvas.Measure(size);
            canvas.Arrange(new Rect(size));
            var renderBitmap = new RenderTargetBitmap((int)canvas.Width, (int)canvas.Height, 96, 96, PixelFormats.Pbgra32);
            renderBitmap.Render(canvas);
            PageSize pageSize;
            if (canvas.Height > canvas.Width) pageSize = PageSize.A4;
            else pageSize = PageSize.A4.Rotate();
            using (var ms = new MemoryStream())
            {
                var encoder = new TiffBitmapEncoder();
                encoder.Compression = TiffCompressOption.Ccitt4;
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(ms);
                byte[] buf = ms.GetBuffer();
                var itextImage = ImageDataFactory.CreateTiff(buf, true, 1, true);
                var rectangle = new iText.Kernel.Geom.Rectangle(pageSize.GetWidth(), pageSize.GetHeight());
                PdfPage newPage = doc.AddNewPage(pageSize);
                PdfCanvas pdfCanvas = new PdfCanvas(newPage);
                pdfCanvas.AddImageFittedIntoRectangle(itextImage,rectangle,false);
            }

        }

        public void LoadSettings()
        {
            string elastic = "";
            string elasticSettingPathFile = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"elasticsettingpath.ini");
            if (File.Exists(elasticSettingPathFile))
            {
                using (var sr = new StreamReader(elasticSettingPathFile, System.Text.Encoding.UTF8))
                {
                    var elasticpath = sr.ReadToEnd().Trim();
                    elastic = System.IO.Path.Combine(elasticpath, @"elastic.ini");
                }
            }
            string elastic1st = "";
            string elastic2nd = "";
            if(elastic != "" && File.Exists(elastic))
            {
                elastic1st = elastic;
                elastic2nd = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"elastic.ini");
            }
            else
            {
                elastic1st = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"elastic.ini");
            }

            if (File.Exists(elastic1st))
            {
                using (var sr = new StreamReader(elastic1st,System.Text.Encoding.UTF8))
                {
                    var ipaddr = sr.ReadToEnd().Trim();
                    ElasticClient.ElasticServiceHost = new Uri(ipaddr);
                }
            }
            else if(elastic2nd != "")
            {
                using (var sr = new StreamReader(elastic2nd, System.Text.Encoding.UTF8))
                {
                    var ipaddr = sr.ReadToEnd().Trim();
                    ElasticClient.ElasticServiceHost = new Uri(ipaddr);
                }
            }

            string printer = PrinterConfigWindowViewModel.LoadPrinterSetting();
            if (printer != "" && !ConstValue.Re_ContainsExNumberAlphabet.Match(printer).Success) App.MainView.CurrentPrinterName = printer;

        }

        public void PageNavigate(int pageNo)
        {
            CurrentPage = Pages[pageNo - 1];
        }

        public void RemoveInvoicePdf()
        {
            for(int i = App.MainView.Pages.Count-1; i >= 0; i--)
            {
                if (!App.MainView.Pages[i].IsGenerated && !App.MainView.Pages[i].IsInputPaper) App.MainView.Pages.RemoveAt(i);
            }
            App.MainView.CurrentPage = new Page();
        }
        public void ResetCompanyProfiles()
        {
            var companies = ElasticClient.GetCompanyIndicies();
            CompanyIndicies.Clear();
            foreach (var com in companies)
            {
                CompanyIndicies.Add(com);
            }
            RemoveBucketData();
        }

        public void RemoveInvoiceData()
        {
            if (InvoiceData != null)
            {
                InvoiceData.Clear();
            }
        }

        public void RemovePackingListData()
        {
            if (PackingListData != null)
            {
                PackingListData.Clear();
            }
        }

        public void RemoveBucketData()
        {
            if(InvoiceBucketData != null && InvoiceBucketData.BucketDataColumn != null)
            {
                foreach(var bucketCol in InvoiceBucketData.BucketDataColumn.Values)
                {
                    bucketCol.BucketData.Clear();
                }
            }
        }
        public void RemoveQrCodePage()
        {
            if (Pages != null)
            {
                var page = Pages.FindLast(new Page() { PageNo = int.MaxValue, IsGenerated = true });
                while (page != null && page.IsGenerated == true)
                {
                    Pages.Remove(page);
                    page = Pages.FindLast(new Page() { PageNo = int.MaxValue, IsGenerated = true });
                }

            }
        }
        public void RemoveInputPaper()
        {
            if (Pages != null)
            {
                for (int i = App.MainView.Pages.Count - 1; i >= 0; i--)
                {
                    if (Pages[i].IsInputPaper)
                    {
                        Pages.RemoveAt(i);
                    }
                }
            }
        }
        public void RemoveDeclArticle()
        {
            App.MainView.DeclArtcile = new DeclArticle();
        }

        public void RemoveDeclAccum()
        {
            App.MainView.DeclearationAccum.Clear();
        }
        public void ResetDocNo()
        {
            App.MainView.CurrentDocumentNo = "";
        }
        public void ResetBLNo()
        {
            App.MainView.CurrentBLNo = "";
            App.MainView.IsBLNoValid = false;
        }

        public void ResetDataRegister()
        {
            DataRegister.SetUp();
        }

        public void ResetIsPrimeCurrencyForced()
        {
            IsPrimeCurrencyForced = false;
        }

        public string DecidePrimeCurrency()
        {
            var bucket = new SortedList<string, int>();
            foreach(var accum in DeclearationAccum.DeclAccumDictionary)
            {
                if (!bucket.ContainsKey(accum.Value.Currency))
                {
                    bucket.Add(accum.Value.Currency, 1);
                }
                else
                {
                    bucket[accum.Value.Currency]++;
                }
            }
            var top = bucket.OrderByDescending(b => b.Value).FirstOrDefault();
            return top.Key;
        }

        public void UpdateCurrencyProfile(string Currency)
        {
            if (CurrencyProfiles.Find(new CurrencyProfile() { Code = Currency }) == null)
            {
                CurrencyProfiles.Add(new CurrencyProfile() { Code = Currency });
            }
        }

        public void UpdatePrimeCurrency(CurrencyProfile prime)
        {
            if (prime == null) return;
            foreach(var cur in CurrencyProfiles)
            {
                if(cur.Code == prime.Code)
                {
                    cur.IsPrime = true;
                }
                else
                {
                    cur.IsPrime = false;
                }
            }
        }

        public CurrencyProfile FindPrimeCurrency()
        {
            foreach(var cur in CurrencyProfiles)
            {
                if (cur.IsPrime) return cur;
            }
            return null;
        }

        public void SwitchThumbneilMouseOver(object sender)
        {
            var canvas = sender as Canvas;
            if (canvas == null) return;
            else
            {
                var tag = canvas.Tag.ToString();
                if (tag == null) return;
                int t = 0;
                if (!int.TryParse(tag, out t)) return;
                var target = PageThumbneils.Find(new PageThumbneil { PageNo = t });
                if (target == null) return;
                else target.IsMouseOver = !target.IsMouseOver;
            }
        }
        public void DeletePage(int page)
        {
            var target = PageThumbneils.Find(new PageThumbneil { PageNo = page });
            if (target == null) return;
            if (System.Windows.MessageBox.Show("消しますか？",
                    "はい",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Pages.Remove(target);
            }
        }
    }
}
