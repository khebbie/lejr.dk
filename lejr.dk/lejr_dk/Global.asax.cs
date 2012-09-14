using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;
using lejr_dk.DataAccess;
using lejr_dk.Models;
using Microsoft.Practices.Unity;

namespace lejr_dk
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

            IUnityContainer unityContainer = BootStrapUnity();
            ControllerBuilder.Current.SetControllerFactory(new UnityControllerFactory(unityContainer));
            InitializeActiveRecord();
        }

        private static IUnityContainer BootStrapUnity()
        {
            IUnityContainer unityContainer = new UnityContainer();
            unityContainer.RegisterType<ICampService, CampService>();
            unityContainer.RegisterType<ICampDataAccess, CampDataAccess>();
            return unityContainer;
        }

        private static void InitializeActiveRecord()
        {
            IConfigurationSource config = ActiveRecordSectionHandler.Instance;
            ActiveRecordStarter.Initialize(config, typeof (Camp));
        }
    }
}