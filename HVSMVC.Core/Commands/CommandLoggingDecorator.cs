
using System.Threading.Tasks;
namespace MVCBlog.Core.Commands
{
    public class CommandLoggingDecorator<TCommand> : ICommandHandler<TCommand>
    {
        private readonly ICommandHandler<TCommand> handler;

        public CommandLoggingDecorator(ICommandHandler<TCommand> handler)
        {
            this.handler = handler;
        }

        public async Task HandleAsync(TCommand command)
        {
            await this.handler.HandleAsync(command);
        }
    }
}
