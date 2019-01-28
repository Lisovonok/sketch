using System;
using System.ComponentModel.DataAnnotations;

namespace MVCBlog.Core.Entities
{
    public abstract class EntityBase
    {
        public EntityBase()
        {
            Id = Guid.NewGuid();
            Created = DateTime.Now;
        }

        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}
