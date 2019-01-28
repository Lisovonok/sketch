using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MVCBlog.Core.Resources;

namespace MVCBlog.Core.Entities
{
    public class BlogEntry : EntityBase
    {
        private string header;

        private DateTime publishDate;

        [StringLength(150)]
        [Required(ErrorMessage = "*")]
        [Display(Name = "HeaderLabel", ResourceType = typeof(Labels))]
        public string Header
        {
            get
            {
                return header;
            }

            set
            {
                header = value;
                UpdateHeaderUrl();
            }
        }

        [StringLength(160)]
        [Required(ErrorMessage = "*")]
        public string HeaderUrl { get; set; }

        [StringLength(100)]
        [Display(Name = "AuthorLabel", ResourceType = typeof(Labels))]
        public string Author { get; set; }

        [StringLength(1500)]
        [AllowHtml]
        [Required(ErrorMessage = "*")]
        [Display(Name = "ShortContentLabel", ResourceType = typeof(Labels))]
        public string ShortContent { get; set; }

        [AllowHtml]
       
        [Column(TypeName = "ntext")] 
        [Display(Name = "ContentLabel", ResourceType = typeof(Labels))]
        public string Content { get; set; }

        public int Visits { get; set; }

        [Display(Name = "VisibleLabel", ResourceType = typeof(Labels))]
        public bool Visible { get; set; }

        [Display(Name = "PublishDateLabel", ResourceType = typeof(Labels))]
        public DateTime PublishDate
        {
            get
            {
                return publishDate;
            }

            set
            {
                publishDate = value;
                UpdateHeaderUrl();
            }
        }

        public virtual ICollection<Tag> Tags { get; set; }

        public virtual ICollection<BlogEntryComment> BlogEntryComments { get; set; }

        public virtual ICollection<BlogEntryFile> BlogEntryFiles { get; set; }

        [NotMapped]
        public string Url
        {
            get
            {
                return Created.Year + "/" + Created.Month + "/" + Created.Day + "/" + HeaderUrl;
            }
        }

        private void UpdateHeaderUrl()
        {
            if (Header != null)
            {
                HeaderUrl = Regex.Replace(
                        Header.ToLowerInvariant().Replace(" - ", "-").Replace(" ", "-"),
                        "[^\\w^-]",
                        string.Empty);
            }
        }
    }
}
