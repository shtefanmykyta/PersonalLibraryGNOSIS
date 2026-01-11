using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Особиста_Біблотека_Гнозіс
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int Pages { get; set; }
        public string Status { get; set; }
        public int Year { get; set; }

        public Book() { }

        public Book(string title, string author, string genre,int year, int pages, string status)
        {
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
            Pages = pages;
            Status = status;
        }
    }
}
