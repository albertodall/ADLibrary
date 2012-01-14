using System;
using System.Web;
using Microsoft.Practices.Unity;

namespace AD.IoC.Unity
{
    /// <summary>
    /// Lifetime manager per Unity che permette di memorizzare le istanze degli oggetti nell'HttpContext.
    /// </summary>
    public class HttpContextLifetimeManager : LifetimeManager
    {
        private readonly Guid _key = Guid.NewGuid();

        public override object GetValue()
        {
            EnsureHttpContext();
            return HttpContext.Current.Items[_key];
        }

        public override void RemoveValue()
        {
            EnsureHttpContext();
            HttpContext.Current.Items.Remove(_key);
        }

        public override void SetValue(object newValue)
        {
            EnsureHttpContext();
            HttpContext.Current.Items[_key] = newValue;
        }

        private static void EnsureHttpContext()
        {
            if (HttpContext.Current == null)
            {
                throw new HttpException("HttpContext non presente. Ci si trova in un contesto web?");
            }
        }
    }
}
