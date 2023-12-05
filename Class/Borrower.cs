using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library
{
    public class Borrower
    {
        public int BorrowerId { get; set; }
        public string Name { get; set; }
        public List<Book> BooksBorrowed { get; set; }

        public Borrower(int borrowerId, string name, List<Book> booksBorrowed)
        {
            BorrowerId = borrowerId;
            Name = name;
            BooksBorrowed = new List<Book>();
        }

        public bool IsBorrowed(Book book)
        {
            return BooksBorrowed.Any(borrowedBook => borrowedBook.ID == book.ID); // Assuming ID is a unique identifier for books
        }
    }
}