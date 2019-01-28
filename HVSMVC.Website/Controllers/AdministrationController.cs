using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MVCBlog.Core.Commands;
using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;
using MVCBlog.Website.Models.OutputModels.Administration;

namespace MVCBlog.Website.Controllers
{
    [Authorize]
    public partial class AdministrationController : Controller
    {
        private readonly IRepository repository;
        private readonly ICommandHandler<AddBlogEntryCommand> addBlogEntryCommandHandler;
        private readonly ICommandHandler<UpdateBlogEntryCommand> updateBlogEntryCommandHandler;

           public AdministrationController(
            IRepository repository,
            ICommandHandler<AddBlogEntryCommand> addBlogEntryCommandHandler,
            ICommandHandler<UpdateBlogEntryCommand> updateBlogEntryCommandHandler)
        {
            this.repository = repository;
            this.addBlogEntryCommandHandler = addBlogEntryCommandHandler;
            this.updateBlogEntryCommandHandler = updateBlogEntryCommandHandler;
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        public async virtual Task<ActionResult> Statistics()
        {
            var minDate = DateTime.Now.Subtract(new TimeSpan(60, 0, 0, 0));

            var feedStatistics = await repository.FeedStatistics
                    .AsNoTracking()
                    .OrderBy(f => f.Application)
                    .ToArrayAsync();

            var model = new Downloads()
            {
                BlogEntries = await repository.BlogEntries
                    .AsNoTracking()
                    .OrderByDescending(f => f.PublishDate)
                    .ToArrayAsync(),

                FeedStatistics = feedStatistics
                    .GroupBy(f => f.Created.Date)
                    .OrderBy(f => f.Key)
            };

            return View(model);
        }

        public async virtual Task<ActionResult> EditBlogEntry(Guid? id)
        {
            var blogEntry = id.HasValue ? await repository.BlogEntries
                    .Include(b => b.Tags)
                    .AsNoTracking()
                .SingleAsync(b => b.Id == id.Value) : new BlogEntry()
                {
                    PublishDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0)
                };

            var model = new EditBlogEntry()
            {
                BlogEntry = blogEntry,
                Tags = await repository.Tags.AsNoTracking().OrderBy(t => t.Name).ToArrayAsync(),
                IsUpdate = id.HasValue
            };

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async virtual Task<ActionResult> EditBlogEntry(Guid? id, [Bind(Include = "Header, Author, ShortContent, Content, Visible")]BlogEntry blogEntry, FormCollection formValues)
        {
            if (!ModelState.IsValid)
            {
                return View(new EditBlogEntry()
                {
                    BlogEntry = blogEntry,
                    Tags = await repository.Tags.AsNoTracking().OrderBy(t => t.Name).ToArrayAsync(),
                    IsUpdate = id.HasValue
                });
            }

            if (id.HasValue)
            {
                var existingBlogEntry = await repository.BlogEntries
                    .Include(b => b.Tags)
                    .SingleAsync(b => b.Id == id.Value);
                existingBlogEntry.Header = blogEntry.Header;
                existingBlogEntry.Author = blogEntry.Author;
                existingBlogEntry.ShortContent = blogEntry.ShortContent;
                existingBlogEntry.Content = blogEntry.Content;
                existingBlogEntry.PublishDate = blogEntry.PublishDate;
                existingBlogEntry.Visible = blogEntry.Visible;

                blogEntry = existingBlogEntry;
            }

            blogEntry.PublishDate = DateTime.Parse(formValues["PublishDate"]);

            var tags = formValues.AllKeys.Where(k => k.StartsWith("Tag") && !string.IsNullOrEmpty(formValues[k])).Select(k => formValues[k]);

            if (id.HasValue)
            {
                await updateBlogEntryCommandHandler.HandleAsync(new UpdateBlogEntryCommand()
                {
                    Entity = blogEntry,
                    Tags = tags
                });
            }
            else
            {
                await addBlogEntryCommandHandler.HandleAsync(new AddBlogEntryCommand()
                {
                    Entity = blogEntry,
                    Tags = tags
                });
            }

            return RedirectToAction(MVC.Administration.EditBlogEntry(blogEntry.Id).Result);
        }
    }
}
