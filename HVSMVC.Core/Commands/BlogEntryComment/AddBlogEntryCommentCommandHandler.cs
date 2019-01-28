using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using MVCBlog.Core.Database;

namespace MVCBlog.Core.Commands
{
    public class AddBlogEntryCommentCommandHandler : ICommandHandler<AddBlogEntryCommentCommand>
    {
        private readonly IRepository repository;

        public AddBlogEntryCommentCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task HandleAsync(AddBlogEntryCommentCommand command)
        {
            this.repository.BlogEntryComments.Add(command.Entity);
            await this.repository.SaveChangesAsync();

            string subject = ConfigurationManager.AppSettings["CommentNotificationSubject"];
            
            var body = new StringBuilder();
            body.Append("Name: ");
            body.Append(command.Entity.Name);
            body.Append("\nComment: ");
            body.Append(command.Entity.Comment);
        }
    }
}
