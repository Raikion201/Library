using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace Library
{
    public class User
    {
        // List of all books in the library
        public virtual void DisplayLibrary(List<Book> books)
        {
            Console.WriteLine(new string('-', 200));
            Console.WriteLine("{0,-5} {1,-90} {2,-30} {3,-18} {4,-15} {5,-10} {6,-10} {7,-10}",
                "ID", "Title", "Author", "Genre", "ISBN", "Quantity", "Availability", "BorrowStatus");
            Console.WriteLine(new string('-', 200));
            
            foreach (var book in books)
            {
                Console.WriteLine($"{book.ID,-5} {book.Title,-90} {book.Author,-30} {book.Genre,-18} {book.ISBN,-15} {book.Quantity,8} {book.Availability,-12} {book.BorrowStatus,-10}");
            }
        }

        // In ascending order (A-Z) (smallest to largest for numbers) (true to false for boolean)
        public IEnumerable<Book> SortBooks(IEnumerable<Book> books, string sortBy)
        {
            switch (sortBy)
            {
                case "ID":
                    // Parse the ID to int to sort
                    // If we don't parse it to int, it will sort by the first character of the ID because ID is string
                    // For example: 1, 10, 11, 2, 3, 4, 5, 6, 7, 8, 9
                    // If we parse it to int, it will sort by the number
                    // For example: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11
                    return books.OrderBy(book => int.Parse(book.ID)); 
                case "Title":
                    return books.OrderBy(book => book.Title);
                case "Author":
                    return books.OrderBy(book => book.Author);
                case "Genre":
                    return books.OrderBy(book => book.Genre);
                case "Quantity":
                    return books.OrderBy(book => book.Quantity);
                case "Availability":
                    return books.OrderBy(book => book.Availability);
                case "BorrowStatus":
                    return books.OrderByDescending(book => book.BorrowStatus);
                default:
                    return books;
            }
        }

        public List<Book> SearchBooks(List<Book> books, string keyword)
        {
            List<Book> foundBooks = new List<Book>();

            foreach (Book book in books)
            {
                if (book.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    book.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    book.Genre.ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    foundBooks.Add(book);
                }
            }

            return foundBooks;
        }
    }
}
