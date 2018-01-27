using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
using FYJ.Framework.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.BLL
{
    public class BLLArticleView :IBLL.IBLLArticleView
    {
        private IDALArticleView dal
        {
            get
            {
                return IocFactory<IDALArticleView>.Instance;
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pager">分页数据</param>
        /// <param name="keySelector">排序字段</param>
        /// <param name="exp">查询条件</param>
        /// <returns></returns>
        public IEnumerable<blog_view_article> GetList(ref GridPager pager)
        {
            return dal.GetList(ref pager);
        }


        public IEnumerable<blog_view_article> Query(ArticleViewQueryEntity queryEntity, ref GridPager pager)
        {
            return dal.Query(queryEntity,ref pager);
        }
    }
}
