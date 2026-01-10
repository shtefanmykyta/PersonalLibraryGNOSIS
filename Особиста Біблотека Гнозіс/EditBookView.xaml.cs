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
using System.Text.RegularExpressions;

namespace Особиста_Біблотека_Гнозіс
{
    public partial class EditBookView : UserControl
    {
        private Book _bookOriginal;

        public EditBookView(Book book)
        {
            InitializeComponent();
            _bookOriginal = book;

            TitleInput.Text = book.Title;
            AuthorInput.Text = book.Author;
            PagesInput.Text = book.Pages.ToString();
            YearInput.Text = book.Year.ToString();

            foreach (ComboBoxItem item in StatusInput.Items)
            {
                if (item.Content.ToString() == book.Status)
                {
                    StatusInput.SelectedItem = item;
                    break;
                }
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleInput.Text))
            {
                MessageBox.Show("Вкажіть назву книги!");
                return;
            }

            int maxYear = DateTime.Now.Year + 1;
            if (!int.TryParse(YearInput.Text, out int year) || year < 1450 || year > maxYear)
            {
                MessageBox.Show($"Будь ласка, введіть коректний рік видання (від 1450 до {maxYear})!");
                return;
            }

            var allBooks = DataService.LoadBooks();
            var bookInDb = allBooks.FirstOrDefault(b => b.Title == _bookOriginal.Title && b.Author == _bookOriginal.Author);

            if (bookInDb != null)
            {
                bookInDb.Title = TitleInput.Text;
                bookInDb.Author = AuthorInput.Text;
                bookInDb.Year = year;

                if (int.TryParse(PagesInput.Text, out int p))
                    bookInDb.Pages = p;

                if (StatusInput.SelectedItem is ComboBoxItem selectedItem)
                    bookInDb.Status = selectedItem.Content.ToString();

                DataService.SaveBooks(allBooks);

                MessageBox.Show("Успішно оновлено!");
                GoBack();
            }
            else
            {
                MessageBox.Show("Помилка: оригінальну книгу не знайдено в базі даних.");
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        private void GoBack()
        {
            var mainWin = Application.Current.MainWindow as MainWindow;
            if (mainWin != null)
            {
                mainWin.PagesControl.Content = new LibraryView();
            }
        }
    }
}