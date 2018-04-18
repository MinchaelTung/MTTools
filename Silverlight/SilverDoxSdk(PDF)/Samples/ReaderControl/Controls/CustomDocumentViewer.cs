using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PDFTron.SilverDox.Controls;
using PDFTron.SilverDox.Samples.Utility;
using System.Security;
using PDFTron.SilverDox.Samples.Resources;
using System.Windows.Media.Imaging;
using System.Windows.Data;


namespace PDFTron.SilverDox.Samples.Controls
{

    /// <summary>
    /// A custom <see cref="DocumentViewer"/> that overrides the default context menu
    /// </summary>
    public class CustomDocumentViewer : DocumentViewer
    {
        internal ContextMenu ContextMenu { get; set; }

        public CustomDocumentViewer()
            : base()
        {
            this.CopyCommand = new DelegateCommand(CopyCommandAction, CopyCommandActionPredicate);
            this.ZoomInCommand = new DelegateCommand(ZoomInCommandAction, DocumentNotNullPredicate);
            this.ZoomOutCommand = new DelegateCommand(ZoomOutCommandAction, DocumentNotNullPredicate);
            this.PanToolCommand = new DelegateCommand(PanToolCommandAction);
            this.SelectToolCommand = new DelegateCommand(SelectToolCommandAction);
            this.FitWidthCommand = new DelegateCommand(FitWidthCommandAction, DocumentNotNullPredicate);
            this.FitPageCommand = new DelegateCommand(FitPageCommandAction, DocumentNotNullPredicate);

            this.RemoveAnnotationCommand = new DelegateCommand(
                this.RemoveAnnotationCommandAction, this.RemoveAnnotationCommandActionPredicate);
            this.CopyAnnotationCommand = new DelegateCommand(
                this.CopyAnnotationCommandAction, this.CopyAnnotationCommandActionPredicate);
            this.PasteAnnotationCommand = new DelegateCommand(
                this.PasteAnnotationCommandAction, this.PasteAnnotationCommandActionPredicate);
            this.UndoAnnotationCommand = new DelegateCommand(
                this.UndoAnnotationCommandAction, this.UndoAnnotationCommandActionPredicate);
            this.RedoAnnotationCommand = new DelegateCommand(
                this.RedoAnnotationCommandAction, this.RedoAnnotationCommandActionPredicate);
        }

        #region Commands
        public ICommand CopyCommand { get; private set; }
        private void CopyCommandAction(object p)
        {
            try
            {
                Clipboard.SetText(this.GetSelectedText());
            }
            catch (SecurityException)
            {
                System.Diagnostics.Debug.WriteLine("The user did not grant Silverlight clipboard access");
            }

        }
        private bool CopyCommandActionPredicate(object p)
        {
            return (!String.IsNullOrEmpty(this.GetSelectedText()));
        }

        private bool DocumentNotNullPredicate(object p)
        {
            return this.Document != null;
        }


        public ICommand ZoomInCommand { get; private set; }
        private void ZoomInCommandAction(object p)
        {
            this.SetFitMode(DocumentViewer.FitModes.None, DocumentViewer.FitModes.None);
            this.Zoom = this.Zoom * 1.5;
        }


        public ICommand ZoomOutCommand { get; private set; }
        private void ZoomOutCommandAction(object p)
        {
            this.SetFitMode(DocumentViewer.FitModes.None, DocumentViewer.FitModes.None);
            this.Zoom = this.Zoom / 1.5;
        }

        public ICommand PanToolCommand { get; private set; }
        private void PanToolCommandAction(object p)
        {
            this.ToolMode = DocumentViewer.ToolModes.Pan;
        }

        public ICommand SelectToolCommand { get; private set; }
        private void SelectToolCommandAction(object p)
        {
            this.ToolMode = DocumentViewer.ToolModes.TextSelect;
        }

        public ICommand FitWidthCommand { get; private set; }
        private void FitWidthCommandAction(object p)
        {
            this.SetFitMode(DocumentViewer.FitModes.Panel, DocumentViewer.FitModes.None);

        }

        public ICommand FitPageCommand { get; private set; }
        private void FitPageCommandAction(object p)
        {
            this.SetFitMode(DocumentViewer.FitModes.Panel, DocumentViewer.FitModes.Page);
        }

        // Annotation DelegateCommands

        //Remove Annotation
        public ICommand RemoveAnnotationCommand { get; private set; }
        private void RemoveAnnotationCommandAction(object p)
        {
            if (this.AnnotationManager == null) return;
            this.AnnotationManager.RemoveAnnotations(this.AnnotationManager.SelectedAnnotations);
        }
        private bool RemoveAnnotationCommandActionPredicate(object p)
        {
            if (this.AnnotationManager == null) return false;
            return (this.AnnotationManager.SelectedAnnotations.Count > 0);
        }

        // Copy Annotation
        public ICommand CopyAnnotationCommand { get; private set; }
        private void CopyAnnotationCommandAction(object p)
        {
            if (this.AnnotationManager == null) return;
            this.AnnotationManager.CopySelectedAnnotations();
        }
        private bool CopyAnnotationCommandActionPredicate(object p)
        {
            if (this.AnnotationManager == null) return false;

            if(this.AnnotationManager.SelectedAnnotations.Count == 0)
                return false;

            foreach( var annot in this.AnnotationManager.SelectedAnnotations)
            {
                if (annot.IsCopyable) return true;
            }
            return false;
        }

        //Paste Annotation
        public ICommand PasteAnnotationCommand { get; private set; }
        private void PasteAnnotationCommandAction(object p)
        {
            if (this.AnnotationManager == null) return;
            var pastedAnnots = 
                this.AnnotationManager.PasteCopiedAnnotations(this.clickedPageNum);

            foreach (var annot in pastedAnnots)
            {
                this.AnnotationManager.RedrawAnnotation(annot);
            }
        }
        private bool PasteAnnotationCommandActionPredicate(object p)
        {
            if (this.AnnotationManager == null) return false;
            return (this.AnnotationManager.CanPaste);
        }

        // Undo Annotation
        public ICommand UndoAnnotationCommand { get; private set; }
        private void UndoAnnotationCommandAction(object p)
        {
            if (this.AnnotationManager == null) return;
            this.AnnotationManager.Undo();
        }
        private bool UndoAnnotationCommandActionPredicate(object p)
        {
            if (this.AnnotationManager == null) return false;
            return (this.AnnotationManager.CanUndo);
        }

        // Redo Annotation
        public ICommand RedoAnnotationCommand { get; private set; }
        private void RedoAnnotationCommandAction(object p)
        {
            if (this.AnnotationManager == null) return;
            this.AnnotationManager.Redo();
        }
        private bool RedoAnnotationCommandActionPredicate(object p)
        {
            if (this.AnnotationManager == null) return false;
            return (this.AnnotationManager.CanRedo);
        }

        #endregion
        
        /// <summary>
        /// Override of the DocumentViewer's content Loaded event handler.
        /// Register a toolkit ContextMenu on the sender once it is loaded.
        /// </summary>
        /// <param name="sender">The Content area of the DocumentViewer</param>
        /// <param name="e"></param>
        protected override void OnContentLoaded(object sender, RoutedEventArgs e)
        {
            base.OnContentLoaded(sender, e);

            this.ToolMode = ToolModes.Pan;
            
            if (ContextMenuService.GetContextMenu(sender as DependencyObject) == null)
            {
                
                
                ContextMenu menu = new ContextMenu();
                this.ContextMenu = menu;

                var copyMenuItem = new MenuItem()
                {
                    Header = StringResource.CopyCommand,
                    Command = CopyCommand,
                };
                this.SetMenutItemVisibilityBinding(copyMenuItem);
                menu.Items.Add(copyMenuItem);
                //-----

                menu.Items.Add(new MenuItem() { Header = StringResource.PanToolCommand, Command = PanToolCommand ,
                                                Icon = getImageFromRelativePath("/ReaderControl;component/Resources/hand-16.png")
                });
                menu.Items.Add(new MenuItem() { Header = StringResource.SelectToolCommand, Command = SelectToolCommand ,
                                                Icon = getImageFromRelativePath("/ReaderControl;component/Resources/ibeam-16.png")
                });
                menu.Items.Add(new Separator());
                menu.Items.Add(new MenuItem() { Header = StringResource.ZoomInCommand, Command = ZoomInCommand,
                                                Icon = getImageFromRelativePath("/ReaderControl;component/Resources/zoom_in.png")
                });
                menu.Items.Add(new MenuItem() { Header = StringResource.ZoomOutCommand, Command = ZoomOutCommand,
                                                Icon = getImageFromRelativePath("/ReaderControl;component/Resources/zoom_out.png")
                });
                menu.Items.Add(new Separator());
                menu.Items.Add(new MenuItem() { Header = StringResource.FitWidthCommand, Command = FitWidthCommand,
                                                Icon = getImageFromRelativePath("/ReaderControl;component/Resources/arrow_ew.png")
                });
                menu.Items.Add(new MenuItem() { Header = StringResource.FitPageCommand, Command = FitPageCommand,
                                                Icon = getImageFromRelativePath("/ReaderControl;component/Resources/arrow_nsew.png")
                });

                menu.Items.Add(new Separator());
                //----

                menu.Items.Add(new MenuItem()  { 
                    Header = StringResource.UndoAnnotationCommand, Command = this.UndoAnnotationCommand,
                    Icon = getImageFromRelativePath("/ReaderControl;component/Resources/arrow_undo.png")
                });
                menu.Items.Add(new MenuItem()  { 
                    Header = StringResource.RedoAnnotationCommand, Command = this.RedoAnnotationCommand,
                    Icon = getImageFromRelativePath("/ReaderControl;component/Resources/arrow_redo.png")
                });
                menu.Items.Add(new Separator());
                //-----

                var removeAnnotMenuItem = new MenuItem()  { 
                    Header = StringResource.RemoveAnnotationCommand, Command = this.RemoveAnnotationCommand,
                    Icon = getImageFromRelativePath("/ReaderControl;component/Resources/cross.png")
                };
                menu.Items.Add(removeAnnotMenuItem);
                this.SetMenutItemVisibilityBinding(removeAnnotMenuItem);

                menu.Items.Add(new MenuItem()  { 
                    Header = StringResource.CopyAnnotationCommand, Command = this.CopyAnnotationCommand,
                    Icon = getImageFromRelativePath("/ReaderControl;component/Resources/page_copy.png")
                });
                menu.Items.Add(new MenuItem()  { 
                    Header = StringResource.PasteAnnotationCommand, Command = this.PasteAnnotationCommand,
                    Icon = getImageFromRelativePath("/ReaderControl;component/Resources/page_white_paste_table.png")
                });

                ContextMenuService.SetContextMenu((sender as DependencyObject), menu);
            }
        }

        private void SetMenutItemVisibilityBinding(MenuItem menuItem)
        {
            //Binds the Visibility to the IsEnabled property for a given MenuItem
            //Essentially collapses items rather than gray them out when predicate is not matched
            var visibilityBinding = new Binding()
            {
                Source = menuItem,
                Path = new PropertyPath("IsEnabled"),
                Converter = new Utility.VisibilityConverter()
            };
            menuItem.SetBinding(MenuItem.VisibilityProperty, visibilityBinding);
        }

        /// <summary>
        /// Suppress the default context menu menu.
        /// Re-evaluate whether if the copy command can be executed.
        /// </summary>
        /// <param name="sender">The Content area of the DocumentViewer</param>
        /// <param name="e"></param>
        protected override void OnContentMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Get page number/ x and y offset of page that was clicked
            var keyValPair = this.GetCanvasAtPoint(e.GetPosition(null));

            Canvas currCanvas = keyValPair.Value;

            this.clickedPageNum = keyValPair.Key;
            this.clickedPageXOffset = e.GetPosition(currCanvas).X;
            this.clickedPageYOffset = e.GetPosition(currCanvas).Y;

            //Get annotation that was clicked and select it if in AnnotationEdit mode
            if (this.ToolMode == ToolModes.AnnotationEdit)
            {
                var clickedAnnot = this.AnnotationManager.GetAnnotationByPoint(e.GetPosition(null));

                if (clickedAnnot != null && !this.AnnotationManager.SelectedAnnotations.Contains(clickedAnnot))
                {
                    this.AnnotationManager.DeselectAllAnnotations();
                    this.AnnotationManager.SelectAnnotation(clickedAnnot);
                }
            }


            this.RaiseCanExecuteChangedForAllContextMenuItems();
        }

        private int clickedPageNum = 0;
        private double clickedPageXOffset = 0;
        private double clickedPageYOffset = 0;

        private void RaiseCanExecuteChangedForAllContextMenuItems()
        {
            foreach (var item in this.ContextMenu.Items)
            {
                var menuItem = item as MenuItem;
                if( menuItem != null && menuItem.Command != null)
                {
                    (menuItem.Command as DelegateCommand).RaiseCanExecuteChanged();
                }
            }
        }


        private Image getImageFromRelativePath(String path)
        {
            Image img = new Image() { Source = new BitmapImage(new Uri(path, UriKind.Relative)) };
            img.Stretch = Stretch.None;
            return img;
        }

    }
}
