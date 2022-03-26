using AllOrNothing.Data;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;

namespace AllOrNothing.Mapping
{
    [AutoMap(typeof(Question), ReverseMap = true)]
    public class QuestionDto : ObservableRecipient
    {
        public int Id { get; set; }
        public QuestionResourceType ResourceType { get; set; }
        public QuestionType Type { get; set; }
        public byte[] Resource { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
        public int Value { get; set; }


        //used in the questionSeriePage
        public void TextBoxChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var trimmedText = textBox.Text.Trim();
                switch (textBox.Name)
                {
                    case "questionTextBox":
                        if (true)
                        {
                            //UnderEdit?.Invoke(this, null);
                            Text = trimmedText;
                        }
                        break;
                    case "answerTextBox":
                        if (true)
                        {
                            //UnderEdit?.Invoke(this, null);
                            Answer = trimmedText;
                        }
                        break;
                    case "valueTextBox":
                        if (true)
                        {
                            //UnderEdit?.Invoke(this, null);
                            Value = int.Parse(trimmedText);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
