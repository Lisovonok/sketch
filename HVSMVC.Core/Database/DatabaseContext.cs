using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using MVCBlog.Core.Entities;

namespace MVCBlog.Core.Database
{
    public class DatabaseContext : DbContext, IRepository
    {

        public DatabaseContext()
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public IDbSet<BlogEntry> BlogEntries { get; set; }

        public IDbSet<BlogEntryComment> BlogEntryComments { get; set; }

        public IDbSet<BlogEntryPingback> BlogEntryPingbacks { get; set; }

        public IDbSet<BlogEntryFile> BlogEntryFiles { get; set; }

        public IDbSet<Image> Images { get; set; }

        public IDbSet<Tag> Tags { get; set; }

        public IDbSet<FeedStatistic> FeedStatistics { get; set; }

        public override async Task<int> SaveChangesAsync()
        {
            foreach (var entity in ChangeTracker.Entries<EntityBase>().Where(e => e.State == EntityState.Modified))
            {
                entity.Entity.Modified = DateTime.Now;
            }

            try
            {
                return await base.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                var errors = ex.EntityValidationErrors
                        .Where(e => !e.IsValid)
                        .Select(e => e.Entry.Entity.GetType().Name + " - Errors: " + string.Join(", ", e.ValidationErrors.Select(v => v.PropertyName + ": " + v.ErrorMessage)));

                string errorText = string.Join("\r\n", errors);
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
