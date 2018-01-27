using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.UI.Manage
{
   public class TopicViewModel
    {
        public int topicID { get; set; }
        public int blogID { get; set; }
        public string topicName { get; set; }

        [Required]
        [Display(Name = "主题名称")]
        public string topicDisplay { get; set; }
        [Required]
        [Display(Name = "添加时间")]
        public System.DateTime ADD_DATE { get; set; }
        [Required]
        [Display(Name = "修改时间")]
        public System.DateTime UPDATE_DATE { get; set; }

    }
}
