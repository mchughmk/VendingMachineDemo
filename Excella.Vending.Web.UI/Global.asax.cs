using Autofac;
using Autofac.Integration.Mvc;
using Excella.Vending.DAL;
using Excella.Vending.Domain;
using Excella.Vending.Machine;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Excella.Vending.Web.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RegisterDependencies();
        }

        protected void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            //Register project abstractions
            builder.RegisterType<VendingMachine>().As<IVendingMachine>();
            builder.RegisterType<CoinPaymentProcessor>().As<IPaymentProcessor>();
            builder.RegisterType<EFPaymentDAO>().As<IPaymentDAO>();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
