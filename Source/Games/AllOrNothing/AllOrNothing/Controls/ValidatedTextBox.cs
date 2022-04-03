using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllOrNothing.Controls
{
    class ValidatedTextBox : TextBox
    {
        public ValidatedTextBox()
        {
            TextChanging += ValidatedTextBox_TextChanging;
            LostFocus += ValidatedTextBox_LostFocus;
            _originalContent = "";
        }

        private void ValidatedTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (ValidateNumbers)
            {
                int result;
                ValidatedText = int.TryParse(textBox.Text, out result) ? result.ToString() : _originalContent;
                return;
            }

            if (SpecialCharactersEnabled)
            {
            }
        }

        public bool ValidateNumbers
        {
            get { return (bool)GetValue(ValidateNumbersProperty); }
            set 
            {
                SetValue(ValidateNumbersProperty, value);
                if(value)
                {
                    _originalContent = "0";
                }
            }
        }

        // Using a DependencyProperty as the backing store for ValidateNumbers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidateNumbersProperty =
            DependencyProperty.Register("ValidateNumbers", typeof(bool), typeof(ValidatedTextBox), null);



        public string ValidatedText
        {
            get { return (string)GetValue(ValidatedTextProperty); }
            set 
            {
                Text = value;
                _originalContent = value;
                SetValue(ValidatedTextProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ValidatedText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidatedTextProperty =
            DependencyProperty.Register("ValidatedText", typeof(string), typeof(ValidatedTextBox), null);



        public bool SpecialCharactersEnabled
        {
            get { return (bool)GetValue(SpecialCharactersEnabledProperty); }
            set { SetValue(SpecialCharactersEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SpecialCharactersEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpecialCharactersEnabledProperty =
            DependencyProperty.Register("SpecialCharactersEnabled", typeof(bool), typeof(ValidatedTextBox), null);

        private string _originalContent;

        private void ValidatedTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            
        }
    }
}
