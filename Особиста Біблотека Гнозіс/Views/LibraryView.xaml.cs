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
    public partial class LibraryView : UserControl
    {
        private List<Book> _allBooks = new List<Book>();

        public LibraryView()
        {
            InitializeComponent();
            RefreshData();
            StatusFilter.SelectedIndex = 0;
        }

        private void RefreshData()
        {
            _allBooks = DataService.LoadBooks() ?? new List<Book>();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_allBooks == null) return;

            string searchText = SearchBox.Text.Trim().ToLower();
            string selectedStatus = (StatusFilter.SelectedItem as ComboBoxItem)?.Content.ToString();

            var filtered = _allBooks.Where(b =>
                (string.IsNullOrEmpty(searchText) ||
                 (b.Title?.ToLower().Contains(searchText) ?? false) ||
                 (b.Author?.ToLower().Contains(searchText) ?? false) ||
                 (b.Year.ToString().Contains(searchText))) &&
                (selectedStatus == "Всі" || b.Status == selectedStatus)
            )
            .OrderByDescending(b => b.Year) // Спочатку новіші книги
            .ThenBy(b => b.Title)           // Якщо рік однаковий сортуємо за назвою від А до Я
            .ToList();

            BooksGrid.ItemsSource = filtered;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void DeleteSelectedBook_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BooksGrid.SelectedItem as Book;

            if (selectedBook == null)
            {
                MessageBox.Show("Будь ласка, спочатку виберіть книгу у списку!", "Попередження");
                return;
            }

            var result = MessageBox.Show($"Ви впевнені, що хочете видалити книгу \"{selectedBook.Title}\"?",
                                       "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _allBooks.RemoveAll(b =>
                    b.Title == selectedBook.Title &&
                    b.Author == selectedBook.Author &&
                    b.Year == selectedBook.Year);

                DataService.SaveBooks(_allBooks);
                ApplyFilters();
            }
        }

        private void BooksGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BooksGrid.SelectedItem is Book selectedBook)
            {
                var mainWin = Application.Current.MainWindow as MainWindow;
                if (mainWin != null)
                {
                    mainWin.PagesControl.Content = new EditBookView(selectedBook);
                }
            }
        }
    }
}
