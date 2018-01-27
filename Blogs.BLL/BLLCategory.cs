
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Blogs.Entity;
using Blogs.IDAL;


namespace Blogs.BLL
{
    public class BLLCategory : FYJ.Framework.Core.BLL.BLLAbstract<blog_tb_category, IDALCategory>, IBLL.IBLLCategory
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


        public List<blog_tb_category> GetTreeData(string blogID)
        {
            List<blog_tb_category> list = Dal.GetList(blogID,null);
            List<blog_tb_category> result = new List<blog_tb_category>();
            foreach (var item in list)
            {
                if (item.parentID == 0)
                {
                    item.treeDeep = 0;
                    item.TextWithTreeSpace = item.categoryDisplay;
                    result.Add(item);
                    if (item.rowcount > 0)
                    {
                        LoadTreeData(item, list, result);
                    }
                }
            }

            return result;
        }

        private void LoadTreeData(blog_tb_category parent, List<blog_tb_category> total, List<blog_tb_category> result)
        {
            foreach (var item in total.Where(x => x.parentID == parent.categoryID))
            {
                item.treeDeep = parent.treeDeep + 1;
                item.TextWithTreeSpace = DeepSpace(item.treeDeep) + item.categoryDisplay;
                result.Add(item);
                if (item.rowcount > 0)
                {
                    LoadTreeData(item, total, result);
                }
            }
        }
        #endregion


        public List<blog_tb_category> GetAllList(string blogID)
        {
            List<blog_tb_category> list = Dal.GetList(blogID,null);
            List<blog_tb_category> result = new List<blog_tb_category>();
            foreach (var item in list)
            {
                if (item.parentID ==0)
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

        private void LoadTreeGridData(blog_tb_category parent, List<blog_tb_category> total)
        {
            parent.children = total.Where(x => x.parentID == parent.categoryID).ToArray();
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
