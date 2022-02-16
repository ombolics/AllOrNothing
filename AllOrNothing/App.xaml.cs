﻿using AllOrNothing.Activation;
using AllOrNothing.Contracts.Services;
using AllOrNothing.Helpers;
using AllOrNothing.Services;
using AllOrNothing.ViewModels;
using AllOrNothing.Views;

using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

// To learn more about WinUI3, see: https://docs.microsoft.com/windows/apps/winui/winui3/.
namespace AllOrNothing
{
    public partial class App : Application
    {
        public static Window MainWindow { get; set; } = new Window() { Title = "AppDisplayName".GetLocalized() };

        public App()
        {
            InitializeComponent();
            UnhandledException += App_UnhandledException;
            Ioc.Default.ConfigureServices(ConfigureServices());

            //Borderless mode
            //MainWindow.ExtendsContentIntoTitleBar = true;

        }

        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // TODO WTS: Please log and handle the exception as appropriate to your scenario
            // For more info see https://docs.microsoft.com/windows/winui/api/microsoft.ui.xaml.unhandledexceptioneventargs
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
            var activationService = Ioc.Default.GetService<IActivationService>();
            await activationService.ActivateAsync(args);
        }

        private System.IServiceProvider ConfigureServices()
        {
            // TODO WTS: Register your services, viewmodels and pages here
            var services = new ServiceCollection();

            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services

            // Views and ViewModels
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();

            services.AddSingleton<MenuPage>();
            services.AddSingleton<AllOrNothingViewModel>();

            services.AddSingleton<AllOrNothingGameSettings>();
            services.AddSingleton<AllOrNothingSettingsViewModel>();

            services.AddSingleton<TematicalPage>();
            services.AddSingleton<AllOrNothingTematicalViewModel>();

            services.AddSingleton<StatisticsPage>();
            services.AddSingleton<StatisticsViewModel>();

            services.AddTransient<PlayerStatViewModel>();
            services.AddTransient<PlayerStatPage>();

            services.AddTransient<QuestionSeriesViewModel>();
            services.AddTransient<QuestionSeriesPage>();

            return services.BuildServiceProvider();
        }
    }
}
