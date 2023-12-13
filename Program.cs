// Purpose: The main program of the Library Management System.
// Remember to uncomment the relative path to the CSV files depent on your IDE
// If you use another IDE, you need to change the relative path to the CSV files or update it with absolute path

// *****************************************
// * DEFAULT PASSWORD FOR LIBRARIAN: admin *
// *****************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Globalization;
using Library.Class.TFIDF;
using System.Formats.Tar;

namespace Library
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Relative path to the CSV files
            // When run the program it will load all data before the menu is show

            // For Visual Studio Code user only
            // string rootPath = @"D:/coding/Library/Library/";
            // string storePath = Path.Combine(rootPath, "DOC/STORE/Store.csv");
            // string borrowerPath = Path.Combine(rootPath, "DOC/BORROWER/BorrowerInfo.csv");
            // string borrowHistoryPath = Path.Combine(rootPath, "DOC/TRANSACTION_HISTORY/BorrowHistory.xls");
            // string returnHistoryPath = Path.Combine(rootPath, "DOC/TRANSACTION_HISTORY/ReturnHistory.xls");
            // string documentsPath = Path.Combine(rootPath, "DOC/DETAIL/");

            // For Visual Studio users only
            string storePath = "../../../DOC/STORE/Store.csv";
            string borrowerPath = "../../../DOC/BORROWER/BorrowerInfo.csv";
            string borrowHistoryPath = "../../../DOC/TRANSACTION_HISTORY/BorrowHistory.xls";
            string returnHistoryPath = "../../../DOC/TRANSACTION_HISTORY/ReturnHistory.xls";
            string documentsPath = "../../../DOC/DETAIL/";

            // When you run the program, you need to uncomment the code related to your IDE
            // And also uncomment the relative path to the CSV files in 'Display transaction history'

            // Prepare data
            var e = new Execution();
            var documents = e.ProcessDocuments(documentsPath);
            var tfidf = new TFIDF(documents);
            var documentVectors = tfidf.Transform();
            var books = Book.ReadBooksFromCSV(storePath);
            var borrowers = Borrower.ReadBorrowersFromCSV(borrowerPath);
            var borrowLogs = HistoryLog.ReadBorrowLogFromCSV(borrowHistoryPath, borrowers, books);
            var returnLogs = HistoryLog.ReadReturnLogFromCSV(returnHistoryPath, borrowers, books);

            // Update borrowed books for each borrower
            // Because when we turn off program, the borrowed books of each borrower will be reset to null
            // So we need to update it again, make sure we always keep the data
            foreach (var borrower in borrowers)
            {
                Borrower newBorrower = new Borrower();
                newBorrower.UpdateBorrowedBooksFromCSV(borrowHistoryPath, returnHistoryPath, books, borrower);
            }

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
                Console.WriteLine("3. Search");
                Console.WriteLine("0. Exit");
                Console.WriteLine("Enter your choice:");

                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("ENTER PASSWORD: ");
                            string password = Console.ReadLine();
                            if (password == "admin")
                            {
                                Console.Clear();
                                LibrarianMenu(books, borrowers, borrowLogs, returnLogs, borrowHistoryPath, returnHistoryPath);
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("LOGIN FAILED");
                                Console.ReadKey();
                                break;
                            }
                            break;

                        case 2:
                            Console.Clear();
                            BorrowerMenu(books, borrowers, borrowerPath, borrowHistoryPath, returnHistoryPath);
                            break;

                        case 3:
                            Console.Clear();
                            Console.WriteLine("===== SEARCH OPTION =====");
                            Console.WriteLine("1. Search by keyword");
                            Console.WriteLine("2. Search by content summary");
                            Console.WriteLine("0. Return to librarian menu");
                            Console.WriteLine("Enter your choice: ");
                            int searchChoice;
                            if (int.TryParse(Console.ReadLine(), out searchChoice))
                            {
                                switch (searchChoice)
                                {
                                    case 1:
                                        SearchBooks(books);
                                        break;

                                    case 2:
                                        SearchSummary(books, tfidf, documentVectors);
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

                        case 0:
                            // Exit the program
                            // And save all data to CSV files
                            Console.Clear();
                            User user = new User();
                            books = user.SortBooks(books, "ID").ToList();
                            Book.WriteBooksToCSV(books, storePath);
                            Borrower.WriteBorrowersToCSV(borrowers, borrowerPath);
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
        /// <param name="books">List of books</param>
        /// <param name="borrowers">List of borrowers</param>
        /// <param name="borrowLogs">List of borrow history logs</param>
        /// <param name="returnLogs">List of return history logs</param>
        static void LibrarianMenu(List<Book> books, List<Borrower> borrowers, List<HistoryLog> borrowLogs, List<HistoryLog> returnLogs, string borrowHistoryPath, string returnHistoryPath)
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
                                        Console.WriteLine("\nPress 1 to start sort the library.\nPress any key to return to librarian menu.");
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
                                        else
                                        {
                                            Console.Clear();
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

                                        borrowLogs = HistoryLog.ReadBorrowLogFromCSV(borrowHistoryPath, borrowers, books);
                                        returnLogs = HistoryLog.ReadReturnLogFromCSV(returnHistoryPath, borrowers, books);

                                        bool exitTransactionLogDisplay = false;
                                        do
                                        {
                                            Console.Clear();
                                            HistoryLog historyLog = new HistoryLog();
                                            Console.WriteLine("===== DISPLAY TRANSACTION HISTORY =====");
                                            Console.WriteLine("1. Display borrowed book history");
                                            Console.WriteLine("2. Display return book history");
                                            Console.WriteLine("3. Display transaction history by keyword");
                                            Console.WriteLine("4. Display late return");
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

                                                    case 3:
                                                        bool exitLogSearch = false;
                                                        do
                                                        {
                                                            Console.Clear();
                                                            Console.WriteLine("===== SEARCH TRANSACTION HISTORY BY KEYWORD =====");
                                                            Console.WriteLine("1. By borrower name");
                                                            Console.WriteLine("2. By book title");
                                                            Console.WriteLine("3. By date");
                                                            Console.WriteLine("0. Return to librarian menu");
                                                            Console.WriteLine("Enter your choice:");
                                                            if (int.TryParse(Console.ReadLine(), out choice))
                                                            {
                                                                switch (choice)
                                                                {
                                                                    case 1:
                                                                        Console.Clear();
                                                                        bool exitSearch = false;
                                                                        do
                                                                        {
                                                                            Console.WriteLine("Enter borrower name: ");
                                                                            string borrowerName = Console.ReadLine();
                                                                            historyLog.DisplayLogsByBorrower(borrowLogs, borrowerName);
                                                                            Console.WriteLine("\nPress 1 to search again. Press any key to return to search option.");
                                                                            userInput = Console.ReadKey().KeyChar;
                                                                            if (userInput != '1')
                                                                                exitSearch = true;
                                                                        } while (!exitSearch);
                                                                        break;

                                                                    case 2:
                                                                        Console.Clear();
                                                                        exitSearch = false;
                                                                        do
                                                                        {
                                                                            Console.WriteLine("Enter book title: ");
                                                                            string bookTitle = Console.ReadLine();
                                                                            historyLog.DisplayLogsByBookTitle(borrowLogs, bookTitle);
                                                                            Console.WriteLine("\nPress 1 to search again. Press any key to return to search option.");
                                                                            userInput = Console.ReadKey().KeyChar;
                                                                            if (userInput != '1')
                                                                                exitSearch = true;
                                                                        } while (!exitSearch);
                                                                        break;

                                                                    case 3:
                                                                        Console.Clear();
                                                                        exitSearch = false;
                                                                        do
                                                                        {
                                                                            Console.WriteLine("Enter date (dd/MM/yyyy): ");
                                                                            string dateString = Console.ReadLine();
                                                                            if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                                                                            {
                                                                                historyLog.DisplayLogsByDate(borrowLogs, date);
                                                                            }
                                                                            else
                                                                            {
                                                                                Console.WriteLine("Invalid date format. Please enter date in the format dd/MM/yyyy.");
                                                                            }
                                                                            Console.WriteLine("\nPress 1 to search again. Press any key to return to search option.");
                                                                            userInput = Console.ReadKey().KeyChar;
                                                                            if (userInput != '1')
                                                                                exitSearch = true;
                                                                        } while (!exitSearch);
                                                                        break;

                                                                    case 0:
                                                                        Console.Clear();
                                                                        exitLogSearch = true;
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
                                                        } while (!exitLogSearch);
                                                        break;

                                                    case 4:
                                                        Console.Clear();
                                                        Console.WriteLine("===== LATE RETURN =====");
                                                        historyLog.DisplayLateReturn(borrowLogs, returnLogs);
                                                        Console.ReadKey();
                                                        break;

                                                    case 0:
                                                        Console.Clear();
                                                        exitTransactionLogDisplay = true;
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
                                        } while (!exitTransactionLogDisplay);
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
                                        Console.WriteLine("Enter the book's title to delete: ");
                                        string bookTitle = Console.ReadLine();
                                        librarian.DeleteBook(books, bookTitle);
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
        /// The librarian menu
        /// </summary>
        /// <param name="books">List of books</param>
        /// <param name="borrowers">List of borrowers</param>
        static void BorrowerMenu(List<Book> books, List<Borrower> borrowers, string borrowerPath, string borrowHistoryPath, string returnHistoryPath)
        {
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
                Console.WriteLine("4. Change password");
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

                            Console.WriteLine("Enter your name: ");
                            string borrowerName = Console.ReadLine()!;
                            Console.WriteLine("Enter your password: ");
                            string borrowerPassword = Console.ReadLine()!;

                            // Check login credentials
                            var loggedInBorrower = borrowers.FirstOrDefault(borrower => borrower.Login(borrowerName, borrowerPassword));

                            if (loggedInBorrower == null)
                            {
                                Console.WriteLine("Login unsuccessfully. Your username or password is wrong ");
                                Console.ReadLine();
                                break;
                            }
                            else
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
                                            if (!borrowerToBorrow.HasBorrowedBook(bookToBorrow))
                                            {
                                                borrowerToBorrow.BorrowBook(borrowerToBorrow, bookToBorrow);
                                                HistoryLog historyLog = new HistoryLog(borrowerToBorrow, bookToBorrow, DateTime.Now, DateTime.Now.AddDays(1));
                                                historyLog.ExportBorrowLogToCSV(borrowHistoryPath);
                                            }
                                            else
                                            {
                                                Console.WriteLine(" ");
                                                Console.WriteLine("You have already borrowed this book. Please remember to return it on time.");
                                            }

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
                                            Console.ReadKey();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Book not found. Please ask librarian to add the book first.");
                                    }
                                }
                            }
                            Console.ReadKey();
                            break;

                        // Return book
                        case 3:
                            Console.Clear();
                            // Get borrower's information
                            Console.WriteLine("Enter your name: ");
                            string borrowerNameReturn = Console.ReadLine()!;
                            Console.WriteLine("Enter your password: ");
                            string borrowerPasswordReturn = Console.ReadLine()!;

                            // Check login credentials
                            var loggedInBorrowerReturn = borrowers.FirstOrDefault(borrower => borrower.Login(borrowerNameReturn, borrowerPasswordReturn));

                            if (loggedInBorrowerReturn == null)
                            {
                                Console.WriteLine("Login unsuccessfully. Your username or password is wrong ");
                                Console.ReadLine();
                                break;
                            }
                            else
                            {
                                if (loggedInBorrowerReturn.BorrowedBooks.Any())
                                {
                                    Console.WriteLine("List of books borrowed by the user:");
                                    Console.WriteLine(new string('-', 160));
                                    Console.WriteLine("{0,-5} {1,-90} {2,-30} {3,-18} {4,-15}",
                                        "ID", "Title", "Author", "Genre", "ISBN", "Quantity", "Availability", "BorrowStatus");
                                    Console.WriteLine(new string('-', 160));
                                    foreach (var book in loggedInBorrowerReturn.BorrowedBooks)
                                    {
                                        Console.WriteLine($"{book.ID,-5} {book.Title,-90} {book.Author,-30} {book.Genre,-18} {book.ISBN,-15}");
                                    }

                                    Console.Write("Enter the ID of the book to return: ");
                                    if (int.TryParse(Console.ReadLine(), out int bookIdToReturn))
                                    {
                                        var bookToReturn = loggedInBorrowerReturn.BorrowedBooks.FirstOrDefault(book => book.ID == bookIdToReturn.ToString());

                                        if (bookToReturn != null)
                                        {
                                            loggedInBorrowerReturn.ReturnBook(loggedInBorrowerReturn, bookToReturn);
                                            Console.WriteLine("Book returned successfully!");
                                            // Update history log or other necessary actions here
                                            HistoryLog historyLog = new HistoryLog(loggedInBorrowerReturn, bookToReturn, DateTime.Now);
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
                            Console.ReadKey();
                            break;

                        case 4:
                            Console.Clear();
                            Console.WriteLine("Enter your name: ");
                            string borrowerNameChangePassword = Console.ReadLine()!;
                            Console.WriteLine("Enter your password: ");
                            string borrowerPasswordChangePassword = Console.ReadLine()!;
                            var loggedInBorrowerChangePassword = borrowers.FirstOrDefault(borrower => borrower.Login(borrowerNameChangePassword, borrowerPasswordChangePassword));
                            if (loggedInBorrowerChangePassword != null)
                            {
                                Console.WriteLine(" ");
                                Console.WriteLine("Enter your new password: ");
                                string newPassword = Console.ReadLine()!;
                                loggedInBorrowerChangePassword.ChangePassword(borrowerNameChangePassword, borrowerPasswordChangePassword, newPassword);
                                Borrower.WriteBorrowersToCSV(borrowers, borrowerPath);
                            }
                            else
                            {
                                Console.WriteLine("Invalid name or password.");
                            }
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

        // Search books by keyword
        static void SearchBooks(List<Book> books)
        {
            Console.Clear();
            bool exitSearch = false;
            Librarian librarian = new Librarian();
            char userInput;
            do
            {
                Console.Clear();
                Console.WriteLine("===== SEARCH BY KEYWORD =====");
                Console.WriteLine("Enter your keyword : ");
                string query = Console.ReadLine();
                List<Book> foundBooks = librarian.SearchBooks(books, query);
                librarian.DisplayLibrary(foundBooks);
                Console.WriteLine("\nPress 1 to search again. Press any key to return to search option.");
                userInput = Console.ReadKey().KeyChar;
                if (userInput != '1')
                    exitSearch = true;
            } while (!exitSearch);
        }

        // Search books by content summary
        static void SearchSummary(List<Book> books, TFIDF tfidf, double[][] documentVectors)
        {
            var borrower = new Borrower();
            // Use LINQ to find the books with the same IDs
            // Convert the result to an array

            Console.Clear();
            bool exitSearch = false;
            char userInput;
            do
            {
                Console.Clear();
                Console.WriteLine("===== SEARCH BY CONTENT SUMMARY =====");
                Console.WriteLine("Enter content to search : ");
                string query = Console.ReadLine();
                List<Book> isbnBook = books.Where(x => x.ISBN.Equals(query)).ToList();
                if (isbnBook.Count() > 0)
                {
                    borrower.DisplayLibrary(isbnBook);
                }
                else
                {
                    var searchResults = TFIDF.Search(query, documentVectors, tfidf);
                    var selectedBooks = books.Where(book => searchResults.Contains(int.Parse(book.ID)));
                    List<Book> results = selectedBooks.ToList();
                    borrower.DisplayLibrary(results);
                }
                Console.WriteLine("\nPress 1 to search again. Press any key to return to search option.");
                userInput = Console.ReadKey().KeyChar;
                if (userInput != '1')
                    exitSearch = true;
            } while (!exitSearch);
        }
    }
}