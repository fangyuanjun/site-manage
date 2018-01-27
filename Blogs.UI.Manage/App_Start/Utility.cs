using Blogs.IBLL;
using FYJ;
using FYJ.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Blogs.UI.Manage
{
    public static class Utility
    {
        public static IBLLBlog BlogBll
        {
            get { return IocFactory<IBLLBlog>.Instance; }
        }

        public static IBLLCategory CategoryBll
        {
            get { return IocFactory<IBLLCategory>.Instance; }
        }

        public static IBLLArticle ArticleBll
        {
            get { return IocFactory<IBLLArticle>.Instance; }
        }

        public static IBLLArticleView ArticleViewBll
        {
            get { return IocFactory<IBLLArticleView>.Instance; }
        }

        public static IBLLTopic TopicBll
        {
            get { return IocFactory<IBLLTopic>.Instance; }
        }


        public static IBLLTag TagBll
        {
            get { return IocFactory<IBLLTag>.Instance; }
        }


        public static IBLLComment CommentBll
        {
            get { return IocFactory<IBLLComment>.Instance; }
        }



        public static IBLLAlbum AlbumBll
        {
            get { return IocFactory<IBLLAlbum>.Instance; }
        }

        public static IBLLPhoto PhotoBll
        {
            get { return IocFactory<IBLLPhoto>.Instance; }
        }

        public static IBLLSlider SliderBll
        {
            get { return IocFactory<IBLLSlider>.Instance; }
        }

        public static IBLLMenu MenuBll
        {
            get { return IocFactory<IBLLMenu>.Instance; }
        }

        public static IBLLLink LinkBll
        {
            get { return IocFactory<IBLLLink>.Instance; }
        }


        public static IBLLTalk TalkBll
        {
            get { return IocFactory<IBLLTalk>.Instance; }
        }

        public static string Version
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["Version"];
            }
        }

        public static string UploadUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadUrl"];
            }
        }

        public static string Sign(string url)
        {
            url = url.Trim();
            if (!url.EndsWith("&"))
            {
                url = url + "&";
            }
            string reg = "[&|?](.*?)=(.*?)(?=&)";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (Match m in Regex.Matches(url, reg, RegexOptions.IgnoreCase))
            {
                string key = m.Groups[1].Value.Trim();
                string value = m.Groups[2].Value.Trim();
                //if(new string[]{}.Select(c=>c=="").Count()==0)
                //{

                //}
                dic.Add(key, value);
            }

            dic.Add("ak", System.Configuration.ConfigurationManager.AppSettings["appKey"]);
            dic.Add("time", DateTime.Now.ToString("yyyyMMddHHmmss"));

            string par = "";
            string sign = HttpHelper.Sign(dic, System.Configuration.ConfigurationManager.AppSettings["secretKey"], out par, "sign");
            if (url.IndexOf("?") > 0)
            {
                url = url.Substring(0, url.IndexOf("?"));
            }
            url = url + "?" + par + "&sign=" + sign;

            return url;
        }



        public static string GetModelValidateError(System.Web.Mvc.ModelStateDictionary ModelState)
        {
            string result = "";

            //获取所有错误的Key
            List<string> Keys = ModelState.Keys.ToList();
            //获取每一个key对应的ModelStateDictionary
            foreach (var key in Keys)
            {
                var errors = ModelState[key].Errors.ToList();
                //将错误描述添加到sb中
                foreach (var error in errors)
                {
                    result += error.ErrorMessage + Environment.NewLine;
                }
            }
            return result;
        }
    }
}