using System;
using NewIVEdit;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows;
using ZXing;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class InputWindowViewModel
{
	public InputWindowViewModel()
	{
	}
	public void GenerateSpecificationDoc()
	{
		int shorterpix = FundamentalSettings.ShorterPixels;
		float aspectFactor = FundamentalSettings.LongerPixelsIn72DPI / FundamentalSettings.ShorterPixelsIn72DPI;
		int longerpix = (int)((float)shorterpix * aspectFactor);
		int _pageCntr = 1;
		int titleMargin = 10;
		int bottomMargin =10;
		int pageTitleHeight = 120;
		int titleHeight = 100;
		int lineHeight = 55;
		int fontsize = lineHeight - 10;
		int titleFontSize = titleHeight - 10;
		int margin = 5;
		int _y = margin;
		var page = new Page()
		{
			IsInputPaper = true,
			Width = shorterpix,
			Height = longerpix,
			CanvasWidth = shorterpix,
			CanvasHeight = longerpix,
			PageNo = _pageCntr,
			Caption = "明細書",
		};
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 5,
			Y = _y,
			FontSize = 90,
			Width = 700,
			Height = pageTitleHeight,
			Text = "HS別明細書",
		});
		_y = pageTitleHeight + 5 + titleMargin;
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 20,
			Y = _y,
			FontSize = titleFontSize,
			Width = 80,
			Height = titleHeight,
			Text = "No.",
			IsViewBox = true
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 100,
			Y = _y,
			FontSize = titleFontSize,
			Width = 375,
			Height = titleHeight,
			Text = "商品CD",
			IsViewBox = true
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 480,
			Y = _y,
			FontSize = titleFontSize,
			Width = 475,
			Height = titleHeight,
			Text = "説明",
			IsViewBox = true
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 960,
			Y = _y,
			FontSize = titleFontSize,
			Width = 200,
			Height = titleHeight,
			Text = "N/W",
			IsViewBox = true,
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 1165,
			Y = _y,
			FontSize = titleFontSize,
			Width = 200,
			Height = titleHeight,
			Text = "Qty",
			IsViewBox = true,

		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 1370,
			Y = _y,
			FontSize = titleFontSize,
			Width = 300,
			Height = titleHeight,
			Text = "Amount",
			IsViewBox = true
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 1675,
			Y = _y,
			FontSize = titleFontSize,
			Width = 250,
			Height = titleHeight,
			Text = "HSCODE",
			IsViewBox = true,
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 1930,
			Y = _y,
			FontSize = titleFontSize,
			Width = 100,
			Height = titleHeight,
			Text = "OR",
			IsViewBox = true
		});
		_y += titleHeight + titleMargin;
		int prevIndex = 0;
		var invData = App.MainView.InvoiceData.OrderBy(item => App.MainView.DeclearationAccum.DeclAccumDictionary[item.Hash()].DeclIndex).ThenBy(item => item.IndexNo);
		long ttlNw = 0L;
		double dttlNw = 0.0;
		string sttlNw = "";
		long ttlQty = 0L;
		double dttlQty = 0.0;
		string sttlQty = "";
		long ttlAmt = 0L;
		double dttlAmt = 0.0;
		string sttlAmt = "";
		
		foreach(var data in invData)
        {
			if (prevIndex != 0 && App.MainView.DeclearationAccum.DeclAccumDictionary[data.Hash()].DeclIndex != prevIndex)
			{
				if (_y + lineHeight + margin * 4 > page.Height)
				{
					_y = margin;
					App.MainView.Pages.Add(page);
					page = new Page()
					{
						IsInputPaper = true,
						Width = shorterpix,
						Height = longerpix,
						CanvasWidth = shorterpix,
						CanvasHeight = longerpix,
						PageNo = ++_pageCntr,
						Caption = "明細書",
					};
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 5,
						Y = _y,
						FontSize = 90,
						Width = 700,
						Height = pageTitleHeight,
						Text = "HS別明細書",
					});
					_y = pageTitleHeight + titleMargin;
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 20,
						Y = _y,
						FontSize = titleFontSize,
						Width = 80,
						Height = titleHeight,
						Text = "No.",
						IsViewBox = true
					});
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 100,
						Y = _y,
						FontSize = titleFontSize,
						Width = 375,
						Height = titleHeight,
						Text = "商品CD",
						IsViewBox = true
					});
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 480,
						Y = _y,
						FontSize = titleFontSize,
						Width = 475,
						Height = titleHeight,
						Text = "説明",
						IsViewBox = true
					});
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 960,
						Y = _y,
						FontSize = titleFontSize,
						Width = 200,
						Height = titleHeight,
						Text = "N/W",
						IsViewBox = true,
					});
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 1165,
						Y = _y,
						FontSize = titleFontSize,
						Width = 200,
						Height = titleHeight,
						Text = "Qty",
						IsViewBox = true,

					});
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 1370,
						Y = _y,
						FontSize = titleFontSize,
						Width = 300,
						Height = titleHeight,
						Text = "Amount",
						IsViewBox = true
					});
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 1675,
						Y = _y,
						FontSize = titleFontSize,
						Width = 250,
						Height = titleHeight,
						Text = "HSCODE",
						IsViewBox = true,
					});
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 1930,
						Y = _y,
						FontSize = titleFontSize,
						Width = 100,
						Height = titleHeight,
						Text = "OR",
						IsViewBox = true
					});
					_y += titleHeight + titleMargin;
				}
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 480,
					Y = _y,
					FontSize = fontsize,
					Width = 475,
					Height = lineHeight,
					Text = "合計",
					IsViewBox = true
				});
				dttlNw = ttlNw / (double)ConstValue.InternalValueFactor;
				sttlNw = String.Format("{0:0.0000}", dttlNw);
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 960,
					Y = _y,
					FontSize = fontsize,
					Width = 200,
					Height = lineHeight,
					Text = sttlNw,
					IsViewBox = false,
				});
				dttlQty = ttlQty / (double)ConstValue.InternalValueFactor;
				sttlQty = String.Format("{0:0}", dttlQty);
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 1165,
					Y = _y,
					FontSize = fontsize,
					Width = 200,
					Height = lineHeight,
					Text = sttlQty,
					IsViewBox = false,

				});
				dttlAmt = ttlAmt / (double)ConstValue.InternalValueFactor;
				sttlAmt = String.Format("{0:0.00}", dttlAmt);
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 1370,
					Y = _y,
					FontSize = fontsize,
					Width = 300,
					Height = lineHeight,
					Text = sttlAmt,
					IsViewBox = false
				});
				ttlNw = 0L;
				ttlQty = 0L;
				ttlAmt = 0L;
				_y += lineHeight + margin * 4;

			}
			var decl = App.MainView.DeclearationAccum.DeclAccumDictionary[data.Hash()];
			if (App.MainView.DeclearationAccum.DeclAccumDictionary[data.Hash()].DeclIndex != prevIndex)
            {
				if (_y + titleHeight + titleMargin > page.Height)
				{
					_y = margin;
					App.MainView.Pages.Add(page);
					page = new Page()
					{
						IsInputPaper = true,
						Width = shorterpix,
						Height = longerpix,
						CanvasWidth = shorterpix,
						CanvasHeight = longerpix,
						PageNo = ++_pageCntr,
						Caption = "明細書",
					};
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 5,
						Y = _y,
						FontSize = 90,
						Width = 700,
						Height = pageTitleHeight,
						Text = "HS別明細書",
					});
					_y = pageTitleHeight + titleMargin;
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 20,
						Y = _y,
						FontSize = titleFontSize,
						Width = 80,
						Height = titleHeight,
						Text = "No.",
						IsViewBox = true
					});
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 100,
						Y = _y,
						FontSize = titleFontSize,
						Width = 375,
						Height = titleHeight,
						Text = "商品CD",
						IsViewBox = true
					});
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X =480,
						Y = _y,
						FontSize = titleFontSize,
						Width = 475,
						Height = titleHeight,
						Text = "説明",
						IsViewBox = true
					});
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 960,
						Y = _y,
						FontSize = titleFontSize,
						Width = 200,
						Height = titleHeight,
						Text = "N/W",
						IsViewBox = true,
					});
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 1165,
						Y = _y,
						FontSize = titleFontSize,
						Width = 200,
						Height = titleHeight,
						Text = "Qty",
						IsViewBox = true,

					});
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 1370,
						Y = _y,
						FontSize = titleFontSize,
						Width = 300,
						Height = titleHeight,
						Text = "Amount",
						IsViewBox = true
					});
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 1675,
						Y = _y,
						FontSize = titleFontSize,
						Width = 250,
						Height = titleHeight,
						Text = "HSCODE",
						IsViewBox = true,
					});
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 1930,
						Y = _y,
						FontSize = titleFontSize,
						Width = 100,
						Height = titleHeight,
						Text = "OR",
						IsViewBox = true
					});
					_y += titleHeight + titleMargin;
				}
				string text = decl.DeclIndex + "アイテム目:HS:" + decl.HSCode + " " + decl.Postfix + "通貨:" + decl.Currency;
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 20,
					Y = _y,
					FontSize = 70,
					Width = 2000,
					Height = 100,
					Text = text,
					IsViewBox = true
				});
				_y += titleHeight + margin;
			}
			if (_y + lineHeight + margin > page.Height)
			{
				_y = margin;
				App.MainView.Pages.Add(page);
				page = new Page()
				{
					IsInputPaper = true,
					Width = shorterpix,
					Height = longerpix,
					CanvasWidth = shorterpix,
					CanvasHeight = longerpix,
					PageNo = ++_pageCntr,
					Caption = "明細書",
				};
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 5,
					Y = _y,
					FontSize = 90,
					Width = 700,
					Height = pageTitleHeight,
					Text = "HS別明細書",
				});
				_y = pageTitleHeight + titleMargin;
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 20,
					Y = _y,
					FontSize = titleFontSize,
					Width = 80,
					Height = titleHeight,
					Text = "No.",
					IsViewBox = true
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 100,
					Y = _y,
					FontSize = titleFontSize,
					Width = 375,
					Height = titleHeight,
					Text = "商品CD",
					IsViewBox = true
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 480,
					Y = _y,
					FontSize = titleFontSize,
					Width = 475,
					Height = titleHeight,
					Text = "説明",
					IsViewBox = true
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 960,
					Y = _y,
					FontSize = titleFontSize,
					Width = 200,
					Height = titleHeight,
					Text = "N/W",
					IsViewBox = true,
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 1165,
					Y = _y,
					FontSize = titleFontSize,
					Width = 200,
					Height = titleHeight,
					Text = "Qty",
					IsViewBox = true,

				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 1370,
					Y = _y,
					FontSize = titleFontSize,
					Width = 300,
					Height = titleHeight,
					Text = "Amount",
					IsViewBox = true
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 1675,
					Y = _y,
					FontSize = titleFontSize,
					Width = 250,
					Height = titleHeight,
					Text = "HSCODE",
					IsViewBox = true
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 1930,
					Y = _y,
					FontSize = titleFontSize,
					Width = 100,
					Height = titleHeight,
					Text = "OR",
					IsViewBox = true
				});
				_y += titleHeight + titleMargin;
			}
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 20,
				Y = _y,
				FontSize = fontsize,
				Width = 80,
				Height = lineHeight,
				Text = (data.IndexNo+1).ToString(),
				IsViewBox = false
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 100,
				Y = _y,
				FontSize = fontsize,
				Width = 375,
				Height = lineHeight,
				Text = data.HSCodeKey,
				IsViewBox = false
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 480,
				Y = _y,
				FontSize = fontsize,
				Width = 475,
				Height = lineHeight,
				Text = data.Description,
				IsViewBox = false
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 960,
				Y = _y,
				FontSize = fontsize,
				Width = 200,
				Height = lineHeight,
				Text = String.Format("{0:0.0000}", data.NetWeight),
				IsViewBox = false,
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 1165,
				Y = _y,
				FontSize = fontsize,
				Width = 200,
				Height = lineHeight,
				Text = String.Format("{0:0.00}", data.Number),
				IsViewBox = false,

			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 1370,
				Y = _y,
				FontSize = fontsize,
				Width = 300,
				Height = lineHeight,
				Text = String.Format("{0:0.000}", data.Amount),
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 1675,
				Y = _y,
				FontSize = fontsize,
				Width = 250,
				Height = lineHeight,
				Text = data.HSCode,
				IsViewBox = false,
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 1930,
				Y = _y,
				FontSize = fontsize,
				Width = 100,
				Height = lineHeight,
				Text = data.Origin,
			});
			_y += lineHeight + titleMargin;
			prevIndex = decl.DeclIndex;
			ttlAmt += data.AmountInternal;
			ttlNw += data.NetWeightInternal;
			ttlQty += data.NumberInternal;
		}
		if (_y + lineHeight + margin * 4 > page.Height)
		{
			_y = margin;
			App.MainView.Pages.Add(page);
			page = new Page()
			{
				IsInputPaper = true,
				Width = shorterpix,
				Height = longerpix,
				CanvasWidth = shorterpix,
				CanvasHeight = longerpix,
				PageNo = ++_pageCntr,
				Caption = "明細書",
			};
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 5,
				Y = _y,
				FontSize = 90,
				Width = 700,
				Height = pageTitleHeight,
				Text = "HS別明細書",
			});
			_y = pageTitleHeight + titleMargin;
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 20,
				Y = _y,
				FontSize = titleFontSize,
				Width = 80,
				Height = titleHeight,
				Text = "No.",
				IsViewBox = true
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 100,
				Y = _y,
				FontSize = titleFontSize,
				Width = 375,
				Height = titleHeight,
				Text = "商品CD",
				IsViewBox = true
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 480,
				Y = _y,
				FontSize = titleFontSize,
				Width = 475,
				Height = titleHeight,
				Text = "説明",
				IsViewBox = true
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 960,
				Y = _y,
				FontSize = titleFontSize,
				Width = 200,
				Height = titleHeight,
				Text = "N/W",
				IsViewBox = true,
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 1165,
				Y = _y,
				FontSize = titleFontSize,
				Width = 200,
				Height = titleHeight,
				Text = "Qty",
				IsViewBox = true,

			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 1370,
				Y = _y,
				FontSize = titleFontSize,
				Width = 300,
				Height = titleHeight,
				Text = "Amount",
				IsViewBox = true
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 1675,
				Y = _y,
				FontSize = titleFontSize,
				Width = 250,
				Height = titleHeight,
				Text = "HSCODE",
				IsViewBox = true,
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 1930,
				Y = _y,
				FontSize = titleFontSize,
				Width = 100,
				Height = titleHeight,
				Text = "OR",
				IsViewBox = true
			});
			_y += titleHeight + titleMargin;
		}
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 480,
			Y = _y,
			FontSize = fontsize,
			Width = 475,
			Height = lineHeight,
			Text = "合計",
			IsViewBox = true
		});
		dttlNw = ttlNw / (double)ConstValue.InternalValueFactor;
		sttlNw = String.Format("{0:0.0000}", dttlNw);
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 960,
			Y = _y,
			FontSize = fontsize,
			Width = 200,
			Height = lineHeight,
			Text = sttlNw,
			IsViewBox = false,
		});
		dttlQty = ttlQty / (double)ConstValue.InternalValueFactor;
		sttlQty = String.Format("{0:0}", dttlQty);
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 1165,
			Y = _y,
			FontSize = fontsize,
			Width = 200,
			Height = lineHeight,
			Text = sttlQty,
			IsViewBox = false,

		});
		dttlAmt = ttlAmt / (double)ConstValue.InternalValueFactor;
		sttlAmt = String.Format("{0:0.00}", dttlAmt);
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 1370,
			Y = _y,
			FontSize = fontsize,
			Width = 300,
			Height = lineHeight,
			Text = sttlAmt,
			IsViewBox = false
		});
		ttlNw = 0L;
		ttlQty = 0L;
		ttlAmt = 0L;
		_y += lineHeight + margin * 4;
		App.MainView.Pages.Add(page);
		return;

	}
	public void GenerateStateOfAccountDoc()
    {
		int shorterpix = FundamentalSettings.ShorterPixels;
		float factor = FundamentalSettings.LongerPixelsIn72DPI / FundamentalSettings.ShorterPixelsIn72DPI;
		int longerpix = (int)((float)shorterpix * factor);
		int _pageCntr = 1;
		var page = new Page()
		{
			IsInputPaper = true,
			Width = shorterpix,
			Height = longerpix,
			CanvasWidth = shorterpix,
			CanvasHeight = longerpix,
			PageNo = _pageCntr,
			Caption = "計算書",
		};
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 5,
			Y = 5,
			FontSize = 90,
			Width = 1000,
			Height = 110,
			Text = "アイテム順　HS一覧",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 20,
			Y = 120,
			FontSize = 70,
			Width = 200,
			Height = 80,
			Text = "商品CD",
			IsViewBox = true
		}) ;
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 225,
			Y = 120,
			FontSize = 70,
			Width = 300,
			Height = 80,
			Text = "説明",
			IsViewBox = true
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 535,
			Y = 120,
			FontSize = 70,
			Width = 200,
			Height = 80,
			Text = "N/W",
			IsViewBox = true
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 740,
			Y = 120,
			FontSize = 70,
			Width = 200,
			Height = 80,
			Text = "Qty",
			IsViewBox = true
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 945,
			Y = 120,
			FontSize = 70,
			Width = 300,
			Height = 80,
			Text = "Amount",
			IsViewBox = true
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 1250,
			Y = 120,
			FontSize = 70,
			Width = 200,
			Height = 80,
			Text = "通貨",
			IsViewBox = true
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 1455,
			Y = 120,
			FontSize = 70,
			Width = 100,
			Height = 80,
			Text = "欄",
			IsViewBox = true,
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 1555,
			Y = 120,
			FontSize = 70,
			Width = 250,
			Height = 80,
			Text = "HS",
			IsViewBox = true
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 1815,
			Y = 120,
			FontSize = 70,
			Width = 100,
			Height = 80,
			Text = "OR",
			IsViewBox = true
		});

		int _y = 205;
		int _cntr = 0;
		int rowheight = 45;
		int ttlRowHeight = 60;
		int margin = 5;
		int fontsize = (int)(rowheight * 0.8);
		int ttlFontSize = (int)(ttlRowHeight * 0.8);
		long ttlAmt = 0L;
		long ttlQty = 0L;
		long ttlNetWt = 0L;
		foreach (var invdata in App.MainView.InvoiceData)
        {
			ttlAmt += invdata.AmountInternal;
			ttlQty += invdata.NumberInternal;
			ttlNetWt += invdata.NetWeightInternal;

			string prodCode = invdata.HSCodeKey;
			string description = invdata.Description;
			string netWeight = invdata.NetWeight;
			string quantitiy = invdata.Number;
			string amount = invdata.Amount;
			string currency = invdata.Currency;
			string declIdx = "";
            if (App.MainView.DeclearationAccum.DeclAccumDictionary.ContainsKey(invdata.Hash()))
            {
				declIdx = App.MainView.DeclearationAccum.DeclAccumDictionary[invdata.Hash()].DeclIndex.ToString();
			}
			string hs = invdata.HSCode;
			string or = invdata.Origin;
			
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 20,
				Y = _y + _cntr* (rowheight + margin),
				FontSize = fontsize,
				Width = 200,
				Height = rowheight,
				Text = prodCode,
				IsViewBox = true
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 225,
				Y = _y + _cntr * (rowheight + margin),
				FontSize = fontsize,
				Width = 300,
				Height = rowheight,
				Text = description,
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 535,
				Y = _y + _cntr * (rowheight + margin),
				FontSize = fontsize,
				Width = 200,
				Height = rowheight,
				Text = netWeight,
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 740,
				Y = _y + _cntr * (rowheight + margin),
				FontSize = fontsize,
				Width = 200,
				Height = rowheight,
				Text = quantitiy,
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 945,
				Y = _y + _cntr * (rowheight + margin),
				FontSize = fontsize,
				Width = 300,
				Height = rowheight,
				Text = amount,
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 1250,
				Y = _y + _cntr * (rowheight + margin),
				FontSize = fontsize,
				Width = 200,
				Height = rowheight,
				Text = currency,
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 1455,
				Y = _y + _cntr * (rowheight + margin),
				FontSize = fontsize,
				Width = 100,
				Height = rowheight,
				Text = declIdx,
				IsViewBox = true,
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 1555,
				Y = _y + _cntr * (rowheight+margin),
				FontSize = fontsize,
				Width = 250,
				Height = rowheight,
				Text = hs,
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 1815,
				Y = _y + _cntr * (rowheight+margin),
				FontSize = fontsize,
				Width = 100,
				Height = rowheight,
				Text = or,
			});
			_cntr++;
			if(_y + (_cntr + 5) * (rowheight + margin) > page.Height)
            {
				App.MainView.Pages.Add(page);
				page = new Page()
				{
					IsInputPaper = true,
					Width = shorterpix,
					Height = longerpix,
					CanvasWidth = shorterpix,
					CanvasHeight = longerpix,
					PageNo = ++_pageCntr,
					Caption = "計算書",
				};
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 5,
					Y = 5,
					FontSize = 90,
					Width = 1000,
					Height = 110,
					Text = "アイテム順 HS一覧",
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 20,
					Y = 120,
					FontSize = 70,
					Width = 200,
					Height = 80,
					Text = "商品CD",
					IsViewBox = true
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 225,
					Y = 120,
					FontSize = 70,
					Width = 300,
					Height = 80,
					Text = "説明",
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 535,
					Y = 120,
					FontSize = 70,
					Width = 200,
					Height = 80,
					Text = "N/W",
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 740,
					Y = 120,
					FontSize = 70,
					Width = 200,
					Height = 80,
					Text = "Qty",
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 945,
					Y = 120,
					FontSize = 70,
					Width = 300,
					Height = 80,
					Text = "Amount",
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 1250,
					Y = 120,
					FontSize = 70,
					Width = 100,
					Height = 80,
					Text = "通貨",
					IsViewBox = true,
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 1355,
					Y = 120,
					FontSize = 70,
					Width = 200,
					Height = 80,
					Text = "申告欄",
					IsViewBox = true,
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 1555,
					Y = 120,
					FontSize = 70,
					Width = 250,
					Height = 80,
					Text = "HS",
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 1815,
					Y = 120,
					FontSize = 70,
					Width = 100,
					Height = 80,
					Text = "OR",
				});
				_y = 205;
				_cntr = 0;
			}
		}
		_y = _y + _cntr * (rowheight + margin);
		_y += 20;

		if (_y + ttlRowHeight > page.Height)
        {
			_y = 105;
			App.MainView.Pages.Add(page);
			page = new Page()
			{
				IsInputPaper = true,
				Width = shorterpix,
				Height = longerpix,
				CanvasWidth = shorterpix,
				CanvasHeight = longerpix,
				PageNo = ++_pageCntr,
				Caption = "計算書",
			};
		}

		string ttlNetWtStr = string.Format("{0:0.000}", ttlNetWt / (double)ConstValue.InternalValueFactor);
		string ttlQtyStr = string.Format("{0:0.000}", ttlQty / (double)ConstValue.InternalValueFactor);
		string ttlAmtStr = string.Format("{0:0.000}", ttlAmt / (double)ConstValue.InternalValueFactor);


		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 5,
			Y = _y,
			FontSize = ttlFontSize,
			Width = 300,
			Height = ttlRowHeight,
			Text = "総合計:",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 740,
			Y = _y,
			FontSize = ttlFontSize,
			Width = 200,
			Height = ttlRowHeight,
			Text = ttlQtyStr,
			IsViewBox = true
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 945,
			Y = _y,
			FontSize = ttlFontSize,
			Width = 200,
			Height = ttlRowHeight,
			Text = ttlAmtStr,
			IsViewBox = true
		});

		if (_cntr > 0)
		{
			App.MainView.Pages.Add(page);
		}

		return;

	}
	public void GenerateInputPaper()
    {
		int shorterpix = FundamentalSettings.ShorterPixels;
		float factor = FundamentalSettings.LongerPixelsIn72DPI / FundamentalSettings.ShorterPixelsIn72DPI;
		int longerpix = (int)((float)shorterpix * factor);
		int _pageCntr = 1;
		var page = new Page()
		{
			IsInputPaper = true,
			Width = shorterpix,
			Height = longerpix,
			CanvasWidth = shorterpix,
			CanvasHeight = longerpix,
			PageNo = _pageCntr,
			Caption = "入力用紙",
		};
		//BL No入力欄
		var blNo = new TextBoxAnnotation()
		{
			X = 1400,
			Y = 10,
			Width = 536,
			Height = 90,
			FontSize = 80,
		};
		blNo.BindTo += (string val) => {
			App.MainView.CurrentBLNo = val;
		};
		App.MainView.CurrentBLNoAnnot = blNo;
		page.Annotaion.Add(blNo);
		blNo.SetText(App.MainView.CurrentBLNo);


		//Header枠線
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 105,
			EndX = 1936,
			EndY = 105,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 105,
			EndX = 105,
			EndY = 1855,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 1855,
			EndX = 1936,
			EndY = 1855,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 1936,
			StartY = 105,
			EndX = 1936,
			EndY = 1855,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 499,
			StartY = 105,
			EndX = 499,
			EndY = 1855,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 215,
			EndX = 1936,
			EndY = 215,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 325,
			EndX = 1936,
			EndY = 325,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 655,
			EndX = 1540,
			EndY = 655,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 765,
			EndX = 1936,
			EndY = 765,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 875,
			EndX = 778,
			EndY = 875,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 985,
			EndX = 1936,
			EndY = 985,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 1130,
			EndX = 1936,
			EndY = 1130,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 1275,
			EndX = 1936,
			EndY = 1275,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 1420,
			EndX = 1936,
			EndY = 1420,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 1565,
			EndX = 1936,
			EndY = 1565,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 1637,
			EndX = 1936,
			EndY = 1637,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 1710,
			EndX = 1936,
			EndY = 1710,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 1855,
			EndX = 1936,
			EndY = 1855,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 778,
			StartY = 105,
			EndX = 778,
			EndY = 655,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 778,
			StartY = 875,
			EndX = 778,
			EndY = 985,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 1540,
			StartY = 215,
			EndX = 1540,
			EndY = 765,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 1088,
			StartY = 105,
			EndX = 1088,
			EndY = 215,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 1398,
			StartY = 105,
			EndX = 1398,
			EndY = 215,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 1243,
			StartY = 215,
			EndX = 1243,
			EndY = 325,
			Pitch = 3,
		});
		//文字
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 115,
			Y = 105,
			FontSize = 75,
			Width = 489,
			Height = 110,
			Text = "種 別",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 115,
			Y = 215,
			FontSize = 75,
			Width = 489,
			Height = 110,
			Text = "積込港",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 115,
			Y = 430,
			FontSize = 75,
			Width = 489,
			Height = 110,
			Text = "承認証番号",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 115,
			Y = 655,
			FontSize = 75,
			Width = 489,
			Height = 110,
			Text = "INV No.",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 115,
			Y = 765,
			FontSize = 75,
			Width = 489,
			Height = 110,
			Text = "仕 入 書",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 115,
			Y = 875,
			FontSize = 65,
			Width = 489,
			Height = 110,
			Text = "INV No 入力",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 115,
			Y = 985,
			FontSize = 65,
			Width = 489,
			Height = 145,
			Text = "税関1",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 115,
			Y = 1130,
			FontSize = 65,
			Width = 489,
			Height = 145,
			Text = "税関2",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 115,
			Y = 1275,
			FontSize = 65,
			Width = 489,
			Height = 145,
			Text = "通関",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 115,
			Y = 1420,
			FontSize = 65,
			Width = 489,
			Height = 145,
			Text = "荷主",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 115,
			Y = 1565,
			FontSize = 40,
			Width = 489,
			Height = 72,
			Text = "荷主SEC",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 115,
			Y = 1637,
			FontSize = 40,
			Width = 489,
			Height = 72,
			Text = "荷主REF No.",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 115,
			Y = 1710,
			FontSize = 75,
			Width = 489,
			Height = 145,
			Text = "少額品名",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 788,
			Y = 160,
			FontSize = 45,
			Width = 120,
			Height = 55,
			Text = "税関:",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 1098,
			Y = 160,
			FontSize = 45,
			Width = 120,
			Height = 55,
			Text = "部門:",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 1398,
			Y = 160,
			FontSize = 45,
			Width = 200,
			Height = 55,
			Text = "貿易形態:",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 788,
			Y = 265,
			FontSize = 45,
			Width = 150,
			Height = 55,
			Text = "仕向地:",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 1253,
			Y = 265,
			FontSize = 45,
			Width = 150,
			Height = 55,
			Text = "要搭確:",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 1550,
			Y = 265,
			FontSize = 45,
			Width = 200,
			Height = 55,
			Text = "外為関係:",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 1550,
			Y = 370,
			FontSize = 45,
			Width = 200,
			Height = 55,
			Text = "按分N/W:",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 1650,
			Y = 620,
			FontSize = 75,
			Width = 100,
			Height = 105,
			Text = "KG",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 509,
			Y = 820,
			FontSize = 45,
			Width = 140,
			Height = 55,
			Text = "決済:",
		});

		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 679,
			Y = 875,
			Width = 100,
			Height = 100,
			FontSize = 65,
			Text = "件",
		});

		//form
		var declType = new TextBoxAnnotation()
		{
			X = 515,
			Y = 107,
			Width = 260,
			Height = 103,
			FontSize = 75,
		};
		declType.BindTo += (string val) => {
			App.MainView.DeclArtcile.DeclType = val;
		};
		App.MainView.DeclArtcile.DeclTypeAnnot = declType;
		page.Annotaion.Add(declType);
		declType.SetText(App.MainView.DeclArtcile.DeclType);

		var zeikan = new TextBoxAnnotation()
		{
			X = 918,
			Y = 107,
			Width = 130,
			Height = 103,
			FontSize = 75,
		};
		zeikan.BindTo += (string val) => {
			App.MainView.DeclArtcile.DeclSubmiss = val;
		};
		App.MainView.DeclArtcile.DeclSubmissAnnot = zeikan;
		page.Annotaion.Add(zeikan);
		zeikan.SetText(App.MainView.DeclArtcile.DeclSubmiss);

		var bumon = new TextBoxAnnotation()
		{
			X = 1228,
			Y = 107,
			Width = 130,
			Height = 103,
			FontSize = 75,
		};
		bumon.BindTo += (string val) => {
			App.MainView.DeclArtcile.DeclSubmissBumon = val;
		};
		App.MainView.DeclArtcile.DeclSubmissBumonAnnot = bumon;
		page.Annotaion.Add(bumon);
		bumon.SetText(App.MainView.DeclArtcile.DeclSubmissBumon);
		var bouekiType = new TextBoxAnnotation()
		{
			X = 1608,
			Y = 107,
			Width = 320,
			Height = 103,
			FontSize = 75,
		};
		bouekiType.BindTo += (string val) => {
			App.MainView.DeclArtcile.BouekiType = val;
		};
		App.MainView.DeclArtcile.BouekiTypeAnnot = bouekiType;
		page.Annotaion.Add(bouekiType);
		bouekiType.SetText(App.MainView.DeclArtcile.BouekiType);

		var portOfDept = new TextBoxAnnotation()
		{
			X = 509,
			Y = 217,
			Width = 260,
			Height = 103,
			FontSize = 75,
		};
		portOfDept.BindTo += (string val) => {
			App.MainView.DeclArtcile.PortOfDept = val;
		};
		App.MainView.DeclArtcile.PortOfDeptAnnot = portOfDept;
		page.Annotaion.Add(portOfDept);
		portOfDept.SetText(App.MainView.DeclArtcile.PortOfDept);

		var destination = new TextBoxAnnotation()
		{
			X = 948,
			Y = 245,
			Width = 292,
			Height = 80,
			FontSize = 65,
			IsViewBox = true,
		};
		destination.BindTo += (string val) => {
			App.MainView.DeclArtcile.Destination = val;
		};
		App.MainView.DeclArtcile.DestinationAnnot = destination;
		page.Annotaion.Add(destination);
		destination.SetText(App.MainView.DeclArtcile.Destination);

		var youtoukaku = new TextBoxAnnotation()
		{
			X = 1413,
			Y = 217,
			Width = 130,
			Height = 103,
			FontSize = 75,
		};
		youtoukaku.BindTo += (string val) => {
			App.MainView.DeclArtcile.Youtoukaku = val;
		};
		App.MainView.DeclArtcile.YoutoukakuAnnot = youtoukaku;
		page.Annotaion.Add(youtoukaku);
		youtoukaku.SetText(App.MainView.DeclArtcile.Youtoukaku);

		var gaitameCode = new TextBoxAnnotation()
		{
			X = 1780,
			Y = 217,
			Width = 150,
			Height = 103,
			FontSize = 75,
		};
		gaitameCode.BindTo += (string val) => {
			App.MainView.DeclArtcile.GaitameCode = val;
		};
		App.MainView.DeclArtcile.GaitameCodeAnnot = gaitameCode;
		page.Annotaion.Add(gaitameCode);
		gaitameCode.SetText(App.MainView.DeclArtcile.GaitameCode);

		var ivNetWeight = new TextBoxAnnotation()
		{
			X = 1550,
			Y = 496,
			Width = 383,
			Height = 103,
			FontSize = 75,
		};
		ivNetWeight.BindTo += (string val) => {
			App.MainView.DeclArtcile.IVNetWeight = val;
		};
		App.MainView.DeclArtcile.IVNetWeightAnnot = ivNetWeight;
		page.Annotaion.Add(ivNetWeight);
		ivNetWeight.SetText(App.MainView.DeclArtcile.IVNetWeight);

		var invNo = new TextBoxAnnotation()
		{
			X = 509,
			Y = 658,
			Width = 1021,
			Height = 103,
			FontSize = 75,
			IsViewBox = true,
		};
		invNo.BindTo += (string val) => {
			App.MainView.DeclArtcile.InvNo = val;
		};
		App.MainView.DeclArtcile.InvNoAnnot = invNo;
		page.Annotaion.Add(invNo);
		invNo.SetText(App.MainView.DeclArtcile.InvNo);

		var paymentCode = new TextBoxAnnotation()
		{
			X = 655,
			Y = 768,
			Width = 130,
			Height = 103,
			FontSize = 75,
			IsViewBox = true,
		};
		paymentCode.BindTo += (string val) => {
			App.MainView.DeclArtcile.PaymentCode = val;
		};
		App.MainView.DeclArtcile.PaymentCodeAnnot = paymentCode;
		page.Annotaion.Add(paymentCode);
		paymentCode.SetText(App.MainView.DeclArtcile.PaymentCode);

		var tradeTerm = new TextBoxAnnotation()
		{
			X = 788,
			Y = 768,
			Width = 210,
			Height = 103,
			FontSize = 75,
			IsViewBox = true,
		};
		tradeTerm.BindTo += (string val) => {
			App.MainView.DeclArtcile.TradeTerm = val;
		};
		App.MainView.DeclArtcile.TradeTermAnnot = tradeTerm;
		page.Annotaion.Add(tradeTerm);
		tradeTerm.SetText(App.MainView.DeclArtcile.TradeTerm);

		var currency = new TextBoxAnnotation()
		{
			X = 1018,
			Y = 768,
			Width = 210,
			Height = 103,
			FontSize = 75,
			IsViewBox = true,
		};
		currency.BindTo += (string val) => {
			App.MainView.DeclArtcile.Currency = val;
		};
		App.MainView.DeclArtcile.CurrencyAnnot = currency;
		page.Annotaion.Add(currency);
		currency.SetText(App.MainView.DeclArtcile.Currency);

		var currency2 = new TextBoxAnnotation()
		{
			X = 1018,
			Y = 873,
			Width = 210,
			Height = 103,
			FontSize = 75,
			IsViewBox = true,
		};
		currency2.BindTo += (string val) => {
			App.MainView.DeclArtcile.Currency = val;
		};
		App.MainView.DeclArtcile.CurrencyAnnot2 = currency2;
		page.Annotaion.Add(currency2);
		currency2.SetText(App.MainView.DeclArtcile.Currency);

		var termAmount = new TextBoxAnnotation()
		{
			X = 1238,
			Y = 768,
			Width = 688,
			Height = 103,
			FontSize = 65,
			IsViewBox = true,
		};
		termAmount.BindTo += (string val) => {
			App.MainView.DeclArtcile.TermAmount = val;
		};
		App.MainView.DeclArtcile.TermAmountAnnot = termAmount;
		page.Annotaion.Add(termAmount);
		termAmount.SetText(App.MainView.DeclArtcile.TermAmount);

		var fobAmount = new TextBoxAnnotation()
		{
			X = 1238,
			Y = 873,
			Width = 688,
			Height = 103,
			FontSize = 65,
			IsViewBox = true,
		};
		fobAmount.BindTo += (string val) => {
			App.MainView.DeclArtcile.FOBAmount = val;
		};
		App.MainView.DeclArtcile.FOBAmountAnnot = fobAmount;
		page.Annotaion.Add(fobAmount);
		fobAmount.SetText(App.MainView.DeclArtcile.FOBAmount);

		var ivNoToInput = new TextBoxAnnotation()
		{
			X = 509,
			Y = 875,
			Width = 160,
			Height = 100,
			FontSize = 65,
			IsViewBox = true,
		};
		ivNoToInput.BindTo += (string val) => {
			App.MainView.DeclArtcile.IVNoToInput = val;
		};
		App.MainView.DeclArtcile.IVNoToInputAnnot = ivNoToInput;
		page.Annotaion.Add(ivNoToInput);
		ivNoToInput.SetText(App.MainView.DeclArtcile.IVNoToInput);

		var zeikanArticle1 = new TextBoxAnnotation()
		{
			X = 509,
			Y = 988,
			Width = 1417,
			Height = 140,
			FontSize = 65,
			IsViewBox = true,
		};
		zeikanArticle1.BindTo += (string val) => {
			App.MainView.DeclArtcile.ZeikanArticle1 = val;
		};
		App.MainView.DeclArtcile.ZeikanArticle1Annot = zeikanArticle1;
		page.Annotaion.Add(zeikanArticle1);
		zeikanArticle1.SetText(App.MainView.DeclArtcile.ZeikanArticle1);

		var zeikanArticle2 = new TextBoxAnnotation()
		{
			X = 509,
			Y = 1133,
			Width = 1417,
			Height = 140,
			FontSize = 65,
			IsViewBox = true,
		};
		zeikanArticle2.BindTo += (string val) => {
			App.MainView.DeclArtcile.ZeikanArticle2 = val;
		};
		App.MainView.DeclArtcile.ZeikanArticle2Annot = zeikanArticle2;
		page.Annotaion.Add(zeikanArticle2);
		zeikanArticle2.SetText(App.MainView.DeclArtcile.ZeikanArticle2);

		var tuukanArticle = new TextBoxAnnotation()
		{
			X = 509,
			Y = 1278,
			Width = 1417,
			Height = 140,
			FontSize = 65,
			IsViewBox = true,
		};
		tuukanArticle.BindTo += (string val) => {
			App.MainView.DeclArtcile.TuukanArticle = val;
		};
		App.MainView.DeclArtcile.TuukanArticleAnnot = tuukanArticle;
		page.Annotaion.Add(tuukanArticle);
		tuukanArticle.SetText(App.MainView.DeclArtcile.TuukanArticle);

		var ninushiArticle = new TextBoxAnnotation()
		{
			X = 509,
			Y = 1423,
			Width = 1417,
			Height = 140,
			FontSize = 65,
			IsViewBox = true,
		};
		ninushiArticle.BindTo += (string val) => {
			App.MainView.DeclArtcile.NinushiArticle = val;
		};
		App.MainView.DeclArtcile.NinushiArticleAnnot = ninushiArticle;
		page.Annotaion.Add(ninushiArticle);
		ninushiArticle.SetText(App.MainView.DeclArtcile.NinushiArticle);

		var ninushiSecArticle = new TextBoxAnnotation()
		{
			X = 509,
			Y = 1568,
			Width = 1417,
			Height = 70,
			FontSize = 38,
			IsViewBox = true,
		};
		ninushiSecArticle.BindTo += (string val) => {
			App.MainView.DeclArtcile.NinushiSecArticle = val;
		};
		App.MainView.DeclArtcile.NinushiSecArticleAnnot = ninushiSecArticle;
		page.Annotaion.Add(ninushiSecArticle);
		ninushiSecArticle.SetText(App.MainView.DeclArtcile.NinushiSecArticle);

		var ninushiRefArticle = new TextBoxAnnotation()
		{
			X = 509,
			Y = 1637,
			Width = 1417,
			Height = 70,
			FontSize = 38,
			IsViewBox = true,
		};
		ninushiRefArticle.BindTo += (string val) => {
			App.MainView.DeclArtcile.NinushiRefArticle = val;
		};
		App.MainView.DeclArtcile.NinushiRefArticleAnnot = ninushiRefArticle;
		page.Annotaion.Add(ninushiRefArticle);
		ninushiRefArticle.SetText(App.MainView.DeclArtcile.NinushiRefArticle);

		var goodsName = new TextBoxAnnotation()
		{
			X = 509,
			Y = 1713,
			Width = 850,
			Height = 140,
			FontSize = 50,
			IsViewBox = true,
		};
		goodsName.BindTo += (string val) => {
			App.MainView.DeclArtcile.GoodsName = val;
		};
		App.MainView.DeclArtcile.GoodsNameAnnot = goodsName;
		page.Annotaion.Add(goodsName);
		goodsName.SetText(App.MainView.DeclArtcile.GoodsName);

		var minorBeppyouTitle = new TextBlockAnnotation()
		{
			X = 1364,
			Y = 1713,
			Width = 280,
			Height = 60,
			FontSize = 50,
			Text = "少額別表CD"
		};
		page.Annotaion.Add(minorBeppyouTitle);
		var minorBeppyou = new TextBoxAnnotation()
		{
			X = 1364,
			Y = 1778,
			Width = 280,
			Height = 75,
			FontSize = 50,
			IsViewBox = true,
		};
		minorBeppyou.BindTo += (string val) => {
			App.MainView.DeclArtcile.MinorBeppyou = val;
		};
		App.MainView.DeclArtcile.MinorBeppyouAnnot = minorBeppyou;
		page.Annotaion.Add(minorBeppyou);
		minorBeppyou.SetText(App.MainView.DeclArtcile.MinorBeppyou);

		var minorLicenceClassCodeTitle = new TextBlockAnnotation()
		{
			X = 1649,
			Y = 1713,
			Width = 120,
			Height = 60,
			FontSize = 50,
			Text = "48条"
		};
		page.Annotaion.Add(minorLicenceClassCodeTitle);
		var minorLicenceClassCode = new TextBoxAnnotation()
		{
			X = 1649,
			Y = 1778,
			Width = 120,
			Height = 75,
			FontSize = 50,
			IsViewBox = true,
		};
		minorLicenceClassCode.BindTo += (string val) => {
			App.MainView.DeclArtcile.MinorLicenceClassCode = val;
		};
		App.MainView.DeclArtcile.MinorLicenceClassCodeAnnot = minorLicenceClassCode;
		page.Annotaion.Add(minorLicenceClassCode);
		minorLicenceClassCode.SetText(App.MainView.DeclArtcile.MinorLicenceClassCode);

		var minorOLTitle = new TextBlockAnnotation()
		{
			X = 1769,
			Y = 1713,
			Width = 120,
			Height = 60,
			FontSize = 50,
			Text = "OL"
		};
		page.Annotaion.Add(minorOLTitle);
		var minorOL = new TextBoxAnnotation()
		{
			X = 1769,
			Y = 1778,
			Width = 120,
			Height = 75,
			FontSize = 50,
			IsViewBox = true,
		};
		minorOL.BindTo += (string val) => {
			App.MainView.DeclArtcile.MinorOL = val;
		};
		App.MainView.DeclArtcile.MinorOLAnnot = minorOL;
		page.Annotaion.Add(minorOL);
		minorOL.SetText(App.MainView.DeclArtcile.MinorOL);

		if (App.MainView.DeclArtcile.Licences.Count > 0)
        {
			int _licy = 325 + 5;
			int _licCntr = 0;
			foreach(var lic in App.MainView.DeclArtcile.Licences)
            {
				if(_licCntr > 3)
                {
					page.Annotaion.Add(new TextBlockAnnotation()
					{
						X = 515,
						Y = _licy + 60 * _licCntr,
						Width = 270,
						Height = 60,
						FontSize = 50,
						Text = "省略されました..."
					});
					break;
				}
				var licType = new TextBoxAnnotation()
				{
					X = 515,
					Y = _licy + 60 * _licCntr,
					Width = 240,
					Height = 60,
					FontSize = 50,
				};
				licType.BindTo += (string val) => {
					lic.LicenceTypeCode = val;
				};
				lic.LicenceTypeCodeAnnot = licType;
				page.Annotaion.Add(licType);
				licType.SetText(lic.LicenceTypeCode);
				var licNo = new TextBoxAnnotation()
				{
					X = 798,
					Y = _licy + 60 * _licCntr,
					Width = 700,
					Height = 60,
					FontSize = 50,
				};
				licNo.BindTo += (string val) => {
					lic.LicenceNo = val;
				};
				lic.LicenceNoAnnot = licNo;
				page.Annotaion.Add(licNo);
				licNo.SetText(lic.LicenceNo);
				_licCntr++;
			}
        }

		if (App.MainView.DeclArtcile.DeclElements.Count == 0)
        {
			App.MainView.Pages.Add(page);
			return;
        }

		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 1865,
			EndX = 1936,
			EndY = 1865,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 1915,
			EndX = 1936,
			EndY = 1915,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 105,
			StartY = 1865,
			EndX = 105,
			EndY = 1915,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 1936,
			StartY = 1865,
			EndX = 1936,
			EndY = 1915,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 655,
			StartY = 1865,
			EndX = 655,
			EndY = 1915,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 1295,
			StartY = 1865,
			EndX = 1295,
			EndY = 1915,
			Pitch = 3,
		});
		page.Annotaion.Add(new LineAnnotation()
		{
			StartX = 1936,
			StartY = 1865,
			EndX = 1936,
			EndY = 1915,
			Pitch = 3,
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 115,
			Y = 1863,
			FontSize = 30,
			Width = 530,
			Height = 45,
			Text = "統計品目番号",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 665,
			Y = 1863,
			FontSize = 30,
			Width = 630,
			Height = 45,
			Text = "QN1",
		});
		page.Annotaion.Add(new TextBlockAnnotation()
		{
			X = 1305,
			Y = 1863,
			FontSize = 30,
			Width = 630,
			Height = 45,
			Text = "QN2",
		});

		int _y = 1915;
		int _cntr = 0;

		foreach(var declElem in App.MainView.DeclArtcile.DeclElements)
        {
			page.Annotaion.Add(new LineAnnotation()
			{
				StartX = 105,
				StartY = _y,
				EndX = 1936,
				EndY = _y,
				Pitch = 3,
			});
			page.Annotaion.Add(new LineAnnotation()
			{
				StartX = 105,
				StartY = _y+300,
				EndX = 1936,
				EndY = _y+300,
				Pitch = 3,
			});
			page.Annotaion.Add(new LineAnnotation()
			{
				StartX = 105,
				StartY = _y,
				EndX = 105,
				EndY = _y + 300,
				Pitch = 3,
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 110,
				Y = _y + 130,
				Width = 50,
				Height = 60,
				FontSize = 50,
				Text = (_cntr + 1).ToString()

			}) ;
			page.Annotaion.Add(new LineAnnotation()
			{
				StartX = 165,
				StartY = _y,
				EndX = 165,
				EndY = _y + 300,
				Pitch = 3,
			});
			page.Annotaion.Add(new LineAnnotation()
			{
				StartX = 655,
				StartY = _y,
				EndX = 655,
				EndY = _y + 300,
				Pitch = 3,
			});
			page.Annotaion.Add(new LineAnnotation()
			{
				StartX = 1936,
				StartY = _y,
				EndX = 1936,
				EndY = _y + 300,
				Pitch = 3,
			});
			page.Annotaion.Add(new LineAnnotation()
			{
				StartX = 165,
				StartY = _y + 110,
				EndX = 1936,
				EndY = _y + 110,
				Pitch = 3,
			});
			page.Annotaion.Add(new LineAnnotation()
			{
				StartX = 165,
				StartY = _y + 220,
				EndX = 1936,
				EndY = _y + 220,
				Pitch = 3,
			});
			page.Annotaion.Add(new LineAnnotation()
			{
				StartX = 1295,
				StartY = _y,
				EndX = 1295,
				EndY = _y + 110,
				Pitch = 3,
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 170,
				Y = _y + 125,
				Width = 170,
				Height = 90,
				FontSize = 65,
				Text = "ET欄"

			});
			page.Annotaion.Add(new LineAnnotation()
			{
				StartX = 350,
				StartY = _y + 110,
				EndX = 350,
				EndY = _y + 220,
				Pitch = 3,
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 170,
				Y = _y + 225,
				Width = 100,
				Height = 70,
				FontSize = 50,
				Text = "TX :"

			});
			page.Annotaion.Add(new LineAnnotation()
			{
				StartX = 410,
				StartY = _y + 220,
				EndX = 410,
				EndY = _y + 300,
				Pitch = 3,
			});
			page.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 415,
				Y = _y + 225,
				Width = 100,
				Height = 70,
				FontSize = 50,
				Text = "OL :"

			});
			var otherLawCode = new TextBoxAnnotation()
			{
				X = 515,
				Y = _y + 225,
				Width = 135,
				Height = 70,
				FontSize = 50,
			};
			otherLawCode.BindTo += (string val) => {
				declElem.OtherLawCode = val;
			};
			declElem.OtherLawCodeAnnot = otherLawCode;
			page.Annotaion.Add(otherLawCode);
			otherLawCode.SetText(declElem.OtherLawCode);

			page.Annotaion.Add(new LineAnnotation()
			{
				StartX = 1155,
				StartY = _y + 220,
				EndX = 1155,
				EndY = _y + 300,
				Pitch = 3,
			});

			var hsCode = new TextBoxAnnotation()
			{
				X = 165,
				Y = _y+5,
				Width = 480,
				Height = 100,
				FontSize = 70,
				IsViewBox = true,
			};
			hsCode.BindTo += (string val) => {
				declElem.HSCode = val;
			};
			declElem.HSCodeAnnot = hsCode;
			page.Annotaion.Add(hsCode);
			hsCode.SetText(declElem.HSCode);

			var quantity1 = new TextBoxAnnotation()
			{
				X = 665,
				Y = _y + 5,
				Width = 520,
				Height = 100,
				FontSize = 70,
				IsViewBox = true,
			};
			quantity1.BindTo += (string val) => {
				declElem.Quantity1 = val;
			};
			declElem.Quantity1Annot = quantity1;
			page.Annotaion.Add(quantity1);
			quantity1.SetText(declElem.Quantity1);
			var unit1 = new TextBoxAnnotation()
			{
				X = 1185,
				Y = _y + 5,
				Width = 110,
				Height = 100,
				FontSize = 70,
				IsViewBox = true,
			};
			unit1.BindTo += (string val) => {
				declElem.Unit1 = val;
			};
			declElem.Unit1Annot = unit1;
			page.Annotaion.Add(unit1);
			unit1.SetText(declElem.Unit1);
			var quantity2 = new TextBoxAnnotation()
			{
				X = 1305,
				Y = _y + 5,
				Width = 520,
				Height = 100,
				FontSize = 70,
				IsViewBox = true,
			};
			quantity2.BindTo += (string val) => {
				declElem.Quantity2 = val;
			};
			declElem.Quantity2Annot = quantity2;
			page.Annotaion.Add(quantity2);
			quantity2.SetText(declElem.Quantity2);
			var unit2 = new TextBoxAnnotation()
			{
				X = 1825,
				Y = _y + 5,
				Width = 110,
				Height = 100,
				FontSize = 70,
				IsViewBox = true,
			};
			unit2.BindTo += (string val) => {
				declElem.Unit2 = val;
			};
			declElem.Unit2Annot = unit2;
			page.Annotaion.Add(unit2);
			unit2.SetText(declElem.Unit2);
			var beppyouCode = new TextBoxAnnotation()
			{
				X = 360,
				Y = _y + 115,
				Width = 220,
				Height = 100,
				FontSize = 65,
				IsViewBox = true,
			};
			beppyouCode.BindTo += (string val) => {
				declElem.BeppyouCode = val;
			};
			declElem.BeppyouCodeAnnot = beppyouCode;
			page.Annotaion.Add(beppyouCode);
			beppyouCode.SetText(declElem.BeppyouCode);
			var licenceClassCode = new TextBoxAnnotation()
			{
				X = 585,
				Y = _y + 115,
				Width = 60,
				Height = 100,
				FontSize = 65,
				IsViewBox = true,
			};
			licenceClassCode.BindTo += (string val) => {
				declElem.LicenceClassCode = val;
			};
			declElem.LicenceClassCodeAnnot = licenceClassCode;
			page.Annotaion.Add(licenceClassCode);
			licenceClassCode.SetText(declElem.LicenceClassCode);
			var elemCurrency = new TextBoxAnnotation()
			{
				X = 665,
				Y = _y + 115,
				Width = 240,
				Height = 100,
				FontSize = 75,
				IsViewBox = true,
			};
			elemCurrency.BindTo += (string val) => {
				declElem.Currency = val;
			};
			declElem.CurrencyAnnot = elemCurrency;
			page.Annotaion.Add(elemCurrency);
			elemCurrency.SetText(declElem.Currency);

			var basicPrice = new TextBoxAnnotation()
			{
				X = 925,
				Y = _y + 115,
				Width = 1000,
				Height = 100,
				FontSize = 75,
				IsViewBox = true,
			};
			basicPrice.BindTo += (string val) => {
				declElem.BasicPrice = val;
			};
			declElem.BasicPriceAnnot = basicPrice;
			page.Annotaion.Add(basicPrice);
			basicPrice.SetText(declElem.BasicPrice);

			_cntr++;
			_y += 300;
			if((_cntr-3) % 9 == 0)
            {
				_pageCntr++;
				App.MainView.Pages.Add(page);
				page = new Page()
				{
					IsInputPaper = true,
					Width = shorterpix,
					Height = longerpix,
					CanvasWidth = shorterpix,
					CanvasHeight = longerpix,
					PageNo = _pageCntr,
					Caption = "入力用紙",
				};
				_y = 115;
				page.Annotaion.Add(new LineAnnotation()
				{
					StartX = 105,
					StartY = _y,
					EndX = 1936,
					EndY = _y,
					Pitch = 3,
				});
				page.Annotaion.Add(new LineAnnotation()
				{
					StartX = 105,
					StartY = _y+50,
					EndX = 1936,
					EndY = _y+50,
					Pitch = 3,
				});
				page.Annotaion.Add(new LineAnnotation()
				{
					StartX = 105,
					StartY = _y,
					EndX = 105,
					EndY = _y+50,
					Pitch = 3,
				});
				page.Annotaion.Add(new LineAnnotation()
				{
					StartX = 1936,
					StartY = _y,
					EndX = 1936,
					EndY = _y+50,
					Pitch = 3,
				});
				page.Annotaion.Add(new LineAnnotation()
				{
					StartX = 655,
					StartY = _y,
					EndX = 655,
					EndY = _y+50,
					Pitch = 3,
				});
				page.Annotaion.Add(new LineAnnotation()
				{
					StartX = 1295,
					StartY = _y,
					EndX = 1295,
					EndY = _y+50,
					Pitch = 3,
				});
				page.Annotaion.Add(new LineAnnotation()
				{
					StartX = 1936,
					StartY = _y,
					EndX = 1936,
					EndY = _y+50,
					Pitch = 3,
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 115,
					Y = _y+3,
					FontSize = 30,
					Width = 530,
					Height = 45,
					Text = "統計品目番号",
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 665,
					Y = _y+3,
					FontSize = 30,
					Width = 630,
					Height = 45,
					Text = "QN1",
				});
				page.Annotaion.Add(new TextBlockAnnotation()
				{
					X = 1305,
					Y = _y+3,
					FontSize = 30,
					Width = 630,
					Height = 45,
					Text = "QN2",
				});
				_y += 60;
			}
        }
		if((_cntr - 3) % 9 != 0) App.MainView.Pages.Add(page);

	}
	public void GenerateQrPages()
    {
		int shorterpix = FundamentalSettings.ShorterPixels;
		float factor = FundamentalSettings.LongerPixelsIn72DPI / FundamentalSettings.ShorterPixelsIn72DPI;
		int longerpix = (int)((float)shorterpix * factor);
		BitmapSource[] images = GenerateInputQr();
		int cntr = 0;
		var qrCode = new BarcodeWriter
		{
			Format = BarcodeFormat.QR_CODE,
			Options = new ZXing.QrCode.QrCodeEncodingOptions
			{
				ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.M,
				CharacterSet = "ISO-8859-1", // default							 //CharacterSet = "UTF-8", // japanese
				Width = 512,
				Height = 512,
				Margin = 1,
			},
		};
		BitmapSource docNoQr = UtilityFunc.BitmapToBitmapSource(qrCode.Write(App.MainView.CurrentDocumentNo)); 
		foreach (var image in images) {
			var newPage = new Page()
			{
				IsGenerated = true,
				Width = shorterpix,
				Height = longerpix,
				CanvasHeight = longerpix,
				CanvasWidth = shorterpix,
				PageNo = int.MaxValue,
				Caption = "QR",
			};
			newPage.Annotaion.Add(new QrImageAnnotation()
			{
				X = 120.0,
				Y = 240.0,
				Image = images[cntr],
				Height = 1024.0,
				Width = 1024.0,
			});
			newPage.Annotaion.Add(new QrImageAnnotation()
			{
				X = shorterpix - 380 - 25,
				Y = 240.0,
				Image = docNoQr,
				Height = 380,
				Width = 380,
			});


			string pageTitle = "";
            switch (cntr)
            {
				case 0:
					pageTitle = "1 画面目";
					break;
				case 1:
					pageTitle = "2 画面目 (1)";
					break;
				case 2:
					pageTitle = "2 画面目 (2)INVN";
					break;
				case 3:
					pageTitle = "3 画面目";
					break;
				default:
					break;
			}

			newPage.Annotaion.Add(new TextBlockAnnotation()
			{
				X = 120.0,
				Y = 10,
				Text = pageTitle,
				FontSize = 200,
				Height = 230,
				Width = 2048.0,
			});
			App.MainView.Pages.Add(newPage);
			cntr++;
		}
		
    }
	public BitmapSource[] GenerateInputQr()
    {
		var qrCode = new BarcodeWriter
		{
			Format = BarcodeFormat.QR_CODE,
			Options = new ZXing.QrCode.QrCodeEncodingOptions
			{
				ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.M,
				CharacterSet = "ISO-8859-1", // default							 //CharacterSet = "UTF-8", // japanese
				Width = 1024,
				Height = 1024,
				Margin = 1,
			},
		};
		
		var Code = EncodeInputData();
		if (Code == null) return null;
		BitmapSource[] res = new BitmapSource[4];
		using (var bmp = qrCode.Write(Code[0]))
		{
			res[0] = UtilityFunc.BitmapToBitmapSource(bmp);
		}
		using (var bmp = qrCode.Write(Code[1]))
		{
			res[1] = UtilityFunc.BitmapToBitmapSource(bmp);
		}
		using (var bmp = qrCode.Write(Code[2]))
		{
			res[2] = UtilityFunc.BitmapToBitmapSource(bmp);
		}
		using (var bmp = qrCode.Write(Code[3]))
		{
			res[3] = UtilityFunc.BitmapToBitmapSource(bmp);
		}
		return res;


	}
	public string[] EncodeInputData()
    {
		if (App.MainView.DeclArtcile == null) return null;
		string[] res = new string[4];
		res[0] = encode1StGamenData();
		res[1] = encode2ndGamenData();
		res[2] = encodeLicenceData();
		res[3] = encode3rdGamenData();
		return res;
    }

	private string encode1StGamenData()
    {
		var data = App.MainView.DeclArtcile;
		if (data == null) return "";
		string delimiter = "%";
		var delete_delim = ConstValue.Re_Delete_delim;
		var delete_exUse = ConstValue.Re_Delete_exUse;
		
		string zouti = delete_delim.Replace(delete_exUse.Replace(data.Zouti, String.Empty), String.Empty);
		string declType = delete_delim.Replace(delete_exUse.Replace(data.DeclType, String.Empty), String.Empty);
		string bumon = delete_delim.Replace(delete_exUse.Replace(data.DeclSubmissBumon, String.Empty), String.Empty);
		string zeikan = delete_delim.Replace(delete_exUse.Replace(data.DeclSubmiss, String.Empty), String.Empty);
		string shprHoujin13 = delete_delim.Replace(delete_exUse.Replace(data.shprHoujinCode13, String.Empty), String.Empty);
		string shprHoujin4 = delete_delim.Replace(delete_exUse.Replace(data.shprHoujinCode4, String.Empty), String.Empty);
		string shprName = delete_delim.Replace(delete_exUse.Replace(data.ShprName, String.Empty), String.Empty);
		string shprZip = delete_delim.Replace(delete_exUse.Replace(data.ShprZipCode, String.Empty), String.Empty);
		string shprPref = delete_delim.Replace(delete_exUse.Replace(data.ShprPref, String.Empty), String.Empty);
		string shprMuni = delete_delim.Replace(delete_exUse.Replace(data.ShprMuni, String.Empty), String.Empty);
		string shprAddr1 = delete_delim.Replace(delete_exUse.Replace(data.ShprAddr1, String.Empty), String.Empty);
		string shprAddr2 = delete_delim.Replace(delete_exUse.Replace(data.ShprAddr2, String.Empty), String.Empty);
		string shprTelephone = delete_delim.Replace(delete_exUse.Replace(data.ShprTelephone, String.Empty), String.Empty);
		string cineeCode = delete_delim.Replace(delete_exUse.Replace(data.cineeCode, String.Empty), String.Empty);
		string cineeName = delete_delim.Replace(delete_exUse.Replace(data.cineeName, String.Empty), String.Empty);
		string cineeAddr1 = delete_delim.Replace(delete_exUse.Replace(data.cineeAddr1, String.Empty), String.Empty);
		string cineeAddr2 = delete_delim.Replace(delete_exUse.Replace(data.cineeAddr2, String.Empty), String.Empty);
		string cineeAddr3 = delete_delim.Replace(delete_exUse.Replace(data.cineeAddr3, String.Empty), String.Empty);
		string cineeAddr4 = delete_delim.Replace(delete_exUse.Replace(data.cineeAddr4, String.Empty), String.Empty);
		string cineeZip = delete_delim.Replace(delete_exUse.Replace(data.cineeZipCode, String.Empty), String.Empty);
		string cineeCountryCD = delete_delim.Replace(delete_exUse.Replace(data.cineeCountryCode, String.Empty), String.Empty);
		string destPort = delete_delim.Replace(delete_exUse.Replace(data.Iata3, String.Empty), String.Empty);
		string portOfLoad = delete_delim.Replace(delete_exUse.Replace(data.PortOfDept, String.Empty), String.Empty);
		string destCode = delete_delim.Replace(delete_exUse.Replace(data.Destination, String.Empty), String.Empty);
		string destName = delete_delim.Replace(delete_exUse.Replace(data.DestinationName, String.Empty), String.Empty);
		string daisyou = delete_delim.Replace(delete_exUse.Replace(data.Daisyou, String.Empty), String.Empty);
		string date = delete_delim.Replace(delete_exUse.Replace(data.DeclDate, String.Empty), String.Empty);
		StringBuilder sb = new StringBuilder();
		string shimukeKoku = "";
		string shimukeChi = "";
		if (destCode.Length > 4)
		{
			shimukeKoku = destCode.Substring(0, 2);
			shimukeChi = destCode.Substring(2, 3);
		}

		sb.Append(zouti).Append(delimiter);
		sb.Append(declType).Append(delimiter);
		sb.Append(bumon).Append(delimiter);
		sb.Append(zeikan).Append(delimiter);
		sb.Append(shprHoujin13).Append(delimiter);
		sb.Append(shprHoujin4).Append(delimiter);
		sb.Append(shprName).Append(delimiter);
		sb.Append(shprZip).Append(delimiter);
		sb.Append(shprPref).Append(delimiter);
		sb.Append(shprMuni).Append(delimiter);
		sb.Append(shprAddr1).Append(delimiter);
		sb.Append(shprAddr2).Append(delimiter);
		sb.Append(shprTelephone).Append(delimiter);
		sb.Append(cineeCode).Append(delimiter);
		sb.Append(cineeName).Append(delimiter);
		sb.Append(cineeAddr1).Append(delimiter);
		sb.Append(cineeAddr2).Append(delimiter);
		sb.Append(cineeAddr3).Append(delimiter);
		sb.Append(cineeAddr4).Append(delimiter);
		sb.Append(cineeZip).Append(delimiter);
		sb.Append(cineeCountryCD).Append(delimiter);
		sb.Append(destPort).Append(delimiter);
		sb.Append(portOfLoad).Append(delimiter);
		sb.Append(shimukeKoku).Append(delimiter);
		sb.Append(shimukeChi).Append(delimiter);
		sb.Append(destName).Append(delimiter);
		sb.Append(daisyou).Append(delimiter);
		sb.Append(date).Append(delimiter);


		var lorem = sb.ToString();
		if (lorem.Length < 2) return "";
		lorem += "\0";
		lorem = lorem.ToUpper();
		var suf = new InputStringGenerator.SuffixArray(lorem);
		var a = suf.SAIS();
		var s = InputStringGenerator.BWTransform(lorem, a.Item2);
		var mtf2 = new InputStringGenerator.MTF2(s);
		mtf2.Encode();
		var imtf2 = new InputStringGenerator.MTF2(mtf2.Output);
		string str = imtf2.Decode();
		var pb = new InputStringGenerator.PackBits(mtf2.Output, 20);
		pb.Encode();
		var ARC = new InputStringGenerator.AdaptiveRangeCoder(pb.Output, InputStringGenerator.AdaptiveRangeCoder.CodingMode.ENCODE, 69);
		ARC.EncodeData();
		string payload = InputStringGenerator.EncBASE32(ARC.Output);
		payload = payload.ToLower();
		string qrData = "%$%0:" + payload + ":$%$";
		return qrData;
	}

	private string encode2ndGamenData()
    {
		var data = App.MainView.DeclArtcile;
		if (data == null) return "";
		string delimiter = "%";
		//if (col.Count < 1) return "";		
		var delete_delim = ConstValue.Re_Delete_delim;
		var delete_exUse = ConstValue.Re_Delete_exUse;
		var delete_alpha = ConstValue.Re_Delete_Alpha;
		var delete_comma = ConstValue.Re_Delete_comma;
		var priceQty = ConstValue.Re_PriceQty;
		string daisyou = delete_delim.Replace(delete_exUse.Replace(data.Daisyou, String.Empty), String.Empty);
		string bouekiType = delete_delim.Replace(delete_exUse.Replace(data.BouekiType, String.Empty), String.Empty);
		string payment = delete_delim.Replace(delete_exUse.Replace(data.PaymentCode, String.Empty), String.Empty);
		string currency = InputStringGenerator.CodeBook.Currency.ContainsKey(data.Currency) ? InputStringGenerator.CodeBook.Currency[data.Currency].ToString() : "";
		string currency2 = data.TradeTerm.ToUpper() == "FOB" ? "" : InputStringGenerator.CodeBook.Currency.ContainsKey(data.Currency) ? InputStringGenerator.CodeBook.Currency[data.Currency].ToString() : "";
		string tradeTerm = InputStringGenerator.CodeBook.TradeTerm.ContainsKey(data.TradeTerm) ? InputStringGenerator.CodeBook.TradeTerm[data.TradeTerm].ToString() : "";
		var termAmt = priceQty.Match(delete_comma.Replace(delete_alpha.Replace(delete_delim.Replace(data.TermAmount, String.Empty), String.Empty), String.Empty));
		string termAmtUp = InputStringGenerator.RiceCodeModoki32(termAmt.Groups["upper"].Value, 25);
		string termAmtDown = InputStringGenerator.RiceCodeModoki32(termAmt.Groups["lower"].Value, 5);
		var FOBAmt = priceQty.Match(delete_comma.Replace(delete_alpha.Replace(delete_delim.Replace(data.FOBAmount, String.Empty), String.Empty), String.Empty));
		string FOBAmtUp = InputStringGenerator.RiceCodeModoki32(FOBAmt.Groups["upper"].Value, 25);
		string FOBAmtDown = InputStringGenerator.RiceCodeModoki32(FOBAmt.Groups["lower"].Value, 5);
		string youtoukaku = delete_exUse.Replace(delete_delim.Replace(data.Youtoukaku, String.Empty), String.Empty);
		string zeikan1 = delete_exUse.Replace(delete_delim.Replace(data.ZeikanArticle1, String.Empty), String.Empty);
		string zeikan2 = delete_exUse.Replace(delete_delim.Replace(data.ZeikanArticle2, String.Empty), String.Empty);
		string tuukan = delete_exUse.Replace(delete_delim.Replace(data.TuukanArticle, String.Empty), String.Empty);
		string ninushi = delete_exUse.Replace(delete_delim.Replace(data.NinushiArticle, String.Empty), String.Empty);
		string ninushiSec = delete_exUse.Replace(delete_delim.Replace(data.NinushiSecArticle, String.Empty), String.Empty);
		string ninushiRef = delete_exUse.Replace(delete_delim.Replace(data.NinushiRefArticle, String.Empty), String.Empty);

		StringBuilder sb = new StringBuilder();

		sb.Append(daisyou).Append(delimiter);
		sb.Append(bouekiType).Append(delimiter);
		sb.Append(payment).Append(delimiter);
		sb.Append(tradeTerm).Append(delimiter);
		sb.Append(currency).Append(delimiter);
		sb.Append(currency2).Append(delimiter);
		sb.Append(termAmtUp).Append(delimiter);
		sb.Append(termAmtDown).Append(delimiter);
		sb.Append(FOBAmtUp).Append(delimiter);
		sb.Append(FOBAmtDown).Append(delimiter);
		sb.Append(youtoukaku).Append(delimiter);
		sb.Append(zeikan1).Append(delimiter);
		sb.Append(zeikan2).Append(delimiter);
		sb.Append(tuukan).Append(delimiter);
		sb.Append(ninushi).Append(delimiter);
		sb.Append(ninushiSec).Append(delimiter);
		sb.Append(ninushiRef).Append(delimiter);

		var lorem = sb.ToString();
		if (lorem.Length < 2) return "";
		lorem += "\0";
		lorem = lorem.ToUpper();
		var suf = new InputStringGenerator.SuffixArray(lorem);
		var a = suf.SAIS();
		var s = InputStringGenerator.BWTransform(lorem, a.Item2);
		var mtf2 = new InputStringGenerator.MTF2(s);
		mtf2.Encode();
		var imtf2 = new InputStringGenerator.MTF2(mtf2.Output);
		string str = imtf2.Decode();
		var pb = new InputStringGenerator.PackBits(mtf2.Output, 20);
		pb.Encode();
		var ARC = new InputStringGenerator.AdaptiveRangeCoder(pb.Output, InputStringGenerator.AdaptiveRangeCoder.CodingMode.ENCODE, 69);
		ARC.EncodeData();
		string payload = InputStringGenerator.EncBASE32(ARC.Output);
		payload = payload.ToLower();
		string qrData = "%$%1:" + payload + ":$%$";
		return qrData;
	}

	private string encodeLicenceData()
    {
		var data = App.MainView.DeclArtcile;
		if (data == null) return "";
		string delimiter = "%";
		//if (col.Count < 1) return "";		
		var delete_delim = ConstValue.Re_Delete_delim;
		var delete_exUse = ConstValue.Re_Delete_exUse;

		string invoiceNo = delete_delim.Replace(delete_exUse.Replace(data.InvNo, String.Empty), String.Empty);
		string gaitame = delete_delim.Replace(delete_exUse.Replace(data.GaitameCode, String.Empty), String.Empty);
		string temp = invoiceNo + delimiter + gaitame;

		string payload1 = "";
		if (temp.Length > 0)
		{
			var lorem = temp;
			lorem += "\0";
			lorem = lorem.ToUpper();
			var suf = new InputStringGenerator.SuffixArray(lorem);
			var a = suf.SAIS();
			var s = InputStringGenerator.BWTransform(lorem, a.Item2);
			var mtf2 = new InputStringGenerator.MTF2(s);
			mtf2.Encode();
			var imtf2 = new InputStringGenerator.MTF2(mtf2.Output);
			string str = imtf2.Decode();
			var pb = new InputStringGenerator.PackBits(mtf2.Output, 20);
			pb.Encode();
			var ARC = new InputStringGenerator.AdaptiveRangeCoder(pb.Output, InputStringGenerator.AdaptiveRangeCoder.CodingMode.ENCODE, 69);
			ARC.EncodeData();
			payload1 = InputStringGenerator.EncBASE32(ARC.Output);
			payload1 = payload1.ToLower();
		}
		StringBuilder sb = new StringBuilder();
		foreach (var lic in data.Licences)
		{
			sb.Append(delete_delim.Replace(delete_exUse.Replace(lic.LicenceTypeCode, String.Empty), String.Empty)).Append(delimiter);
			sb.Append(delete_delim.Replace(delete_exUse.Replace(lic.LicenceNo, String.Empty), String.Empty)).Append(delimiter);
		}
		string payload2 = "";
		if (sb.ToString().Length > 0)
		{
			string lorem = "";
			lorem += sb.ToString() + "\0";
			lorem = lorem.ToUpper();
			var suf = new InputStringGenerator.SuffixArray(lorem);
			var a = suf.SAIS();
			var s = InputStringGenerator.BWTransform(lorem, a.Item2);
			var mtf2 = new InputStringGenerator.MTF2(s);
			mtf2.Encode();
			var pb = new InputStringGenerator.PackBits(mtf2.Output, 20);
			pb.Encode();
			var ARC = new InputStringGenerator.AdaptiveRangeCoder(pb.Output, InputStringGenerator.AdaptiveRangeCoder.CodingMode.ENCODE, 69);
			ARC.EncodeData();
			payload2 = InputStringGenerator.EncBASE32(ARC.Output);
			payload2 = payload2.ToLower();
		}
		string qrData = "%$%2:" + payload1 + ":" + payload2 + "$%$";
		return qrData;
	}

	private string encode3rdGamenData()
    {
		var data = App.MainView.DeclArtcile;
		if (data == null) return "";
		string delimiter = "%";
		var cols = data.DeclElements;
		//if (col.Count < 1) return "";
		var delete_exDigitAndPeriod = ConstValue.Re_DeleteExDigitAndPeriod;
		var priceQty = ConstValue.Re_PriceQty;
		var hs = ConstValue.Re_HSCode;
		var gaitame = ConstValue.Re_Gaitame;
		var delete_exUse = ConstValue.Re_Delete_exUse;
		var delete_delim = ConstValue.Re_Delete_delim;
		string anbunWt = delete_exDigitAndPeriod.Replace(data.IVNetWeight, String.Empty);
		var m_anbunWt = priceQty.Match(anbunWt);
		string anbunWtUpper = InputStringGenerator.RiceCodeModoki32(m_anbunWt.Groups["upper"].Value, 15);
		string anbunWtLower = InputStringGenerator.RiceCodeModoki32(m_anbunWt.Groups["lower"].Value, 5);
		string goodsName = delete_delim.Replace(delete_exUse.Replace(data.GoodsName, String.Empty), String.Empty);

		var m_minorGai = gaitame.Match(data.MinorBeppyou);
		string minorBeppyou = InputStringGenerator.RiceCodeModoki32(m_minorGai.Groups["code"].Value,10);
		string minorLicClassCode = data.MinorLicenceClassCode;
		if (minorLicClassCode.Length > 1) minorLicClassCode = minorLicClassCode.Substring(0, 1);
		minorLicClassCode = minorLicClassCode == "" ? "0" : minorLicClassCode.ToUpper();
		char minorOL = '0';
		if (InputStringGenerator.CodeBook.OL.ContainsKey(data.MinorOL))
		{
			minorOL = InputStringGenerator.CodeBook.OL[data.MinorOL];
		}
		string printerName = App.MainView.CurrentPrinterName.ToUpper();

		
		var base_sb = new StringBuilder();
		base_sb.Append(anbunWtUpper).Append(delimiter);
		base_sb.Append(anbunWtLower).Append(delimiter);
		base_sb.Append(goodsName).Append(delimiter);
		base_sb.Append(minorOL.ToString()).Append(delimiter);
		base_sb.Append(minorBeppyou).Append(delimiter);
		base_sb.Append(minorLicClassCode).Append(delimiter);
		base_sb.Append(printerName).Append(delimiter);
		string payload1 = "";
		string payload2 = "";
		var lorem1 = base_sb.ToString();
		if (lorem1.Length > 0)
		{
			lorem1 += "\0";
			lorem1 = lorem1.ToUpper();
			var suf = new InputStringGenerator.SuffixArray(lorem1);
			var a = suf.SAIS();
			var s = InputStringGenerator.BWTransform(lorem1, a.Item2);
			var mtf2 = new InputStringGenerator.MTF2(s);
			mtf2.Encode();
			var pb = new InputStringGenerator.PackBits(mtf2.Output, 20);
			pb.Encode();
			var ARC = new InputStringGenerator.AdaptiveRangeCoder(pb.Output, InputStringGenerator.AdaptiveRangeCoder.CodingMode.ENCODE, 69);
			ARC.EncodeData();
			payload1 = InputStringGenerator.EncBASE32(ARC.Output);
			payload1 = payload1.ToLower();
		}
		else
		{
			payload1 = "";
		}

		StringBuilder loop_sb = new StringBuilder();
		foreach (var item in cols)
		{
			var m_price = priceQty.Match(delete_exDigitAndPeriod.Replace(item.BasicPrice, String.Empty));
			var m_qn1 = priceQty.Match(delete_exDigitAndPeriod.Replace(item.Quantity1, String.Empty));
			var m_qn2 = priceQty.Match(delete_exDigitAndPeriod.Replace(item.Quantity2, String.Empty));
			var m_hs = hs.Match(item.HSCode);
			var m_gai = gaitame.Match(item.BeppyouCode);
			loop_sb.Append(InputStringGenerator.RiceCodeModoki32(m_hs.Groups["nine"].Value, 15));
			loop_sb.Append(m_hs.Groups["postfix"].Value == "" ? "Z" : m_hs.Groups["postfix"].Value);
			loop_sb.Append(InputStringGenerator.RiceCodeModoki32(m_qn1.Groups["upper"].Value, 15));
			loop_sb.Append(InputStringGenerator.RiceCodeModoki32(m_qn1.Groups["lower"].Value, 5));
			char unit1 = '0';
			if (InputStringGenerator.CodeBook.Unit.ContainsKey(item.Unit1))
			{
				unit1 = InputStringGenerator.CodeBook.Unit[item.Unit1];
			}
			loop_sb.Append(unit1.ToString());
			loop_sb.Append(InputStringGenerator.RiceCodeModoki32(m_qn2.Groups["upper"].Value, 15));
			loop_sb.Append(InputStringGenerator.RiceCodeModoki32(m_qn2.Groups["lower"].Value, 5));
			char unit2 = '0';
			if (InputStringGenerator.CodeBook.Unit.ContainsKey(item.Unit2))
			{
				unit2 = InputStringGenerator.CodeBook.Unit[item.Unit2];
			}
			loop_sb.Append(unit2.ToString());
			loop_sb.Append(InputStringGenerator.RiceCodeModoki32(m_gai.Groups["code"].Value, 10));
			var liccode = item.LicenceClassCode.ToUpper();
			if (liccode.Length > 1) liccode = liccode.Substring(0, 1);
			loop_sb.Append(liccode == "" ? "0" : liccode);
			char cur = '0';
			if (InputStringGenerator.CodeBook.Currency.ContainsKey(item.Currency))
			{
				cur = InputStringGenerator.CodeBook.Currency[item.Currency];
			}
			loop_sb.Append(cur.ToString());
			loop_sb.Append(InputStringGenerator.RiceCodeModoki32(m_price.Groups["upper"].Value, 20));
			loop_sb.Append(InputStringGenerator.RiceCodeModoki32(m_price.Groups["lower"].Value, 5));
			char ol = '0';
			if (InputStringGenerator.CodeBook.OL.ContainsKey(item.OtherLawCode))
			{
				ol = InputStringGenerator.CodeBook.OL[item.OtherLawCode];
			}
			loop_sb.Append(ol.ToString());
		}
		var lorem2 = loop_sb.ToString();
		if (lorem2.Length > 0)
		{
			lorem2 += "\0";
			lorem2 = lorem2.ToUpper();
			var suf = new InputStringGenerator.SuffixArray(lorem2);
			var a = suf.SAIS();
			var s = InputStringGenerator.BWTransform(lorem2, a.Item2);
			var mtf2 = new InputStringGenerator.MTF2(s);
			mtf2.Encode();
			var pb = new InputStringGenerator.PackBits(mtf2.Output, 20);
			pb.Encode();
			var ARC = new InputStringGenerator.AdaptiveRangeCoder(pb.Output, InputStringGenerator.AdaptiveRangeCoder.CodingMode.ENCODE, 69);
			ARC.EncodeData();
			payload2 = InputStringGenerator.EncBASE32(ARC.Output);
			payload2 = payload2.ToLower();
		}
		else
		{
			payload2 = "";
		}
		string qrData = "%$%3:" + payload1 + ":" + payload2 + "$%$";
		return qrData;
	}

	public static bool GenerateInput(object sender, RoutedEventArgs e)
	{
		App.MainView.DeclArtcile.ValidateAll();
		if (App.MainView.DeclArtcile.CommonValid != DeclArticle.CommonValidFlag.All)
		{
			MessageBox.Show("入力内容に不備があります。反転箇所を確認の上再度クリックしてください。");
			return false;
		}
        if (App.MainView.DeclArtcile.IsShprOverride)
        {
			var result = MessageBox.Show("輸出者住所が変更されています。内容を保存しますか？", "", MessageBoxButton.YesNo);
            switch (result)
            {
				case MessageBoxResult.Yes:
					saveShprAdrress();
					break;
				case MessageBoxResult.No:
					break;
				default:
					return false;
			}
        }
		if (!App.MainView.IsDocnoValid || !App.MainView.IsBLNoValid)
		{
			App.MainView.IsBLNoValid = true;
			App.MainView.OpenDocnoWindow(sender, e);
			return false;
		};
		App.MainView.RemoveQrCodePage();
		App.MainView.RemoveInputPaper();
		if(App.MainView.SubWindowController.InputWindow == null)
        {
			App.MainView.SubWindowController.OpenInputWindow(sender, e);
		}
		var view = App.MainView.SubWindowController.InputWindow.View;
		view.GenerateStateOfAccountDoc();
		view.GenerateSpecificationDoc();
		view.GenerateInputPaper();
		view.GenerateQrPages();
		if (App.IVEditMainWindow != null)
		{
			App.IVEditMainWindow.Activate();
		}
		return true;
	}
	private static void saveShprAdrress()
    {
		System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
		dialog.Filter = "textファイル|*.txt";
		string path = "";
		if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
		{
			path = dialog.FileName;
			if (File.Exists(path))
			{
				File.Delete(path);
			}

			using (FileStream fs = File.Create(path))
            {
				_addText(fs, App.MainView.DeclArtcile.ShprName + "\r\n");
				_addText(fs, App.MainView.DeclArtcile.ShprPref + "\r\n");
				_addText(fs, App.MainView.DeclArtcile.ShprMuni + "\r\n");
				_addText(fs, App.MainView.DeclArtcile.ShprAddr1 + "\r\n");
				_addText(fs, App.MainView.DeclArtcile.ShprAddr2 + "\r\n");
				_addText(fs, App.MainView.DeclArtcile.ShprZipCode + "\r\n");
				_addText(fs, App.MainView.DeclArtcile.ShprTelephone);
			}
		}
		else
		{
			return;
		}
	}

	private static void _addText(FileStream fs, string value)
	{
		byte[] info = new UTF8Encoding(true).GetBytes(value);
		fs.Write(info, 0, info.Length);
	}

	public static void LoadShpr(object sender, RoutedEventArgs e)
    {
		System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
		dialog.Filter = "textファイル|*.txt";
		string path = "";
		if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
		{
			path = dialog.FileName;
			if (File.Exists(path))
			{
				string temp = "";
				StringBuilder sb = new StringBuilder();
				using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
				{
					temp = sr.ReadToEnd();
				}
				string[] shprInfo = temp.Split(new string[]{ "\r\n"},StringSplitOptions.None);
				if (shprInfo.Length > 6)
				{
					App.MainView.DeclArtcile.ShprName = shprInfo[0];
					App.MainView.DeclArtcile.ShprPref = shprInfo[1];
					App.MainView.DeclArtcile.ShprMuni = shprInfo[2];
					App.MainView.DeclArtcile.ShprAddr1 = shprInfo[3];
					App.MainView.DeclArtcile.ShprAddr2 = shprInfo[4];
					App.MainView.DeclArtcile.ShprZipCode = shprInfo[5];
					App.MainView.DeclArtcile.ShprTelephone = shprInfo[6];
				}
				App.MainView.DeclArtcile.IsShprOverride = false;

			}

		}
	}
	public static void ForceAnbun(object sender, RoutedEventArgs e)
    {
		double ttlnetweight = 0.0;
		if (!double.TryParse(App.MainView.DeclArtcile.IVNetWeight,out ttlnetweight)) return;
		if (ttlnetweight <= 0.0) return;
		if (App.MainView.DeclArtcile.DeclElements == null || App.MainView.DeclArtcile.DeclElements.Count == 0) return;
		var rates = new SortedList<string, double>();
		foreach(var cur in App.MainView.CurrencyProfiles)
        {
			double rate = 0.0;
			double.TryParse(cur.Rate, out rate);
			rates.Add(cur.IsPrime ? "" : cur.Code, rate);
        }
		double ttl = 0.0;
		foreach(var decl in App.MainView.DeclArtcile.DeclElements)
        {
			if (rates.ContainsKey(decl.Currency))
            {
				double bpr = 0.0;
				double.TryParse(decl.BasicPrice, out bpr);
				ttl += rates[decl.Currency] * bpr;
            }
        }
		if (ttl == 0.0) return;
		foreach (var decl in App.MainView.DeclArtcile.DeclElements)
		{
			if (rates.ContainsKey(decl.Currency))
			{
				double bpr = 0.0;
				double.TryParse(decl.BasicPrice, out bpr);
				bpr = rates[decl.Currency] * bpr;
				if(decl.Unit1 == "KG" || decl.Unit1 == "KGIC" || decl.Unit1 == "KGII")
                {
					decl.Quantity1 = String.Format("{0:0.000}", (bpr / ttl) * ttlnetweight);
					decl.Quantity1 = decl.Quantity1.Length>1 ? decl.Quantity1.Substring(0, decl.Quantity1.Length - 1) : decl.Quantity1;
				}
				if (decl.Unit2 == "KG" || decl.Unit2 == "KGIC" || decl.Unit2 == "KGII")
				{
					decl.Quantity2 = String.Format("{0:0.000}", (bpr / ttl) * ttlnetweight);
					decl.Quantity2 = decl.Quantity2.Length > 1 ? decl.Quantity2.Substring(0, decl.Quantity2.Length - 1) : decl.Quantity2;
				}
			}
		}
		App.MainView.DeclArtcile.IVNetWeight = "";
	}
}
