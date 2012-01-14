using System.Web;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle;

namespace AD.IoC.Windsor
{
    public class HybridLifeStyleManager : ILifestyleManager
    {
        private readonly ILifestyleManager _currentLifestyleManager;

        public HybridLifeStyleManager()
        {
            if (HttpContext.Current != null)
            {
                _currentLifestyleManager = new PerWebRequestLifestyleManager();
            }
            else
            {
                _currentLifestyleManager = new PerThreadLifestyleManager();
            }
        }

        public void Init(IComponentActivator componentActivator, IKernel kernel, ComponentModel model)
        {
            _currentLifestyleManager.Init(componentActivator, kernel, model);
        }

        public object Resolve(CreationContext context)
        {
            return _currentLifestyleManager.Resolve(context);
        }

        public bool Release(object instance)
        {
            return _currentLifestyleManager.Release(instance);
        }

        public void Dispose()
        {
            _currentLifestyleManager.Dispose();
        }
    }
}
