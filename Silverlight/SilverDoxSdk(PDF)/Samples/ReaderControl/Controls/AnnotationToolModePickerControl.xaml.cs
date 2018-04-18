using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;

using PDFTron.SilverDox.Controls;
using PDFTron.SilverDox.Documents.Annotations;

namespace PDFTron.SilverDox.Samples.Controls
{
    /// <summary>
    /// A control to choose the tool mode for creating annotations
    /// </summary>
    public partial class AnnotationToolModePickerControl : UserControl
    {

        #region Properties

        /// <summary>
        /// Dependency Property for Author
        /// </summary>
        public static readonly DependencyProperty AnnotationAuthorProperty =
            DependencyProperty.Register(
                    "AnnotationAuthor", typeof(string), typeof(AnnotationToolModePickerControl), null
            );

        /// <summary>
        /// The author of the annotations created
        /// </summary>
        public string AnnotationAuthor
        {
            get { return (string)this.GetValue(AnnotationAuthorProperty); }
            set { this.SetValue(AnnotationAuthorProperty, value); }
        }

        /// <summary>
        /// Reference to the underlying DocumentViewer object
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
        
        #endregion

        /// <summary>
        /// Creates a new instance of the AnnotationPropertiesControl
        /// </summary>
        public AnnotationToolModePickerControl()
        {
            InitializeComponent();
        }

        private void annotationToolsPicker_Loaded(object sender, RoutedEventArgs e)
        {
            this.DocumentViewer.AnnotationsCreated +=
                new EventHandler<AnnotationsCreatedEventArgs>(this.OnAnnotationCreated);

        }

        private void OnAnnotationCreated(object sender, AnnotationsCreatedEventArgs e)
        {
            foreach (var annot in e.CreatedAnnotations)
            {
                if( !String.IsNullOrWhiteSpace(this.AnnotationAuthor) )
                    annot.Author = this.AnnotationAuthor;

                //this.AnnotationManager.SelectAnnotation(annot);
            }

            // Switch tool mode to annotation edit, if created annotation
            // is not a TextMarkup/FreeHand annotation for UX puposes
            if (e.CreatedAnnotations.Count == 1)
            {
                var annot = e.CreatedAnnotations[0];

                if (annot as TextMarkup == null && annot as FreeHand == null)
                {
                    this.DocumentViewer.ToolMode = 
                        DocumentViewer.ToolModes.AnnotationEdit;
                }
            }
        }
    }
}
