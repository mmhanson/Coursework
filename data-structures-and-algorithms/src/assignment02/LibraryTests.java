package assignment02;

import static org.junit.Assert.assertTrue;
import static org.junit.Assert.fail;
import java.util.ArrayList;
import org.junit.Before;
import org.junit.Test;

/**
 * Testing suite for the "Library" class.
 */
public class LibraryTests {
	/**
	 * A library with no books
	 */
	private Library emptyLib;
	/**
	 * A library with less than a dozen books
	 */
	private Library smallLib;
	/**
	 * A library with between a dozen and a hundred books
	 */
	private Library mediumLib;
	/**
	 * An ISBN that is not in the empty library.
	 */
	private long emptyNonexistentIsbn;
	/**
	 * An ISBN that is not in the small library.
	 */
	private long smallNonexistentIsbn;
	/**
	 * An ISBN that is in the small library.
	 */
	private long smallExistentIsbn;
	/**
	 * Another ISBN known to be included in the small library.
	 */
	private long smallExistentIsbn2;
	/**
	 * An ISBN that is not in the medium library.
	 */
	private long mediumNonexistentIsbn;
	/**
	 * An ISBN that is in the medium library.
	 */
	private long mediumExistentIsbn;
	/**
	 * Another ISBN known to be included in the medium library.
	 */
	private long mediumExistentIsbn2;
	/**
	 * A list of one library book.
	 */
	private ArrayList<LibraryBook> oneBookList;
	/**
	 * The isbn of the book in the oneBookList
	 */
	private long oneBookListIsbn;
	/**
	 * A list of many books
	 */
	private ArrayList<LibraryBook> manyBookList;
	/**
	 * The list of isbns in the manyBookList
	 */
	private ArrayList<Long> manyBookListIsbns;
	/**
	 * The name of an empty file
	 */
	private String emptyBookListFname;
	/**
	 * The name of a file with one book
	 */
	private String oneBookListFname;
	/**
	 * The name of the file with many books.
	 */
	private String manyBookListFname;
	/**
	 * The name of a malformed book list file.
	 */
	private String malformedBookListFname;
	/**
	 * The name of a book list file that does not exist.
	 */
	private String nonExistentBookListFname;
	
	/**
	 * Set up testing libraries for use in the test suite.
	 */
	@Before
	public void setup() {
		// library metadata
		emptyNonexistentIsbn = 978037429279L;
		smallNonexistentIsbn = 9780330351691L;
		smallExistentIsbn = 9780330351690L;  // "Into the Wild" by "Jon Krakauer"
		smallExistentIsbn2 = 9780374292799L; // "The World is Flat" by "Thomas Friedman"
		mediumNonexistentIsbn = 9781842190073L;
		mediumExistentIsbn = 9781843190677L; // Cheryl Jones	Herbs for Healthy Skin
		mediumExistentIsbn2 = 9781843191230L; // "An Endless Exile" by "Mary Lancaster"
		
		// libraries
		this.emptyLib = new Library();
		smallLib = new Library();
		smallLib.add(9780374292799L, "Thomas L. Friedman", "The World is Flat");
		smallLib.add(9780330351690L, "Jon Krakauer", "Into the Wild");
		smallLib.add(9780446580342L, "David Baldacci", "Simple Genius");
		mediumLib = new Library();
		mediumLib.addAll("Mushroom_Publishing.txt");
		
		// book lists
		oneBookListIsbn = 9780440351690L;
		oneBookList = new ArrayList<LibraryBook>();
		oneBookList.add(new LibraryBook(oneBookListIsbn, "Not-A-Real Author", "Not-A-Real-Book"));
		manyBookListIsbns = new ArrayList<Long>();
		manyBookListIsbns.add(9780440351690L);
		manyBookListIsbns.add(9780440351890L);
		manyBookListIsbns.add(9780440351990L);
		manyBookList = new ArrayList<LibraryBook>();
		manyBookList.add(new LibraryBook(manyBookListIsbns.get(0), "Not-A-Real Author0", "Not-A-Real-Book0"));
		manyBookList.add(new LibraryBook(manyBookListIsbns.get(1), "Not-A-Real Author1", "Not-A-Real-Book1"));
		manyBookList.add(new LibraryBook(manyBookListIsbns.get(2), "Not-A-Real Author2", "Not-A-Real-Book2"));
	
		// book list filenames
		emptyBookListFname = "emptyBookList.txt";
		oneBookListFname = "oneBookList.txt";
		manyBookListFname = "manyBookList.txt";
		malformedBookListFname = "malformedBookList.txt";
		nonExistentBookListFname = "thisFileDoesNotExist.txt";
	}
	
	/**
	 * Test the "add" method of the "Library" class.
	 * 
	 * Test adding a book and verifying it is in the library.
	 * Perform the above test for empty, small, and medium sized libraries.
	 */
	@Test
	public void testAdd() {
		String dummyAlias = "John Doe";
		long dummyIsbn = 9780440351690L;
		int dummyDay = 1;
		int dummyMonth = 1;
		int dummyYear = 2008;
		
		/// ---empty library---
		emptyLib.add(dummyIsbn, "Not-A-Real Author", "Not-A-Real-Book");
		emptyLib.checkout(dummyIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(emptyLib.lookup(dummyIsbn) == dummyAlias);
		
		/// ---small library---
		smallLib.add(dummyIsbn, "Not-A-Real Author", "Not-A-Real-Book");
		smallLib.checkout(dummyIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(smallLib.lookup(dummyIsbn) == dummyAlias);
		
		/// ---medium library---
		mediumLib.add(dummyIsbn, "Not-A-Real Author", "Not-A-Real-Book");
		mediumLib.checkout(dummyIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(mediumLib.lookup(dummyIsbn) == dummyAlias);
	}
	
	/**
	 * Test that "addAll(String)" method of the "Library" class.
	 * 
	 * Test adding books from an empty file.
	 * Test adding books from a file with one book.
	 * Test adding books from a file with many books.
	 * Test adding books from a malformed file.
	 * Test adding books from a nonexistent file.
	 * Test parameterizing null.
	 * All above tests are performed on empty, small, and medium sized libraries.
	 */
	@Test
	public void testAddAllString() {
		String dummyAlias = "John Doe";
		int dummyDay = 1;
		int dummyMonth = 1;
		int dummyYear = 2008;
		
		/// ---empty lib---
		// empty file
		emptyLib.addAll(emptyBookListFname);
		// file of one book
		emptyLib.addAll(oneBookListFname);
		emptyLib.checkout(oneBookListIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(emptyLib.lookup(oneBookListIsbn).equals(dummyAlias));
		// file of many books
		emptyLib.addAll(manyBookListFname);
		emptyLib.checkout(manyBookListIsbns.get(0), dummyAlias, dummyDay, dummyMonth, dummyYear);
		emptyLib.checkout(manyBookListIsbns.get(1), dummyAlias, dummyDay, dummyMonth, dummyYear);
		emptyLib.checkout(manyBookListIsbns.get(2), dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(emptyLib.lookup(manyBookListIsbns.get(0)).equals(dummyAlias));
		assertTrue(emptyLib.lookup(manyBookListIsbns.get(1)).equals(dummyAlias));
		assertTrue(emptyLib.lookup(manyBookListIsbns.get(2)).equals(dummyAlias));
		// malformed file
		emptyLib.addAll(malformedBookListFname);
		// nonexistent file
		emptyLib.addAll(nonExistentBookListFname);
		// null
		try {
			String param = null;
			emptyLib.addAll(param);
			fail("");
		}
		catch (NullPointerException e) {}
		
		/// ---small lib---
		// empty file
		smallLib.addAll(emptyBookListFname);
		// file of one book
		smallLib.addAll(oneBookListFname);
		smallLib.checkout(oneBookListIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(smallLib.lookup(oneBookListIsbn).equals(dummyAlias));
		// file of many books
		smallLib.addAll(manyBookListFname);
		smallLib.checkout(manyBookListIsbns.get(0), dummyAlias, dummyDay, dummyMonth, dummyYear);
		smallLib.checkout(manyBookListIsbns.get(1), dummyAlias, dummyDay, dummyMonth, dummyYear);
		smallLib.checkout(manyBookListIsbns.get(2), dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(smallLib.lookup(manyBookListIsbns.get(0)).equals(dummyAlias));
		assertTrue(smallLib.lookup(manyBookListIsbns.get(1)).equals(dummyAlias));
		assertTrue(smallLib.lookup(manyBookListIsbns.get(2)).equals(dummyAlias));
		// malformed file
		smallLib.addAll(malformedBookListFname);
		// nonexistent file
		smallLib.addAll(nonExistentBookListFname);
		// null
		try {
			String param = null;
			smallLib.addAll(param);
			fail("");
		}
		catch (NullPointerException e) {}
		
		/// ---medium lib---
		// empty file
		mediumLib.addAll(emptyBookListFname);
		// file of one book
		mediumLib.addAll(oneBookListFname);
		mediumLib.checkout(oneBookListIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(smallLib.lookup(oneBookListIsbn).equals(dummyAlias));
		// file of many books
		mediumLib.addAll(manyBookListFname);
		mediumLib.checkout(manyBookListIsbns.get(0), dummyAlias, dummyDay, dummyMonth, dummyYear);
		mediumLib.checkout(manyBookListIsbns.get(1), dummyAlias, dummyDay, dummyMonth, dummyYear);
		mediumLib.checkout(manyBookListIsbns.get(2), dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(mediumLib.lookup(manyBookListIsbns.get(0)).equals(dummyAlias));
		assertTrue(mediumLib.lookup(manyBookListIsbns.get(1)).equals(dummyAlias));
		assertTrue(mediumLib.lookup(manyBookListIsbns.get(2)).equals(dummyAlias));
		// malformed file
		mediumLib.addAll(malformedBookListFname);
		// nonexistent file
		mediumLib.addAll(nonExistentBookListFname);
		// null
		try {
			String param = null;
			mediumLib.addAll(param);
			fail("");
		}
		catch (NullPointerException e) {}
	}
	
	/**
	 * Test the "addAll(ArrayList<LibraryBook>)" method of the "Library" class.
	 * 
	 * Test adding a list of one.
	 * Test adding a list of many.
	 * Perform all above tests on empty, small, and medium sized libraries.
	 */
	@Test
	public void testAddAllArrayList() {
		String dummyAlias = "John Doe";
		int dummyDay = 1;
		int dummyMonth = 1;
		int dummyYear = 2008;
		
		/// ---empty lib---
		// list of one
		emptyLib.addAll(oneBookList);
		emptyLib.checkout(oneBookListIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(emptyLib.lookup(oneBookListIsbn).equals(dummyAlias));
		// list of many
		emptyLib.addAll(manyBookList);
		emptyLib.checkout(manyBookListIsbns.get(0), dummyAlias, dummyDay, dummyMonth, dummyYear);
		emptyLib.checkout(manyBookListIsbns.get(1), dummyAlias, dummyDay, dummyMonth, dummyYear);
		emptyLib.checkout(manyBookListIsbns.get(2), dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(emptyLib.lookup(manyBookListIsbns.get(0)).equals(dummyAlias));
		assertTrue(emptyLib.lookup(manyBookListIsbns.get(1)).equals(dummyAlias));
		assertTrue(emptyLib.lookup(manyBookListIsbns.get(2)).equals(dummyAlias));
		
		/// ---small lib---
		// list of one
		smallLib.addAll(oneBookList);
		smallLib.checkout(oneBookListIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(smallLib.lookup(oneBookListIsbn).equals(dummyAlias));
		// list of many
		smallLib.addAll(manyBookList);
		smallLib.checkout(manyBookListIsbns.get(0), dummyAlias, dummyDay, dummyMonth, dummyYear);
		smallLib.checkout(manyBookListIsbns.get(1), dummyAlias, dummyDay, dummyMonth, dummyYear);
		smallLib.checkout(manyBookListIsbns.get(2), dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(smallLib.lookup(manyBookListIsbns.get(0)).equals(dummyAlias));
		assertTrue(smallLib.lookup(manyBookListIsbns.get(1)).equals(dummyAlias));
		assertTrue(smallLib.lookup(manyBookListIsbns.get(2)).equals(dummyAlias));
		
		/// ---medium lib---
		// list of one
		mediumLib.addAll(oneBookList);
		mediumLib.checkout(oneBookListIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(mediumLib.lookup(oneBookListIsbn).equals(dummyAlias));
		// list of many
		mediumLib.addAll(manyBookList);
		mediumLib.checkout(manyBookListIsbns.get(0), dummyAlias, dummyDay, dummyMonth, dummyYear);
		mediumLib.checkout(manyBookListIsbns.get(1), dummyAlias, dummyDay, dummyMonth, dummyYear);
		mediumLib.checkout(manyBookListIsbns.get(2), dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(mediumLib.lookup(manyBookListIsbns.get(0)).equals(dummyAlias));
		assertTrue(mediumLib.lookup(manyBookListIsbns.get(1)).equals(dummyAlias));
		assertTrue(mediumLib.lookup(manyBookListIsbns.get(2)).equals(dummyAlias));
	}

	/**
	 * Test the "lookup(String holder)" method of the "Library" class.
	 * 
	 * Test looking up books known to be checked out under a specific name.
	 * Test looking up books known not to be checked out under a specific name.
	 * All tests are performed on empty, small, medium, and large sized libraries.
	 */
	@Test
	public void testLookupHolder() {
		ArrayList<LibraryBook> booksCheckedOut;
		String dummyAlias = "John Doe";
		int dummyDay = 1;
		int dummyMonth = 1;
		int dummyYear = 2008;
		
		/// ---emtpy library---
		booksCheckedOut = emptyLib.lookup(dummyAlias);
		assertTrue(booksCheckedOut.isEmpty());
		
		/// ---small library---
		// nonexistent name
		booksCheckedOut = smallLib.lookup(dummyAlias);
		assertTrue(booksCheckedOut.isEmpty());
		// existent name
		smallLib.checkout(smallExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		booksCheckedOut = smallLib.lookup(dummyAlias);
		assertTrue(booksCheckedOut.size() == 1);
		assertTrue(booksCheckedOut.get(0).getHolder().equals(dummyAlias));
		
		/// ---medium library---
		// nonexistent name
		booksCheckedOut = mediumLib.lookup(dummyAlias);
		assertTrue(booksCheckedOut.isEmpty());
		// existent name
		mediumLib.checkout(mediumExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		booksCheckedOut = mediumLib.lookup(dummyAlias);
		assertTrue(booksCheckedOut.size() == 1);
		assertTrue(booksCheckedOut.get(0).getHolder().equals(dummyAlias));
	}
	
	/**
	 * Test the "lookup(long isbn)" method of the "Library" class.
	 * 
	 * Test looking up an ISBN known not to be in the library.
	 * Test looking up an ISNB known to be in the library.
	 * All tests are performed on empty, small, medium, and large sized libraries.
	 */
	@Test
	public void testLookupIsbn() {
		String retVal;
		String dummyAlias = "John Doe";
		int dummyDay = 1;
		int dummyMonth = 1;
		int dummyYear = 2008;
		
		/// ---empty library---
		retVal = emptyLib.lookup(emptyNonexistentIsbn);
		assertTrue(retVal == null);
		
		/// ---small library---
		// nonexistent isbn
		retVal = smallLib.lookup(smallNonexistentIsbn);
		assertTrue(retVal == null);
		// existent isbn
		smallLib.checkout(smallExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		retVal = smallLib.lookup(smallExistentIsbn);
		assertTrue(retVal.equals(dummyAlias));
		
		/// ---medium library---
		// nonexistent isbn
		retVal = mediumLib.lookup(mediumNonexistentIsbn);
		assertTrue(retVal == null);
		// existent isbn
		mediumLib.checkout(mediumExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		retVal = mediumLib.lookup(mediumExistentIsbn);
		assertTrue(retVal.equals(dummyAlias));
	}
	
	/**
	 * Test the "checkout" method of the "Library" class.
	 * 
	 * Test checking out a book of an isbn not in the library.
	 * Test checking out a book of an isbn in the library.
	 * Test checking out a book that is already checked out.
	 * All above tests are performed on empty, small, and medium sized libraries.
	 */
	@Test
	public void testCheckout() {
		boolean retVal;
		String dummyAlias = "John Doe";
		int dummyDay = 1;
		int dummyMonth = 1;
		int dummyYear = 2008;
		
		/// ---empty library---
		retVal = emptyLib.checkout(emptyNonexistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(!retVal);
		
		/// ---small library---
		// nonexistent isbn
		retVal = smallLib.checkout(smallNonexistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(!retVal);
		// existent isbn
		retVal = smallLib.checkout(smallExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(retVal);
		// already checked out
		retVal = emptyLib.checkout(smallExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(!retVal);
		
		/// ---medium library---
		// nonexistent isbn
		retVal = mediumLib.checkout(mediumNonexistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(!retVal);
		// existent isbn
		retVal = mediumLib.checkout(mediumExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(retVal);
		// already checked out
		retVal = mediumLib.checkout(mediumExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		assertTrue(!retVal);
	}
	
	/**
	 * Test the "checkin(String holder)" method of the "LibraryGeneric" class.
	 * 
	 * Test checking in books under a name that has never had any books out.
	 * Test checking in books under a name that checked out books but returned them.
	 * Test checking in books under a name with one book out.
	 * Test checking in books under a name with many books out.
	 * All above tests are performed on empty, small, and medium sized libraries.
	 */
	@Test
	public void testCheckinHolder() {
		boolean retVal;
		String dummyAlias = "John Doe";
		int dummyDay = 1;
		int dummyMonth = 1;
		int dummyYear = 2008;
		
		/// ---empty library---
		retVal = emptyLib.checkin(dummyAlias);
		assertTrue(!retVal);
		
		/// ---small library---
		// name never had books
		retVal = smallLib.checkin(dummyAlias);
		assertTrue(!retVal);
		// name with returned books
		smallLib.checkout(smallExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		smallLib.checkin(dummyAlias);
		retVal = smallLib.checkin(dummyAlias);
		assertTrue(!retVal);
		// name with one book out
		smallLib.checkout(smallExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		retVal = smallLib.checkin(dummyAlias);
		assertTrue(retVal);
		// name with many books out
		smallLib.checkout(smallExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		smallLib.checkout(smallExistentIsbn2, dummyAlias, dummyDay, dummyMonth, dummyYear);
		retVal = smallLib.checkin(dummyAlias);
		assertTrue(retVal);
		
		/// ---medium library---
		// name never had books
		retVal = mediumLib.checkin(dummyAlias);
		assertTrue(!retVal);
		// name checked in books
		mediumLib.checkout(mediumExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		mediumLib.checkin(dummyAlias);
		retVal = mediumLib.checkin(dummyAlias);
		assertTrue(!retVal);
		// name with one book
		mediumLib.checkout(mediumExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		retVal = mediumLib.checkin(dummyAlias);
		assertTrue(retVal);
		// name with many books
		mediumLib.checkout(mediumExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		mediumLib.checkout(mediumExistentIsbn2, dummyAlias, dummyDay, dummyMonth, dummyYear);
		retVal = mediumLib.checkin(dummyAlias);
		assertTrue(retVal);
	}
	
	/**
	 * Test the "checkin(long isbn)" method of the "LibraryGeneric" class.
	 * 
	 * Test checking in an isbn known not to be in the library.
	 * Test checking in an isbn that is not checked out.
	 * Test checking in an isbn that is checked out.
	 * Test checking in an isbn that has already been checked in.
	 * All above tests are performed on empty, small, and medium sized libraries.
	 */
	@Test
	public void testCheckinIsbn() {
		boolean retVal;
		String dummyAlias = "John Doe";
		int dummyDay = 1;
		int dummyMonth = 1;
		int dummyYear = 2008;
		
		/// ---empty library---
		retVal = emptyLib.checkin(emptyNonexistentIsbn);
		assertTrue(!retVal);
		
		/// ---small library---
		// nonexistent isbn
		retVal = smallLib.checkin(smallNonexistentIsbn);
		assertTrue(!retVal);
		// not checked out
		retVal = smallLib.checkin(smallExistentIsbn);
		assertTrue(!retVal);
		// check in a book that is checked out
		smallLib.checkout(smallExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		retVal = smallLib.checkin(smallExistentIsbn);
		assertTrue(retVal);
		// check in book that has already been checked in
		smallLib.checkout(smallExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		smallLib.checkin(dummyAlias);
		retVal = smallLib.checkin(dummyAlias);
		assertTrue(!retVal);
		
		/// ---medium library---
		// nonexistent isbn
		retVal = mediumLib.checkin(mediumNonexistentIsbn);
		assertTrue(!retVal);
		// not checked out
		retVal = mediumLib.checkin(mediumExistentIsbn);
		assertTrue(!retVal);
		// check in a book that is checked out
		mediumLib.checkout(mediumExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		retVal = mediumLib.checkin(mediumExistentIsbn);
		assertTrue(retVal);
		// check in a book that has already been returned
		mediumLib.checkout(mediumExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		mediumLib.checkin(dummyAlias);
		retVal = mediumLib.checkin(dummyAlias);
		assertTrue(!retVal);
	}
}
