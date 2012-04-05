using System.Web;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle;

namespace AD.IoC.Windsor
{
    public class HybridLifeStyleManager : AbstractLifestyleManager
    {
        private readonly ILifestyleManager _currentLifestyleManager;

        public HybridLifeStyleManager()
        {
            if (HttpContext.Current != null)
            {
                _currentLifestyleManager = new ScopedLifestyleManager(new WebRequestScopeAccessor());
            }
            else
            {
                _currentLifestyleManager = new ScopedLifestyleManager(new ThreadScopeAccessor());
            }
        }

        public override void Init(IComponentActivator componentActivator, IKernel kernel, ComponentModel model)
        {
            _currentLifestyleManager.Init(componentActivator, kernel, model);
        }

        public override bool Release(object instance)
        {
            return _currentLifestyleManager.Release(instance);
        }

        public override object Resolve(CreationContext context, IReleasePolicy releasePolicy)
        {
            return _currentLifestyleManager.Resolve(context, releasePolicy);
        }

        public override void Dispose()
        {
            _currentLifestyleManager.Dispose();
        }
    }
}
