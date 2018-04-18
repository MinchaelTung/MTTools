using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using PDFTron.SilverDox.Controls;
using PDFTron.SilverDox.Documents.Annotations;
using PDFTron.SilverDox.Documents.Text;
using PDFTron.SilverDox.IO;
using Sys = System.Windows.Shapes;
using System.Windows.Input;
using System.Collections.Generic;

namespace Annotations
{

    /// <summary>
    /// Demonstrates a custom TextHighlight annotation.
    /// </summary>
    public class TextBlockHighlight : TextMarkup
    {
        protected override void DrawTextAnnotation()
        {
            double minX = double.MaxValue, maxX = 0, minY = double.MaxValue, maxY = 0;

            if (Appearance.Children.Count > 0)
                return;

            // write new highlight rectangles to annotation layer
            foreach (Quad quad in TextQuads)
            {
                minX = Math.Min(minX, Math.Min(quad.x1, Math.Min(quad.x2, Math.Min(quad.x3, quad.x4))));
                minY = Math.Min(minY, Math.Min(quad.y1, Math.Min(quad.y2, Math.Min(quad.y3, quad.y4))));

                maxX = Math.Max(maxX, (Math.Max(quad.x1, Math.Max(quad.x2, Math.Max(quad.x3, quad.x4)))));
                maxY = Math.Max(maxY, (Math.Max(quad.y1, Math.Max(quad.y2, Math.Max(quad.y3, quad.y4)))));
            }

            X = minX; Appearance.Width = maxX - minX; Width = maxX - minX;
            Y = minY; Appearance.Height = maxY - minY; Height = maxY - minY;

            Sys.Rectangle highlightRect = new Sys.Rectangle()
            {
                Stroke = new SolidColorBrush() { Color = StrokeColor },
                StrokeThickness = StrokeThickness,
                Fill = new SolidColorBrush() { Color = FillColor }
            };


            if (StrokeDashArray != null)
                foreach (double d in StrokeDashArray)
                    highlightRect.StrokeDashArray.Add(d);

            highlightRect.SetValue(Canvas.LeftProperty, 0.0);
            highlightRect.Width = Math.Abs(Appearance.Width);
            highlightRect.Height = Math.Abs(Appearance.Height);
            highlightRect.SetValue(Canvas.TopProperty, 0.0);

            Appearance.Children.Add(highlightRect);
        }

        public static TextBlockHighlight Create(AnnotationManager annotationManager)
        {
            return new TextBlockHighlight(annotationManager);
        }

        public TextBlockHighlight(AnnotationManager annotationManager) : base(annotationManager)
        {
           
        }

        public TextBlockHighlight()
        {

        }
    }

    /// <summary>
    /// Demonstrates a custom Markup annotation.
    /// </summary>
    public class Arrow : Markup
    {

        /// <summary>
        /// Creates and returns the annotation's appearance canvas that will be displayed on screen.
        /// </summary>
        /// <returns>The annotation's appearance canvas.</returns>
        public override Canvas CreateAppearanceCanvas()
        {
            Canvas appearance = new Canvas() { Width = Width, Height = Height, Background = new SolidColorBrush() { Color = Colors.Transparent } };

            //Create arrow's main line

            LineGeometry LineGeometry = new LineGeometry();
            Sys.Path path = new System.Windows.Shapes.Path();

            path.Data = LineGeometry;
            path.Stroke = new SolidColorBrush() { Color = StrokeColor };
            path.StrokeThickness = StrokeThickness;

            if (StrokeDashArray != null)
                foreach (double d in StrokeDashArray)
                    path.StrokeDashArray.Add(d);

            path.Fill = new SolidColorBrush() { Color = FillColor };

            LineGeometry.StartPoint = new Point(0, 0);

            LineGeometry.EndPoint = new Point(Width, Height);

            appearance.Children.Add(path);

            //Create arrow head part 1
            double arrowHeadLength = 15;
            double theta = Math.Atan2(Height, Width);

            // arrow head line segments offsets
            double y = arrowHeadLength * Math.Cos(theta);
            double x = arrowHeadLength * Math.Sin(theta);

            LineGeometry = new LineGeometry();
            path = new System.Windows.Shapes.Path();

            path.Data = LineGeometry;
            path.Stroke = new SolidColorBrush() { Color = StrokeColor };
            path.StrokeThickness = StrokeThickness;

            if (StrokeDashArray != null)
                foreach (double d in StrokeDashArray)
                    path.StrokeDashArray.Add(d);

            path.Fill = new SolidColorBrush() { Color = FillColor };

            LineGeometry.StartPoint = new Point(0, 0);
            LineGeometry.EndPoint = new Point(x + y, x - y);

            
            appearance.Children.Add(path);

            //Create arrow head part 2
            LineGeometry = new LineGeometry();
            path = new System.Windows.Shapes.Path();

            path.Data = LineGeometry;
            path.Stroke = new SolidColorBrush() { Color = StrokeColor };
            path.StrokeThickness = StrokeThickness;


            if (StrokeDashArray != null)
                foreach (double d in StrokeDashArray)
                    path.StrokeDashArray.Add(d);

            path.Fill = new SolidColorBrush() { Color = FillColor };

            LineGeometry.StartPoint = new Point(0, 0);
            LineGeometry.EndPoint = new Point(y - x, y + x);

            appearance.Children.Add(path);

            return appearance;

        }

        bool mouseDownPointSet = false;
        Point mouseDownPoint;

        /// <summary>
        /// Called by the annotation's <see cref="AnnotationManager"/> when a MouseMove event is received and the <c>AnnotationManager</c>
        /// is set to create this type of annotation.
        /// </summary>
        /// <param name="mouseMoveEventArgs">The event data.</param>
        public override void OnCreateMouseMove(MouseEventArgs mouseMoveEventArgs)
        {
            if (mouseDownPointSet == false)
            {
                mouseDownPoint = new Point(X, Y);
                mouseDownPointSet = true;
            }

            Point mouseMovePoint = mouseMoveEventArgs.GetPosition(AnnotationManager.CurrentAnnotationPage);

            Width = Math.Abs(mouseDownPoint.X - mouseMovePoint.X);
            Height = Math.Abs(mouseDownPoint.Y - mouseMovePoint.Y);

            if (mouseDownPoint.X - mouseMovePoint.X < 0)
                MirroredHorizontally = false;
            else
                MirroredHorizontally = true;

            if (mouseDownPoint.Y - mouseMovePoint.Y < 0)
                MirroredVertically = false;
            else
                MirroredVertically = true;
        }

        public static Arrow Create(AnnotationManager annotationManager)
        {
            return new Arrow(annotationManager);
        }

        public Arrow(AnnotationManager annotationManager) : base(annotationManager)
        {

        }

        public Arrow()
        {

        }

    }



    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

            Application.Current.Host.Content.Resized += new EventHandler(Content_Resized);

            LoadDocument();
        }

        void Content_Resized(object sender, EventArgs e)
        {
            MyDocumentViewer.Width = Math.Max(Application.Current.Host.Content.ActualWidth-10,1.0);
            MyDocumentViewer.Height = Math.Max(Application.Current.Host.Content.ActualHeight - 240, 1.0);
        }

        void LoadDocument()
        {
            // load document into the viewer
            Uri documentUri = new Uri("http://www.pdftron.com/silverdox/samples/ClientBin/PDFTron_PDF2XPS_User_Manual.xod");

            HttpPartRetriever myHttpPartRetriever = new HttpPartRetriever(documentUri);

            ToolModeComboBox.SelectedIndex = 0;
            FillColorComboBox.SelectedIndex = 7;
            StrokeColorComboBox.SelectedIndex = 0;
            StrokeThicknessComboBox.SelectedIndex = 0;

            MyDocumentViewer.LoadAsync(myHttpPartRetriever, OnHttpLoadAsyncCallback);
        }

        private LocalPartRetriever MyRetriever;

        public void LoadLocalDocument(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            dlg.Filter = "XOD Files (*.xod)|*.xod";

            // open dialog
            bool ok = (bool)dlg.ShowDialog();

            if (ok)
            {
                if (MyRetriever as LocalPartRetriever != null)
                    ((LocalPartRetriever)MyRetriever).Dispose();
                FileStream fileStream = dlg.File.OpenRead();
                MyRetriever = new LocalPartRetriever(fileStream);

                MyDocumentViewer.LoadAsync(MyRetriever, OnLocalLoadAsyncCallback);
            }
        }

        void RegisterForNotification(string propertyName, FrameworkElement element, PropertyChangedCallback callback)
        {
            Binding b = new Binding(propertyName) { Source = element };
            var prop = DependencyProperty.RegisterAttached(
                "ListenAttached" + propertyName,
                typeof(object),
                typeof(UserControl),
                new PropertyMetadata(callback));

            element.SetBinding(prop, b);
        }

        void OnHttpLoadAsyncCallback(Exception ex)
        {
            
            // receive notification via a callback when DocumentViewer.ToolMode changes via contextual menu
            RegisterForNotification("ToolMode", MyDocumentViewer, ToolModeChanged);

            // needed for saving/loading custom annotations to/from XML
            MyDocumentViewer.AnnotationManager.AddCustomAnnotationTypes(new Type[]{typeof(TextBlockHighlight), typeof(Arrow)});

            MyDocumentViewer.ToolMode = DocumentViewer.ToolModes.PanAndAnnotationEdit;


        }


        void OnLocalLoadAsyncCallback(Exception ex)
        {

            // receive notification via a callback when DocumentViewer.ToolMode changes via contextual menu
            RegisterForNotification("ToolMode", MyDocumentViewer, ToolModeChanged);

            // needed for saving/loading custom annotations to/from XML
            MyDocumentViewer.AnnotationManager.AddCustomAnnotationTypes(new Type[] { typeof(TextBlockHighlight), typeof(Arrow) });

            MyDocumentViewer.ToolMode = DocumentViewer.ToolModes.PanAndAnnotationEdit;
        }
        

        void ToolModeChanged(Object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((DocumentViewer.ToolModes)e.NewValue) == DocumentViewer.ToolModes.PanAndAnnotationEdit)
                ToolModeComboBox.SelectedIndex = 0;
            else if (((DocumentViewer.ToolModes)e.NewValue) == DocumentViewer.ToolModes.TextSelect)
                ToolModeComboBox.SelectedIndex = 1;
        }

        void SerializeAnnotationsButton_Click(object sender, RoutedEventArgs e)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (FileStream stream = store.CreateFile("annotation.store"))
                {
                    Byte[] annots = MyDocumentViewer.AnnotationManager.SaveAnnotations();
                    stream.Write(annots, 0, annots.Length);
                }
            }
        }

        void ClearAnnotationsButton_Click(object sender, RoutedEventArgs e)
        {
            MyDocumentViewer.AnnotationManager.ClearAnnotations();
        }

        void LoadAnnotationsButton_Click(object sender, RoutedEventArgs e)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (FileStream stream = store.OpenFile("annotation.store", FileMode.Open))
                {
                    MyDocumentViewer.AnnotationManager.LoadAnnotations(stream);
                }
            }
        }

        Color FillColor;
        Color StrokeColor;
        double StrokeThickness = 2;

        void FillColorComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String selected = ((ComboBoxItem)(FillColorComboBox.SelectedItem)).Tag.ToString();

            SetColor(selected, ref FillColor);

            UpdateColors();
        }

        void StrokeColorComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String selected = ((ComboBoxItem)(StrokeColorComboBox.SelectedItem)).Tag.ToString();

            SetColor(selected, ref StrokeColor);

            UpdateColors();
        }

        private void SetColor(String selected, ref Color colorProperty)
        {
            if (selected.Equals("Red"))
            {
                colorProperty = Colors.Red;
            }
            else if (selected.Equals("Green"))
            {
                colorProperty = Colors.Green;
            }
            else if (selected.Equals("Blue"))
            {
                colorProperty = Colors.Blue;
            }
            else if (selected.Equals("Yellow"))
            {
                colorProperty = Colors.Yellow;
            }
            else if (selected.Equals("Light Green"))
            {
                colorProperty = Color.FromArgb(0xFF, 90, 0xEE, 0x90);
            }
            else if (selected.Equals("Orange"))
            {
                colorProperty = Colors.Orange;
            }
            else if (selected.Equals("Black"))
            {
                colorProperty = Colors.Black;
            }
            else if (selected.Equals("Transparent"))
            {
                colorProperty = Colors.Transparent;
            }
        }

        private void UpdateColors()
        {
            Markup defaultAnnotation = MyDocumentViewer.AnnotationManager != null ? MyDocumentViewer.AnnotationManager.DefaultAnnotation as Markup : null;

            if (defaultAnnotation != null)
            {

                defaultAnnotation = MyDocumentViewer.AnnotationManager.DefaultAnnotation as Markup;
                defaultAnnotation.FillColor = FillColor;
                defaultAnnotation.StrokeColor = StrokeColor;
                defaultAnnotation.StrokeThickness = StrokeThickness;
            }

            if (defaultAnnotation as TextMarkup != null)
            {
                if (defaultAnnotation as TextHighlight != null)
                {
                    defaultAnnotation.FillColor =
                        Color.FromArgb(
                            (byte)Math.Min((byte)0x44, defaultAnnotation.FillColor.A),
                            this.FillColor.R, this.FillColor.G, this.FillColor.B
                            );
                    //defaultAnnotation.FillColor.A = (byte)Math.Min((byte)0x44,defaultAnnotation.FillColor.A);
                    defaultAnnotation.StrokeColor = StrokeColor;
                }
                else
                {
                    defaultAnnotation.FillColor = FillColor;
                    defaultAnnotation.StrokeColor = StrokeColor;
                    defaultAnnotation.StrokeThickness = StrokeThickness;
                }
            }
        }

        void StrokeThicknessComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StrokeThickness = double.Parse(((ComboBoxItem)(StrokeThicknessComboBox.SelectedItem)).Tag.ToString());

            UpdateColors();
        }

        void ToolModeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String selected = ((ComboBoxItem)(ToolModeComboBox.SelectedItem)).Tag.ToString();

            if (selected.Equals("Pan"))
            {
                MyDocumentViewer.ToolMode = DocumentViewer.ToolModes.PanAndAnnotationEdit;
            }
            else if (selected.Equals("Select Text"))
            {
                MyDocumentViewer.ToolMode = DocumentViewer.ToolModes.TextSelect;
            }
            else if (selected.Equals("Ellipse"))
            {
                MyDocumentViewer.ToolMode = DocumentViewer.ToolModes.AnnotationCreateEllipse;
                UpdateColors();
            }
            else if (selected.Equals("Rectangle"))
            {
                MyDocumentViewer.ToolMode = DocumentViewer.ToolModes.AnnotationCreateRectangle;
                UpdateColors();
            }
            else if (selected.Equals("Free Hand"))
            {
                MyDocumentViewer.ToolMode = DocumentViewer.ToolModes.AnnotationCreateFreeHand;
                UpdateColors();
            }

            else if (selected.Equals("Text Highlight"))
            {
                MyDocumentViewer.ToolMode = DocumentViewer.ToolModes.AnnotationCreateTextHighlight;
                UpdateColors();
            }
            else if (selected.Equals("Text Underline"))
            {
                //create
                MyDocumentViewer.ToolMode = DocumentViewer.ToolModes.AnnotationCreateTextUnderline;
                UpdateColors();
                ((Markup)MyDocumentViewer.AnnotationManager.DefaultAnnotation).FillColor = Colors.Transparent;

            }
            else if (selected.Equals("Text Strikeout"))
            {
                MyDocumentViewer.ToolMode = DocumentViewer.ToolModes.AnnotationCreateTextStrikeout;
                UpdateColors();
            }
            else if (selected.Equals("Line"))
            {
                MyDocumentViewer.ToolMode = DocumentViewer.ToolModes.AnnotationCreateLine;
                UpdateColors();
            }
            else if (selected.Equals("Arrow"))
            {
                MyDocumentViewer.ToolMode = DocumentViewer.ToolModes.AnnotationCreateCustom;

                if (MyDocumentViewer.AnnotationManager != null)
                {
                    // necessary for all custom annotations
                    MyDocumentViewer.AnnotationManager.AnnotationCreator = Arrow.Create;
                    UpdateColors();
                }
            }
            else if (selected.Equals("Sticky"))
            {
                MyDocumentViewer.ToolMode = DocumentViewer.ToolModes.AnnotationCreateSticky;
            }
            else if (selected.Equals("Edit"))
            {
                MyDocumentViewer.ToolMode = DocumentViewer.ToolModes.AnnotationEdit;
            }
            
        }

        private void PrintDocument(object sender, RoutedEventArgs e)
        {
            MyDocumentViewer.Document.Print(true, MyDocumentViewer.AnnotationManager.GetNewAnnotationCanvases(true));
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            foreach (Annotation a in MyDocumentViewer.AnnotationManager.SelectedAnnotations)
            {

                Markup annotation = a as Markup;

                if (annotation != null)
                {
                    annotation.FillColor = FillColor;
                    annotation.StrokeColor = StrokeColor;
                    annotation.StrokeThickness = StrokeThickness;

                    if (annotation.GetType().ToString().Contains("Highlight"))
                    {
                        annotation.FillColor = Color.FromArgb(0x44, 
                            annotation.FillColor.R,
                            annotation.FillColor.G,
                            annotation.FillColor.B
                       );
                    }
                }

                MyDocumentViewer.AnnotationManager.RedrawAnnotation(a);

            }
        }

    }
}
