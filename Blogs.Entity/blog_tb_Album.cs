using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FYJ;

namespace Blogs.Entity
{
    [TableInfoAttribute("blog_tb_Album", "ID","UserID")]
    public class blog_tb_Album : FYJ.Entity.EntityBase
    {

        ///<summary>
        /// 
        ///</summary>
        public String ID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String UserID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String Name { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String Display { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public Int32 Permission { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public DateTime ADD_DATE { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public DateTime UPDATE_DATE { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string CoverUrl{ get; set;}

        [Ignore]
        public int PhotoCount { get; set; }
    }
}