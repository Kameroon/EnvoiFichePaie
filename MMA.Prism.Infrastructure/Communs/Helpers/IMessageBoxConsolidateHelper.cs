using System.Windows;

namespace MMA.Prism.Infrastructure.Communs.Helpers
{
    public interface IMessageBoxConsolidateHelper
    {
        void DisplayMessageBox(string errorMessage, 
            string messageTitle, MessageBoxButton messageBoxButton, 
            MessageBoxImage messageBoxImage, MessageBoxResult messageBoxResult);
    }
}