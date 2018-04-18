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
using PDFTron.SilverDox.IO;
using PDFTron.SilverDox.Documents;
using PDFTron.SilverDox.Samples;
using PDFTron.SilverDox.Samples.SubControls;
using PDFTron.SilverDox.Controls;
using System.IO;
using System.Diagnostics;
using PDFTron.SilverDox.Samples.Utility;
using System.ComponentModel;
using System.Windows.Data;

using PDFTron.SilverDox.Samples.Controls;
using PDFTron.SilverDox.Samples.Resources;
using PDFTron.SilverDox.Documents.Annotations;


namespace PDFTron.SilverDox.Samples
{

    /// <summary>
    /// Represents a control that provides tools for user interaction with the DocumentViewer.
    /// </summary>
    public partial class ReaderControl : UserControl, INotifyPropertyChanged
    {
        private IPartRetriever myRetriever = null;
        private bool _toolsCreated;
        private bool _documentLoaded;
        private GridLength _sideWindowWidth = new GridLength(200, GridUnitType.Pixel);
        private const double MIN_SIDE_WINDOW_WIDTH = 110; // minimum size of the TabControl before the TabItems start to wrap vertically

        #region Toolbar & Sidewindow Creation

        /// <summary>
        /// Determines the creation of the OpenLocalFileControl upon tool bar generation
        /// </summary>       
        public bool EnableOpenLocalFileControl { get; set; }

        /// <summary>
        /// Determines the creation of the PageNumberControl upon tool bar generation
        /// </summary>
        public bool EnablePageNumberControl { get; set; }

        /// <summary>
        /// Determines the creation of the PageNavigationControl upon tool bar generation
        /// </summary>
        public bool EnablePageNavigationControl { get; set; }

        /// <summary>
        /// Determines the creation of the ZoomTextBoxControl; upon tool bar generation
        /// </summary>
        public bool EnableZoomTextBoxControl { get; set; }

        /// <summary>
        /// Determines the creation of the ZoomSliderControl upon tool bar generation
        /// </summary>
        public bool EnableZoomSliderControl { get; set; }

        /// <summary>
        /// Determines the creation of the FitModeControl upon tool bar generation
        /// </summary>
        public bool EnableFitModeControl { get; set; }

        /// <summary>
        /// Determines the creation of the ToolModeControl upon tool bar generation
        /// </summary>
        public bool EnableToolModeControl { get; set; }

        /// <summary>
        /// Determines the creation of the SearchControl upon tool bar generation
        /// </summary>
        public bool EnableSearchControl { get; set; }

        /// <summary>
        /// Determines the creation of the FullScreenControl upon tool bar generation
        /// </summary>
        public bool EnableFullScreenControl { get; set; }

        /// <summary>
        /// Determines the creation of the PrintControl upon tool bar generation
        /// </summary>
        public bool EnablePrintControl { get; set; }

        /// <summary>
        /// Determines the creation of the OutlineToggleButton upon tool bar generation
        /// </summary>
        public bool EnableOutlineToggleControl { get; set; }

        /// <summary>
        /// Determines the creation of the ThumbnailListControl upon side windowr generation
        /// </summary>
        public bool EnableThumbnailListControl { get; set; }

        /// <summary>
        /// Determines the creation of the OutlineTreeControl upon side window generation
        /// </summary>
        public bool EnableOutlineTreeControl { get; set; }

        /// <summary>
        /// Determines the creation of the EnableFullTextSearchControl upon side window generation
        /// </summary>
        public bool EnableFullTextSearchControl { get; set; }

        /// <summary>
        /// Determines the creation of the LayoutControl upon tool bar generation
        /// </summary>
        public bool EnableLayoutControl { get; set; }

        /// <summary>
        /// Determines the creation of the RotateControl upon tool bar generation
        /// </summary>
        public bool EnableRotateControl { get; set; }

        private bool enableAnnotationWindowControl;
        /// <summary>
        /// Determines the creation of the Annotation control in the sidebar
        /// </summary>
        public bool EnableAnnotationWindowControl
        {
            get { return this.enableAnnotationWindowControl; }
            set
            {
                this.enableAnnotationWindowControl = value;
                if (this.DocumentSideWindow == null) return;

                if (this.EnableAnnotationWindowControl)
                {
                    this.DocumentSideWindow.SetAnnotationWindowControlTabVisibility(Visibility.Visible);
                }
                else
                {
                    this.DocumentSideWindow.SetAnnotationWindowControlTabVisibility(Visibility.Collapsed);
                }
            }
        }

        /// <summary>
        /// Notification event that is raised when a bound property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Used for opening the search side panel if node is opened from a search result.
        /// </summary>
        public String InitialSearchTerm { get; set; }

        /// <summary>
        /// Layout mode enumerations
        /// </summary>
        public enum LayoutModes
        {
            Continuous = 0,
            FacingContinous = 1,
            FacingCoverContinuous = 2,
            SinglePage = 3,
            Facing = 4,
            FacingCover = 5
        }

        private LayoutModes _layoutMode;
        /// <summary>
        /// Determines the page presentation mode of the document
        /// </summary>
        public LayoutModes LayoutMode
        {
            get { return _layoutMode; }
            set
            {
                if (_layoutMode != value)
                {
                    _layoutMode = value;
                    this.SetLayout(_layoutMode);
                    if (PropertyChanged != null)
                        this.PropertyChanged(this, new PropertyChangedEventArgs("LayoutMode"));
                }
            }
        }

        /// <summary>
        /// Page View mode enumerations
        /// </summary>
        public enum PageViewModes
        {
            /// <summary>
            /// Page is zoomed. The zoom ratio is specified using <see cref="DocumentViewer.Zoom"/> property.
            /// </summary>
            Zoom = 0,

            /// <summary>
            /// Page zoom is automatically adjusted so that page width fits into available space.
            /// </summary>
            FitWidth = 1,

            /// <summary>
            /// Page zoom is automatically adjusted so that page height fits into available space.
            /// </summary>
            FitHeight = 2,

            /// <summary>
            /// Page zoom is automatically adjusted so that entire page fits into available space.
            /// </summary>
            FitPage = 3
        }


        private void UpdatePageViewMode()
        {
            if (this.FixedDocViewer.FitModeWidth == DocumentViewer.FitModes.Panel && this.FixedDocViewer.FitModeHeight == DocumentViewer.FitModes.None)
                PageViewMode = PageViewModes.FitWidth;
            else if (this.FixedDocViewer.FitModeWidth == DocumentViewer.FitModes.Panel && this.FixedDocViewer.FitModeHeight == DocumentViewer.FitModes.Page)
                PageViewMode = PageViewModes.FitPage;
            else if (this.FixedDocViewer.FitModeWidth == DocumentViewer.FitModes.None && this.FixedDocViewer.FitModeHeight == DocumentViewer.FitModes.Page)
                PageViewMode = PageViewModes.FitHeight;
            else
                PageViewMode = PageViewModes.Zoom;

        }

        private PageViewModes _PageViewMode;

        /// <summary>
        /// Gets or sets the ReaderControl's PageViewMode
        /// </summary>
        public PageViewModes PageViewMode
        {
            get
            {
                return _PageViewMode;
            }
            set 
            {
                if (_PageViewMode != value)
                {
                    _PageViewMode = value;

                    if (PropertyChanged != null)
                        this.PropertyChanged(this, new PropertyChangedEventArgs("PageViewMode"));

                    if (_PageViewMode == PageViewModes.FitWidth)
                        FixedDocViewer.SetFitMode(DocumentViewer.FitModes.Panel, DocumentViewer.FitModes.None);
                    else if (_PageViewMode == PageViewModes.FitHeight)
                        FixedDocViewer.SetFitMode(DocumentViewer.FitModes.None, DocumentViewer.FitModes.Page);
                    else if (_PageViewMode == PageViewModes.FitPage)
                        FixedDocViewer.SetFitMode(DocumentViewer.FitModes.Panel, DocumentViewer.FitModes.Page);
                    else if (_PageViewMode == PageViewModes.Zoom)
                        FixedDocViewer.SetFitMode(DocumentViewer.FitModes.None, DocumentViewer.FitModes.None);                    
                }
            } 
        }

        private void SetToolsAndWindowDefault()
        {
            EnableOutlineToggleControl = true;
            EnablePageNumberControl = false;
            EnablePageNavigationControl = true;

            EnableZoomSliderControl = true;
            EnableZoomTextBoxControl = true;
            EnableFitModeControl = true;

            EnableToolModeControl = true;
            EnableFullScreenControl = true;
            EnablePrintControl = true;
            EnableOpenLocalFileControl = true;
            EnableSearchControl = true;

            EnableThumbnailListControl = true;
            EnableOutlineTreeControl = true;
            EnableFullTextSearchControl = true;

            EnableLayoutControl = true;
            EnableRotateControl = true;

            this.EnableAnnotationWindowControl = true;
        }

        #endregion

        #region Dependency Properties
        /// <summary>
        /// Dependency Property for ShowToolbar
        /// </summary>
        public static readonly DependencyProperty ShowToolbarProperty
        = DependencyProperty.Register("ShowToolbarProperty", typeof(bool),
        typeof(ReaderControl), new PropertyMetadata(true, new PropertyChangedCallback(OnShowToolbarPropertyChanged)));

        /// <summary>
        /// Displays or Hides the tool bar on the side window
        /// </summary>
        public bool ShowToolbar
        {
            get { return (bool)GetValue(ShowToolbarProperty); }
            set { SetValue(ShowToolbarProperty, value); }
        }

        private static void OnShowToolbarPropertyChanged(Object sender, DependencyPropertyChangedEventArgs e)
        {
            ReaderControl source = (ReaderControl)sender;

            if ((bool)e.NewValue)
            {
                source.DocumentToolbar.Visibility = Visibility.Visible;
            }
            else
            {
                source.DocumentToolbar.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Dependency Property for ShowSideWindow
        /// </summary>
        public static readonly DependencyProperty ShowSideWindowProperty
        = DependencyProperty.Register("ShowSideWindowProperty", typeof(bool),
        typeof(ReaderControl), new PropertyMetadata(false, new PropertyChangedCallback(OnShowSideWindowPropertyChanged)));

        /// <summary>
        /// Displays or hides the side window.
        /// </summary>
        public bool ShowSideWindow
        {
            get { return (bool)GetValue(ShowSideWindowProperty); }
            set { SetValue(ShowSideWindowProperty, value); }
        }

        private static void OnShowSideWindowPropertyChanged(Object sender, DependencyPropertyChangedEventArgs e)
        {
            ReaderControl source = (ReaderControl)sender;
            source.SetSideWindowVisible((bool)e.NewValue);

        }

        /// <summary>
        /// Dependency Property for InitialDocumentUrl
        /// </summary>
        public static readonly DependencyProperty InitialDocumentUrlProperty
        = DependencyProperty.Register("InitialDocumentUrlProperty", typeof(string),
        typeof(ReaderControl), new PropertyMetadata(null, new PropertyChangedCallback(OnInitialDocumentUrlPropertyChanged)));

        /// <summary>
        /// Uri of the document to be loaded initially.
        /// </summary>
        public string InitialDocumentUrl
        {
            get { return (string)GetValue(InitialDocumentUrlProperty); }
            set { SetValue(InitialDocumentUrlProperty, value); }
        }

        private static void OnInitialDocumentUrlPropertyChanged(Object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                ReaderControl source = (ReaderControl)sender;
                string url = e.NewValue as string;
                //source.LoadDocument(url);
            }
        }
        #endregion

        /// <summary>
        /// Reference to the underlying DocumentViewer used by this ReaderControl
        /// </summary>
        public DocumentViewer DocumentViewer { get { return this.FixedDocViewer; } }

        /// <summary>
        /// Creates a new ReaderControl
        /// </summary>
        public ReaderControl()
        {
            InitializeComponent();

            _toolsCreated = false;
            _documentLoaded = false;

            SetToolsAndWindowDefault();
            this.FixedDocViewer.PropertyChanged += new PropertyChangedEventHandler(FixedDocViewer_PropertyChanged);
            this.PageViewMode = PageViewModes.FitWidth;

            //MenuItem dockMenuItem = new MenuItem();
            //dockMenuItem.Header = "Dock on bottom";
            //dockMenuItem.Click += new RoutedEventHandler(dockMenuItem_Click);
            //this.DocumentToolbar.ContextMenu.Items.Add(dockMenuItem);
        }

        void dockMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (DocumentToolbar.VerticalAlignment == System.Windows.VerticalAlignment.Top)
            {
                (sender as MenuItem).Header = "Dock on top";
                DocumentToolbar.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            }
            else if (DocumentToolbar.VerticalAlignment == System.Windows.VerticalAlignment.Bottom)
            {
                (sender as MenuItem).Header = "Dock on botton";
                DocumentToolbar.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            }
        }

        private void FixedDocViewer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("FitModeWidth") ||e.PropertyName.Equals("FitModeHeight"))
            {
                UpdatePageViewMode();
            }
        }


        private void OnLoadAsyncCallback(Exception error)
        {

            if (error != null)
            {
                String errorString = StringResource.ErrorLoadingDocument + ": ";
                WebException webException = error as WebException;

                if (error.Message != String.Empty)
                    errorString += error.Message;
                if (error.InnerException != null && error.InnerException.Message != String.Empty && error.InnerException.Message != error.Message)
                    errorString += " " + error.InnerException.Message;

                if (error.Message == String.Empty && (error.InnerException == null || error.InnerException.Message == String.Empty))
                    errorString += StringResource.UnknownError;

                MessageBox.Show(errorString);
                this.OnDocumentLoaded(errorString);
            }
            else
            {
              this.OnDocumentLoaded(null);
            }

            
        }

        /// <summary>
        /// Loads a remote document through url
        /// </summary>
        /// <param name="path">url path in string</param>
        /// <param name="streaming">whether to use HttpStreamingPartRetriever instead of HttpPartRetriever</param>
        public void LoadDocument(String path, bool streaming = false)
        {
            if (System.ComponentModel.DesignerProperties.IsInDesignTool)
                return;

            if (path == null || path == String.Empty)
                return;

            if (myRetriever != null)
                myRetriever.CancelAllRequests();

            Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);

            if (streaming)
                myRetriever = new HttpStreamingPartRetriever(uri);
            else
                myRetriever = new HttpPartRetriever(uri);

            FixedDocViewer.LoadAsync(myRetriever, OnLoadAsyncCallback);
            this.OnDocumentChanged(uri);
        }

        /// <summary>
        /// Loads a remote document through url
        /// </summary>
        /// <param name="uri">url path in Uri</param>
        /// <param name="streaming">whether to use HttpStreamingPartRetriever instead of HttpPartRetriever</param>
        public void LoadDocument(Uri uri, bool streaming = false)
        {
            if (System.ComponentModel.DesignerProperties.IsInDesignTool)
                return;

            if (uri == null)
                return;

            if (streaming)
                myRetriever = new HttpStreamingPartRetriever(uri);
            else
                myRetriever = new HttpPartRetriever(uri);

            FixedDocViewer.LoadAsync(myRetriever, OnLoadAsyncCallback);
            this.OnDocumentChanged(uri);
        }

        /// <summary>
        /// Loads a local document from client file system
        /// </summary>
        public void LoadLocalDocument()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            dlg.Filter = "XOD Files (*.xod)|*.xod";

            // open dialog
            bool ok = (bool)dlg.ShowDialog();

            if (ok)
            {
                if (myRetriever as LocalPartRetriever != null)
                    ((LocalPartRetriever)myRetriever).Dispose();
                FileStream fileStream = dlg.File.OpenRead();
                myRetriever = new LocalPartRetriever(fileStream);
                FixedDocViewer.LoadAsync(myRetriever, OnLoadAsyncCallback);
                this.OnDocumentChanged(dlg.File);
            }
        }

        /// <summary>
        /// Adds a custom tab item to the side tab control
        /// </summary>
        /// <param name="item"></param>
        public void AddCustomTabItem(TabItem item)
        {
            this.DocumentSideWindow.SideTabControl.Items.Add(item);
        }

        /// <summary>
        /// Adds a custom tab item to the DocumentViewer context menu
        /// </summary>
        /// <param name="item"></param>
        public void AddCustomContextMenuItem(object item)
        {
            //ContextMenu not loaded fast enough...
            var cDV = (this.DocumentViewer as CustomDocumentViewer);
            if (cDV != null && cDV.ContextMenu != null)
                (this.DocumentViewer as CustomDocumentViewer).ContextMenu.Items.Add(item);
        }

        /// <summary>
        /// Adds a custom tool item (UI element) to the tool bar
        /// </summary>
        /// <param name="item"></param>
        public void AddCustomToolItem(FrameworkElement item)
        {
            this.DocumentToolbar.ToolStackPanel.Children.Add(item);
        }

        /// <summary>
        /// Returns all the annotations that have been changed since the last time annotations were loaded.
        /// </summary>
        public IDictionary<Annotation, AnnotationManager.TypeOfChange> GetAllChangedAnnotations()
        {
            return this.DocumentViewer.AnnotationManager.GetAllChangedAnnotations();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            if (!_toolsCreated)
            {
                // Generate toolbar
                DocumentToolbar.CreateToolbar(this);
                if (this.EnableOutlineTreeControl == true || this.EnableThumbnailListControl == true ||
                    this.EnableSearchControl == true)
                {
                    this.DocumentSideWindow.CreateSideWindow(this);
                }
                _toolsCreated = true;
                SetSideWindowVisible(this.ShowSideWindow);
            }
            if (!_documentLoaded && InitialDocumentUrl != null)
            {
                //load initial document uri specified by xaml
                this.LoadDocument(InitialDocumentUrl);
                _documentLoaded = true;

            }

            //commandStack is to support redo/undo
            //this.commandStack = AnnotationCommandStack.Instance;
            //Hook up event handlers in the CommandStack to capture actions for undo/redo
            //AnnotationCommandHelper.HookAnnotationCommandEventHandlers(this.DocumentViewer);

        }

        // Toggles the visilbility of the side window.
        // Called when the Dependency Property ShowSideWindow is changed.
        private void SetSideWindowVisible(bool visible)
        {
            if (visible)
            {
                DocumentSideWindow.Visibility = Visibility.Visible;
                SideWindowSplitter.Visibility = Visibility.Visible;
                LowerGrid.ColumnDefinitions[0].Width = _sideWindowWidth;
                this.DocViewerBorder.Margin = new Thickness(5, 0, 0, 0);
            }
            else
            {


                if (this.IsTabItemsVerticalStacked())
                    _sideWindowWidth = new GridLength(200, GridUnitType.Pixel);
                else
                    _sideWindowWidth = LowerGrid.ColumnDefinitions[0].Width;

                LowerGrid.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
                this.DocViewerBorder.Margin = new Thickness(0, 0, 0, 0);
                DocumentSideWindow.Visibility = Visibility.Collapsed;
                SideWindowSplitter.Visibility = Visibility.Collapsed;
            }
        }


        private bool IsTabItemsVerticalStacked()
        {
            if (this.DocumentSideWindow.SideTabControl.Items.Count == 0)
                return false;

            TabItem firstTabItem =  this.DocumentSideWindow.SideTabControl.Items[0] as TabItem;
            if (firstTabItem.ActualWidth == 0.0
                || firstTabItem.ActualHeight == 0.0)
                return false;

            Point firstPoint = firstTabItem.TransformToVisual(this.DocumentSideWindow.SideTabControl).Transform(new Point());
            for (int i = 1; i < this.DocumentSideWindow.SideTabControl.Items.Count; i++)
            {
                TabItem ti = this.DocumentSideWindow.SideTabControl.Items[i] as TabItem;
                Point p = ti.TransformToVisual(this.DocumentSideWindow.SideTabControl).Transform(new Point());

                if (firstPoint.Y != p.Y)
                {
                    return true;
                }

            }
            return false;
        }

        private void DocumentSideWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.IsTabItemsVerticalStacked())
            {
                this.ShowSideWindow = false;
            }

            if (this.DocumentSideWindow.ActualWidth + 8 > this.LayoutRoot.ActualWidth)
            {
                LowerGrid.ColumnDefinitions[0].Width = new GridLength(this.LayoutRoot.ActualWidth - 8, GridUnitType.Pixel);
            }


        }

        /// <summary>
        ///  Sets the page layout mode of the current document viewer
        /// </summary>
        /// <param name="mode">the LayoutModes to change to</param>
        private void SetLayout(LayoutModes mode)
        {
            Debug.WriteLine("Entering SetLayout " + DateTime.Now);
            if (mode == LayoutModes.Continuous)
                FixedDocViewer.Template = (ControlTemplate)this.Resources["VerticalLayoutTemplate"];
            else if (mode == LayoutModes.FacingContinous)
                FixedDocViewer.Template = (ControlTemplate)this.Resources["FacingLayoutTemplate"];
            else if (mode == LayoutModes.FacingCoverContinuous)
                FixedDocViewer.Template = (ControlTemplate)this.Resources["FacingCoverContinousLayoutTemplate"];
            else if (mode == LayoutModes.SinglePage)
                FixedDocViewer.Template = (ControlTemplate)this.Resources["VerticalLayoutTemplate"];
            else if (mode == LayoutModes.FacingCover)
                FixedDocViewer.Template = (ControlTemplate)this.Resources["FacingCoverContinousLayoutTemplate"];
            else if (mode == LayoutModes.Facing)
                FixedDocViewer.Template = (ControlTemplate)this.Resources["FacingLayoutTemplate"];

            if (mode == LayoutModes.SinglePage)
            {
                FixedDocViewer.DisplayMode = DocumentViewer.DisplayModes.SinglePage;
            }
            else if (mode == LayoutModes.Facing)
            {
                FixedDocViewer.DisplayMode = DocumentViewer.DisplayModes.DualPageFacing;
            }
            else if (mode == LayoutModes.FacingCover)
            {
                FixedDocViewer.DisplayMode = DocumentViewer.DisplayModes.DualPageCoverFacing;
            }
            else
            {
                FixedDocViewer.DisplayMode = DocumentViewer.DisplayModes.AllPages;
            }

            FixedDocViewer.RefreshTemplate();


        }

        private void DocumentToolbar_IsPinnedChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (e.NewValue)
            {
                //pin
                Grid.SetRow(DocumentToolbar, 0);
            }
            else
            {
                //unpin
                Grid.SetRow(DocumentToolbar, 1);
            }
        }

        /// <summary>
        /// Loads the Document with the given annotations in serialized-xml form
        /// </summary>
        /// <param name="serializedAnnotations">String containing the serailized annotations</param>
        /// <param name="filter">
        /// A predicate specifying the conditions that need to be satisfied before a new deserialized annotation is added 
        /// to the document. A return value of true means the annotation is added to the document. Null by default. 
        /// </param>
        /// <returns>A list of annotations that were deserialized and then added to the document</returns>
        public IEnumerable<Annotation> LoadAnnotations(String serializedAnnotations, Predicate<Annotation> filter=null)
        {
            if( this.DocumentViewer != null && this.DocumentViewer.AnnotationManager != null)
            {
                using( var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(serializedAnnotations)) )
                {
                    return this.DocumentViewer.AnnotationManager.LoadAnnotations(stream, filter);
                }

                //foreach (var annot in this.DocumentViewer.AnnotationManager.AnnotationsList)
                //{
                //    annot.IsSelectable = false;
                //}
            }
            return new List<Annotation>();
        }

        /// <summary>
        /// Occurs when LoadDocument or LoadLocalDocument has been called.
        /// </summary>
        public event EventHandler<DocumentChangedEventArgs> DocumentChanged;
        /// <summary>
        /// Occurs when a document loaded successfully.
        /// </summary>
        public event EventHandler<DocumentLoadedEventArgs> DocumentLoaded;

        /// <summary>
        /// Called when the user saves the annotations of the document
        /// </summary>
        //public Action<AnnotationsSavedEventArgs> AnnotationsSavedCallback { get; set; }
        /// <summary>
        /// Called when the user loads the annotations of the document
        /// </summary>
        //public Action AnnotationsLoadedCallback { get; set; }


        private void OnDocumentChanged(Uri uri)
        {
            if (this.DocumentChanged != null)
            {
                this.DocumentChanged(this, new DocumentChangedEventArgs(uri));
            }
        }
        private void OnDocumentChanged(FileInfo info)
        {
            if (this.DocumentChanged != null)
            {
                this.DocumentChanged(this, new DocumentChangedEventArgs(info));
            }
        }
        private void OnDocumentLoaded(String error)
        {
            if (this.DocumentLoaded != null)
            {
                this.DocumentLoaded(this, new DocumentLoadedEventArgs(error));
            }

        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            // Don't undo if this bubbled up from a TextBox control
            // TextBox controls already have their own undo feature
            if (e.OriginalSource as TextBox != null) return;

            var annotManager = this.DocumentViewer.AnnotationManager;

            switch(e.Key)
            {
                // Undo
                case Key.Z:
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        if(annotManager.CanUndo) annotManager.Undo();
                    }
                    e.Handled = true;
                    break;

                // Redo
                case Key.Y:
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        if(annotManager.CanRedo) annotManager.Redo();
                    }
                    e.Handled = true;
                    break;

                // Copy
                case Key.C:
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        annotManager.CopySelectedAnnotations();
                    }
                    e.Handled = true;
                    break;

                // Paste
                case Key.V:
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        annotManager.PasteCopiedAnnotations(this.DocumentViewer.CurrentPageNumber);
                    }
                    e.Handled = true;
                    break;

                // Cut
                case Key.X:
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        annotManager.CutSelectedAnnotations();
                    }
                    e.Handled = true;
                    break;
            }
        }
    }

    /// <summary>
    /// Provides data for ReaderControl's DocumentChanged event
    /// </summary>
    public class DocumentChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The document's source if loaded from an Uri
        /// </summary>
        public Uri DocumentUri { get; set; }
        /// <summary>
        /// The document's source if loaded from a local file
        /// </summary>
        public FileInfo DocumentFileInfo { get; set; }

        /// <summary>
        /// Creates a new instance of DocumentChangedEventArgs
        /// </summary>
        /// <param name="uri"></param>
        public DocumentChangedEventArgs(Uri uri)
        {
            this.DocumentUri = uri;
        }

        /// <summary>
        /// Creates a new instance of DocumentChangedEventArgs
        /// </summary>
        /// <param name="fInfo"></param>
        public DocumentChangedEventArgs(FileInfo fInfo)
        {
            this.DocumentFileInfo = fInfo;
        }
    }

    /// <summary>
    /// Provides data for ReaderControl's DocumentLoaded event
    /// </summary>
    public class DocumentLoadedEventArgs: EventArgs
    {
        /// <summary>
        /// Indicates if the document failed when loaded
        /// </summary>
        public Boolean HasError { get; set; }

        /// <summary>
        /// Description of the error if there was a failure to load the document
        /// </summary>
        public String ErrorMessage { get; set; }

        /// <summary>
        /// Creates a new instance of DocumentLoadedEventArgs
        /// </summary>
        /// <param name="error"></param>
        public DocumentLoadedEventArgs(String error)
        {
            this.ErrorMessage = error;
            this.HasError = true;
        }

    }

}
