using Blogs.Entity;
using Blogs.UI.Manage.ViewModel;
using FYJ.Common;
using FYJ.Framework.Core;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using FYJ;

namespace Blogs.UI.Manage.Controllers
{
    public class ArticleController : Controller
    {
        public ActionResult Index()
        {
            string blogID = UserInfo.BlogID;
            string userID = UserInfo.UserID;

            var categorys = Utility.CategoryBll.GetTreeData(blogID);
            var topics = Utility.TopicBll.GetTopic(blogID);
            ViewData["categorySelect"] = new SelectList(categorys, "categoryID", "TextWithTreeSpace");
            ViewData["TopicSelect"] = new SelectList(topics, "topicID", "topicDisplay");
            if (Request["v"] == "m")
            {
                return View("~/Views/" + Utility.Version + "/Article/Index-mobile.cshtml");
            }

            if (Request["v"] == "e")
            {
                return View("~/Views/" + Utility.Version + "/Article/Index-easyui.cshtml");
            }

            string userAgent = Request.UserAgent;
            if (Regex.IsMatch(userAgent, "iphone", RegexOptions.IgnoreCase))
            {
                return View("~/Views/" + Utility.Version + "/Article/Index-mobile.cshtml");
            }

            //return View("~/Views/" + Utility.Version + "/Article/Index.cshtml");
            return View("~/Views/" + Utility.Version + "/Article/Index.cshtml");
        }


        public JsonResult GetList(GridPager pager)
        {
            string blogID = UserInfo.BlogID;
            if (!String.IsNullOrEmpty(Request["page"]))
            {
                pager.CurrentPage = Convert.ToInt32(Request["page"]);
            }

            if (!String.IsNullOrEmpty(Request["rows"]))
            {
                pager.PageSize = Convert.ToInt32(Request["rows"]);
            }

            if (!String.IsNullOrEmpty(Request["limit"]))
            {
                pager.PageSize = Convert.ToInt32(Request["limit"]);
            }


            if (!String.IsNullOrEmpty(Request["offset"]))
            {
                pager.Offset = Convert.ToInt32(Request["offset"]);
            }

            pager.OrderColumn = Request["sort"];
            pager.Order = Request["order"];

            ArticleViewQueryEntity queryEntity = new ArticleViewQueryEntity();
            queryEntity.BlogID = UserInfo.BlogID + "";

            queryEntity.CategoryID = Request["ddlcategorySelect"];
            queryEntity.TopicID = Request["ddlTopicSelect"];
            queryEntity.ArticleIsOriginal = Request["ddlarticleIsOriginalSelect"];
            queryEntity.StartDate = Request["StartDate"];
            queryEntity.EndDate = Request["EndDate"];
            queryEntity.ArticleTitle = Request["articleTitle"];



            string where = " 1=1";
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            if (!String.IsNullOrWhiteSpace(queryEntity.BlogID))
            {
                where += " and blogID=@blogID";
                parameters.Add("@blogID", queryEntity.BlogID);
            }

            if (!String.IsNullOrWhiteSpace(queryEntity.SiteCategoryID))
            {
                where += " and siteCategoryID=@siteCategoryID";
                parameters.Add("@siteCategoryID", queryEntity.SiteCategoryID);
            }
            if (!String.IsNullOrWhiteSpace(queryEntity.CategoryID))
            {
                where += " and categoryID=@categoryID";
                parameters.Add("@categoryID", queryEntity.CategoryID);
            }
            if (!String.IsNullOrWhiteSpace(queryEntity.TopicID))
            {
                where += " and topicID=@topicID";
                parameters.Add("@topicID", queryEntity.TopicID);
            }
            if (!String.IsNullOrWhiteSpace(queryEntity.ArticleIsOriginal))
            {
                bool b = (Int32.Parse(queryEntity.ArticleIsOriginal) == 1);
                where += " and articleIsOriginal=@articleIsOriginal";
                parameters.Add("@articleIsOriginal", b);
            }
            if (!String.IsNullOrWhiteSpace(queryEntity.StartDate))
            {
                where += " and articleDatetim>=@StartDate";
                parameters.Add("@StartDate", Convert.ToDateTime(queryEntity.StartDate).Date);
            }
            if (!String.IsNullOrWhiteSpace(queryEntity.EndDate))
            {
                where += " and articleDatetim<=@EndDate";
                parameters.Add("@EndDate", Convert.ToDateTime(queryEntity.EndDate).Date);
            }
            if (!String.IsNullOrWhiteSpace(queryEntity.ArticleTitle))
            {
                where += " and LOWER(articleTitle)  like @articleTitle";
                parameters.Add("@articleTitle", "%" + queryEntity.ArticleTitle + "%");

                //where += " and LOWER(articleTitle)  like '%"+ FYJ.Common.StringHelper.SqlFilter(queryEntity.ArticleTitle) + "%'";
            }

            if (!String.IsNullOrEmpty(Request["search"]))
            {
                where += " and (LOWER(articleTitle)  like @search or articleID=@search1)";
                parameters.Add("@search", "%" + Request["search"].Trim() + "%");
                parameters.Add("@search1", Request["search"].Trim());
            }

            pager.Where = where;
            pager.Parameters = parameters;

            IEnumerable<Blogs.Entity.blog_view_article> list = Utility.ArticleViewBll.GetList(ref pager);

            var json = new
            {
                total = pager.TotalRows,
                rows = (from r in list
                        select new
                        {
                            ID = r.articleID,
                            r.articleID,
                            r.siteCategoryDisplay,
                            r.categoryDisplay,
                            r.topicDisplay,
                            r.articleTitle,
                            r.articlePic,
                            r.articleIsPic,
                            r.articleIsOriginal,
                            r.articleIsHidden,
                            r.articleIsDisabled,
                            r.articleIsTop,
                            r.articleClickTimes,
                            r.articleCommentTimes,
                            r.articlePostBy,
                            r.themeID,
                            r.articlePassword,
                            r.articleKeywords,
                            r.articleDescription,
                            r.articleAuthor,
                            articleDatetime = r.articleDatetime.ToString("yyyy-MM-dd HH:mm:ss"),
                            ADD_DATE = r.ADD_DATE.ToString("yyyy-MM-dd HH:mm:ss"),
                            UPDATE_DATE = r.UPDATE_DATE.ToString("yyyy-MM-dd HH:mm:ss"),
                            isDisableComment = Utility.CommentBll.isDisableComment(r.articleCommentLimit),
                            isAllowAnonymousComment = Utility.CommentBll.isDisabledAnonymousComment(r.articleCommentLimit),
                            isVerifyComment = Utility.CommentBll.isVerifyComment(r.articleCommentLimit)
                        }).ToArray()
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }



        //private ActionResult ActionPage(int page, int pageSize, int recordCount)
        //{
        //    string urlRewritePattern = "/Article?page={0}&rows=15&sort=articleDatetime&order=desc";

        //    Pager pager = new Pager()
        //    {
        //        RecordCount = recordCount,
        //        PageSize = pageSize,
        //        MaxShowPageSize = 15,
        //        PageIndex = page,
        //        ShowSpanText = false,
        //        EnableUrlRewriting = true,
        //        UrlRewritePattern = urlRewritePattern
        //    };

        //    ContentResult result= new ContentResult();
        //    result.ContentEncoding = Encoding.UTF8;
        //    result.Content= pager.ToString();

        //    return result;
        //}


        public ActionResult GetCategoryJson()
        {
            string blogID = Request["blogID"];
            List<blog_tb_category> list = Utility.CategoryBll.GetTreeData(UserInfo.BlogID);
            blog_tb_category insertmodel = new blog_tb_category();
            insertmodel.categoryID = 0;
            insertmodel.TextWithTreeSpace = "--请选择--";
            list.Insert(0, insertmodel);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTopicJson()
        {
            int blogID = Convert.ToInt32(Request["blogID"]);
            var topics = Utility.TopicBll.GetTopic(blogID.ToString());
            List<object> list = new List<object>();
            foreach (blog_tb_topic topic in topics)
            {
                list.Add(new { topicID = topic.topicID, topicDisplay = topic.topicDisplay });
            }
            list.Insert(0, new { topicID = "", topicDisplay = "--请选择--" });

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ChangeArticle(FormCollection collection)
        {
            int result = Utility.ArticleBll.ChangeArticle(Request["ids"], Request["categoryID"]);
            if (result > 0)
            {
                return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { code = -1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ChangeState(FormCollection collection)
        {
            if (Utility.ArticleBll.Update(Request["ids"], Request["fieldName"], Request["state"] == "1" ? true : false) > 0)
            {
                return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { code = -1, message = "操作失败" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult TagString(FormCollection collection)
        {
            var v = Utility.TagBll.GetTags(Request["blogID"]);
            string tagString = "";
            foreach (var item in v)
            {
                tagString += item.tagDisplay + ",";
            }
            tagString = tagString.TrimEnd(',');

            return Content(tagString);
        }


        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return null;
            }
            int objectID = 0;
            int blogID = Convert.ToInt32(UserInfo.BlogID);
            blog_tb_article model = new blog_tb_article { blogID = blogID, ADD_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now, articleDatetime = DateTime.Now };
            if ((!String.IsNullOrEmpty(id)) && id.ToString() != "0")
            {
                //model = Utility.ArticleBll.GetEntity(id);
                objectID = Convert.ToInt32(id);
                model = Utility.ArticleBll.GetEntity(id);
                string tagString = "";
                foreach (var item in Utility.ArticleBll.GetArticleTags(id))
                {
                    tagString += item.tagDisplay + ",";
                }
                tagString = tagString.TrimEnd(',');
                ViewData["tagString"] = tagString;
                model.UPDATE_DATE = DateTime.Now;
                blog_tb_article_content contentModel = Utility.ArticleBll.GetArticleContent(id);
                //加载临时正文
                string tempContent = Utility.ArticleBll.ReadTempContent(id);
                if (!String.IsNullOrEmpty(tempContent))
                {
                    //contentModel.articleContent = tempContent;
                }
                ViewData["content"] = contentModel;
            }
            else
            {
                string lastArticleID = Utility.ArticleBll.LoadLastArticleID();
                if (!String.IsNullOrEmpty(lastArticleID))
                {
                    objectID = Convert.ToInt32(lastArticleID);
                    string tempContent = Utility.ArticleBll.ReadTempContent(lastArticleID);
                    blog_tb_article_content contentModel = new blog_tb_article_content();
                    contentModel.articleContent = tempContent;
                    contentModel.UPDATE_DATE = DateTime.Now;
                    ViewData["content"] = contentModel;
                }
                else
                {
                    objectID = Convert.ToInt32(Utility.ArticleBll.NewID());
                }
            }

            string userID = UserInfo.UserID;

            var categorys = Utility.CategoryBll.GetTreeData(UserInfo.BlogID);
            var topics = Utility.TopicBll.GetTopic(blogID + "");
            var tags = Utility.TagBll.GetTags(blogID + "");
            var siteCategorys = Utility.BlogBll.QuerySiteCategory();
            ViewData["categorySelect"] = new SelectList(categorys, "categoryID", "TextWithTreeSpace");
            ViewData["TopicSelect"] = new SelectList(topics, "topicID", "topicDisplay");
            ViewData["TagSelect"] = new SelectList(tags, "tagID", "tagDisplay");
            ViewData["siteCategorySelect"] = new SelectList(siteCategorys, "ID", "Display");
            if (tags.Count() > 0)
            {

            }

            ViewData["objectID"] = objectID;

            ArticleViewModel m = ObjectHelper.CloneProperties<ArticleViewModel>(model);
            m.IsDisableComment = Utility.CommentBll.isDisableComment(model.articleCommentLimit);
            m.IsDisabledAnonymouComment = Utility.CommentBll.isDisabledAnonymousComment(model.articleCommentLimit);
            m.IsVerifyComment = Utility.CommentBll.isVerifyComment(model.articleCommentLimit);

            IList<blog_attachment> filelist = Utility.ArticleBll.GetFileRelation(id, "attachment");
            if (filelist.Count > 0)
            {
                List<Models.Attachment> atts = new List<Models.Attachment>();
                foreach (var v in filelist)
                {
                    atts.Add(new Models.Attachment() { fileUrl = v.fileUrl, fileSize = v.fileSize });
                }

                m.AttachmentCollection = atts;
            }

            //DataTable dt = FYJ.Common.JsonHelper.JsonToDataTable(json);
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        str += "<p><a href='" + dr["fileUrl"] + "' target='_blank'>" + dr["fileName"] + "</a></p>";
            //    }
            //}

            if (Request["v"] == "simple")
            {
                return View("~/Views/" + Utility.Version + "/Article/Edit-simple.cshtml", m);
            }
            else if (Request["v"] == "property")
            {
                return View("~/Views/" + Utility.Version + "/Article/Edit-property.cshtml", m);
            }

            return View("~/Views/" + Utility.Version + "/Article/Edit.cshtml", m);
        }

        [HttpPost]
        public JsonResult Edit(string id, FormCollection collection)
        {
            int articleID = Convert.ToInt32(Request["objectID"]);
            string userID = UserInfo.UserID;
            ArticleViewModel m = new ArticleViewModel();
            ObjectHelper.UpdateModel(m, collection);
            CommentLimit articleCommentLimit = 0;
            if (m.IsDisableComment)
            {
                articleCommentLimit |= CommentLimit.禁止回复;
            }
            if (m.IsVerifyComment)
            {
                articleCommentLimit |= CommentLimit.需要审核;
            }
            if (m.IsDisabledAnonymouComment)
            {
                articleCommentLimit |= CommentLimit.禁止匿名用户回复;
            }

            AttachmentLimit attachmentLimit = 0;
            if (!String.IsNullOrEmpty(Request["chkAttachmentLimit"]))
            {
                if (Request["chkAttachmentLimit"].Contains("1"))
                {
                    attachmentLimit |= AttachmentLimit.禁止未登录用户下载;
                }
                if (Request["chkAttachmentLimit"].Contains("2"))
                {
                    attachmentLimit |= AttachmentLimit.禁止未回复用户下载;
                }
                if (Request["chkAttachmentLimit"].Contains("3"))
                {
                    attachmentLimit |= AttachmentLimit.禁止下载;
                }
            }

            blog_tb_article model = new blog_tb_article();

            if (String.IsNullOrEmpty(id) || id == "0")  //新增
            {
                //UpdateModel(model); 
                ObjectHelper.UpdateModel(model, collection);
                model.articleID = articleID;
                model.ADD_DATE = DateTime.Now;
                model.UPDATE_DATE = DateTime.Now;
                Blogs.Entity.blog_tb_article_content contentModel = new blog_tb_article_content();
                contentModel.blogID = model.blogID;
                contentModel.UPDATE_DATE = DateTime.Now;
                ObjectHelper.UpdateModel(contentModel, collection);
                contentModel.articleID = articleID;

                model.articleCommentLimit = (int)articleCommentLimit;
                model.articleAttachmentLimit = (int)attachmentLimit;
                model.siteCategoryID = 33927842;
                model.blogID = Convert.ToInt32(UserInfo.BlogID);
                Utility.ArticleBll.Insert(model, contentModel);
            }
            else
            {
                model = Utility.ArticleBll.GetEntity(articleID + "");
                ObjectHelper.UpdateModel(model, collection);
                model.UPDATE_DATE = DateTime.Now;
                Blogs.Entity.blog_tb_article_content contentModel = Utility.ArticleBll.GetArticleContent(id);
                contentModel.blogID = model.blogID;
                ObjectHelper.UpdateModel(contentModel, collection);

                model.articleCommentLimit = (int)articleCommentLimit;
                model.articleAttachmentLimit = (int)attachmentLimit;
                Utility.ArticleBll.Update(model, contentModel);
            }

            //处理标签  必须后处理 因为之前还没有插入articleID 
            if (!String.IsNullOrEmpty(Request["txt_tag"]))
            {
                Utility.TagBll.UpdateTag(Request["blogID"], articleID + "", Request["txt_tag"].TrimEnd(','));
            }

            return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Delete(FormCollection collection)
        {
            Utility.ArticleBll.Delete(collection["ids"]);
            //ServiceFileReference.ServiceFileClient client = new ServiceFileReference.ServiceFileClient();
            //client.DeleteObjectID(collection["ids"]);
            return Json(new { code = 1, message = "删除成功" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateCatelog(FormCollection collection)
        {
            Utility.ArticleBll.CreateCatelog(collection["ids"]);
            return Json(new { code = 1, message = "生成目录成功" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Preview(string id)
        {
            blog_tb_article article = Utility.ArticleBll.GetEntity(id + "");
            string content = Utility.ArticleBll.GetArticleContent(article.articleID + "").articleContent;
            string reg = "<\\s*?img.+?(src=\"(http://img.+?)\")";
            foreach (Match m in Regex.Matches(content, reg))
            {
                string src = m.Groups[1].Value;
                string img = m.Groups[2].Value;
                content = content.Replace(src, " src=\"http://static.kecq.com/images/common/big-loading.gif\" data-original=\"" + img + "\"");
            }

            reg = "<\\s*?img.+?(src='(http://img.+?)')";
            foreach (Match m in Regex.Matches(content, reg))
            {
                string src = m.Groups[1].Value;
                string img = m.Groups[2].Value;
                content = content.Replace(src, " src=\"http://static.kecq.com/images/common/big-loading.gif\" data-original=\"" + img + "\"");
            }
            ArticlePreviewViewModel model = new ArticlePreviewViewModel();
            model.Article = article;
            IList<blog_attachment> filelist = Utility.ArticleBll.GetFileRelation(id, "attachment");
            foreach (var v in filelist)
            {
                content += "<p><a href=\"" + v.fileUrl + "\">" + v.fileName + "</a></p>";
            }

            model.Content = content;
            return View("~/Views/" + Utility.Version + "/Article/Preview.cshtml", model);
        }

        [HttpPost]
        public JsonResult SaveTempContent(FormCollection collection)
        {
            string articleID = Request["objectID"];
            string articleContent = Request["articleContent"];
            Utility.ArticleBll.SaveTempContent(articleID, articleContent);
            return Json(new { code = 1, message = DateTime.Now.ToString("HH:mm:ss") + "草稿保存成功" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ueditorEdit(int id)
        {
            var model = Utility.ArticleBll.GetEntity(id + "");
            blog_tb_article_content contentModel = Utility.ArticleBll.GetArticleContent(id + "");
            //加载临时正文
            string tempContent = Utility.ArticleBll.ReadTempContent(id + "");
            if (!String.IsNullOrEmpty(tempContent))
            {
                contentModel.articleContent = tempContent;
            }
            ViewData["content"] = contentModel;
            ArticleViewModel m = ObjectHelper.CloneProperties<ArticleViewModel>(model);
            return View("~/Views/" + Utility.Version + "/Article/ueditorEdit.cshtml", m);
        }

        public ActionResult Photos()
        {
            PhotoListViewModel model = new PhotoListViewModel();
            string articleID = Request["articleID"];
            IList<blog_attachment> filelist = Utility.ArticleBll.GetArticlePhotos(articleID);

            model.PhotoCollection = new List<blog_tb_Photo>();
            foreach (var v in filelist)
            {
                Entity.blog_tb_Photo p = new blog_tb_Photo();
                p.AlbumID = articleID;
                p.ThumbUrl = v.fileThumbUrl;
                p.Url = v.fileUrl;
                if (String.IsNullOrEmpty(p.ThumbUrl))
                {
                    p.ThumbUrl = p.Url;
                }
                model.PhotoCollection.Add(p);
            }

            return View("~/Views/" + Utility.Version + "/Article/Photos.cshtml", model);
        }

        public JsonResult SetFirstPic()
        {
            string articleID = Request["articleID"];
            string articlePic = Server.UrlDecode(Request["articlePic"]);
            string articleThumbPic = Server.UrlDecode(Request["articleThumbPic"]);
            Utility.ArticleBll.Update(articleID, "articlePic", articlePic);
            Utility.ArticleBll.Update(articleID, "articleThumbPic", articleThumbPic);
            return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
        }
    }
}
