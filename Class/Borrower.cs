using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace Library
{
    public class Borrower : User
    {
        public string Name { get; set; }
        public string ContactInformation { get; set; }
        public List<Book> BorrowedBooks { get; private set; } // List of borrowed books
		public List<HistoryLog> HistoryLog { get; private set; }

        // Default constructor with no parameters
        public Borrower(){}

        // Constructor with parameters
        public Borrower(string name, string contactInformation, List<Book> borrowedBooks)
        {
            Name = name;
            ContactInformation = contactInformation;
            BorrowedBooks = borrowedBooks ?? new List<Book>();
            HistoryLog = new List<HistoryLog>();
        }

        // Read all borrower from CSV file
        // example read data: John Doe johndoe@gmail.com 75
        public static List<Borrower> ReadBorrowersFromCSV(string filePath)
        {
            List<Borrower> borrowers = new List<Borrower>();
            try
            {
                using (TextFieldParser parser = new TextFieldParser(filePath))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    while (!parser.EndOfData)
                    {
                        string[] data = parser.ReadFields();
                        
                        string name = data[0];
                        string contactInfo = data[1];

                        List<Book> borrowedBooks = new List<Book>(); 

                        // Start from index 2 to read book IDs
                        // Because index 0 is name, index 1 is contact info
                        for (int i = 2; i < data.Length; i++)
                        {
                            string bookId = data[i];
                            Book book = Book.GetBookById(bookId);
                            if (book != null)
                                borrowedBooks.Add(book);
                        }

                        Borrower borrower = new Borrower(name, contactInfo, borrowedBooks); 
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

        // Display a borrower's information
        public void DisplayBorrowerInfo() {
            Console.WriteLine($"{Name,-25} {ContactInformation,-25}");
        }

        // // Display a borrower's history log
        // public void DisplayBorrowerInfo() {
        //     Console.WriteLine(" ");
        //     Console.WriteLine(new string('-', 200));
        //     Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20}",
        //         "Name", "Contact Information", "Borrower ID", "Number of Borrowed Books");
        //     Console.WriteLine(new string('-', 200));
        //     Console.WriteLine($"{Name,-20} {ContactInformation,-20} {BorrowerID,-20} {HistoryLog.Count,-20}");
        // }

        // Borrow book
        public void BorrowBook(Borrower borrower, Book book) 
        {
            if (book.Availability == true) 
            {
                book.Quantity--;
                if (book.Quantity == 0) book.Availability = false;
                book.BorrowStatus = true;

                // Add the borrower to the book
                // Add the book to the borrower
                book.Borrowers.Add(this);
                borrower.BorrowedBooks.Add(book);

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
                // Remove the book from the borrower
                book.Borrowers.Remove(borrower);
                borrower.BorrowedBooks.Remove(book);
            }
            // Remove the log record from the borrower
            // If we don't remove it, the borrower will have a list of log records that are already returned
            HistoryLog.Remove(recordToReturn);
        }

        // Get a borrower by its name
        public static Borrower GetBorrowerByName(string borrowerName, List<Borrower> borrowers)
        {
            Borrower foundBorrower = borrowers.FirstOrDefault(borrower => borrower.Name == borrowerName);
            return foundBorrower; // Return the found Borrower object or null if not found
        }
    }
}