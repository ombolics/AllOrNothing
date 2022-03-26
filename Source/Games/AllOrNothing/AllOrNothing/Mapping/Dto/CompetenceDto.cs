using AllOrNothing.Data;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AllOrNothing.Mapping
{
    [AutoMap(typeof(Competence), ReverseMap = true)]
    public class CompetenceDto : ObservableRecipient
    {
        public CompetenceDto()
        {

        }
        public CompetenceDto(CompetenceDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
        }
        public int Id { get; set; }

        private string _name;
        public string Name 
        {
            get => _name;
            set => SetProperty(ref _name, value); 
        }

        internal bool HasTheSameValue(CompetenceDto dto)
        {
            return Id == dto.Id && Name == dto.Name;
        }
    }
}