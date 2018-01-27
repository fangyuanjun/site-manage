using Blogs.Entity;
using FYJ;
using FYJ.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.IBLL
{
    public interface IBLLArticleView
    {

       /// <summary>
       /// 分页查询
       /// </summary>
       /// <param name="pager">分页数据</param>
       /// <param name="keySelector">排序字段</param>
       /// <param name="exp">查询条件</param>
       /// <returns></returns>
       IEnumerable<blog_view_article> GetList(ref GridPager pager);

 
       IEnumerable<blog_view_article> Query(ArticleViewQueryEntity queryEntity, ref GridPager pager);
    }
}
