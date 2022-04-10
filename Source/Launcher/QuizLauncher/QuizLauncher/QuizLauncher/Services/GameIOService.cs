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
    public class GameIOService
    {
        public static void CreateConfigFolder()
        {
            var rootLocation = AppDomain.CurrentDomain.BaseDirectory;//Assembly.GetExecutingAssembly().Location.;
            // C:\Szakdolgozat\Szakdolgozat\QuizProgram_LauncherConcept\Source\Launcher\QuizLauncher\QuizLauncher\QuizLauncher (Package)\bin\x86\Debug\AppX\QuizLauncher   \Config\
            //  @"..\..\..\..\..\..\\QuizLauncher\bin\x86\Debug\AppX\QuizLauncher\Config"
            //_configPath = $@"{rootLocation}..\..\..\..\..\..\\QuizLauncher\bin\x86\Debug\AppX\QuizLauncher\Config";// @$"{rootLocation}Config\";
            _configPath = @$"{System.AppDomain.CurrentDomain.BaseDirectory}..\\Config1";
            if (!Directory.Exists(_configPath))
            {
                Directory.CreateDirectory(_configPath);
            }
            
            if (!File.Exists($"{ConfigPath}\\GameConfig.json"))
            {
                File.Create($"{ConfigPath}\\GameConfig.json");
            }

        }
        private static string _configPath;
        public static string ConfigPath => _configPath;

        private string _gameDirectoryPath;

        public string GameDirectoryPath { get => _gameDirectoryPath; set => _gameDirectoryPath = value; }

        public void SaveGames(List<GamePreviewModel> games)
        {
            StreamWriter sw = new StreamWriter($"{ConfigPath}\\GameConfig.json", false, Encoding.UTF8);
            bool hasErrors = false;

          
            var value = JsonSerializer.Serialize(games);
            sw.Write(value);

            sw.Close();
            sw.Dispose();
        }

        public List<GamePreviewModel> ListAllGame()
        {
            if (ConfigPath == null)
                throw new InvalidOperationException("Config path must have a value for this operation");

            var configFile = File.OpenRead($"{ConfigPath}\\GameConfig.json");
            StreamReader sr = new StreamReader(configFile, Encoding.UTF8);
            bool hasErrors = false;
            
            List<GamePreviewModel> value;
            var content = sr.ReadToEnd();
            value = JsonSerializer.Deserialize<List<GamePreviewModel>>(content);
            try
            {
                
            }
            catch
            {
                hasErrors = true;
                value = new List<GamePreviewModel>();
            }



            
            sr.Close();
            sr.Dispose();
            configFile.Close();
            configFile.Dispose();

            return value;
        }
    }
}
