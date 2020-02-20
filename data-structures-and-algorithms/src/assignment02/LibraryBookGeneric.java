package assignment02;

import java.util.GregorianCalendar;

/**
 * A generic library book.
 * 
 * @author Maxwell Hanson
 * @version 1.1.0
 * @param <T> -- the type of holder associated with this library book
 */
public class LibraryBookGeneric<T> extends Book {
	/**
	 * The holder of the book. Null iff the book is not checked out.
	 */
	private T holder;
	/**
	 * The date that the book is due. Null iff book is not checked out.
	 */
	private GregorianCalendar dueDate;
	
	/**
	 * Construct a generic library book with an ISBN, author, and title.
	 * 
	 * @param isbn -- the isbn of the book
	 * @param author -- the author of the book
	 * @param title -- the title of the book
	 */
	public LibraryBookGeneric(long isbn, String author, String title) {
		super(isbn, author, title);
	}
	
	/**
	 * Retrieve the holder of the book.
	 * 
	 * @return -- holder of the book, null if the book is not checked out
	 */
	public T getHolder() {
		return holder;
	}
	
	/**
	 * Retrieve the due date of the book.
	 * 
	 * @return -- due date of the book, null if the book is not checked out
	 */
	public GregorianCalendar getDueDate() {
		return dueDate;
	}
	
	/**
	 * Check this book in.
	 * 
	 * Holder and due date are set to null.
	 */
	public void checkIn() {
		holder = null;
		dueDate = null;
	}
	
	/**
	 * Check out this book under a holder and due date.
	 * 
	 * @param holder -- new holder of book
	 * @param dueDate -- new due date of the book
	 */
	public void checkOut(T holder, GregorianCalendar dueDate) {
		this.holder = holder;
		this.dueDate = dueDate;
	}

}
