
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using FYJ.Common;
using FYJ.Data.Entity;
using FYJ.Data;
using Blogs.Entity;
using Blogs.IDAL;
using FYJ.Framework.Core;
using FYJ.Data.Util;

namespace Blogs.DAL
{
    public class DALArticle : FYJ.Framework.Core.DAL.DALAbstract<Blogs.Entity.blog_tb_article>, IDALArticle
    {
        protected override string PrimaryKey
        {
            get { return "articleID"; }
        }

        #region IArticle 成员

        /// <summary>
        /// 获取个人博客首页的置顶文章和文章预览列表 tag categoryID 可以传''不能传null  因为存储过程Null判断有点问题
        /// </summary>
        /// <param name="blogID"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryID"></param>
        /// <param name="tagID"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataSet GetArticleDataSet(string blogID, int page = 1, int pageSize = 10, string categoryID = "", string tagID = "", int year = 0, int month = 0)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@blogID", blogID);
            dic.Add("@page", page);
            dic.Add("@pageSize", pageSize);
            dic.Add("@categoryID", categoryID);
            dic.Add("@tagID", tagID);
            dic.Add("@year", year);
            dic.Add("@month", month);
            DataSet ds = this.DbInstance.RunProcedure("blog_proc_article", dic);

            //string sql = "DECLARE	@return_value int EXEC	@return_value = [dbo].[blog_proc_article] @blogID = N'" + blogID + "'";
            //sql += ",@page=" + page;
            //sql += ",@pageSize=" + pageSize;
            //sql += ",@categoryID='" + categoryID + "'";
            //sql += ",@tagID='" + tagID + "'";
            //sql += ",@year=" + year;
            //sql += ",@month=" + month;
            //sql += "  SELECT	'Return Value' = @return_value";
            //DataSet ds = this.DbInstance.GetDataSet(sql);
            return ds;
        }

        /// <summary>
        /// 获取个人博客文章列表
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        public DataTable GetArticleList(string blogID)
        {
            string sql = @"
            select  row_number() over(order by articleDatetime desc) as rownum, blog_tb_article.articleID,articleTitle,articleDatetime,articleClickTimes,articleCommentTimes 
            from blog_tb_article
            left join blog_tb_article_extend e on e.articleID=blog_tb_article.articleID
             where  blogID=@blogID and articleIsDisabled=0 and articleIsHidden=0 order by articleIsTop DESC,articleOrder DESC,articleDatetime DESC
                ";
            DataTable dt = this.DbInstance.GetDataTable(sql, this.DbInstance.CreateParameter("@blogID", blogID));

            return dt;
        }

        /// <summary>
        /// 获取单个文章信息
        /// </summary>
        /// <param name="articleID">文章ID</param>
        /// <param name="ip">访问IP</param>
        /// <param name="userID">访问用户ID</param>
        /// <returns></returns>
        public DataSet GetArticleByID(string articleID, string ip, string userID = null)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@articleID", articleID);
            dic.Add("@ip", ip);
            dic.Add("@userID", userID);
            DataSet ds = this.DbInstance.RunProcedure("blog_proc_singleArticle", dic);

            return ds;
        }

        /// <summary>
        /// 获取相关主题文章 
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        public DataTable GetArticleTopicList(string articleID)
        {
            string sql = @"select row_number() over(order by articleDatetime desc) as rownum,articleID,articleTitle,articleTitle2,articleUrl,topicDisplay from blog_view_article
 where topicID =(select topicID from blog_view_article where articleID=@articleID)
 and articleID <>@articleID and isDisabled=0";
            DataTable dt = this.DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@articleID", articleID));

            return dt;
        }

        public string GetWriteTableName(string blogID)
        {
            return "blog_tb_article_" + blogID.Substring(0, 1);
        }
        #endregion


        public override int Insert(Blogs.Entity.blog_tb_article entity)
        {
            throw new NotImplementedException();
        }

        public override int Update(Entity.blog_tb_article entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// 2015-6-10
        public override int Update(string id, string fieldName, object value)
        {
            base.Update(id, "UPDATE_DATE", DateTime.Now);
            return base.Update(id, fieldName, value);
        }

        public override int Delete(string id)
        {
            string tmp = "";
            foreach (string s in id.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string t = s.Trim('\'');
                tmp += "'" + t + "',";
            }
            tmp = tmp.TrimEnd(',');

            string sql = "delete from blog_tb_article_content where articleID in (" + tmp + ")";
            DbInstance.ExecuteSql(sql);
            return base.Delete(id);
        }


        public int UpdateLastOpenDatetime(string articleID)
        {
            int count = DbInstance.GetInt("select count(*) from blog_tb_article_extend where articleID=@articleID", DbInstance.CreateParameter("@articleID", articleID));
            if (count == 0)
            {
                return DbInstance.ExecuteSql("insert into blog_tb_article_extend(extendID,articleID,lastOpenDatetime,UPDATE_DATE) values (@extendID,@articleID,@lastOpenDatetime,@UPDATE_DATE)"
                    , DbInstance.CreateParameter("@extendID", Guid.NewGuid().ToString("N"))
                    , DbInstance.CreateParameter("@articleID", articleID)
                    , DbInstance.CreateParameter("@lastOpenDatetime", DateTime.Now)
                    , DbInstance.CreateParameter("@UPDATE_DATE", DateTime.Now));
            }
            else
            {
                return DbInstance.ExecuteSql("update  blog_tb_article_extend set lastOpenDatetime=@lastOpenDatetime,UPDATE_DATE=@UPDATE_DATE where articleID=@articleID"
                  , DbInstance.CreateParameter("@articleID", articleID)
                  , DbInstance.CreateParameter("@lastOpenDatetime", DateTime.Now)
                  , DbInstance.CreateParameter("@UPDATE_DATE", DateTime.Now));
            }
        }

        private void UpdateIndex()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("userid", typeof(string));
            dt.Columns.Add("blogid", typeof(string));
            dt.Columns.Add("classname", typeof(string));
            dt.Columns.Add("ishidden", typeof(string));
            dt.Columns.Add("title", typeof(string));
            dt.Columns.Add("url", typeof(string));
            dt.Columns.Add("datetime", typeof(string));
            dt.Columns.Add("content", typeof(string));

            //Blogs.IDAL.IArticle dal = FYJ.Data.Entity.DALContainerContext<Blogs.IDAL.IArticle>.Load();
            //string blogID = (User.Identity as Passport.Login.ICustomIdentity).BlogID;
            //string userID = (User.Identity as Passport.Login.ICustomIdentity).UserID;
            //string url = dal.DbInstance.GetString("select  articleUrl from blog_view_article where articleID='" + objectID + "'");
            //dt.Rows.Add(new object[] { objectID, userID, blogID, "blogarticle", (this.articleIsDisabled.Checked ? "1" : "0"), this.articleTitle.Text, url, DateTime.Now.ToString(), this.articleContent.Text });

            //DataSet ds = new DataSet();
            //if (dt.DataSet == null)
            //{
            //    ds.Tables.Add(dt);
            //}
            //else
            //{
            //    ds = dt.DataSet;
            //}

            //string src = FYJ.Common.DataSetHelper.Serialize(ds);
            //IndexServiceReference.IndexServiceClient client = new IndexServiceReference.IndexServiceClient();
            //string result = client.Update("", src);
        }


        public string LoadLastArticleID()
        {
            string articleID = DbInstance.GetString("select articleID from blog_tb_article_temp where articleID not in (select articleID from blog_tb_article)");

            return articleID;
        }


        public int CreateCatelog(string ids)
        {
            ids = ids.Trim(',');
            string sql = "select articleTitle,articleUrl from blog_view_article where articleID in (" + ids + ")  order by ADD_DATE";
            DataTable dt = DbInstance.GetDataTable(sql);
            string str = "";
            foreach (DataRow dr in dt.Rows)
            {
                str += "<p><a href=\"" + dr["articleUrl"] + "\">" + dr["articleTitle"] + "</a></p>";
            }
            DbInstance.BeginTran();
            int result = DbInstance.ExecuteSql("update blog_tb_article_content set articleContent=@articleContent+articleContent,UPDATE_DATE=@UPDATE_DATE where articleID in (" + ids + ")"
                , DbInstance.CreateParameter("@articleContent", str)
                , DbInstance.CreateParameter("@UPDATE_DATE", DateTime.Now));
            DbInstance.Commit();

            return result;
        }


        public long GetArticleUpdateLong(string articleID)
        {
            //不用EF  因为有缓存
            //long date = GetEntities().Where(c => c.articleID == articleID).Select(c => c.UPDATE_DATE).FirstOrDefault().ToFileTimeUtc();
            object obj = DbInstance.GetObject("select UPDATE_DATE from blog_tb_article where articleID=@articleID"
                , DbInstance.CreateParameter("@articleID", articleID));
            long date = Convert.ToInt64(Convert.ToDateTime(obj).ToString("yyyyMMddHHmmss"));

            return date;
        }

        //public override object NewID()
        //{
        //    int max = DbInstance.GetInt("select max(articleID) from blog_tb_article");
        //    return base.NewID();
        //}


        public int UpdateExtend(string articleID, string lastOpenIP, string lastOpenUserID)
        {
            string sql = "select * from blog_tb_article_extend where articleID=@articleID";
            if(DbInstance.Exists(sql,DbInstance.CreateParameter("@articleID", articleID)))
            {
                sql = " update blog_tb_article_extend set lastOpenDatetime=GETDATE(),lastOpenIP=@ip,lastOpenUserID=@userID,UPDATE_DATE=GETDATE(),articleClickTimes=articleClickTimes+1 where articleID=@articleID";
                 DbInstance.ExecuteSql(sql
                    , DbInstance.CreateParameter("@articleID", articleID)
                    , DbInstance.CreateParameter("@ip", lastOpenIP)
                    , DbInstance.CreateParameter("@userID", lastOpenUserID));
            }
            else
            {
                blog_tb_article_extend entity = new blog_tb_article_extend();
                entity.articleClickTimes = 1;
                entity.articleCommentTimes = 0;
                entity.articleID = Convert.ToInt32(articleID);
                entity.extendID = Guid.NewGuid().ToString("N");
                entity.lastOpenDatetime = DateTime.Now;
                entity.UPDATE_DATE = DateTime.Now;
                entity.lastOpenIP = lastOpenIP;
                entity.lastOpenUserID = lastOpenUserID;
                FYJ.Data.Entity.EntityHelper<blog_tb_article_extend>.Insert(entity, "blog_tb_article_extend", "extendID", true, DbInstance);
            }

            return 1;
        }


        public blog_tb_article GetSingle(int articleID, string select = "*")
        {
            DataTable dt = DbInstance.GetDataTable("select " + select + " from blog_tb_article where articleID=@articleID"
               , DbInstance.CreateParameter("@articleID", articleID));

            blog_tb_article model = FYJ.Data.Util.DataTableHelper.DataTableToModel<blog_tb_article>(dt).FirstOrDefault();

            return model;
        }


        public string GetarticleUserID(string articleID)
        {
            string sql = "select userID from blog_tb_article a left join blog_tb_blog b on a.blogID=b.blogID where a.articleID=@articleID";
            return DbInstance.GetString(sql, DbInstance.CreateParameter("@articleID", articleID));
        }


        public int ChangeArticle(string ids, string categoryID)
        {
            string tmp = "";
            foreach (string s in ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string t = s.Trim('\'');
                tmp += "'" + t + "',";
            }
            tmp = tmp.TrimEnd(',');
            string sql = "update  blog_tb_article  set categoryID=@categoryID where articleID in (" + tmp + ")";

            return DbInstance.ExecuteSql(sql, DbInstance.CreateParameter("@categoryID", categoryID));
        }


        public string ReadTempContent(string articleID)
        {
            string content = "";
            if (articleID == "0")
            {
                content = DbInstance.GetString("select top 1 articleContent from blog_tb_article_temp order by UPDATE_DATE DESC");
            }
            else
            {
                content = DbInstance.GetString("select articleContent from blog_tb_article_temp where articleID=@articleID", DbInstance.CreateParameter("@articleID", articleID));
            }
            return content;
        }

        public int SaveTempContent(string articleID, string content)
        {
            int result = 0;
            if (this.DbInstance.Exists("select 1 from blog_tb_article_temp where articleID=@articleID", DbInstance.CreateParameter("@articleID", articleID)))
            {
                result = DbInstance.ExecuteSql("update blog_tb_article_temp set articleContent=@articleContent,UPDATE_DATE=GETDATE() where articleID=@articleID"
                    , DbInstance.CreateParameter("@articleID", articleID)
                    , DbInstance.CreateParameter("@articleContent", content));
            }
            else
            {
                result = DbInstance.ExecuteSql("insert into blog_tb_article_temp values(@ID,@articleID,@articleContent,GETDATE(),GETDATE())",
                    DbInstance.CreateParameter("@ID", Guid.NewGuid().ToString("N")),
                    DbInstance.CreateParameter("@articleID", articleID),
                    DbInstance.CreateParameter("@articleContent", content));
            }

            return result;
        }


        public blog_tb_article_content GetArticleContent(string articleID)
        {
            string sql = "select * from blog_tb_article_content where articleID=@articleID";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@articleID", articleID));
            return ObjectHelper.DataTableToSingleModel<blog_tb_article_content>(dt);
        }

        public IEnumerable<blog_tb_tag> GetArticleTags(string articleID)
        {
            string sql = "select * from blog_tb_tag where tagID in ( select tagID from blog_tb_tagArticle where articleID=@articleID)";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@articleID", articleID));
            return ObjectHelper.DataTableToModel<blog_tb_tag>(dt);
        }


        public DataTable GetFileRelation(string objectID, string objectTag)
        {
            IDbHelper db =IocHelper<IDbHelperCreate>.Instance.GetDbInstance("Common");
            string sql = "select f.*,r.relationID from blog_tb_fileRelation r left join blog_tb_file f on r.fileID=f.fileID where objectID=@objectID and objectTag=@objectTag";
            DataTable dt = db.GetDataTable(sql, db.CreateParameter("@objectID", objectID), db.CreateParameter("@objectTag", objectTag));
            return dt;
        }

        public int Insert(blog_tb_article article, blog_tb_article_content content)
        {
            //if (!String.IsNullOrEmpty(entity.articleSourceUrl))
            //{
            //    if (this.GetEntities().Where(c => c.articleSourceUrl == entity.articleSourceUrl).Count() > 0)
            //    {
            //        throw new CustomException("来源Url已存在");
            //    }
            //}

            //删除临时正文
            this.DbInstance.ExecuteSql("delete from blog_tb_article_temp where articleID=@articleID", DbInstance.CreateParameter("@articleID", article.articleID));
            //如果描述文本为空并且正文不为空 则取正文的html格式化后的一小段
            if (String.IsNullOrEmpty(content.articleSubContentText) && (!String.IsNullOrEmpty(content.articleContent)))
            {
                string articleSubContentText = HttpHelper.HtmlFilter(content.articleContent);
                content.articleSubContentText = articleSubContentText.Substring(0, Math.Min(300, articleSubContentText.Length));
            }
            //插入正文
            content.UPDATE_DATE = DateTime.Now;
            content.contentID = Guid.NewGuid().ToString("N");
            EntityHelper<blog_tb_article_content>.Insert(content, "blog_tb_article_content", "contentID", true, this.DbInstance);
            return base.Insert(article);
        }

        public int Update(blog_tb_article article, blog_tb_article_content content)
        {
            //删除临时正文
            this.DbInstance.ExecuteSql("delete from blog_tb_article_temp where articleID=@articleID", DbInstance.CreateParameter("@articleID", article.articleID));
            //如果描述文本为空并且正文不为空 则取正文的html格式化后的一小段
            if (String.IsNullOrEmpty(content.articleSubContentText) && (!String.IsNullOrEmpty(content.articleContent)))
            {
                string articleSubContentText = HttpHelper.HtmlFilter(content.articleContent);
                content.articleSubContentText = articleSubContentText.Substring(0, Math.Min(300, articleSubContentText.Length));
            }
            //修改正文
            content.UPDATE_DATE = DateTime.Now;
            EntityHelper<blog_tb_article_content>.Update(content, "blog_tb_article_content", this.DbInstance, "contentID");
            return base.Update(article);
        }


        public DataTable GetArticlePhotos(string articleID)
        {
            IDbHelper db = IocHelper<IDbHelperCreate>.Instance.GetDbInstance("Common");
            string sql = "select f.fileID,f.fileThumbUrl,f.fileUrl,f.fileName,f.Exif from blog_tb_fileRelation r inner join blog_tb_file  f on r.fileID=f.fileID where f.fileIsImage=1 and r.objectID=@objectID";
            DataTable dt = db.GetDataTable(sql, db.CreateParameter("@objectID", articleID));
            return dt;
        }




        public int UpdateComment(string articleID, int add)
        {
            string addstr = (add >= 0 ? "+" + add : add.ToString());
            string sql = "update blog_tb_article_extend set articleCommentTimes=articleCommentTimes" + addstr + " where articleID=@articleID";
            return DbInstance.ExecuteSql(sql, DbInstance.CreateParameter("@articleID", articleID));
        }

        public int GetArticleCommentLimit(string articleID)
        {
            string sql = "select articleCommentLimit from blog_tb_article  where articleID=@articleID";
            return DbInstance.GetInt(sql, DbInstance.CreateParameter("@articleID", articleID));
        }
    }
}

