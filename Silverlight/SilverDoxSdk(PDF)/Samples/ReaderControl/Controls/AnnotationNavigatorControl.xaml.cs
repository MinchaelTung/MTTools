using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;

using PDFTron.SilverDox.Documents.Annotations;
using PDFTron.SilverDox.Controls;
using PDFTron.SilverDox.Samples.Utility;

namespace PDFTron.SilverDox.Samples
{
    public partial class AnnotationNavigatorControl : UserControl
    {

        public AnnotationNavigatorControl()
        {
            this.RemoveAnnotationCommand = new DelegateCommand(
                RemoveAnnotation, RemoveAnnotationPredicate);

            InitializeComponent();
        }
        public ICommand RemoveAnnotationCommand { get; private set; }
        private void RemoveAnnotation(object p) { this.annotationsListBox.DeleteSelectedItem(); }
        private bool RemoveAnnotationPredicate(object p)
        {
            if (this.annotationsListBox.SelectedAnnotations.Count == 1)
            {
                return this.annotationsListBox.SelectedAnnotations[0].IsSelectable;
            }
            return false;
        }

        private void annotationListContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            var contextMenu = sender as ContextMenu;
            var selectedAnnotation = contextMenu.DataContext as Annotation;

            //if( !selectedAnnotation.IsSelectable )
            if (selectedAnnotation != null)
                this.annotationsListBox.SelectedAnnotations = new List<Annotation>() { selectedAnnotation };

            //recompute predicate for command
            (this.RemoveAnnotationCommand as DelegateCommand).RaiseCanExecuteChanged();
        }

        private void annotationNavigationUserControl_KeyDown(object sender, KeyEventArgs e)
        {
            //If delete key call delete
            if (e.Key == Key.Delete) this.annotationsListBox.DeleteSelectedItem();
        }
 
    }
}
