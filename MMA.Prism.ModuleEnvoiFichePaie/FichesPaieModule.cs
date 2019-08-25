using MMA.Prism.ModuleEnvoiFichePaie.MVVM.Views;
using NLog;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace MMA.Prism.ModuleEnvoiFichePaie
{
    public class FichesPaieModule : IModule
    {
        private IRegionManager _regionManager;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public FichesPaieModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        #region -- Concerning tab Control --

        /// <summary>
        /// -- Nouveau reservé prism 7.0.439  --
        /// </summary>
        /// <param name="containerProvider"></param>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _logger.Debug($"==> Initialisation de la region associée à la vue [EnvoiFichePaieView].");
            // On initialise les region qui s'affichent dès l'ouverture de l'application
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(EnvoiFichePaieView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }

        #endregion
    }
}
