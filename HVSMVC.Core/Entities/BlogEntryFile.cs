using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.IO;

namespace MVCBlog.Core.Entities
{
    public class BlogEntryFile : EntityBase
    {
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [StringLength(4)]
        [Required]
        public string Extension { get; set; }

        public int Counter { get; set; }

        public Guid BlogEntryId { get; set; }

        public virtual BlogEntry BlogEntry { get; set; }

        [NotMapped]
        public byte[] Data
        {
            get
            {
                return File.ReadAllBytes(FullPath);
            }

            set
            {
                File.WriteAllBytes(FullPath, value);
            }
        }

        /// <summary>
        /// Gets the relative path.
        /// </summary>
        private string RelativePath
        {
            get
            {
                return ConfigurationManager.AppSettings["BlogEntryFilePath"] + this.Id.ToString() + "." + this.Extension;
            }
        }

        /// <summary>
        /// Gets the file path.
        /// </summary>
        private string FullPath
        {
            get
            {
                var applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                return Path.Combine(applicationPath, this.RelativePath);
            }
        }

        /// <summary>
        /// Deletes the corresponding file.
        /// </summary>
        internal void DeleteData()
        {
            File.Delete(this.FullPath);
        }
    }
}
