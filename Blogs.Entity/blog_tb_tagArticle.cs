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

    public partial class blog_tb_tagArticle : FYJ.Entity.EntityBase
    {
        public string tagarticleID { get; set; }
        public int tagID { get; set; }
        public int articleID { get; set; }
        public Nullable<System.DateTime> ADD_DATE { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
    
    }
}
