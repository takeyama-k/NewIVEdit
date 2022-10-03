using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NewIVEdit;
using OpenCvSharp;
using System.Linq;
using cv2 = OpenCvSharp.Cv2;
public static class UtilityFunc 
{
	static UtilityFunc()
	{
		regexModokiDictionary.Add('+'.ToString(), @"[0-9]{{{0},{1}}}");
		regexModokiDictionary.Add('@'.ToString(), @"[A-Z]{{{0},{1}}}");
		regexModokiDictionary.Add('*'.ToString(), @"[A-Z0-9]{{{0},{1}}}");
		regexModokiDictionary.Add('?'.ToString(), @"[A-Z0-9 ]{{{0},{1}}}");
		regexModokiDictionary.Add('&'.ToString(), @"[a-zA-Z0-9 ]{{{0},{1}}}");
		regexModokiDictionary.Add('!'.ToString(), @"[A-Z0-9\-]{{{0},{1}}}");
		regexModokiDictionary.Add('='.ToString(), @"[a-zA-Z0-9\-]{{{0},{1}}}");
		regexModokiDictionary.Add('#'.ToString(), @"[A-Z0-9!-/:-@¥[-`{{-~]{{{0},{1}}}");
		regexModokiDictionary.Add('%'.ToString(), @".{{{0},{1}}}");
		regexModokiDictionary.Add('$'.ToString(), @"[0-9,.]{{{0},{1}}}");
		regexModokiDictionary.Add('\\'.ToString(), @"[0-9,]{{{0},{1}}}");
		regexModokiDictionary.Add(':'.ToString(), @"[^0-9]{{{0},{1}}}");
		regexModokiDictionary.Add(';'.ToString(), @"[^0-9A-Z]{{{0},{1}}}");
		regexModokiDictionary.Add('~'.ToString(), @"[^0-9a-zA-Z]{{{0},{1}}}");
		replaceDictionary.Add(new Tuple<string, string>("<escape>", "\\"));
		replaceDictionary.Add(new Tuple<string, string>("<space>", "\\s"));
		replaceDictionary.Add(new Tuple<string, string>("<linebreak>", "\\n"));
		replaceDictionary.Add(new Tuple<string, string>("<linebreak2>", "\\r\\n"));
		replaceDictionary.Add(new Tuple<string, string>("<colon>", @":"));
		replaceDictionary.Add(new Tuple<string, string>("<semicolon>", @";"));
		replaceDictionary.Add(new Tuple<string, string>("<slash>", @"\/"));
		replaceDictionary.Add(new Tuple<string, string>("<equal>", @"="));
		replaceDictionary.Add(new Tuple<string, string>("<parcent>", @"%"));
		replaceDictionary.Add(new Tuple<string, string>("<dot>", @"\."));
		replaceDictionary.Add(new Tuple<string, string>("<question>", "?"));
		replaceDictionary.Add(new Tuple<string, string>("<lit_question>", @"\?"));
		replaceDictionary.Add(new Tuple<string, string>("<dollar>", "$"));
		replaceDictionary.Add(new Tuple<string, string>("<lit_dollar>", @"\$"));
		replaceDictionary.Add(new Tuple<string, string>("<lpharen>", @"\("));
		replaceDictionary.Add(new Tuple<string, string>("<rpharen>", @"\)"));
		replaceDictionary.Add(new Tuple<string, string>("<plus>", "+"));
		replaceDictionary.Add(new Tuple<string, string>("<lit_plus>", @"\+"));
		replaceDictionary.Add(new Tuple<string, string>("<asterisk>", "*"));
		replaceDictionary.Add(new Tuple<string, string>("<lit_asterisk>", @"\*"));
		replaceDictionary.Add(new Tuple<string, string>("<bar>", "|"));
		replaceDictionary.Add(new Tuple<string, string>("<or>", "|"));
		replaceDictionary.Add(new Tuple<string, string>("<nogroup>", "?:"));
	}

	private static Dictionary<string, string> regexModokiDictionary = new Dictionary<string, string>();
	private static List<Tuple<string, string>> replaceDictionary = new List<Tuple<string, string>>();
	private static char mustChar = '|';
	public static Regex ParseRegexModoki(string pattern,string prefix_pattern,string postfix_pattern,bool isCompmatch ,int  fuzzyLevel,bool isExcel = true)
	{
		string full_pattern = "";
		if (isExcel)
		{
			full_pattern = compilePattern(prefix_pattern, true) + compilePattern(pattern, false) + compilePattern(postfix_pattern, true);
		}
		else
		{
			full_pattern = compilePattern(prefix_pattern, false) + compilePattern(pattern, false) + compilePattern(postfix_pattern, false);
		}
		if (isCompmatch)
        {
			full_pattern = @"^" + full_pattern + @"$";
        }
		return new Regex(full_pattern, RegexOptions.Compiled);
    }

	private static string compilePattern (string pattern,bool isIgnored)
    {
		if (pattern == null || pattern.Length == 0) return "";

		pattern += '\0'.ToString();

		StringBuilder sb = new StringBuilder();
		int cntr = 0;
		int mustcntr = 0;
		char prev = '\0';
		bool isMust = false;

		foreach (char c in pattern)
		{
			if (c != prev)
			{
				if (c == mustChar && !isMust)
				{
					if (!regexModokiDictionary.ContainsKey(prev.ToString()))
					{
						cntr = 0;
						mustcntr = 0;
						if (prev != '\0') sb.Append(prev.ToString());
					}
					isMust = true;
				}
				else if (c == mustChar && isMust)
				{
					if (regexModokiDictionary.ContainsKey(prev.ToString()))
					{
						sb.Append(String.Format(regexModokiDictionary[prev.ToString()], mustcntr, cntr));
						cntr = 0;
						mustcntr = 0;
					}
					else if (prev != '\0')
					{
						sb.Append(prev.ToString());
					}
				}
				else if (prev == mustChar) // c!= mustChar && isMust = true;
				{
					if (regexModokiDictionary.ContainsKey(c.ToString()))
					{
						cntr++;
						mustcntr++;
					}
				}
				else if (prev != mustChar) //c!=mustchar
				{
					if (prev == '\0')
					{
						if (regexModokiDictionary.ContainsKey(c.ToString()))
						{
							cntr++;
							if (isMust) mustcntr++;
						}
						prev = c;
						continue;
					}
					else if (regexModokiDictionary.ContainsKey(prev.ToString()))
					{
						sb.Append(String.Format(regexModokiDictionary[prev.ToString()], mustcntr, cntr));
						if (regexModokiDictionary.ContainsKey(c.ToString()))
						{
							cntr = 1;
							mustcntr = 0;
						}
					}
					else
					{
						if (regexModokiDictionary.ContainsKey(c.ToString()))
						{
							cntr = 1;
							mustcntr = 0;
						}
						if (prev != '\0') sb.Append(prev.ToString());
					}
					isMust = false;
				}
			}
			else
			{
				if (regexModokiDictionary.ContainsKey(c.ToString()))
				{
					cntr++;
					if (isMust) mustcntr++;
				}
				else if (prev != '\0')
				{
					sb.Append(prev.ToString());
				}
			}
			prev = c;
		}
        if (isIgnored)
        {
			sb = new StringBuilder(@"(?:").Append(sb.ToString()).Append(@")");
        }
        else
        {
			sb = new StringBuilder(@"(").Append(sb.ToString()).Append(@")");
		}
		string res = sb.ToString();
		foreach (var repl in replaceDictionary)
		{
			res = Regex.Replace(res, repl.Item1, repl.Item2);
		}
		return res;
	}
	public static Bitmap BitmapSourceToBitmap(BitmapSource source)
    {
		// 処理
		var bitmap = new System.Drawing.Bitmap(
			source.PixelWidth,
			source.PixelHeight,
			System.Drawing.Imaging.PixelFormat.Format32bppRgb
		);
		var bitmapData = bitmap.LockBits(
			new System.Drawing.Rectangle(System.Drawing.Point.Empty, bitmap.Size),
			System.Drawing.Imaging.ImageLockMode.WriteOnly,
			System.Drawing.Imaging.PixelFormat.Format32bppRgb
		) ;
		source.CopyPixels(
			System.Windows.Int32Rect.Empty,
			bitmapData.Scan0,
			bitmapData.Height * bitmapData.Stride,
			bitmapData.Stride
		);
		bitmap.UnlockBits(bitmapData);
		return bitmap;
	}
	public static BitmapSource BitmapToBitmapSource(Bitmap bitmap)
	{
		var bitmapData = bitmap.LockBits(
			new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
			System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);
		PixelFormat format;
		if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
		{
			format = PixelFormats.Rgb24;
		}
		else
		{
			format = PixelFormats.Bgra32;
		}
		var bitmapSource = BitmapSource.Create(
			bitmapData.Width, bitmapData.Height,
			bitmap.HorizontalResolution, bitmap.VerticalResolution,
			format, null,
			bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

		bitmap.UnlockBits(bitmapData);

		return bitmapSource;
	}
	public static bool ExistsField(string name)
    {
		if (Enum.IsDefined(typeof(ConstValue.FieldNameEnum), name)) return true;
		if (App.MainView.InvoiceBucketData.BucketDataColumn.ContainsKey(name)) return true;
		if (App.MainView.SideChannelData.ContainsKey(name)) return true;
		return false;
    }

	public static int ComapreValue(string _this, string that)
    {
		double tempThis, tempThat;
		if (!(double.TryParse(_this, out tempThis) && double.TryParse(that, out tempThat)))
        {
			return String.Compare(that, _this);
        }
        else
        {
			if (tempThis < tempThat) return 1;
			else if (tempThis > tempThat) return -1;
			else return 0;
        }
    }

	public static string ClenseTextAsAlpha(string input)
    {
		if (input == null) return "";
		input = input.ToUpper();
		input = ConstValue.Re_CovertHyphenLikeCharacter.Replace(input, "-");
		return ConstValue.Re_DeleteExAlphaDigitHyphen.Replace(input, "");
    }
	public static string ClenseTextAsNumber(string input)
	{
		if (input == null) return "";
		return ConstValue.Re_DeleteExDigit.Replace(input, "");
	}

	private static OpenCvSharp.Point[][] calcContours(Mat target)
    {
		var dest = new Mat();
		target.CopyTo(dest);
		cv2.CvtColor(dest, dest, ColorConversionCodes.BGR2GRAY);
		cv2.GaussianBlur(dest, dest, new OpenCvSharp.Size(9, 9), 0);
		cv2.Threshold(dest, dest, 0, 255, ThresholdTypes.Otsu);
		cv2.BitwiseNot(dest, dest);
		var kernel = cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(6, 1));
		cv2.Dilate(dest, dest, kernel, new OpenCvSharp.Point(-1, -1), 5);
		OpenCvSharp.Point[][] contours;
		OpenCvSharp.HierarchyIndex[] hierarchy;
		cv2.FindContours(dest, out contours, out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxSimple);
		return contours;
	}
	public static Tuple<System.Windows.Point, Tuple<int,int>> DetectRange(Mat target)
    {
		var contours = calcContours(target);
		if (contours == null || contours.Length == 0) return null;
		else
		{
			float resWidth = 0.0F;
			float resHeight = 0.0F;
			System.Windows.Point resPoint = new System.Windows.Point(0, 0);
			double cx = target.Width / 2;
			double cy = target.Height / 2;
			Point2f[] rectPoints = new Point2f[1] ;
			var filterd = contours.Select(x => x).Where(x => cv2.ContourArea(x) > 200);
			if (filterd.ToArray().Count() == 0) return null;
			else
			{
				var nearest = (filterd.OrderBy(
					contour =>
					{
						var mom = cv2.Moments(contour);
						double x = mom.M10 / mom.M00;
						double y = mom.M01 / mom.M00;
						return Math.Sqrt((cx - x) * (cx - x) + (cy - y) * (cy - y));
					}
					)).ToArray()[0];

				var minRect = cv2.MinAreaRect(nearest);
				rectPoints = cv2.BoxPoints(minRect);
				resPoint = new System.Windows.Point(Math.Min(rectPoints[0].X, Math.Min(rectPoints[1].X, rectPoints[2].X)), Math.Min(rectPoints[0].Y, Math.Min(rectPoints[1].Y, rectPoints[2].Y)));
				resWidth = rectPoints[0].X != rectPoints[1].X ? Math.Max(rectPoints[0].X, rectPoints[1].X) - Math.Min(rectPoints[0].X, rectPoints[1].X) : Math.Max(rectPoints[0].X, rectPoints[2].X) - Math.Min(rectPoints[0].X, rectPoints[2].X);
				resHeight = rectPoints[0].Y != rectPoints[1].Y ? Math.Max(rectPoints[0].Y, rectPoints[1].Y) - Math.Min(rectPoints[0].Y, rectPoints[1].Y) : Math.Max(rectPoints[0].Y, rectPoints[2].Y) - Math.Min(rectPoints[0].Y, rectPoints[2].Y);
			}
			return Tuple.Create(resPoint, Tuple.Create((int)resWidth, (int)resHeight));
		}

	}
	public static double CalcTilt(Mat target)
    {
		double cx = target.Width / 2;
		double cy = target.Height / 2;
		var dest = new Mat();
		target.CopyTo(dest);
		cv2.CvtColor(dest, dest, ColorConversionCodes.BGR2GRAY);
		cv2.GaussianBlur(dest, dest,new OpenCvSharp.Size(9,9),0);
		cv2.Threshold(dest, dest, 0, 255, ThresholdTypes.Otsu);
		cv2.BitwiseNot(dest, dest);
		var kernel = cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(6, 1));
		cv2.Dilate(dest, dest, kernel,new OpenCvSharp.Point(-1,-1),5);
		var bmp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(dest);
		OpenCvSharp.Point[][] contours;
		OpenCvSharp.HierarchyIndex[] hierarchy;
		cv2.FindContours(dest, out contours, out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxSimple);
		double res = 0.0;
		if (contours == null || contours.Length == 0) res =  0.0;
        
		else
        
		{
			var filterd = contours.Select(x => x).Where(x => cv2.ContourArea(x) > 200);
			if (filterd.ToArray().Count() == 0) res = 0.0;
			else
			{
				var nearest = (filterd.OrderBy(
					contour =>
					{
						var mom = cv2.Moments(contour);
						double x = mom.M10 / mom.M00;
						double y = mom.M01 / mom.M00;
						return Math.Sqrt((cx - x) * (cx - x) + (cy - y) * (cy - y));
					}
					)).ToArray()[0];
				var minRect = cv2.MinAreaRect(nearest);
				res = minRect.Angle;
			}
        }
		return res;
    }

	public static double CalcTiltByPoint(double x1, double y1, double x2, double y2)
    {
		if(x1 > x2)
        {
			double temp = x1;
			x1 = x2;
			x2 = temp;
			temp = y1;
			y1 = y2;
			y2 = temp;
        }
		var rad = Math.Atan2(y2 - y1, x2 - x1);
		if (rad is double.NaN) return 0.0;
		else return rad * 180 / Math.PI;
    }

	public static List<Tuple<int,int>> FuzzyPatternMatch(string input, string pattern, int lebDist)
    {
		var res = new List<Tuple<int, int>>();
		//bitap法でパタンマッチ
		//lebDist = レーベンシュタイン距離
		//lebDIst は　DB上の fuzzy_levelに該当する。
		//マッチ位置と長さのペアのリストを返却する。
		return res;
    }
	public static long LongRound(long value)
    {
		if (value % 10 < 5) value -= value % 10;
        else
        {
			value += (10 - value % 10);
        }
		return value;
    }
}
