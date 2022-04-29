using AllOrNothing.Activation;
using AllOrNothing.Contracts.Services;
using AllOrNothing.Data;
using AllOrNothing.Repository;
using AllOrNothing.Views;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AllOrNothing.Services
{
    public class ActivationService : IActivationService
    {
        #region Fields
        private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
        private readonly IEnumerable<IActivationHandler> _activationHandlers;
        private readonly INavigationService _navigationService;
        private readonly IThemeSelectorService _themeSelectorService;
        private UIElement _shell = null;
        #endregion

        #region Constructors
        public ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, IEnumerable<IActivationHandler> activationHandlers, INavigationService navigationService, IThemeSelectorService themeSelectorService)
        {
            _defaultHandler = defaultHandler;
            _activationHandlers = activationHandlers;
            _navigationService = navigationService;
            _themeSelectorService = themeSelectorService;
        }
        #endregion

        #region Methods
        public async Task ActivateAsync(object activationArgs)
        {
            // Initialize services that you need before app activation
            // take into account that the splash screen is shown while this code runs.
            await InitializeAsync();

            if (App.MainWindow.Content == null)
            {
                _shell = Ioc.Default.GetService<ShellPage>();
                App.MainWindow.Content = _shell ?? new Frame();
            }

            // Depending on activationArgs one of ActivationHandlers or DefaultActivationHandler
            // will navigate to the first page
            await HandleActivationAsync(activationArgs);

            // Ensure the current window is active
            App.MainWindow.Activate();

            // Tasks after activation
            await StartupAsync();
        }

        private async Task HandleActivationAsync(object activationArgs)
        {
            var activationHandler = _activationHandlers
                                                .FirstOrDefault(h => h.CanHandle(activationArgs));

            if (activationHandler != null)
            {
                await activationHandler.HandleAsync(activationArgs);
            }

            if (_defaultHandler.CanHandle(activationArgs))
            {
                await _defaultHandler.HandleAsync(activationArgs);
            }
        }

        private async Task InitializeAsync()
        {
            //create database if not exists
            using (var serviceScope = Ioc.Default.GetService<IServiceScopeFactory>().CreateScope())
            {
                
                var path = @$"{System.AppDomain.CurrentDomain.BaseDirectory}..\..\..\..\..\..\..\AllOrNothing.Repository\AllOrNothingDb.db";
                bool dbExistedBefore = File.Exists(path);

                var context = serviceScope.ServiceProvider.GetRequiredService<AllOrNothingDbContext>();
                context.Database.Migrate();

                //Add competences if the db didnt existed before
                if(! dbExistedBefore)
                {
                    var unitOfWork = Ioc.Default.GetService<IUnitOfWork>();
                    Competence[] competences =
                    {
                    new Competence{Name = "Biológia"},
                    new Competence{Name = "Kémia"},
                    new Competence{Name = "Sport"},
                    new Competence{Name = "Szórakozás"},
                    new Competence{Name = "Életmód"},
                    new Competence{Name = "Művészet"},
                    new Competence{Name = "Mindennapok"},
                    new Competence{Name = "Földrajz"},
                    new Competence{Name = "Történelem"},
                    new Competence{Name = "Irodalom"},
                    new Competence{Name = "Matematika "},
                    new Competence{Name = "Fizika"},
                    new Competence{Name = "Technológia"},
                    };

                    foreach (var item in competences)
                    {
                        unitOfWork.Competences.Add(item);
                    }
                    unitOfWork.Complete();

                }               
            }

            await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
            await Task.CompletedTask;
        }

        private async Task StartupAsync()
        {
            await _themeSelectorService.SetRequestedThemeAsync();
            await Task.CompletedTask;
        }
        #endregion
    }
}
