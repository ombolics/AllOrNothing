using AllOrNothing.Data;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AllOrNothing.Mapping
{
    [AutoMap(typeof(QuestionSerie), ReverseMap = true)]
    public class QuestionSerieDto : ObservableRecipient
    {
        public QuestionSerieDto()
        {

        }
        public QuestionSerieDto(QuestionSerieDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            FromFile = dto.FromFile;
            IsDeleted = dto.IsDeleted;
            CreatedOn = dto.CreatedOn;
            var topics = new List<TopicDto>();

            foreach (var topic in dto.Topics)
            {
                topics.Add(new TopicDto(topic));
            }
            Topics = topics;
        }
        public int Id { get; set; }
        private List<TopicDto> _topics;
        public List<TopicDto> Topics 
        {
            get => _topics;
            set => SetProperty(ref _topics, value);
        }
        public HashSet<PlayerDto> Authors 
        {
            get => GetAuthors();
        }
        private bool _fromFile;
        public bool FromFile 
        {
            get => _fromFile;
            set => SetProperty(ref _fromFile, value);
        }
        private string _name;
        public string Name 
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        private bool _isDeleted;
        public bool IsDeleted 
        {
            get => _isDeleted;
            set => SetProperty(ref _isDeleted, value);
        }
        private HashSet<PlayerDto> GetAuthors()
        {
            var value = new HashSet<PlayerDto>();
            var ids = new List<int>();
            foreach (var item in Topics)
            {
                if(item.Author != null && !ids.Contains(item.Author.Id))
                {
                    ids.Add(item.Author.Id);
                    value.Add(item.Author);
                }
                
            }
            return value;
        }

        public DateTime CreatedOn { get; set; }

        private HashSet<CompetenceDto> GetCompetences()
        {
            var value = new HashSet<CompetenceDto>();
            foreach (var topic in Topics)
            {
                foreach (var item in topic.Competences)
                {
                    if(!value.Any(c => c.Id == item.Id))
                        value.Add(item);
                }
            }
            return value;
        }

        public HashSet<CompetenceDto> Competences
        {
            get => GetCompetences();
        }

        public bool HasTheSameValue(QuestionSerieDto dto)
        {
            bool val =
                Name == dto.Name &&
                CreatedOn == dto.CreatedOn &&
                FromFile == dto.FromFile &&
                IsDeleted == dto.IsDeleted;

            for (int i = 0; i < Topics.Count; i++)
            {
                val = val && Topics[i].HasTheSameValue(dto.Topics[i]);
            }

            return val;
        }





        public event EventHandler UnderEdit;
        //Used in the question serie page
        public void TextBoxChanged(object sender, TextChangedEventArgs e)
        {
            //TODO validálás
            if (sender is TextBox textBox && textBox.Name == "serieNameTextBox" && Name != textBox.Text.Trim())
            {
                Name = textBox.Text.Trim();
                //UnderEdit?.Invoke(this, null);
            }           
        }
    }
}
