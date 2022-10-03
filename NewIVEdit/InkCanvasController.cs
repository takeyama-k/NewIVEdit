using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Ink;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using NewIVEdit;

public class InkCanvasController
{
	public ObservableSortedCollection<StrokeProfile> StrokeProfiles { set; get; } = new ObservableSortedCollection<StrokeProfile>();
	public InkCanvasController()
	{
		StrokeProfiles.Add(new StrokeProfile
		{
			Icon = (BitmapSource)App.Current.TryFindResource("sharpPenIcon"),
			Priority = 0,
			DA = new DrawingAttributes() { Color = Colors.Black, Width = 3.0, Height = 3.0 }
		});
		StrokeProfiles.Add(new StrokeProfile
		{
			Icon = (BitmapSource)App.Current.TryFindResource("boldPenIcon"),
			Priority = 1,
			DA = new DrawingAttributes() { Color = Colors.Black, Width = 10, Height = 10 }
		});
		StrokeProfiles.Add(new StrokeProfile
		{
			isEraser = true,
			Icon = (BitmapSource)App.Current.TryFindResource("eraserIcon"),
			Priority = 2,
			DA = new DrawingAttributes() { Color = Colors.Black, Width = 3.0 }
		});
		StrokeProfiles.Add(new StrokeProfile
		{
			Icon = (BitmapSource)App.Current.TryFindResource("redPenIcon"),
			Priority = 3,
			DA = new DrawingAttributes() { Color = Colors.Red, Width = 5, Height = 5 }
		});
		StrokeProfiles.Add(new StrokeProfile
		{
			Icon = (BitmapSource)App.Current.TryFindResource("greenMarkerIcon"),
			Priority = 4,
			DA = new DrawingAttributes() { Color = Colors.Green, Width = 15, Height=15, IsHighlighter=true }
		}); 

		StrokeProfiles.Add(new StrokeProfile
		{
			Icon = (BitmapSource)App.Current.TryFindResource("yellowMarkerIcon"),
			Priority = 5,
			DA = new DrawingAttributes() { Color = Colors.Yellow, Width = 15, Height=15, IsHighlighter = true }
		});
		StrokeProfiles.Add(new StrokeProfile
		{
			Icon = (BitmapSource)App.Current.TryFindResource("orangeMarkerIcon"),
			Priority = 6,
			DA = new DrawingAttributes() { Color = Colors.Orange, Width = 15, Height=15, IsHighlighter = true }
		});

	}

	public void ChangeStroke(StrokeProfile stroke)
    {
		if (stroke.isEraser)
		{
			App.IVEditMainWindow.InkCanvas.EditingMode = System.Windows.Controls.InkCanvasEditingMode.EraseByStroke;
		}
		else
		{
			App.IVEditMainWindow.InkCanvas.EditingMode = System.Windows.Controls.InkCanvasEditingMode.Ink;
			App.IVEditMainWindow.InkCanvas.DefaultDrawingAttributes = stroke.DA;
		}
    }

	public class StrokeProfile : IComparable<StrokeProfile>, IIndexed, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

	private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
	{
		if (PropertyChanged != null)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
		public bool isEraser { set; get; } = false;
		public DrawingAttributes DA { set; get; }
		public int Priority { set; get; }
		public int IndexNo { set; get; }
		public BitmapSource Icon { set; get; }
		public int CompareTo(StrokeProfile that)
        {
			if(this.Priority < that.Priority)
            {
				return 1;
            }else if(this.Priority > that.Priority)
            {
				return -1;
            }
            else
            {
				return 0;
            }
        }
		public int Compare(StrokeProfile _this,StrokeProfile that)
		{
			if (_this.Priority < that.Priority)
			{
				return 1;
			}
			else if (_this.Priority > that.Priority)
			{
				return -1;
			}
			else
			{
				return 0;
			}
		}
	}
}
