using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MVCBlog.Core.Commands;
using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;
using MVCBlog.Website.Models.OutputModels.Blog;
using MvcSiteMapProvider.Web.Mvc.Filters;
using Lisovonok.Common.Linq;

namespace MVCBlog.Website.Controllers
{
    public partial class BlogController : Controller
    {
        private const int ENTRIESPERPAGE = 8;
        private readonly IRepository repository;
        private readonly ICommandHandler<AddBlogEntryCommentCommand> addBlogEntryCommentCommandHandler;
        private readonly ICommandHandler<DeleteCommand<BlogEntryComment>> deleteBlogEntryCommentCommandHander;
        private readonly ICommandHandler<DeleteBlogEntryCommand> deleteBlogEntryCommandHander;

        private readonly ICommandHandler<UpdateCommand<BlogEntry>> updateBlogEntryCommandHandler;
        
        public BlogController(
            IRepository repository,
            ICommandHandler<AddBlogEntryCommentCommand> addBlogEntryCommentCommandHandler,
            ICommandHandler<DeleteCommand<BlogEntryComment>> deleteBlogEntryCommentCommandHander,
            ICommandHandler<DeleteBlogEntryCommand> deleteBlogEntryCommandHander,
            ICommandHandler<UpdateCommand<BlogEntry>> updateBlogEntryCommandHandler)
        {
            this.repository = repository;

            this.addBlogEntryCommentCommandHandler = addBlogEntryCommentCommandHandler;
            this.deleteBlogEntryCommentCommandHander = deleteBlogEntryCommentCommandHander;
            this.deleteBlogEntryCommandHander = deleteBlogEntryCommandHander;
            this.updateBlogEntryCommandHandler = updateBlogEntryCommandHandler;
        }

        [ValidateInput(false)]
        public virtual ActionResult Index(string tag, string search, int? page)
        {
            var entries = GetAll(
                tag,
                search,
                new Paging(page.GetValueOrDefault(1), ENTRIESPERPAGE, PropertyResolver.GetPropertyName<BlogEntry>(b => b.PublishDate), SortDirection.Descending));

            var model = new IndexModel();
            model.Entries = entries.ToArray();
            model.TotalPages = (int)Math.Ceiling((double)entries.TotalNumberOfItems / ENTRIESPERPAGE);
            model.CurrentPage = page;
            model.Tag = tag;
            model.Search = search;

            return View(model);
        }

        [SiteMapTitle("Header")]
        public async virtual Task<ActionResult> Entry(string id)
        {
            var entry = await GetByHeader(id);

            if (!Request.IsAuthenticated)
            {
                entry.Visits++;
                await updateBlogEntryCommandHandler.HandleAsync(new UpdateCommand<BlogEntry>()
                {
                    Entity = entry
                });
            }

            return View(new BlogEntryDetail()
            {
                BlogEntry = entry,
                RelatedBlogEntries = await GetRelatedBlogEntries(entry)
            });
        }

        [SiteMapTitle("Header")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async virtual Task<ActionResult> Entry(string id, [Bind(Include = "Name, Homepage, Comment")] BlogEntryComment blogEntryComment)
        {
            var entry = await GetByHeader(id);

            if (entry == null)
            {
                return new HttpNotFoundResult();
            }

            ModelState.Remove("BlogEntryId");

            if (!ModelState.IsValid)
            {
                var errorModel = new BlogEntryDetail()
                {
                    BlogEntry = entry
                };

                if (Request.IsAjaxRequest())
                {
                    return PartialView(MVC.Blog.Views._CommentsControl, errorModel);
                }
                else
                {
                    errorModel.RelatedBlogEntries = await GetRelatedBlogEntries(entry);
                    return View(errorModel);
                }
            }

            blogEntryComment.AdminPost = Request.IsAuthenticated;
            blogEntryComment.BlogEntryId = entry.Id;
            entry.BlogEntryComments.Add(blogEntryComment);

            await addBlogEntryCommentCommandHandler.HandleAsync(new AddBlogEntryCommentCommand()
            {
                Entity = blogEntryComment
            });

            var model = new BlogEntryDetail()
            {
                BlogEntry = entry,
                HideNewCommentsForm = true
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView(MVC.Blog.Views._CommentsControl, model);
            }
            else
            {
                model.RelatedBlogEntries = await GetRelatedBlogEntries(entry);
                return View(model);
            }
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public virtual ActionResult Tags()
        {
            var tags = repository.Tags
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .ToArray();

            if (tags.Length > 0)
            {
                return PartialView(MVC.Shared.Views._TagListControl, tags);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public virtual ActionResult   PopularBlogEntries()
        {
            var blogEntries = repository.BlogEntries
                .AsNoTracking()
                .OrderByDescending(b => b.Visits)
                .Take(10)
                .ToArray();

            if (blogEntries.Length > 0)
            {
                return this.PartialView(MVC.Shared.Views._PopularBlogEntriesControl, blogEntries);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [Authorize]
        public async virtual Task<ActionResult> Delete(Guid id)
        {
            await this.deleteBlogEntryCommandHander.HandleAsync(new DeleteBlogEntryCommand()
            {
                Id = id
            });

            return this.RedirectToAction(MVC.Blog.Index());
        }

        [Authorize]
        public async virtual Task<ActionResult> DeleteComment(Guid id)
        {
            await deleteBlogEntryCommentCommandHander.HandleAsync(new DeleteCommand<BlogEntryComment>()
            {
                Id = id
            });

            return Redirect(Request.UrlReferrer.ToString());
        }


        private Task<BlogEntry> GetByHeader(string header)
        {
            var entry = repository.BlogEntries
                .Include(b => b.Tags)
                .Include(b => b.BlogEntryComments)
                .Include(b => b.BlogEntryFiles)
                .AsNoTracking()
                .Where(e => (e.Visible && e.PublishDate <= DateTime.Now) || Request.IsAuthenticated)
                .SingleOrDefaultAsync(e => e.HeaderUrl.Equals(header));

            return entry;
        }

        private PagedResult<BlogEntry> GetAll(string tag, string search, Paging paging)
        {
            var query = repository.BlogEntries
                .Include(b => b.BlogEntryComments)
                .AsNoTracking()
                .Where(e => (e.Visible && e.PublishDate <= DateTime.Now) || Request.IsAuthenticated);

            if (!string.IsNullOrEmpty(tag))
            {
                query = query.Where(e => e.Tags.Count(t => t.Name.Equals(tag, StringComparison.OrdinalIgnoreCase)) > 0);
            }

            if (!string.IsNullOrEmpty(search))
            {
                foreach (var item in search.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Where(e => e.Header.Contains(item));
                }
            }

            return query.GetPagedResult(paging);
        }

        private Task<BlogEntry[]> GetRelatedBlogEntries(BlogEntry entry)
        {
            var tagIds = entry.Tags.Select(t => t.Id).ToArray();

            var query = repository.BlogEntries
                .AsNoTracking()
                .Where(e => e.Visible && e.PublishDate <= DateTime.Now && e.Id != entry.Id)
                .Where(e => e.Tags.Any(t => tagIds.Contains(t.Id)))
                .OrderByDescending(e => e.Tags.Count(t => tagIds.Contains(t.Id)))
                .ThenByDescending(e => e.Created)
                .Take(5)
                .ToArrayAsync();

            return query;
        }
    }
}
