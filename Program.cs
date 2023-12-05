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
            string defaultPath = "../../../DOC/STORE/Store.csv";
            var books = BookReader.ReadBooksFromCSV(defaultPath);

            foreach (var book in books)
            {
                Console.WriteLine($"{book.ID,-5} {book.Title,-90} {book.Author,-25} {book.Genre,-20} {book.ISBN,-15} {book.Quantity,-5} {book.Availability,-6} {book.BorrowStatus}");
            }
        }
    }
}