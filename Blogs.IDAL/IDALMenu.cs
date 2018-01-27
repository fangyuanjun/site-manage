
using System.Collections.Generic;
using Blogs.Entity;

namespace Blogs.IDAL
{
    public interface IDALMenu : FYJ.Framework.Core.IDAL.IDALBase<blog_tb_menu>
    {
        List<blog_tb_menu> GetList(string blogID, string parentID);
    }
}
