
using System;
using System.Collections.Generic;
using System.Data;
using Blogs.Entity;
using FYJ.Framework.Core.IBLL;


namespace Blogs.IBLL
{
    public interface IBLLArticle : IBLLBase<blog_tb_article>
    {
       

        /// <summary>
        /// 获取写入的表名
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        string GetWriteTableName(string blogID);

        /// <summary>
        /// 读取最后保存的临时正文新增的文章ID
        /// </summary>
        /// <returns></returns>
        string LoadLastArticleID();

        /// <summary>
        /// 生成目录  按照传入的id顺序  id已英文逗号隔开
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        int CreateCatelog(string ids);

        /// <summary>
        /// 获取文章用户ID
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        string GetarticleUserID(string articleID);

        /// <summary>
        /// 修改文章分类
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        /// fangyj 2015-5-8
        int ChangeArticle(string ids, string categoryID);

        /// <summary>
        /// 读取临时正文
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string ReadTempContent(string articleID);

        /// <summary>
        /// 保存临时正文
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        int SaveTempContent(string articleID, string content);

        /// <summary>
        /// 获取文章正文
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        /// fangyj 2015-5-8
        blog_tb_article_content GetArticleContent(string articleID);

        /// <summary>
        /// 获取文章标签
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        /// fangyj 2015-5-8
        IEnumerable<blog_tb_tag> GetArticleTags(string articleID);

        /// <summary>
        /// 获取文件关系
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="objectTag"></param>
        /// <returns></returns>
        IList<blog_attachment> GetFileRelation(string objectID, string objectTag);

        /// <summary>
        /// 插入文章
        /// </summary>
        /// <param name="article"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// fangyj 2015-5-9
        int Insert(blog_tb_article article, blog_tb_article_content content);

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="article"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// fangyj 2015-5-9
        int Update(blog_tb_article article, blog_tb_article_content content);

        /// <summary>
        /// 获取文章的图片
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        /// 2015-6-16
        IList<blog_attachment> GetArticlePhotos(string articleID);


    }
}
