using System;
using System.Collections.Generic;
using System.Linq;
using MVCBlog.Core.Entities;

namespace MVCBlog.Website.Models.OutputModels.Administration
{

    public class Downloads
    {
        public IEnumerable<BlogEntry> BlogEntries { get; set; }
        public IEnumerable<IGrouping<DateTime, FeedStatistic>> FeedStatistics { get; set; }
    }
}
