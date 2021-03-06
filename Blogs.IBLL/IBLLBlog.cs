﻿using System;
using System.Collections.Generic;
using System.Data;
using Blogs.Entity;
using FYJ.Framework.Core;
using FYJ.Framework.Core.IBLL;

namespace Blogs.IBLL
{
    public interface IBLLBlog 
    {
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        blog_tb_blog GetEntity(string id);

        /// <summary>
        /// 获取数据库第一条博客
        /// </summary>
        /// <returns></returns>
        /// fangyj 2015-6-22
        blog_tb_blog GetFirstEntity();

        /// <summary>
        /// 获取用户的一个博客  (第一个)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>2015-07-25</returns>
        blog_tb_blog GetSingleBlogByUserID(string userID);

 
      

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Save(blog_tb_blog entity);

  
  
        /// <summary>
        /// 检查ID 是否是用户可以操作的
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        /// fangyj 2-015-5-13
        bool CheckID(string id, string userID);

        /// <summary>
        /// 根据域名获取博客信息  
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        blog_tb_blog GetSingleBlogByDomain(string domain,int port);

        /// <summary>
        /// 获取系统分类
        /// </summary>
        /// <returns></returns>
        List<blog_tb_SiteCategory> QuerySiteCategory();

        /// <summary>
        /// 更新统计信息
        /// </summary>
        /// <param name="blogID"></param>
        /// <param name="ip"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        /// 2015-8-13
        int UpdateBlogCount(string blogID,string ip,string sessionID);

        /// <summary>
        /// 获取总访问量
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        /// 2015-8-13
        int GetTotalPV(string blogID);

        /// <summary>
        /// 是否存在博客名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// 2015-8-22
        bool IsExistsBlogName(string name);


        /// <summary>
        /// 博客是否禁用
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        bool IsBlogDisabled(string blogID);

        /// <summary>
        /// 绑定域名
        /// </summary>
        /// <param name="blogID"></param>
        /// <param name="domain">域名</param>
        /// <param name="port">端口号</param>
        /// <returns></returns>
        bool AddDomain(string blogID, string domain, int port);

        bool UpdateDomain(string ID, string domain, int port);

        bool DeleteDomain(string ID);

        List<blog_tb_domain> GetDomainList(string blogID);
    }
}
