using AllOrNothing.Activation;
using AllOrNothing.AutoMapper;
using AllOrNothing.Contracts.Services;
using AllOrNothing.Helpers;
using AllOrNothing.Repository;
using AllOrNothing.Services;
using AllOrNothing.ViewModels;
using AllOrNothing.Views;
using AutoMapper;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System.IO;

// To learn more about WinUI3, see: https://docs.microsoft.com/windows/apps/winui/winui3/.
namespace AllOrNothing
{
    public partial class App : Application
    {
        public static Window MainWindow { get; set; } = new Window() { Title = "AppDisplayName".GetLocalized() };
        public static readonly string QuestionSerieFolder = @$"{System.AppDomain.CurrentDomain.BaseDirectory}\Kérdéssorok";

        public App()
        {
            InitializeComponent();
            UnhandledException += App_UnhandledException;
            Ioc.Default.ConfigureServices(ConfigureServices());

            //Borderless mode
            //MainWindow.ExtendsContentIntoTitleBar = true;

            if (!Directory.Exists(QuestionSerieFolder))
            {
                Directory.CreateDirectory(QuestionSerieFolder);
            }

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
            //services.AddTransient<IAllOrNothingDbContext, AllOrNothingDbContext>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<AllOrNothingDbContext>(optionsBuilder => optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=AllOrNothingDb;Trusted_Connection=True;"));


            //Mapping

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);


            // Views and ViewModels
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();

            services.AddSingleton<MenuPage>();
            services.AddSingleton<AllOrNothingViewModel>();

            services.AddSingleton<AllOrNothingGameSettings>();
            services.AddSingleton<AllOrNothingSettingsViewModel>();

            services.AddSingleton<GamePage>();
            services.AddSingleton<AllOrNothingGameViewModel>();

            services.AddSingleton<StatisticsPage>();
            services.AddSingleton<StatisticsViewModel>();

            services.AddTransient<PlayerStatViewModel>();
            services.AddTransient<PlayerStatPage>();

            services.AddTransient<QuestionSeriesViewModel>();
            services.AddTransient<QuestionSeriesPage>();

            services.AddSingleton<ScoreBoardPageViewModel>();
            services.AddSingleton<ScoreBoardPage>();

            return services.BuildServiceProvider();
        }
    }
}
