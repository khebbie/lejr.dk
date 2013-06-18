using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace lejr_dk
{
    public class UnityControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer _container;

        public UnityControllerFactory(IUnityContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            IController controller;

            if (controllerType == null)
            {
                throw new HttpException(404, string.Format("The controller for path '{0}' could not be found or it does not implement IController.", requestContext.HttpContext.Request.Path));
            }
            else
            {
                controller = _container.Resolve(controllerType) as IController;
            }

            return controller;
        }
    }

}