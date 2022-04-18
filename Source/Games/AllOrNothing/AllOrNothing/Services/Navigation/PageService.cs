﻿using AllOrNothing.Contracts.Services;
using AllOrNothing.ViewModels;
using AllOrNothing.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AllOrNothing.Services
{
    public class PageService : IPageService
    {
        private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();

        public PageService()
        {
            //core pages
            Configure<SettingsViewModel, SettingsPage>();
            Configure<AllOrNothingViewModel, MenuPage>();
            Configure<AllOrNothingSettingsViewModel, AllOrNothingGameSettings>();
            Configure<AllOrNothingGameViewModel, GamePage>();
            Configure<StatisticsViewModel, StatisticsPage>();
            Configure<PlayerStatViewModel, PlayerStatPage>();
            Configure<QuestionSeriesPageViewModel, QuestionSeriesPage>();
            Configure<ScoreBoardPageViewModel, ScoreBoardPage>();
            Configure<PlayerAddingViewModel, PlayerAddingPage>();

        }

        public Type GetPageType(string key)
        {
            Type pageType;
            lock (_pages)
            {
                if (!_pages.TryGetValue(key, out pageType))
                {
                    throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
                }
            }

            return pageType;
        }

        public bool IsPageKey(string key)
        {
            return _pages.ContainsKey(key);
        }

        private void Configure<VM, V>()
            where VM : ObservableObject
            where V : Page
        {
            lock (_pages)
            {
                var key = typeof(VM).FullName;
                if (_pages.ContainsKey(key))
                {
                    throw new ArgumentException($"The key {key} is already configured in PageService");
                }

                var type = typeof(V);
                if (_pages.Any(p => p.Value == type))
                {
                    throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
                }

                _pages.Add(key, type);
            }
        }
    }
}