using AllOrNothing.Mapping;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Diagnostics;
using Windows.UI;
using Windows.UI.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AllOrNothing.Controls
{
    public sealed partial class QuestionGrid : UserControl
    {
        #region Fields
        private QuestionSerieDto _questionSerie;
        private Grid _buttonGrid;
        #endregion

        #region Construtors
        public QuestionGrid()
        {
            this.InitializeComponent();
        }
        #endregion

        #region Propeties
        public QuestionSerieDto QuestionSerie
        {
            get => _questionSerie;
            set
            {
                _questionSerie = value;
                if (value != null)
                    CreateGrid();
            }
        }
        public string QuestionDisplay
        {
            get { return (string)GetValue(QuestionDisplayProperty); }
            set { SetValue(QuestionDisplayProperty, value); }
        }
        public QuestionDto CurrentQuestion
        {
            get { return (QuestionDto)GetValue(CurrentQuestionProperty); }
            set
            {
                Debug.WriteLine("QG\t" + value?.GetHashCode());
                SetValue(CurrentQuestionProperty, value);
                QuestionDisplay = CurrentQuestion == null ? "" : CurrentQuestion.Text;

            }
        }
        #endregion

        #region Dependecy properties
        public static readonly DependencyProperty QuestionDisplayProperty =
            DependencyProperty.Register("QuestionDisplay", typeof(string), typeof(QuestionGrid), null);

        public static readonly DependencyProperty CurrentQuestionProperty =
            DependencyProperty.Register("CurrentQuestion", typeof(QuestionDto), typeof(QuestionGrid), null);
        #endregion

        #region Mehtods
        private void CreateGrid()
        {
            MainGrid.Children.Remove(_buttonGrid);
            CurrentQuestion = null;

            _buttonGrid = new Grid();
            for (int i = 0; i < _questionSerie.Topics.Count; i++)
            {
                _buttonGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1.0, GridUnitType.Star)
                });
            }

            for (int i = 0; i < _questionSerie.Topics[0].Questions.Count + 1; i++)
            {
                _buttonGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(1.0, i == 0 ? GridUnitType.Auto : GridUnitType.Star)
                });
            }

            for (int i = 0; i < _questionSerie.Topics.Count; i++)
            {

                Button headerButton = new Button()
                {
                    CornerRadius = new CornerRadius(10),
                    Background = new SolidColorBrush((Color)App.Current.Resources["MainColor4"]),
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                    FontSize = 20,
                    FontWeight = new FontWeight(700),
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    BorderThickness = new Thickness(0, 0, 0, 0),
                    Margin = new Thickness(5, 5, 5, 20),

                    Content = new TextBlock
                    {
                        Text = _questionSerie.Topics[i].Name,
                        TextWrapping = TextWrapping.WrapWholeWords,
                        TextAlignment = TextAlignment.Center,
                    },
                    Flyout = !string.IsNullOrEmpty(_questionSerie.Topics[i].Description)
                    ? new Flyout()
                    {
                        Content = new TextBlock
                        {
                            Text = _questionSerie.Topics[i].Description,
                            TextWrapping = TextWrapping.WrapWholeWords,
                        },
                    }
                    : null,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                };

                _buttonGrid.Children.Add(headerButton);
                Grid.SetColumn(headerButton, i);
                Grid.SetRow(headerButton, 0);
                for (int j = 1; j < _questionSerie.Topics[i].Questions.Count + 1; j++)
                {
                    QuestionButton b = new QuestionButton(_questionSerie.Topics[i].Questions[j - 1], _questionSerie.Topics[i].Name)
                    {
                        CornerRadius = new CornerRadius(10),
                        Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                        Foreground = new SolidColorBrush((Color)App.Current.Resources["MainColor1"]),
                        BorderThickness = new Thickness(0, 0, 0, 0),
                        FontSize = 17,

                        Content = _questionSerie.Topics[i].Questions[j - 1].Value.ToString(),
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Margin = new Thickness(5, 5, 5, 5),
                        UseLayoutRounding = true,
                    };
                    b.Click += B_Click;
                    _buttonGrid.Children.Add(b);
                    Grid.SetColumn(b, i);
                    Grid.SetRow(b, j);
                }
            }
            MainGrid.Children.Add(_buttonGrid);
            Grid.SetRow(_buttonGrid, 0);
        }
        private void B_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as QuestionButton);
            button.Visibility = Visibility.Collapsed;

            CurrentQuestion = button.Question;
        }
        #endregion      
    }
}
