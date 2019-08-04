using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MMA.Prism.Infrastructure.Communs.Helpers
{
    public class MessageBoxConsolidateHelper : IMessageBoxConsolidateHelper
    {

        private readonly IDialogService _dialogService;

        public MessageBoxConsolidateHelper(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public void DisplayMessageBox(string errorMessage,
            string messageTitle, MessageBoxButton messageBoxButton,
            MessageBoxImage messageBoxImage, MessageBoxResult messageBoxResult)
        {
            _dialogService.ShowMessage(errorMessage,
                                      messageTitle,
                                      messageBoxButton,
                                      messageBoxImage,
                                      messageBoxResult);
        }
    }
}
