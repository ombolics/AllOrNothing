using AllOrNothing.Data;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;

namespace AllOrNothing.Mapping
{
    [AutoMap(typeof(Question), ReverseMap = true)]
    public class QuestionDto : ObservableRecipient
    {
        public QuestionDto()
        {

        }
        public QuestionDto(QuestionDto dto)
        {
            Id = dto.Id;
            ResourceType = dto.ResourceType;
            Resource = dto.Resource;
            Type = dto.Type;
            Text = dto.Text;
            Answer = dto.Answer;
            Value = dto.Value;
        }
        public int Id { get; set; }
        private QuestionResourceType _resourceType;
        public QuestionResourceType ResourceType
        {
            get => _resourceType;
            set => SetProperty(ref _resourceType, value);
        }
        private QuestionType _questionType;
        public QuestionType Type
        {
            get => _questionType;
            set => SetProperty(ref _questionType, value);
        }
        private byte[] _resource;
        public byte[] Resource
        {
            get => _resource;
            set => SetProperty(ref _resource, value);
        }
        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }
        private string _answer;
        public string Answer
        {
            get => _answer;
            set => SetProperty(ref _answer, value);
        }
        private int _value;
        public int Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        internal bool HasTheSameValue(QuestionDto dto)
        {
            bool val =
                Id == dto.Id &&
                ResourceType == dto.ResourceType &&
                Type == dto.Type &&
                Resource == dto.Resource &&
                Text == dto.Text &&
                Answer == dto.Answer &&
                Value == dto.Value;

            return val;
        }



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
