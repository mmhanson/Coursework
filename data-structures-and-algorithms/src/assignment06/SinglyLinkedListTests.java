package assignment06;

import static org.junit.Assert.assertTrue;
import static org.junit.Assert.fail;

import java.util.Arrays;
import java.util.Iterator;
import java.util.NoSuchElementException;

import org.junit.Before;
import org.junit.Test;

/**
 * A class to test the SinglyLinkedList class.
 * 
 * @author Maxwell Hanson
 * @since Mar 1, 2018
 * @version 1.1.0
 */
public class SinglyLinkedListTests {
	private SinglyLinkedList<Integer> emptyList;
	private SinglyLinkedList<Integer> oneList;
	private SinglyLinkedList<Integer> manyList;
	
	/**
	 * Setup the class SinglyLinkedLIstTests for testing.
	 */
	@Before
	public void setup() {
		this.emptyList = new SinglyLinkedList<Integer>();
		
		this.oneList = new SinglyLinkedList<Integer>();
		this.oneList.addFirst(1);
		
		this.manyList = new SinglyLinkedList<Integer>();
		this.manyList.addFirst(10);
		this.manyList.addFirst(9);
		this.manyList.addFirst(8);
		this.manyList.addFirst(7);
		this.manyList.addFirst(6);
		this.manyList.addFirst(5);
		this.manyList.addFirst(4);
		this.manyList.addFirst(3);
		this.manyList.addFirst(2);
		this.manyList.addFirst(1);
	}
	
	/**
	 * Test the default of the SinglyLinkedList class.
	 * 
	 * Test that the default constructor returns a non-null object.
	 * Test that the size method returns 0.
	 * Test that isEmpty returns true.
	 * Test that calling the get method throws IndexOutOfBoundsException.
	 * Test that calling getFirst throws IOOBE.
	 * Test that calling remove throws IOOBE.
	 * Test that calling removeFirst throws NoSuchElementException.
	 */
	@Test
	public void testDefaultConstructor() {
		SinglyLinkedList<Integer> list = new SinglyLinkedList<Integer>();
		
		// not equal to null
		assertTrue(list != null);
		
		// size
		assertTrue(list.size() == 0);
		
		// isEmpty
		assertTrue(list.isEmpty());
		
		// get method 
		try {
			list.get(0);
			fail("The defualt constructor returned an element of an empty list.");
		}
		catch (IndexOutOfBoundsException e) {}
		try {
			list.getFirst();
			fail("The defualt constructor returned an element of an empty list.");
		}
		catch (NoSuchElementException e) {}
		
		// remove method
		try {
			list.remove(0);
			fail("The defualt constructor removed an element of an empty list.");
		}
		catch (IndexOutOfBoundsException e) {}
		try {
			list.removeFirst();
			fail("The defualt constructor removed an element of an empty list.");
		}
		catch (NoSuchElementException e) {}
	}
	
	/**
	 * Test the addFirst method of the SinglyLinkedList class.
	 * 
	 * Test adding an element to an empty list.
	 * Test adding an element to a list of size one.
	 * Test adding an element to a list of size many.
	 * For all previous tests, test that, after the call, the list size is incremented, and
	 *   that the element added is the first element of the list.
	 */
	@Test
	public void testAddFirst() {
		int sizeBefore;
		Integer elemAdded;
		
		// empty list
		sizeBefore = emptyList.size();
		elemAdded = 1;
		emptyList.addFirst(elemAdded);
		assertTrue(sizeBefore == (emptyList.size() - 1));
		assertTrue(emptyList.getFirst().equals(elemAdded));
		
		// one list
		sizeBefore = oneList.size();
		elemAdded = 0;
		oneList.addFirst(elemAdded);
		assertTrue(sizeBefore == (oneList.size() - 1));
		assertTrue(oneList.getFirst().equals(elemAdded));
		
		// many list
		sizeBefore = manyList.size();
		elemAdded = 0;
		manyList.addFirst(elemAdded);
		assertTrue(sizeBefore == (manyList.size() - 1));
		assertTrue(manyList.getFirst().equals(elemAdded));
		
	}
	
	/**
	 * Test the  method of the SinglyLinkedList class.
	 * 
	 * Test adding an element to an empty list.
	 * Test adding an element to the front of a list of size one.
	 * Test adding an element to the back of a list of size one.
	 * Test adding an element to the front of a list of size many.
	 * Test adding an element to the back of a list of size many.
	 * Test adding an element to a position between the front and the back of a list of size many.
	 * For all previous tests, test that, after the call, the list size is incremented, and
	 *   that the element at the index parameter is equal the the element parameter.
	 * Test adding an element to a nonexistent index of a list of size many.
	 * 
	 */
	@Test
	public void testAdd() {
		int indexAdded;
		int sizeBefore;
		Integer elemAdded;
		
		/// empty list
		indexAdded = 0;
		elemAdded = 0;
		sizeBefore = emptyList.size();
		emptyList.add(indexAdded, elemAdded);
		assertTrue(emptyList.get(indexAdded).equals(elemAdded));
		assertTrue(sizeBefore == (emptyList.size() - 1));
		
		/// one list
		// front
		indexAdded = 0;
		elemAdded = 0;
		sizeBefore = oneList.size();
		oneList.add(indexAdded, elemAdded);
		assertTrue(oneList.get(indexAdded).equals(elemAdded));
		assertTrue(sizeBefore == (oneList.size() - 1));
		// back
		indexAdded = oneList.size();
		elemAdded = 0;
		sizeBefore = oneList.size();
		oneList.add(indexAdded, elemAdded);
		assertTrue(oneList.get(indexAdded).equals(elemAdded));
		assertTrue(sizeBefore == (oneList.size() - 1));
		
		/// many list
		// front
		indexAdded = 0;
		elemAdded = 0;
		sizeBefore = manyList.size();
		manyList.add(indexAdded, elemAdded);
		assertTrue(manyList.get(indexAdded).equals(elemAdded));
		assertTrue(sizeBefore == (manyList.size() - 1));
		// back
		indexAdded = manyList.size();
		elemAdded = 0;
		sizeBefore = manyList.size();
		manyList.add(indexAdded, elemAdded);
		assertTrue(manyList.get(indexAdded).equals(elemAdded));
		assertTrue(sizeBefore == (manyList.size() - 1));
		// mid
		indexAdded = manyList.size() / 2;
		elemAdded = 0;
		sizeBefore = manyList.size();
		manyList.add(indexAdded, elemAdded);
		assertTrue(manyList.get(indexAdded).equals(elemAdded));
		assertTrue(sizeBefore == (manyList.size() - 1));
		
		// nonexistent index
		try {
			manyList.add(manyList.size() + 10, 10);
			fail ("Successfully added element to a nonexistent index.");
		}
		catch (IndexOutOfBoundsException e) {}
	}
	
	/**
	 * Test the getFirst method of the SinglyLinkedList class.
	 * 
	 * Test that an empty list throws an exception.
	 * Test that a list of size one returns the sole element.
	 * Test that a list of size many returns the first element.
	 */
	@Test
	public void testGetFirst() {
		Integer expRes;
		Integer res;
		
		// emtpy list
		try {
			emptyList.getFirst();
			fail ("Successfully retrieved an element that does not exsist.");
		}
		catch (NoSuchElementException e) {}
		
		// one list
		expRes = oneList.get(0);
		res = oneList.getFirst();
		assertTrue(expRes.equals(res));
		
		// many list
		expRes = manyList.get(0);
		res = manyList.getFirst();
		assertTrue(expRes.equals(res));
	}
	
	/**
	 * Test the get method of the SinglyLinkedList class.
	 * 
	 * Test that getting the first element of an empty list throws an exception.
	 * Test that getting the first element of a list of size one returns the sole element.
	 * Test getting the first element of a list of size many.
	 * Test getting the last element of a list of size many.
	 * Test getting the "middle" element of a list of size many.
	 * Test getting an element at an index that doesn't exist of a list of size many.
	 */
	@Test
	public void testGet() {
		Integer expRes, res;
		
		/// empty list
		try {
			emptyList.get(0);
			fail("Successfully retrieved nonexistent elemtn.");
		}
		catch (IndexOutOfBoundsException e) {}
		
		/// one list
		// first
		expRes = oneList.getFirst();
		res = oneList.get(0);
		assertTrue(expRes.equals(res));
		
		/// many list
		// first
		expRes = manyList.getFirst();
		res = manyList.get(0);
		assertTrue(expRes.equals(res));
		// last
		expRes = 10;
		res = manyList.get(manyList.size() - 1);
		assertTrue(expRes.equals(res));
		// mid
		expRes = 5;
		res = manyList.get(4);
		assertTrue(expRes.equals(res));
		
		// nonexistent index
		try {
			manyList.get(manyList.size() + 10);
			fail ("Successfully retrieved a nonexistent element.");
		}
		catch (IndexOutOfBoundsException e) {}
	}
	
	/**
	 * Test the removeFirst method of the SinglyLinkedList class.
	 * 
	 * Test an empty list (should throw exception).
	 * Test a list of size one.
	 * Test a list of size many.
	 */
	@Test
	public void testRemoveFirst() {
		int sizeBefore;
		int secElem;
		
		// empty list
		try {
			emptyList.removeFirst();
			fail("");
		}
		catch (NoSuchElementException e) {}
		
		// one list
		sizeBefore = oneList.size();
		oneList.removeFirst();
		assertTrue(oneList.size() == (sizeBefore - 1));
		assertTrue(oneList.isEmpty());
		
		// many list
		sizeBefore = manyList.size();
		secElem = manyList.get(1);
		manyList.removeFirst();
		assertTrue(manyList.size() == (sizeBefore - 1));
		assertTrue(manyList.getFirst().equals(secElem));
	}
	
	/**
	 * Test the remove method of the SinglyLinkedList class.
	 * 
	 * Test an empty list.
	 * Test removing the first element of a list of size one.
	 * Test removing the last element of a list of size one.
	 * Test removing the first element of a list of size many.
	 * Test removing the last element of a list of size many.
	 * Test removing the "middle" element of a list of size many.
	 * Test removing the element at an index that doesn't exist of a list of size manuy.
	 */
	@Test
	public void testRemove() {
		int sizeBefore;
		Integer elemRem;
		
		/// empty list
		try {
			emptyList.remove(0);
			fail ("");
		}
		catch (IndexOutOfBoundsException e) {}
		
		/// one list
		// first
		oneList.remove(0);
		assertTrue(oneList.size() == 0);
		assertTrue(oneList.isEmpty());
		oneList.addFirst(1);
		// last
		oneList.remove(oneList.size() - 1);
		assertTrue(oneList.size() == 0);
		assertTrue(oneList.isEmpty());
		
		/// many list
		// first
		sizeBefore = manyList.size();
		elemRem = manyList.getFirst();
		manyList.remove(0);
		assertTrue(manyList.size() == (sizeBefore - 1));
		manyList.addFirst(elemRem);
		// last
		sizeBefore = manyList.size();
		elemRem = manyList.get(sizeBefore - 1);
		manyList.remove(manyList.size() - 1);
		assertTrue(manyList.size() == (sizeBefore - 1));
		manyList.add(manyList.size(), elemRem);
		// mid
		sizeBefore = manyList.size();
		manyList.remove(manyList.size() / 2);
		assertTrue(manyList.size() == (sizeBefore - 1));
		
		// nonexistent element
		try {
			manyList.remove(manyList.size() + 10);
			fail ("");
		}
		catch (IndexOutOfBoundsException e) {}
	}
	
	/**
	 * Test the indexOf method of the SinglyLinkedList class.
	 * 
	 * Test an empty list.
	 * Test a list of size one with an element that is in it.
	 * Test a list of size one with an element that is not in it.
	 * Test a list of size many with an element that is in it.
	 * Test a list of size many with an element that is not in it.
	 * Test parameterizing null.
	 */
	@Test
	public void testIndexOf() {
		int res, expRes;
		
		/// empty list
		expRes = -1;
		res = emptyList.indexOf(3);
		assertTrue(expRes == res);
		
		/// one list
		// in
		expRes = 0;
		res = oneList.indexOf(1);
		assertTrue(expRes == res);    
		// not in
		expRes = -1;
		res = oneList.indexOf(10);
		assertTrue(expRes == res);
		
		/// many list
		// in
		expRes = 4;
		res = manyList.indexOf(5);
		assertTrue(expRes == res);
		// not in
		expRes = -1;
		res = manyList.indexOf(11);
		assertTrue(expRes == res);
	}
	
	/**
	 * Test the size method of the SinglyLinkedList class.
	 * 
	 * Test an empty list.
	 * Test a list of size one.
	 * Test a list of size many.
	 */
	@Test
	public void testSize() {
		// empty list
		assertTrue(emptyList.size() == 0);
		
		// one list
		assertTrue(oneList.size() == 1);
		
		// many list
		assertTrue(manyList.size() == 10);
	}
	
	/**
	 * Test the isEmpty method of the SinglyLinkedList class.
	 * 
	 * Test an empty list.
	 * Test a list of size one.
	 * Test a list of size many.
	 */
	@Test
	public void testIsEmpty() {
		assertTrue(emptyList.isEmpty());
		assertTrue(!oneList.isEmpty());
		assertTrue(!manyList.isEmpty());
	}
	
	/**
	 * Test the clear method of the SinglyLinkedList class.
	 * 
	 * Test an empty list.
	 * Test a list of size one.
	 * Test a list of size many.
	 */
	@Test
	public void testClear() {
		// empty list
		emptyList.clear();
		assertTrue(emptyList.isEmpty());
		
		// one list
		assertTrue(!oneList.isEmpty());
		oneList.clear();
		assertTrue(oneList.isEmpty());
		
		// many list
		assertTrue(!manyList.isEmpty());
		manyList.clear();
		assertTrue(manyList.isEmpty());
	}
	
	/**
	 * Test the toArray method of the SinglyLinkedList class.
	 * 
	 * Test an empty list.
	 * Test a list of size one.
	 * Test a list of size many.
	 * For all above tests, test that the method returns an array of the same size as the list.
	 * For all above tests, test that each element in the array is the same as the corresponding
	 *   element in the list.
	 */
	@Test
	public void testToArray() {
		Object[] expRes;
		Object[] res;
		
		// empty list
		expRes = new Object[] {};
		res = emptyList.toArray();
		assertTrue(res.length == emptyList.size());
		assertTrue(Arrays.equals(expRes, res));
		
		// one list
		expRes = new Object[] {1};
		res = oneList.toArray();
		assertTrue(res.length == oneList.size());
		assertTrue(Arrays.equals(expRes, res));
		
		// many list
		expRes = new Object[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
		res = manyList.toArray();
		assertTrue(res.length == manyList.size());
		assertTrue(Arrays.equals(expRes, res));
	}
	
	/**
	 * Test the iterator method of the SinglyLinkedList class.
	 * 
	 * Test that the iterator of an empty list does not have a next.
	 * Test that the iterator of an empty list throws NoSuchElementException on next().
	 * Test that a list of one has next.
	 * Test that the next method of a set of one returns the sole element.
	 * Test that the list of one no longer has next after previous test.
	 * Test that a list of many has next.
	 * Test that each call of next returns the correct element for a list of many.
	 * Test that after `manylist.size()` calls of next, list of many no longer has next.
	 */
	@Test
	public void testIterator() {
		Iterator<Integer> iter;
		
		/// empty list
		iter = emptyList.iterator();
		assertTrue(!iter.hasNext());
		try {
			iter.next();
			fail ("");
		}
		catch (NoSuchElementException e) {}
		
		/// one list
		iter = oneList.iterator();
		assertTrue(iter.hasNext());
		assertTrue(iter.next() == oneList.getFirst());
		assertTrue(!iter.hasNext());
		
		/// many list
		iter = manyList.iterator();
		assertTrue(iter.hasNext());
		for (int i = 0; i < manyList.size(); i++) {
			assertTrue(iter.next().equals(manyList.get(i)));
		}
		assertTrue(!iter.hasNext());
	}
}
