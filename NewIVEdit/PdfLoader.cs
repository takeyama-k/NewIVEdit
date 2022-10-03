using System;
using Patagames.Pdf;
using Patagames.Pdf.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using NewIVEdit;

public class PdfLoader
{
	private string licence_key = PdfiumLicence.Key;
	public PdfLoader()
	{
		PdfCommon.Initialize(licence_key);
	}
	public void GetThumbNeils(string path,ref ObservableCollection<BitmapSource> thumbneils, int size = 128)
	{
		thumbneils.Clear();
		int pageNum = GetPageNum(path);
		for (int i = 0; i < pageNum; i++)
		{
			var bmp = RenderPdfBmpWithSize(path, i, size);
			BitmapSource bs = ImageFunc.BitmapToBitmapSource(bmp);
			thumbneils.Add(bs);
		}

	}
	public int GetPageNum(string path)
	{
		PdfCommon.Initialize();
		int i = 0;
		using (var doc = PdfDocument.Load(path))
		{
			i = doc.Pages.Count;
		}
		return i;
	}
	private IntPtr _bitmapPtr;
	public IntPtr BitmapPtr
	{
		set
		{
			_bitmapPtr = value;
		}
		get
        {
			return _bitmapPtr;
		}
	}

	public Bitmap RenderPdfBmpWithSize(string path, int pagenum, int size)
	{
		var doc = Pdfium.FPDF_LoadDocument(path);
		var page = Pdfium.FPDF_LoadPage(doc, pagenum);
		double width, height;
		Pdfium.FPDF_GetPageSizeByIndex(doc, pagenum, out width, out height);

		double fac = (double)size / Math.Min(width, height);
		int nw = (int)(width * fac);
		int nh = (int)(height * fac);
		var pbmp = Pdfium.FPDFBitmap_Create(nw, nh, true);
		BitmapPtr = pbmp;
		if (pbmp == IntPtr.Zero)
		{
			Pdfium.FPDF_ClosePage(page);
			Pdfium.FPDF_CloseDocument(doc);
			return null;
		}
		Pdfium.FPDFBitmap_Clear(pbmp, 4294967295);
		Pdfium.FPDF_RenderPageBitmap(pbmp, page, 0, 0, nw, nh,
		Patagames.Pdf.Enums.PageRotate.Normal,
		Patagames.Pdf.Enums.RenderFlags.FPDF_NONE);
		var buffer = Pdfium.FPDFBitmap_GetBuffer(pbmp);
		int stride = Pdfium.FPDFBitmap_GetStride(pbmp);
		var bmp = new Bitmap(nw, nh, stride, System.Drawing.Imaging.PixelFormat.Format32bppArgb, buffer);
		Pdfium.FPDF_ClosePage(page);
		Pdfium.FPDF_CloseDocument(doc);
		return bmp;
	}

	public string GetTextWithinBound(string path ,float x, float y, float width, float height, int pageNum)
    {
		using (var doc = PdfDocument.Load(path))
		{
			PdfPage page = doc.Pages[pageNum - 1];
			//A rectangle in a user space coordinate system. Left, top, right and bottom.
			float pageHight = page.Height;
			float pageWidth = page.Width;
			float left = x;
			if (left < 0) left = 0;
			else if (left > pageWidth) left = pageWidth - 1;
			float right = x + width;
			if (right < 1) right = 1;
			else if (right > pageWidth) right = pageWidth;
			float top = pageHight - y;
			if (top < 0) top = 1.0f;
			else if (top > pageHight) top = pageHight;
			float bottom = top - height;
			if (bottom < 0) bottom = 0.0f;
			FS_RECTF rect = new FS_RECTF(left,top,right,bottom);
			string text = page.Text.GetBoundedText(rect);
			return text;
		}
	}

	public List<InvoiceOcrDataContainer> SearchLocsByText(string path, int pageNum, string[] patterns,string[] matched, float limitTop, float limitBottom, float limitLeft, float limitRight)
	{
		List<InvoiceOcrDataContainer> res = new List<InvoiceOcrDataContainer>();
		using (var doc = PdfDocument.Load(path))
		{
			PdfPage page = doc.Pages[pageNum - 1];

			float width = page.Width;
			float height = page.Height;
			float shorter = FundamentalSettings.ShorterPixels;
			double factor = shorter / Math.Min(width, height);
			int idx = 0;
			foreach (string pattern in patterns)
			{
				PdfFind found = page.Text.Find(pattern, Patagames.Pdf.Enums.FindFlags.MatchCase, 0);
				if (found == null) continue;
				do
				{

					PdfTextInfo foundText = found.FoundText;
					string text = foundText.Text;
					foreach (var rect in foundText.Rects)
					{
						if (rect.left < limitLeft || rect.right > limitRight) continue;
						if (rect.bottom < limitBottom || rect.top > limitTop) continue;
						float y = (float)(factor * (height - rect.top));
						float x = (float)(factor * rect.left);
						float pixWidth = (float)(factor * (rect.right - rect.left));
						float pixHeight = (float)(factor * (rect.top - rect.bottom));
						res.Add(new InvoiceOcrDataContainer()
						{
							PageNo = pageNum,
							Coord = new System.Windows.Point(x, y),
							Width = pixWidth,
							Height = pixHeight,
							Text = matched[idx],
						});
					}

				} while (found.FindNext());
				idx++;
			}
		}
		return res.OrderBy(x => x.Coord.Y).ThenBy(x => x.Coord.X).ToList<InvoiceOcrDataContainer>();
		
	}
}
