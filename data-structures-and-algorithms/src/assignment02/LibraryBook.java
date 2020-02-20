package assignment02;

import java.util.GregorianCalendar;

/**
 * Representation of a library book.
 * 
 * @author Maxwell Hanson
 * @version 1.1.0
 */
public class LibraryBook extends Book {
	private String holder;
	private GregorianCalendar dueDate;
	
	public LibraryBook(long isbn, String author, String title) {
		super(isbn, author, title);
	}
	
	public String getHolder() {
		return this.holder;
	}
	
	public GregorianCalendar getDueDate() {
		return this.dueDate;
	}
	
	/*
	 * methods for checking the book in and out
	 */
	public void checkIn() {
		this.holder = null;
		this.dueDate = null;
	}
	
	public void checkOut(String holder, GregorianCalendar dueDate) {
		this.holder = holder;
		this.dueDate = dueDate;
	}

}
