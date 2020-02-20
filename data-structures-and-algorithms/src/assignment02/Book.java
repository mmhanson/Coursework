package assignment02;

/**
 * Representation of a book.
 * Member variables (ISBN, author, title) are immutable after instansiation.
 * 
 * @author Maxwell Hanson
 * @version 1.1.0
 */
public class Book {
	/**
	 * The ISBN of the book.
	 */
	private long isbn;

	/**
	 * The author of the book.
	 */
	private String author;

	/**
	 * The title of the book.
	 */
	private String title;

	/**
	 * Construct a book with an ISBN, author, and title.
	 * 
	 * @param isbn -- the isbn of the book
	 * @param author -- the author of the book
	 * @param title -- the title of the book
	 */
	public Book(long isbn, String author, String title) {
		this.isbn = isbn;
		this.author = author;
		this.title = title;
	}

	/**
	 * Retrieve the author of the book.
	 * 
	 * @return -- the author of the book
	 */
	public String getAuthor() {
		return this.author;
	}

	/**
	 * Retrieve the ISBN of the book.
	 * 
	 * @return -- the ISBN of the book
	 */
	public long getIsbn() {
		return this.isbn;
	}

	/**
	 * Retrieve the title of the book.
	 * 
	 * @return -- the title of the book
	 */
	public String getTitle() {
		return this.title;
	}


	/**
	 * Represent this instance as a string.
	 * 
	 * Strings are formatted as follows:
	 * '<isbn>, <author>, "<title>"'
	 */
	public String toString() {
		return isbn + ", " + author + ", \"" + title + "\"";
	}

	/**
	 * Determine if this is equal to another object.
	 * 
	 * This instance is equal to another object iff the object is a book
	 * and the object has the same ISBN, author, and title as this.
	 * 
	 * @param other -- the object begin compared with this
	 * @return true if "other" is a Book and is equal to this, false otherwise
	 */
	public boolean equals(Object other) {
		// cast to book if applicable
		if(!(other instanceof Book)) {
			return false;
		}
		Book otherBook = (Book)other;
		
		// determine if the attributes are equal
		if (this.getIsbn() != otherBook.getIsbn() ||
				!this.getAuthor().equals(otherBook.getAuthor()) ||
				!this.getTitle().equals(otherBook.getTitle())) {
			return false;
		}
		return true;
	}
	
	@Override
	public int hashCode() {
		return (int) isbn + author.hashCode() + title.hashCode();
	}
}
