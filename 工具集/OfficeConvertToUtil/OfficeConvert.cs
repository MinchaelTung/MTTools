using System;
using System.Collections.Generic;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Word = Microsoft.Office.Interop.Word;
using Visio = Microsoft.Office.Interop.Visio;

namespace MTFramework.OfficeConvertToUtil
{
    /// <summary>
    /// Office 文档转换工具
    /// </summary>
    public class OfficeConvert
    {
        #region --- 私有成员 Begin ---

        /// <summary>
        /// Word文件类型
        /// </summary>
        private static List<string> _WordFileExtensions = new List<string>() { ".doc", ".docx" };
        /// <summary>
        /// Excel文件类型
        /// </summary>
        private static List<string> _ExcelFileExtensions = new List<string>() { ".xls", ".xlsx" };
        /// <summary>
        /// PowerPoint文件类型
        /// </summary>
        private static List<string> _PowerPointFileExtensions = new List<string>() { ".ppt", ".pptx" };

        private static string _VisioFileExtensions = ".vsd";

        #endregion --- 私有成员 End ---

        #region --- 公开调用方法 Begin ---

        /// <summary>
        /// Office 文档转换
        /// </summary>
        /// <param name="sourceFilePath">文档原路径</param>
        /// <param name="targetFilePath">新文档目标路径</param>
        /// <param name="convertToType">需要转换的新文档类型</param>
        /// <returns>执行结果</returns>
        public static OfficeConvertResultInfo OfficeConvertTo(string sourceFilePath, string targetFilePath, FileType convertToType = FileType.Auto)
        {
            int validResult = onIsValidTargetFilePath(targetFilePath);
            switch (validResult)
            {
                case 2:
                case 3:
                case 4:
                case 5:
                    break;
                case -1:
                    return new OfficeConvertResultInfo(OfficeConvertResult.InvalidFileType, "目标文件扩展名不正确");
                default:
                    return new OfficeConvertResultInfo(OfficeConvertResult.InvalidPath, "来源不正确");
            }

            switch (convertToType)
            {
                case FileType.Auto:
                    convertToType = (FileType)validResult;
                    break;
                case FileType.Xps:
                    if (validResult != 2)
                    {
                        return new OfficeConvertResultInfo(OfficeConvertResult.InvalidFileType, "目标文件类型不正确，必须是XPS扩展名");
                    }
                    break;
                case FileType.Pdf:
                    if (validResult != 3)
                    {
                        return new OfficeConvertResultInfo(OfficeConvertResult.InvalidFileType, "目标文件类型不正确，必须是PDF扩展名");
                    }
                    break;
                case FileType.Html:
                    if (validResult != 4)
                    {
                        return new OfficeConvertResultInfo(OfficeConvertResult.InvalidFileType, "目标文件类型不正确，必须是HTML扩展名");
                    }
                    break;
                case FileType.Mht:
                    if (validResult != 5)
                    {
                        return new OfficeConvertResultInfo(OfficeConvertResult.InvalidFileType, "目标文件类型不正确，必须是MHT扩展名");
                    }
                    break;
                default:
                    return new OfficeConvertResultInfo(OfficeConvertResult.InvalidPath, "来源不正确");
            }
            int isValidSourceFilePathResult = onIsValidSourceFilePath(sourceFilePath);

            switch (isValidSourceFilePathResult)
            {
                case 1:
                    return OfficeConvert.word_Convert(sourceFilePath, targetFilePath, convertToType);
                case 2:
                    return OfficeConvert.excel_Convert(sourceFilePath, targetFilePath, convertToType);
                case 3:
                    return OfficeConvert.powerPoint_Convert(sourceFilePath, targetFilePath, convertToType);
                case 4:
                    if (convertToType == FileType.Mht)
                    {
                        convertToType = FileType.Html;
                        targetFilePath = targetFilePath.Replace(".", "") + ".html";
                    }
                    return OfficeConvert.visio_Convert(sourceFilePath, targetFilePath, convertToType);
                case 0:
                    return new OfficeConvertResultInfo(OfficeConvertResult.InvalidFileType, "来源文件类型不正确");
                case -2:
                    return new OfficeConvertResultInfo(OfficeConvertResult.InvalidPath, "来源文件不存在");
                default:
                    return new OfficeConvertResultInfo(OfficeConvertResult.InvalidPath);
            }
        }

        #endregion --- 公开调用方法 End ---

        #region --- 私有辅助方法 Begin ---

        /// <summary>
        /// 检查目标文件路径
        /// </summary>
        /// <param name="targetFilePath">目标路径</param>
        /// <returns>-2=来源文件路径不能为空，-1=文件类型不正确，2=xps文件，3=pdf文件</returns>
        private static int onIsValidTargetFilePath(string targetFilePath)
        {

#if(Net45)
            if (string.IsNullOrWhiteSpace(targetFilePath) == true)
            {
                return -2;
            }
#elif(Net40)
            if (string.IsNullOrWhiteSpace(targetFilePath) == true)
            {
                return -2;
            }
#else
            if (string.IsNullOrEmpty(targetFilePath.Trim()) == true)
            {
                return -2;
            }
#endif
            string type = targetFilePath.Substring(targetFilePath.LastIndexOf(".")).ToLower();
            switch (type)
            {
                case ".xps":
                    return 2;
                case ".pdf":
                    return 3;
                case ".htm":
                case ".html":
                    return 4;
                case ".mht":
                case ".mhtml":
                    return 5;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// 检查来源文件
        /// </summary>
        /// <param name="sourceFilePath">来源文件路径</param>
        /// <returns>-2=文件不存在, -1=来源文件路径不能为空，0=文件类型不存在，1=Word 文档,2=Excel 文档,3=PowerPoint 文档</returns>
        private static int onIsValidSourceFilePath(string sourceFilePath)
        {
#if(Net45)
            if (string.IsNullOrWhiteSpace(sourceFilePath) == true)
            {
                return -1;
            }
#elif(Net40)
            if (string.IsNullOrWhiteSpace(sourceFilePath) == true)
            {
                return -1;
            }
#else
            if (string.IsNullOrEmpty(sourceFilePath.Trim()) == true)
            {
                return -1;
            }
#endif

            if (File.Exists(sourceFilePath) == false)
            {
                return -2;
            }

            string type = sourceFilePath.Substring(sourceFilePath.LastIndexOf(".")).ToLower();

            if (_WordFileExtensions.Contains(type) == true)
            {
                return 1;
            }
            else if (_ExcelFileExtensions.Contains(type) == true)
            {
                return 2;
            }
            else if (_PowerPointFileExtensions.Contains(type) == true)
            {
                return 3;
            }
            else if (_VisioFileExtensions.Equals(type) == true)
            {
                return 4;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Word 转换
        /// </summary>
        /// <param name="sourceFilePath">来源文件路径</param>
        /// <param name="targetFilePath">目标路径</param>
        /// <param name="fileType">转换类型</param>
        /// <returns>执行结果</returns>
        private static OfficeConvertResultInfo word_Convert(string sourceFilePath, string targetFilePath, FileType fileType)
        {
            Word.Application wordApp = null;
            Word.Document wordDocument = null;
            object source = sourceFilePath;
            string target = targetFilePath;
            try
            {
                try
                {
                    wordApp = new Word.Application();
                }
                catch (Exception ex1)
                {
                    return new OfficeConvertResultInfo(OfficeConvertResult.InitializeOfficeAppError, "Word", ex1);
                }

                try
                {
                    wordDocument = wordApp.Documents.Open(ref source);
                }
                catch (Exception ex2)
                {
                    return new OfficeConvertResultInfo(OfficeConvertResult.OpenOfficeFileError, ex2.Message, ex2);
                }

                if (wordDocument == null)
                {
                    return new OfficeConvertResultInfo(OfficeConvertResult.OpenOfficeFileError, "未能打开源文件");
                }
                try
                {
                    object saveasfilepath = targetFilePath;
                    object wdFormat = null;
                    switch (fileType)
                    {
                        case FileType.Pdf:

                            wdFormat = Word.WdSaveFormat.wdFormatPDF;
                            //wordDocument.ExportAsFixedFormat(
                            //    OutputFileName: targetFilePath
                            //    , ExportFormat: Word.WdExportFormat.wdExportFormatPDF
                            //    , OptimizeFor: Word.WdExportOptimizeFor.wdExportOptimizeForOnScreen
                            //    , From: 0
                            //    , To: 0
                            //    , IncludeDocProps: true
                            //    );
                            break;
                        case FileType.Xps:
                            wdFormat = Word.WdSaveFormat.wdFormatXPS;
                            //wordDocument.ExportAsFixedFormat(
                            //    OutputFileName: targetFilePath
                            //    , ExportFormat: Word.WdExportFormat.wdExportFormatXPS
                            //    , OptimizeFor: Word.WdExportOptimizeFor.wdExportOptimizeForOnScreen
                            //    , From: 0
                            //    , To: 0
                            //    , IncludeDocProps: true
                            //    );
                            break;
                        case FileType.Html:
                            wdFormat = Word.WdSaveFormat.wdFormatHTML;
                            break;
                        case FileType.Mht:
                            wdFormat = Word.WdSaveFormat.wdFormatWebArchive;
                            break;
                        default:
                            return new OfficeConvertResultInfo(OfficeConvertResult.InvalidFileType, targetFilePath);
                    }

                    wordDocument.SaveAs2(FileName: ref saveasfilepath, FileFormat: ref wdFormat);
                }
                catch (Exception ex3)
                {
                    switch (fileType)
                    {
                        case FileType.Xps:
                            return new OfficeConvertResultInfo(OfficeConvertResult.ExportToXpsError, "Word", ex3);
                        case FileType.Pdf:
                            return new OfficeConvertResultInfo(OfficeConvertResult.ExportToPdfError, "Word", ex3);
                        default:
                            return new OfficeConvertResultInfo(OfficeConvertResult.ExportToError, "Word", ex3);
                    }
                }
                return new OfficeConvertResultInfo(OfficeConvertResult.Success, targetFilePath);
            }
            catch (Exception exc)
            {
                return new OfficeConvertResultInfo(OfficeConvertResult.OfficeInteropError, "Word", exc);
            }
            finally
            {
                if (wordDocument != null)
                {
                    wordDocument.Close();
                    wordDocument = null;
                }
                if (wordApp != null)
                {
                    wordApp.Quit();
                    wordApp = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

        }

        /// <summary>
        /// PowerPoint 转换
        /// </summary>
        /// <param name="sourceFilePath">来源文件路径</param>
        /// <param name="targetFilePath">目标路径</param>
        /// <param name="fileType">转换类型</param>
        /// <returns>执行结果</returns>
        private static OfficeConvertResultInfo powerPoint_Convert(string sourceFilePath, string targetFilePath, FileType fileType)
        {
            PowerPoint.Application pptApp = null;
            PowerPoint.Presentation pptPresentation = null;
            try
            {
                try
                {
                    pptApp = new PowerPoint.Application();
                }
                catch (Exception ex1)
                {
                    return new OfficeConvertResultInfo(OfficeConvertResult.InitializeOfficeAppError, "PowerPoint", ex1);
                }

                try
                {
                    pptPresentation = pptApp.Presentations.Open(
                          FileName: sourceFilePath
                        , ReadOnly: Microsoft.Office.Core.MsoTriState.msoTrue
                        , Untitled: Microsoft.Office.Core.MsoTriState.msoTrue
                        , WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse
                        );
                }
                catch (Exception ex2)
                {
                    return new OfficeConvertResultInfo(OfficeConvertResult.OpenOfficeFileError, ex2.Message, ex2);
                }

                if (pptPresentation == null)
                {
                    return new OfficeConvertResultInfo(OfficeConvertResult.OpenOfficeFileError);
                }
                try
                {
                    switch (fileType)
                    {
                        case FileType.Pdf:
                            pptPresentation.SaveAs(FileName: targetFilePath, FileFormat: PowerPoint.PpSaveAsFileType.ppSaveAsPDF);
                            //pptPresentation.ExportAsFixedFormat(
                            //    Path: targetFilePath
                            //    , FixedFormatType: PowerPoint.PpFixedFormatType.ppFixedFormatTypePDF
                            //    , IncludeDocProperties: true
                            //    );
                            break;
                        case FileType.Xps:
                            //pptPresentation.ExportAsFixedFormat(
                            //   Path: targetFilePath
                            //   , FixedFormatType: PowerPoint.PpFixedFormatType.ppFixedFormatTypeXPS
                            //   , IncludeDocProperties: true);
                            pptPresentation.SaveAs(FileName: targetFilePath, FileFormat: PowerPoint.PpSaveAsFileType.ppSaveAsXPS);
                            break;
                        case FileType.Html:
                            pptPresentation.SaveAs(FileName: targetFilePath, FileFormat: PowerPoint.PpSaveAsFileType.ppSaveAsHTML);
                            break;
                        case FileType.Mht:
                            pptPresentation.SaveAs(FileName: targetFilePath, FileFormat: PowerPoint.PpSaveAsFileType.ppSaveAsWebArchive);
                            break;
                        default:
                            return new OfficeConvertResultInfo(OfficeConvertResult.InvalidFileType, targetFilePath);
                    }
                }
                catch (Exception ex3)
                {
                    switch (fileType)
                    {
                        case FileType.Xps:
                            return new OfficeConvertResultInfo(OfficeConvertResult.ExportToXpsError, "PowerPoint", ex3);
                        case FileType.Pdf:
                            return new OfficeConvertResultInfo(OfficeConvertResult.ExportToPdfError, "PowerPoint", ex3);
                        default:
                            return new OfficeConvertResultInfo(OfficeConvertResult.ExportToError, "PowerPoint", ex3);
                    }
                }
                return new OfficeConvertResultInfo(OfficeConvertResult.Success, targetFilePath);
            }
            catch (Exception exc)
            {
                return new OfficeConvertResultInfo(OfficeConvertResult.OfficeInteropError, "PowerPoint", exc);
            }
            finally
            {
                if (pptPresentation != null)
                {
                    pptPresentation.Close();
                    pptPresentation = null;
                }
                if (pptApp != null)
                {
                    pptApp.Quit();
                    pptApp = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

        }

        /// <summary>
        /// Excel 转换
        /// </summary>
        /// <param name="sourceFilePath">来源文件路径</param>
        /// <param name="targetFilePath">目标路径</param>
        /// <param name="fileType">转换类型</param>
        /// <returns>执行结果</returns>
        private static OfficeConvertResultInfo excel_Convert(string sourceFilePath, string targetFilePath, FileType fileType)
        {
            Excel.Application excelApp = null;
            Excel.Workbook excelWorkbook = null;
            try
            {
                try
                {
                    excelApp = new Excel.Application();
                }
                catch (Exception ex1)
                {
                    return new OfficeConvertResultInfo(OfficeConvertResult.InitializeOfficeAppError, "Excel", ex1);
                }

                try
                {
                    excelWorkbook = excelApp.Workbooks.Open(sourceFilePath);
                }
                catch (Exception ex2)
                {
                    return new OfficeConvertResultInfo(OfficeConvertResult.OpenOfficeFileError, ex2.Message, ex2);
                }
                if (excelWorkbook == null)
                {
                    return new OfficeConvertResultInfo(OfficeConvertResult.OpenOfficeFileError);
                }

                try
                {
                    //excelWorkbook.SaveAs(Filename: targetFilePath, FileFormat: Excel.XlFileFormat.xl, AccessMode: Excel.XlSaveAsAccessMode.xlExclusive);
                    switch (fileType)
                    {
                        case FileType.Pdf:
                            excelWorkbook.ExportAsFixedFormat(
                                 Type: Excel.XlFixedFormatType.xlTypePDF
                                , Filename: targetFilePath
                                , Quality: Excel.XlFixedFormatQuality.xlQualityStandard
                                , IncludeDocProperties: true
                                , IgnorePrintAreas: true
                                , OpenAfterPublish: false
                                );
                            break;
                        case FileType.Xps:
                            excelWorkbook.ExportAsFixedFormat(
                                  Type: Excel.XlFixedFormatType.xlTypeXPS
                                , Filename: targetFilePath
                                , Quality: Excel.XlFixedFormatQuality.xlQualityStandard
                                , IncludeDocProperties: true
                                , IgnorePrintAreas: true
                                , OpenAfterPublish: false
                                );
                            break;
                        case FileType.Html:
                            excelWorkbook.SaveAs(Filename: targetFilePath, FileFormat: Excel.XlFileFormat.xlHtml, AccessMode: Excel.XlSaveAsAccessMode.xlExclusive);
                            break;
                        case FileType.Mht:
                            excelWorkbook.SaveAs(Filename: targetFilePath, FileFormat: Excel.XlFileFormat.xlWebArchive, AccessMode: Excel.XlSaveAsAccessMode.xlExclusive);
                            break;
                        default:
                            return new OfficeConvertResultInfo(OfficeConvertResult.InvalidFileType, targetFilePath);
                    }
                }
                catch (Exception ex3)
                {
                    switch (fileType)
                    {
                        case FileType.Xps:
                            return new OfficeConvertResultInfo(OfficeConvertResult.ExportToXpsError, "Excel", ex3);
                        case FileType.Pdf:
                            return new OfficeConvertResultInfo(OfficeConvertResult.ExportToPdfError, "Excel", ex3);
                        default:
                            return new OfficeConvertResultInfo(OfficeConvertResult.ExportToError, "Excel", ex3);
                    }
                }

                return new OfficeConvertResultInfo(OfficeConvertResult.Success, targetFilePath);
            }
            catch (Exception exc)
            {
                return new OfficeConvertResultInfo(OfficeConvertResult.OfficeInteropError, "Excel", exc);
            }
            finally
            {
                if (excelWorkbook != null)
                {
                    excelWorkbook.Close();
                    excelWorkbook = null;
                }
                if (excelApp != null)
                {
                    excelApp.Quit();
                    excelApp = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

        }

        /// <summary>
        /// Visio 转换
        /// </summary>
        /// <param name="sourceFilePath">来源文件路径</param>
        /// <param name="targetFilePath">目标路径</param>
        /// <param name="fileType">转换类型</param>
        /// <returns>执行结果</returns>
        private static OfficeConvertResultInfo visio_Convert(string sourceFilePath, string targetFilePath, FileType fileType)
        {
            Visio.Application visioApp = null;
            Visio.Document visioDocument = null;
            try
            {
                try
                {
                    visioApp = new Visio.Application();
                    //打开应用程序就隐藏
                    visioApp.Visible = false;
                    //重要：转换时不用打开确认窗口，直接转换
                    visioApp.AlertResponse = (short)1;
                }
                catch (Exception ex1)
                {
                    return new OfficeConvertResultInfo(OfficeConvertResult.InitializeOfficeAppError, "Visio", ex1);
                }
                try
                {
                    //visioDocument = visioApp.Documents.Open(sourceFilePath);
                    visioDocument = visioApp.Documents.OpenEx(sourceFilePath, (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenRO);
                }
                catch (Exception ex2)
                {
                    return new OfficeConvertResultInfo(OfficeConvertResult.OpenOfficeFileError, "Visio", ex2);
                }
                if (visioDocument == null)
                {
                    return new OfficeConvertResultInfo(OfficeConvertResult.OpenOfficeFileError);
                }
                switch (fileType)
                {
                    case FileType.Xps:
                        visioDocument.ExportAsFixedFormat(Visio.VisFixedFormatTypes.visFixedFormatXPS, targetFilePath, Visio.VisDocExIntent.visDocExIntentPrint, Visio.VisPrintOutRange.visPrintAll);
                        break;
                    case FileType.Pdf:
                        visioDocument.ExportAsFixedFormat(Visio.VisFixedFormatTypes.visFixedFormatPDF, targetFilePath, Visio.VisDocExIntent.visDocExIntentPrint, Visio.VisPrintOutRange.visPrintAll);
                        break;
                    case FileType.Html:
                        Microsoft.Office.Interop.Visio.SaveAsWeb.VisSaveAsWeb saveAsWeb = visioApp.SaveAsWebObject as Microsoft.Office.Interop.Visio.SaveAsWeb.VisSaveAsWeb;
                        Microsoft.Office.Interop.Visio.SaveAsWeb.VisWebPageSettings webPageSettings = saveAsWeb.WebPageSettings;
                        webPageSettings.TargetPath = targetFilePath;
                        webPageSettings.QuietMode = 1;
                        //安静模式，不然会显示转换进度窗口
                        webPageSettings.SilentMode = 1;
                        //将文档添加到需要转换的列表中
                        saveAsWeb.AttachToVisioDoc(visioDocument);
                        //开始转换
                        saveAsWeb.CreatePages();
                        break;
                    default:
                        return new OfficeConvertResultInfo(OfficeConvertResult.InvalidFileType, targetFilePath);
                }
                return new OfficeConvertResultInfo(OfficeConvertResult.Success, targetFilePath);
            }
            catch (Exception exc)
            {
                switch (fileType)
                {
                    case FileType.Xps:
                        return new OfficeConvertResultInfo(OfficeConvertResult.ExportToXpsError, "Visio", exc);
                    case FileType.Pdf:
                        return new OfficeConvertResultInfo(OfficeConvertResult.ExportToPdfError, "Visio", exc);
                    default:
                        return new OfficeConvertResultInfo(OfficeConvertResult.OfficeInteropError, "Visio", exc);
                }
            }
            finally
            {
                if (visioDocument != null)
                {
                    visioDocument.Close();
                    visioDocument = null;
                }
                if (visioApp != null)
                {
                    visioApp.Quit();
                    visioApp = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        #endregion --- 私有辅助方法 End ---
    }
}
