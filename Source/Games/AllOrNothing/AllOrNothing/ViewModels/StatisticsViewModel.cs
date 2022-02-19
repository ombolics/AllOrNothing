using AllOrNothing.Contracts.Services;
using AllOrNothing.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.ViewModels
{
    public class StatisticsViewModel : ObservableRecipient
    {
        public INavigationService NavigationService { get; }

        public INavigationViewService NavigationViewService { get; }
        private object _selected;
        public object Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }
        public StatisticsViewModel()
        {
            var pgServ = new PageService(); //Ioc.Default.GetService<PageService>();
            NavigationService = new NavigationService(pgServ);//Ioc.Default.GetService<NavigationService>();
            NavigationViewService = new NavigationViewService(NavigationService, pgServ);//Ioc.Default.GetService<NavigationViewService>();
        }
        
        private void OnNavigated(object sender, NavigationEventArgs e)
        {

            var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
            if (selectedItem != null)
            {
                Selected = selectedItem;
            }
        }
    }
}
