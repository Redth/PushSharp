using System.Web.Http;
using Ninject;
using Ninject.Modules;

namespace PushSharp.Web.App_Start.IoC
{
    /// <summary>
    /// This class is to Register Ninject Modules and Resolve Dependencies
    /// </summary>
    public class NinjectWebApiContainer
    {
        private static NinjectHttpResolver _resolver;

        //Register Ninject Modules
        public static void RegisterModules(NinjectModule[] modules)
        {
            _resolver = new NinjectHttpResolver(modules);
            GlobalConfiguration.Configuration.DependencyResolver = _resolver;
        }

        //Manually Resolve Dependencies
        public static T Resolve<T>()
        {
            return _resolver.Kernel.Get<T>();
        }
    }
}