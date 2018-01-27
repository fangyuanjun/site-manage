
using Blogs.Entity;
using Blogs.IDAL;
using FYJ.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Blogs.DAL
{
    public class DALComment : FYJ.Framework.Core.DAL.DALAbstract<Blogs.Entity.blog_tb_comment>, IDALComment
    {
        protected override string PrimaryKey
        {
            get { return "commentID"; }
        }

        public override int Insert(blog_tb_comment entity)
        {
            entity.commentID = Guid.NewGuid().ToString("N");
            entity.ADD_DATE = DateTime.Now;
            entity.UPDATE_DATE = DateTime.Now;
            
            return base.Insert(entity);
        }

        public new int Delete(string commentID)
        {
            try
            {
                DbInstance.BeginTran();
                string sql = "select articleID,articleCommentTimes from blog_view_article where articleID=(select articleID from blog_tb_comment where commentID=@commentID)";
                DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@commentID", commentID));
                if (dt.Rows.Count > 0)
                {
                    int articleCommentTimes = Convert.ToInt32(dt.Rows[0]["articleCommentTimes"]);
                    articleCommentTimes = Math.Max(articleCommentTimes - 1, 0);
                    sql = "update blog_tb_article_extend  set articleCommentTimes=" + articleCommentTimes + " where articleID=@articleID";
                    DbInstance.ExecuteSql(sql, DbInstance.CreateParameter("@articleID", dt.Rows[0]["articleID"]));
                }

                base.Delete(commentID);
                DbInstance.Commit();

                return 1;
            }
            catch
            {
                DbInstance.Rollback();
                throw;
            }
        }

        public int GetNoReadCommentCount(string blogID)
        {
            string sql = @"select COUNT(*) from blog_tb_comment 
                            inner join blog_view_article on blog_view_article.articleID=blog_tb_comment.articleID 
                            left join blog_tb_article_extend on blog_tb_article_extend.articleID=blog_view_article.articleID
                            where  blogID=@blogID
                            and blog_tb_comment.ADD_DATE>=blog_tb_article_extend.lastOpenDatetime";
            int count = DbInstance.GetInt(sql, DbInstance.CreateParameter("@blogID", blogID));

            return count;
        }

        public int Vote(string articleID, string typeID, string userID, string ip)
        {
            string stateID = String.Empty;
            //如果评论状态表中已存在该文章的该状态
            string sql = "select * from blog_tb_commentState where articleID=@articleID and typeID=typeID";
            DataTable dt = DbInstance.GetDataTable(sql
                , this.DbInstance.CreateParameter("@articleID", articleID)
                , this.DbInstance.CreateParameter("@typeID", typeID));
            if (dt.Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(userID))
                {
                    if (DbInstance.Exists("select 1 from blog_tb_commentStateSupport where stateID=@stateID and userID=@userID"
                     , this.DbInstance.CreateParameter("@stateID", dt.Rows[0]["stateID"])
                     , this.DbInstance.CreateParameter("@userID", userID)))
                    {
                        throw new CustomException("你已经支持过了");
                    }
                }

                if (DbInstance.Exists("select 1 from blog_tb_commentStateSupport where stateID=@stateID and supportIP=@supportIP"
                    , this.DbInstance.CreateParameter("@stateID", dt.Rows[0]["stateID"])
                    , this.DbInstance.CreateParameter("@supportIP", ip)))
                {
                    throw new CustomException("你已经支持过了");
                }

                stateID = dt.Rows[0]["stateID"].ToString();
                DbInstance.ExecuteSql("update blog_tb_commentState set stateCount=stateCount+1,UPDATE_DATE=getdate() where stateID=@stateID"
                    , this.DbInstance.CreateParameter("@stateID", dt.Rows[0]["stateID"]));
            }
            else
            {
                stateID = Guid.NewGuid().ToString("N");
                blog_tb_commentState state = new blog_tb_commentState { stateID = stateID, articleID = Convert.ToInt32(articleID), typeID = typeID, stateCount = 1, ADD_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now };
                FYJ.Data.Entity.EntityHelper<blog_tb_commentState>.Insert(state, "blog_tb_commentState", "stateID", true, this.DbInstance);
            }

            blog_tb_commentStateSupport stateSupport = new blog_tb_commentStateSupport { supportID = Guid.NewGuid().ToString("N"), stateID = stateID, supportDatetime = DateTime.Now, supportIP = ip, userID = userID, UPDATE_DATE = DateTime.Now };
            FYJ.Data.Entity.EntityHelper<blog_tb_commentStateSupport>.Insert(stateSupport, "blog_tb_commentStateSupport", "supportID", true, this.DbInstance);

            return 1;
        }


        public int Support(string commentID, string userID, string ip)
        {
            string sql = string.Empty;
            if (!String.IsNullOrEmpty(userID))
            {
                sql = "select * from blog_tb_commentSupport where commentID=@commentID and userID=@UserID and isSupport=@isSupport";
                if (DbInstance.Exists(sql
                    , DbInstance.CreateParameter("@commentID", commentID)
                    , DbInstance.CreateParameter("@userID", userID)
                    , DbInstance.CreateParameter("@isSupport", true)
                    ))
                {
                    throw new CustomException("你已经支持过了");
                }
            }

            sql = "select * from blog_tb_commentSupport where commentID=@commentID and supportIP=@supportIP and isSupport=@isSupport";
            if (DbInstance.Exists(sql
                , DbInstance.CreateParameter("@commentID", commentID)
                , DbInstance.CreateParameter("@supportIP", ip)
                , DbInstance.CreateParameter("@isSupport", true)
                ))
            {
                throw new CustomException("你已经支持过了");
            }

            blog_tb_commentSupport support = new blog_tb_commentSupport { supportID = Guid.NewGuid().ToString("N"), supportIP = ip, commentID = commentID, supportDatetime = DateTime.Now, isSupport = true, UPDATE_DATE = DateTime.Now };
            FYJ.Data.Entity.EntityHelper<blog_tb_commentSupport>.Insert(support, "blog_tb_commentSupport", "supportID", true, this.DbInstance);
            sql = "update blog_tb_comment set supportCount=supportCount+1 where commentID=@commentID";
            return this.DbInstance.ExecuteSql(sql, this.DbInstance.CreateParameter("@commentID", commentID));
        }

        public int Against(string commentID, string userID, string ip)
        {
            string sql = string.Empty;
            if (!String.IsNullOrEmpty(userID))
            {
                sql = "select * from blog_tb_commentSupport where commentID=@commentID and userID=@UserID and isSupport=@isSupport";
                if (DbInstance.Exists(sql
                    , DbInstance.CreateParameter("@commentID", commentID)
                    , DbInstance.CreateParameter("@userID", userID)
                    , DbInstance.CreateParameter("@isSupport", false)
                    ))
                {
                    throw new CustomException("你已经反对过了");
                }
            }

            sql = "select * from blog_tb_commentSupport where commentID=@commentID and supportIP=@supportIP and isSupport=@isSupport";
            if (DbInstance.Exists(sql
                , DbInstance.CreateParameter("@commentID", commentID)
                , DbInstance.CreateParameter("@supportIP", ip)
                , DbInstance.CreateParameter("@isSupport", false)
                ))
            {
                throw new CustomException("你已经反对过了");
            }

            blog_tb_commentSupport support = new blog_tb_commentSupport { supportID = Guid.NewGuid().ToString("N"), supportIP = ip, commentID = commentID, supportDatetime = DateTime.Now, isSupport = false, UPDATE_DATE = DateTime.Now };
            FYJ.Data.Entity.EntityHelper<blog_tb_commentSupport>.Insert(support, "blog_tb_commentSupport", "supportID", true, this.DbInstance);
            sql = "update blog_tb_comment set againstCount=againstCount+1 where commentID=@commentID";

            return this.DbInstance.ExecuteSql(sql, this.DbInstance.CreateParameter("@commentID", commentID));
        }

        public DataSet GetCommentPage(string articleID, int page, int pageSize)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@articleID", articleID);
            dic.Add("@page", page);
            dic.Add("@pageSize", pageSize);
            DataSet ds = this.DbInstance.RunProcedure("blog_proc_comment", dic);

            return ds;
        }


        public void ValidateCommentLimit(string articleID, string userID)
        {
            return;
        }

        public bool isDisableComment(int articleCommentLimit)
        {
            CommentLimit limit = (CommentLimit)Enum.Parse(typeof(CommentLimit), articleCommentLimit + "");
            //bool test = (limit & CommentLimit.禁止回复) == CommentLimit.禁止回复;
            bool hasFlag = ((limit & CommentLimit.禁止回复) != 0);

            return hasFlag;
        }

        public bool isVerifyComment(int articleCommentLimit)
        {
            CommentLimit limit = (CommentLimit)Enum.Parse(typeof(CommentLimit), articleCommentLimit + "");
            bool hasFlag = ((limit & CommentLimit.需要审核) != 0);

            return hasFlag;
        }


        public bool isDisabledAnonymousComment(int articleCommentLimit)
        {
            CommentLimit limit = (CommentLimit)Enum.Parse(typeof(CommentLimit), articleCommentLimit + "");
            bool hasFlag = ((limit & CommentLimit.禁止匿名用户回复) != 0);

            return hasFlag;
        }

        public int GetMaxFloor(string articleID)
        {
            string sql = "select MAX(floor) from blog_tb_comment c left join blog_tb_article a on a.articleID=c.articleID where c.articleID=@articleID";
            return DbInstance.GetInt(sql, DbInstance.CreateParameter("@articleID", articleID));
        }


        public bool IsReplyed(string articleID, string userID)
        {
            string sql = "select 1 from blog_tb_comment where articleID=@articleID and userID=@userID";
            return DbInstance.Exists(sql, DbInstance.CreateParameter("@articleID", articleID)
                                        , DbInstance.CreateParameter("@userID", userID));
        }
    }
}

