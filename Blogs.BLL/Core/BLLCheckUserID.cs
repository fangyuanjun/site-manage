using FYJ;
using FYJ.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.BLL
{
    public static class BLLCheckUserID
    {
        private static IDbHelper DbInstance
        {
            get
            {
                return IocFactory<FYJ.Data.IDbFactory>.Instance.GetDbInstance("Blogs");
            }
        }

        /// <summary>
        /// 验证ids是否属于用户  如果不属于抛出异常
        /// </summary>
        /// <param name="modeltype"></param>
        /// <param name="ids"></param>
        public static void ValidateUserID(Type modeltype, string ids)
        {
            if(!IsIdsValidateUserID(modeltype,ids))
            {
                throw new CustomException("无权限");
            }
        }

        /// <summary>
        /// 验证ids是否属于用户  如果不属于抛出异常
        /// </summary>
        /// <param name="model"></param>
        public static void ValidateUserID(object model)
        {
            if (!IsModelValidateUserID(model))
            {
                throw new CustomException("无权限");
            }
        }


        public static bool IsIdsValidateUserID(Type modeltype,string ids)
        {
            if(String.IsNullOrEmpty(ids))
            {
                return true;
            }

            object[] tableAttributes = modeltype.GetCustomAttributes(typeof(TableInfoAttribute), true);
            if (tableAttributes.Length == 0)
            {
                throw new Exception("modeltype无TableInfoAttribute特性，不能验证");
            }

            TableInfoAttribute att = tableAttributes[0] as TableInfoAttribute;
           
            string tmp = "";
            foreach (string s in ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string t = s.Trim('\'');
                tmp += "'" + t + "',";
            }
            tmp = tmp.TrimEnd(',');

            string sql = "select " + att.UserIDName + "  from " + att.TableName + " where " + att.PrimaryName + "  in (" + tmp + ")";

            DataTable dt = DbInstance.GetDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                throw new CustomException("id不存在");
            }

            foreach (DataRow dr in dt.Rows)
            {
                //只要有一项的userID不等于参数userID 则认证失败
                if (dr[0].ToString() != IocFactory<IUserInfo>.Instance.GetUserID())
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsModelValidateUserID(object model)
        {
            if (model == null)
            {
                throw new Exception("验证model不能为空");
            }
            object[] tableAttributes = model.GetType().GetCustomAttributes(typeof(TableInfoAttribute), true);
            if (tableAttributes.Length == 0)
            {
                throw new Exception("model无TableInfoAttribute特性，不能验证");
            }

            TableInfoAttribute att = tableAttributes[0] as TableInfoAttribute;
            PropertyInfo primary = model.GetType().GetProperty(att.PrimaryName);
            if (primary == null)
            {
                throw new Exception("model无" + att.PrimaryName + "属性,不能验证");
            }

            object value = primary.GetValue(model, null);
            if (value == null)  //如果主键值为空  说明是新增  直接通过
            {
                return true;
            }

            string sql = "select " + att.UserIDName + "  from " + att.TableName + " where " + att.PrimaryName + "  =@" + att.PrimaryName;
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@" + att.PrimaryName, value));
            if (dt.Rows.Count == 0)
            {
                throw new CustomException("id不存在");
            }

            if (dt.Rows[0][0].ToString() == IocFactory<IUserInfo>.Instance.GetUserID())
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
