package assignment09;

import static org.junit.Assert.assertTrue;
import static org.junit.Assert.fail;

import java.util.ArrayList;
import java.util.Collection;

import org.junit.Before;
import org.junit.Test;

/**
 * Testing suite for the "ChainingHashTable" class.
 * 
 * @author Max Hanson
 * @since 3/27/18
 * @version 1.0.0
 */
public class ChainingHashTableTests {
	/**
	 * The initial capacity of all hash tables constructed.
	 */
	private int initCap;
	/**
	 * The hash function used for all hash tables constructed.
	 */
	private HashFunctor hasher;
	/**
	 * A set of no elements.
	 */
	private ChainingHashTable emptySet;
	/**
	 * A list of all the elements in the empty set.
	 */
	private ArrayList<String> emptySetElems;
	/**
	 * A set of one element.
	 */
	private ChainingHashTable oneSet;
	/**
	 * A list of all the elements in the one set.
	 */
	private ArrayList<String> oneSetElems;
	/**
	 * A set of many elements.
	 */
	private ChainingHashTable manySet;
	/**
	 * A list of all the elements in the many set.
	 */
	private ArrayList<String> manySetElems;
	/**
	 * A collection that is entirely not in any set.
	 */
	Collection<String> collNotIn;
	/**
	 * A collection such that each set contains at least one
	 *   element of it, and each set does not contain at least
	 *   one element of it.
	 */
	Collection<String> collSomeIn;
	/**
	 * A collection that is in every set (except empty set)
	 */
	Collection<String> collAllIn;
	
	/**
	 * Set up attributes for the testing methods.
	 */
	@Before
	public void setup() {
		// initialize and populate element lists
		emptySetElems = new ArrayList<String>();
		oneSetElems = new ArrayList<String>();
		oneSetElems.add("a");
		manySetElems = new ArrayList<String>();
		manySetElems.add("a");
		manySetElems.add("b");
		manySetElems.add("c");
		
		// hash table parameters
		initCap = 100;
		hasher = new GoodHashFunctor();
		
		// create hash tables
		emptySet = new ChainingHashTable(initCap, hasher);
		oneSet = new ChainingHashTable(initCap, hasher);
		for (String elem: oneSetElems) {
			oneSet.add(elem);
		}
		manySet = new ChainingHashTable(initCap, hasher);
		for (String elem: manySetElems) {
			manySet.add(elem);
		}
		
		// collections
		collNotIn = new ArrayList<String>();
		collNotIn.add("asdf");
		collNotIn.add("fda");
		collNotIn.add("fdsa");
		collNotIn.add("feck");
		collNotIn.add("qwerty");
		collSomeIn = new ArrayList<String>();
		collSomeIn.add("a");
		collSomeIn.add("b");
		collSomeIn.add("fda");
		collSomeIn.add("asdf");
		collSomeIn.add("dfdd");
		collAllIn = new ArrayList<String>();
		collAllIn.add("a");
	}
	
	/**
	 * Test the constructor.
	 * 
	 * Test that the set is initially empty.
	 * Test adding n^2 elements for a capacity of n
	 *   to ensure proper resizing occurs.
	 * Test a capacity of zero.
	 * Test a negative capacity.
	 * Test that parameterizing null throws NPE.
	 */
	@Test
	public void testConstructor() {
		ChainingHashTable newSet = new ChainingHashTable(initCap, hasher);
		
		assertTrue(newSet.isEmpty());
		
		for (int i = 1; i < Math.pow(initCap, 2); i++) {
			newSet.add(String.valueOf(i));
		}
		
		newSet = new ChainingHashTable(0, hasher);
		
		try {
			newSet = new ChainingHashTable(-5, hasher);
		}
		catch (NegativeArraySizeException e) {}
		
		try {
			ChainingHashTable cht = new ChainingHashTable(1, null);
			cht.add("abc");
			fail("");
		}
		catch (NullPointerException e) {}
	}
	
	/**
	 * Test the "add" method.
	 * 
	 * -- For an element not in the set --
	 * Test that adding an element is successful.
	 * Test that after adding an element, the element is
	 *   contained in the set.
	 * Test that after adding an element, the size is
	 *   incremented.
	 *   
	 * -- For an element in the set --
	 * Test that adding an element is not successful.
	 * Test that after adding an element, the element is
	 *   contained in the set.
	 * Test that after adding an element, the size is
	 *   unchanged.
	 * 
	 * Test all above tests for a set of none, one, and many.
	 */
	@Test
	public void testAdd() {
		String elemNotIn = "asdf";
		String elemIn = "a";
		int sizeBefore;
		
		// -- elem not in the set --
		// empty set
		sizeBefore = emptySet.size();
		assertTrue(!emptySet.contains(elemNotIn));
		assertTrue(emptySet.add(elemNotIn));
		assertTrue(emptySet.contains(elemNotIn));
		assertTrue(emptySet.size() == (sizeBefore + 1));
		// one set
		sizeBefore = oneSet.size();
		assertTrue(!oneSet.contains(elemNotIn));
		assertTrue(oneSet.add(elemNotIn));
		assertTrue(oneSet.contains(elemNotIn));
		assertTrue(oneSet.size() == (sizeBefore + 1));
		// many set
		sizeBefore = manySet.size();
		assertTrue(!manySet.contains(elemNotIn));
		assertTrue(manySet.add(elemNotIn));
		assertTrue(manySet.contains(elemNotIn));
		assertTrue(manySet.size() == (sizeBefore + 1));
		
		// -- elem in the set --
		// one set
		sizeBefore = oneSet.size();
		assertTrue(oneSet.contains(elemIn));
		assertTrue(!oneSet.add(elemIn));
		assertTrue(oneSet.contains(elemIn));
		assertTrue(sizeBefore == oneSet.size());
		// many set
		sizeBefore = manySet.size();
		assertTrue(manySet.contains(elemIn));
		assertTrue(!manySet.add(elemIn));
		assertTrue(manySet.contains(elemIn));
		assertTrue(sizeBefore == manySet.size());

	}
	
	/**
	 * Test the "addAll" method.
	 * 
	 * -- For a collection entirely not in the set --
	 * Test that the set contains all the elements in 
	 *   the collection.
	 * Test that adding the collection returns true.
	 * Test that adding the collection changes the size
	 *   the appropriate amount by the size of the coll.
	 * Test that adding the collection results in each
	 *   element being contained in the set.
	 *   
	 * -- For a collection partially in the set --
	 * Test that the set contains at least one element
	 *   in the collection.
	 * Test that adding the collection returns true.
	 * Test that adding the collection changes the size.
	 * Test that adding the collection results in each
	 *   element being contained in the set.
	 *   
	 * -- For a collection entirely in the set --
	 * Test that adding the collection returns false.
	 * Test that adding the collection does not change
	 *   the size of the set.
	 * Test that adding the collection results in each
	 *   element being contained in the set.
	 *   
	 * For all above tests, test a set of none, one, and many.
	 */
	@Test
	public void testAddAll() {
		int sizeBefore;
		boolean inFlag;
		
		// -- not in --
		// empty set
		sizeBefore = emptySet.size();
		for (String elem: collNotIn) {
			assertTrue(!emptySet.contains(elem));
		}
		assertTrue(emptySet.addAll(collNotIn));
		for (String elem: collNotIn) {
			assertTrue(emptySet.contains(elem));
		}
		assertTrue(emptySet.size() == (sizeBefore + collNotIn.size()));
		
		// one set
		sizeBefore = oneSet.size();
		for (String elem: collNotIn) {
			assertTrue(!oneSet.contains(elem));
		}
		assertTrue(oneSet.addAll(collNotIn));
		for (String elem: collNotIn) {
			assertTrue(oneSet.contains(elem));
		}
		assertTrue(oneSet.size() == (sizeBefore + collNotIn.size()));
		
		// many set
		sizeBefore = manySet.size();
		for (String elem: collNotIn) {
			assertTrue(!manySet.contains(elem));
		}
		assertTrue(manySet.addAll(collNotIn));
		for (String elem: collNotIn) {
			assertTrue(manySet.contains(elem));
		}
		assertTrue(manySet.size() == (sizeBefore + collNotIn.size()));
		
		// reset attributes
		setup();
		
		// -- some in --
		// one set
		sizeBefore = oneSet.size();
		// check at least one elem of coll in set
		inFlag = false;
		for (String elem: collSomeIn) {
			if (oneSet.contains(elem)) {
				inFlag = true;
			}
		}
		assertTrue(inFlag);
		assertTrue(oneSet.addAll(collSomeIn));
		assertTrue(oneSet.size() != sizeBefore);
		for (String elem: collSomeIn) {
			assertTrue(oneSet.contains(elem));
		}
		// many set
		sizeBefore = manySet.size();
		// check at least one elem of coll in set
		inFlag = false;
		for (String elem: collSomeIn) {
			if (manySet.contains(elem)) {
				inFlag = true;
			}
		}
		assertTrue(inFlag);
		assertTrue(manySet.addAll(collSomeIn));
		assertTrue(manySet.size() != sizeBefore);
		for (String elem: collSomeIn) {
			assertTrue(manySet.contains(elem));
		}
		
		// reset attributes
		setup();
		
		// -- all in --
		// one set
		sizeBefore = oneSet.size();
		for (String elem: collAllIn) {
			assertTrue(oneSet.contains(elem));
		}
		assertTrue(!oneSet.addAll(collAllIn));
		assertTrue(oneSet.size() == sizeBefore);
		for (String elem: collAllIn) {
			assertTrue(oneSet.contains(elem));
		}
		// many set
		sizeBefore = manySet.size();
		for (String elem: collAllIn) {
			assertTrue(manySet.contains(elem));
		}
		assertTrue(!manySet.addAll(collAllIn));
		assertTrue(manySet.size() == sizeBefore);
		for (String elem: collAllIn) {
			assertTrue(manySet.contains(elem));
		}
	}
	
	/**
	 * Test the "clear" method.
	 * 
	 * Test that after the call, the set is empty.
	 * Test that all elements contained in the set before
	 *   are no longer contained in the set after the call.
	 * For each of the above tests, test a set of none, one, and many.
	 */
	@Test
	public void testClear() {
		// -- empty set --
		assertTrue(emptySet.isEmpty());
		emptySet.clear();
		assertTrue(emptySet.isEmpty());
		
		// -- one set --
		assertTrue(!oneSet.isEmpty());
		oneSet.clear();
		assertTrue(oneSet.isEmpty());
		for (String elem: oneSetElems) {
			assertTrue(!oneSet.contains(elem));
		}
		
		// -- many set --
		assertTrue(!manySet.isEmpty());
		manySet.clear();
		assertTrue(manySet.isEmpty());
		for (String elem: manySetElems) {
			assertTrue(!manySet.contains(elem));
		}
	}
	
	/**
	 * Test the "contains" method.
	 * 
	 * Test that each element known to be in the set is
	 *   contained in the set.
	 * Test that an element known not to be in the set is
	 *   not contained in the set.
	 * For each of the above test, test a set of none, one, and many.
	 */
	@Test
	public void testContains() {
		String elemNotInEmpty = "a";
		String elemNotInOne = "b";
		String elemNotInMany = "asdf";
		
		// -- empty set --
		for (String elem: emptySetElems) {
			assertTrue(emptySet.contains(elem));
		}
		assertTrue(!emptySet.contains(elemNotInEmpty));
		
		// -- one set --
		for (String elem: oneSetElems) {
			assertTrue(oneSet.contains(elem));
		}
		assertTrue(!oneSet.contains(elemNotInOne));

		// -- many set --
		for (String elem: manySetElems) {
			assertTrue(manySet.contains(elem));
		}
		assertTrue(!manySet.contains(elemNotInMany));
	}
	
	/**
	 * Test the "containsAll" method.
	 * 
	 * -- For a collection entirely not in the set --
	 * Test that the call returns false.
	 * Test that each element in the collection is
	 *   not contained in the set.
	 *   
	 * -- For a collection partially in the set --
	 * Test that the call returns false.
	 * Test that there is at least one element in the collection
	 *   that is not in the set.
	 *   
	 * -- For a collection entirely in the set --
	 * Test that the call returns true.
	 * Test that each element in the collection is contained in
	 *   the set.
	 *   
	 * For all above tests, test a set of none, one, and many.
	 */
	@Test
	public void testContainsAll() {
		boolean inFlag = false;
		
		// -- not in --
		// empty set
		assertTrue(!emptySet.containsAll(collNotIn));
		// one set
		assertTrue(!oneSet.containsAll(collNotIn));
		// many set
		assertTrue(!manySet.containsAll(collNotIn));
		
		// -- part in --
		// empty set
		assertTrue(!emptySet.containsAll(collSomeIn));
		for (String elem: collSomeIn) {
			assertTrue(!emptySet.contains(elem));
		}
		// one set
		assertTrue(!oneSet.containsAll(collSomeIn));
		for (String elem: collSomeIn) {
			if (oneSet.contains(elem)) {
				inFlag = true;
			}
		}
		assertTrue(inFlag);
		// many set
		assertTrue(!manySet.containsAll(collSomeIn));
		for (String elem: collSomeIn) {
			if (manySet.contains(elem)) {
				inFlag = true;
			}
		}
		assertTrue(inFlag);
		
		// -- all in --
		// empty set
		assertTrue(!emptySet.containsAll(collAllIn));
		// one set
		assertTrue(oneSet.containsAll(collAllIn));
		// many set
		assertTrue(manySet.containsAll(collAllIn));
	}
	
	/**
	 * Test the "isEmpty" method.
	 * 
	 * Test that a set known to be empty is empty.
	 * Test that a set that is empty has a size of zero.
	 * Test that a set known to have one element is not empty.
	 * Test that a set known to have one element's size is not zero.
	 * Test that a set of many is not empty.
	 * Test that after adding one element to a set of none,
	 *   one, and many, the sets are all not empty.
	 */
	@Test
	public void testIsEmpty() {
		String dummyStr = "asdf";
		
		// empty set
		assertTrue(emptySet.isEmpty());
		assertTrue(emptySet.size() == 0);
		emptySet.add(dummyStr);
		assertTrue(!emptySet.isEmpty());
		
		// one set
		assertTrue(!oneSet.isEmpty());
		assertTrue(oneSet.size() != 0);
		oneSet.add(dummyStr);
		assertTrue(!oneSet.isEmpty());
		
		// many set
		assertTrue(!manySet.isEmpty());
		assertTrue(manySet.size() != 0);
		manySet.add(dummyStr);
		assertTrue(!manySet.isEmpty());
	}
	
	/**
	 * Test the "size" method.
	 * 
	 * Test that an empty set has a size of zero.
	 * Test that a set of one has a size of one.
	 * Test that a set of many has a size of i: i > 1.
	 */
	@Test
	public void testSize() {
		assertTrue(emptySet.size() == 0);
		
		assertTrue(oneSet.size() == 1);
		
		assertTrue(manySet.size() > 1);
	}
}
