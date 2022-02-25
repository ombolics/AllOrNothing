using AllOrNothing.Contracts.Services;
using AllOrNothing.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Navigation;
using OxyPlot;
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

        private Random rand = new Random(0);

        private double[] RandomWalk(int points = 5, double start = 100, double mult = 50)
        {
            // return an array of difting random numbers
            double[] values = new double[points];
            values[0] = start;
            for (int i = 1; i < points; i++)
                values[i] = values[i - 1] + (rand.NextDouble() - .5) * mult;
            return values;
        }
        private void SetupModel()
        {
            int pointCount = 1_000;
            double[] xs1 = RandomWalk(pointCount);
            double[] ys1 = RandomWalk(pointCount);
            double[] xs2 = RandomWalk(pointCount);
            double[] ys2 = RandomWalk(pointCount);

            // create lines and fill them with data points
            var line1 = new OxyPlot.Series.LineSeries()
            {
                Title = $"Series 1",
                Color = OxyPlot.OxyColors.Blue,
                StrokeThickness = 1,
                MarkerSize = 2,
                MarkerType = OxyPlot.MarkerType.Circle
            };

            var line2 = new OxyPlot.Series.LineSeries()
            {
                Title = $"Series 2",
                Color = OxyPlot.OxyColors.Red,
                StrokeThickness = 1,
                MarkerSize = 2,
                MarkerType = OxyPlot.MarkerType.Circle
            };

            for (int i = 0; i < pointCount; i++)
            {
                line1.Points.Add(new OxyPlot.DataPoint(xs1[i], ys1[i]));
                line2.Points.Add(new OxyPlot.DataPoint(xs2[i], ys2[i]));
            }

            _plotModel.Title = $"Scatter Plot ({pointCount:N0} points each)";
            _plotModel.Series.Add(line1);
            _plotModel.Series.Add(line2);
        }

        private PlotModel _plotModel;
        public PlotModel PlotModel
        {
            get => _plotModel;
            set => SetProperty(ref _plotModel, value);
        }

        public StatisticsViewModel()
        {
            var pgServ = new PageService(); //Ioc.Default.GetService<PageService>();
            NavigationService = new NavigationService(pgServ);//Ioc.Default.GetService<NavigationService>();
            NavigationViewService = new NavigationViewService(NavigationService, pgServ);//Ioc.Default.GetService<NavigationViewService>();
            _plotModel = new PlotModel();
            SetupModel();

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
