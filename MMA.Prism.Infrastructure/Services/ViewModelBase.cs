using NLog;
using System;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA.Prism.Infrastructure.Services
{
    public class ViewModelBase : BindableBase
    {
        public static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private string applicationTitle;
        public string ApplicationTitle
        {
            get { return applicationTitle; }
            set { SetProperty(ref applicationTitle, value); }
        }

        public ViewModelBase()
        {

        }
    }
}
