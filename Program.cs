using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Globalization;

namespace Library
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Relative path to the CSV files
            // When run the program it will load all data before the menu is show
            string storePath = @"D:/coding/Library/Library/DOC/STORE/Store.csv";
            string borrowerPath = @"D:/coding/Library/Library/DOC/BORROWER/BorrowerInfo.csv";
            string borrowHistoryPath = @"D:/coding/Library/Library/DOC/TRANSACTION_HISTORY/BorrowHistory.xls";
            string returnHistoryPath = @"D:/coding/Library/Library/DOC/TRANSACTION_HISTORY/ReturnHistory.xls";

            // Prepare data
            var books = Book.ReadBooksFromCSV(storePath);
            var borrowers = Borrower.ReadBorrowersFromCSV(borrowerPath);
            var borrowLogs = HistoryLog.ReadBorrowLogFromCSV(borrowHistoryPath, borrowers, books);
            var returnLogs = HistoryLog.ReadReturnLogFromCSV(returnHistoryPath, borrowers, books);

            // The menu
            Console.WriteLine("Welcome to the Library Management System!");
            Console.WriteLine();
            bool exit = false;
            do
            {
                Console.Clear();
                Console.WriteLine("===== MENU =====");
                Console.WriteLine("1. Librarian");
                Console.WriteLine("2. Borrower");
                Console.WriteLine("0. Exit");
                Console.WriteLine("Enter your choice:");

                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
                            LibrarianMenu(books, borrowers, borrowLogs, returnLogs);
                            break;

                        case 2:
                            Console.Clear();
                            BorrowerMenu(books, borrowers, borrowLogs, returnLogs);
                            break;

                        case 0:
                            Console.Clear();
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            } while (!exit);
        }

        /// <summary>
        /// The librarian menu
        /// </summary>
        /// <param name="books"></param>
        static void LibrarianMenu(List<Book> books, List<Borrower> borrowers, List<HistoryLog> borrowLogs, List<HistoryLog> returnLogs) 
        {   
            bool exit = false;
            while (!exit) 
            {
                // The first librarian menu box
                Console.Clear();
                Librarian librarian = new Librarian();
                Console.WriteLine("===== LIBRARIAN MENU =====");
                Console.WriteLine("1. Data interaction");
                Console.WriteLine("2. Data Manipulation");
                Console.WriteLine("0. Return to main menu");
                Console.WriteLine("Enter your choice:");
                int choice;
                if (int.TryParse(Console.ReadLine(), out choice)) 
                {
                    switch (choice) 
                    {
                        // Data interaction
                        case 1:
                            // The second librarian menu box
                            Console.Clear();
                            Console.WriteLine("===== DATA INTERACTION =====");
                            Console.WriteLine("1. Display the library");
                            Console.WriteLine("2. Display borrowers");
                            Console.WriteLine("3. Display transaction history");
                            Console.WriteLine("0. Return to librarian menu");
                            Console.WriteLine("Enter your choice:");
                            if (int.TryParse(Console.ReadLine(), out choice)) 
                            {   
                                switch (choice) 
                                {
                                    // Display the library
                                    case 1:
                                        Console.Clear();
                                        librarian.DisplayLibrary(books);
                                        Console.WriteLine("Press 1 to start sort the library. Press any key to return to librarian menu.");
                                        Console.WriteLine("Enter your choice: ");
                                        char userInput = Console.ReadKey().KeyChar;
                                        if (userInput == '1') 
                                        {
                                            bool exitSort = false;
                                            do
                                            {
                                                Console.Clear();
                                                Console.WriteLine("===== SORT OPTION =====");
                                                Console.WriteLine("1. Sort by ID");
                                                Console.WriteLine("2. Sort by Title");
                                                Console.WriteLine("3. Sort by Author");
                                                Console.WriteLine("4. Sort by Genre");
                                                Console.WriteLine("5. Sort by Quantity");
                                                Console.WriteLine("6. Sort by Availability");
                                                Console.WriteLine("7. Sort by BorrowStatus");
                                                Console.WriteLine("0. Return to librarian menu");
                                                Console.WriteLine("Enter your choice:");
                                                if (int.TryParse(Console.ReadLine(), out choice)) 
                                                {   
                                                    switch (choice)     
                                                    {
                                                        // Sort by ID
                                                        case 1:
                                                            Console.Clear();
                                                            books = librarian.SortBooks(books, "ID").ToList();
                                                            Console.WriteLine("\nSORTED BY ID");
                                                            librarian.DisplayLibrary(books);
                                                            Console.ReadKey();
                                                            break;
                                                        
                                                        // Sort by Title    
                                                        case 2:
                                                            Console.Clear();
                                                            books = librarian.SortBooks(books, "Title").ToList();
                                                            Console.WriteLine("\nSORTED BY TITLE");
                                                            librarian.DisplayLibrary(books);
                                                            Console.ReadKey();
                                                            break;

                                                        // Sort by Author
                                                        case 3:
                                                            Console.Clear();
                                                            books = librarian.SortBooks(books, "Author").ToList();
                                                            Console.WriteLine("\nSORTED BY AUTHOR");
                                                            librarian.DisplayLibrary(books);
                                                            Console.ReadKey();
                                                            break;

                                                        // Sort by Genre
                                                        case 4:
                                                            Console.Clear();
                                                            books = librarian.SortBooks(books, "Genre").ToList();
                                                            Console.WriteLine("\nSORTED BY GENRE");
                                                            librarian.DisplayLibrary(books);
                                                            Console.ReadKey();
                                                            break;

                                                        // Sort by Quantity
                                                        case 5:
                                                            Console.Clear();
                                                            books = librarian.SortBooks(books, "Quantity").ToList();
                                                            Console.WriteLine("\nSORTED BY QUANTITY");
                                                            librarian.DisplayLibrary(books);
                                                            Console.ReadKey();
                                                            break;

                                                        // Sort by Availability
                                                        case 6:
                                                            Console.Clear();
                                                            books = librarian.SortBooks(books, "Availability").ToList();
                                                            Console.WriteLine("\nSORTED BY AVAILABILITY");
                                                            librarian.DisplayLibrary(books);
                                                            Console.ReadKey();
                                                            break;

                                                        // Sort by BorrowStatus
                                                        case 7:
                                                            Console.Clear();
                                                            books = librarian.SortBooks(books, "BorrowStatus").ToList();
                                                            Console.WriteLine("\nSORTED BY BORROWSTATUS");
                                                            librarian.DisplayLibrary(books);
                                                            Console.ReadKey();
                                                            break;

                                                        case 0:
                                                            Console.Clear();
                                                            exitSort = true;
                                                            break;

                                                        default:
                                                            Console.WriteLine("Invalid choice. Please enter a valid number.");
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Invalid input. Please enter a valid number.");
                                                }
                                            } while (!exitSort);
                                        }
                                        break;

                                    // Display borrowers
                                    case 2:
                                        Console.Clear();
                                        Console.WriteLine(" ");
                                        Console.WriteLine(new string('-', 50));
                                        Console.WriteLine("{0,-25} {1,-25}",
                                                        "Name", "Contact Information");
                                        Console.WriteLine(new string('-', 50));
                                        foreach (var borrower in borrowers)
                                        {
                                            borrower.DisplayBorrowerInfo();
                                        }
                                        Console.ReadKey();
                                        break;

                                    // Display transaction history
                                    case 3:
                                    Console.Clear();
                                    HistoryLog historyLog = new HistoryLog();
                                    Console.WriteLine("===== DISPLAY TRANSACTION HISTORY =====");
                                    Console.WriteLine("1. Display borrowed book history");
                                    Console.WriteLine("2. Display return book history");
                                    Console.WriteLine("0. Return to librarian menu");
                                    Console.WriteLine("Enter your choice:");

                                    if (int.TryParse(Console.ReadLine(), out choice))
                                    {
                                        switch (choice)
                                        {
                                            case 1:
                                                Console.Clear();
                                                Console.WriteLine("===== BORROWED BOOK HISTORY =====");
                                                historyLog.DisplayBorrowedBooks(borrowLogs);
                                                Console.ReadKey();
                                                break;

                                            case 2:
                                                Console.Clear();
                                                Console.WriteLine("===== RETURN BOOK HISTORY =====");
                                                historyLog.DisplayReturnedBooks(returnLogs);
                                                Console.ReadKey();
                                                break;

                                            case 0:
                                                Console.Clear();
                                                break;

                                            default:
                                                Console.WriteLine("Invalid choice. Please enter a valid number.");
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid input. Please enter a valid number.");
                                    }
                                    break;
                                    
                                    // Return to librarian menu
                                    case 0:
                                        Console.Clear();
                                        break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid number.");
                            }
                            break;

                        // Data manipulation
                        case 2:
                            // The third librarian menu box
                            Console.Clear();
                            
                            Console.WriteLine("===== DATA MANIPULATION =====");
                            Console.WriteLine("1. Add a book");
                            Console.WriteLine("2. Delete a book");
                            Console.WriteLine("3. Edit a book");
                            Console.WriteLine("4. Add a borrower");
                            Console.WriteLine("5. Delete a borrower");
                            Console.WriteLine("6. Edit a borrower");
                            Console.WriteLine("0. Return to librarian menu");
                            Console.WriteLine("Enter your choice:");
                            if (int.TryParse(Console.ReadLine(), out choice)) 
                            {   
                                switch (choice) 
                                {
                                    // Add a book
                                    case 1:
                                        Console.Clear();
                                        librarian.AddBook(books);
                                        break;
                                    
                                    // Delete a book
                                    case 2:
                                        Console.Clear();
                                        librarian.DeleteBook(books);
                                        break;

                                    // Edit a book
                                    case 3:
                                        Console.Clear();
                                        librarian.EditBook(books);
                                        break;

                                    // Add a borrower            
                                    case 4:
                                        Console.Clear();
                                        librarian.AddBorrower(borrowers);
                                        break;

                                    // Delete a borrower            
                                    case 5:
                                        Console.Clear();
                                        librarian.DeleteBorrower(borrowers);
                                        break;

                                    // Edit a borrower  
                                    case 6:
                                        Console.Clear();
                                        librarian.EditBorrower(borrowers);
                                        break;  

                                    // Return to librarian menu
                                    case 0:
                                        Console.Clear();
                                        break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid number.");
                            }
                            break;

                        case 0:
                            Console.Clear();
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please enter valid number.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }
        }

        /// <summary>
        /// The borrower menu
        /// </summary>
        /// <param name="books"></param>
        static void BorrowerMenu(List<Book> books, List<Borrower> borrowers, List<HistoryLog> borrowLogs, List<HistoryLog> returnLogs) 
        {
            // Relative path to the CSV files
            string borrowHistoryPath = @"D:/coding/Library/Library/DOC/TRANSACTION_HISTORY/BorrowHistory.xls";
            string returnHistoryPath = @"D:/coding/Library/Library/DOC/TRANSACTION_HISTORY/ReturnHistory.xls";
            bool exit = false;
            while (!exit) 
            {
                // The first borrower menu box
                Console.Clear();
                Borrower borrower = new Borrower();
                Console.WriteLine("===== BORROWER MENU =====");
                Console.WriteLine("1. Display the library");
                Console.WriteLine("2. Borrow book");
                Console.WriteLine("3. Return book");
                Console.WriteLine("0. Return to main menu");
                Console.WriteLine("Enter your choice:");
                int choice;
                if (int.TryParse(Console.ReadLine(), out choice)) 
                {
                    switch (choice) 
                    {
                        // Display the library
                        case 1:
                            Console.Clear();
                            borrower.DisplayLibrary(books);
                            Console.ReadKey();
                            break;
                        
                        // Borrow book
                        case 2:
                            Console.Clear();
                            // Get borrower's information
                            Console.WriteLine("Enter your name: ");
                            string borrowerName = Console.ReadLine()!;
                            bool borrowerExists = borrowers.Any(borrower => borrower.Name == borrowerName);

                            if (borrowerExists)
                            {   
                                bool borrowerExits = false;
                                while (!borrowerExits)
                                {
                                    Console.Clear();
                                    borrower.DisplayLibrary(books);
                                    Console.WriteLine(" ");
                                    Console.WriteLine("Enter the book's title: ");
                                    string bookTitle = Console.ReadLine()!;

                                    var borrowerToBorrow = borrowers.FirstOrDefault(borrower => borrower.Name == borrowerName);
                                    var bookToBorrow = books.FirstOrDefault(book => book.Title == bookTitle);

                                    if (bookToBorrow != null)
                                    {
                                            if (bookToBorrow.Availability)
                                            {
                                                borrowerToBorrow.BorrowBook(borrowerToBorrow, bookToBorrow);
                                                Console.WriteLine("Borrowed successfully!");
                                                HistoryLog historyLog = new HistoryLog(borrowerToBorrow, bookToBorrow, DateTime.Now, DateTime.Now.AddDays(1));
                                                historyLog.ExportBorrowLogToCSV(borrowHistoryPath);
                                                Console.WriteLine(" ");
                                                Console.WriteLine("Press 1 to borrow another book. Press any key to return to borrower menu.");
                                                char userInput = Console.ReadKey().KeyChar;
                                                if (userInput != '1')
                                                {
                                                    borrowerExits = true;
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("This book is not available.");
                                            }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Book not found. Please ask librarian to add the book first.");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Borrower not found. Please ask librarian to register first.");
                            }     
                            Console.ReadKey();
                            break;

                        // Return book
                        case 3:
                            Console.Clear();
                            Console.WriteLine("Enter the name of the borrower who wants to return a book:");
                            borrowerName = Console.ReadLine();

                            // Find the borrower in the list by name
                            Borrower selectedBorrower = borrowers.FirstOrDefault(borrower => borrower.Name.Equals(borrowerName, StringComparison.OrdinalIgnoreCase));

                            if (selectedBorrower != null)
                            {
                                // Check if the borrower has any borrowing history
                                if (selectedBorrower.HistoryLog != null)
                                {
                                    List<HistoryLog> borrowedBooks = selectedBorrower.HistoryLog.ToList();

                                    if (borrowedBooks.Any())
                                    {
                                        Console.WriteLine("List of books borrowed by the user:");
                                        foreach (var record in borrowedBooks)
                                        {
                                            Console.WriteLine($"Book ID: {record.Book.ID}, Title: {record.Book.Title}");
                                        }

                                        Console.Write("Enter the ID of the book to return: ");
                                        if (int.TryParse(Console.ReadLine(), out int bookIdToReturn))
                                        {
                                            var bookToReturn = borrowedBooks.FirstOrDefault(log => log.Book.ID.ToString() == bookIdToReturn.ToString())?.Book;

                                            if (bookToReturn != null)
                                            {
                                                selectedBorrower.ReturnBook(selectedBorrower, bookToReturn);
                                                Console.WriteLine("Book returned successfully!");
                                                HistoryLog historyLog = new HistoryLog(selectedBorrower, bookToReturn, DateTime.Now, DateTime.Now.AddDays(1));
                                                historyLog.ExportReturnLogToCSV(returnHistoryPath);
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid book ID or book not found in the borrowed list of the selected borrower.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid input. Please enter a valid book ID.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("The selected borrower hasn't borrowed any books.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No borrowing history available for the selected borrower.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Borrower not found. Please enter a valid borrower's name.");
                            }
                            Console.ReadKey();
                            break;

                        case 0:
                            Console.Clear();
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a valid number.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }
        }
    }
} 