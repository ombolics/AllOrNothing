﻿using AllOrNothing.Controls;
using AllOrNothing.Data;
using AllOrNothing.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AllOrNothing.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        public AllOrNothingGameViewModel ViewModel { get; } = Ioc.Default.GetService< AllOrNothingGameViewModel>();
        public GamePage()
        {
            NavigationCacheMode = NavigationCacheMode.Enabled;
            this.InitializeComponent();       
        }

        private void gamePage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PageXamlRoot = this.Content.XamlRoot;
        }
    }
}
