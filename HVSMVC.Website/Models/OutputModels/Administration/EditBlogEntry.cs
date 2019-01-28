using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBlog.Core.Entities;

namespace MVCBlog.Website.Models.OutputModels.Administration
{
    public class EditBlogEntry
    {
        public BlogEntry BlogEntry { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public bool IsUpdate { get; set; }

        public IHtmlString TagsAsJsonString
        {
            get
            {
                return MvcHtmlString.Create("[" + string.Join(",", Tags.Select(t => "\"" + t.Name + "\"").ToArray()) + "]");
            }
        }

        public string GetTagName(int index)
        {
            if (BlogEntry != null && BlogEntry.Tags != null && BlogEntry.Tags.Count > index)
            {
                return BlogEntry.Tags.ElementAt(index).Name;
            }
            else
            {
                return null;
            }
        }
    }
}
