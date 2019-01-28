using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.IO;

namespace MVCBlog.Core.Entities
{
    public class Image : EntityBase
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [StringLength(4)]
        [Required]
        public string Extension { get; set; }

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

        [NotMapped]
        public string RelativePath
        {
            get
            {
                return ConfigurationManager.AppSettings["ImagesPath"] + Id.ToString() + "." + Extension;
            }
        }

        private string FullPath
        {
            get
            {
                var applicationPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                return Path.Combine(applicationPath, RelativePath);
            }
        }

        internal void DeleteData()
        {
            File.Delete(FullPath);
        }
    }
}
