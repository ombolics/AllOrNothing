﻿using AllOrNothing.Data;
using CommunityToolkit.Mvvm.Input;
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
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AllOrNothing.Controls
{
    public sealed partial class TeamScore : UserControl
    {
        public TeamScore()
        {
            this.InitializeComponent();
        }


        public static readonly DependencyProperty ScoreProperty = DependencyProperty.Register(
        "Score", typeof(int),
        typeof(TeamScore),
        null
        );



        public Team Team
        {
            get { return (Team)GetValue(TeamProperty); }
            set { SetValue(TeamProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Team.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TeamProperty =
            DependencyProperty.Register("Team", typeof(Team), typeof(TeamScore), null);



        public Question CurrentQuestion
        {
            get { return (Question)GetValue(CurrentQuestionProperty); }
            set { SetValue(CurrentQuestionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentQuestion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentQuestionProperty =
            DependencyProperty.Register("CurrentQuestion", typeof(Question), typeof(TeamScore), null);



        public int Score
        {
            get { return (int)GetValue(ScoreProperty); }
            set { SetValue(ScoreProperty, value); }
        }

        private void On_ButtonPressed(object sender, RoutedEventArgs e)
        {
            if (CurrentQuestion == null)
                return;

            if((sender as Button).Name == "PlusButton")
                Score += CurrentQuestion.Value;
            else
                Score -= CurrentQuestion.Value;

            CurrentQuestion = null;
        }


        private void On_TextChanged(object sender, TextChangedEventArgs e)
        {
            //int scr;
            //if (int.TryParse(ScoreBox.Text, out scr))
            //    Score = scr;
        }     
    }
}