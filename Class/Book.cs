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
		public bool Availability { get; set; }
		public bool BorrowStatus { get; set; }
		public List<Borrower> Borrowers { get; set; }

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

			// If someone has borrowed this book, find all borrowers
			if (borrowStatus) Borrowers = FindBorrowersByID(id);
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

		// Find all borrowers of a book by matched their ID with bookID
		private List<Borrower> FindBorrowersByID(string bookID)
		{
			List<Borrower> allBorrowers = Borrower.ReadBorrowersFromCSV(@"D:\coding\Library\Library\DOC\BORROWER\BorrowerInfo.csv");
			List<Borrower> foundBorrowers = new List<Borrower>();

			foreach (Borrower borrower in allBorrowers)
			{
				if (borrower.BorrowerId.ToString() == bookID)
				{
					foundBorrowers.Add(borrower);
				}
			}
			return foundBorrowers;
		}
	}
}