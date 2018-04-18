using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SharePoint.Client;


namespace MT.SharePoint.Control
{
    public class DocLibMgt
    {
        #region --- 字段和属性 Begin ---

        private string rootFolderName;
        private Regex rx;
        private Regex regex;

        private string _ReplaceString;
        /// <summary>
        /// 非法字符替换字符(注:非法字符为 # % & * : < > ? / { | } )
        /// </summary>
        public string ReplaceString
        {
            get
            {
                return this._ReplaceString;
            }
            set
            {
                this._ReplaceString = regex.Replace(value, "_");
            }
        }

        #endregion --- 字段和属性 End ---

        #region --- Ctors Begin ---

        public DocLibMgt()
        {
            this.rootFolderName = string.Empty;
            this.rx = new Regex("/Forms/AllItems.aspx");
            this.regex = new Regex("[#%&*:<>?/{|}]");
            this._ReplaceString = "_";
        }

        #endregion --- Ctors End ---

        #region --- 显示方法 Begin ---

        /// <summary>
        /// 查询所有列表包含隐藏
        /// </summary>
        /// <returns></returns>
        public ListCollection QueryAllLists()
        {
            return MOSSClientContext.FetchClientObject(MOSSClientContext.ClientContext.Web.Lists);
        }

        /// <summary>
        /// 查询可见的所有列表
        /// </summary>
        /// <returns></returns>
        public ListCollection QueryAllListsForVisible()
        {
            return MOSSClientContext.FetchClientObject(MOSSClientContext.ClientContext.Web.Lists
      , listCollection => listCollection
          .Include(
          list => list.Title
          , list => list.Hidden
          , list => list.IsCatalog
          , list => list.IsApplicationList
          , list => list.BaseType
          , list => list.DefaultViewUrl)
          .Where(list => !list.Hidden));
        }

        /// <summary>
        /// 查询库表中可见的列表
        /// </summary>
        /// <returns></returns>
        public ListCollection QueryAllListsForVisibleLib()
        {
            return MOSSClientContext.FetchClientObject(MOSSClientContext.ClientContext.Web.Lists
                , listCollection =>
                    listCollection
                    .Include(
                            list => list.Title
                            , list => list.Hidden
                            , list => list.IsCatalog
                            , list => list.IsApplicationList
                            , list => list.BaseType
                            , list => list.DefaultViewUrl)
                    .Where(list =>
                        list.Hidden == false
                        && list.IsCatalog == false
                        && list.IsApplicationList == false
                        && list.BaseType == BaseType.DocumentLibrary)
                 );
        }

        /// <summary>
        /// 根据列表标题查找列表对象
        /// </summary>
        /// <param name="listTitle">列表标题</param>
        /// <returns></returns>
        public List QueryListsByTitle(string spListTitle)
        {
            return MOSSClientContext.FetchClientObject(MOSSClientContext.ClientContext.Web.Lists.GetByTitle(spListTitle));
        }

        /// <summary>
        /// 根据列表ID查找列表对象
        /// </summary>
        /// <param name="listId">列表ID</param>
        /// <returns></returns>
        public List QueryListsByID(Guid spListId)
        {
            return MOSSClientContext.FetchClientObject(MOSSClientContext.ClientContext.Web.Lists.GetById(spListId));
        }

        /// <summary>
        /// 中列表中获取列表的子项列表
        /// </summary>
        /// <param name="spList">指定列表</param>
        /// <returns></returns>
        public ListItemCollection QueryListItemsByList(List spList)
        {

            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View/>";
            return this.QueryListItemsByList(spList, camlQuery);
        }

        /// <summary>
        /// 中列表中获取列表的子项列表
        /// </summary>
        /// <param name="spList">指定列表</param>
        /// <param name="spCamlQuery">查询内容如“&lt;View/&gt;”</param>
        /// <returns></returns>
        public ListItemCollection QueryListItemsByList(List spList, CamlQuery spCamlQuery)
        {
            return MOSSClientContext.FetchClientObject(spList.GetItems(spCamlQuery));
        }

        /// <summary>
        /// 中列表中获取指定的子项
        /// </summary>
        /// <param name="spList">指定列表</param>
        /// <param name="spListItemID">子项ID</param>
        /// <returns></returns>
        public ListItem QueryListItemsByListAndListItemID(List spList, int spListItemID)
        {
            return MOSSClientContext.FetchClientObject(spList.GetItemById(spListItemID));
        }

        /// <summary>
        /// 中列表中获取指定的子项
        /// </summary>
        /// <param name="spList">指定列表</param>
        /// <param name="spListItemID">子项ID</param>
        /// <returns></returns>
        public ListItem QueryListItemsByListAndListItemID(List spList, string spListItemID)
        {
            return MOSSClientContext.FetchClientObject(spList.GetItemById(spListItemID));
        }

        /// <summary>
        /// 获取列表根目录对象
        /// </summary>
        /// <param name="spList">列表对象</param>
        /// <returns></returns>
        public Folder QueryRootFolderByList(List spList)
        {
            return MOSSClientContext.FetchClientObject(spList.RootFolder);
        }


        /// <summary>
        /// 获取列表对象的子目录不包含文件
        /// </summary>
        /// <param name="spList">指定列表</param>
        /// <returns></returns>
        public FolderCollection QueryFoldersByList(List spList)
        {
            return MOSSClientContext.FetchClientObject(spList.RootFolder.Folders);
        }

        /// <summary>
        /// 获取列表对象的子项表面文件不包含目录
        /// </summary>
        /// <param name="spList">指定列表</param>
        /// <returns></returns>
        public FileCollection QueryFilesByList(List spList)
        {
            return MOSSClientContext.FetchClientObject(spList.RootFolder.Files);
        }

        /// <summary>
        /// 获取目录中的子目录列表
        /// </summary>
        /// <param name="spFolder">指定目录</param>
        /// <returns></returns>
        public FolderCollection QueryChildrenFoldersByRootFolder(Folder spFolder)
        {
            return MOSSClientContext.FetchClientObject(spFolder.Folders);
        }

        /// <summary>
        /// 获取目录中文件
        /// </summary>
        /// <param name="spFolder">指定目录</param>
        /// <returns></returns>
        public FileCollection QueryFilesByFolder(Folder spFolder)
        {
            return MOSSClientContext.FetchClientObject(spFolder.Files);
        }

        #endregion --- 显示方法 End ---

        #region --- 上传方法 Begin ---

        /// <summary>
        /// 创建文档库       
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool CreateListItem(string title, string description)
        {
            try
            {

                Web spWeb = MOSSClientContext.FetchClientObject(MOSSClientContext.ClientContext.Web);
                ListCreationInformation spListCreationInformation = new ListCreationInformation();
                spListCreationInformation.Title = title;
                spListCreationInformation.Description = description;
                spListCreationInformation.TemplateType = (int)ListTemplateType.DocumentLibrary;
                spListCreationInformation.QuickLaunchOption = QuickLaunchOptions.On;

                List spList = spWeb.Lists.Add(spListCreationInformation);

                spList.Update();

                MOSSClientContext.ExecuteQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 上传目录结构到SharePoint网站的指定目录项中
        /// </summary>
        /// <param name="localPath">本地路径</param>
        /// <param name="spFolder">SharePoint网站的指定目录项</param>
        /// <returns></returns>
        public bool UploadDirectorys(string localPath, Folder spFolder)
        {
            try
            {
                this.createDirectories(localPath, spFolder);
                MOSSClientContext.ExecuteQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 上传文件包含目录结构到SharePoint网站的指定目录项中
        /// </summary>
        /// <param name="localPath">本地路径</param>
        /// <param name="spFolde">SharePoint网站的指定目录项</param>
        /// <returns></returns>
        public bool UploadFiles(string localPath, Folder spFolde)
        {
            try
            {
                this.UploadDirectorys(localPath, spFolde);
                this.recursionLoadFolders(spFolde);
                this.createFiles(localPath, spFolde);
                MOSSClientContext.ExecuteQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion --- 上传方法 End ---

        #region --- 下载方法 Begin ---

        /// <summary>
        /// 创建本地目录结构
        /// </summary>
        /// <param name="localPath">本地保存路径</param>
        /// <param name="spFolder">网站目录对象</param>
        /// <returns></returns>
        public bool CreateLocalDirectory(string localPath, Folder spFolder)
        {
            try
            {
                //递归获取所有子目录结构
                this.recursionLoadFolders(spFolder);
                this.createDirectory(localPath, spFolder);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 下载文件到本地包含本目录结构
        /// </summary>
        /// <param name="localPath">本地路径</param>
        /// <param name="spFolder">网站目录</param>
        /// <returns></returns>
        public bool CreateLocalFiles(string localPath, Folder spFolder)
        {
            try
            {
                string tempFolderName = this.rootFolderName;
                this.CreateLocalDirectory(localPath, spFolder);
                this.rootFolderName = tempFolderName;
                this.createFile(localPath, spFolder);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 下载文件到本地包含本目录结构
        /// </summary>
        /// <param name="localPath">本地路径</param>
        /// <param name="spList">网站列表</param>
        /// <returns></returns>
        public bool CreateLocalFiles(string localPath, List spList)
        {
            try
            {
                bool bl = false;
                this.rootFolderName = spList.Title;
                string folderUrl = rx.Replace(spList.DefaultViewUrl, string.Empty);
                folderUrl = folderUrl.Substring(folderUrl.LastIndexOf("/") + 1);
                Folder spFolder = MOSSClientContext.FetchClientObject(spList.RootFolder);
                bl = this.CreateLocalFiles(localPath, spFolder);
                return bl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 下载文件到本地
        /// </summary>
        /// <param name="localPath">本地路径</param>
        /// <param name="spFile">网站文件</param>
        /// <returns></returns>
        public bool CreateLocalFile(string localPath, File spFile)
        {
            try
            {
                this.createFile(localPath, spFile);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion --- 下载方法 End ---

        #region --- 删除对象 Begin ---

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="spFile"></param>
        public void DeleteClientObject(File spFile)
        {
            spFile.DeleteObject();
            MOSSClientContext.ExecuteQuery();
        }

        /// <summary>
        /// 删除整个目录
        /// </summary>
        /// <param name="spFolder"></param>
        public void DeleteClientObject(Folder spFolder)
        {
            spFolder.DeleteObject();
            MOSSClientContext.ExecuteQuery();
        }

        /// <summary>
        /// 删除整个列表
        /// </summary>
        /// <param name="spList"></param>
        public void DeleteClientObject(List spList)
        {
            spList.DeleteObject();
            MOSSClientContext.ExecuteQuery();
        }

        #endregion --- 删除对象 End ---

        #region --- Private Functions Begin ---

        /// <summary>
        /// 递归创建目录结构
        /// </summary>
        /// <param name="localPath">本地路径</param>
        /// <param name="spFolder">SharePoint网站的指定目录项</param>
        private void createDirectories(string localPath, Folder spFolder)
        {
            foreach (System.IO.DirectoryInfo itemDirectoryInfo in new System.IO.DirectoryInfo(localPath).GetDirectories())
            {
                string sFolderName = regex.Replace(itemDirectoryInfo.Name, this._ReplaceString);
                Folder spFolderClient = spFolder.Folders.Add(regex.Replace(sFolderName, this._ReplaceString));
                this.createDirectories(itemDirectoryInfo.FullName, spFolderClient);
            }
            spFolder.Update();
        }

        /// <summary>
        /// 获取递归获取所有子级目录结构
        /// </summary>
        /// <param name="spFolder">SharePoint网站的指定目录项</param>
        private void recursionLoadFolders(Folder spFolder)
        {
            FolderCollection folderCollection = MOSSClientContext.FetchClientObject(spFolder.Folders);
            foreach (Folder itemFolder in folderCollection)
            {
                this.recursionLoadFolders(itemFolder);
            }

        }

        /// <summary>
        /// 创建文件到SharePoint网站的目录对象中
        /// </summary>
        /// <param name="localPath">本地路径</param>
        /// <param name="spFolder">SharePoint网站的指定目录项</param>
        private void createFiles(string localPath, Folder spFolder)
        {
            //上传文件
            foreach (System.IO.FileInfo itemFileInfo in new System.IO.DirectoryInfo(localPath).GetFiles())
            {
                FileCreationInformation spFile = new FileCreationInformation();
                spFile.Content = System.IO.File.ReadAllBytes(itemFileInfo.FullName);
                string fileName = regex.Replace(itemFileInfo.Name, this._ReplaceString);
                spFile.Url = string.Format("{0}/{1}", spFolder.ServerRelativeUrl, fileName);
                spFile.Overwrite = true;
                spFolder.Files.Add(spFile);
            }
            spFolder.Update();
            //检查是否有子目录递归调用上传创建文件方法
            foreach (System.IO.DirectoryInfo itemDirectoryInfo in new System.IO.DirectoryInfo(localPath).GetDirectories())
            {
                string directoryName = regex.Replace(itemDirectoryInfo.Name, this._ReplaceString);
                foreach (Folder itemFolder in spFolder.Folders)
                {
                    if (directoryName.Equals(itemFolder.Name, StringComparison.CurrentCultureIgnoreCase) == true)
                    {
                        this.createFiles(itemDirectoryInfo.FullName, itemFolder);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Copy 数据流
        /// </summary>
        /// <param name="source">来源数据流</param>
        /// <param name="destination">目标数据流</param>
        private void copyStream(System.IO.Stream source, System.IO.Stream destination)
        {
            byte[] buffer = new byte[32768];
            int bytesRead;
            do
            {
                bytesRead = source.Read(buffer, 0, buffer.Length);
                destination.Write(buffer, 0, bytesRead);
            } while (bytesRead != 0);
        }


        /// <summary>
        /// 创建单个文件
        /// </summary>
        /// <param name="localPath">本地路径</param>
        /// <param name="spFile">网站文件对象</param>
        private void createFile(string localPath, File spFile)
        {
            try
            {
                FileInformation fileInformation = File.OpenBinaryDirect(MOSSClientContext.ClientContext, spFile.ServerRelativeUrl);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    this.copyStream(fileInformation.Stream, ms);
                    byte[] bytes = ms.ToArray();
                    System.IO.FileStream fs = new System.IO.FileStream(localPath + @"\" + spFile.Name, System.IO.FileMode.Create);
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs);
                    bw.Write(bytes);
                    bw.Close();
                    fs.Close();
                    ms.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建多个文件
        /// </summary>
        /// <param name="localPath">本地路径</param>
        /// <param name="spFolder">网站目录</param>
        private void createFile(string localPath, Folder spFolder)
        {
            try
            {
                string cPath = localPath;
                if (string.IsNullOrEmpty(this.rootFolderName) == true)
                {
                    cPath = localPath + @"\" + spFolder.Name;
                }
                else
                {
                    cPath = localPath + @"\" + this.rootFolderName;
                    this.rootFolderName = string.Empty;
                }
                //创建文件
                FileCollection spFileCollection = MOSSClientContext.FetchClientObject(spFolder.Files);
                foreach (File itemFile in spFileCollection)
                {
                    this.createFile(cPath, itemFile);
                }

                //包含多个目录
                foreach (Folder itemFolder in spFolder.Folders)
                {
                    if (itemFolder.Name.Equals("Forms", StringComparison.CurrentCultureIgnoreCase) == true)
                    {
                        continue;
                    }
                    this.createFile(cPath, itemFolder);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建递归目录
        /// </summary>
        /// <param name="localPath">本地路径</param>
        /// <param name="spFolder">网站目录</param>
        private void createDirectory(string localPath, Folder spFolder)
        {
            //生成本地路径
            string cPath = localPath;
            if (string.IsNullOrEmpty(this.rootFolderName) == true)
            {
                cPath = cPath + @"\" + spFolder.Name;
            }
            else
            {
                cPath = cPath + @"\" + this.rootFolderName;
                this.rootFolderName = string.Empty;
            }
            //创建目录
            System.IO.Directory.CreateDirectory(cPath);

            foreach (Folder itemFolder in spFolder.Folders)
            {
                if (itemFolder.Name.Equals("Forms", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    continue;
                }
                this.createDirectory(cPath, itemFolder);
            }
        }

        #endregion --- Private Functions End ---

    }
}
