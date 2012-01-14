using System.Web;
using Microsoft.Practices.Unity;

namespace AD.IoC.Unity
{
    /// <summary>
    /// Lifetime manager per Unity che memorizza le istanze degli oggetti nell'HttpContext in caso di applicazione web,
    /// altrimenti nel Thread corrente.
    /// </summary>
    public class HybridLifetimeManager : LifetimeManager
    {
        private readonly LifetimeManager _currentLifeTimeManager;

        public HybridLifetimeManager()
        {
            if (HttpContext.Current != null)
            {
                _currentLifeTimeManager = new HttpContextLifetimeManager();
            }
            else
            {
                _currentLifeTimeManager = new PerThreadLifetimeManager();
            }
        }

        public override object GetValue()
        {
            return _currentLifeTimeManager.GetValue();
        }

        public override void RemoveValue()
        {
            _currentLifeTimeManager.RemoveValue();
        }

        public override void SetValue(object newValue)
        {
            _currentLifeTimeManager.SetValue(newValue);
        }
    }
}
