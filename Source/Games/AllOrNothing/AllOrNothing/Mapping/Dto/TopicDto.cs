using AllOrNothing.Data;
using AutoMapper;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;

namespace AllOrNothing.Mapping
{
    [AutoMap(typeof(Topic), ReverseMap = true)]
    public class TopicDto
    {

        public TopicDto()
        {
            Questions = new List<QuestionDto>();
            for (int i = 0; i < 6; i++)
            {
                Questions.Add(new QuestionDto());
            }
            
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<QuestionDto> Questions { get; set; }
        public List<CompetenceDto> Competences { get; set; }
        public PlayerDto Author { get; set; }




        // used in the questionserie page
        public string OriginalName { get; set; }
        public string OriginalDescription { get; set; }
        //public event EventHandler UnderEdit;
        public void TextBoxChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var trimmedText = textBox.Text.Trim();
                switch (textBox.Name)
                {
                    case "topicNameTextBox":
                        if(trimmedText != OriginalName)
                        {
                            //UnderEdit?.Invoke(this, null);
                            Name = trimmedText;
                        }
                        break;
                    case "topicDescriptionTextBox":
                        if (trimmedText != OriginalDescription)
                        {
                            //UnderEdit?.Invoke(this, null);
                            Description = trimmedText;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
