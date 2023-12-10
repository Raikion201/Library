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
        // example read data: John Doe johndoe@gmail.com 
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

        // Write all borrower to CSV file
        public static void WriteBorrowersToCSV(List<Borrower> borrowers, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var borrower in borrowers)
                    {
                        string line = $"{borrower.Name},{borrower.ContactInformation}";
                        writer.WriteLine(line);
                    }
                }
                Console.WriteLine($"Borrower information written to {filePath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing Borrower information: {ex.Message}");
            }
        }

        // Display a borrower's information
        public void DisplayBorrowerInfo() {
            Console.WriteLine($"{Name,-25} {ContactInformation,-25}");
        }

        // Borrow book
        // If the book is available and the borrower has not borrowed it yet, borrow it
        // A borrower can borrow a certain book only once
        // And due date is 1 day from the time they borrow the book
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

                HistoryLog.Add(new HistoryLog(this, book, DateTime.Now, DateTime.Now.AddDays(1)));

                Console.WriteLine("Borrowed successfully!");
            }
        }

        // Helper method to check if a borrower has borrowed a book
        public bool HasBorrowedBook(Book book)
        {
            return BorrowedBooks.Contains(book);
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

        // This function is use for updating data when we start the program
        public void UpdateBorrowedBooksFromCSV(string borrowFilePath, string returnFilePath, List<Book> books, Borrower borrower)
        {
            // Read data from CSV file for borrowed books
            string[] borrowLines = File.ReadAllLines(borrowFilePath);

            // Read data from CSV file for returned books
            string[] returnLines = File.ReadAllLines(returnFilePath);

            // Create a list to store returned books
            List<string> returnedBooks = new List<string>();
            foreach (string line in returnLines)
            {
                string[] parts = line.Split(',');
                if (parts.Length >= 2 && parts[0].Trim() == borrower.Name)
                {
                    returnedBooks.Add(parts[1].Trim());
                }
            }

            // Update the list of borrowed books for borrower base on their returned books
            foreach (string borrowLine in borrowLines)
            {
                string[] parts = borrowLine.Split(',');
                if (parts.Length >= 2 && parts[0].Trim() == borrower.Name)
                {
                    string bookName = parts[1].Trim();
                    // Check if the book is in the returned books list
                    if (!returnedBooks.Contains(bookName))
                    {
                        // Find the book in the list of books by its title
                        Book foundBook = books.Find(book => book.Title == bookName);
                        if (foundBook != null)
                        {
                            borrower.BorrowedBooks.Add(foundBook);
                        }
                    }
                }
            }
        }
    }
}