using AllOrNothing.Data;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
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

        private RelayCommand<object> relayCommand;
        public RelayCommand<object> RemoveCommand 
        {
            get => relayCommand;
            set => SetProperty(ref relayCommand, value);
        }

        public override string ToString()
        {
            return Name;
        }

        internal bool HasTheSameValue(CompetenceDto dto)
        {
            return Id == dto.Id && Name == dto.Name;
        }

        public  void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}