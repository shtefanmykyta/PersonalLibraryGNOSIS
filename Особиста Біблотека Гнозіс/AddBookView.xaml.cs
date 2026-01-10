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
    public partial class AddBookView : UserControl
    {
        public AddBookView()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SaveBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedGenre = (GenreInput.SelectedItem as ComboBoxItem)?.Content.ToString();
                var selectedStatus = (StatusInput.SelectedItem as ComboBoxItem)?.Content.ToString();

                int currentYearPlusOne = DateTime.Now.Year + 1;
                if (!int.TryParse(YearInput.Text, out int y) || y < 1450 || y > currentYearPlusOne)
                {
                    MessageBox.Show($"Будь ласка, вкажіть коректний рік видання (від 1450 до {currentYearPlusOne})!", "Увага");
                    return;
                }

                if (string.IsNullOrWhiteSpace(TitleInput.Text) || string.IsNullOrEmpty(selectedGenre))
                {
                    MessageBox.Show("Будь ласка, введіть назву книги та оберіть жанр!", "Увага");
                    return;
                }

                var newBook = new Book
                {
                    Title = TitleInput.Text,
                    Author = AuthorInput.Text,
                    Genre = selectedGenre,
                    Year = y,
                    Pages = int.TryParse(PagesInput.Text, out int p) ? p : 0,
                    Status = selectedStatus ?? "Не вказано"
                };

                List<Book> books = DataService.LoadBooks();
                books.Add(newBook);
                DataService.SaveBooks(books);

                MessageBox.Show("Книгу успішно додано до бібліотеки!", "Успіх");
                ClearForm();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Сталася помилка при збереженні: " + ex.Message, "Помилка");
            }
        }

        private void ClearForm()
        {
            TitleInput.Text = "";
            AuthorInput.Text = "";
            PagesInput.Text = "";
            YearInput.Text = "";
            GenreInput.SelectedIndex = -1;
            StatusInput.SelectedIndex = -1;
        }
    }
}