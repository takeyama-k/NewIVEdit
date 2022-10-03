using System;
using System.Windows;
using System.Windows.Controls;
namespace NewIVEdit
{
	public class AnnotationTemplateSelector : DataTemplateSelector
	{
		public DataTemplate QrImageTemplate { set; get; }
		public DataTemplate TextBlockTemplate { set; get; }
		public DataTemplate TextBlockTemplateWViewBox { set; get; }
		public DataTemplate TextBoxTemplate { set; get; }
		public DataTemplate LineTemplate { set; get; }
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item.GetType() == typeof(QrImageAnnotation))
			{
				return QrImageTemplate;
			}
			else if (item.GetType() == typeof(TextBlockAnnotation))
			{
				if ((item as TextBlockAnnotation).IsViewBox)
				{
					return TextBlockTemplateWViewBox;
				}
				else
				{
					return TextBlockTemplate;
				}
			}
			else if (item.GetType() == typeof(TextBoxAnnotation))
			{
				return TextBoxTemplate;
			}
			else if (item.GetType() == typeof(LineAnnotation))
			{
				return LineTemplate;
			}
			return null;
		}
	}
	public class PageNaviTemplateSelector : DataTemplateSelector
	{
		public DataTemplate ThumbneilTemplate { set; get; }
		public DataTemplate ThumbneilTemplateWCaption { set; get; }
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item.GetType() == typeof(PageThumbneil))
			{
				if ((item as PageThumbneil).ThumbneilImage != null)
				{
					return ThumbneilTemplate;
				}else
                {
					return ThumbneilTemplateWCaption;

				}
			}
			return null;
		}
	}
}
