using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DrawCurve
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
 
        public partial class MainWindow : Window
        {
            public MainWindow()
            {
                InitializeComponent();
       
        }
        List<Protractor> _protractorList = new List<Protractor>();
     
        //  Canvas _protractorCanvas;
        Protractor protractor;
            private void Button_Click(object sender, RoutedEventArgs e)
            {
            var assemblyPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            /// placing the protractor tool in a canvas
            protractor = new Protractor();

            protractor.id = _protractorList.Count + 1.ToString() ;
                Canvas _protractorCanvas = new Canvas();
                _protractorCanvas.Width = 400;
                _protractorCanvas.Height = 200;
            _protractorCanvas.Tag = protractor.id;
                Path _protractorPath = new Path();
                _protractorPath.Width = 400;
                _protractorPath.Height = 200;
                _protractorPath.Stretch = Stretch.Uniform;
                _protractorPath.Data = Geometry.Parse(@"M 0 50 A 50 50 0 0 1 100 50Z");

                /// adding protrator image to canvas
                ImageBrush _protractorImage = new ImageBrush();
                _protractorImage.ImageSource = new BitmapImage(new Uri(assemblyPath + @"\Protractor.png", UriKind.Relative));
                _protractorPath.Fill = _protractorImage;
                _protractorImage.Stretch = Stretch.UniformToFill;
                _protractorCanvas.Children.Add(_protractorPath);


                _protractorCanvas.MouseMove += _protractorCanvas_MouseMove;

                /// adding a grid (arrow mark and line for arc) to canvas
                Grid _gridArcLine = new Grid();
                _gridArcLine.Height = 20;
                ColumnDefinition c1 = new ColumnDefinition();
                c1.Width = new GridLength(20, GridUnitType.Star);

                ColumnDefinition c2 = new ColumnDefinition();
                c2.Width = new GridLength(80, GridUnitType.Star);
                _gridArcLine.ColumnDefinitions.Add(c1);
                _gridArcLine.ColumnDefinitions.Add(c2);

                ///arrow on column 1 and line on column 2
                var imageSource = new BitmapImage(new Uri(assemblyPath + @"\LeftArrow.ico"));
                var img_Arrow = new Image { Source = imageSource, };


                Grid.SetRow(img_Arrow, 0);
                Grid.SetColumn(img_Arrow, 0);
                _gridArcLine.Children.Add(img_Arrow);

                Line _line = new Line()
                {
                    X1 = 0,
                    Y1 = 0,
                    X2 = 196,
                    Y2 = 0,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Margin = new Thickness(-5, 11, 0, 0),
                    Width = 196
                };


                Grid.SetRow(_line, 0);
                Grid.SetColumn(_line, 1);
                _gridArcLine.Children.Add(_line);
                Canvas.SetLeft(_gridArcLine, -10);
                Canvas.SetBottom(_gridArcLine, 2);




                /// placing the grid in canvas for  manipulations
                Canvas canv_angleLine = new Canvas()
                {
                    Height = 20,
                    Width = _protractorCanvas.Width / 2
                };
                canv_angleLine.Children.Add(_gridArcLine);
                canv_angleLine.MouseDown += Canv_angleLine_MouseDown;
                canv_angleLine.MouseMove += Canv_angleLine_MouseMove;
                canv_angleLine.MouseUp += Canv_angleLine_MouseUp;
                Canvas.SetLeft(canv_angleLine, 0);
                Canvas.SetBottom(canv_angleLine, -5);

                _protractorCanvas.Children.Add(canv_angleLine);

                ///Adding a textblock to canvas
                TextBlock _txtBlock = new TextBlock()
                {
                    Background = Brushes.Transparent,
                    Text = "0°",
                    TextWrapping = TextWrapping.NoWrap,
                    FontSize = 15,
                    Width = 50,
                    Height = 25

                };
                Canvas.SetTop(_txtBlock, _protractorCanvas.Height);
                Canvas.SetLeft(_txtBlock, _protractorCanvas.Width / 2 - _txtBlock.Width / 2);
                _protractorCanvas.Children.Add(_txtBlock);


                Line _stableLine = new Line()
                {
                    X1 = 0,
                    Y1 = 0,
                    X2 = 196,
                    Y2 = 0,
                    Stroke = Brushes.Red,
                    StrokeThickness = 0,
                    Margin = new Thickness(5, -11, 0, 0)
                };
                _stableLine.Loaded += _stableLine_Loaded;

                Canvas.SetLeft(_stableLine, 0);
                Canvas.SetTop(_stableLine, _protractorCanvas.Height + 5);
                _protractorCanvas.Children.Add(_stableLine);


                ///Protractor's Close button

                Canvas canv_protractor_Close = new Canvas();
                canv_protractor_Close.Height = 20;
                canv_protractor_Close.Width = 20;

                canv_protractor_Close.Background = new ImageBrush(new BitmapImage(new Uri(assemblyPath + @"\closeicon.png", UriKind.Relative)));
                Canvas.SetLeft(canv_protractor_Close, _protractorCanvas.Width - canv_protractor_Close.Width - 30);
                Canvas.SetTop(canv_protractor_Close, 20);
                canv_protractor_Close.MouseDown += Canv_protractor_Close_MouseDown;
            canv_protractor_Close.TouchDown += Canv_protractor_Close_TouchDown; ;

            _protractorCanvas.Children.Add(canv_protractor_Close);
            Canvas.SetZIndex(canv_protractor_Close, 9999);

            Canvas.SetZIndex(_protractorCanvas, 9999);
                // adding protractor to the inkcanvas(board)
                InkCanvas.SetLeft(_protractorCanvas, 400);
                InkCanvas.SetTop(_protractorCanvas, 300);
                inkcanvas.Children.Add(_protractorCanvas);
            _protractorList.Add(protractor);

                protractor.size = new Size(195, 195);
               
            }

        private void Canv_protractor_Close_TouchDown(object sender, TouchEventArgs e)
        {
            Canv_protractor_Close_MouseDown(sender, null);
          }

        private void Canv_protractor_Close_MouseDown(object sender, MouseButtonEventArgs e)
            {
            if(e!=null)
            e.Handled = true;
                FrameworkElement fe = ((System.Windows.FrameworkElement)sender).Parent as FrameworkElement;
                inkcanvas.Children.Remove(fe);
                fe = null;
            }

            private void _stableLine_Loaded(object sender, RoutedEventArgs e)
            {
         
           protractor = _protractorList.FirstOrDefault(x => x.id == ((System.Windows.FrameworkElement)((System.Windows.FrameworkElement)sender).Parent).Tag.ToString());
                Line _line = sender as Line;
                if (protractor != null)
                    protractor._staticInitialZeroDegreePoint = _line.TranslatePoint(new Point(_line.X1, _line.Y1), inkcanvas);

            }



            private void _protractorCanvas_MouseMove(object sender, MouseEventArgs e)
            {
                FrameworkElement fe = sender as FrameworkElement;
                fe.IsManipulationEnabled = true;
                fe.ManipulationStarting += ProtractorCanvas_ManipulationStarting;
                fe.ManipulationDelta += ProtractorCanvas_ManipulationDelta;
                fe.ManipulationCompleted += ProtractorCanvas_ManipulationCompleted;
            }

            private void Canv_angleLine_MouseDown(object sender, MouseButtonEventArgs e)
            {
            protractor = _protractorList.FirstOrDefault(x => x.id == ((System.Windows.FrameworkElement)((System.Windows.FrameworkElement)sender).Parent).Tag.ToString());

            e.Handled = true;
                FrameworkElement fe = sender as FrameworkElement;
                Mouse.Capture(fe);

                Point currentpoint = e.GetPosition(fe);
                if (protractor != null)
                {
                    protractor.pbCurrentX = currentpoint.X;
                    protractor.pbCurrentY = currentpoint.Y;
                    Line line = ((System.Windows.Controls.Panel)(((System.Windows.Controls.Panel)fe).Children[0])).Children[1] as Line;


                    protractor._initialZeroDegreePoint = line.TranslatePoint(new Point(line.X1, line.Y1), inkcanvas);

                    protractor.sweepDirectionPoint = line.TranslatePoint(new Point(line.X1, line.Y1), inkcanvas);
                    protractor.arcSegment = new ArcSegment();
                    protractor.pathGeometry = new PathGeometry();
                    protractor.pathFigure = new PathFigure();
                    protractor.arc_path = new Path();
                    protractor.arcSegment.Size = protractor.size;
                    protractor.arcSegment.IsStroked = true;
                    protractor.arcSegment.IsLargeArc = false;
                    protractor.arcSegment.RotationAngle = 0;
                    protractor.arcSegment.Point = protractor._initialZeroDegreePoint;
                    //Set start of arc
                    protractor.pathFigure.StartPoint = protractor._initialZeroDegreePoint;


                    //set end point of arc.

                    protractor.arc_path.Stroke = Brushes.Black;
                    protractor.arc_path.StrokeThickness = 2;
                    //arc_path.Name = "_arc";
                    protractor.pathFigure.Segments.Add(protractor.arcSegment);
                    protractor.pathGeometry.Figures.Add(protractor.pathFigure);
                    protractor.arc_path.Data = protractor.pathGeometry;
                    inkcanvas.Children.Add(protractor.arc_path);



                    ///clockwise or counter clockwise
                    var _centerPoint = line.TranslatePoint(new Point(line.X2, line.Y2), inkcanvas);
                    protractor._initialZeroDegreePoint = line.TranslatePoint(new Point(line.X1, line.Y1), inkcanvas);
                    var v1 = protractor._initialZeroDegreePoint - _centerPoint;
                    currentpoint = e.GetPosition(inkcanvas);
                    var v2 = currentpoint - _centerPoint;
                    double _rotationAngle = Vector.AngleBetween(v1, v2);
                    var v3 = protractor._staticInitialZeroDegreePoint - _centerPoint;
                    double _angleBetweenMovingandFixedLine = Vector.AngleBetween(v3, v2);

                    protractor._clockWiseOrCounterClockWise = Math.Abs(_angleBetweenMovingandFixedLine);

                    protractor._arcToolSelected = true;

                }


            }




            private void Canv_angleLine_MouseMove(object sender, MouseEventArgs e)
            {
            protractor = _protractorList.FirstOrDefault(x => x.id == ((System.Windows.FrameworkElement)((System.Windows.FrameworkElement)sender).Parent).Tag.ToString());
            e.Handled = true;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (protractor._arcToolSelected)
                    {
                        FrameworkElement fe = sender as FrameworkElement;
                        Point currentpoint = e.GetPosition(inkcanvas);
                        Line line = ((System.Windows.Controls.Panel)(((System.Windows.Controls.Panel)fe).Children[0])).Children[1] as Line;


                        var transformation = fe.RenderTransform as MatrixTransform;
                        Matrix m = transformation == null ? Matrix.Identity : transformation.Matrix;
                        Point center = new Point(fe.Width, fe.Height / 2.25);
                        var centerPoint = line.TranslatePoint(new Point(line.X2, line.Y2), inkcanvas);
                        protractor._initialZeroDegreePoint = line.TranslatePoint(new Point(line.X1, line.Y1), inkcanvas);
                        var v1 = protractor._initialZeroDegreePoint - centerPoint;
                        var v2 = currentpoint - centerPoint;

                        /// _rotationAngle is angle between canvas and mouse or touch point
                        /// _angleBetweenMovingandFixedLine is angle between zero degree to current line
                        double _rotationAngle = Vector.AngleBetween(v1, v2);
                        var v3 = protractor._staticInitialZeroDegreePoint - centerPoint;
                        double _angleBetweenMovingandFixedLine = Vector.AngleBetween(v3, v2);
                        if (_angleBetweenMovingandFixedLine < 0)
                            _angleBetweenMovingandFixedLine = 360 - Math.Abs(_angleBetweenMovingandFixedLine);
                        if (_angleBetweenMovingandFixedLine >= 0 && _angleBetweenMovingandFixedLine <= 180.2)
                        {
                            m.RotateAt(_rotationAngle, center.X, center.Y);
                            fe.RenderTransform = new MatrixTransform(m);
                        }

                        else if (_angleBetweenMovingandFixedLine < 359 && _angleBetweenMovingandFixedLine > 180.2)
                        {
                            if (protractor.arcSegment != null && protractor.arcSegment.SweepDirection == SweepDirection.Clockwise && _angleBetweenMovingandFixedLine > 90 && _angleBetweenMovingandFixedLine < 270)
                            {
                                m = new Matrix(-0.9999, 0.0009, -0.0009, -0.99999, center.X * 2, center.Y * 2);
                                fe.RenderTransform = new MatrixTransform(m);
                            }
                            else if (protractor.arcSegment != null && protractor.arcSegment.SweepDirection == SweepDirection.Counterclockwise && _angleBetweenMovingandFixedLine < 360 && _angleBetweenMovingandFixedLine > 270)
                            {
                                m = new Matrix(0.99999, 0.00199, -0.00199, 0.99999, 0.01, -0.397);
                                fe.RenderTransform = new MatrixTransform(m);
                            }
                        }
                        Path _protractorPath = ((System.Windows.Controls.Panel)fe.Parent).Children[0] as Path;




                        if (protractor._clockWiseOrCounterClockWise < _angleBetweenMovingandFixedLine && (_angleBetweenMovingandFixedLine >= 0 && _angleBetweenMovingandFixedLine <= 180.2))
                        {
                            protractor.arcSegment.SweepDirection = SweepDirection.Clockwise;
                        }
                        else if (protractor._clockWiseOrCounterClockWise > _angleBetweenMovingandFixedLine && _angleBetweenMovingandFixedLine >= 0 && _angleBetweenMovingandFixedLine <= 180.2)

                        {
                            protractor.arcSegment.SweepDirection = SweepDirection.Counterclockwise;

                        }


                        if (protractor.arcSegment != null)
                        {
                            protractor.arcSegment.Point = protractor._initialZeroDegreePoint;
                            protractor.pathFigure.Segments.Add(protractor.arcSegment);
                            protractor.pathGeometry.Figures.Add(protractor.pathFigure);
                            protractor.arc_path.Data = protractor.pathGeometry;
                        }
                    }

                });

            }


            private void Canv_angleLine_MouseUp(object sender, MouseButtonEventArgs e)
            {
            protractor = _protractorList.FirstOrDefault(x => x.id == ((System.Windows.FrameworkElement)((System.Windows.FrameworkElement)sender).Parent).Tag.ToString());
            e.Handled = true;
                Mouse.Capture(null);
                protractor._arcToolSelected = false;

                var geometry = protractor.arc_path.Data as PathGeometry;
                Canvas canv = new Canvas();
                canv.Height = geometry.Bounds.Height;
                canv.Width = geometry.Bounds.Width;
                canv.Background = Brushes.Transparent;
                inkcanvas.Children.Remove(protractor.arc_path);
                canv.Children.Add(protractor.arc_path);
                InkCanvas.SetLeft(canv, geometry.Bounds.TopLeft.X);
                InkCanvas.SetTop(canv, geometry.Bounds.TopLeft.Y);
                Canvas.SetLeft(protractor.arc_path, -geometry.Bounds.TopLeft.X);
                Canvas.SetTop(protractor.arc_path, -geometry.Bounds.TopLeft.Y);
                inkcanvas.Children.Add(canv);
            
            }



            #region protractor manipulations 

            private void ProtractorCanvas_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
            {
                e.Handled = true;
                e.ManipulationContainer = inkcanvas;
            }

            private void ProtractorCanvas_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
            {
            protractor = _protractorList.FirstOrDefault(x => x.id == ((System.Windows.FrameworkElement)sender).Tag.ToString());
            e.Handled = true;
                FrameworkElement fe = e.Source as FrameworkElement;
                var transformation = fe.RenderTransform as MatrixTransform;
                Matrix m = transformation == null ? Matrix.Identity : transformation.Matrix;
                Point center = new Point(fe.ActualWidth / 2, fe.ActualHeight / 2);
                center = m.Transform(center);
                m.RotateAt(e.DeltaManipulation.Rotation, center.X, center.Y);
                m.ScaleAt(e.DeltaManipulation.Scale.X, e.DeltaManipulation.Scale.X, center.X, center.Y);

                m.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);
                fe.RenderTransform = new MatrixTransform(m);
                var radians = Math.Atan2(m.M21, m.M11);
                var degrees = (radians * 180 / Math.PI);
                if (degrees <= 0)
                    degrees = Math.Abs(degrees);
                else
                    degrees = 360 - degrees;
                if (degrees == 0)
                    ((System.Windows.Controls.TextBlock)(((System.Windows.Controls.Panel)fe).Children[2])).Text = "0°";
                else
                    ((System.Windows.Controls.TextBlock)(((System.Windows.Controls.Panel)fe).Children[2])).Text = degrees.ToString("F") + "°";

                Line _line = ((System.Windows.Controls.Panel)fe).Children[3] as Line;
                protractor._staticInitialZeroDegreePoint = _line.TranslatePoint(new Point(_line.X1, _line.Y1), inkcanvas);

                if (protractor.arcSegment == null)
                    protractor.arcSegment = new ArcSegment();
                protractor.size = new Size(protractor.size.Width * e.DeltaManipulation.Scale.X, protractor.size.Height * e.DeltaManipulation.Scale.Y);

          
            }


            private void ProtractorCanvas_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
            {
                e.Handled = true;
                FrameworkElement fe = sender as FrameworkElement;
                fe.IsManipulationEnabled = false;
                fe.ManipulationStarting -= ProtractorCanvas_ManipulationStarting;
                fe.ManipulationDelta -= ProtractorCanvas_ManipulationDelta;
                fe.ManipulationCompleted -= ProtractorCanvas_ManipulationCompleted;

            }

            #endregion


            #region protractor arc


            #endregion

            private void EditingModeSwitchButton_Click(object sender, RoutedEventArgs e)
            {
                if (inkcanvas.EditingMode == InkCanvasEditingMode.None)
                    inkcanvas.EditingMode = InkCanvasEditingMode.Select;
                else
                    inkcanvas.EditingMode = InkCanvasEditingMode.None;
            }

    
    }

    public class Protractor
        {
        #region variables
        public string id;
            public PathGeometry pathGeometry;// = new PathGeometry();
            public PathFigure pathFigure;// = new PathFigure();
            public ArcSegment arcSegment;
            public double pbCurrentX;
            public double pbCurrentY;

            public Point _initialZeroDegreePoint;
            public Point _staticInitialZeroDegreePoint;
            public Point sweepDirectionPoint;
            public double _clockWiseOrCounterClockWise;

            public Path arc_path;// = new Path();


            public Size size = new Size(195, 195);
            public bool _arcToolSelected = false;


            #endregion

        }
    }
