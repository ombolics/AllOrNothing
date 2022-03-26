using AllOrNothing.Data;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;

namespace AllOrNothing.Mapping
{
    [AutoMap(typeof(Topic), ReverseMap = true)]
    public class TopicDto : ObservableRecipient
    {

        public TopicDto()
        {
            Questions = new List<QuestionDto>();
            Competences = new List<CompetenceDto>();
            
            for (int i = 0; i < 6; i++)
            {
                Questions.Add(new QuestionDto());
            }
            
        }

        public TopicDto(TopicDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Description = dto.Description;
            Author = new PlayerDto(dto.Author);
            var questions = new List<QuestionDto>();

            foreach (var question in dto.Questions)
            {
                questions.Add(new QuestionDto(question));
            }
            Questions = questions;

            var competences = new List<CompetenceDto>();
            if(dto.Competences != null)
            {
                foreach (var competence in dto.Competences)
                {
                    competences.Add(new CompetenceDto(competence));
                }
            }
            Competences = competences;
        }
        public int Id { get; set; }
        private string _name;
        public string Name 
        {
            get => _name;
            set => SetProperty(ref _name, value); 
        }
        private string _description;
        public string Description 
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        
        private List<QuestionDto> _questions;
        public List<QuestionDto> Questions 
        {
            get => _questions;
            set => SetProperty(ref _questions, value);
        }
        private List<CompetenceDto> _competences;
        public List<CompetenceDto> Competences 
        {
            get => _competences;
            set => SetProperty(ref _competences, value);
        }
        private PlayerDto _author;
        public PlayerDto Author 
        {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        internal bool HasTheSameValue(TopicDto dto)
        {
            bool val =
                Name == dto.Name &&
                Id == dto.Id &&
                Description == dto.Description;

            for (int i = 0; i < Questions.Count; i++)
            {
                val = val && Questions[i].HasTheSameValue(dto.Questions[i]);
            }

            for (int i = 0; i < Competences.Count; i++)
            {
                val = val && Competences[i].HasTheSameValue(dto.Competences[i]);
            }

            return val;

        }



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
