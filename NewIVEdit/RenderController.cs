using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using NewIVEdit;
using System.Windows.Media;
using System.Windows;
using System.Timers;
using System.Windows.Media.Imaging;

public class RenderController : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private void NofityPropertyChanged([CallerMemberName] String propertyName = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    private static Point _startPoint;
    private const double ROOT3 = 1.73205080757;

    private Timer _leftPushdowntimer = new Timer(1000);
    private Point _leftPushdownstartpos = new Point();
    private System.Windows.Controls.Canvas _penSelector = null;
    public bool MouseCaptured { set; get; } = false;
    public ConstValue.EditingModeEnum CurrentEditingMode { set; get; } = ConstValue.EditingModeEnum.Hand;
	public RenderController()
	{
        _leftPushdowntimer.Elapsed += leftPushDownElapsed;
	}

    private void leftPushDownElapsed(object sender, ElapsedEventArgs e)
    {
        App.IVEditMainWindow.Dispatcher.Invoke(
            () =>
            {
                DrawStrokeStyleSelector();
            });
    }

    private void DrawStrokeStyleSelector()
    {
        var canvas = new System.Windows.Controls.Canvas()
        {
            Width = 504,
            Height = 504,
            Background = new SolidColorBrush(Colors.Transparent),
            Name = "PenSelector"
        };

        var maincanvas = App.IVEditMainWindow.MainCanvas;
        maincanvas.Children.Add(canvas);

        if (_penSelector != null)
        {
            maincanvas.Children.Remove(_penSelector);
        }
        System.Windows.Controls.Canvas.SetZIndex(canvas, 2);
        System.Windows.Controls.Canvas.SetLeft(canvas, _leftPushdownstartpos.X - canvas.Width / 2.0);
        System.Windows.Controls.Canvas.SetTop(canvas, _leftPushdownstartpos.Y - canvas.Height / 2.0);
        _penSelector = canvas;


        int cntr = 0;
        foreach (var stroke in App.MainView.IinkCanvasController.StrokeProfiles)
        {
            double a = canvas.Width / 6.0;
            var btn = new System.Windows.Controls.Button
            {
                Width = 2 * a,
                Height = 2 * a,
                Background = new SolidColorBrush(Colors.Transparent),
                BorderThickness = new Thickness(0, 0, 0, 0),
                
            };
            btn.Click += (sender, e) =>
            {
                App.MainView.IinkCanvasController.ChangeStroke(stroke);
                maincanvas.Children.Remove(_penSelector);
            };
            var img = new System.Windows.Controls.Image
            {
                Width = a * 2.0,
                Height = a * 2.0,
                Source = stroke.Icon,
            };
            switch (cntr)
            {
                case 0:
                    
                    canvas.Children.Add(btn);
                    System.Windows.Controls.Canvas.SetZIndex(btn, 3);
                    System.Windows.Controls.Canvas.SetLeft(btn, a * 2.0);
                    System.Windows.Controls.Canvas.SetTop(btn, a * 2.0);
                    canvas.Children.Add(img);
                    System.Windows.Controls.Canvas.SetZIndex(img, 2);
                    System.Windows.Controls.Canvas.SetLeft(img, a * 2.0);
                    System.Windows.Controls.Canvas.SetTop(img, a * 2.0);
                    break;
                case 1:
                    canvas.Children.Add(btn);
                    System.Windows.Controls.Canvas.SetZIndex(btn, 3);
                    System.Windows.Controls.Canvas.SetLeft(btn, a * 2.0 - ROOT3 * a);
                    System.Windows.Controls.Canvas.SetTop(btn, 0);
                    canvas.Children.Add(img);
                    System.Windows.Controls.Canvas.SetZIndex(img, 2);
                    System.Windows.Controls.Canvas.SetLeft(img, a * 2.0 - ROOT3 * a);
                    System.Windows.Controls.Canvas.SetTop(img, 0);
                    break;
                case 2:
                    canvas.Children.Add(btn);
                    System.Windows.Controls.Canvas.SetZIndex(btn, 3);
                    System.Windows.Controls.Canvas.SetLeft(btn, a * 2.0 + ROOT3 * a);
                    System.Windows.Controls.Canvas.SetTop(btn, 0);
                    canvas.Children.Add(img);
                    System.Windows.Controls.Canvas.SetZIndex(img, 2);
                    System.Windows.Controls.Canvas.SetLeft(img, a * 2.0 + ROOT3 * a);
                    System.Windows.Controls.Canvas.SetTop(img, 0);
                    break;
                case 3:
                    canvas.Children.Add(btn);
                    System.Windows.Controls.Canvas.SetZIndex(btn, 3);
                    System.Windows.Controls.Canvas.SetLeft(btn,0);
                    System.Windows.Controls.Canvas.SetTop(btn, a * 2.0);
                    canvas.Children.Add(img);
                    System.Windows.Controls.Canvas.SetZIndex(img, 2);
                    System.Windows.Controls.Canvas.SetLeft(img, 0);
                    System.Windows.Controls.Canvas.SetTop(img, a* 2.0);
                    break;
                case 4:
                    canvas.Children.Add(btn);
                    System.Windows.Controls.Canvas.SetZIndex(btn, 3);
                    System.Windows.Controls.Canvas.SetLeft(btn, a * 4.0);
                    System.Windows.Controls.Canvas.SetTop(btn, a* 2.0);
                    canvas.Children.Add(img);
                    System.Windows.Controls.Canvas.SetZIndex(img, 2);
                    System.Windows.Controls.Canvas.SetLeft(img, a * 4.0);
                    System.Windows.Controls.Canvas.SetTop(img, a * 2.0);
                    break;
                case 5:
                    canvas.Children.Add(btn);
                    System.Windows.Controls.Canvas.SetZIndex(btn, 3);
                    System.Windows.Controls.Canvas.SetLeft(btn, a*2.0 - ROOT3 * a);
                    System.Windows.Controls.Canvas.SetTop(btn, a * 4.0);
                    canvas.Children.Add(img);
                    System.Windows.Controls.Canvas.SetZIndex(img, 2);
                    System.Windows.Controls.Canvas.SetLeft(img, a * 2.0 - ROOT3 * a);
                    System.Windows.Controls.Canvas.SetTop(img, a * 4.0);
                    break;
                case 6:
                    canvas.Children.Add(btn);
                    System.Windows.Controls.Canvas.SetZIndex(btn, 3);
                    System.Windows.Controls.Canvas.SetLeft(btn, a * 2.0 + ROOT3 * a);
                    System.Windows.Controls.Canvas.SetTop(btn, a * 4.0);
                    canvas.Children.Add(img);
                    System.Windows.Controls.Canvas.SetZIndex(img, 2);
                    System.Windows.Controls.Canvas.SetLeft(img, a * 2.0 + ROOT3 * a);
                    System.Windows.Controls.Canvas.SetTop(img, a * 4.0);
                    break;
                default:
                    break;
            }
            cntr++;
        }

    }
    public void ScaleMain(object sender, MouseWheelEventArgs e)
    {
        const double scale = 1.2;
        var matrix = App.IVEditMainWindow.MainCanvas.RenderTransform.Value;
        if (e.Delta > 0)
        {
            matrix.ScaleAt(scale, scale, e.GetPosition(App.IVEditMainWindow.MainFrame).X, e.GetPosition(App.IVEditMainWindow.MainFrame).Y);
            App.MainView.CurrentPage.CurrentScale *= scale;

        }
        else
        {
            matrix.ScaleAt(1.0 / scale, 1.0 / scale, e.GetPosition(App.IVEditMainWindow.MainFrame).X, e.GetPosition(App.IVEditMainWindow.MainFrame).Y);
            App.MainView.CurrentPage.CurrentScale *= 1.0 / scale;
        }
        App.IVEditMainWindow.MainCanvas.RenderTransform = new MatrixTransform(matrix);
    }

    public void MainLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        dragMainStart(sender, e);
    }
    public void MainLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        dragMainEnd(sender, e);
    }

    public void InkCanvasLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (_penSelector != null)
        {
            App.IVEditMainWindow.MainCanvas.Children.Remove(_penSelector);
            _penSelector = null;
        }
        _leftPushdowntimer.Start();
        var matrix = App.IVEditMainWindow.MainCanvas.RenderTransform.Value;
        matrix.Invert();
        _leftPushdownstartpos = matrix.Transform(e.GetPosition(App.IVEditMainWindow.MainFrame));

    }
    public void InkCanvasLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _leftPushdowntimer.Stop();
        _leftPushdownstartpos = new Point();

    }

    private void dragMainStart(object sender, MouseButtonEventArgs e)
    {
        Point curpos = default;
        curpos = e.GetPosition(App.IVEditMainWindow.MainFrame);
        if (curpos == default) return;
        _startPoint = curpos;
        MouseCaptured = true;
    }

    public void DragMain(object sender, MouseEventArgs e)
    {
        if (_leftPushdowntimer.Enabled)
        {
            var matrix = App.IVEditMainWindow.MainCanvas.RenderTransform.Value;
            matrix.Invert();
            var currentpos = matrix.Transform(e.GetPosition(App.IVEditMainWindow.MainFrame));
            var width = Math.Abs(_leftPushdownstartpos.X - currentpos.X);
            var height = Math.Abs(_leftPushdownstartpos.Y - currentpos.Y);
            var dist = Math.Sqrt(width * width + height * height);
            if(dist > 30)
            {
                _leftPushdowntimer.Stop();
                _leftPushdownstartpos = new Point();
            }
        }
        if (MouseCaptured)
        {
            var matrix = App.IVEditMainWindow.MainCanvas.RenderTransform.Value;
            Vector v = _startPoint - e.GetPosition(App.IVEditMainWindow.MainFrame);
            matrix.Translate(-v.X, -v.Y);
            App.IVEditMainWindow.MainCanvas.RenderTransform = new MatrixTransform(matrix);
            App.MainView.CurrentPage.CurrentMatrix = matrix;
            _startPoint = e.GetPosition(App.IVEditMainWindow.MainFrame);
        }
    }
    private void dragMainEnd(object sender, MouseButtonEventArgs e)
    {
        MouseCaptured = false;
    }
}
