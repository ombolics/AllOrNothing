using AllOrNothing.Data;
using AllOrNothing.Mapping;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AllOrNothing.Controls
{
    public sealed partial class CompetenceSearchPanel : UserControl
    {
        public CompetenceSearchPanel()
        {
            this.InitializeComponent();
        }

        public List<Competence> AllCompetences
        {
            get { return (List<Competence>)GetValue(AllCompetencesProperty); }
            set { SetValue(AllCompetencesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllCompetencesProperty =
            DependencyProperty.Register("AllCompetences", typeof(List<Competence>), typeof(CompetenceSearchPanel), null);



        public ObservableCollection<CompetenceDto> SelectedCompetences
        {
            get { return (ObservableCollection<CompetenceDto>)GetValue(SelectedCompetencesProperty); }
            set
            {
                SetValue(SelectedCompetencesProperty, value);

            }
        }

        // Using a DependencyProperty as the backing store for SelectedCompetences.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedCompetencesProperty =
            DependencyProperty.Register("SelectedCompetences", typeof(ObservableCollection<CompetenceDto>), typeof(CompetenceSearchPanel), null);

        public IMapper Mapper
        {
            get { return (IMapper)GetValue(MapperProperty); }
            set { SetValue(MapperProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Mapper.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapperProperty =
            DependencyProperty.Register("Mapper", typeof(IMapper), typeof(CompetenceSearchPanel), null);


        private void SetRemoveCommands()
        {
            foreach (var item in SelectedCompetences)
            {
                item.RemoveCommand = RemoveCompetenceCommand as RelayCommand<object>;
            }
        }

        private List<Competence> _selectableCompetences;
        public void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Since selecting an item will also change the text,
            // only listen to changes caused by user entering text.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<object>();
                var splitText = sender.Text.ToLower().Split(" ");

                if (_selectableCompetences == null)
                {
                    _selectableCompetences = AllCompetences
                        .Where(c => SelectedCompetences.Any(c1 => c.Id == c1.Id) == false)
                        .ToList();
                }

                //TODO Search for players in database

                foreach (var comp in _selectableCompetences)
                {
                    //var found = splitText.All((key) =>
                    //{
                    //    return player.Name.ToLower().Contains(key);
                    //});

                    //found
                    if (splitText.All((key) => comp.Name.ToLower().Contains(key)))
                    {
                        suitableItems.Add(Mapper.Map<CompetenceDto>(comp));
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
                //        //Command = NavigateToNewPlayerPageCommand,
                //    });
                //    suitableItems.Add(notfoundDisplay);
                //}
                sender.ItemsSource = suitableItems;
            }
        }
        private ICommand _removeCompetenceCommand;
        public ICommand RemoveCompetenceCommand => _removeCompetenceCommand ??= new RelayCommand<object>(RemoveCompetence);

        private void RemoveCompetence(object obj)
        {
            SelectedCompetences.Remove(obj as CompetenceDto);
        }

        public void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is CompetenceDto comp)
            {
                comp.RemoveCommand = RemoveCompetenceCommand as RelayCommand<object>;
                SelectedCompetences.Add(comp);
                _selectableCompetences = AllCompetences
                        .Where(c => SelectedCompetences.Any(c1 => c.Id == c1.Id) == false)
                        .ToList();
                sender.Text = string.Empty;
                sender.ItemsSource = null;
                return;
            }

            //if (args.SelectedItem is StackPanel notFoundDisplay)
            //{
            //    NavigateTo?.Invoke(this, new NavigateToEventargs { PageName = "Új játékos", PageVM = typeof(PlayerAddingViewModel) });
            //}
        }

        private void ItemsControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetRemoveCommands();
        }
    }
}
