using MVCBlog.Core.Entities;

namespace MVCBlog.Website.Models.OutputModels.Blog
{
    public class IndexModel
    {
        public BlogEntry[] Entries { get; set; }
        public int? CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Search { get; set; }
        public string Tag { get; set; }
    }
}