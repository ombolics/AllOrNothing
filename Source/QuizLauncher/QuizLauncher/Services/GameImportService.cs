using QuizLauncher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuizLauncher.Services
{
    public class GameImportService
    {
        public static void CreateConfigFolder()
        {
            var rootLocation = AppDomain.CurrentDomain.BaseDirectory;//Assembly.GetExecutingAssembly().Location.;
            _configPath = @$"{rootLocation}Config\";
            if (!Directory.Exists(_configPath))
            {
                Directory.CreateDirectory(_configPath);
            }
            
            if (!File.Exists($"{ConfigPath}\\launcherSettings.json"))
            {
                File.Create($"{ConfigPath}\\launcherSettings.json");
            }

        }
        private static string _configPath;
        public static string ConfigPath => _configPath;

        private string _gameDirectoryPath;

        public string GameDirectoryPath { get => _gameDirectoryPath; set => _gameDirectoryPath = value; }

        public List<GamePreviewModel> ListAllGame()
        {
            if (ConfigPath == null)
                throw new InvalidOperationException("Config path must have a value for this operation");

            var configFile = File.OpenRead($"{ConfigPath}\\launcherSettings.json");
            StreamReader sr = new StreamReader(configFile);
            var value = JsonSerializer.Deserialize<List<GamePreviewModel>>(sr.ReadToEnd());
            
            sr.Close();
            sr.Dispose();
            configFile.Close();
            configFile.Dispose();

            return value;
        }
    }
}
