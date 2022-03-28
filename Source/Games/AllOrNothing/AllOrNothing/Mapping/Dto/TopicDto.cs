using AllOrNothing.Data;
using AllOrNothing.Repository;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.Foundation;

namespace AllOrNothing.Mapping
{
    [AutoMap(typeof(Topic), ReverseMap = true)]
    public class TopicDto : ObservableRecipient
    {
        public TopicDto() 
        {
            _unitOfWork = Ioc.Default.GetService<UnitOfWork>();
            _mapper = Ioc.Default.GetService<Mapper>();
        }
        public TopicDto(int questionCount, IUnitOfWork unitOfWork, IMapper mapper)
        {
            Questions = new List<QuestionDto>();
            Competences = new ObservableCollection<CompetenceDto>();
            
            for (int i = 0; i < questionCount; i++)
            {
                Questions.Add(new QuestionDto());
            }

            _unitOfWork = unitOfWork;
            _mapper = mapper;

            AutosuggestBox_TextChanged_Handler = AutosuggestBox_TextChanged_Handler;
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

            var competences = new ObservableCollection<CompetenceDto>();
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
        private ObservableCollection<CompetenceDto> _competences;
        public ObservableCollection<CompetenceDto> Competences 
        {
            get => _competences;
            set => SetProperty(ref _competences, value);
        }
        private PlayerDto _author;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

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

        private ICommand _removeCompetenceCommand;
        public ICommand RemoveCompetenceCommand => _removeCompetenceCommand ??= new RelayCommand<object>(RemoveCompetence);

        private TypedEventHandler<AutoSuggestBox, AutoSuggestBoxTextChangedEventArgs> _autosuggestBox_TextChanged_Handler;
        public  TypedEventHandler<AutoSuggestBox, AutoSuggestBoxTextChangedEventArgs> AutosuggestBox_TextChanged_Handler
        {
            get => _autosuggestBox_TextChanged_Handler;
            set => SetProperty(ref _autosuggestBox_TextChanged_Handler, value);
        }

        private void RemoveCompetence(object param)
        {
            var dto = param as CompetenceDto;
            Competences.Remove(dto);           
        }

        public void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if(args.SelectedItem is CompetenceDto competence)
            {
                Competences.Add(competence);
                sender.Text = string.Empty;
                sender.ItemsSource = null;
                return;
            }

            //if(args.SelectedItem is StackPanel notFoundDisplay)
            //{
            //    NavigateTo?.Invoke(this, new NavigateToEventargs { PageName = "Új játékos", PageVM = typeof(PlayerAddingViewModel) });
            //}
          
        }
        public void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Since selecting an item will also change the text,
            // only listen to changes caused by user entering text.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<object>();
                var splitText = sender.Text.ToLower().Split(" ");

                //TODO Search for players in database

                foreach (var competence in _unitOfWork.Topics.GetAll())
                {
                    //var found = splitText.All((key) =>
                    //{
                    //    return player.Name.ToLower().Contains(key);
                    //});

                    //found
                    if (splitText.All((key) => competence.Name.ToLower().Contains(key)))
                    {
                        var dto = _mapper.Map<CompetenceDto>(competence);
                        dto.RemoveCommand = (RelayCommand<object>)RemoveCompetenceCommand;
                        suitableItems.Add(dto);
                    }
                }

                //if (suitableItems.Count == 0)
                //{
                //    var notfoundDisplay = new StackPanel
                //    {
                //        Orientation = Orientation.Horizontal,
                //        Spacing = 30.0,
                //    };

                //    notfoundDisplay.Children.Add(new TextBlock
                //    {
                //        Text = "Nincs ilyen játkos!",
                //    });

                //    notfoundDisplay.Children.Add(new Button
                //    {
                //        Content = new TextBlock
                //        {
                //            Text = "Új játékos",
                //        },
                //        Command = NavigateToNewPlayerPageCommand,
                //    });
                //    suitableItems.Add(notfoundDisplay);
                //}
                sender.ItemsSource = suitableItems;
            }
        }
    }
}
