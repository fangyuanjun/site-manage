//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Blogs.Entity
{
    using System;
    using System.Collections.Generic;

    public partial class blog_tb_commentSupport : FYJ.Entity.EntityBase
    {
        public string supportID { get; set; }
        public string commentID { get; set; }
        public string userID { get; set; }
        public string supportIP { get; set; }
        public System.DateTime supportDatetime { get; set; }
        public bool isSupport { get; set; }
        public System.DateTime UPDATE_DATE { get; set; }
    }
}
