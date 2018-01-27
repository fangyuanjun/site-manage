using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.UI.Manage
{
    public class ArticleViewModel
    {
        public int articleID { get; set; }
        [Display(Name = "所属博客")]
        public int blogID { get; set; }

        [Required]
        [Display(Name = "系统分类")]
        public int siteCategoryID { get; set; }

        [Required]
        [Display(Name = "个人分类")]
        public int categoryID { get; set; }

        [Display(Name = "所属专题")]
        public int topicID { get; set; }
        [Required]
        [Display(Name = "标题")]
        public string articleTitle { get; set; }

        [Display(Name = "标题颜色")]
        public string articleTitleColor { get; set; }

        [Display(Name = "关键字")]
        public string articleKeywords { get; set; }

        [Display(Name = "描述")]
        public string articleDescription { get; set; }

        [Display(Name = "来源")]
        public string articleSource { get; set; }

        [Display(Name = "作者")]
        public string articleAuthor { get; set; }

        [Display(Name = "来源Url")]
        public string articleSourceUrl { get; set; }

        [Display(Name = "图片")]
        public string articlePic { get; set; }

        [Display(Name = "缩略图片")]
        public string articleThumbPic { get; set; }

        [Required]
        [Display(Name = "显示时间")]
        public DateTime articleDatetime { get; set; }

        [Display(Name = "跳转Url")]
        public string articleRedirectUrl { get; set; }

        [Display(Name = "添加用户")]
        public string articleAddUserID { get; set; }

        [Display(Name = "修改用户")]
        public string articleModifyUserID { get; set; }

        [Required]
        [Display(Name = "排序")]
        public int articleOrder { get; set; }

        [Required]
        [Display(Name = "点击次数")]
        public int articleClickTimes { get; set; }

        [Display(Name = "评论次数")]
        public int articleCommentTimes { get; set; }
        [Required]
        [Display(Name = "回复次数")]
        public int articleReplyCount { get; set; }
        [Required]
        [Display(Name = "是否置顶")]
        public bool articleIsTop { get; set; }

        [Required]
        [Display(Name = "是否隐藏")]
        public bool articleIsHidden { get; set; }

        [Required]
        [Display(Name = "是否禁用")]
        public bool articleIsDisabled { get; set; }
        [Required]
        [Display(Name = "是否已索引")]
        public bool articleIsIndex { get; set; }
        [Required]
        [Display(Name = "是否原创")]
        public bool articleIsOriginal { get; set; }
        [Required]
        [Display(Name = "是否系统")]
        public bool articleIsSystem { get; set; }
        [Required]
        [Display(Name = "是否图片")]
        public bool articleIsPic { get; set; }
        [Required]
        [Display(Name = "是否已删除")]
        public bool articleIsDelete { get; set; }
        [Required]
        [Display(Name = "附件权限")]
        public int attachmentLimit { get; set; }

        [Display(Name = "访问密码")]
        public string articlePassword { get; set; }
        [Required]
        [Display(Name = "添加时间")]
        public DateTime ADD_DATE { get; set; }
        [Required]
        [Display(Name = "修改时间")]
        public DateTime UPDATE_DATE { get; set; }

        [Display(Name = "主题")]
        public string themeID { get; set; }

        [Display(Name = "显示来源")]
        public bool IsShowSource { get; set; }

        #region 回复限制
        [Required]
        [Display(Name = "回复需验证")]
        public bool IsVerifyComment { get; set; }

        [Required]
        [Display(Name = "禁止回复")]
        public bool IsDisableComment { get; set; }

        [Required]
        [Display(Name = "禁止匿名用户回复")]
        public bool IsDisabledAnonymouComment { get; set; }

        [Display(Name = "回复权限限制")]
        public int articleCommentLimit { get; set; }
        #endregion

        [Display(Name = "附件权限限制")]
        public int articleAttachmentLimit { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public IEnumerable<Models.Attachment> AttachmentCollection { get; set; } = new List<Models.Attachment>();
    }
}
