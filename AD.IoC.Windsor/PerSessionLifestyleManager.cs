using System;
using System.Web;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle;

namespace AD.IoC.Windsor
{
    /// <summary>
    /// Implements a Lifestyle manager for web apps that creates at most one object per http session.
    /// </summary>
    /// <remarks>
    /// Since the http session end event is not really reliable (it only fires with an InProc session provider) 
    /// there is no way to properly release any components with this lifestyle
    /// </remarks>
    public class PerWebSessionLifestyleManager : AbstractLifestyleManager
    {
        private readonly string _objectId = "PerWebSessionLifestyleManager_" + Guid.NewGuid();

        internal Func<HttpContextBase> ContextProvider { get; set; }

        public PerWebSessionLifestyleManager()
        {
            ContextProvider = () => new HttpContextWrapper(HttpContext.Current);
        }

        public override object Resolve(CreationContext context, IReleasePolicy releasePolicy)
        {
            var httpContext = ContextProvider();
            if (httpContext == null)
                throw new InvalidOperationException("HttpContext.Current is null. PerWebSessionLifestyle can only be used in ASP.NET");

            var session = httpContext.Session;
            if (session == null)
                throw new InvalidOperationException("ASP.NET session not found");

            if (session[_objectId] == null)
            {
                var instance = base.Resolve(context, releasePolicy);
                session[_objectId] = instance;
                return instance;
            }

            return session[_objectId];
        }

        public override void Dispose()
        {
            var current = ContextProvider();
            if (current == null)
            {
                return;
            }

            if (current.Session == null)
            {
                throw new InvalidOperationException("ASP.NET session not found");
            }

            var instance = current.Session[_objectId];
            if (instance == null)
            {
                return;
            }

            Kernel.ReleaseComponent(instance);
        }
    }

}
