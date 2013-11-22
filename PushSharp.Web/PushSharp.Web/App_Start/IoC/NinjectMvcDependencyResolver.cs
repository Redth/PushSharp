using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Ninject.Modules;

namespace PushSharp.Web.App_Start.IoC
{
    public class NinjectMvcDependencyResolver : IDependencyResolver
    {
        public IKernel Kernel { get; private set; }
        public NinjectMvcDependencyResolver(params NinjectModule[] modules)
        {
            Kernel = new StandardKernel(modules);
        }
 
        public object GetService(Type serviceType)
        {
            return Kernel.TryGet(serviceType);
        }
 
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Kernel.GetAll(serviceType);
        }
    }
}