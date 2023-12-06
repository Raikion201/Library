using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace Library
{
    public class Borrower : User
    {
        public string ContactInformation { get; set; }
        public string BorrowerID { get; set; }
		public List<HistoryLog> HistoryLog { get; private set; }
        public Borrower(string name, string contactInformation, string borrowerId)
        {
            Name = name;
            ContactInformation = contactInformation;
            BorrowerID = borrowerId;
			HistoryLog = new List<HistoryLog>();
        }

        // Read all borrower from CSV file
        // example read data: John Doe johndoe@gmail.com 75
        public static List<Borrower> ReadBorrowersFromCSV(string filePath) {
            List<Borrower> borrowers = new List<Borrower>();

            try
			{
				using (TextFieldParser parser = new TextFieldParser(filePath))
				{
					parser.TextFieldType = FieldType.Delimited;
					parser.SetDelimiters(",");

					while (!parser.EndOfData)
					{
                        string[] data = parser.ReadFields()!;

                        Borrower borrower = new Borrower(data[0], data[1], data[2]);

                        borrowers.Add(borrower);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}

            return borrowers;
        }

        // Borrow book
        public void BorrowBook(Borrower borrower, Book book) 
        {
            if (book.Availability == true && book.Quantity > 0) 
            {
                book.Quantity--;
                if (book.Quantity == 0) book.Availability = false;
                book.BorrowStatus = true;
                book.Borrowers.Add(this);

                // Add history log, borrow date is today, return date is 1 days from today
                HistoryLog.Add(new HistoryLog(this, book, DateTime.Now, DateTime.Now.AddDays(1)));
            }
        }

        // Return book
        public void ReturnBook(Borrower borrower, Book book) 
        {
            // Find the log record to return
            HistoryLog recordToReturn = HistoryLog.FirstOrDefault(record => record.Book == book && record.Borrower == borrower);
            
            if (recordToReturn != null) 
            {
                book.Quantity++;
                book.Availability = book.Quantity > 0;
                book.BorrowStatus = false;
                // Remove the borrower from the book
                book.Borrowers.Remove(borrower);
            }
            // Remove the log record from the borrower
            // If we don't remove it, the borrower will have a list of log records that are already returned
            HistoryLog.Remove(recordToReturn);
        }
    }
}