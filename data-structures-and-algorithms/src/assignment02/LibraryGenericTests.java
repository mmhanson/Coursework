package assignment02;

import static org.junit.Assert.assertTrue;
import static org.junit.Assert.fail;

import java.util.ArrayList;
import org.junit.Before;
import org.junit.Test;

/**
 * Testing suite for the LibraryGeneric class.
 */
public class LibraryGenericTests {
	/**
	 * A library of no books.
	 * Holders are defined by strings.
	 */
	private LibraryGeneric<String> emptyLib;
	/**
	 * A library of less than a dozen books.
	 * Holders are defined by strings.
	 */
	private LibraryGeneric<String> smallLib;
	/**
	 * A library of between a dozen and a hundred books.
	 * Holders are defined by strings.
	 */
	private LibraryGeneric<String> mediumLib;
	/**
	 * An ISBN known to not be included in the empty library
	 */
	private long emptyNonexistentIsbn;
	/**
	 * An ISBN known not to be included in the small library.
	 */
	private long smallNonexistentIsbn;
	/**
	 * An ISBN known to be included in the small library.
	 */
	private long smallExistentIsbn;
	/**
	 * Another ISBN known to be included in the small library.
	 */
	private long smallExistentIsbn2;
	/**
	 * An ISBN known not to be included in the medium library.
	 */
	private long mediumNonexistentIsbn;
	/**
	 * An ISBN known to be included in the medium library.
	 */
	private long mediumExistentIsbn;
	/**
	 * Another ISBN known to be included in the medium library.
	 */
	private long mediumExistentIsbn2;
	/**
	 * A list of one library book.
	 */
	private ArrayList<LibraryBookGeneric<String>> oneBookList;
	/**
	 * The isbn of the book in the oneBookList
	 */
	private long oneBookListIsbn;
	/**
	 * A list of many books
	 */
	private ArrayList<LibraryBookGeneric<String>> manyBookList;
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
		mediumExistentIsbn = 9781843190677L; // "Herbs for Healthy Skin" by "Cheryl Jones"
		mediumExistentIsbn2 = 9781843191230L; // "An Endless Exile" by "Mary Lancaster"
		
		// libraries
		// empty library
		this.emptyLib = new LibraryGeneric<String>();
		// small library
		smallLib = new LibraryGeneric<String>();
		smallLib.add(9780374292799L, "Thomas L. Friedman", "The World is Flat");
		smallLib.add(9780330351690L, "Jon Krakauer", "Into the Wild");
		smallLib.add(9780446580342L, "David Baldacci", "Simple Genius");
		// medium library
		mediumLib = new LibraryGeneric<String>();
		mediumLib.addAll("Mushroom_Publishing.txt");
		
		
		// book lists
		oneBookListIsbn = 9780440351690L;
		oneBookList = new ArrayList<LibraryBookGeneric<String>>();
		oneBookList.add(new LibraryBookGeneric<String>(oneBookListIsbn, "Not-A-Real Author", "Not-A-Real-Book"));
		manyBookListIsbns = new ArrayList<Long>();
		manyBookListIsbns.add(9780440351690L);
		manyBookListIsbns.add(9780440351890L);
		manyBookListIsbns.add(9780440351990L);
		manyBookList = new ArrayList<LibraryBookGeneric<String>>();
		manyBookList.add(new LibraryBookGeneric<String>(manyBookListIsbns.get(0), "Not-A-Real Author0", "Not-A-Real-Book0"));
		manyBookList.add(new LibraryBookGeneric<String>(manyBookListIsbns.get(1), "Not-A-Real Author1", "Not-A-Real-Book1"));
		manyBookList.add(new LibraryBookGeneric<String>(manyBookListIsbns.get(2), "Not-A-Real Author2", "Not-A-Real-Book2"));
	
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
	 * Test the "getInventoryList" method of the "LibraryGeneric" class.
	 * 
	 * Test retrieving the inventory.
	 * Test that the inventory is the correct size,
	 * Test that the inventory is the correct order, that is, ascending by isbn.
	 * All above test are conducted on empty, small, and medium libraries.
	 */
	@Test
	public void testGetInventoryList() {
		ArrayList<LibraryBookGeneric<String>> bookList;
		
		/// ---empty library---
		bookList = emptyLib.getInventoryList();
		assertTrue(bookList.isEmpty());
		
		/// ---small library---
		bookList = smallLib.getInventoryList();
		assertTrue(bookList.size() == 3);
		for (int cursor = 1; cursor < bookList.size(); cursor++) {
			long prevIsbn = bookList.get(cursor - 1).getIsbn();
			long currIsbn = bookList.get(cursor).getIsbn();
			assertTrue(prevIsbn < currIsbn);
		}
		
		/// ---medium library---
		bookList = mediumLib.getInventoryList();
		assertTrue(bookList.size() == 23);
		for (int cursor = 1; cursor < bookList.size(); cursor++) {
			long prevIsbn = bookList.get(cursor - 1).getIsbn();
			long currIsbn = bookList.get(cursor).getIsbn();
			assertTrue(prevIsbn < currIsbn);
		}
	}
	
	/**
	 * Test the "getOrderedByAuthor" method of the "LibraryGeneric" class.
	 * 
	 * Test retrieving the inventory.
	 * Test the inventory is the correct size.
	 * Test the inventory is of the correct  order, that is, ordered lexicographically
	 *   by author name, then lexicographically by title.
	 * All above test are conducted on empty, small, and medium sized libraries.
	 */
	@Test
	public void testGetOrderedByAuthorString() {
		ArrayList<LibraryBookGeneric<String>> bookList;
		
		/// ---empty library---
		bookList = emptyLib.getOrderedByAuthor();
		assertTrue(bookList.isEmpty());
		
		/// ---small library ---
		bookList = smallLib.getOrderedByAuthor();
		assertTrue(bookList.size() == 3);
		// test order (lexicographically by author, then by title)
		for (int cursor = 1; cursor < bookList.size(); cursor++) {
			String prevAuthor = bookList.get(cursor - 1).getAuthor();
			String currAuthor = bookList.get(cursor).getAuthor();
			String prevTitle = bookList.get(cursor - 1).getTitle();
			String currTitle = bookList.get(cursor).getTitle();
			assertTrue(prevAuthor.compareTo(currAuthor) < 0 ||
				prevTitle.compareTo(currTitle) < 0);
		}
		
		/// ---medium library---
		bookList = mediumLib.getOrderedByAuthor();
		assertTrue(bookList.size() == 23);
		// test order (lexicographically by author, then by title)
		for (int cursor = 1; cursor < bookList.size(); cursor++) {
			String prevAuthor = bookList.get(cursor - 1).getAuthor();
			String currAuthor = bookList.get(cursor).getAuthor();
			String prevTitle = bookList.get(cursor - 1).getTitle();
			String currTitle = bookList.get(cursor).getTitle();
			assertTrue(prevAuthor.compareTo(currAuthor) < 0 ||
				prevTitle.compareTo(currTitle) < 0);
		}
	}
	
	/**
	 * Test the "getOverdueList" method of the "LibraryGeneric" class.
	 * 
	 * Test retrieving a list of overdue books when there are none.
	 * Test retrieving a list of overdue books when there is one.
	 * Test retrieving a list of overdue books when there are many.
	 * All above tests are performed on empty, small, medium, and large sized libraries.
	 */
	@Test
	public void testGetOverdueList() {
		ArrayList<LibraryBookGeneric<String>> overdueList;
		String dummyAlias = "John Doe";
		int dummyDay = 1;
		int dummyMonth = 1;
		int dummyYear = 2008;
		
		/// ---empty library---
		overdueList = emptyLib.getOverdueList(dummyDay, dummyMonth, dummyYear);
		assertTrue(overdueList.isEmpty());
		
		/// ---small library---
		// no overdue books
		overdueList = smallLib.getOverdueList(dummyDay, dummyMonth, dummyYear);
		assertTrue(overdueList.isEmpty());
		// one overdue book
		smallLib.checkout(smallExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		overdueList = smallLib.getOverdueList(dummyDay, dummyMonth, Integer.MAX_VALUE);
		assertTrue(overdueList.size() == 1);
		assertTrue(overdueList.get(0).getIsbn() == smallExistentIsbn);
		// many overdue books
		smallLib.checkout(smallExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		smallLib.checkout(smallExistentIsbn2, dummyAlias, dummyDay, dummyMonth, dummyYear + 1);
		overdueList = smallLib.getOverdueList(dummyDay, dummyMonth, Integer.MAX_VALUE);
		assertTrue(overdueList.size() == 2);
		assertTrue(overdueList.get(0).getIsbn() == smallExistentIsbn);
		assertTrue(overdueList.get(1).getIsbn() == smallExistentIsbn2);
		
		
		/// ---medium library---
		// no overdue books
		overdueList = mediumLib.getOverdueList(dummyDay, dummyMonth, dummyYear);
		assertTrue(overdueList.isEmpty());
		// one overdue book
		mediumLib.checkout(mediumExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		overdueList = mediumLib.getOverdueList(dummyDay, dummyMonth, Integer.MAX_VALUE);
		assertTrue(overdueList.size() == 1);
		assertTrue(overdueList.get(0).getIsbn() == mediumExistentIsbn);
		// many overdue books
		mediumLib.checkout(mediumExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
		mediumLib.checkout(mediumExistentIsbn2, dummyAlias, dummyDay, dummyMonth, dummyYear);
		overdueList = mediumLib.getOverdueList(dummyDay, dummyMonth, Integer.MAX_VALUE);
		assertTrue(overdueList.size() == 2);
		assertTrue(overdueList.get(0).getIsbn() == mediumExistentIsbn);
		assertTrue(overdueList.get(1).getIsbn() == mediumExistentIsbn2);
	}
		
	/**
	 * Test the "lookup(String holder)" method of the "LibraryGeneric" class.
	 * 
	 * Test looking up books known to be checked out under a specific name.
	 * Test looking up books known not to be checked out under a specific name.
	 * All tests are performed on empty, small, medium, and large sized libraries.
	 */
	@Test
	public void testLookupHolder() {
		ArrayList<LibraryBookGeneric<String>> booksCheckedOut;
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
	 * Test the "lookup(long isbn)" method of the "LibraryGeneric" class.
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
		assertTrue(retVal == dummyAlias);
	}
	
	/**
	 * Test the "checkout" method of the "LibraryGeneric" class.
	 * 
	 * Test checking out a book that is not in the library.
	 * Test checking out a book that is in the library.
	 * Test checking out a book that is already in the library.
	 * All of the above tests are performed on empty, small, and medium sized libraries.
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
		retVal = smallLib.checkout(smallExistentIsbn, dummyAlias, dummyDay, dummyMonth, dummyYear);
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
		smallLib.checkin(smallExistentIsbn);
		retVal = smallLib.checkin(smallExistentIsbn);
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
		retVal = mediumLib.checkin(mediumExistentIsbn);
		assertTrue(!retVal);
	}
}
