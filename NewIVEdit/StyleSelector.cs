using System;
using System.Windows;
using System.Windows.Controls;

namespace NewIVEdit
{
	public class AnnotationStyleSelector : StyleSelector
	{
		public Style QrImageStyle { set; get; }
		public Style TextBlockStyle { set; get; }
		public Style TextBoxStyle { set; get; }
		public Style LineStyle { set; get; }
		public AnnotationStyleSelector()
		{
		}
		public override Style SelectStyle(object item, DependencyObject container)
		{
			if (item.GetType() == typeof(QrImageAnnotation))
			{
				return QrImageStyle;
			}
			else if (item.GetType() == typeof(TextBlockAnnotation))
            {
				return TextBlockStyle;
			}
			else if (item.GetType() == typeof(TextBoxAnnotation))
			{
				return TextBoxStyle;
			}
			else if (item.GetType() == typeof(LineAnnotation))
			{
				return LineStyle;
			}
			return null;

		}
	}

	public class PageNaviStyleSelector : StyleSelector
	{
		public Style ThumbneilStyle { set; get; }
		public PageNaviStyleSelector()
		{
		}
		public override Style SelectStyle(object item, DependencyObject container)
		{
			if (item.GetType() == typeof(PageThumbneil))
			{
				return ThumbneilStyle;
			}
			return null;
		}
	}
}
