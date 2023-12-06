using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;


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

            // The menu
            Console.WriteLine("Welcome to the Library Management System!");
            bool exit = false;
            do
            {
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
                            LibrarianMenu();
                            break;

                        case 2:
                            Console.Clear();
                            BorrowerMenu();
                            break;

                        case 0:
                            Console.WriteLine("Exiting the program...");
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

                Console.WriteLine();
            } while (!exit);
        }

        static void LibrarianMenu() 
        {
            Console.WriteLine("===== LIBRARIAN MENU =====");
        }

        static void BorrowerMenu() 
        {
            Console.WriteLine("===== BORROWER MENU =====");
        }
    }
} 