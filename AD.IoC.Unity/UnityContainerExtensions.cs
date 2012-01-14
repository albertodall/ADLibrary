using Microsoft.Practices.Unity;

namespace AD.IoC.Unity
{
    public static class UnityContainerExtensions
    {
        /// <summary>
        /// Configura il container utilizzando il registry passato in ingresso.
        /// </summary>
        /// <param name="container">Container da configurare.</param>
        /// <param name="registry">Classe che definisce il Registry.</param>
        public static void ConfigureWithRegistry(this IUnityContainer container, IUnityContainerRegistry registry)
        {
            registry.Configure(container);
        }
    }
}
