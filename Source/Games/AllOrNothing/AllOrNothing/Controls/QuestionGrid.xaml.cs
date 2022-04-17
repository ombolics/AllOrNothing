using AllOrNothing.Mapping;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AllOrNothing.Controls
{
    public sealed partial class QuestionGrid : UserControl
    {
        private QuestionSerieDto _questionSerie;
        public QuestionSerieDto QuestionSerie
        {
            get => _questionSerie;
            set
            {
                _questionSerie = value;
                if(value != null)
                    CreateGrid();
            }
        }

        public QuestionGrid()
        {
            this.InitializeComponent();
        }



        public string QuestionDisplay
        {
            get { return (string)GetValue(QuestionDisplayProperty); }
            set { SetValue(QuestionDisplayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for QuestionDisplay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QuestionDisplayProperty =
            DependencyProperty.Register("QuestionDisplay", typeof(string), typeof(QuestionGrid), null);



        public QuestionDto CurrentQuestion
        {
            get { return (QuestionDto)GetValue(CurrentQuestionProperty); }
            set
            {
                SetValue(CurrentQuestionProperty, value);
                QuestionDisplay = CurrentQuestion == null ? "" : CurrentQuestion.Text;
            }
        }

        // Using a DependencyProperty as the backing store for CurrentQuestion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentQuestionProperty =
            DependencyProperty.Register("CurrentQuestion", typeof(QuestionDto), typeof(QuestionGrid), null);



        public bool IsAnswered
        {
            get { return (bool)GetValue(IsAnsweredProperty); }
            set
            {
                SetValue(IsAnsweredProperty, value);
                if (value)
                {
                    //QuestionText.Text = "";
                    CurrentQuestion = null;
                }
            }
        }

        // Using a DependencyProperty as the backing store for IsAnswered.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAnsweredProperty =
            DependencyProperty.Register("IsAnswered", typeof(bool), typeof(QuestionGrid), null);


        private Grid _buttonGrid;
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
                    BorderThickness = new Thickness(0,0,0,0),
                    Margin = new Thickness(5, 5, 5, 20),

                    Content = _questionSerie.Topics[i].Name,
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
                    QuestionButton b = new QuestionButton(_questionSerie.Topics[i].Questions[j - 1])
                    {
                        CornerRadius = new CornerRadius(10),
                        Background = new SolidColorBrush(Color.FromArgb(255,255,255,255)),
                        Foreground = new SolidColorBrush((Color)App.Current.Resources["MainColor1"]),
                        BorderThickness = new Thickness(0, 0, 0, 0),
                        FontSize = 17,

                        Content = _questionSerie.Topics[i].Questions[j - 1].Value.ToString(),
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Margin = new Thickness(5,5,5,5),
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

            //TODO kép hang videó typusok megjelenítése         
            CurrentQuestion = button.Question;
        }
    }
}
