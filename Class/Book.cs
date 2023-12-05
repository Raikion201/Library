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
		public string BorrowStatus { get; set; }

		public Book(string id, string title, string author, Genres genre, string isbn, int quantity, bool available, string borrowStatus)
		{
			ID = id;
			Title = title;
			Author = author;
			Genre = genre;
			ISBN = isbn;
			Quantity = quantity;
			Availability = available;
			BorrowStatus = borrowStatus;
		}
	}
	public class BookReader
	{
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

						Book book = new Book(data[0], data[1], data[2], bookGenre, data[4], int.Parse(data[5]), bool.Parse(data[6]), data[7]);

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
	}
}