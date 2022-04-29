using AllOrNothing.Contracts.Services;
using AllOrNothing.Data;
using AllOrNothing.Mapping;
using AllOrNothing.Repository;
using AllOrNothing.Services;
using AllOrNothing.ViewModels;
using AllOrNothingTest.Helpers;
using AutoMapper;
using CommunityToolkit.Mvvm.DependencyInjection;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AllOrNothingTest
{
    public class AllOrNothingSettingsViewModelTest : IClassFixture<SettingsViewModelTestFixture>
    {
        GameSettingsViewModel _viewModel;
        private readonly SettingsViewModelTestFixture _fixture;
        public AllOrNothingSettingsViewModelTest(SettingsViewModelTestFixture fixture)
        {
            _fixture = fixture;
            if (_fixture.MyProperty == 0)
            {
                Ioc.Default.ConfigureServices(ConfigureServices());
                _fixture.MyProperty = 1;
            }        
        }

        #region Helper Methods
        private System.IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();


            services.AddSingleton<INavigationViewService, NavigationViewService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IQuestionSerieLoader, QuestionSerieLoader>();

            // Core Services
            //services.AddTransient<IAllOrNothingDbContext, AllOrNothingDbContext>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            //services.AddDbContext<AllOrNothingDbContext>(optionsBuilder => optionsBuilder.UseSqlServer(@"Server=DESKTOP-B5C457P\SQLEXPRESS;Integrated Security=true;Database=AllOrNothingDb;"));

            //Mapping

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services.BuildServiceProvider();
        }

        private List<PlayerDto> GetPlayerCollection(int playerCount)
        {
            var value = new List<PlayerDto>();
            for (int i = 0; i < playerCount; i++)
            {
                value.Add(new PlayerDto
                {
                    Id = i,
                    Name = $"TestName{i}",
                    NickName = $"T{i}",
                    Institute = "Test",                 
                });
            }
            return value;
        }
        #endregion

        #region TeamGeneration tests
        [Fact]
        public void TeamGenerationTest_MaxTeamSize()
        {
            _viewModel = new GameSettingsViewModel(
                Ioc.Default.GetService<INavigationViewService>(),
                Ioc.Default.GetService<IMapper>(),
                Ioc.Default.GetService<IUnitOfWork>(),
                Ioc.Default.GetService<IQuestionSerieLoader>());
            
            var teams = _viewModel.GenerateTeams(GetPlayerCollection(10), 2);
            teams.Count.Should().Be(5);
            teams.All(t => t.Players.Count >= 1).Should().BeTrue();
            teams.All(t => t.Players.Count <= 2).Should().BeTrue();

            teams = _viewModel.GenerateTeams(GetPlayerCollection(11), 3);
            teams.Count.Should().Be(4);
            teams.All(t => t.Players.Count >= 1).Should().BeTrue();
            teams.All(t => t.Players.Count <= 4).Should().BeTrue();

            teams = _viewModel.GenerateTeams(GetPlayerCollection(11), 1);
            teams.Count.Should().Be(11);
            teams.All(t => t.Players.Count == 1).Should().BeTrue();

            teams = _viewModel.GenerateTeams(GetPlayerCollection(4), 30);
            teams.Count.Should().Be(1);
            teams.All(t => t.Players.Count == 4).Should().BeTrue();
        }

        [Fact]
        public void TeamGenerationTest_EachPlayerInOneTeam()
        {
            _viewModel = new GameSettingsViewModel(
                Ioc.Default.GetService<INavigationViewService>(),
                Ioc.Default.GetService<IMapper>(),
                Ioc.Default.GetService<IUnitOfWork>(),
                Ioc.Default.GetService<IQuestionSerieLoader>());

            var players = GetPlayerCollection(10);
            var teams = _viewModel.GenerateTeams(players, 2);
            foreach (var player in players)
            {
                teams.Count(t => t.Players.Any(p => p.Id == player.Id)).Should().Be(1);
            }

        }
        #endregion

        #region SheduleGeneration tests
        [Fact]
        public void EachTeamAppearsTheRequieredTime()
        {
            _viewModel = new GameSettingsViewModel(
                Ioc.Default.GetService<INavigationViewService>(),
                Ioc.Default.GetService<IMapper>(),
                Ioc.Default.GetService<IUnitOfWork>(),
                Ioc.Default.GetService<IQuestionSerieLoader>());

            var teams = _viewModel.GenerateTeams(GetPlayerCollection(2), 1);
            var shedule = _viewModel.GenerateSchedule(teams, 3);

            foreach (var team in teams)
            {
                shedule.Count(s => s.Teams.Contains(team)).Should().Be(3);
            }

            teams = _viewModel.GenerateTeams(GetPlayerCollection(10), 1);
            shedule = _viewModel.GenerateSchedule(teams, 3);

            foreach (var team in teams)
            {
                shedule.Count(s => s.Teams.Contains(team)).Should().Be(3);
            }
        }

        [Fact]
        public void EachSheduleContainsDifferentTeams()
        {
            _viewModel = new GameSettingsViewModel(
                Ioc.Default.GetService<INavigationViewService>(),
                Ioc.Default.GetService<IMapper>(),
                Ioc.Default.GetService<IUnitOfWork>(),
                Ioc.Default.GetService<IQuestionSerieLoader>());

            // (round count * team count ) mod 4

            // team count is low
            var teams = _viewModel.GenerateTeams(GetPlayerCollection(2), 1);
            var shedules = _viewModel.GenerateSchedule(teams, 2);
            shedules.All(s => s.Teams.Distinct().Count() == s.Teams.Count).Should().BeTrue();

            teams = _viewModel.GenerateTeams(GetPlayerCollection(3), 1);
            shedules = _viewModel.GenerateSchedule(teams, 4);
            shedules.All(s => s.Teams.Distinct().Count() == s.Teams.Count).Should().BeTrue();

            teams = _viewModel.GenerateTeams(GetPlayerCollection(5), 1);
            shedules = _viewModel.GenerateSchedule(teams, 1);
            shedules.All(s => s.Teams.Distinct().Count() == s.Teams.Count).Should().BeTrue();

            // 0
            teams = _viewModel.GenerateTeams(GetPlayerCollection(10), 1);
            shedules = _viewModel.GenerateSchedule(teams, 2);
            shedules.All(s => s.Teams.Distinct().Count() == s.Teams.Count).Should().BeTrue();

            //1
            teams = _viewModel.GenerateTeams(GetPlayerCollection(9), 1);
            shedules = _viewModel.GenerateSchedule(teams, 5);
            shedules.All(s => s.Teams.Distinct().Count() == s.Teams.Count).Should().BeTrue();

            //2
            teams = _viewModel.GenerateTeams(GetPlayerCollection(11), 1);
            shedules = _viewModel.GenerateSchedule(teams, 6);
            shedules.All(s => s.Teams.Distinct().Count() == s.Teams.Count).Should().BeTrue();

            //3
            teams = _viewModel.GenerateTeams(GetPlayerCollection(13), 1);
            shedules = _viewModel.GenerateSchedule(teams, 3);
            shedules.All(s => s.Teams.Distinct().Count() == s.Teams.Count).Should().BeTrue();
        }

        [Fact]
        public void EachSheduleContainsTheRequieredAmountOfTeams()
        {
            _viewModel = new GameSettingsViewModel(
                Ioc.Default.GetService<INavigationViewService>(),
                Ioc.Default.GetService<IMapper>(),
                Ioc.Default.GetService<IUnitOfWork>(),
                Ioc.Default.GetService<IQuestionSerieLoader>());

            var teams = _viewModel.GenerateTeams(GetPlayerCollection(2), 1);
            var shedules = _viewModel.GenerateSchedule(teams, 3);

            shedules.All(s => s.Teams.Count <= 4).Should().BeTrue();
        }

        [Fact]
        public void EmptyTeamList()
        {
            _viewModel = new GameSettingsViewModel(
                Ioc.Default.GetService<INavigationViewService>(),
                Ioc.Default.GetService<IMapper>(),
                Ioc.Default.GetService<IUnitOfWork>(),
                Ioc.Default.GetService<IQuestionSerieLoader>());

            
        }
        #endregion
    }
}
