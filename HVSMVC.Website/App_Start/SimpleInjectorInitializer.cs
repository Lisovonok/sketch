using System.Reflection;
using System.Web.Mvc;
using MVCBlog.Core.Commands;
using MVCBlog.Core.Database;
using MVCBlog.Core.Entities;
using SimpleInjector;
using SimpleInjector.Extensions;
using SimpleInjector.Integration.Web.Mvc;

namespace MVCBlog.Website
{
    public static class SimpleInjectorInitializer
    {
        public static void Initialize()
        {
            var container = new Container();
            InitializeContainer(container);
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterMvcAttributeFilterProvider();
            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container)
        {
            container.RegisterPerWebRequest<IRepository, DatabaseContext>();

            container.Register<ICommandHandler<DeleteCommand<BlogEntryComment>>, DeleteCommandHandler<BlogEntryComment>>();
            container.Register<ICommandHandler<UpdateCommand<BlogEntry>>, UpdateCommandHandler<BlogEntry>>();
            container.Register<ICommandHandler<UpdateCommand<BlogEntryFile>>, UpdateCommandHandler<BlogEntryFile>>();

            container.RegisterManyForOpenGeneric(
                typeof(ICommandHandler<>),
                typeof(ICommandHandler<>).Assembly);

            container.RegisterDecorator(
                typeof(ICommandHandler<>),
                typeof(CommandLoggingDecorator<>));
        }
    }
}