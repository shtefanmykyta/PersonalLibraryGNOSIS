using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Особиста_Біблотека_Гнозіс
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                var settings = DataService.LoadSettings();
                ApplyTheme(settings.SelectedTheme);

                if (settings.TextScale > 0)
                {
                    Application.Current.Resources["GlobalFontSize"] = settings.TextScale;
                }
                else
                {
                    Application.Current.Resources["GlobalFontSize"] = 16.0; // Стандарт, якщо файл порожній
                }
            }
            catch
            {
                ApplyTheme("Dark");
                Application.Current.Resources["GlobalFontSize"] = 16.0;
            }
        }

        public void ApplyTheme(string themeName)
        {
            string themeUri = "";

            switch (themeName)
            {
                case "Dark": themeUri = "Themes/DarkTheme.xaml"; break;
                case "Mint": themeUri = "Themes/MintTheme.xaml"; break;
                case "Pink": themeUri = "Themes/PinkTheme.xaml"; break;
                case "Peach": themeUri = "Themes/PeachTheme.xaml"; break;
                default: themeUri = "Themes/DarkTheme.xaml"; break;
            }

            try
            {
                ResourceDictionary dict = new ResourceDictionary
                {
                    Source = new Uri(themeUri, UriKind.Relative)
                };

                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Не вдалося завантажити тему: {ex.Message}");
            }
        }
    }
}