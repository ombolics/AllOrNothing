using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using QuizLauncher.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuizLauncher.Models
{
    public class GamePreviewModel : ObservableRecipient
    {
        private string _name;
        private string _description;
        private BitmapImage _previewImage;
        private string _entryPointLocation;
        private string _previewImageLocation;
        private bool _isRelativePath;

        private ICommand _launchGameCommand;
        public ICommand LaunchGameCommand => _launchGameCommand ??= new RelayCommand(LaunchGame);
        public event Action ErrorWhileOpening;
        private void LaunchGame()
        {
            var actualPath = EntryPointLocation;
            if(IsRelativePath)
            {
                actualPath = GameIOService.ConfigPath + EntryPointLocation;
            }
            if(!string.IsNullOrEmpty(actualPath) && File.Exists(actualPath))
            {
                try
                {
                    Process.Start(actualPath);
                }
                catch (Exception)
                {

                    ErrorWhileOpening?.Invoke();
                }
            }
                
        }

        public string Description 
        { 
            get => _description;
            set => SetProperty(ref _description, value); 
        }
        public BitmapImage PreviewImage { get; set; }
        public string EntryPointLocation 
        { 
            get => _entryPointLocation;
            set => SetProperty(ref _entryPointLocation, value); 
        }
        public string Name 
        { 
            get => _name;
            set => SetProperty(ref _name, value); 
        }
        public string PreviewImageLocation 
        { 
            get => _previewImageLocation; 
            set => SetProperty(ref _previewImageLocation, value);
        }
        public bool IsRelativePath { get => _isRelativePath; set => _isRelativePath = value; }
    }
}
