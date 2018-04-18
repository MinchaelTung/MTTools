/**
* Copyright (c) 2001-2011 by PDFTron Systems Inc. All Rights Reserved.
* Consult legal.txt regarding legal and license information.
*
* Javadoc comments compatible with JsDoc Toolkit: http://code.google.com/p/jsdoc-toolkit/
*/

// helper functions
function getSilverlightHtml(a) { var b = "<div id='id" + (new Date).getTime() + "'>"; b += "<object data='data:application/x-silverlight-2,' type='application/x-silverlight-2' width='100%' height='100%'>"; b += "<param name='source' value='ReaderControlSample.xap' />"; b += "<param name='onError' value='onSilverlightError' />"; b += "<param name='background' value='white' />"; b += "<param name='minRuntimeVersion' value='4.0.50401.0' />"; b += "<param name='autoUpgrade' value='true' />"; b += "<param name='windowless' value='true' />"; b += "<param name='initParams' value='DocumentUri=" + a + "' />"; b += "<a href='http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0' style='text-decoration:none'>"; b += "<img src='http://go.microsoft.com/fwlink/?LinkId=161376' alt='Get Microsoft Silverlight' style='border-style:none' />"; b += "</a>"; b += "</object>"; b += "</div>"; return b }
if (!window.Silverlight) window.Silverlight = {}; Silverlight._silverlightCount = 0; Silverlight.__onSilverlightInstalledCalled = false; Silverlight.fwlinkRoot = "http://go2.microsoft.com/fwlink/?LinkID="; Silverlight.__installationEventFired = false; Silverlight.onGetSilverlight = null; Silverlight.onSilverlightInstalled = function () { window.location.reload(false) }; Silverlight.isInstalled = function (b) { if (b == undefined) b = null; var a = false, m = null; try { var i = null, j = false; if (window.ActiveXObject) try { i = new ActiveXObject("AgControl.AgControl"); if (b === null) a = true; else if (i.IsVersionSupported(b)) a = true; i = null } catch (l) { j = true } else j = true; if (j) { var k = navigator.plugins["Silverlight Plug-In"]; if (k) if (b === null) a = true; else { var h = k.description; if (h === "1.0.30226.2") h = "2.0.30226.2"; var c = h.split("."); while (c.length > 3) c.pop(); while (c.length < 4) c.push(0); var e = b.split("."); while (e.length > 4) e.pop(); var d, g, f = 0; do { d = parseInt(e[f]); g = parseInt(c[f]); f++ } while (f < e.length && d === g); if (d <= g && !isNaN(d)) a = true } } } catch (l) { a = false } return a }; Silverlight.WaitForInstallCompletion = function () { if (!Silverlight.isBrowserRestartRequired && Silverlight.onSilverlightInstalled) { try { navigator.plugins.refresh() } catch (a) { } if (Silverlight.isInstalled(null) && !Silverlight.__onSilverlightInstalledCalled) { Silverlight.onSilverlightInstalled(); Silverlight.__onSilverlightInstalledCalled = true } else setTimeout(Silverlight.WaitForInstallCompletion, 3e3) } }; Silverlight.__startup = function () { navigator.plugins.refresh(); Silverlight.isBrowserRestartRequired = Silverlight.isInstalled(null); if (!Silverlight.isBrowserRestartRequired) { Silverlight.WaitForInstallCompletion(); if (!Silverlight.__installationEventFired) { Silverlight.onInstallRequired(); Silverlight.__installationEventFired = true } } else if (window.navigator.mimeTypes) { var b = navigator.mimeTypes["application/x-silverlight-2"], c = navigator.mimeTypes["application/x-silverlight-2-b2"], d = navigator.mimeTypes["application/x-silverlight-2-b1"], a = d; if (c) a = c; if (!b && (d || c)) { if (!Silverlight.__installationEventFired) { Silverlight.onUpgradeRequired(); Silverlight.__installationEventFired = true } } else if (b && a) if (b.enabledPlugin && a.enabledPlugin) if (b.enabledPlugin.description != a.enabledPlugin.description) if (!Silverlight.__installationEventFired) { Silverlight.onRestartRequired(); Silverlight.__installationEventFired = true } } if (!Silverlight.disableAutoStartup) if (window.removeEventListener) window.removeEventListener("load", Silverlight.__startup, false); else window.detachEvent("onload", Silverlight.__startup) }; if (!Silverlight.disableAutoStartup) if (window.addEventListener) window.addEventListener("load", Silverlight.__startup, false); else window.attachEvent("onload", Silverlight.__startup); Silverlight.createObject = function (m, f, e, k, l, h, j) { var d = {}, a = k, c = l; d.version = a.version; a.source = m; d.alt = a.alt; if (h) a.initParams = h; if (a.isWindowless && !a.windowless) a.windowless = a.isWindowless; if (a.framerate && !a.maxFramerate) a.maxFramerate = a.framerate; if (e && !a.id) a.id = e; delete a.ignoreBrowserVer; delete a.inplaceInstallPrompt; delete a.version; delete a.isWindowless; delete a.framerate; delete a.data; delete a.src; delete a.alt; if (Silverlight.isInstalled(d.version)) { for (var b in c) if (c[b]) { if (b == "onLoad" && typeof c[b] == "function" && c[b].length != 1) { var i = c[b]; c[b] = function (a) { return i(document.getElementById(e), j, a) } } var g = Silverlight.__getHandlerName(c[b]); if (g != null) { a[b] = g; c[b] = null } else throw "typeof events." + b + " must be 'function' or 'string'"; } slPluginHTML = Silverlight.buildHTML(a) } else slPluginHTML = Silverlight.buildPromptHTML(d); if (f) f.innerHTML = slPluginHTML; else return slPluginHTML }; Silverlight.buildHTML = function (a) { var b = []; b.push('<object type="application/x-silverlight" data="data:application/x-silverlight,"'); if (a.id != null) b.push(' id="' + Silverlight.HtmlAttributeEncode(a.id) + '"'); if (a.width != null) b.push(' width="' + a.width + '"'); if (a.height != null) b.push(' height="' + a.height + '"'); b.push(" >"); delete a.id; delete a.width; delete a.height; for (var c in a) if (a[c]) b.push('<param name="' + Silverlight.HtmlAttributeEncode(c) + '" value="' + Silverlight.HtmlAttributeEncode(a[c]) + '" />'); b.push("</object>"); return b.join("") }; Silverlight.createObjectEx = function (b) { var a = b, c = Silverlight.createObject(a.source, a.parentElement, a.id, a.properties, a.events, a.initParams, a.context); if (a.parentElement == null) return c }; Silverlight.buildPromptHTML = function (b) { var a = "", d = Silverlight.fwlinkRoot, c = b.version; if (b.alt) a = b.alt; else { if (!c) c = ""; a = "<a href='javascript:Silverlight.getSilverlight(\"{1}\");' style='text-decoration: none;'><img src='{2}' alt='Get Microsoft Silverlight' style='border-style: none'/></a>"; a = a.replace("{1}", c); a = a.replace("{2}", d + "108181") } return a }; Silverlight.getSilverlight = function (e) { if (Silverlight.onGetSilverlight) Silverlight.onGetSilverlight(); var b = "", a = String(e).split("."); if (a.length > 1) { var c = parseInt(a[0]); if (isNaN(c) || c < 2) b = "1.0"; else b = a[0] + "." + a[1] } var d = ""; if (b.match(/^\d+\056\d+$/)) d = "&v=" + b; Silverlight.followFWLink("149156" + d) }; Silverlight.followFWLink = function (a) { top.location = Silverlight.fwlinkRoot + String(a) }; Silverlight.HtmlAttributeEncode = function (c) { var a, b = ""; if (c == null) return null; for (var d = 0; d < c.length; d++) { a = c.charCodeAt(d); if (a > 96 && a < 123 || a > 64 && a < 91 || a > 43 && a < 58 && a != 47 || a == 95) b = b + String.fromCharCode(a); else b = b + "&#" + a + ";" } return b }; Silverlight.default_error_handler = function (e, b) { var d, c = b.ErrorType; d = b.ErrorCode; var a = "\nSilverlight error message     \n"; a += "ErrorCode: " + d + "\n"; a += "ErrorType: " + c + "       \n"; a += "Message: " + b.ErrorMessage + "     \n"; if (c == "ParserError") { a += "XamlFile: " + b.xamlFile + "     \n"; a += "Line: " + b.lineNumber + "     \n"; a += "Position: " + b.charPosition + "     \n" } else if (c == "RuntimeError") { if (b.lineNumber != 0) { a += "Line: " + b.lineNumber + "     \n"; a += "Position: " + b.charPosition + "     \n" } a += "MethodName: " + b.methodName + "     \n" } alert(a) }; Silverlight.__cleanup = function () { for (var a = Silverlight._silverlightCount - 1; a >= 0; a--) window["__slEvent" + a] = null; Silverlight._silverlightCount = 0; if (window.removeEventListener) window.removeEventListener("unload", Silverlight.__cleanup, false); else window.detachEvent("onunload", Silverlight.__cleanup) }; Silverlight.__getHandlerName = function (b) { var a = ""; if (typeof b == "string") a = b; else if (typeof b == "function") { if (Silverlight._silverlightCount == 0) if (window.addEventListener) window.addEventListener("unload", Silverlight.__cleanup, false); else window.attachEvent("onunload", Silverlight.__cleanup); var c = Silverlight._silverlightCount++; a = "__slEvent" + c; window[a] = b } else a = null; return a }; Silverlight.onRequiredVersionAvailable = function () { }; Silverlight.onRestartRequired = function () { }; Silverlight.onUpgradeRequired = function () { }; Silverlight.onInstallRequired = function () { }; Silverlight.IsVersionAvailableOnError = function (d, a) { var b = false; try { if (a.ErrorCode == 8001 && !Silverlight.__installationEventFired) { Silverlight.onUpgradeRequired(); Silverlight.__installationEventFired = true } else if (a.ErrorCode == 8002 && !Silverlight.__installationEventFired) { Silverlight.onRestartRequired(); Silverlight.__installationEventFired = true } else if (a.ErrorCode == 5014 || a.ErrorCode == 2106) { if (Silverlight.__verifySilverlight2UpgradeSuccess(a.getHost())) b = true } else b = true } catch (c) { } return b }; Silverlight.IsVersionAvailableOnLoad = function (b) { var a = false; try { if (Silverlight.__verifySilverlight2UpgradeSuccess(b.getHost())) a = true } catch (c) { } return a }; Silverlight.__verifySilverlight2UpgradeSuccess = function (d) { var c = false, b = "4.0.50401", a = null; try { if (d.IsVersionSupported(b + ".99")) { a = Silverlight.onRequiredVersionAvailable; c = true } else if (d.IsVersionSupported(b + ".0")) a = Silverlight.onRestartRequired; else a = Silverlight.onUpgradeRequired; if (a && !Silverlight.__installationEventFired) { a(); Silverlight.__installationEventFired = true } } catch (e) { } return c }
function TestSilverlight(v) { try { if (Silverlight.isInstalled(v)) return true } catch (e) { } return false }
function TestHTML5() { if (CanvasRenderingContext2D) return true; return false }
function IsMobileDetected(){return(navigator.userAgent.match(/Android/i)||navigator.userAgent.match(/webOS/i)||navigator.userAgent.match(/iPhone/i)||navigator.userAgent.match(/iPod/i)||navigator.userAgent.match(/iPad/i))}

/**
* Constructor. Creates a ReaderControl instance and embeds it on the HTML page. This constructor must be invoked on the
* section of the page where the ReaderControl must be rendered (i.e. within <body><div> tag).
*
* @class Represents a ReaderControl which is a viewer built using either Silverlight or HTML5 technologies.
*
* @param id                the id of the ReaderControl to create. Each ReaderControl instance must have its own unique id
* @param type              the type of ReaderControl to load. Values must be comma-separated in order of preferred ReaderControl. I.E. "html5,silverlight"
* @param options           options passed to the specific ReaderControl. Options must have the following:
*                          <pre>
*                          {
*                              html5 : "url/to/html5/ReaderControl.html",
*                              silverlight : "url/to/silverlight/ReaderControlSample.xap",
*                              showSilverlightControls : true  // indicates whether to show the controls on the Silverlight ReaderControl.
*                          }
*                          </pre>
* @param onLoadCallback    the name of the callback (in string) to invoke after the control is loaded.
*/
function ReaderControl(id, type, options, onLoadCallback) {
    // detect the ordering of the available readers
    if (!type) {
        type = 'html5,silverlight'; // default ordering
    }
    
    // if mobile, only the html5 is allowed
    if (IsMobileDetected()) {
        type = 'html5';
    }

    viewers = type.split(",");
    if (viewers.length < 1) viewers[0] = "html5";
    for (var i = 0; i < viewers.length; i++) {
        if (viewers[i] === "html5") {
            if (TestHTML5()) {
                this.selectedType = "html5";
                break;
            }
        }
        else if (viewers[i] === "silverlight") {
            if (TestSilverlight("4.0")) {
                this.selectedType = "silverlight";
                break;
            }
        }
        else {
            // unsupported
            this.selectedType = "none";
        }
    }

    this.rcId = id;

    if (this.selectedType === "html5") {
        // draw HTML5 reader control in an iframe
        document.write("<iframe id='" + this.rcId + "' src='" + options.html5 + "' frameborder='0' width='100%'></iframe>");
        // height has to be programmatically
        document.getElementById(this.rcId).style.height = "99%";

        // execute the callback
        if (!onLoadCallback && onLoadCallback !== undefined && onLoadCallback != null && callbackOnLoad !== "")
            window[onLoadCallback]();
    }
    else if (this.selectedType === "silverlight") {
        document.write(
            "<object id='" + this.rcId + "' data='data:application/x-silverlight-2,' type='application/x-silverlight-2' width='100%' height='100%'>" +
                "<param name='source' value='" + options.silverlight + "'/>" +
                "<param name='onError' value='onSilverlightError' />" +
                "<param name='background' value='white' />" +
                "<param name='minRuntimeVersion' value='4.0.50401.0' />" +
                "<param name='autoUpgrade' value='true' />" +
                (onLoadCallback != null ? "<param name='onLoad' value='" + onLoadCallback + "' />" : "") +
                "<param name='initParams' value='UseJavaScript=" + (options.showSilverlightControls ? "false" : "true") + "' />" +
                "<a href='http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0' style='text-decoration:none'>" +
                    "<img src='http://go.microsoft.com/fwlink/?LinkId=161376' alt='Get Microsoft Silverlight' style='border-style:none'/>" +
                "</a>" +
            "</object>" +
            "<iframe id='_sl_historyFrame' style='visibility:hidden;height:0px;width:0px;border:0px'></iframe>"
        );
    }
    else {
        // no runtimes supported
        document.write("<div>No runtimes supported!</div>");
        this.rcId = null;
        this.selectedType = null;
    }
}

/**
* Gets the instance representing the ReaderControl created.
*
* @return the instance of the ReaderControl
*/
ReaderControl.prototype.getInstance = function () {
    if (this.instance == null) {
        if (this.selectedType === "html5") {
            // get the iframe holding the ReaderControl
            this.instance = document.getElementById(this.rcId).contentWindow.readerControl;
        }
        else if (this.selectedType === "silverlight") {
            // return the Silverlight ReaderControl
            this.instance = document.getElementById(this.rcId).Content.ReaderControl;
        }
        else {
            // return nothing
            this.instance = null;
        }
    }

    return (this.instance);
}
/**
* Loads a document to the ReaderControl.
*
* @param url               url of the document to be loaded
* @param callbackOnLoad    JavaScript function to invoke after the file is loaded
*/
ReaderControl.prototype.loadDocument = function (url, callbackOnLoad) {
    if (this.selectedType === "html5") {
        this.getInstance().LoadDocument(url);
        // execute the callback
        if (!callbackOnLoad && callbackOnLoad !== undefined && callbackOnLoad != null && callbackOnLoad !== "")
            window[callbackOnLoad]();
    }
    else if (this.selectedType === "silverlight") {
        this.getInstance().LoadDocument(url, callbackOnLoad);
    }
}

/**
* Gets the value whether the side window is visible or not.
*
* @return true if the side window is shown
*/
ReaderControl.prototype.getShowSideWindow = function () {
    if (this.selectedType === "html5") {
        return (this.getInstance().GetShowSideWindow());
    }
    else if (this.selectedType === "silverlight") {
        return (this.getInstance().GetShowSideWindow());
    }
}

/**
* Sets the value whether the side window is visible or not.
*
* @param value true to show the side window
*/
ReaderControl.prototype.setShowSideWindow = function (value) {
    if (this.selectedType === "html5") {
        this.getInstance().SetShowSideWindow(value);
    }
    else if (this.selectedType === "silverlight") {
        this.getInstance().SetShowSideWindow(value);
    }
}

/**
* Gets the current page number of the document loaded in the ReaderControl.
*
* @return the current page number of the document
*/
ReaderControl.prototype.getCurrentPageNumber = function () {
    if (this.selectedType === "html5") {
        return (this.getInstance().docViewer.GetCurrentPage());
    }
    else if (this.selectedType === "silverlight") {
        return (this.getInstance().CurrentPageNumber);
    }
}

/**
* Sets the current page number of the document loaded in the ReaderControl.
*
* @param pageNumber    the page number of the document to set
*/
ReaderControl.prototype.setCurrentPageNumber = function (pageNumber) {
    if (this.selectedType === "html5") {
        this.getInstance().docViewer.SetCurrentPage(pageNumber);
    }
    else if (this.selectedType === "silverlight") {
        this.getInstance().CurrentPageNumber = pageNumber;
    }
}

/**
* Gets the total number of pages of the loaded document.
*
* @return the total number of pages of the loaded document
*/
ReaderControl.prototype.getPageCount = function () {
    if (this.selectedType === "html5") {
        return (this.getInstance().docViewer.GetPageCount());
    }
    else if (this.selectedType === "silverlight") {
        return (this.getInstance().PageCount);
    }
}

/**
* Gets the zoom level of the document.
*
* @return the zoom level of the document
*/
ReaderControl.prototype.getZoomLevel = function () {
    if (this.selectedType === "html5") {
        return (this.getInstance().docViewer.GetZoom());
    }
    else if (this.selectedType === "silverlight") {
        return (this.getInstance().ZoomLevel);
    }
}

/**
* Sets the zoom level of the document.
*
* @param zoomLevel the new zoom level to set
*/
ReaderControl.prototype.setZoomLevel = function (zoomLevel) {
    if (this.selectedType === "html5") {
        this.getInstance().docViewer.ZoomTo(zoomLevel);
    }
    else if (this.selectedType === "silverlight") {
        this.getInstance().ZoomLevel = zoomLevel;
    }
}

/**
* Rotates the document in the ReaderControl clockwise.
*/
ReaderControl.prototype.rotateClockwise = function () {
    if (this.selectedType === "html5") {
        this.getInstance().docViewer.RotateClockwise();
    }
    else if (this.selectedType === "silverlight") {
        this.getInstance().RotateClockwise();
    }
}

/**
* Rotates the document in the ReaderControl counter-clockwise.
*/
ReaderControl.prototype.rotateCounterClockwise = function () {
    if (this.selectedType === "html5") {
        this.getInstance().docViewer.RotateCounterClockwise();
    }
    else if (this.selectedType === "silverlight") {
        this.getInstance().RotateCounterClockwise();
    }
}

/**
* Lists the possible modes for displaying pages.
*
* @name ReaderControl.LayoutMode
* @namespace
*/
ReaderControl.LayoutMode =
{
    /**
    * Only the current page will be visible.
    *
    * @name ReaderControl.LayoutMode.Single
    */
    Single: "SinglePage",

    /**
    * All pages are visible in one column.
    *
    * @name ReaderControl.LayoutMode.Continuous
    */
    Continuous: "Continuous",

    /**
    * Up to two pages will be visible, with an odd numbered page rendered first.
    *
    * @name ReaderControl.LayoutMode.Facing
    */
    Facing: "Facing",

    /**
    * All pages visible in two columns.
    *
    * @name ReaderControl.LayoutMode.FacingContinuous
    */
    FacingContinuous: "FacingContinuous",

    /**
    * All pages visible, with an even numbered page rendered first.
    *
    * @name ReaderControl.LayoutMode.FacingCover
    */
    FacingCover: "FacingCover",

    /**
    * 
    *
    * @name ReaderControl.LayoutMode.FacingCoverContinuous
    */
    FacingCoverContinuous: "CoverContinuous"
}

/**
* Gets the layout mode of the document in the ReaderControl.
*
* @returns the layout mode of the document
*/
ReaderControl.prototype.getLayoutMode = function () {
    if (this.selectedType === "html5") {
        // the HTML5 viewer have different naming schemes for this
        var layoutMode = this.getInstance().docViewer.GetDisplayMode();
        var displayModes = this.getInstance().docViewer.DisplayMode;
        if (layoutMode === displayModes.Single)
            return (ReaderControl.LayoutMode.Single);
        else if (layoutMode === displayModes.Continuous)
            return (ReaderControl.LayoutMode.Continuous);
        else if (layoutMode === displayModes.Facing)
            return (ReaderControl.LayoutMode.Facing);
        else if (layoutMode === displayModes.FacingContinuous)
            return (ReaderControl.LayoutMode.FacingContinuous);
        else if (layoutMode === displayModes.Cover)
            return (ReaderControl.LayoutMode.FacingCover);
        else if (layoutMode === displayModes.CoverContinuous)
            return (ReaderControl.LayoutMode.FacingCoverContinuous);
        else
            return (undefined);
    }
    else if (this.selectedType === "silverlight") {
        return (this.getInstance().GetLayoutMode());
    }
}

/**
* Sets the layout mode of the document in the ReaderControl.
*
* @param layout    the layout mode to set (see ReaderControl.LayoutMode)
*/
ReaderControl.prototype.setLayoutMode = function (layoutMode) {
    if (this.selectedType === "html5") {
        // the HTML5 viewer have different naming schemes for this
        var displayModes = this.getInstance().docViewer.DisplayMode;

        var displayMode = displayModes.Continuous;

        if (layoutMode === ReaderControl.LayoutMode.Single)
            displayMode = displayModes.Single;
        else if (layoutMode === ReaderControl.LayoutMode.Continuous)
            displayMode = displayModes.Continuous;
        else if (layoutMode === ReaderControl.LayoutMode.Facing)
            displayMode = displayModes.Facing;
        else if (layoutMode === ReaderControl.LayoutMode.FacingContinuous)
            displayMode = displayModes.FacingContinuous;
        else if (layoutMode === ReaderControl.LayoutMode.FacingCover)
            displayMode = displayModes.Cover;
        else if (layoutMode === ReaderControl.LayoutMode.FacingCoverContinuous)
            displayMode = displayModes.CoverContinuous;

        this.getInstance().docViewer.SetDisplayMode(displayMode);
    }
    else if (this.selectedType === "silverlight") {
        this.getInstance().SetLayoutMode(layoutMode);
    }
}


/**
* Lists the possible modes for the tool control.
*
* @name ReaderControl.ToolMode
* @namespace
*/
ReaderControl.ToolMode =
{
    Pan: "Pan",
    TextSelect: "TextSelect",
    PanAndAnnotationEdit: "PanAndAnnotationEdit",
    AnnotationEdit: "AnnotationEdit",
    AnnotationCreateCustom: "AnnotationCreateCustom",
    AnnotationCreateEllipse: "AnnotationCreateEllipse",
    AnnotationCreateFreeHand: "AnnotationCreateFreeHand",
    AnnotationCreateLine: "AnnotationCreateLine",
    AnnotationCreateRectangle: "AnnotationCreateRectangle",
    AnnotationCreateSticky: "AnnotationCreateSticky",
    AnnotationCreateTextHighlight: "AnnotationCreateTextHighlight",
    AnnotationCreateTextStrikeout: "AnnotationCreateTextStrikeout",
    AnnotationCreateTextUnderline: "AnnotationCreateTextUnderline"
}

/**
* Gets the current tool mode of the ReaderControl.
*
* @returns the current tool mode of the ReaderControl
*/
ReaderControl.prototype.getToolMode = function () {
    if (this.selectedType === "html5") {
        // Currently, there are only 2 tool modes for HTML5
        var toolMode = this.getInstance().docViewer.GetToolMode();
        var toolModes = this.getInstance().docViewer.ToolModes;

        if (toolMode === toolModes.Pan)
            return (ReaderControl.ToolMode.Pan);
        else if (toolMode === toolModes.TextSelect)
            return (ReaderControl.ToolMode.TextSelect);
        else
            return (undefined);
    }
    else if (this.selectedType === "silverlight") {
        return (this.getInstance().GetToolMode());
    }
}

/**
* Sets the tool mode of the ReaderControl.
*
* @param toolMode  must be one of the above tool modes
*/
ReaderControl.prototype.setToolMode = function (toolMode) {
    if (this.selectedType === "html5") {
        // Currently, there are only 2 tool modes for HTML5
        var toolModes = this.getInstance().docViewer.ToolModes;
        var modeToSet = toolModes.Pan;

        if (toolMode === ReaderControl.ToolMode.Pan)
            modeToSet = toolModes.Pan;
        else if (toolMode === ReaderControl.ToolMode.TextSelect)
            modeToSet = toolModes.TextSelect;
        // TODO: Add support for other tool modes

        this.getInstance().docViewer.SetToolMode(modeToSet);
    }
    else if (this.selectedType === "silverlight") {
        this.getInstance().SetToolMode(toolMode);
    }
}

/**
* Controls if the document's Zoom property will be adjusted so that the width of the current page or panel
* will exactly fit into the available space. 
*/
ReaderControl.prototype.fitWidth = function () {
    if (this.selectedType === "html5") {
        this.getInstance().fitWidth();
    }
    else if (this.selectedType === "silverlight") {
        this.getInstance().FitWidth();
    }
}

/**
*/
ReaderControl.prototype.fitHeight = function () {
    if (this.selectedType === "html5") {
        // unsupported
    }
    else if (this.selectedType === "silverlight") {
        this.getInstance().FitHeight();
    }
}

/**
* Controls if the document's Zoom property will be adjusted so that the width and height of the current page or panel
* will fit into the available space.
*/
ReaderControl.prototype.fitPage = function () {
    if (this.selectedType === "html5") {
        this.getInstance().fitPage();
    }
    else if (this.selectedType === "silverlight") {
        this.getInstance().FitPage();
    }
}

/**
*/
ReaderControl.prototype.zoom = function () {
    if (this.selectedType === "html5") {
        this.getInstance().zoom();
    }
    else if (this.selectedType === "silverlight") {
        this.getInstance().Zoom();
    }
}

/**
* Goes to the first page of the document. Makes the document viewer display the first page of the document.
*/
ReaderControl.prototype.goToFirstPage = function () {
    if (this.selectedType === "html5") {
        this.getInstance().docViewer.DisplayFirstPage();
    }
    else if (this.selectedType === "silverlight") {
        this.getInstance().CurrentPageNumber = 1;
    }
}

/**
* Goes to the last page of the document. Makes the document viewer display the last page of the document.
*/
ReaderControl.prototype.goToLastPage = function () {
    if (this.selectedType === "html5") {
        this.getInstance().docViewer.DisplayLastPage();
    }
    else if (this.selectedType === "silverlight") {
        var totalPages = this.getInstance().PageCount;
        this.getInstance().CurrentPageNumber = totalPages;
    }
}

/**
* Goes to the next page of the document. Makes the document viewer display the next page of the document.
*/
ReaderControl.prototype.goToNextPage = function () {
    var currentPage;
    if (this.selectedType === "html5") {
        currentPage = this.getInstance().docViewer.GetCurrentPage();

        if (currentPage <= 0)
            return;

        currentPage = currentPage + 1;
        this.getInstance().docViewer.SetCurrentPage(currentPage);
    }
    else if (this.selectedType === "silverlight") {
        currentPage = this.getInstance().CurrentPageNumber;

        if (currentPage <= 0)
            return;

        currentPage = currentPage + 1;
        this.getInstance().CurrentPageNumber = currentPage;
    }
}

/**
* Goes to the previous page of the document. Makes the document viewer display the previous page of the document.
*/
ReaderControl.prototype.goToPrevPage = function () {
    var currentPage;
    if (this.selectedType === "html5") {
        currentPage = this.getInstance().docViewer.GetCurrentPage();

        if (currentPage <= 1)
            return;

        currentPage = currentPage - 1;
        this.getInstance().docViewer.SetCurrentPage(currentPage);
    }
    else if (this.selectedType === "silverlight") {
        currentPage = this.getInstance().CurrentPageNumber;

        if (currentPage <= 1)
            return;

        currentPage = currentPage - 1;
        this.getInstance().CurrentPageNumber = currentPage;
    }
}

/**
* Searches the loaded document finding for the matching pattern.
*
* Search mode includes:
* <ul>
* <li>None</li>
* <li>CaseSensitive</li>
* <li>WholeWord</li>
* <li>SearchUp</li>
* <li>PageStop</li>
* <li>ProvideQuads</li>
* <li>AmbientString</li>
* </ul>
*
* @param pattern       the pattern to look for
* @param searchMode    must one or a combination of the above search modes. To
*                      combine search modes, simply pass them as comma separated
*                      values in one string. i.e. "CaseSensitive,WholeWord"
*/
ReaderControl.prototype.searchText = function (pattern, searchMode) {
    if (this.selectedType === "html5") {
        // string split with comma
        if (!searchMode) searchMode = "None";
        modes = searchMode.split(",");
        if (modes.length < 1) modes[0] = "None";
        modeToSet = new Object();
        for (var i = 0; i < modes.length; i++) {
            if (modes === "None") {
                modeToSet.None = true;
            }
            else if (modes === "CaseSensitive") {
                modeToSet.CaseSensitive = true;
            }
            else if (modes === "WholeWord") {
                modeToSet.WholeWord = true;
            }
        }

        this.getInstance().SetSearchModes(modeToSet);
        this.getInstance().fullTextSearch(pattern);
    }
    else if (this.selectedType === "silverlight") {
        this.getInstance().SearchText(pattern, searchMode);
    }
}

/**
* Registers a callback when the document's page number is changed.
*
* @param callback  the JavaScript function to invoke when the document page number is changed
*/
ReaderControl.prototype.setOnPageChangeCallback = function (callback) {
    if (this.selectedType === "html5") {
        alert("Not yet supported");
    }
    else if (this.selectedType === "silverlight") {
        this.getInstance().OnPageChangeCallback = callback;
    }
}

/**
* Registers a callback when the document's zoom level is changed.
*
* @param callback  the JavaScript function to invoke when the document zoom level is changed
*/
ReaderControl.prototype.setOnPageZoomCallback = function (callback) {
    if (this.selectedType === "html5") {
        alert("Not yet supported");
    }
    else if (this.selectedType === "silverlight") {
        this.getInstance().OnPageZoomCallback = callback;
    }
}
