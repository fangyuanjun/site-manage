
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
using FYJ;

namespace Blogs.DAL
{
    public class DALArticle : FYJ.Framework.Core.DAL.DALAbstract<Blogs.Entity.blog_tb_article>, IDALArticle
    {
        public override object NewID()
        {
            int r = new Random().Next(100, 999);
            string result = "1" + DateTime.Now.ToString("MMdd") + r + "";
            string sql = "select 1 from blog_tb_article where articleID='"+result+"'";
            while( DbInstance.Exists(sql))
            {
                r = new Random().Next(100, 999);
                result = "1" + DateTime.Now.ToString("MMdd") + r + "";
                sql = "select 1 from blog_tb_article where articleID='" + result + "'";
            }
            //string result = "1" + new Random().Next(1000000, 9999999);
           
            return result;
        }

        protected override string PrimaryKey
        {
            get { return "articleID"; }
        }

      
       
        public string GetWriteTableName(string blogID)
        {
            return "blog_tb_article_" + blogID.Substring(0, 1);
        }
       


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

            sql = "delete from blog_tb_from where fromUrl in (select articleSourceUrl from blog_tb_article where ifnull(articleSourceUrl,'')<>'' and articleID in (" + tmp + "))";
            DbInstance.ExecuteSql(sql);
            return base.Delete(id);
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
        }


        public string LoadLastArticleID()
        {
            string articleID = DbInstance.GetString("select articleID from blog_tb_article_temp where articleID not in (select articleID from blog_tb_article)");

            return articleID;
        }


        public int CreateCatelog(string ids)
        {
            ids = ids.Trim(',');
            string sql = "select articleTitle,articleID from blog_tb_article where articleID in (" + ids + ")  order by ADD_DATE";
            DataTable dt = DbInstance.GetDataTable(sql);
            string str = "";
            foreach (DataRow dr in dt.Rows)
            {
                str += "<p><a href=\"/artic-" + dr["articleID"] + ".html\">" + dr["articleTitle"] + "</a></p>";
            }
            DbInstance.BeginTran();
            int result = DbInstance.ExecuteSql("update blog_tb_article_content set articleContent=@articleContent+articleContent,UPDATE_DATE=@UPDATE_DATE where articleID in (" + ids + ")"
                , DbInstance.CreateParameter("@articleContent", str)
                , DbInstance.CreateParameter("@UPDATE_DATE", DateTime.Now));
            DbInstance.Commit();

            return result;
        }

        //public override object NewID()
        //{
        //    int max = DbInstance.GetInt("select max(articleID) from blog_tb_article");
        //    return base.NewID();
        //}


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
                content = DbInstance.GetString("select  articleContent from blog_tb_article_temp order by UPDATE_DATE DESC limit 0,1");
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
                result = DbInstance.ExecuteSql("update blog_tb_article_temp set articleContent=@articleContent,UPDATE_DATE=@UPDATE_DATE where articleID=@articleID"
                    , DbInstance.CreateParameter("@articleID", articleID)
                    , DbInstance.CreateParameter("@articleContent", content)
                    , DbInstance.CreateParameter("@UPDATE_DATE", DateTime.Now));
            }
            else
            {
                result = DbInstance.ExecuteSql("insert into blog_tb_article_temp values(@articleID,@articleContent,@ADD_DATE,@UPDATE_DATE)"
                    , DbInstance.CreateParameter("@articleID", articleID)
                    , DbInstance.CreateParameter("@articleContent", content)
                    , DbInstance.CreateParameter("@ADD_DATE", DateTime.Now)
                    , DbInstance.CreateParameter("@UPDATE_DATE", DateTime.Now));
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


        public IList<blog_attachment> GetFileRelation(string objectID, string objectTag)
        {
            // select fileUrl,[fileName],fileSize from db_common.dbo.blog_tb_fileRelation inner join  db_common.dbo.blog_tb_file on db_common.dbo.blog_tb_file.fileID=blog_tb_fileRelation.fileID
            //where objectID = @articleID and objectType = 'attachment'

            IDbHelper db = IocFactory<IDbFactory>.Instance.GetDbInstance("Files");
            string sql = "select f.*,r.relationID,c.DownloadCount from blog_tb_fileRelation r left join blog_tb_file f on r.fileID=f.fileID  LEFT JOIN blog_tb_filecount c on c.FileID=f.fileID where objectID=@objectID and objectTag=@objectTag";
            DataTable dt = db.GetDataTable(sql, db.CreateParameter("@objectID", objectID), db.CreateParameter("@objectTag", objectTag));

            return FYJ.ObjectHelper.DataTableToModel<blog_attachment>(dt);
        }

        public IList<blog_attachment> GetArticlePhotos(string articleID)
        {
            IDbHelper db = IocFactory<IDbFactory>.Instance.GetDbInstance("Files");
            string sql = "select f.fileID,f.fileThumbUrl,f.fileUrl,f.fileName,f.Exif from blog_tb_fileRelation r inner join blog_tb_file  f on r.fileID=f.fileID where f.fileIsImage=1 and r.objectID=@objectID";
            DataTable dt = db.GetDataTable(sql, db.CreateParameter("@objectID", articleID));

            return FYJ.ObjectHelper.DataTableToModel<blog_attachment>(dt);
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
            EntityHelper<blog_tb_article_content>.Insert(content, "blog_tb_article_content", "articleID", true, this.DbInstance);
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
            EntityHelper<blog_tb_article_content>.Update(content, "blog_tb_article_content", this.DbInstance, "articleID");
            return base.Update(article);
        }

        public int UpdateComment(string articleID, int add)
        {
            string addstr = (add >= 0 ? "+" + add : add.ToString());
            string sql = "update blog_tb_article_extend set articleCommentTimes=articleCommentTimes" + addstr + " where articleID=@articleID";
            return DbInstance.ExecuteSql(sql, DbInstance.CreateParameter("@articleID", articleID));
        }


    }
}

