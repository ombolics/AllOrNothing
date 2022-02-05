using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using AllOrNothing.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AllOrNothing.Controls
{
    public sealed partial class QuestionGrid : UserControl
    {
        private int _columnCount;
        private int _rowCount;

        public int ColumnCount
        {
            get => _columnCount;
            set => _columnCount = value;
        }
        public int RowCount
        {
            get => _rowCount;
            set => _rowCount = value;
        }

        private string _currentQuestionText;
        public string CurrentQuestionText
        {
            get => _currentQuestionText;
            set => _currentQuestionText = value;
        }

        private QuestionSerie _questionSerie;
        public QuestionSerie QuestionSerie
        {
            get => _questionSerie;
            set
            {
                _questionSerie = value;
                CreateGrid();
            }
        }

        public event EventHandler<QuestionPickedEventArgs> QuestionPicked;

        public QuestionGrid()
        {
            this.InitializeComponent();

            _columnCount = 5;
            _rowCount = 6;
        }

        private void CreateGrid()
        {

            var buttonGrid = new Grid();
            for (int i = 0; i < _questionSerie.Topics.Count; i++)
            {
                buttonGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1.0, GridUnitType.Star)
                });
            }

            for (int i = 0; i < _questionSerie.Topics[0].Questions.Count + 1; i++)
            {
                buttonGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(1.0, GridUnitType.Star)
                });
            }

            for (int i = 0; i < _questionSerie.Topics.Count; i++)
            {
                TextBlock tb = new TextBlock()
                {
                    Text = _questionSerie.Topics[i].Name,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };

                buttonGrid.Children.Add(tb);
                Grid.SetColumn(tb, i);
                Grid.SetRow(tb, 0);
                for (int j = 1; j < _questionSerie.Topics[i].Questions.Count + 1; j++)
                {
                    QuestionButton b = new QuestionButton(_questionSerie.Topics[i].Questions[j - 1])
                    {
                        Content = _questionSerie.Topics[i].Questions[j - 1].Value.ToString(),
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        BorderThickness = new Thickness(10.0, 1.0, 1.0, 1.0),
                        Margin = new Thickness(2, 2, 2, 2),
                        UseLayoutRounding = true,
                    };
                    b.Click += B_Click;
                    buttonGrid.Children.Add(b);
                    Grid.SetColumn(b, i);
                    Grid.SetRow(b, j);
                }
            }
            MainGrid.Children.Add(buttonGrid);
            Grid.SetRow(buttonGrid, 0);

        }
        private void B_Click(object sender, RoutedEventArgs e)
        {
            var button = (sender as QuestionButton);
            button.Visibility = Visibility.Collapsed;
            QuestionText.Text = button.Question.Text;
            QuestionPicked?.Invoke(this, new QuestionPickedEventArgs(button.Question));
        }
    }
}
