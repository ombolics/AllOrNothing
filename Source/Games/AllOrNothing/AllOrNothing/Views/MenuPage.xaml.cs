﻿using AllOrNothing.ViewModels;

using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml.Controls;

namespace AllOrNothing.Views
{
    public sealed partial class MenuPage : Page
    {
        public AllOrNothingViewModel ViewModel { get; set; } = Ioc.Default.GetService<AllOrNothingViewModel>();

        public MenuPage()
        {
            ViewModel = Ioc.Default.GetService<AllOrNothingViewModel>();
            InitializeComponent();
        }
    }
}