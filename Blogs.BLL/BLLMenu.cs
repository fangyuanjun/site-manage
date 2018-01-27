
using Blogs.Entity;
using Blogs.IDAL;
using FYJ.Framework.Core.BLL;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;


namespace Blogs.BLL
{
    public class BLLMenu : BLLAbstract<Blogs.Entity.blog_tb_menu, IDALMenu>, IBLL.IBLLMenu
    {

        #region 选择树
        private String DeepSpace(int treeDeepIndex)
        {
            String str = "";
            for (int i = 0; i < treeDeepIndex; i++)
            {
                str += "　";//全角空格
            }

            return str;
        }


        public List<blog_tb_menu> GetTreeData(string blogID)
        {
            List<blog_tb_menu> list = Dal.GetList(blogID, null);
            List<blog_tb_menu> result = new List<blog_tb_menu>();
            foreach (var item in list)
            {
                if (item.menuPic == null || item.menuPic == "")
                {
                    item.menuPic = "http://static.kecq.com/images/common/nopic.jpg";
                }

                if (String.IsNullOrEmpty(item.parentID) || item.parentID == "0")
                {
                    item.treeDeep = 0;
                    item.TextWithTreeSpace = item.menuDisplay;
                    result.Add(item);
                    if (item.rowcount > 0)
                    {
                        LoadTreeData(item, list, result);
                    }
                }
            }

            return result;
        }

        private void LoadTreeData(blog_tb_menu parent, List<blog_tb_menu> total, List<blog_tb_menu> result)
        {
            foreach (var item in total.Where(x => x.parentID == parent.menuID))
            {
                item.treeDeep = parent.treeDeep + 1;
                item.TextWithTreeSpace = DeepSpace(item.treeDeep) + item.menuDisplay;
                result.Add(item);
                if (item.rowcount > 0)
                {
                    LoadTreeData(item, total, result);
                }
            }
        }
        #endregion


        public List<blog_tb_menu> GetAllList(string blogID)
        {
            List<blog_tb_menu> list = Dal.GetList(blogID, null);
            List<blog_tb_menu> result = new List<blog_tb_menu>();
            foreach (var item in list)
            {
                if (item.menuPic == null || item.menuPic == "")
                {
                    item.menuPic = "http://static.kecq.com/images/common/nopic.jpg";
                }

                if (String.IsNullOrEmpty(item.parentID) || item.parentID == "0")
                {
                    result.Add(item);
                    if (item.rowcount > 0)
                    {
                        LoadTreeGridData(item, list);
                    }
                }
            }

            return result;
        }

        private void LoadTreeGridData(blog_tb_menu parent, List<blog_tb_menu> total)
        {
            parent.children = total.Where(x => x.parentID == parent.menuID).ToArray();
            foreach (var item in parent.children)
            {
                if (item.rowcount > 0)
                {
                    LoadTreeGridData(item, total);
                }
            }
        }
    }
}
