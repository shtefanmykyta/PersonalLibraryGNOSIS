using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Особиста_Біблотека_Гнозіс
{
    // Модель налаштувань
    public class UserSettings
    {
        public string SelectedTheme { get; set; } = "Dark";

        public double TextScale { get; set; } = 16.0;
    }

    public static class DataService
    {
        private static string filePath = "books.json";
        private static string settingsPath = "settings.json";

        public static void SaveBooks(List<Book> books)
        {
            string json = JsonConvert.SerializeObject(books, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static List<Book> LoadBooks()
        {
            if (!File.Exists(filePath))
            {
                return new List<Book>();
            }

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Book>>(json) ?? new List<Book>();
        }

        public static void SaveSettings(UserSettings settings)
        {
            if (settings.TextScale < 12) settings.TextScale = 16.0;

            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(settingsPath, json);
        }

        public static UserSettings LoadSettings()
        {
            try
            {
                if (!File.Exists(settingsPath))
                    return new UserSettings();

                string json = File.ReadAllText(settingsPath);
                var settings = JsonConvert.DeserializeObject<UserSettings>(json);

                if (settings == null || settings.TextScale < 1)
                    return new UserSettings();

                return settings;
            }
            catch
            {
                return new UserSettings();
            }
        }
    }
}