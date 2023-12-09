using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;

namespace Library 
{
    public class HistoryLog 
    {
        public Borrower Borrower { get; }
        public Book Book { get; }
        public DateTime BorrowDate { get; }
        public DateTime ReturnDate { get; }

        // Default constructor
        public HistoryLog() {}

        // Constructor with parameters
        public HistoryLog(Borrower borrower, Book book, DateTime borrowDate, DateTime returnDate)
        {
            Borrower = borrower;
            Book = book;
            BorrowDate = borrowDate;
            ReturnDate = returnDate;
        }

        // Constructor with parameters for return log
        public HistoryLog(Borrower borrower, Book book, DateTime returnDate)
        {
            Borrower = borrower;
            Book = book;
            ReturnDate = returnDate;
        }

        // Read all borrow log from CSV file
        public static List<HistoryLog> ReadBorrowLogFromCSV(string filePath, List<Borrower> borrowers, List<Book> books)
        {
            List<HistoryLog> borrowLogs = new List<HistoryLog>();

            try
            {
                using (TextFieldParser parser = new TextFieldParser(filePath))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    while (!parser.EndOfData)
                    {
                        string[] data = parser.ReadFields();

                        string borrowerName = data[0];
                        string bookTitle = data[1];
                        DateTime borrowDate = DateTime.ParseExact(data[2], "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                        DateTime returnDate = DateTime.ParseExact(data[3], "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                        
                        // Get the borrower and book object from the list
                        // We need to get the object from the list because we only have the name and title
                        Borrower borrower = Borrower.GetBorrowerByName(borrowerName, borrowers);
                        Book book = Book.GetBookByTitle(bookTitle, books);
                        
                        HistoryLog borrowLog = new HistoryLog(borrower, book, borrowDate, returnDate);
                        borrowLogs.Add(borrowLog);
                    }
                }
                Console.WriteLine($"Borrow history log read from {filePath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading borrow history log: {ex.Message}");
            }
            return borrowLogs;
        }

        // Read all return log from CSV file
        public static List<HistoryLog> ReadReturnLogFromCSV(string filePath, List<Borrower> borrowers, List<Book> books)
        {
            List<HistoryLog> returnLogs = new List<HistoryLog>();

            try
            {
                using (TextFieldParser parser = new TextFieldParser(filePath))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    while (!parser.EndOfData)
                    {
                        string[] data = parser.ReadFields();

                        string borrowerName = data[0];
                        string bookTitle = data[1];
                        DateTime returnDate = DateTime.ParseExact(data[2], "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                        // Get the borrower and book object from the list
                        // We need to get the object from the list because we only have the name and title
                        Borrower borrower = Borrower.GetBorrowerByName(borrowerName, borrowers);
                        Book book = Book.GetBookByTitle(bookTitle, books);

                        HistoryLog returnLog = new HistoryLog(borrower, book, returnDate);
                        returnLogs.Add(returnLog);
                    }
                }
                Console.WriteLine($"Return history log read from {filePath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading return history log: {ex.Message}");
            }
            return returnLogs;
        }

        // Export borrow log to CSV file
        // Example data: John Doe,The Hobbit,01/01/2021 12:00:00 AM,02/01/2021 12:00:00 AM
        public void ExportBorrowLogToCSV(string filePath)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
                {
                    string line = $"{Borrower.Name},{Book.Title},{BorrowDate:dd/MM/yyyy h:mm:ss tt},{ReturnDate:dd/MM/yyyy h:mm:ss tt}";
                    file.WriteLine(line);
                }
                Console.WriteLine($"HistoryLog exported to {filePath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while exporting HistoryLog: {ex.Message}");
            }
        }

        // Export return log to CSV file
        // Example data: John Doe,The Hobbit,02/01/2021 11:00:00 AM
        public void ExportReturnLogToCSV(string filePath)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true)) 
                {
                    string line = $"{Borrower.Name},{Book.Title},{DateTime.Now:dd/MM/yyyy h:mm:ss tt}";
                    file.WriteLine(line);
                }
                Console.WriteLine($"ReturnLog exported to {filePath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while exporting ReturnLog: {ex.Message}");
            }
        }

        // Display all borrow log
        public void DisplayBorrowedBooks(List<HistoryLog> historyLogs)
        {
            foreach (var log in historyLogs)
            {
                Console.WriteLine($"{log.Borrower.Name},{log.Book.Title},{log.BorrowDate:dd/MM/yyyy h:mm:ss tt},{log.ReturnDate:dd/MM/yyyy h:mm:ss tt}");
            }
        }

        // Display all return log
        public void DisplayReturnedBooks(List<HistoryLog> historyLogs)
        {
            foreach (var log in historyLogs)
            {
                string returnDate = log.ReturnDate.ToString("dd/MM/yyyy h:mm:ss tt");
                Console.WriteLine($"{log.Borrower.Name},{log.Book.Title},{returnDate}");
            }
        }

        public void DisplayLogsByBorrower(List<HistoryLog> historyLogs, string borrowerName)
        {
            var logsByBorrower = historyLogs.Where(log => log.Borrower.Name.Equals(borrowerName, StringComparison.OrdinalIgnoreCase));

            if (logsByBorrower.Any())
            {
                Console.WriteLine($"Logs for borrower '{borrowerName}':");
                Console.WriteLine(new string('-', 100));
                Console.WriteLine("{0,-20} {1,-30} {2,-25} {3,-25}",
                                    "Borrower", "Title", "Borrow Date", "Due Date");
                Console.WriteLine(new string('-', 100));
                foreach (var log in logsByBorrower)
                {
                    var borrower = log.Borrower.Name.PadRight(20);
                    var title = log.Book.Title.PadRight(30);
                    var borrowDate = log.BorrowDate.ToString("dd/MM/yyyy h:mm:ss tt").PadRight(25);
                    var returnDate = log.ReturnDate.ToString("dd/MM/yyyy h:mm:ss tt").PadRight(25);

                    Console.WriteLine($"{borrower} {title} {borrowDate} {returnDate}");
                }
            }
            else
            {
                Console.WriteLine($"No logs found for borrower '{borrowerName}'.");
            }
        }

        public void DisplayLogsByBookTitle(List<HistoryLog> historyLogs, string bookTitle)
        {
            var logsByBookTitle = historyLogs.Where(log => log.Book.Title.Equals(bookTitle, StringComparison.OrdinalIgnoreCase));

            if (logsByBookTitle.Any())
            {
                Console.WriteLine($"Logs for book '{bookTitle}':");
                Console.WriteLine(new string('-', 100));
                Console.WriteLine("{0,-20} {1,-30} {2,-25} {3,-25}",
                                    "Borrower", "Title", "Borrow Date", "Due Date");
                Console.WriteLine(new string('-', 100));
                foreach (var log in logsByBookTitle)
                {
                    var borrower = log.Borrower.Name.PadRight(20);
                    var title = log.Book.Title.PadRight(30);
                    var borrowDate = log.BorrowDate.ToString("dd/MM/yyyy h:mm:ss tt").PadRight(25);
                    var returnDate = log.ReturnDate.ToString("dd/MM/yyyy h:mm:ss tt").PadRight(25);

                    Console.WriteLine($"{borrower} {title} {borrowDate} {returnDate}");
                }
            }
            else
            {
                Console.WriteLine($"No logs found for book '{bookTitle}'.");
            }
        }

        public void DisplayLogsByDate(List<HistoryLog> historyLogs, DateTime date)
        {
            var logsByDate = historyLogs.Where(log => log.BorrowDate.Date == date.Date || log.ReturnDate.Date == date.Date);

            if (logsByDate.Any())
            {
                Console.WriteLine($"Logs for date '{date:dd/MM/yyyy}':");
                Console.WriteLine(new string('-', 100));
                Console.WriteLine("{0,-20} {1,-30} {2,-25} {3,-25}",
                                    "Borrower", "Title", "Borrow Date", "Due Date");
                Console.WriteLine(new string('-', 100));
                foreach (var log in logsByDate)
                {
                    var borrower = log.Borrower.Name.PadRight(20);
                    var title = log.Book.Title.PadRight(30);
                    var borrowDate = log.BorrowDate.ToString("dd/MM/yyyy h:mm:ss tt").PadRight(25);
                    var returnDate = log.ReturnDate.ToString("dd/MM/yyyy h:mm:ss tt").PadRight(25);

                    Console.WriteLine($"{borrower} {title} {borrowDate} {returnDate}");
                }
            }
            else
            {
                Console.WriteLine($"No logs found for date '{date:dd/MM/yyyy}'.");
            }
        }

        public void DisplayLateReturn(List<HistoryLog> borrowLogs, List<HistoryLog> returnLogs)
        {
            Console.WriteLine("Late Returns:");
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("{0,-20} {1,-30} {2,-25} {3,-25}",
                                "Borrower", "Title", "Return Date", "Current Date");
            Console.WriteLine(new string('-', 100));

            DateTime currentDate = DateTime.Now;

            foreach (var borrowLog in borrowLogs)
            {
                DateTime returnDate = borrowLog.ReturnDate;

                // Check if the current date is later than the return date
                if (currentDate > returnDate)
                {
                    // Check if the book has been returned
                    if (!BookReturned(borrowLog, returnLogs))
                    {
                        var borrower = borrowLog.Borrower.Name.PadRight(20);
                        var title = borrowLog.Book.Title.PadRight(30);
                        var returnDateString = returnDate.ToString("dd/MM/yyyy h:mm:ss tt").PadRight(25);
                        var currentDateString = currentDate.ToString("dd/MM/yyyy h:mm:ss tt").PadRight(25);

                        Console.WriteLine($"{borrower} {title} {returnDateString} {currentDateString}");
                    }
                }
            }
        }

        // Helper method to check if a book has been returned
        private bool BookReturned(HistoryLog log, List<HistoryLog> historyLogs)
        {
            foreach (var returnLog in historyLogs)
            {
                if (returnLog.Borrower.Name == log.Borrower.Name && returnLog.Book.Title == log.Book.Title)
                {
                    return true; // If the book has been returned, return true
                }
            }
            return false; // Else return false
        }

    }
}