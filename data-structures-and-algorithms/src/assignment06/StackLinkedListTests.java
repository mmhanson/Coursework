package assignment06;

import static org.junit.Assert.assertTrue;
import static org.junit.Assert.fail;

import java.util.NoSuchElementException;

import org.junit.Before;
import org.junit.Test;

/**
 * Automated testing for the StackLinkedList class.
 * 
 * @author Maxwell Hanson
 * @since Jul 17, 2018
 * @version 1.1.0
 */
public class StackLinkedListTests {
	private StackLinkedList<Integer> emptyList;
	private StackLinkedList<Integer> oneList;
	private StackLinkedList<Integer> manyList;
	private int manyListSize;
	
	/**
	 * Setup the class SinglyLinkedLIstTests for testing.
	 */
	@Before
	public void setup() {
		this.emptyList = new StackLinkedList<Integer>();
		
		this.oneList = new StackLinkedList<Integer>();
		this.oneList.push(1);
		
		this.manyList = new StackLinkedList<Integer>();
		this.manyList.push(10);
		this.manyList.push(9);
		this.manyList.push(8);
		this.manyList.push(7);
		this.manyList.push(6);
		this.manyList.push(5);
		this.manyList.push(4);
		this.manyList.push(3);
		this.manyList.push(2);
		this.manyList.push(1);
		this.manyListSize = 10;
	}
	
	/**
	 * Test the defualt constructor of the StackLinkedList class
	 * 
	 * Test that it returns a non-null object.
	 * Test that it is empty.
	 * Test that peek throws NoSuchElementException.
	 * Test that pop throws NoSuchElementException.
	 * Test that size returns zero.
	 */
	@Test
	public void testDefaultConstructor() {
		StackLinkedList<Integer> defList = new StackLinkedList<Integer>();
		
		// null test
		assertTrue(defList != null);
		
		// empty test
		assertTrue(defList.isEmpty());
		
		// peek
		try {
			defList.peek();
			fail ("");
		}
		catch (NoSuchElementException e) {}
		
		// pop
		try {
			defList.pop();
			fail ("");
		}
		catch (NoSuchElementException e) {}
		
		// size
		assertTrue(defList.size() == 0);
	}
	
	/**
	 * Test the "clear" method of the StackLinkedList class
	 * 
	 * Test an empty list.
	 * Test a list of size one.
	 * Test a list of size many.
	 */
	@Test
	public void testClear() {
		// empty list
		assertTrue(emptyList.isEmpty());
		emptyList.clear();
		assertTrue(emptyList.isEmpty());
		
		// one list
		assertTrue(!oneList.isEmpty());
		oneList.clear();
		assertTrue(emptyList.isEmpty());
		
		// many list
		assertTrue(!manyList.isEmpty());
		manyList.clear();
		assertTrue(manyList.isEmpty());
	}
	
	/**
	 * Test the "isEmpty" method of the StackLinkedList class
	 * 
	 * Test an empty list.
	 * Test a list of size one.
	 * Test a list of size many.
	 */
	@Test
	public void testIsEmpty() {
		// empty list
		assertTrue(emptyList.isEmpty());
		
		// one list
		assertTrue(!oneList.isEmpty());
		
		// many list
		assertTrue(!manyList.isEmpty());
	}
	
	/**
	 * Test the "peek" method of the StackLinkedList class
	 * 
	 * Test an empty list.
	 * Test a list of size one.
	 * Test a list of size many.
	 * For each test above, test that the size remains the same, and test that the 
	 *   element returned is the top element.
	 */
	@Test
	public void testPeek() {
		int sizeBefore;
		
		// empty list
		try {
			emptyList.peek();
			fail ("");
		}
		catch (NoSuchElementException e) {}
		
		// one list
		sizeBefore = oneList.size();
		assertTrue(oneList.peek() == 1);
		assertTrue(sizeBefore == oneList.size());
		
		// many list
		sizeBefore = manyList.size();
		assertTrue(manyList.peek() == 1);
		assertTrue(sizeBefore == manyList.size());
	}
	
	/**
	 * Test the "pop" method of the StackLinkedList class
	 * 
	 * Test an empty list.
	 * Test a list of size one.
	 * Test a list of size many.
	 * For each of the above tests, test that the size decrements and that the popped element
	 *   is not in the the stack after the call.
	 */
	@Test
	public void testPop() {
		int sizeBefore;
		Integer res, expRes;
		
		// empty list
		try {
			emptyList.pop();
			fail ("");
		}
		catch (NoSuchElementException e) {}
		
		// one list
		sizeBefore = oneList.size();
		expRes = 1;
		res = oneList.pop();
		assertTrue(expRes.equals(res));
		assertTrue(oneList.size() == (sizeBefore - 1));
		
		// many list
		sizeBefore = manyList.size();
		expRes = 1;
		res = manyList.pop();
		assertTrue(expRes.equals(res));
		assertTrue(manyList.size() == (sizeBefore - 1));
	}
	
	/**
	 * Test the "push" method of the StackLinkedList class
	 * 
	 * Test an empty list.
	 * Test a list of size one.
	 * Test a list of size many.
	 * For each of the above tests, test that the size is incrimented, and that the element
	 *   is in the stack after, but not before, the call.
	 */
	@Test
	public void testPush() {
		int sizeBefore;
		
		// emtpy list
		sizeBefore = emptyList.size();
		emptyList.push(1);
		assertTrue(emptyList.size() == (sizeBefore + 1));
		assertTrue(emptyList.peek().equals(1));
		
		// one list
		sizeBefore = oneList.size();
		oneList.push(2);
		assertTrue(oneList.size() == (sizeBefore + 1));
		assertTrue(oneList.peek().equals(2));
		
		// many list
		sizeBefore = manyList.size();
		manyList.push(20);
		assertTrue(manyList.size() == (sizeBefore + 1));
		assertTrue(manyList.peek().equals(20));
	}
	
	/**
	 * Test the "size" method of the StackLinkedList class
	 * 
	 * Test an empty list.
	 * Test a list of size one.
	 * Test a list of size many.
	 * For each of the above tests, test that the call returns the correct size.
	 */
	@Test
	public void testSize() {
		// empty list
		assertTrue(emptyList.size() == 0);
		
		// one list
		assertTrue(oneList.size() == 1);
		
		// many list
		assertTrue(manyList.size() == manyListSize);
	}
}
