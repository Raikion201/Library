using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace Library
{
	public class Book
	{
		public string ID { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public Genres Genre { get; set; }
		public string ISBN { get; set; }
		public int Quantity { get; set; }
		public bool Availability { get; set; } // true if the book is available to borrow, false if not (when quantity below 0 or the book is lost)
		public bool BorrowStatus { get; set; } // true if the book is borrowed by someone, false if not
		public List<Borrower> Borrowers { get; set; }

		// Default constructor
		public Book(){}

		// Constructor with parameters
		public Book(string id, string title, string author, Genres genre, string isbn, int quantity, bool available, bool borrowStatus)
		{
			ID = id;
			Title = title;
			Author = author;
			Genre = genre;
			ISBN = isbn;
			Quantity = quantity;
			Availability = available;
			BorrowStatus = borrowStatus;
			Borrowers = new List<Borrower>();
		}

		// Read all book data from a CSV file
		// Data include: ID, Title, Author, Genre, ISBN, Quantity, Availability, and BorrowStatus
		public static List<Book> ReadBooksFromCSV(string filePath)
		{
			List<Book> books = new List<Book>();

			try
			{
				using (TextFieldParser parser = new TextFieldParser(filePath))
				{
					parser.TextFieldType = FieldType.Delimited;
					parser.SetDelimiters(",");

					while (!parser.EndOfData)
					{
						string[] data = parser.ReadFields()!;

						// Parse the genre directly to Genres enum
						Genres bookGenre = (Genres)Enum.Parse(typeof(Genres), data[3]);

						Book book = new Book(data[0], data[1], data[2], bookGenre, data[4], int.Parse(data[5]), bool.Parse(data[6]), bool.Parse(data[7]));

						books.Add(book);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}

			return books;
		}

		// Write all book data to a CSV file
		// In format: ID,Title,Author,Genre,ISBN,Quantity,Availability,BorrowStatus
		public static void WriteBooksToCSV(List<Book> books, string filePath)
		{
			try
			{
				using (var writer = new System.IO.StreamWriter(filePath))
				{
					foreach (var book in books)
					{
						// Format the book data into a CSV row
						string bookData = $"{book.ID},{book.Title},{book.Author},{book.Genre},{book.ISBN},{book.Quantity},{book.Availability},{book.BorrowStatus}";

						// Write the formatted data to the CSV file
						writer.WriteLine(bookData);
					}
				}
				Console.WriteLine("Book data has been successfully written to the CSV file.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
			}
		}

		// Display a book's information
		public void DisplayBookInfo(Book book)
		{
			Console.WriteLine($"{book.ID,-5} {book.Title,-90} {book.Author,-30} {book.Genre,-18} {book.ISBN,-15} {book.Quantity,-10} {book.Availability,-12} {book.BorrowStatus,-10}");
		}

		// Get a book by its ID
		public static Book GetBookById(string bookID)
		{
			List<Book> books = new List<Book>(); 

			// Find the book with the given ID
			Book book = books.FirstOrDefault(b => b.ID == bookID);
			return book; // Return the found Book object or null if not found
		}

		// Get a book by its title
		public static Book GetBookByTitle(string bookTitle, List<Book> books)
		{
			Book book = books.FirstOrDefault(book => book.Title == bookTitle);
			return book; // Return the found Book object or null if not found
		}
	}
}