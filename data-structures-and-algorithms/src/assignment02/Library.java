package assignment02;

import java.io.File;
import java.io.FileNotFoundException;
import java.text.ParseException;
import java.util.ArrayList;
import java.util.GregorianCalendar;
import java.util.Scanner;

/**
 * Representation of a library.
 * A library is defined as a set of Library Books.
 * 
 * @author Maxwell
 * @version 1.1.0
 */
public class Library {
	/**
	 * Representation of the library
	 */
	private ArrayList<LibraryBook> library;

	/**
	 * Construct an emtpy library.
	 */
	public Library() {
		library = new ArrayList<LibraryBook>();
	}

	/**
	 * Add the specified book to the library.
	 * 
	 * This method assumes the book to add is not already in the library.
	 * 
	 * @param isbn -- ISBN of the book to be added
	 * @param author -- author of the book to be added
	 * @param title -- title of the book to be added
	 */
	public void add(long isbn, String author, String title) {
		library.add(new LibraryBook(isbn, author, title));
	}

	/**
	 * Add a list of library books to the library.
	 * 
	 * @param list -- list of library books to be added
	 */
	public void addAll(ArrayList<LibraryBook> list) {
		library.addAll(list);
	}

	/**
	 * Add books from an input file to the library.
	 * 
	 * Each line of the file is to be formatted as follows:
	 * "<ISBN>\t<author>\t<title>\n"
	 * If file does not exist or format is violated, then the
	 * library is not modified.
	 * 
	 * @param filename -- the name of the file to add books from
	 */
	public void addAll(String filename) {
		ArrayList<LibraryBook> buffer = new ArrayList<LibraryBook>();

		try (Scanner inFile = new Scanner(new File(filename))) {
			int lineNum = 1;
			
			while (inFile.hasNextLine()) {
				String line = inFile.nextLine();

				try (Scanner lineIn = new Scanner(line)) {
					lineIn.useDelimiter("\\t");

					if (!lineIn.hasNextLong()) {
						throw new ParseException("ISBN", lineNum);
					}
					long isbn = lineIn.nextLong();

					if (!lineIn.hasNext()) {
						throw new ParseException("Author", lineNum);
					}
					String author = lineIn.next();

					if (!lineIn.hasNext()) {
						throw new ParseException("Title", lineNum);
					}
					String title = lineIn.next();
					buffer.add(new LibraryBook(isbn, author, title));
				}
				lineNum++;
			}
		} catch (FileNotFoundException e) {
			System.err.println(e.getMessage() + " Nothing added to the library.");
			return;
		} catch (ParseException e) {
			System.err.println(e.getLocalizedMessage() + " formatted incorrectly at line " + e.getErrorOffset()
					+ ". Nothing added to the library.");
			return;
		}

		library.addAll(buffer);
	}

	/**
	 * Return the holder of a library book of an ISBN.
	 * 
	 * If there is no book of the ISBN in the library, then returns null.
	 * 
	 * @param isbn -- ISBN of the book, null if ISBN not found
	 */
	public String lookup(long isbn) {
		for (LibraryBook book: this.library) {
			if (book.getIsbn() == isbn) {
				return book.getHolder();
			}
		}
		
		return null;
	}

	/**
	 * Return a list of books check out under a specified holder.
	 * 
	 * If the specified holder has no books checked out, then an empty list is returned.
	 * 
	 * @param holder -- holder whose checked out books are returned
	 */
	public ArrayList<LibraryBook> lookup(String holder) {
		ArrayList<LibraryBook> holderBooks = new ArrayList<LibraryBook>();
		
		for (LibraryBook book: this.library) {
			if (book.getHolder() != null) {
				if (book.getHolder().equals(holder)) {
					holderBooks.add(book);
				}
			}
		}

		return holderBooks;
	}

	/**
	 * Checkout a library book of an ISBN under a holder with a due date.
	 * 
	 * If no book with the specified ISBN is in the library, then returns false.
	 * If the book with the specified ISBN is already checked out, then returns false.
	 * Otherwise, returns true.
	 * 
	 * @param isbn -- ISBN of the library book to be checked out
	 * @param holder -- new holder of the library book
	 * @param month -- month of the new due date of the library book
	 * @param day -- day of the new due date of the library book
	 * @param year -- year of the new due date of the library book
	 * @return -- true if the book was checked out, false otherwise
	 * 
	 */
	public boolean checkout(long isbn, String holder, int month, int day, int year) {
		for (LibraryBook book: this.library) {
			if (book.getIsbn() == isbn) {
				// book is in library and not checked out
				if (book.getHolder() == null && book.getDueDate() == null) {
					book.checkOut(holder, new GregorianCalendar(year, month, day));
					return true;
				}
				// book is in library and checked out
				else {
					return false;
				}
			}
		}
		
		// book is not in library
		return false;
	}

	/**
	 * Checkin a library book of an ISBN.
	 * 
	 * If no book with the specified ISBN is in the library, then returns false.
	 * If the book with the specified ISBN is already checked in, then returns false.
	 * Otherwise, returns true.
	 * 
	 * @param isbn -- ISBN of the library book to be checked in
	 * @return -- true if the library book was checked out, false otherwise
	 */
	public boolean checkin(long isbn) {
		for (LibraryBook book: library) {
			// book in library 
			if (book.getIsbn() == isbn) {
				// book not checked out
				if (book.getHolder() == null && book.getDueDate() == null) {
					return false;
				}
				// book checked out
				else {
					book.checkIn();
					return true;
				}
			}
		}
		
		// book not in library
		return false;
	}

	/**
	 * Checkin all books under a holder.
	 * 
	 * If no books with the specified holder are in the library, then returns false.
	 * Otherwise, returns true.
	 * 
	 * @param holder -- holder of the library books to be checked in
	 */
	public boolean checkin(String holder) {
		// compile list of books checked out by the holder
		ArrayList<LibraryBook> holderBooks = new ArrayList<LibraryBook>();
		for (LibraryBook book: library) {
			if (book.getHolder() != null) {
				if (book.getHolder().equals(holder)) {
					holderBooks.add(book);
				}
			}
		}
		
		// if holder has no books out
		if (holderBooks.isEmpty()) {
			return false;
		}
		
		// holder has books-return them
		for (LibraryBook book: holderBooks) {
			book.checkIn();
		}
		
		return true;
	}
}
