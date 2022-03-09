using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Media.Imaging;
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


        private ICommand _launchGameCommand;
        public ICommand LaunchGameCommand => _launchGameCommand ??= new RelayCommand(LaunchGame);

        private void LaunchGame()
        {
            if(!string.IsNullOrEmpty(EntryPointLocation) && File.Exists(EntryPointLocation))
                Process.Start(EntryPointLocation);
        }

        public string Description { get => _description; set => _description = value; }
        public BitmapImage PreviewImage { get; set; }
        public string EntryPointLocation { get => _entryPointLocation; set => _entryPointLocation = value; }
        public string Name { get => _name; set => _name = value; }
        public string PreviewImageLocation { get => _previewImageLocation; set => _previewImageLocation = value; }
    }
}
