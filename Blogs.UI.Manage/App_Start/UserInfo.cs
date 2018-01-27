using Blogs.BLL;
using FYJ;
using FYJ.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Blogs.UI.Manage
{
    public class UserInfo:IUserInfo,IBlogInfo
    {
        /// <summary>
        /// 当前BlogID
        /// </summary>
        public static string BlogID
        {
            get
            {
                if (HttpContext.Current.Session["blog"] != null)
                {
                    Blogs.Entity.blog_tb_blog blog = (Blogs.Entity.blog_tb_blog)HttpContext.Current.Session["blog"];

                    return blog.blogID + "";
                }
                else
                {
                    Blogs.Entity.blog_tb_blog blog = Utility.BlogBll.GetSingleBlogByUserID(UserID);
                    return blog.blogID + "";
                }

                //throw new Exception("获取BlogID错误:没有登录或登录超时");
            }
        }

        /// <summary>
        /// 当前博客标题
        /// </summary>
        public static string BlogTitle
        {
            get
            {
                if (HttpContext.Current.Session["blog"] != null)
                {
                    Blogs.Entity.blog_tb_blog blog = (Blogs.Entity.blog_tb_blog)HttpContext.Current.Session["blog"];

                    return blog.blogTitle + "";
                }
                else
                {
                    Blogs.Entity.blog_tb_blog blog = Utility.BlogBll.GetSingleBlogByUserID(UserID);
                    return blog.blogTitle + "";
                }
            }
        }

        /// <summary>
        /// 当前AppName
        /// </summary>
        public static string AppName
        {
            get
            {
                if (HttpContext.Current.Session["blog"] != null)
                {
                    Blogs.Entity.blog_tb_blog blog = (Blogs.Entity.blog_tb_blog)HttpContext.Current.Session["blog"];

                    return blog.blogName;
                }
                else
                {
                    Blogs.Entity.blog_tb_blog blog = Utility.BlogBll.GetSingleBlogByUserID(UserID);
                    return blog.blogName;
                }
            }
        }

        /// <summary>
        /// 当前用户ID
        /// </summary>
        public static string UserID
        {
            //if (HttpContext.Current.Session["Token"] != null)
            //{
            //    JsonHelper json = new JsonHelper(HttpContext.Current.Session["Token"].ToString());
            //    string userID = json.GetValue("userID");

            //    return userID;
            //}

            //throw new Exception("获取UserID错误:没有登录或登录超时");

            get
            {
                if (HttpContext.Current.User != null)
                {
                    FormsIdentity identity = HttpContext.Current.User.Identity as FormsIdentity;
                    if (identity != null && identity.IsAuthenticated)
                    {
                        string userJson = identity.Name;
                        JObject jobj = Newtonsoft.Json.JsonConvert.DeserializeObject(userJson) as JObject;
                        string userID = jobj["userID"].ToString();
                        return userID;
                    }
                }

                return null;
                // throw new CustomException("没有登录或认证失败");
            }

            //get
            //{
            //    if (HttpContext.Current.Session["blog"] != null)
            //    {
            //        Blogs.Entity.blog_tb_blog blog = (Blogs.Entity.blog_tb_blog)HttpContext.Current.Session["blog"];

            //        return blog.userID;
            //    }
            //    else
            //    {
            //        Blogs.Entity.blog_tb_blog blog = Utility.BlogBll.GetFirstEntity();
            //        return blog.userID;
            //    }
            //}
        }


        public static string UserName
        {
            get
            {
                if (HttpContext.Current.User != null)
                {
                    FormsIdentity identity = HttpContext.Current.User.Identity as FormsIdentity;
                    if (identity.IsAuthenticated)
                    {
                        //if (HttpContext.Current.User.IsInRole("管理员"))
                        //{
                        //    return "管理员";
                        //}

                        //if (HttpContext.Current.User.IsInRole("会员"))
                        //{
                        //    return "会员";
                        //}

                        JObject v = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(identity.Name);
                        string userName = v["userName"].ToString();
                        return userName;
                    }
                }

                throw new CustomException("没有登录或认证失败");
            }
        }

        public string CurrentUserID
        {
            get
            {
                return UserInfo.UserID;
            }
        }

        public string CurrentBlogID
        {
            get
            {
                return UserInfo.BlogID;
            }
        }

        public string GetUserID()
        {
            return UserInfo.UserID;
        }
    }
}