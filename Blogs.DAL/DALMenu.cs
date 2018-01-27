
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using FYJ.Data.Entity;

namespace Blogs.DAL
{
    public class DALMenu : FYJ.Framework.Core.DAL.DALAbstract<Blogs.Entity.blog_tb_menu>, Blogs.IDAL.IDALMenu
    {
        protected override string PrimaryKey
        {
            get { return "menuID"; }
        }

       int treeDeepIndex = 1;
       private String DeepSpace()
       {
           String str = "";
           for (int i = 0; i < treeDeepIndex; i++)
           {
               str += "　";//全角空格
           }

           return str;
       }

       private void AddDisplayTree(String pid, DataTable newTable, DataTable dt)
       {
           DataRow[] drs = dt.Select("parentID='" + pid + "'");
           foreach (DataRow dr in drs)
           {
               DataRow newRow = newTable.NewRow();
               newRow["menuID"] = dr["menuID"].ToString();
               newRow["menuDisplay"] = DeepSpace() + dr["menuDisplay"].ToString();
               newTable.Rows.Add(newRow); //一定要在下句的前面
               if (Convert.ToInt32(dr["cc"]) > 0)
               {
                   treeDeepIndex++;
                   AddDisplayTree(dr["menuID"].ToString(), newTable, dt);
               }
           }
       }

       public DataTable GetTreeTable(string blogID)
       {
           treeDeepIndex = 1;
           string sql = "select menuID,menuDisplay,parentID,(select count(1) from blog_tb_menu where parentID=table1.menuID ) as cc from blog_tb_menu as table1 where  blogID=@blogID order by menuOrder desc ";
           DataTable dt = DbInstance.GetDataTable(sql,DbInstance.CreateParameter("@blogID",blogID));
           DataTable newTable = dt.Clone();
           foreach (DataRow dr in dt.Rows)
           {
               DataRow newRow = newTable.NewRow();
               newRow["menuID"] = dr["menuID"].ToString();
               newRow["menuDisplay"] = dr["menuDisplay"].ToString();
               if (dr["parentID"].ToString() == "" || dr["parentID"].ToString() == "0")
               {
                   newTable.Rows.Add(newRow); //一定要在下句的前面
                   if (Convert.ToInt32(dr["cc"]) > 0)
                   {
                       AddDisplayTree(dr["menuID"].ToString(), newTable, dt);
                   }
               }
           }

           return newTable;
       }



       public DataTable GetMenuTable(string blogID)
       {
           string sql = "select *,(select count(1) from blog_tb_menu where parentID=table1.menuID ) as cc from blog_tb_menu as table1 where  blogID=@blogID order by menuOrder desc";
           DataTable dt = DbInstance.GetDataTable(sql,DbInstance.CreateParameter("@blogID",blogID));

           return dt;
       }
    }
}

