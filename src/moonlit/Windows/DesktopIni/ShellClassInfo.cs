using System;
using System.Collections.Generic;
using System.Text;

namespace Moonlit.Platform.Windows.Configuration.DesktopIni
{
    public class ShellClassInfo
    {
        string _iconFile;
        int _iconIndex;
        string _classID;
        public string IconFile
        {
            get
            {
                return this._iconFile;
            }
        }
        public string ClassID
        {
            get
            {
                return this._classID;
            }
        }
        /// <summary>
        /// 图标文件索引
        /// </summary>
        public int IconIndex
        {
            get
            {
                return this._iconIndex;
            }
        }
        internal ShellClassInfo(string iconFile, int iconIndex)
        {
            this._classID = string.Empty;
            this._iconIndex = iconIndex;
            this._iconFile = iconFile;
        }

        internal ShellClassInfo(string classID)
        {
            this._classID = classID;
            this._iconIndex = -1;
            this._iconFile = string.Empty;
        }
    }
}
