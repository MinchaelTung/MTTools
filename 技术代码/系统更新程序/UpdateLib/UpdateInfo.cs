using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateLib
{
    [Serializable]
    public class UpdateInfo
    {

        private string _TmpDirectory = "";

        public string TmpDirectory
        {
            get { return _TmpDirectory; }
            set { _TmpDirectory = value; }
        }

        private string _URLAddres = "";

        public string URLAddres
        {
            get { return _URLAddres; }
            set { _URLAddres = value; }
        }
        private DateTime _UpdateTime = DateTime.MinValue;

        public DateTime UpdateTime
        {
            get { return _UpdateTime; }
            set { _UpdateTime = value; }
        }
        private string _Version = "1.0.0.1";

        public string Version
        {
            get { return _Version; }
            set { _Version = value; }
        }
        private List<UpdateFileInfo> _UpdateFileList = new List<UpdateFileInfo>();

        public List<UpdateFileInfo> UpdateFileList
        {
            get { return _UpdateFileList; }
            set { _UpdateFileList = value; }
        }
        private bool _ReStart = true;

        public bool ReStart
        {
            get { return _ReStart; }
            set { _ReStart = value; }
        }
        private string _AppName = "";

        public string AppName
        {
            get { return _AppName; }
            set { _AppName = value; }
        }

        public UpdateInfo()
        {

        }
    }

    [Serializable]
    public class UpdateFileInfo
    {

        private string _FileName;

        public string FileName
        {
            get
            {
                return this._FileName;
            }
            set
            {
                this._FileName = value;
            }
        }


        private string _TagDirectory;

        public string TagDirectory
        {
            get
            {
                return this._TagDirectory;
            }
            set
            {
                this._TagDirectory = value;
            }
        }

        public override string ToString()
        {
            return this._TagDirectory + this._FileName;
        }

    }
}
