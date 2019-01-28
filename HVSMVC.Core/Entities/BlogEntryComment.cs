using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MVCBlog.Core.Resources;

namespace MVCBlog.Core.Entities
{
    public class BlogEntryComment : EntityBase
    {
        [StringLength(50)]
        [Required(ErrorMessageResourceName = "Name", ErrorMessageResourceType = typeof(Validation))]
        [Display(Name = "NameLabel", ResourceType = typeof(Labels))]
        public string Name { get; set; }

        [StringLength(2500)]
        [Required(ErrorMessageResourceName = "Comment", ErrorMessageResourceType = typeof(Validation))]
        [AllowHtml]
        [Display(Name = "CommentLabel", ResourceType = typeof(Labels))]
        public string Comment { get; set; }

        [StringLength(50)]
        [Display(Name = "EmailLabel", ResourceType = typeof(Labels))]
        public string Email { get; set; }

        [StringLength(100)]
        [Display(Name = "HomepageLabel", ResourceType = typeof(Labels))]
        public string Homepage { get; set; }

        public bool AdminPost { get; set; }

        public Guid BlogEntryId { get; set; }

        public virtual BlogEntry BlogEntry { get; set; }
    }
}
