using AllOrNothing.Contracts.Services;
using AllOrNothing.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Navigation;
using OxyPlot;
using System;

namespace AllOrNothing.ViewModels
{
    /// <summary>
    /// ViewModel class for the future implementation of the player statistics page <br/>
    /// <b>To be implemented!</b>
    /// </summary>
    public class StatisticsViewModel : ViewModelBase
    {
        public StatisticsViewModel(INavigationViewService navigationViewService)
            :base(navigationViewService)
        {
        }
    }
}
