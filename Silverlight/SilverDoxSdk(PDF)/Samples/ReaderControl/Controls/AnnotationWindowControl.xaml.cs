using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Linq;

using PDFTron.SilverDox.Samples.SubControls;
using PDFTron.SilverDox.Controls;
using PDFTron.SilverDox.Documents.Annotations;

using System.ComponentModel;
using System.Windows.Data;
using System.IO.IsolatedStorage;
using System.IO;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Input;

using PDFTron.SilverDox.Samples.Utility;


namespace PDFTron.SilverDox.Samples
{
    /// <summary>
    /// Represents a control that contains the outline treeview and thumbnail listbox controls
    /// </summary>
    public partial class AnnotationWindowControl : UserControl
    {
        public DocumentViewer DocumentViewer
        {
            get
            {
                try { return (DocumentViewer)this.DataContext; }
                catch { throw new InvalidCastException("DataContext is not DocumentViewer object"); }
            }
        }

        public AnnotationWindowControl()
        {
            this.RemoveAnnotationCommand = new DelegateCommand(
                RemoveAnnotation, RemoveAnnotationPredicate);

            this.InitializeComponent();
        }

        public ICommand RemoveAnnotationCommand { get; private set; }
        private void RemoveAnnotation(object p) { this.annotationsListBox.RemoveSelectedAnnotations(); }
        private bool RemoveAnnotationPredicate(object p)
        {
            foreach(var annot in this.annotationsListBox.SelectedAnnotations)
            {
                if (annot.IsSelectable) return true;
            }

            return false;
        }

        // Commented out ContextMenu. Having a ContextMenu for each ListBoxItem causes
        // performance issues. The ContextMenu exists in the VisualTree or something and
        // slows down the MouseEventArgs.GetPosition() method. Even when the ListBoxItem
        // and annotation is deleted, the ContextMenu still hangs around and is not garbage
        // collected for some reason so the performance does not get better.
        //private void annotationListContextMenu_Opened(object sender, RoutedEventArgs e)
        //{
        //    var contextMenu = sender as ContextMenu;
        //    var selectedAnnotation = contextMenu.DataContext as Annotation;

        //    if (selectedAnnotation != null && !this.annotationsListBox.SelectedAnnotations.Contains(selectedAnnotation))
        //        this.annotationsListBox.SelectedAnnotations = new List<Annotation>() { selectedAnnotation };

        //    //recompute predicate for command
        //    (this.RemoveAnnotationCommand as DelegateCommand).RaiseCanExecuteChanged();
        //}

        private void annotationNavigationUserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) this.annotationsListBox.RemoveSelectedAnnotations();
        }

        private void annotationsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 &&
                this.DocumentViewer.ToolMode != DocumentViewer.ToolModes.AnnotationEdit)
            {
                this.DocumentViewer.ToolMode = DocumentViewer.ToolModes.AnnotationEdit;
            }

            this.annotationsListBox.UpdateLayout();
            if (e.AddedItems.Count == 1)
                this.annotationsListBox.ScrollIntoView(e.AddedItems[0]);

        }
    }
}