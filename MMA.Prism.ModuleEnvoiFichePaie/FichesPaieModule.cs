using MMA.Prism.ModuleEnvoiFichePaie.MVVM.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA.Prism.ModuleEnvoiFichePaie
{
    public class FichesPaieModule : IModule
    {
        private IRegionManager _regionManager;

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
            // On initialise les region qui s'affichent dès l'ouverture de l'application
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(EnvoiFichePaieView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }

        #endregion
    }
}
