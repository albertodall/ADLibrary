using Microsoft.Practices.Unity;

namespace AD.IoC.Unity
{
    /// <summary>
    /// Interfaccia da implementare per definire un Container Configurator per Unity.
    /// </summary>
    public interface IUnityContainerRegistry
    {
        /// <summary>
        /// Metodo da chiamare per la configurazione del container.
        /// </summary>
        /// <param name="container">Container da configurare.</param>
        void Configure(IUnityContainer container);
    }
}
