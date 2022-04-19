using AllOrNothing.Mapping;
using AllOrNothing.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;

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



        public StandingDto Standing
        {
            get { return (StandingDto)GetValue(StandingProperty); }
            set { SetValue(StandingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Standing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StandingProperty =
            DependencyProperty.Register("Standing", typeof(StandingDto), typeof(TeamScore), null);



        //public static readonly DependencyProperty ScoreProperty = DependencyProperty.Register(
        //"Score", typeof(int),
        //typeof(TeamScore),
        //null
        //);

        //public Team Team
        //{
        //    get { return (Team)GetValue(TeamProperty); }
        //    set { SetValue(TeamProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Team.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty TeamProperty =
        //    DependencyProperty.Register("Team", typeof(Team), typeof(TeamScore), null);



        public QuestionDto CurrentQuestion
        {
            get { return (QuestionDto)GetValue(CurrentQuestionProperty); }
            set { SetValue(CurrentQuestionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentQuestion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentQuestionProperty = DependencyProperty.Register(
            "CurrentQuestion", 
            typeof(QuestionDto), 
            typeof(TeamScore),
            new PropertyMetadata(null,new PropertyChangedCallback(On_CurrentQuestionChanged)));

        private static void On_CurrentQuestionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var o = e.OldValue;
            var n = e.NewValue;
            _ = 0;
        }



        //public int Score
        //{
        //    get { return (int)GetValue(ScoreProperty); }
        //    set { SetValue(ScoreProperty, value); }
        //}

        private void On_ButtonPressed(object sender, RoutedEventArgs e)
        {
            if (CurrentQuestion == null)
                return;

            if ((sender as Button).Name == "PlusButton")
                Standing.Score += CurrentQuestion.Value;
            else
                Standing.Score -= CurrentQuestion.Value;

            //SetValue(CurrentQuestionProperty, null);
            if(GamePhase == GamePhase.TEMATICAL)
                CurrentQuestion = null;
        }



        public GamePhase GamePhase
        {
            get { return (GamePhase)GetValue(GamePhaseProperty); }
            set { SetValue(GamePhaseProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GamePhaseProperty =
            DependencyProperty.Register("GamePhase", typeof(GamePhase), typeof(TeamScore), null);



        private void On_TextChanged(object sender, TextChangedEventArgs e)
        {
            //int scr;
            //if (int.TryParse(ScoreBox.Text, out scr))
            //    Score = scr;
        }
    }
}
