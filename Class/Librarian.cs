using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace Library
{
    public class Librarian : User
    {
        // Read all books from CSV file
        // Data include: ID, Title, Author, Genre, ISBN, Quantity, Availability, and BorrowStatus
        // Input data must be in the correct format
        public void AddBook(List<Book> books)
        {   
            int newID;
            Console.WriteLine("Enter the book's title: ");
            string newTitle = Console.ReadLine()!;

            // Check if the book with the same title already exists, then we just need to add the quantity
            if (books.Any(book => book.Title == newTitle)) {
                // Find the book with the same title
                var existingBook = books.FirstOrDefault(book => book.Title == newTitle);
                
                // Get the quantity from the user
                // If the user enters a negative number, keep asking until they enter a positive number
                Console.WriteLine("The book with the same title already exists. Please enter the quantity to add: ");
                int newQuantity;
                bool validInput = false;
                while (!validInput)
                {
                    if (int.TryParse(Console.ReadLine(), out newQuantity) && newQuantity > 0)
                    {
                        existingBook.Quantity += newQuantity;
                        validInput = true;
                    }
                    else
                        Console.WriteLine("Please enter a valid positive number for the quantity.");
                }
                
                return;
            }

            // If not then we need to add the book to the list
            else {

                // If there is no book in the list, set the ID to 1
                if (!books.Any()) {
                    newID = 1;
                } else {
                    // Get the last ID and add 1 to it
                    int lastID = books.Max(book => int.Parse(book.ID));
                    newID = lastID + 1;
                }

                // Get the author from the user
                Console.WriteLine("Enter the book's author: ");
                string newAuthor = Console.ReadLine()!;

                // Get the genre from the user
                Genres newGenre;
                do
                {
                    Console.WriteLine("Enter the book's genre: ");
                    string inputGenre = Console.ReadLine()!;
                    
                    if (Enum.GetNames(typeof(Genres)).Contains(inputGenre))
                    {
                        newGenre = (Genres)Enum.Parse(typeof(Genres), inputGenre);
                        break; // If the genre is valid, break the loop
                    }
                    else
                        Console.WriteLine("Invalid genre. Please choose from the available genres.");
                } while (true);

                // Get the ISBN from the user
                string newISBN;
                do
                {
                    Console.WriteLine("Enter the book's ISBN: ");
                    newISBN = Console.ReadLine()!;
                    
                    if (newISBN.Length == 14)
                    {
                        if (newISBN.All(char.IsDigit) && newISBN[3] == '-') 
                            break;
                        else
                            Console.WriteLine("Invalid ISBN format. Please enter a valid 13-digit ISBN in the format '###-############'.");
                    }
                    else
                        Console.WriteLine("Invalid ISBN length. ISBN should be 14 characters long.");
                } while (true);

                // Get the quantity from the user
                int newQuantity;
                do
                {
                    Console.WriteLine("Enter the book's quantity: ");
                    string inputQuantity = Console.ReadLine()!;

                    if (int.TryParse(inputQuantity, out newQuantity) && newQuantity > 0)
                        break;
                    else
                        Console.WriteLine("Invalid quantity. Please enter a positive integer value.");
                } while (true);

                // Add the new book to the list
                books.Add(new Book(newID.ToString(), newTitle, newAuthor, newGenre, newISBN, newQuantity, true, false));
                return;
            }
        }

        // Still thinking if it needs to be implemented
        public void DeleteBook(List<Book> books) {
            
        }

        public void EditBook(List<Book> books)
        {
            Console.WriteLine("Enter the title of the book you want to edit: ");
            string editTitle = Console.ReadLine()!;

            // Find the book with the given title
            var bookToEdit = books.FirstOrDefault(book => book.Title == editTitle);

            if (bookToEdit != null)
            {
                Console.WriteLine("Book found. Enter new information (leave blank to keep current value): ");

                Console.WriteLine("Enter the new title: ");
                string newTitle = Console.ReadLine();
                if (!string.IsNullOrEmpty(newTitle))
                    bookToEdit.Title = newTitle;

                Console.WriteLine("Enter the new author: ");
                string newAuthor = Console.ReadLine();
                if (!string.IsNullOrEmpty(newAuthor))
                    bookToEdit.Author = newAuthor;

                Console.WriteLine("Enter the new genre: ");
                string newGenreInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(newGenreInput))
                {
                    if (Enum.TryParse(newGenreInput, out Genres newGenre))
                        bookToEdit.Genre = newGenre;
                    else
                        Console.WriteLine("Invalid genre. Genre remains unchanged.");
                }

                Console.WriteLine("Enter the new ISBN: ");
                string newISBN = Console.ReadLine();
                if (!string.IsNullOrEmpty(newISBN) && newISBN.Length == 14 && newISBN.All(char.IsDigit) && newISBN[3] == '-')
                    bookToEdit.ISBN = newISBN;
                else 
                    Console.WriteLine("Invalid ISBN. ISBN remains unchanged. ISBN should be 14 characters long and in the format '###-############'.");

                Console.WriteLine("Enter the new quantity: ");
                string newQuantityInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(newQuantityInput) && int.TryParse(newQuantityInput, out int newQuantity) && newQuantity >= 0)
                    bookToEdit.Quantity = newQuantity;

                Console.WriteLine("Book information updated.");
            }
            else
                Console.WriteLine("Book not found.");
            
        }

        public void AddBorrower(List<Borrower> borrowers)
        {
            Console.WriteLine("Enter the borrower's name: ");
            string borrowerName = Console.ReadLine()!;

            // Check if the borrower with the same name already exists
            if (borrowers.Any(borrower => borrower.Name == borrowerName))
            {
                Console.WriteLine("The borrower with the same name already exists.");
                return;
            }

            Console.WriteLine("Enter the borrower's email (Gmail): ");
            string borrowerEmail = Console.ReadLine()!;

            // Validate the email format
            if (!IsValidEmail(borrowerEmail))
            {
                Console.WriteLine("Invalid email format. Please enter a valid Gmail address.");
                return;
            }

            // Add the new borrower to the list
            borrowers.Add(new Borrower(borrowerName, borrowerEmail, new List<Book>()));
            Console.WriteLine("Borrower added successfully!");
        }

        // Helper method to validate email format
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email && email.EndsWith("@gmail.com");
            }
            catch
            {
                return false;
            }
        }

        public void DeleteBorrower(List<Borrower> borrowers)
        {
            Console.WriteLine("Enter the name of the borrower to delete: ");
            string borrowerName = Console.ReadLine()!;

            // Find the borrower with the given name
            var borrowerToDelete = borrowers.FirstOrDefault(borrower => borrower.Name.Equals(borrowerName, StringComparison.OrdinalIgnoreCase));

            if (borrowerToDelete != null)
            {
                borrowers.Remove(borrowerToDelete);
                Console.WriteLine("Borrower deleted successfully!");
            }
            else
                Console.WriteLine("Borrower not found.");
            
        }

        public void EditBorrower(List<Borrower> borrowers)
        {
            Console.WriteLine("Enter the name of the borrower you want to edit: ");
            string borrowerName = Console.ReadLine()!;

            // Find the borrower with the given name
            var borrowerToEdit = borrowers.FirstOrDefault(borrower => borrower.Name.Equals(borrowerName, StringComparison.OrdinalIgnoreCase));

            if (borrowerToEdit != null)
            {
                Console.WriteLine("Borrower found. Enter new information (leave blank to keep current value): ");

                Console.WriteLine("Enter the new name: ");
                string newName = Console.ReadLine();
                if (!string.IsNullOrEmpty(newName))
                    borrowerToEdit.Name = newName;

                Console.WriteLine("Enter the new contact information: ");
                string newContactInfo = Console.ReadLine();
                if (!string.IsNullOrEmpty(newContactInfo))
                    borrowerToEdit.ContactInformation = newContactInfo;

                Console.WriteLine("Borrower information updated.");
            }
            else
                Console.WriteLine("Borrower not found.");
        }
    }
}