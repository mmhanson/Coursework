package assignment07;

import static org.junit.Assert.assertTrue;
import static org.junit.Assert.fail;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.NoSuchElementException;

import org.junit.Before;
import org.junit.Test;

/**
 * Automated tests for the BinarySearchTree class.
 * 
 * @author Maxwell
 * @version 1.3.0
 * @since Mar 6, 2018
 */
public class BinarySearchTreeTests {
	// attributes
	BinarySearchTree<Integer> emptySet;
	BinarySearchTree<Integer> oneSet;
	BinarySearchTree<Integer> manySet;
	Collection<Integer> nullColl;
	Collection<Integer> emptyColl;
	// collection of one s.t. the set of one contains the element
	Collection<Integer> oneCollInOneSet;
	Collection<Integer> oneCollNotInOneSet;
	Collection<Integer> oneCollInManySet;
	Collection<Integer> oneCollNotInManySet;
	Collection<Integer> manyCollAllInManySet;
	Collection<Integer> manyCollPartInManySet;
	Collection<Integer> manyCollAllNotInManySet;

	/**
	 * Setup the attributes for testing.
	 */
	@Before
	public void setup() {
		/// sets
		emptySet = new BinarySearchTree<Integer>();

		oneSet = new BinarySearchTree<Integer>();
		oneSet.add(1);

		manySet = new BinarySearchTree<Integer>();
		manySet.add(1);
		manySet.add(2);
		manySet.add(3);
		manySet.add(4);
		manySet.add(5);
		manySet.add(6);
		manySet.add(7);
		manySet.add(8);
		manySet.add(9);
		manySet.add(10);

		/// collections
		nullColl = null;
		emptyColl = new ArrayList<Integer>();

		oneCollInOneSet = new ArrayList<Integer>();
		oneCollInOneSet.add(1);
		oneCollNotInOneSet = new ArrayList<Integer>();
		oneCollNotInOneSet.add(3);

		oneCollInManySet = new ArrayList<Integer>();
		oneCollInManySet.add(5);
		oneCollNotInManySet = new ArrayList<Integer>();
		oneCollNotInManySet.add(32);

		manyCollAllInManySet = new ArrayList<Integer>();
		manyCollAllInManySet.add(1);
		manyCollAllInManySet.add(2);
		manyCollAllInManySet.add(3);
		manyCollAllInManySet.add(4);
		manyCollPartInManySet = new ArrayList<Integer>();
		manyCollPartInManySet.add(8);
		manyCollPartInManySet.add(9);
		manyCollPartInManySet.add(10);
		manyCollPartInManySet.add(11);
		manyCollPartInManySet.add(12);
		manyCollPartInManySet.add(13);
		manyCollAllNotInManySet = new ArrayList<Integer>();
		manyCollAllNotInManySet.add(16);
		manyCollAllNotInManySet.add(17);
		manyCollAllNotInManySet.add(18);
		manyCollAllNotInManySet.add(19);
		manyCollAllNotInManySet.add(20);
	}

	/**
	 * Test the "first" method of the BinarySearchTree class.
	 * 
	 * Test that the value returned is the lowest element for a known dataset. Test
	 * the above test on a dataset of one, and a dataset of many. Test that an empty
	 * set throws NoSuchElementException.
	 */
	@Test
	public void testFirst() {
		Integer res, expRes;

		// empty set
		try {
			emptySet.first();
			fail("");
		} catch (NoSuchElementException e) {
		}

		// one set
		expRes = 1;
		res = oneSet.first();
		assertTrue(expRes.equals(res));

		// many set
		expRes = 1;
		res = manySet.first();
		assertTrue(expRes.equals(res));
	}

	/**
	 * Test the "last" method of the BinarySearchTree class.
	 * 
	 * Test that the value returned is the highest element for a known dataset. Test
	 * the above test on a dataset of one, and a dataset of many. Test that an empty
	 * set throws NoSuchElementException.
	 */
	@Test
	public void testLast() {
		Integer res, expRes;

		// empty set
		try {
			emptySet.first();
			fail("");
		} catch (NoSuchElementException e) {
		}

		// one set
		expRes = 1;
		res = oneSet.first();
		assertTrue(expRes.equals(res));

		// many set
		expRes = 10;
		res = manySet.last();
		assertTrue(expRes.equals(res));
	}

	/**
	 * Test the "add" method of the BinarySearchTree class.
	 * 
	 * Test that the size of the tree is incremented after the call. Test that the
	 * element was not contained in the set before the call, and is contained in the
	 * set after the call. Test the above test for an empty set, set of one element,
	 * and a set of many elements.
	 */
	@Test
	public void testAdd() {
		Integer sizeBefore, elemAdded;

		// empty set
		sizeBefore = emptySet.size();
		elemAdded = 3;
		assertTrue(!emptySet.contains(elemAdded));
		emptySet.add(elemAdded);
		assertTrue(emptySet.contains(elemAdded));
		assertTrue(emptySet.size() == (sizeBefore + 1));

		// one set
		sizeBefore = oneSet.size();
		elemAdded = 5;
		assertTrue(!oneSet.contains(elemAdded));
		oneSet.add(elemAdded);
		assertTrue(oneSet.contains(elemAdded));
		assertTrue(oneSet.size() == (sizeBefore + 1));

		// many set
		sizeBefore = manySet.size();
		elemAdded = 23;
		assertTrue(!manySet.contains(elemAdded));
		manySet.add(elemAdded);
		assertTrue(manySet.contains(elemAdded));
		assertTrue(manySet.size() == (sizeBefore + 1));
	}

	/**
	 * Test the "addAll" method of the BinarySearchTree class.
	 * 
	 * Test adding null collection, should throw NPE. Test adding an empty
	 * collection. Test adding a collection of one that is contained in the set.
	 * Test adding a collection of one that is not contained in the set. Test adding
	 * a collection of many that is all contained in the set. Test adding a
	 * collection of many that is partially contained in the set. Test adding a
	 * collection of many that is all not contained in the set. For each of the
	 * above tests, test an empty set, a set of one, and a set of many.
	 */
	@Test
	public void testAddAll() {
		// empty set
		assertTrue(emptySet.size() == 0);
		emptySet.addAll(emptyColl);
		assertTrue(emptySet.size() == 0);
		emptySet.addAll(oneCollInOneSet);
		assertTrue(emptySet.size() == 1);
		emptySet.clear();
		emptySet.addAll(manyCollAllInManySet);
		assertTrue(emptySet.size() == 4);
		// null
		try {
			emptySet.addAll(nullColl);
			fail("Successfully added a null collection to a set.");
		} catch (NullPointerException e) {
		}

		// one set
		assertTrue(oneSet.size() == 1);
		oneSet.addAll(emptyColl);
		assertTrue(oneSet.size() == 1);
		oneSet.addAll(oneCollInOneSet);
		assertTrue(oneSet.size() == 1);
		oneSet.addAll(oneCollNotInOneSet);
		assertTrue(oneSet.size() == 2);
		// null
		try {
			oneSet.addAll(nullColl);
			fail("Successfully added a null collection to a set.");
		} catch (NullPointerException e) {
		}

		// many set
		// empty colls
		int sizeBefore = manySet.size();
		manySet.addAll(emptyColl);
		assertTrue(manySet.size() == sizeBefore);
		// one colls
		manySet.addAll(oneCollNotInManySet);
		assertTrue(manySet.size() == (sizeBefore + 1));
		manySet.removeAll(oneCollNotInManySet);
		// many colls
		manySet.addAll(manyCollAllInManySet);
		assertTrue(manySet.size() == sizeBefore);
		for (Integer elem : manyCollAllInManySet) {
			assertTrue(manySet.contains(elem));
		}
		manySet.addAll(manyCollAllNotInManySet);
		assertTrue(manySet.size() == (sizeBefore + manyCollAllNotInManySet.size()));
		for (Integer elem : manyCollAllNotInManySet) {
			assertTrue(manySet.contains(elem));
		}
		manySet.removeAll(manyCollAllNotInManySet);
		assertTrue(manySet.size() == sizeBefore);
		manySet.addAll(manyCollPartInManySet);
		assertTrue(manySet.size() != sizeBefore);
		for (Integer elem : manyCollPartInManySet) {
			assertTrue(manySet.contains(elem));
		}
		// null
		try {
			manySet.addAll(nullColl);
			fail("Successfully added a null collection to a set.");
		} catch (NullPointerException e) {
		}
	}

	/**
	 * Test the "clear" method of the BinarySearchTree class.
	 * 
	 * Test an empty set. Test a set of one. Test a set of many.
	 */
	@Test
	public void testClear() {
		// empty set
		assertTrue(emptySet.isEmpty());
		emptySet.clear();
		assertTrue(emptySet.isEmpty());

		// one set
		assertTrue(!oneSet.isEmpty());
		oneSet.clear();
		assertTrue(oneSet.isEmpty());

		// many set
		assertTrue(!manySet.isEmpty());
		manySet.clear();
		assertTrue(manySet.isEmpty());
	}

	/**
	 * Test the "contains" method of the BinarySearchTree class.
	 * 
	 * Test an empty set. Test a set of one with an element that is in it. Test a
	 * set of one with an element that is not in it. Test a set of many with an
	 * element that is in it. Test a set of many with an element that is not in it.
	 * Test null, should throw npe.
	 */
	@Test
	public void testContains() {
		// empty set
		assertTrue(!emptySet.contains(1));
		// test a collection, should return false since the tree does not contain
		// collections
		assertTrue(!emptySet.contains(manyCollAllInManySet));
		// null
		try {
			if (emptySet.contains(null)) {
				fail("Successfully contained null.");
			}
		} catch (NullPointerException e) {
		}

		// one set
		assertTrue(oneSet.contains(1));
		assertTrue(!oneSet.contains(3));
		assertTrue(!oneSet.contains(oneCollInOneSet));
		// null
		try {
			if (oneSet.contains(null)) {
				fail("Successfully contained null.");
			}
		} catch (NullPointerException e) {
		}

		// many set
		assertTrue(manySet.contains(3));
		assertTrue(!manySet.contains(53));
		assertTrue(!manySet.contains(manyCollAllInManySet));
		// null
		try {
			if (manySet.contains(null)) {
				fail("Successfully contained null.");
			}
		} catch (NullPointerException e) {
		}
		
		assertTrue(!manySet.contains("This is not an Integer!"));
	}

	/**
	 * Test the "containsAll" method of the BinarySearchTree class.
	 * 
	 * Test a null collection, should throw NPE. Test an empty collection. Test a
	 * collection of one that is contained in the set. Test a collection of one that
	 * is not contained in the set. Test a collection of many that is all contained
	 * in the set. Test a collection of many that is partially contained in the set.
	 * Test a collection of many that is all not contained in the set. For each of
	 * the above tests, test an empty set, a set of one, and a set of many.
	 */
	@Test
	public void testContainsAll() {
		/// empty set
		assertTrue(emptySet.containsAll(emptyColl));
		assertTrue(!emptySet.containsAll(oneCollInOneSet));
		assertTrue(!emptySet.containsAll(manyCollAllInManySet));
		// null
		try {
			emptySet.containsAll(nullColl);
			fail("Set contained null.");
		} catch (NullPointerException e) {
		}

		/// one set
		assertTrue(oneSet.containsAll(emptyColl));
		assertTrue(oneSet.containsAll(oneCollInOneSet));
		assertTrue(!oneSet.containsAll(oneCollNotInOneSet));
		assertTrue(!oneSet.containsAll(manyCollAllInManySet));
		// null
		try {
			oneSet.containsAll(nullColl);
			fail("Set contained null.");
		} catch (NullPointerException e) {
		}

		/// many set
		assertTrue(manySet.containsAll(emptyColl));
		assertTrue(manySet.containsAll(oneCollInOneSet));
		assertTrue(manySet.containsAll(manyCollAllInManySet));
		assertTrue(!manySet.containsAll(manyCollAllNotInManySet));
		assertTrue(!manySet.containsAll(manyCollPartInManySet));
		// null
		try {
			manySet.containsAll(nullColl);
			fail("Set contained null.");
		} catch (NullPointerException e) {
		}
	}

	/**
	 * Test the "isEmpty" method of the BinarySearchTree class.
	 * 
	 * Test an empty set. Test a set of one. Test a set of many.
	 */
	@Test
	public void testIsEmpty() {
		assertTrue(emptySet.isEmpty());
		assertTrue(!oneSet.isEmpty());
		assertTrue(!manySet.isEmpty());
	}

	/**
	 * Test the "remove" method of the BinarySearchTree class.
	 * 
	 * Test an empty set. Test a set of one with an element that is in it. Test a
	 * set of one with an element that is not in it. Test a set of many with an
	 * element that is in it. Test a set of many with an element that is not in it.
	 */
	@Test
	public void testRemove() {
		int sizeBefore;

		// empty set
		assertTrue(!emptySet.remove(1));

		// one set
		sizeBefore = oneSet.size();
		assertTrue(!oneSet.remove(5));
		assertTrue(oneSet.size() == sizeBefore);
		assertTrue(oneSet.remove(1));
		assertTrue(oneSet.isEmpty());

		// many set
		sizeBefore = manySet.size();
		assertTrue(!manySet.remove(52));
		assertTrue(manySet.size() == sizeBefore);
		assertTrue(manySet.remove(5));
		assertTrue(manySet.remove(3));
		assertTrue(manySet.remove(7));
		assertTrue(manySet.remove(10));
		assertTrue(manySet.remove(1));
		assertTrue(!manySet.contains(5));
		assertTrue(!manySet.contains(3));
		assertTrue(!manySet.contains(7));
		assertTrue(!manySet.contains(10));
		assertTrue(!manySet.contains(1));
		assertTrue(manySet.size() == (sizeBefore - 5));
		
		assertTrue(!manySet.remove("This is not an integer!"));
	}

	/**
	 * Test the "removeAll" method of the BinarySearchTree class.
	 * 
	 * Test an empty set with an empty collection. Test an empty set with a
	 * collection of one. Test an empty set with a collection of many. Test a set of
	 * one with an empty collection. Test a set of one with a collection of one that
	 * is in it. Test a set of one with a collection of one that is not in it. Test
	 * a set of one with a collection of many that is all not in it. Test a set of
	 * one with a collection of many that is partially in it. Test a set of many
	 * with an empty collection. Test a set of many with a collection of one that is
	 * in it. Test a set of many with a collection of one that is not in it. Test a
	 * set of many with a collection of many that is all not in it. Test a set of
	 * many with a collection of many that is partially in it. Test a set of many
	 * with a collectionof many that is all in it. Test null.
	 */
	@Test
	public void testRemoveAll() {
		int sizeBefore;

		/// empty set
		// empty coll
		sizeBefore = emptySet.size();
		emptySet.removeAll(emptyColl);
		assertTrue(emptySet.size() == sizeBefore);
		// one coll
		sizeBefore = emptySet.size();
		emptySet.removeAll(oneCollInOneSet);
		assertTrue(emptySet.size() == sizeBefore);
		// many coll
		sizeBefore = emptySet.size();
		emptySet.removeAll(manyCollAllInManySet);
		assertTrue(emptySet.size() == sizeBefore);
		// null
		sizeBefore = emptySet.size();
		emptySet.removeAll(null);
		assertTrue(emptySet.size() == sizeBefore);

		/// one set
		// empty coll
		sizeBefore = oneSet.size();
		oneSet.removeAll(emptyColl);
		assertTrue(oneSet.size() == sizeBefore);
		// one coll
		sizeBefore = oneSet.size();
		oneSet.removeAll(oneCollNotInOneSet);
		assertTrue(oneSet.size() == sizeBefore);
		oneSet.removeAll(oneCollInOneSet);
		assertTrue(oneSet.isEmpty());
		oneSet.addAll(oneCollInOneSet);
		// many coll
		sizeBefore = oneSet.size();
		oneSet.removeAll(manyCollAllNotInManySet);
		assertTrue(oneSet.size() == sizeBefore);
		oneSet.removeAll(manyCollAllInManySet);
		assertTrue(oneSet.isEmpty());
		// null
		assertTrue(!oneSet.removeAll(null));

		/// many set
		// empty coll
		sizeBefore = manySet.size();
		manySet.removeAll(emptyColl);
		assertTrue(manySet.size() == sizeBefore);
		// one coll
		sizeBefore = manySet.size();
		manySet.removeAll(oneCollInOneSet);
		assertTrue(manySet.size() == (sizeBefore - 1));
		manySet.addAll(oneCollInOneSet);
		// many coll
		sizeBefore = manySet.size();
		manySet.removeAll(manyCollAllNotInManySet);
		assertTrue(manySet.size() == sizeBefore);
		manySet.removeAll(manyCollAllInManySet);
		assertTrue(manySet.size() != sizeBefore);
		manySet.addAll(manyCollAllInManySet);
		sizeBefore = manySet.size();
		assertTrue(manySet.removeAll(manyCollPartInManySet));
		assertTrue(manySet.size() != sizeBefore);
		// null
		assertTrue(!manySet.remove(null));
	}

	/**
	 * Test the "size" method of the BinarySearchTree class.
	 * 
	 * Test an empty set. Test a set of one. Test a set of many.
	 */
	@Test
	public void testSize() {
		assertTrue(emptySet.size() == 0);
		assertTrue(oneSet.size() == 1);
		assertTrue(manySet.size() == 10);
	}

	/**
	 * Test the "toArray" method of the BinarySearchTree class.
	 * 
	 * Test an emtpy set. Test a set of one. Test a set of many.
	 */
	@Test
	public void testToArray() {
		Object[] expRes, res;

		// empty set
		expRes = new Object[] {};
		res = emptySet.toArray();
		assertTrue(Arrays.equals(expRes, res));

		// one set
		expRes = new Object[] { 1 };
		res = oneSet.toArray();
		assertTrue(Arrays.equals(expRes, res));

		// many set
		expRes = new Object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
		res = manySet.toArray();
		assertTrue(Arrays.equals(expRes, res));
	}
	
	/**
	 * Test the "generateDot" method of the BinarySearchTree class.
	 * 
	 * This 'test' only prints the results, it is up to the administrator to copy/paste
	 * this data in the a dot-graph generator to check if it works.
	 * 
	 * Test an empty set.
	 * Test a set of one.
	 * Test a set of many.
	 */
	@Test
	public void testGenerateDot() {
		String dot = "";

		dot = emptySet.generateDot();
		System.out.println(dot);
		
		dot = oneSet.generateDot();
		System.out.println(dot);
		
		dot = manySet.generateDot();
		System.out.println(dot);
	}
}
