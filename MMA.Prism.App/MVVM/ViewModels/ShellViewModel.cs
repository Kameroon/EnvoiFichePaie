using MMA.Prism.App.Resources;
using MMA.Prism.Infrastructure.Services;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MMA.Prism.App.MVVM.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public ICommand CloseWindowCommand { get; private set; }

        #region -- Contructor --
        public ShellViewModel(IRegionManager regionManager)
        {
            ApplicationTitle = ApplicationLabels.ApplicationTitle;

            //CloseWindowCommand = new DelegateCommand<object>(OnCloseWindow, CanCloseWindow);
        }

        #endregion
    }
}
