using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.ComponentModel;

using PDFTron.SilverDox.Controls;
using PDFTron.SilverDox.Documents.Annotations;
using PDFTron.SilverDox.Samples.Utility;
using System.Windows.Input;

namespace PDFTron.SilverDox.Samples.Controls
{
    /// <summary>
    /// A list box control that displays a list of annotations allowing them to be selected
    /// </summary>
    public class AnnotationsListBox : ListBox, INotifyPropertyChanged
    {
        #region Properties

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


        private IEnumerable<Annotation> oldAnnotationList;
        /// <summary>
        /// A list of all annotations in the DocumentViewer
        /// </summary>
        public IEnumerable<Annotation> AnnotationList
        {
            get
            {
                if (this.DocumentViewer != null && this.DocumentViewer.AnnotationManager != null)
                {
                    var annotationList = this.DocumentViewer.AnnotationManager.AnnotationsList;
                    annotationList = annotationList.OrderBy(annotation => annotation.PageNumber).
                           ThenBy(annotation => annotation.Y).
                           ThenBy(annotation => annotation.X).ToList();

                    this.oldAnnotationList = annotationList;
                    return annotationList;
                    //return this.DocumentViewer.AnnotationManager.AnnotationsList;
                }

                return new List<Annotation>();
            }
        }

        /// <summary>
        /// A list of selected annotations in the DocumentViewer
        /// </summary>
        public List<Annotation> SelectedAnnotations
        {
            get
            {
                return this.SelectedItems.Cast<Annotation>().ToList();
            }
            set
            {
                this.SelectedItems.Clear();

                if (value == null) return;

                foreach (var annot in value)
                {
                    this.SelectedItems.Add(annot);
                }
            }
        }

        #endregion

        /// <summary>
        /// Creates a new instance of the AnnotationsListBox
        /// </summary>
        public AnnotationsListBox()
        {
            // Binds ItemsSource for this ListBox to the list of annotations in DocumentViewer
            Binding itemsSourceBinding = new Binding();
            itemsSourceBinding.Source = this;
            itemsSourceBinding.Path = new PropertyPath("AnnotationList"); 
            this.SetBinding(ListBox.ItemsSourceProperty, itemsSourceBinding);

            // Hookup event handlers
            this.Loaded += new RoutedEventHandler(AnnotationsListBox_Loaded);
            this.SelectionChanged += new SelectionChangedEventHandler(AnnotationsListBox_SelectionChanged);
        }

        /// <summary>
        /// Removes the currently selected annotations
        /// </summary>
        public bool RemoveSelectedAnnotations()
        {
            if( this.DocumentViewer != null)
            {
                if (this.SelectedAnnotations.Count > 0 )
                {
                    var annotationManager = this.DocumentViewer.AnnotationManager;
                    annotationManager.RemoveAnnotations(this.SelectedAnnotations);

                    this.SelectedAnnotations = null;
                    return true;
                }
            }
            return false;
        }

        
        #region EventHandlers

        private void OnAnnotationsSelected(object sender, AnnotationsEventArgs e)
        {
            if (e.Annotations.Count > 0)
            {
                this.SelectAnnotationsInAnnotationNav(e.Annotations);
            }
        }
        private void OnAnnotationsDeselected(object sender, AnnotationsEventArgs e)
        {
            if (e.Annotations.Count > 0)
            {
                this.DeselectAnnotationsInAnnotationNav(e.Annotations);
            }
        }

        private void SelectAnnotationsInAnnotationNav(List<Annotation> annotations)
        {
            foreach (var annot in annotations)
            {
                if( !this.SelectedItems.Contains(annot) )
                    this.SelectedItems.Add(annot);
            }
        }
        private void DeselectAnnotationsInAnnotationNav(IList<Annotation> annotations)
        {
            foreach (var annot in annotations)
            {
                if(this.SelectedItems.Contains(annot))
                    this.SelectedItems.Remove(annot);
            }
        }

        private void AnnotationsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Don't deselect annotation from DocumentViewer if SelectedItem in listbox is deselected
            // due to switching context away from this control
            if (this.annotationListBoxLoading) return;
            if( this.DocumentViewer != null && this.SelectedAnnotations != null)
            {
                var annotationManager = this.DocumentViewer.AnnotationManager;

                if (e.RemovedItems.Count == 0)
                {
                    //Check if the items selected in the ListBox
                    //are already selected in the DocumentViewer
                    var addedItems = from annot in e.AddedItems.Cast<Annotation>().ToList()
                                     where !annotationManager.SelectedAnnotations.Contains(annot)
                                     select annot;
                    if (addedItems.Count() == 0) return;
                }
                else
                {
                    //Check if the removed items in AnnotationListBox have already been removed from the 
                    //SelectedAnnotations in DocumentViewer
                    var removedItems = from annot in e.RemovedItems.Cast<Annotation>().ToList()
                                       where !annot.IsSelectable || 
                                             annotationManager.SelectedAnnotations.Contains(annot)
                                       select annot;
                    if (removedItems.Count() == 0) return;
                }

                //Calling DeselectAllAnnotations will trigger my OnAnnotationsDeselected  
                //event handler setting SelectedAnnotations to null, so I must save a reference 
                //to the Selected Annotation before calling this method
                var tempSelectedAnnots = this.SelectedAnnotations;

                annotationManager.DeselectAllAnnotations();

                foreach (var annot in tempSelectedAnnots)
                {
                    annotationManager.SelectAnnotation(annot);
                }

                if(this.SelectedAnnotations.Count == 1)
                    this.DocumentViewer.ScrollToAnnotation(this.SelectedAnnotations[0]);
            }
        }

        //signals if the AnnotationListBox is loading
        private bool annotationListBoxLoading = false; 
        private void AnnotationsListBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.annotationListBoxLoading = true;
            // Hooks up annotation EventHandlers when DocumentViewer is fully loaded
            var docViewer = (this.DocumentViewer as DocumentViewer);
            if (docViewer != null)
            {
                this.NotifyPropertyChanged("AnnotationList");

                //Event Handlers here to update AnnotationList when annotation changes
                this.DocumentViewer.AnnotationsCreated += new EventHandler<AnnotationsEventArgs>(
                    (obj, args) => { this.NotifyPropertyChanged("AnnotationList"); }
                );
                this.DocumentViewer.AnnotationsRemoved += new EventHandler<AnnotationsEventArgs>(
                    (obj, args) => { this.NotifyPropertyChanged("AnnotationList"); }
                );
                this.DocumentViewer.AnnotationsAdded += new EventHandler<AnnotationsEventArgs>(
                    (obj, args) => { this.NotifyPropertyChanged("AnnotationList"); }
                );

                this.DocumentViewer.AnnotationResized += new EventHandler<AnnotationResizedEventArgs>(
                    (obj, args) => 
                    { 
                        this.RefreshAnnotationsList();
                        //this.NotifyPropertyChanged("AnnotationList"); 
                        this.DocumentViewer.AnnotationManager.SelectAnnotation(args.ResizedAnnotation);
                    }
                );

                this.DocumentViewer.AnnotationsMoveCompleted += new EventHandler<AnnotationsMoveCompletedEventArgs>(
                    (obj, args) =>
                    {
                        //this.NotifyPropertyChanged("AnnotationList");
                        this.RefreshAnnotationsList();

                        foreach (var annots in args.MovedAnnotations)
                            this.DocumentViewer.AnnotationManager.SelectAnnotation(annots);
                    }
                );

                this.DocumentViewer.AnnotationsSelected +=
                    new EventHandler<AnnotationsEventArgs>(this.OnAnnotationsSelected);

                this.DocumentViewer.AnnotationsDeselected +=
                    new EventHandler<AnnotationsEventArgs>(this.OnAnnotationsDeselected);
            }

            // Set any selected annotation
            if (this.DocumentViewer != null && this.DocumentViewer.AnnotationManager != null)
            {
                this.SelectedAnnotations = this.DocumentViewer.AnnotationManager.SelectedAnnotations;
            }
            
            this.annotationListBoxLoading = false;
        }
        
        #endregion

        private void RefreshAnnotationsList()
        {
            var annotationList = this.DocumentViewer.AnnotationManager.AnnotationsList;
            annotationList = annotationList.OrderBy(annotation => annotation.PageNumber).
                   ThenBy(annotation => annotation.Y).
                   ThenBy(annotation => annotation.X).ToList();

            if (!annotationList.SequenceEqual(this.oldAnnotationList))
            {
                this.NotifyPropertyChanged("AnnotationList");
            }
        }

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
    }
}