using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Linq;

using PDFTron.SilverDox.Samples.SubControls;
using PDFTron.SilverDox.Controls;
using PDFTron.SilverDox.Documents.Annotations;
using PDFTron.SilverDox.Documents.Text;
using System.ComponentModel;
using System.Windows.Data;
using System.IO.IsolatedStorage;
using System.IO;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Input;

namespace PDFTron.SilverDox.Samples.Controls
{
    /// <summary>
    /// A control to change the properties of an annotation
    /// </summary>
    public partial class AnnotationPropertiesControl : UserControl, INotifyPropertyChanged
    {

        #region Properties

        /// <summary>
        /// Reference to the underlying DocumentViewer
        /// </summary>
        public DocumentViewer DocumentViewer
        {
            get 
            {
                try { return (DocumentViewer)this.DataContext; }
                catch { throw new InvalidCastException("DataContext is not DocumentViewer object"); }
            }
        }
        /// <summary>
        /// Reference to the DocumentViewer's AnnotationManager
        /// </summary>
        private AnnotationManager AnnotationManager
        {
            get 
            {
                Debug.Assert(DocumentViewer != null, "Can't get Annotation Manager; DocumentViewer is null");
                return DocumentViewer.AnnotationManager; 
            }
        }

        /// <summary>
        /// The current annotation that this control is manipulating
        /// </summary>
        public Annotation SelectedAnnotation { get; set; }

        private bool showFillColor = false;
        private bool showStrokeColor = false;
        private bool showStrokeThickness = false;

        public bool ShowAnnotationPropertiesPanel
        {
            get { return this.ShowFillColor || this.ShowStrokeColor || this.ShowStrokeThickness; }
        }
        public bool ShowFillColor
        {
            get { return this.showFillColor; }
            set 
            { 
                this.showFillColor = value; 
                this.NotifyPropertyChanged("ShowFillColor"); 
                this.NotifyPropertyChanged("ShowAnnotationPropertiesPanel"); 
            }
        }
        public bool ShowStrokeColor
        {
            get { return this.showStrokeColor; }
            set 
            { 
                this.showStrokeColor = value;
                this.NotifyPropertyChanged("ShowStrokeColor"); 
                this.NotifyPropertyChanged("ShowAnnotationPropertiesPanel"); 
            }
        }
        public bool ShowStrokeThickness
        {
            get { return this.showStrokeThickness; }
            set
            {
                this.showStrokeThickness = value;
                this.NotifyPropertyChanged("ShowStrokeThickness");
                this.NotifyPropertyChanged("ShowAnnotationPropertiesPanel"); 
            }
        }

        #endregion

        /// <summary>
        /// Creates a new instance of the AnnotationPropertiesControl
        /// </summary>
        public AnnotationPropertiesControl()
        {
            InitializeComponent();
        }

        private void AddUndoCommand(string propertyName, Color oldValue, Color newValue)
        {
            if (oldValue == default(Color) || newValue == default(Color)) return;

            var oldPropertyValues = new Dictionary<string, object>()
            {
                {propertyName, oldValue}
            };

            //Add new undo command
            this.AnnotationManager.AddUndoAnnotationState(
                this.SelectedAnnotation, oldPropertyValues);
        }

        private void RedrawSelectedAnnotation()
        {
            if (this.DocumentViewer == null) return;
            if (this.SelectedAnnotation as Markup != null)
            {
                this.DocumentViewer.AnnotationManager.RedrawAnnotation(this.SelectedAnnotation);
            }
        }

        #region EventHandlers

        private void annotationPropertyControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Hooks up annotation event handlers if DocumentViewer is fully loaded
            if (this.DocumentViewer != null)
            {
                this.DocumentViewer.ToolModeChanged +=
                    new EventHandler<ToolModeChangedEventArgs>(this.OnToolModeChanged);

                this.DocumentViewer.AnnotationsSelected +=
                    new EventHandler<AnnotationsEventArgs>(this.OnAnnotationSelected);

                this.DocumentViewer.AnnotationsDeselected +=
                    new EventHandler<AnnotationsEventArgs>(this.OnAnnotationDeselected);
            }

            // Hook up event handlers for UI controls to use the AnnotationManager's 
            // undo/redo system when values are changed
            this.fillColorColorPicker.ValueChanged += 
                new RoutedPropertyChangedEventHandler<Color>(
                    fillColorColorPicker_ValueChanged);

            this.strokeColorColorPicker.ValueChanged +=
                new RoutedPropertyChangedEventHandler<Color>(
                    strokeColorColorPicker_ValueChanged);

            this.strokeThicknessSlider.GotFocus +=
                new RoutedEventHandler(strokeThicknessSlider_GotFocus);

            this.strokeThicknessSlider.LostMouseCapture +=
                new MouseEventHandler(strokeThicknessSlider_LostMouseCapture);
        }

        private void fillColorColorPicker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            var markup = this.SelectedAnnotation as Markup;
            if (markup == null) return;

            // Workaround for two-way databinding between ColorPicker.Value and Markup.FillColor
            // We only want a new undo command to be added when the binding is flowing from 
            // ColorPicker to the Annotation but not vice versa
            if (markup.FillColor.Equals(e.NewValue)) return;
            this.AddUndoCommand("FillColor", e.OldValue, e.NewValue);
        }

        private void strokeColorColorPicker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            var markup = this.SelectedAnnotation as Markup;
            if (markup == null) return;

            // Workaround for two-way databinding between ColorPicker.Value and Markup.StrokeColor
            // We only want a new undo command to be added when the binding is flowing from 
            // ColorPicker to the Annotation but not vice versa
            if (markup.StrokeColor.Equals(e.NewValue)) return;

            this.AddUndoCommand("StrokeColor", e.OldValue, e.NewValue);
        }

        private void annotationsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.RedrawSelectedAnnotation();
        }

        private void OnToolModeChanged(object sender, ToolModeChangedEventArgs e)
        {
            if( e.NewToolMode != SilverDox.Controls.DocumentViewer.ToolModes.AnnotationEdit)
                this.DocumentViewer.AnnotationManager.DeselectAllAnnotations();

            if (e.NewToolMode.ToString().StartsWith("AnnotationCreate"))
            {
                Markup defaultAnnotation = (this.DocumentViewer.AnnotationManager != null) ?
                    (DocumentViewer.AnnotationManager.DefaultAnnotation as Markup) : (null);

                if (defaultAnnotation != null)
                {
                    this.SetAnnotationDefaultProperties(defaultAnnotation);
                }
            }

        }

        private void OnAnnotationDeselected(object sender, AnnotationsEventArgs e)
        {
            if (this.DocumentViewer.AnnotationManager.SelectedAnnotations.Count == 1)
            {
                this.SetAndBindProperties(null);
                this.SelectedAnnotation = this.DocumentViewer.AnnotationManager.SelectedAnnotations[0];
                var selectedMarkup = this.SelectedAnnotation as Markup;
                this.SetAndBindProperties(selectedMarkup);
            }
            else if( this.SelectedAnnotation != null && 
                e.Annotations.Contains(this.SelectedAnnotation))
            {
                this.SetAndBindProperties(null);
            }
            
        }

        private void OnAnnotationSelected(object sender, AnnotationsEventArgs e)
        {
            // Don't show properties if 0 or more than 1 annotation is selected
            if (e.Annotations.Count != 1)
            {
                this.SetAndBindProperties(null);
                return;
            }

            this.SelectedAnnotation = e.Annotations[0];
            var selectedMarkup = this.SelectedAnnotation as Markup;
            this.SetAndBindProperties(selectedMarkup);
        }
        private void SetAndBindProperties(Markup markup)
        {
            // Bind/Unbind
            if (markup == null)
            {
                this.ShowFillColor = false;
                this.ShowStrokeColor = false;
                this.ShowStrokeThickness = false;

                this.fillColorColorPicker.ClearValue(Controls.ColorPicker.ValueProperty);
                this.strokeColorColorPicker.ClearValue(Controls.ColorPicker.ValueProperty);
                this.strokeThicknessSlider.ClearValue(Slider.ValueProperty);
            }
            else
            {
                Binding b;

                //Fill Color
                b = new Binding();
                b.Source = markup;
                b.Path = new PropertyPath("FillColor");
                b.Mode = BindingMode.TwoWay;

                //Add opacity if markup is highlight
                if (markup as TextHighlight != null)
                {
                    b.Converter = new Utility.ColorAlphaConverter();
                    b.ConverterParameter = (byte)0x44;
                }

                this.fillColorColorPicker.SetBinding(Controls.ColorPicker.ValueProperty, b);

                //Stroke Color
                b = new Binding();
                b.Source = markup;
                b.Path = new PropertyPath("StrokeColor");
                b.Mode = BindingMode.TwoWay;
                this.strokeColorColorPicker.SetBinding(Controls.ColorPicker.ValueProperty, b);

                //Stroke Thickness
                b = new Binding();
                b.Source = markup;
                b.Path = new PropertyPath("StrokeThickness");
                b.Mode = BindingMode.TwoWay;
                this.strokeThicknessSlider.SetBinding(Slider.ValueProperty, b);

                var adhp = AnnotationDefaultPropertyHelper.Instance;
                this.ShowFillColor = (adhp.GetDefaultFillColor(markup.GetType()) != null); 
                this.ShowStrokeColor = (adhp.GetDefaultStrokeColor(markup.GetType()) != null); 
                this.ShowStrokeThickness = (adhp.GetDefaultStrokeThickness(markup.GetType()) != null);
            }
        }


        private void setAsDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            var markup = this.SelectedAnnotation as Markup;

            if (markup == null) return;

            Color? fillColor, strokeColor;
            double? strokeThickness;
            var adph = AnnotationDefaultPropertyHelper.Instance;

            fillColor = (adph.GetDefaultFillColor(markup.GetType()) != null) ? (Color?)markup.FillColor : null;
            strokeColor = (adph.GetDefaultStrokeColor(markup.GetType()) != null) ? (Color?)markup.StrokeColor : null;
            strokeThickness = (adph.GetDefaultStrokeThickness(markup.GetType()) != null) ? (double?)markup.StrokeThickness : null;

            adph.SetNewDefaultValue(
                markup.GetType(),
                new AnnotationDefaultPropertyHelper.DefaultPropertyValue()
                {
                    FillColor = fillColor, StrokeColor = strokeColor, StrokeThickness=strokeThickness
                }
            );

            // Reset the default properties
            Markup defaultAnnotation = (this.DocumentViewer.AnnotationManager != null) ?
                (DocumentViewer.AnnotationManager.DefaultAnnotation as Markup) : (null);

            if (defaultAnnotation != null)
            {
                this.SetAnnotationDefaultProperties(defaultAnnotation);
            }
        }

        //private void loadAnnotationsButton_Click(object sender, RoutedEventArgs e)
        //{
        //    // Defer the load annotations logic to someone else....
        //    if (this.LoadAnnotationsClicked != null)
        //    {
        //        this.LoadAnnotationsClicked.Invoke(this, new EventArgs());
        //        return;
        //    }
        //}
        //private void saveAnnotationsButton_Click(object sender, RoutedEventArgs e)
        //{

        //     //Defer the save annotations logic to someone else....
        //    //if (this.SaveAnnotationsClicked != null)
        //    {
        //        var changeDict = this.AnnotationManager.GetAllChangedAnnotations();
        //        var removedAnnots = changeDict.Where(kvp => kvp.Value == AnnotationManager.TypeOfChange.Removed).Select(kvp => kvp.Key).ToList();
        //        var createdAnnots =  changeDict.Where(kvp =>kvp.Value == AnnotationManager.TypeOfChange.Created). Select( kvp => kvp.Key ).ToList(); 
        //        var modifiedAnnots =  changeDict.Where(kvp =>kvp.Value == AnnotationManager.TypeOfChange.Modified). Select( kvp => kvp.Key ).ToList(); 

        //        this.SaveAnnotationsClicked.Invoke(this, 
        //            new AnnotationsSavedEventArgs(
        //                removedAnnots, createdAnnots, modifiedAnnots
        //            )
        //        );
        //        return;
        //    }

        //}

        #endregion

        #region INotifyPropertyChanged Members
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        #endregion


        double oldStrokeThickness = 0;
        private void strokeThicknessSlider_GotFocus(object sender, RoutedEventArgs e)
        {
            Markup markup = this.SelectedAnnotation as Markup;
            if(markup == null) return;

            this.oldStrokeThickness = markup.StrokeThickness;
        }
        private void strokeThicknessSlider_LostMouseCapture(object sender, MouseEventArgs e)
        {
            var markup = this.SelectedAnnotation as Markup;
            if (markup == null) return;

            // don't do anything if old and new stroke thickness are the same value
            if (markup.StrokeThickness.Equals(this.oldStrokeThickness)) return;

            var oldPropertyValues = new Dictionary<string, object>()
            {
                {"StrokeThickness", this.oldStrokeThickness}
            };

            this.AnnotationManager.AddUndoAnnotationState(
                this.SelectedAnnotation, oldPropertyValues);
        }
        private void SetAnnotationDefaultProperties(Markup markup)
        {
            var adph = AnnotationDefaultPropertyHelper.Instance;

            Color? fillColor, strokeColor;
            double? strokeThickness;

            if ((fillColor = adph.GetDefaultFillColor(markup.GetType())) != null)
            {
                markup.FillColor = (Color)fillColor;

                //Opacity for highlight
                if (markup as TextHighlight != null)
                {
                    Color c = markup.FillColor;
                    c.A = (byte)Math.Min((byte)0x44, markup.FillColor.A);
                    markup.FillColor = c;
                }
            }
            else { markup.FillColor = Colors.Transparent; }

            if ((strokeColor = adph.GetDefaultStrokeColor(markup.GetType())) != null)
            {
                markup.StrokeColor = (Color)strokeColor;
            }
            else { markup.StrokeColor = Colors.Transparent; } 

            if ((strokeThickness = adph.GetDefaultStrokeThickness(markup.GetType())) != null)
            {
                markup.StrokeThickness = (double)strokeThickness;
            }else { markup.StrokeThickness = 0; }
        }

        private class AnnotationDefaultPropertyHelper
        {
            // Helper Class used to manage default values for different types 
            // of annotations.
            private Dictionary<Type, DefaultPropertyValue> defaultPropertyValues;

            private static AnnotationDefaultPropertyHelper instance;
            public static AnnotationDefaultPropertyHelper Instance
            {
                get
                {
                    if (instance == null)
                        instance = new AnnotationDefaultPropertyHelper();
                    return instance;
                }
                
            }

            private AnnotationDefaultPropertyHelper()
            {
                this.defaultPropertyValues = new Dictionary<Type, DefaultPropertyValue>();

                // Annotation
                this.defaultPropertyValues.Add( typeof(Annotation), new DefaultPropertyValue() );

                // Markup
                this.defaultPropertyValues.Add(typeof(Markup),
                    new DefaultPropertyValue
                    {
                        FillColor = Colors.Transparent,
                        StrokeColor = Colors.Red,
                        StrokeThickness = 1
                    }
                );

                // Line
                this.defaultPropertyValues.Add(typeof(Line),
                    new DefaultPropertyValue
                    {
                        StrokeColor = Colors.Red,
                        StrokeThickness = 1
                    }
                );

                // TextMarkup
                this.defaultPropertyValues.Add(typeof(TextMarkup),
                    new DefaultPropertyValue
                    {
                        StrokeColor = Colors.Red,
                        StrokeThickness = 1
                    }
                );

                // TextHighlight
                this.defaultPropertyValues.Add(typeof(TextHighlight),
                    new DefaultPropertyValue
                    {
                        FillColor = Colors.Yellow,
                    }
                );
            }
            public void SetNewDefaultValue(Type annotType, DefaultPropertyValue dpv)
            {
                if (this.defaultPropertyValues.ContainsKey(annotType))
                    this.defaultPropertyValues[annotType] = dpv;
                else
                    this.defaultPropertyValues.Add(annotType, dpv);
            }
            public Color? GetDefaultFillColor(Type annotType)
            {
                Type annotTypeHierarchy = this.GetTypeInHierarchy(annotType);
                if (annotTypeHierarchy == null) return null;
                return this.defaultPropertyValues[annotTypeHierarchy].FillColor;
            }
            public Color? GetDefaultStrokeColor(Type annotType)
            {
                Type annotTypeHierarchy = this.GetTypeInHierarchy(annotType);
                if (annotTypeHierarchy == null) return null;
                return this.defaultPropertyValues[annotTypeHierarchy].StrokeColor;
            }
            public double? GetDefaultStrokeThickness(Type annotType)
            {
                Type annotTypeHierarchy = this.GetTypeInHierarchy(annotType);
                if (annotTypeHierarchy == null) return null;
                return this.defaultPropertyValues[annotTypeHierarchy].StrokeThickness;
            }

            private Type GetTypeInHierarchy(Type annotType)
            {
                if( annotType.Equals(typeof(object)) ) return null;

                if (this.defaultPropertyValues.ContainsKey(annotType)) return annotType;

                return this.GetTypeInHierarchy(annotType.BaseType);
            }

            public class DefaultPropertyValue
            {
                public Color? FillColor = null;
                public Color? StrokeColor = null;
                public double? StrokeThickness = null;
            }
        }
    }
}