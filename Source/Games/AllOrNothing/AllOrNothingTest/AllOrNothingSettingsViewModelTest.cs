using AllOrNothing.Contracts.Services;
using AllOrNothing.Data;
using AllOrNothing.Mapping;
using AllOrNothing.Repository;
using AllOrNothing.Services;
using AllOrNothing.ViewModels;
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
    public class AllOrNothingSettingsViewModelTest
    {
        AllOrNothingSettingsViewModel _viewModel;
        public AllOrNothingSettingsViewModelTest()
        {
            Ioc.Default.ConfigureServices(ConfigureServices());
        }

        private System.IServiceProvider ConfigureServices()
        {
            // TODO WTS: Register your services, viewmodels and pages here
            var services = new ServiceCollection();


            services.AddSingleton<INavigationViewService, NavigationViewService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            //services.AddTransient<IAllOrNothingDbContext, AllOrNothingDbContext>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<AllOrNothingDbContext>(optionsBuilder => optionsBuilder.UseSqlServer(@"Server=DESKTOP-B5C457P\SQLEXPRESS;Integrated Security=true;Database=AllOrNothingDb;"));

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

        [Fact]
        public void TeamGenerationTest_MaxTeamSize()
        {
            _viewModel = new AllOrNothingSettingsViewModel(
                Ioc.Default.GetService<INavigationViewService>(),
                Ioc.Default.GetService<IMapper>(),
                Ioc.Default.GetService<IUnitOfWork>());
            
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
    } 
}
