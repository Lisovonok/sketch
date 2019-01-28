using MVCBlog.Core.Entities;

namespace MVCBlog.Website.Models.OutputModels.Blog
{
    public class BlogEntryDetail
    {
        public BlogEntry BlogEntry { get; set; }
        public BlogEntry[] RelatedBlogEntries { get; set; }
        public string Header
        {
            get
            {
                return BlogEntry.Header;
            }
        }
        public bool HideNewCommentsForm { get; set; }
    }
}
