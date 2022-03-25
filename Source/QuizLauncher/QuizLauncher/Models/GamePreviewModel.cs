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
    public class GamePreviewModel
    {
        private string _name;
        private string _description;
        private BitmapImage _previewImage;
        private string _entryPointLocation;
        private string _previewImageLocation;
        private bool _isRelativePath;

        private ICommand _launchGameCommand;
        public ICommand LaunchGameCommand => _launchGameCommand ??= new RelayCommand(LaunchGame);

        private void LaunchGame()
        {
            var actualPath = EntryPointLocation;
            if(IsRelativePath)
            {
                actualPath = GameImportService.ConfigPath + EntryPointLocation;
            }
            if(!string.IsNullOrEmpty(actualPath) && File.Exists(actualPath))
                Process.Start(actualPath);
        }

        public string Description { get => _description; set => _description = value; }
        public BitmapImage PreviewImage { get; set; }
        public string EntryPointLocation { get => _entryPointLocation; set => _entryPointLocation = value; }
        public string Name { get => _name; set => _name = value; }
        public string PreviewImageLocation { get => _previewImageLocation; set => _previewImageLocation = value; }
        public bool IsRelativePath { get => _isRelativePath; set => _isRelativePath = value; }
    }
}
