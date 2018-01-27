using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogs.UI.Manage.Models
{
    public class Attachment
    {
        public string fileName { get; set; }


        public string fileUrl { get; set; }

        public string Name
        {
            get
            {
                if (String.IsNullOrEmpty(fileName))
                {
                    return System.IO.Path.GetFileName(fileUrl);
                }

                return fileName;
            }
        }

        public long fileSize { get; set; }

        public string SizeString
        {
            get
            {
                if (fileSize > 0 && fileSize < 1024)
                {
                    return fileSize + "B";
                }

                if (fileSize >= 1024 && fileSize < 1048576)
                {
                    return Math.Round(fileSize / 1024.0, 2) + "KB";
                }

                if (fileSize >= 1048576 && fileSize < 1048576 * 1024)
                {
                    return Math.Round(fileSize / 1048576.0, 2) + "MB";
                }

                return "";
            }
        }

        /// <summary>
        /// 下载次数
        /// </summary>
        public int DownloadCount { get; set; }

        public string icon { get; set; }
    }
}