﻿using System;
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
using PDFTron.SilverDox.Samples;
using System.Windows.Browser;
using System.ComponentModel;

using Controls = PDFTron.SilverDox.Controls;
using Documents = PDFTron.SilverDox.Documents;

namespace ReaderControlSample
{
    /// <summary>
    /// Represents a controls that houses the ReaderControl
    /// </summary>
    public partial class MainPage : UserControl
    {
        /// <summary>
        /// Expose the ReaderControl so it can be accessed via JavaScript.
        /// </summary>
        [ScriptableMember]
        public ReaderControl ReaderControl { get { return (myReaderControl); } }

        [ScriptableMember]
        public Controls.DocumentViewer DocumentViewer { get { return (myReaderControl.DocumentViewer); } }

        private IDictionary<string, string> parameters = null;
        
        /// <summary>
        /// Create a new MainPage without any parameters
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            //Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_FullScreenChanged);

            // register this objects as scriptable
            HtmlPage.RegisterScriptableObject("ReaderControl", this);
        }

        /// <summary>
        /// Create a new MainPage with parameters
        /// </summary>
        /// <param name="p">An IDictionary object that contains the parameters</param>
        public MainPage(IDictionary<string,string> p)
        {
            InitializeComponent();
            //Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_FullScreenChanged);
            this.parameters = p;
            // register this objects as scriptable
            HtmlPage.RegisterScriptableObject("ReaderControl", this);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (System.ComponentModel.DesignerProperties.IsInDesignTool)
                return;

            string key = "DocumentUri";
            if (this.parameters.ContainsKey(key) && !string.IsNullOrEmpty(this.parameters[key]))
            {
                // load document uri from html tag only if 
                // InitialDocumentUrl was not set in xaml
                if (string.IsNullOrEmpty(myReaderControl.InitialDocumentUrl))
                    myReaderControl.LoadDocument(this.parameters[key]);
            }

            key = "UseJavaScript";
            if (this.parameters.ContainsKey(key) && !string.IsNullOrEmpty(this.parameters[key]))
            {
                bool useJS = false;
                if (bool.TryParse(this.parameters[key], out useJS)) {
                    if (useJS) {
                        // hide the Silverlight controls so only the HTML controls can be used.
                        myReaderControl.EnableOpenLocalFileControl = false;
                        myReaderControl.EnableThumbnailListControl = true;
                        myReaderControl.EnableFullTextSearchControl = true;
                        myReaderControl.EnableOutlineTreeControl = true;
                        myReaderControl.EnablePageNavigationControl = false;
                        myReaderControl.EnableFitModeControl = false;
                        myReaderControl.ShowToolbar = false;
                        myReaderControl.EnableLayoutControl = false;
                        myReaderControl.EnableRotateControl = false;
                        myReaderControl.ShowSideWindow = false;
                    }
                }
            }
        }


        // Uncomment to provide event handlers for the Uri address bar in MainPage.xaml, which when uncommented provides a UI to load remotely stored .xod documents.
        /*private void Content_FullScreenChanged(object sender, EventArgs e)
        {
            this.UriTextBox.IsReadOnly = Application.Current.Host.Content.IsFullScreen;
        }

        private void UriTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!string.IsNullOrEmpty(UriTextBox.Text))
                {
                    myReaderControl.LoadDocument(UriTextBox.Text);
                }
            }
        }

        private void UriButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(UriTextBox.Text))
            {
                myReaderControl.LoadDocument(UriTextBox.Text);
            }
        } */

        // JavaScript Extensions
        #region JavaScript Extensions

        private void FixedDocViewerJS_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Application.Current.IsRunningOutOfBrowser)
                return;

            if (e.PropertyName.Equals("Zoom")) {
                if (!string.IsNullOrEmpty(OnPageZoomCallback))
                    HtmlPage.Window.Invoke(OnPageZoomCallback);
            }
            else if (e.PropertyName.Equals("CurrentPageNumber")) {
                if (!string.IsNullOrEmpty(OnPageChangeCallback))
                    HtmlPage.Window.Invoke(OnPageChangeCallback);
            }
        }

        [ScriptableMember]
        public void LoadDocument(String path, String jsOnLoadCallback)
        {
            if (path == null || path == String.Empty || Application.Current.IsRunningOutOfBrowser)
                return;

            myReaderControl.DocumentLoaded += new EventHandler<DocumentLoadedEventArgs>((sender, e) => {
                if (!string.IsNullOrEmpty(jsOnLoadCallback))
                    HtmlPage.Window.Invoke(jsOnLoadCallback);

                // register the other callbacks
                DocumentViewer.PropertyChanged -= new PropertyChangedEventHandler(FixedDocViewerJS_PropertyChanged);
                DocumentViewer.PropertyChanged += new PropertyChangedEventHandler(FixedDocViewerJS_PropertyChanged);
            });

            Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);
            myReaderControl.LoadDocument(uri);
        }

        [ScriptableMember]
        public bool GetShowSideWindow()
        {
            return myReaderControl.ShowSideWindow;
        }

        [ScriptableMember]
        public void SetShowSideWindow(bool value)
        {
            myReaderControl.ShowSideWindow = value;
        }

        [ScriptableMember]
        public int CurrentPageNumber
        {
            get
            {
                return DocumentViewer.CurrentPageNumber;
            }
            set
            {
                DocumentViewer.CurrentPageNumber = value;
            }
        }

        [ScriptableMember]
        public int PageCount
        {
            get
            {
                return DocumentViewer.PageCount;
            }
        }

        [ScriptableMember]
        public double ZoomLevel
        {
            get
            {
                return DocumentViewer.Zoom;
            }
            set
            {
                DocumentViewer.Zoom = value;
            }
        }

        [ScriptableMember]
        public void RotateClockwise()
        {
            DocumentViewer.RotateClockwise();
        }

        [ScriptableMember]
        public void RotateCounterClockwise()
        {
            DocumentViewer.RotateCounterClockwise();
        }

        [ScriptableMember]
        public string GetLayoutMode()
        {
            return myReaderControl.LayoutMode.ToString();
        }

        [ScriptableMember]
        public void SetLayoutMode(string layoutMode)
        {
            switch (layoutMode.ToLower()) {
                case "continuous":
                    myReaderControl.LayoutMode = ReaderControl.LayoutModes.Continuous;
                    break;

                case "facingcontinuous":
                    myReaderControl.LayoutMode = ReaderControl.LayoutModes.FacingContinous;
                    break;

                case "facingcovercontinuous":
                    myReaderControl.LayoutMode = ReaderControl.LayoutModes.FacingCoverContinuous;
                    break;

                case "singlepage":
                    myReaderControl.LayoutMode = ReaderControl.LayoutModes.SinglePage;
                    break;

                case "facing":
                    myReaderControl.LayoutMode = ReaderControl.LayoutModes.Facing;
                    break;

                case "facingcover":
                    myReaderControl.LayoutMode = ReaderControl.LayoutModes.FacingCover;
                    break;
            }
        }

        [ScriptableMember]
        public string GetToolMode()
        {
            return DocumentViewer.ToolMode.ToString();
        }

        [ScriptableMember]
        public void SetToolMode(string toolMode)
        {
            switch (toolMode.ToLower()) {
                case "pan":
                    DocumentViewer.ToolMode = Controls.DocumentViewer.ToolModes.Pan;
                    break;

                case "textselect":
                    DocumentViewer.ToolMode = Controls.DocumentViewer.ToolModes.TextSelect;
                    break;

                case "panandannotationedit":
                    DocumentViewer.ToolMode = Controls.DocumentViewer.ToolModes.PanAndAnnotationEdit;
                    break;

                case "annotationedit":
                    DocumentViewer.ToolMode = Controls.DocumentViewer.ToolModes.AnnotationEdit;
                    break;

                case "annotationcreatecustom":
                    DocumentViewer.ToolMode = Controls.DocumentViewer.ToolModes.AnnotationCreateCustom;
                    break;

                case "annotationcreateellipse":
                    DocumentViewer.ToolMode = Controls.DocumentViewer.ToolModes.AnnotationCreateEllipse;
                    break;

                case "annotationcreatefreehand":
                    DocumentViewer.ToolMode = Controls.DocumentViewer.ToolModes.AnnotationCreateFreeHand;
                    break;

                case "annotationcreateline":
                    DocumentViewer.ToolMode = Controls.DocumentViewer.ToolModes.AnnotationCreateLine;
                    break;

                case "annotationcreaterectangle":
                    DocumentViewer.ToolMode = Controls.DocumentViewer.ToolModes.AnnotationCreateRectangle;
                    break;

                case "annotationcreatesticky":
                    DocumentViewer.ToolMode = Controls.DocumentViewer.ToolModes.AnnotationCreateSticky;
                    break;

                case "annotationcreatetexthighlight":
                    DocumentViewer.ToolMode = Controls.DocumentViewer.ToolModes.AnnotationCreateTextHighlight;
                    break;

                case "annotationcreatetextstrikeout":
                    DocumentViewer.ToolMode = Controls.DocumentViewer.ToolModes.AnnotationCreateTextStrikeout;
                    break;

                case "annotationcreatetextunderline":
                    DocumentViewer.ToolMode = Controls.DocumentViewer.ToolModes.AnnotationCreateTextUnderline;
                    break;

                case "custom":
                    DocumentViewer.ToolMode = Controls.DocumentViewer.ToolModes.Custom;
                    break;
            }
        }

        [ScriptableMember]
        public string GetFitModeHeight()
        {
            return DocumentViewer.FitModeHeight.ToString();
        }

        [ScriptableMember]
        public string GetFitModeWidth()
        {
            return DocumentViewer.FitModeWidth.ToString();
        }

        private Controls.DocumentViewer.FitModes GetFitModeFromString(string fitMode)
        {
            switch (fitMode.ToLower()) {
                case "none":
                    return Controls.DocumentViewer.FitModes.None;

                case "page":
                    return Controls.DocumentViewer.FitModes.Page;

                case "panel":
                    return Controls.DocumentViewer.FitModes.Panel;
            }

            return Controls.DocumentViewer.FitModes.None;
        }

        [ScriptableMember]
        public void SetFitMode(string fitModeWidth, string fitModeHeight)
        {
            Controls.DocumentViewer.FitModes width = GetFitModeFromString(fitModeWidth);
            Controls.DocumentViewer.FitModes height = GetFitModeFromString(fitModeHeight);

            DocumentViewer.SetFitMode(width, height);
        }

        [ScriptableMember]
        public void SearchText(string pattern, string searchMode)
        {
            Documents.Text.TextSearch.SearchModes mode = Documents.Text.TextSearch.SearchModes.None;
            switch (searchMode.ToLower()) {
                case "none":
                    mode = Documents.Text.TextSearch.SearchModes.None;
                    break;

                case "casesensitive":
                    mode = Documents.Text.TextSearch.SearchModes.CaseSensitive;
                    break;

                case "wholeword":
                    mode = Documents.Text.TextSearch.SearchModes.WholeWord;
                    break;

                case "searchup":
                    mode = Documents.Text.TextSearch.SearchModes.SearchUp;
                    break;

                case "pagestop":
                    mode = Documents.Text.TextSearch.SearchModes.PageStop;
                    break;

                case "providequads":
                    mode = Documents.Text.TextSearch.SearchModes.ProvideQuads;
                    break;

                case "ambientstring":
                    mode = Documents.Text.TextSearch.SearchModes.AmbientString;
                    break;
            }

            DocumentViewer.SearchTextAsync(pattern, mode, null);
        }

        [ScriptableMember]
        public string OnPageChangeCallback { get; set; }

        [ScriptableMember]
        public string OnPageZoomCallback { get; set; }

        #endregion
    }
}
