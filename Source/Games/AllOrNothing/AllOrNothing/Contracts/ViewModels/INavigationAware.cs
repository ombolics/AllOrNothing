using System.Collections.Generic;

namespace AllOrNothing.Contracts.ViewModels
{
    public interface INavigationAware
    {
        void OnNavigatedTo(object parameter);

        void OnNavigatedFrom();
        bool IsReachable();
    }
}
