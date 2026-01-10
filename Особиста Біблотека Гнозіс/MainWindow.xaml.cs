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

namespace Особиста_Біблотека_Гнозіс
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowLibrary();
        }

        private void ShowLibrary()
        {
            PagesControl.Content = new LibraryView();
        }

        private void LibraryButton_Click(object sender, RoutedEventArgs e)
        {
            ShowLibrary();
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            PagesControl.Content = new AddBookView();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            PagesControl.Content = new SettingsView();
        }

        private void AnalyticsButton_Click(object sender, RoutedEventArgs e)
        {
            PagesControl.Content = new AnalyticsView();
        }
    }
}