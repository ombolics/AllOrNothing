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

        private int _score;

        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                if (!string.Equals(ScoreBox.Text, value.ToString()))
                    ScoreBox.Text = _score.ToString();
            }
        }

        private void On_TextChanged(object sender, TextChangedEventArgs e)
        {
            int scr;
            if (int.TryParse(ScoreBox.Text, out scr))
                Score = scr;
        }
    }
}
