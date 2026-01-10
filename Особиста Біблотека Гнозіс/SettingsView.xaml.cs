using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using static Особиста_Біблотека_Гнозіс.DataService;

namespace Особиста_Біблотека_Гнозіс
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            InitializeSlider();
        }

        private void InitializeSlider()
        {
            FontSizeSlider.ValueChanged -= FontSizeSlider_ValueChanged;
            var settings = DataService.LoadSettings();
            FontSizeSlider.Value = settings.TextScale;
            FontSizeSlider.ValueChanged += FontSizeSlider_ValueChanged;
        }

        private void FontSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Application.Current.Resources["GlobalFontSize"] = e.NewValue;

            var settings = DataService.LoadSettings();
            settings.TextScale = e.NewValue;
            DataService.SaveSettings(settings);
        }

        private void ApplyTheme(string themeName)
        {
            var dictionaries = Application.Current.Resources.MergedDictionaries;
            ResourceDictionary newTheme = new ResourceDictionary { Source = new Uri($"Themes/{themeName}.xaml", UriKind.Relative) };
            if (dictionaries.Count > 0) dictionaries[0] = newTheme;
            else dictionaries.Add(newTheme);
        }

        private void SaveTheme(string themeName)
        {
            var settings = DataService.LoadSettings();
            settings.SelectedTheme = themeName;
            DataService.SaveSettings(settings);
        }

        private void SetDarkTheme_Click(object sender, RoutedEventArgs e) { ApplyTheme("DarkTheme"); SaveTheme("Dark"); }
        private void SetMintTheme_Click(object sender, RoutedEventArgs e) { ApplyTheme("MintTheme"); SaveTheme("Mint"); }
        private void SetPinkTheme_Click(object sender, RoutedEventArgs e) { ApplyTheme("PinkTheme"); SaveTheme("Pink"); }
        private void SetPeachTheme_Click(object sender, RoutedEventArgs e) { ApplyTheme("PeachTheme"); SaveTheme("Peach"); }
    }
}