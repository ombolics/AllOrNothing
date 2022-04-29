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
        #region Constructors
        public TeamScore()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        public StandingDto Standing
        {
            get { return (StandingDto)GetValue(StandingProperty); }
            set { SetValue(StandingProperty, value); }
        }
        public QuestionDto CurrentQuestion
        {
            get { return (QuestionDto)GetValue(CurrentQuestionProperty); }
            set { SetValue(CurrentQuestionProperty, value); }
        }
        public GamePhase GamePhase
        {
            get { return (GamePhase)GetValue(GamePhaseProperty); }
            set { SetValue(GamePhaseProperty, value); }
        }
        public bool IsAddingButtonVisible
        {
            get { return (bool)GetValue(IsAddingButtonVisibleProperty); }
            set { SetValue(IsAddingButtonVisibleProperty, value); }
        }
        #endregion

        #region Dependecy propeties
        public static readonly DependencyProperty StandingProperty =
            DependencyProperty.Register("Standing", typeof(StandingDto), typeof(TeamScore), null);

        public static readonly DependencyProperty CurrentQuestionProperty = DependencyProperty.Register(
            "CurrentQuestion",
            typeof(QuestionDto),
            typeof(TeamScore),
            null);

        public static readonly DependencyProperty IsAddingButtonVisibleProperty =
            DependencyProperty.Register("IsAddingButtonVisible", typeof(bool), typeof(TeamScore), null);

        public static readonly DependencyProperty GamePhaseProperty =
            DependencyProperty.Register("GamePhase", typeof(GamePhase), typeof(TeamScore), null);
        #endregion

        #region Methods
        private void On_ButtonPressed(object sender, RoutedEventArgs e)
        {
            if (CurrentQuestion == null)
                return;

            if ((sender as Button).Name == "PlusButton")
                Standing.Score += CurrentQuestion.Value;
            else
                Standing.Score -= CurrentQuestion.Value;

            if (GamePhase == GamePhase.TEMATICAL)
                CurrentQuestion = null;
        }
        #endregion
    }
}
