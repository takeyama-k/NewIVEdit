using System;
namespace NewIVEdit
{
	public static class FundamentalSettings
	{
		public static int ShorterPixels { set; get; } = 2048;
		public static int DocNoBarcodeX { set; get; } = 50;
		public static int DocNoBarcodeY { set; get; } = 100;
		public static int DocNoBcdWidth { set; get; } = 700;
		public static int DocNoBcdHeight { set; get; } = 300;


		public static float ShorterPixelsIn72DPI { set; get; } = 595.0f;

		public static float LongerPixelsIn72DPI { set; get; } = 847.0f;

		public static float HoujinStartXIn72DPI { set; get; } = 125.0f;

		public static float HoujinStartYIn72DPI { set; get; } = 260.0f;

		public static float HoujinWidthIn72DPI { set; get; } = 210.0f;

		public static float HoujinHeightIn72DPI { set; get; } = 86.0f;

		public static int OcrMarginPx { set; get; } = 100;

		public static int ReOcrMarginPx { set; get; } = 15;

		static FundamentalSettings()
		{
		}
	}
}
