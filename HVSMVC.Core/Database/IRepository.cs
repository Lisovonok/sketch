using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using MVCBlog.Core.Entities;

namespace MVCBlog.Core.Database
{
    public interface IRepository
    {
        IDbSet<BlogEntry> BlogEntries { get; }

        IDbSet<BlogEntryComment> BlogEntryComments { get; }

        IDbSet<BlogEntryPingback> BlogEntryPingbacks { get; }

        IDbSet<BlogEntryFile> BlogEntryFiles { get; }

        IDbSet<Image> Images { get; }

        IDbSet<Tag> Tags { get; }

        IDbSet<FeedStatistic> FeedStatistics { get; }

        DbSet<T> Set<T>() where T : class;

        DbEntityEntry Entry(object entity);

        Task<int> SaveChangesAsync();
    }
}
