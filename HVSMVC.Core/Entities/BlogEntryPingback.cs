using System;
using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Core.Entities
{
    public class BlogEntryPingback : EntityBase
    {
        [Required]
        public string Homepage { get; set; }

        [Required]
        public Guid? BlogEntryId { get; set; }

        public virtual BlogEntry BlogEntry { get; set; }
    }
}
