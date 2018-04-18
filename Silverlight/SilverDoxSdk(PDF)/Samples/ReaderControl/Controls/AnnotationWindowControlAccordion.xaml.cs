using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using PDFTron.SilverDox.Samples.SubControls;
using PDFTron.SilverDox.Controls;
using PDFTron.SilverDox.Documents.Annotations;
using PDFTron.SilverDox.Documents.Text;

namespace PDFTron.SilverDox.Samples
{
    /// <summary>
    /// Represents a control that contains the outline treeview and thumbnail listbox controls
    /// </summary>
    public partial class AnnotationAccordionWindowControl : UserControl
    {

        public DocumentViewer DocViewer
        {
            get
            {
                return (this.DataContext as DocumentViewer);

            }
        }

        /// <summary>
        /// Creates a new instance of the AnotationWindowControl
        /// </summary>
        public AnnotationAccordionWindowControl()
        {
            InitializeComponent();
        }

    }

}