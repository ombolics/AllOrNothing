using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AllOrNothing.Controls
{
    class ValidatedTextBox : TextBox
    {
        #region Fields
        private string _originalContent;
        #endregion

        #region Constructors
        public ValidatedTextBox()
        {         
            LostFocus += ValidatedTextBox_LostFocus;
            _originalContent = "";
        }
        #endregion

        #region Properties
        public bool ValidateNumbers
        {
            get { return (bool)GetValue(ValidateNumbersProperty); }
            set
            {
                SetValue(ValidateNumbersProperty, value);
                if (value)
                {
                    _originalContent = "0";
                }
            }
        }
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
        public bool SpecialCharactersEnabled
        {
            get { return (bool)GetValue(SpecialCharactersEnabledProperty); }
            set { SetValue(SpecialCharactersEnabledProperty, value); }
        }
        #endregion

        #region Dependency properties
        public static readonly DependencyProperty ValidateNumbersProperty =
            DependencyProperty.Register("ValidateNumbers", typeof(bool), typeof(ValidatedTextBox), null);

        public static readonly DependencyProperty ValidatedTextProperty =
            DependencyProperty.Register("ValidatedText", typeof(string), typeof(ValidatedTextBox), null);

        public static readonly DependencyProperty SpecialCharactersEnabledProperty =
            DependencyProperty.Register("SpecialCharactersEnabled", typeof(bool), typeof(ValidatedTextBox), null);
        #endregion

        #region Methods
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
        #endregion
    }
}
