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
    public class GenreStatItem
    {
        public string GenreName { get; set; }
        public int Count { get; set; }
        public int TotalBooks { get; set; }
        public string CountText => $"{Count} кн.";
    }

    public partial class AnalyticsView : UserControl
    {
        public AnalyticsView()
        {
            InitializeComponent();
            this.Loaded += AnalyticsView_Loaded;
        }

        private void AnalyticsView_Loaded(object sender, RoutedEventArgs e)
        {
            CalculateStatistics();
        }

        private void CalculateStatistics()
        {
            try
            {
                var books = DataService.LoadBooks();

                if (books == null || books.Count == 0)
                {
                    ResetInterface();
                    return;
                }

                TotalBooksText.Text = books.Count.ToString();
                TotalPagesText.Text = books.Sum(b => b.Pages).ToString();
                UniqueAuthorsText.Text = books.Select(b => b.Author).Distinct().Count().ToString();

                int home = books.Count(b => b.Status?.Trim().ToLower() == "в наявності");
                int borrowed = books.Count(b => b.Status?.Trim().ToLower() == "віддана");
                int ordered = books.Count(b => b.Status?.Trim().ToLower() == "замовлена");

                UpdateProgressBar(HomeProgress, home, books.Count);
                UpdateProgressBar(BorrowedProgress, borrowed, books.Count);
                UpdateProgressBar(OrderedProgress, ordered, books.Count);

                var stats = books.GroupBy(b => b.Genre)
                                 .Select(g => new GenreStatItem
                                 {
                                     GenreName = string.IsNullOrWhiteSpace(g.Key) ? "Інше" : g.Key,
                                     Count = g.Count(),
                                     TotalBooks = books.Count
                                 })
                                 .OrderByDescending(g => g.Count)
                                 .ToList();

                GenreList.ItemsSource = null;
                GenreList.ItemsSource = stats;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при оновленні аналітики: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateProgressBar(ProgressBar bar, int value, int total)
        {
            if (bar != null)
            {
                bar.Maximum = total > 0 ? total : 1;
                bar.Value = value;
            }
        }

        private void ResetInterface()
        {
            TotalBooksText.Text = "0";
            TotalPagesText.Text = "0";
            UniqueAuthorsText.Text = "0";
            HomeProgress.Value = 0;
            BorrowedProgress.Value = 0;
            OrderedProgress.Value = 0;
            GenreList.ItemsSource = null;
        }
    }
}