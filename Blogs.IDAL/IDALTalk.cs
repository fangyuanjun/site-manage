using Blogs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.IDAL
{
    public interface IDALTalk : FYJ.Framework.Core.IDAL.IDALBase<Blogs.Entity.blog_tb_Talk>
    {
        /// <summary>
        /// 读取临时正文
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        /// fangyj 2015-6-22
        string ReadTemp(string userID);

        /// <summary>
        /// 保存临时正文
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// fangyj 2015-6-22
        int SaveTemp(string userID, string content);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<blog_tb_Talk> Query(string userID);
    }
}
