using AllOrNothing.Data;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Controls;

namespace AllOrNothing.Mapping
{
    [AutoMap(typeof(QuestionSerie), ReverseMap = true)]
    public class QuestionSerieDto
    {
        public int Id { get; set; }
        public List<TopicDto> Topics { get; set; }
        public HashSet<PlayerDto> Authors 
        {
            get => GetAuthors();
        }
        public bool FromFile { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
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
                    value.Add(item);
                }
            }
            return value;
        }

        public HashSet<CompetenceDto> Competences
        {
            get => GetCompetences();
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
