using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.BLL
{
    public interface IBlogInfo
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        string CurrentUserID { get; }

        /// <summary>
        /// 当前博客
        /// </summary>
        string CurrentBlogID { get; }
    }
}
