using Blogs.Entity;
using Blogs.IBLL;
using Blogs.IDAL;
using FYJ;
using FYJ.Framework.Core;
using FYJ.Framework.Core.BLL;
using FYJ.Framework.Core.IBLL;
using FYJ.Framework.Core.IDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Blogs.BLL
{
    public class BLLArticle : BLLAbstract<blog_tb_article,IDALArticle>, IBLLArticle
    {

       
        /// <summary>
        /// 获取写入的表名
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        public string GetWriteTableName(string blogID)
        {
            return Dal.GetWriteTableName(blogID);
        }

        /// <summary>
        /// 读取最后保存的临时正文新增的文章ID
        /// </summary>
        /// <returns></returns>
        public string LoadLastArticleID()
        {
            return Dal.LoadLastArticleID();
        }

        /// <summary>
        /// 生成目录  按照传入的id顺序  id已英文逗号隔开
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int CreateCatelog(string ids)
        {
            return Dal.CreateCatelog(ids);
        }

        public IEnumerable<blog_tb_article> GetList(ref GridPager pager, params Func<blog_tb_article, bool>[] exp)
        {
            throw new NotImplementedException();
        }


        public string GetarticleUserID(string articleID)
        {
            return Dal.GetarticleUserID(articleID);
        }



        public int ChangeArticle(string ids, string categoryID)
        {
            CheckBlog.ValidateBlog(typeof(blog_tb_article), ids);
            return Dal.ChangeArticle(ids, categoryID);
        }


        public string ReadTempContent(string articleID)
        {
            CheckBlog.ValidateBlog(typeof(blog_tb_article), articleID);
            return Dal.ReadTempContent(articleID);
        }

        public int SaveTempContent(string articleID, string content)
        {
            //CheckBlog.ValidateBlog(typeof(blog_tb_article), articleID);
            return Dal.SaveTempContent(articleID, content);
        }




        public blog_tb_article_content GetArticleContent(string articleID)
        {
            CheckBlog.ValidateBlog(typeof(blog_tb_article), articleID);
            return Dal.GetArticleContent(articleID);
        }

        public IEnumerable<blog_tb_tag> GetArticleTags(string articleID)
        {
            return Dal.GetArticleTags(articleID);
        }


        public IList<blog_attachment> GetFileRelation(string objectID, string objectTag)
        {
            return Dal.GetFileRelation(objectID, objectTag);
        }


        public int Insert(blog_tb_article article, blog_tb_article_content content)
        {
            if (String.IsNullOrEmpty(article.articlePic))
            {
                Match m = Regex.Match(content.articleContent, "src=\"(http://static.kecq.com.*?)\"");
                if (m.Success)
                {
                    article.articlePic = m.Groups[1].Value;
                    article.articleThumbPic = m.Groups[1].Value;
                }
            }
            return Dal.Insert(article, content);
        }

        public int Update(blog_tb_article article, blog_tb_article_content content)
        {
            CheckBlog.ValidateBlog(typeof(blog_tb_article), article.articleID+"");
            if (String.IsNullOrEmpty(article.articlePic))
            {
                Match m = Regex.Match(content.articleContent, "src=\"(http://static.kecq.com.*?)\"");
                if (m.Success)
                {
                    article.articlePic = m.Groups[1].Value;
                    article.articleThumbPic = m.Groups[1].Value;
                }
            }
            return Dal.Update(article, content);
        }


        public IList<blog_attachment> GetArticlePhotos(string articleID)
        {
            return Dal.GetArticlePhotos(articleID);
        }

    }
}
