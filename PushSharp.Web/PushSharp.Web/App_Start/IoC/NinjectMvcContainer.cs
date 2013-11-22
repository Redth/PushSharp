using System.Web.Mvc;
using Ninject;
using Ninject.Modules;

namespace PushSharp.Web.App_Start.IoC
{
    /// <summary>
    /// This class is to Register Ninject Modules and Resolve Dependencies for MVC controllers 
    /// </summary>
    public class NinjectMvcContainer
    {
        private static NinjectMvcDependencyResolver _resolver;
 
        public static void RegisterModules(NinjectModule[] modules)
        {
            _resolver = new NinjectMvcDependencyResolver(modules);
            DependencyResolver.SetResolver(_resolver);
        }
 
        public static T Resolve<T>()
        {
            return _resolver.Kernel.Get<T>();
        }
    }
}