package assignment02;

import java.io.File;
import java.io.FileNotFoundException;
import java.text.ParseException;
import java.util.ArrayList;
import java.util.GregorianCalendar;
import java.util.Scanner;
import java.util.Comparator;

/**
 * Generic of the representation of a library.
 * 
 * A library is considered a set of books.
 *
 * @param T -- the type of holder of the books in the library
 */
public class LibraryGeneric<T> {
	/**
	 * Representation of the library.
	 */
	private ArrayList<LibraryBookGeneric<T>> library;

	/**
	 * Construct an empty library.
	 */
	public LibraryGeneric() {
		library = new ArrayList<LibraryBookGeneric<T>>();
	}

	/**
	 * Retrieve the inventory of the library sorted by ISBN.
	 * 
	 * @return -- an arraylist of generic library books in the library sorted by ISBN
	 */
	public ArrayList<LibraryBookGeneric<T>> getInventoryList() {
		// copy books into new arraylist
		ArrayList<LibraryBookGeneric<T>> libraryCopy = new ArrayList<LibraryBookGeneric<T>>();
		libraryCopy.addAll(library);

		// sort
		OrderByIsbn comparator = new OrderByIsbn();
		sort(libraryCopy, comparator);

		return libraryCopy;
	}

	/**
	 * Retrieve the inventory of the library sorted by author.
	 * 
	 * @return -- an arraylist of generic library books in the library sorted by author
	 */
	public ArrayList<LibraryBookGeneric<T>> getOrderedByAuthor() {
		// copy books into new arraylist
		ArrayList<LibraryBookGeneric<T>> libraryCopy = new ArrayList<LibraryBookGeneric<T>>();
		libraryCopy.addAll(library);

		// sort
		OrderByAuthor comparator = new OrderByAuthor();
		sort(libraryCopy, comparator);

		return libraryCopy;
	}

	/**
	 * Retrieve all overdue books in the library sorted by due date.
	 *
	 * @param month -- the month of the date to calculate overdue books
	 * @param day -- the day of the date to calculate overdue books
	 * @param year -- the year of the date to calculate overdue books
	 * @return -- an arraylist of generic library books whose due date is past the specified parameters
	 */
	public ArrayList<LibraryBookGeneric<T>> getOverdueList(int month, int day, int year) {
		// copy library
		ArrayList<LibraryBookGeneric<T>> libraryCopy = new ArrayList<LibraryBookGeneric<T>>();
		libraryCopy.addAll(library);

		// comb out non-overdue books
		GregorianCalendar today = new GregorianCalendar();
		for (LibraryBookGeneric<T> book: this.library) {
			if (book.getDueDate() != null) {
				if (book.getDueDate().compareTo(today) > 0) {
					libraryCopy.remove(book);
				}
			}
			// remove books that are not checked out
			else {
				libraryCopy.remove(book);
			}
		}

		// sort by due date
		OrderByDueDate comparator = new OrderByDueDate();
		sort(libraryCopy, comparator);

		return libraryCopy;
	}

	/**
	 * Sort an arraylist with a comparator.
	 * 
	 * Sorting is done with a selection sort algorithm.
	 * 
	 * TODO: modify to get rid of j
	 * 
	 * @param <ListType> -- the type of object in the list
	 * @param list -- the arraylist to sort
	 * @param comp -- the comparator to use to sort the list
	 */
	private static <ListType> void sort(ArrayList<ListType> list, Comparator<ListType> comp) {
		for (int outerIndex = 0; outerIndex < list.size() - 1; outerIndex++) {
			int innerIndex, minIndex;
			for (innerIndex = outerIndex + 1, minIndex = outerIndex; innerIndex < list.size(); innerIndex++)
				if (comp.compare(list.get(innerIndex), list.get(minIndex)) < 0)
					minIndex = innerIndex;
			ListType temp = list.get(outerIndex);
			list.set(outerIndex, list.get(minIndex));
			list.set(minIndex, temp);
		}
	}

	/**
	 * Comparator that defines an ordering among library books using the ISBN.
	 * 
	 * TODO: use lambda in place of protected class
	 */
	protected class OrderByIsbn implements Comparator<LibraryBookGeneric<T>> {

		/**
		 * Returns a negative value if lhs is smaller than rhs. Returns a positive
		 * value if lhs is larger than rhs. Returns 0 if lhs and rhs are equal.
		 */
		public int compare(LibraryBookGeneric<T> lhs,
				LibraryBookGeneric<T> rhs) {
			return (int) (lhs.getIsbn() - rhs.getIsbn());
		}
	}

	/**
	 * Comparator that defines an ordering among library books using the author, and book title as a tie-breaker.
	 */
	protected class OrderByAuthor implements Comparator<LibraryBookGeneric<T>> {
		/**
		 * Returns a negative value if lhs is smaller than rhs. Returns a positive
		 * value if lhs is larger than rhs. Returns 0 if lhs and rhs are equal.
		 */
		public int compare(LibraryBookGeneric<T> lhs, LibraryBookGeneric<T> rhs) {
			// lexicographically compare authors
			int authorComparison = lhs.getAuthor().compareTo(rhs.getAuthor());
			if (authorComparison != 0) {
				return authorComparison;
			}
			// authors are same at this point, compare by title
			return lhs.getTitle().compareTo(rhs.getTitle());
		}
	}

	/**
	 * Comparator that defines an ordering among library books using the due date.
	 * 
	 * TODO: replace protected class with lambda
	 */
	protected class OrderByDueDate implements Comparator<LibraryBookGeneric<T>> {
		/**
		 * Returns a negative value if lhs is smaller than rhs. Returns a positive
		 * value if lhs is larger than rhs. Returns 0 if lhs and rhs are equal.
		 */
		public int compare(LibraryBookGeneric<T> lhs, LibraryBookGeneric<T> rhs) {
			return lhs.getDueDate().compareTo(rhs.getDueDate());
		}
	}


	/**
	 * Add a new book to the library.
	 * 
	 * The specified book is assumed to not already be in the library.
	 * 
	 * @param isbn -- ISBN of the book to be added
	 * @param author -- author of the book to be added
	 * @param title -- title of the book to be added
	 */
	public void add(long isbn, String author, String title) {
		library.add(new LibraryBookGeneric<T>(isbn, author, title));
	}

	/**
	 * Add a list of new books to the library.
	 * 
	 * The specified books are assumed to not already be in the library.
	 * 
	 * @param list -- list of library books to be added
	 */
	public void addAll(ArrayList<LibraryBookGeneric<T>> list) {
		library.addAll(list);
	}

	/**
	 * Add books from an input file to the library.
	 * 
	 * The books specified in the file are assumed to not be in the library.
	 * Each line of the input file specifies on book.
	 * Each line is to be formatted as such:
	 * '<ISBN>\t<author>\t<title>\n'
	 * If the file does not exist or is malformed, nothing is modified.
	 * 
	 * @param filename -- the name of the file to add books from
	 */
	public void addAll(String filename) {
		ArrayList<LibraryBookGeneric<T>> buffer = new ArrayList<LibraryBookGeneric<T>>();

		try (Scanner inFile = new Scanner(new File(filename))) {
			int lineCursor = 1;

			while (inFile.hasNextLine()) {
				String line = inFile.nextLine();

				try (Scanner lineIn = new Scanner(line)) {
					lineIn.useDelimiter("\\t");

					if (!lineIn.hasNextLong()) {
						throw new ParseException("ISBN", lineCursor);
					}
					long isbn = lineIn.nextLong();

					if (!lineIn.hasNext()) {
						throw new ParseException("Author", lineCursor);
					}
					String author = lineIn.next();

					if (!lineIn.hasNext()) {
						throw new ParseException("Title", lineCursor);
					}
					String title = lineIn.next();
					buffer.add(new LibraryBookGeneric<T>(isbn, author, title));
				}
				lineCursor++;
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
	 * Retrieve the holder of the book of a specified ISBN.
	 * 
	 * If no book with the specified ISBN is in the library, then returns null.
	 * If the book is found but not checked out, then returns null.
	 * 
	 * @param isbn -- ISBN of the book to be looked up
	 * @return -- holder of the book of the ISBN, null if the book is not checked out
	 */
	public T lookup(long isbn) {
		for (LibraryBookGeneric<T> book: this.library) {
			if (book.getIsbn() == isbn) {
				return book.getHolder();
			}
		}
		
		return null;
	}

	/**
	 * Returns the list of library books checked out to the specified holder.
	 * 
	 * If the specified holder has no books checked out, returns an empty list.
	 * 
	 * @param holder -- holder to compile list of checked-out books from
	 * @return -- list of books checked out under the holder
	 */
	public ArrayList<LibraryBookGeneric<T>> lookup(T holder) {
		ArrayList<LibraryBookGeneric<T>> holderBooks = new ArrayList<LibraryBookGeneric<T>>();
		
		for (LibraryBookGeneric<T> book: this.library) {
			if (book.getHolder() != null) {
				if (book.getHolder().equals(holder)) {
					holderBooks.add(book);
				}
			}
		}

		return holderBooks;
	}

	/**
	 * Checkout a book of an isbn out under a holder with a due date.
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
	 * 
	 */
	public boolean checkout(long isbn, T holder, int month, int day, int year) {
		for (LibraryBookGeneric<T> book: library) {
			if (book.getIsbn() == isbn) {
				// if book is not checked out
				if (book.getHolder() == null && book.getDueDate() == null) {
					book.checkOut(holder, new GregorianCalendar(year, month, day));
					return true;
				}
				// if the book is checked out
				else {
					return false;
				}
			}
		}
		
		return false;
	}

	/**
	 * Checkin a book of an isbn.
	 * 
	 * Checking in is done by setting the holder and due date to null.
	 * If no book of the isbn is in the library, then returns false.
	 * If the book is found and already checked in, then returns false.
	 * Otherwise, returns true.
	 * 
	 * @param isbn -- ISBN of the library book to be checked in
	 * @return -- true if the book was checked in, false otherwise
	 */
	public boolean checkin(long isbn) {
		for (LibraryBookGeneric<T> book: this.library) {
			if (book.getIsbn() == isbn) {
				// if book is checked in
				if (book.getHolder() == null && book.getDueDate() == null) {
					return false;
				}
				// book is checked out
				else {
					book.checkIn();
					return true;
				}
			}
		}

		return false;
	}

	/**
	 * Checkin all books under a holder.
	 * 
	 * If no books with the specified holder are in the library, then returns false.
	 * Otherwise, returns true.
	 * 
	 * @param holder -- holder of the library books to be checked in
	 * @return -- true if any books were checked in, otherwise returns false
	 */
	public boolean checkin(T holder) {
		// compile list of books checked out by the holder
		ArrayList<LibraryBookGeneric<T>> holderBooks = new ArrayList<LibraryBookGeneric<T>>();
		for (LibraryBookGeneric<T> book: library) {
			if (book.getHolder() != null) {
				if (book.getHolder().equals(holder)) {
					holderBooks.add(book);
				}
			}
		}

		if (holderBooks.isEmpty()) {
			return false;
		}

		// checkin all books
		for (LibraryBookGeneric<T> book: holderBooks) {
			book.checkIn();
		}

		return true;
	}
}
