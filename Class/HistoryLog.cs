using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace Library 
{
    public class HistoryLog 
    {
        public Borrower Borrower { get; }
        public Book Book { get; }
        public DateTime BorrowDate { get; }
        public DateTime ReturnDate { get; }
        public HistoryLog(Borrower borrower, Book book, DateTime borrowDate, DateTime returnDate)
        {
            Borrower = borrower;
            Book = book;
            BorrowDate = borrowDate;
            ReturnDate = returnDate;
        }

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
    }
}