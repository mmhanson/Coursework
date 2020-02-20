package assignment03;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertTrue;
import static org.junit.Assert.fail;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Comparator;
import java.util.NoSuchElementException;

import org.junit.Before;
import org.junit.Test;

/**
 * Test the BinarySearchSet class of the same package.
 * 
 * Note that each test method in this class assumes that every other method works as prescribed.
 * 
 * @author Maxwell Hanson
 * @version 1.1.0
 * @date Jan 30, 2018
 */
public class BinarySearchSetTests {
	/// custom classes used to define attributes
	/**
	 * A basic, custom class to be used as an element in BinarySearchSet for testing.
	 * The class is a basic wrapper for the int primative type.
	 * 
	 * @author Maxwell
	 */
	protected class CustomElement implements Comparable<CustomElement> {
		private int value;
		
		/**
		 * Construct a new instance of CustomElement with a parameter.
		 * 
		 * @param value -- the value to give the element
		 */
		public CustomElement(int value) {
			this.value = value;
		}
		
		/**
		 * Return the value of the element.
		 */
		public int getValue() {
			return this.value;
		}
		
		/**
		 * Compare this to a foreign instance of the class.
		 * 
		 * Return a value less than zero if this is less than the parameter.
		 * Return zero if this is equal to the parameter.
		 * Return a value greater than zero if this is greater than the parameter.
		 */
		@Override
		public int compareTo(CustomElement rhs) {
			if (this.getValue() < rhs.getValue()) {
				return -1;
			}
			if (this.getValue() > rhs.getValue()) {
				return 1;
			}
			return 0;
		}
	}
	
	/// attributes
	/**
	 * A set of integers, sorted in ascending order, in which a custom comparator was provided.
	 */
	private BinarySearchSet<Integer> parameterizedAscendingIntegerSet;
	/**
	 * The comparator object provided as a parameter to the parameterized, ascending integer set.
	 */
	private Comparator<Integer> parameterizedAscendingIntegerComparator = (lhs, rhs) -> lhs.compareTo(rhs);
	/**
	 * A set of integers, sorted in descending order, in which a custom comparator was provided.
	 */
	private BinarySearchSet<Integer> parameterizedDescendingIntegerSet;
	/**
	 * The comparator object provided as a parameter to the parameterized, descending integer set.
	 */
	private Comparator<Integer> parameterizedDescendingIntegerComparator = (lhs, rhs) -> (-1 * lhs.compareTo(rhs));
	/**
	 * A set of strings, sorted in ascending order, in which a custom comparator was provided.
	 */
	private BinarySearchSet<String> parameterizedAscendingStringSet;
	/**
	 * The comparator used as a parameter to the parameterized, ascending, string set
	 */
	private Comparator<String> parameterizedAscendingStringComparator = (lhs, rhs) -> lhs.compareTo(rhs);
	/**
	 * A set of strings, sorted in descending order, in which a custom comparator was provided.
	 */
	private BinarySearchSet<String> parameterizedDescendingStringSet;
	/**
	 * The comparator used as a parameter to the parameterized, ascending, string set
	 */
	private Comparator<String> parameterizedDescendingStringComparator = (lhs, rhs) -> (-1 * lhs.compareTo(rhs));
	/**
	 * A set of custom elements parameterized to be sorted in ascending order.
	 */
	private BinarySearchSet<CustomElement> parameterizedAscendingCustomSet;
	/**
	 * The comparator used as a parameter to the parameterized, ascending, custom-element set
	 */
	private Comparator<CustomElement> parameterizedAscendingCustomComparator = (lhs, rhs) -> lhs.compareTo(rhs);
	/**
	 * A set of custom elements parameterized to be sorted in descending order.
	 */
	private BinarySearchSet<CustomElement> parameterizedDescendingCustomSet;
	/**
	 * The comparator used as a parameter to the parameterized, descending, custom-element set
	 */
	private Comparator<CustomElement> parameterizedDescendingCustomComparator = (lhs, rhs) -> (-1 * lhs.compareTo(rhs));
	/**
	 * A set of integers in which the initialization was not parameterized.
	 */
	private BinarySearchSet<Integer> defaultIntegerSet;
	/**
	 * A set of strings in which the initialization was not parameterized.
	 */
	private BinarySearchSet<String> defaultStringSet;
	/**
	 * A set of custom elements in which the initialization was not parameterized.
	 */
	private BinarySearchSet<CustomElement> defaultCustomSet;
	
	/**
	 * Set up the attributes for testing.
	 * 
	 * This method is executed once before EVERY test case.
	 */
	@Before
	public void setup() {
		/// parameterized instances
		parameterizedAscendingIntegerSet = new BinarySearchSet<Integer>(parameterizedAscendingIntegerComparator);
		parameterizedDescendingIntegerSet = new BinarySearchSet<Integer>(parameterizedDescendingIntegerComparator);
		parameterizedAscendingStringSet = new BinarySearchSet<String>(parameterizedAscendingStringComparator);
		parameterizedDescendingStringSet = new BinarySearchSet<String>(parameterizedDescendingStringComparator);
		parameterizedAscendingCustomSet = new BinarySearchSet<CustomElement>(parameterizedAscendingCustomComparator);
		parameterizedDescendingCustomSet = new BinarySearchSet<CustomElement>(parameterizedDescendingCustomComparator);
		
		/// unparameterized instances
		defaultIntegerSet = new BinarySearchSet<Integer>();
		defaultStringSet = new BinarySearchSet<String>();
		defaultCustomSet = new BinarySearchSet<CustomElement>();
	}
	
	/**
	 * Test the creation of new BinarySearchSet instances with the default constructor.
	 * 
	 * Test that the constructor returns a non-null object.
	 * Test that the constructor initially creates an empty set.
	 * Test that the default constructor orders types in ascending order.
	 * Test all above cases for instances of Integer, String, and CustomElement sets.
	 */
	@Test
	public void testDefaultConstructor() {
		/// tests use the freshly constructed instances created in the setup method
		// test that the constructor does not return null
		if (defaultIntegerSet == null || defaultStringSet == null || defaultCustomSet == null) {
			fail ("Default constructor returned null");
		}
		
		// test that new sets are empty
		if (!defaultIntegerSet.isEmpty() || !defaultStringSet.isEmpty() || !defaultCustomSet.isEmpty()) {
			fail ("Default constructor did not create an emtpy set");
		}
		
		// test that new sets are ordered in ascending order
		// populate each default list with two elements
		defaultIntegerSet.add(new Integer(1));
		defaultIntegerSet.add(new Integer(12));
		defaultStringSet.add(new String("a"));
		defaultStringSet.add(new String("b"));
		defaultCustomSet.add(new CustomElement(1));
		defaultCustomSet.add(new CustomElement(21));
		// test that the first is less than the last
		Integer firstInt = defaultIntegerSet.first();
		Integer lastInt = defaultIntegerSet.last();
		assertTrue(firstInt.compareTo(lastInt) < 0);
		String firstStr = defaultStringSet.first();
		String lastStr = defaultStringSet.last();
		assertTrue(firstStr.compareTo(lastStr) < 0);
		CustomElement firstCust = defaultCustomSet.first();
		CustomElement lastCust = defaultCustomSet.last();
		assertTrue(firstCust.compareTo(lastCust) < 0);
	}
	
	/**
	 * Test the creation of new BinarySearchSet instances with the comparator constructor.
	 * 
	 * Test that the constructor returns a non-null object.
	 * Test that the constructor initially creates an empty set.
	 * Test that a custom comparator orders integers in ascending order.
	 * Test that a custom comparator orders strings in ascending order.
	 * Test that a custom comparator orders a custom class in ascending order.
	 * Test that a custom comparator orders integers in reverse order.
	 * Test that a custom comparator orders strings in reverse order.
	 * Test that a custom comparator orders a custom class in reverse order.
	 */
	@Test
	public void testComparatorConstructor() {
		/// tests are conducted on freshly constructed instances created int he setup method
		// test that the instances are not null
		if (parameterizedAscendingIntegerSet == null || parameterizedDescendingIntegerSet == null
				|| parameterizedAscendingStringSet == null || parameterizedDescendingStringSet == null
				|| parameterizedAscendingCustomSet == null || parameterizedDescendingCustomSet == null) {
			fail ("The parameterized constructor returned an instance of null.");
		}
		
		// test that all instances are not empty
		if (!parameterizedAscendingIntegerSet.isEmpty() || !parameterizedDescendingIntegerSet.isEmpty()
				|| !parameterizedAscendingStringSet.isEmpty() || !parameterizedDescendingStringSet.isEmpty()
				|| !parameterizedAscendingCustomSet.isEmpty() || !parameterizedDescendingCustomSet.isEmpty()) {
			fail ("The parameterized constructor returned a non-empty set.");
		}
		
		// test that ascending sets order their elements in ascending order
		// populate each default list with two elements
		parameterizedAscendingIntegerSet.add(new Integer(1));
		parameterizedAscendingIntegerSet.add(new Integer(12));
		parameterizedAscendingStringSet.add(new String("a"));
		parameterizedAscendingStringSet.add(new String("b"));
		parameterizedAscendingCustomSet.add(new CustomElement(1));
		parameterizedAscendingCustomSet.add(new CustomElement(21));
		// test that the first is less than the last
		Integer firstIntA = parameterizedAscendingIntegerSet.first();
		Integer lastIntA = parameterizedAscendingIntegerSet.last();
		assertTrue(firstIntA.compareTo(lastIntA) < 0);
		String firstStrA = parameterizedAscendingStringSet.first();
		String lastStrA = parameterizedAscendingStringSet.last();
		assertTrue(firstStrA.compareTo(lastStrA) < 0);
		CustomElement firstCustA = parameterizedAscendingCustomSet.first();
		CustomElement lastCustA = parameterizedAscendingCustomSet.last();
		assertTrue(firstCustA.compareTo(lastCustA) < 0);
		
		// test that descending sets order their elements in descending order
		// populate each default list with two elements
		parameterizedDescendingIntegerSet.add(new Integer(1));
		parameterizedDescendingIntegerSet.add(new Integer(12));
		parameterizedDescendingStringSet.add(new String("a"));
		parameterizedDescendingStringSet.add(new String("b"));
		parameterizedDescendingCustomSet.add(new CustomElement(1));
		parameterizedDescendingCustomSet.add(new CustomElement(21));
		// test that the first is less than the last
		Integer firstIntD = parameterizedDescendingIntegerSet.first();
		Integer lastIntD = parameterizedDescendingIntegerSet.last();
		assertTrue(firstIntD.compareTo(lastIntD) > 0);
		String firstStrD = parameterizedDescendingStringSet.first();
		String lastStrD = parameterizedDescendingStringSet.last();
		assertTrue(firstStrD.compareTo(lastStrD) > 0);
		CustomElement firstCustD = parameterizedDescendingCustomSet.first();
		CustomElement lastCustD = parameterizedDescendingCustomSet.last();
		assertTrue(firstCustD.compareTo(lastCustD) > 0);
	}
	
	/**
	 * Test the getComparator method of the BinarySearchSet class.
	 * 
	 * Test that a default instance returns null.
	 * Test that a parameterized instance returns the correct comparator with multiple instances.
	 */
	@Test
	public void testGetComparator() {
		// test that all unparameterized instances return null
		if (defaultIntegerSet.getComparator() != null 
				|| defaultStringSet.getComparator() != null
				|| defaultCustomSet.getComparator() != null) {
			fail ("An unparameterized instance returned a non-null comparator");
		}
		
		// test that parameterized instances return their comparators
		assertTrue(parameterizedAscendingIntegerSet.getComparator() == parameterizedAscendingIntegerComparator);
		assertTrue(parameterizedDescendingIntegerSet.getComparator() == parameterizedDescendingIntegerComparator);
		assertTrue(parameterizedAscendingStringSet.getComparator() == parameterizedAscendingStringComparator);
		assertTrue(parameterizedDescendingStringSet.getComparator() == parameterizedDescendingStringComparator);
		assertTrue(parameterizedAscendingCustomSet.getComparator() == parameterizedAscendingCustomComparator);
		assertTrue(parameterizedDescendingCustomSet.getComparator() == parameterizedDescendingCustomComparator);
		
	}
	
	/**
	 * Test first method of the BinarySearchSet class.
	 * 
	 * Test that an empty set throws NoSuchElementException.
	 * Test that a set of one returns the sole element.
	 * Test that a set of many returns the first element.
	 * All three of the above tests are performed on a BinarySearchSet of Integers, Strings, and CustomElements.
	 * All three of the above tests are performed with unparameterized and parameterized BinarySearchSet instances.
	 */
	@Test
	public void testFirst() {
		/// unparameterized integer set
		// test that the (empty) set returns null
		try {
			defaultIntegerSet.first();
			fail ("Calling the 'first' method on an empty set did not throw an instance of NoSuchElementException.");
		}
		catch (NoSuchElementException e) {}
		defaultIntegerSet.add(new Integer(1));
		assertTrue(defaultIntegerSet.first().equals(new Integer(1)));
		defaultIntegerSet.add(new Integer(12));
		defaultIntegerSet.add(new Integer(51));
		defaultIntegerSet.add(new Integer(-5));
		assertTrue(defaultIntegerSet.first().equals(new Integer(-5)));
		
		/// parameterized integer set
		// test that the (empty) set returns null
		try {
			parameterizedAscendingIntegerSet.first();
			fail ("Calling the 'first' method on an empty set did not throw an instance of NoSuchElementException.");
		}
		catch (NoSuchElementException e) {}
		parameterizedAscendingIntegerSet.add(new Integer(1));
		assertTrue(parameterizedAscendingIntegerSet.first().equals(new Integer(1)));
		parameterizedAscendingIntegerSet.add(new Integer(12));
		parameterizedAscendingIntegerSet.add(new Integer(51));
		parameterizedAscendingIntegerSet.add(new Integer(-5));
		assertTrue(parameterizedAscendingIntegerSet.first().equals(new Integer(-5)));
		
		/// unparameterized string set
		// test that the (empty) set returns null
		try {
			defaultStringSet.first();
			fail ("Calling the 'first' method on an empty set did not throw an instance of NoSuchElementException.");
		}
		catch (NoSuchElementException e) {}
		defaultStringSet.add(new String("b"));
		assertTrue(defaultStringSet.first().equals(new String("b")));
		defaultStringSet.add(new String("a"));
		defaultStringSet.add(new String("c"));
		defaultStringSet.add(new String("d"));
		assertTrue(defaultStringSet.first().equals(new String("a")));
		
		/// parameterized string set
		// test that the (empty) set returns null
		try {
			parameterizedAscendingStringSet.first();
			fail ("Calling the 'first' method on an empty set did not throw an instance of NoSuchElementException.");
		}
		catch (NoSuchElementException e) {}
		parameterizedAscendingStringSet.add(new String("b"));
		assertTrue(parameterizedAscendingStringSet.first().equals(new String("b")));
		parameterizedAscendingStringSet.add(new String("a"));
		parameterizedAscendingStringSet.add(new String("c"));
		parameterizedAscendingStringSet.add(new String("d"));
		assertTrue(parameterizedAscendingStringSet.first().equals(new String("a")));
		
		/// unparameterized custom set
		// test that the (empty) set returns null
		// test that the (empty) set returns null
		try {
			defaultCustomSet.first();
			fail ("Calling the 'first' method on an empty set did not throw an instance of NoSuchElementException.");
		}
		catch (NoSuchElementException e) {}
		defaultCustomSet.add(new CustomElement(1));
		assertTrue(defaultCustomSet.first().getValue() == (new CustomElement(1).getValue()));
		defaultCustomSet.add(new CustomElement(2));
		defaultCustomSet.add(new CustomElement(3));
		defaultCustomSet.add(new CustomElement(-1));
		assertTrue(defaultCustomSet.first().getValue() == (new CustomElement(-1).getValue()));
		
		/// parameterized custom set
		// test that the (empty) set returns null
		try {
			parameterizedAscendingCustomSet.first();
			fail ("Calling the 'first' method on an empty set did not throw an instance of NoSuchElementException.");
		}
		catch (NoSuchElementException e) {}
		parameterizedAscendingCustomSet.add(new CustomElement(1));
		assertTrue(parameterizedAscendingCustomSet.first().getValue() == (new CustomElement(1).getValue()));
		parameterizedAscendingCustomSet.add(new CustomElement(2));
		parameterizedAscendingCustomSet.add(new CustomElement(3));
		parameterizedAscendingCustomSet.add(new CustomElement(-1));
		assertTrue(parameterizedAscendingCustomSet.first().getValue() == (new CustomElement(-1)).getValue());
	}
	
	/**
	 * Test the last method of the BinarySearchSet class.
	 * 
	 * Test that an empty set throws NoSuchElementException.
	 * Test that a set of one returns the sole element.
	 * Test that a set of many returns the last element.
	 * All three of the above tests are performed on a BinarySearchSet of Integers, Strings, and CustomElements.
	 * All three of the above tests are performed with unparameterized and parameterized BinarySearchSet instances.
	 */
	@Test
	public void testLast() {
		/// unparameterized integer set
		// test that the (empty) set returns null
		try {
			defaultIntegerSet.first();
			fail ("Calling the 'first' method on an empty set did not throw an instance of NoSuchElementException.");
		}
		catch (NoSuchElementException e) {}
		defaultIntegerSet.add(new Integer(1));
		assertTrue(defaultIntegerSet.last().equals(new Integer(1)));
		defaultIntegerSet.add(new Integer(12));
		defaultIntegerSet.add(new Integer(51));
		defaultIntegerSet.add(new Integer(-5));
		assertTrue(defaultIntegerSet.last().equals(new Integer(51)));
		
		/// parameterized integer set
		// test that the (empty) set returns null
		try {
			parameterizedAscendingIntegerSet.first();
			fail ("Calling the 'first' method on an empty set did not throw an instance of NoSuchElementException.");
		}
		catch (NoSuchElementException e) {}
		parameterizedAscendingIntegerSet.add(new Integer(1));
		assertTrue(parameterizedAscendingIntegerSet.last().equals(new Integer(1)));
		parameterizedAscendingIntegerSet.add(new Integer(12));
		parameterizedAscendingIntegerSet.add(new Integer(51));
		parameterizedAscendingIntegerSet.add(new Integer(-5));
		assertTrue(parameterizedAscendingIntegerSet.last().equals(new Integer(51)));
		
		/// unparameterized string set
		// test that the (empty) set returns null
		try {
			defaultStringSet.first();
			fail ("Calling the 'first' method on an empty set did not throw an instance of NoSuchElementException.");
		}
		catch (NoSuchElementException e) {}
		defaultStringSet.add(new String("b"));
		assertTrue(defaultStringSet.last().equals(new String("b")));
		defaultStringSet.add(new String("a"));
		defaultStringSet.add(new String("c"));
		defaultStringSet.add(new String("d"));
		assertTrue(defaultStringSet.last().equals(new String("d")));
		
		/// parameterized string set
		// test that the (empty) set returns null
		try {
			parameterizedAscendingStringSet.first();
			fail ("Calling the 'first' method on an empty set did not throw an instance of NoSuchElementException.");
		}
		catch (NoSuchElementException e) {}
		parameterizedAscendingStringSet.add(new String("b"));
		assertTrue(parameterizedAscendingStringSet.last().equals(new String("b")));
		parameterizedAscendingStringSet.add(new String("a"));
		parameterizedAscendingStringSet.add(new String("c"));
		parameterizedAscendingStringSet.add(new String("d"));
		assertTrue(parameterizedAscendingStringSet.last().equals(new String("d")));
		
		/// unparameterized custom set
		// test that the (empty) set returns null
		// test that the (empty) set returns null
		try {
			defaultCustomSet.first();
			fail ("Calling the 'first' method on an empty set did not throw an instance of NoSuchElementException.");
		}
		catch (NoSuchElementException e) {}
		defaultCustomSet.add(new CustomElement(1));
		assertTrue(defaultCustomSet.last().getValue() == (new CustomElement(1)).getValue());
		defaultCustomSet.add(new CustomElement(2));
		defaultCustomSet.add(new CustomElement(3));
		defaultCustomSet.add(new CustomElement(-1));
		assertTrue(defaultCustomSet.last().getValue() == (new CustomElement(3)).getValue());
		
		/// parameterized custom set
		// test that the (empty) set returns null
		try {
			parameterizedAscendingCustomSet.first();
			fail ("Calling the 'first' method on an empty set did not throw an instance of NoSuchElementException.");
		}
		catch (NoSuchElementException e) {}
		parameterizedAscendingCustomSet.add(new CustomElement(1));
		assertTrue(parameterizedAscendingCustomSet.last().getValue() == (new CustomElement(1)).getValue());
		parameterizedAscendingCustomSet.add(new CustomElement(2));
		parameterizedAscendingCustomSet.add(new CustomElement(3));
		parameterizedAscendingCustomSet.add(new CustomElement(-1));
		assertTrue(parameterizedAscendingCustomSet.last().getValue() == (new CustomElement(3)).getValue());
	}
	
	/**
	 * Test the add method of the BinarySearchSet class.
	 * 
	 * Test adding an element to an empty set.
	 * Test adding an element to a set of one (does not already contain element).
	 * Test adding an element to a set of one (contains element).
	 * Test adding an element to a set of many which does not already contain the element.
	 * Test adding an element to a set of many which already contains the element.
	 * Test adding a null object to a set of many.
	 * All four tests above are performed on unparameterized and parameterized instances of BinarySearchSets with Integers, Strings, and CustomElements.
	 */
	@Test
	public void testAdd() {
		/// unparameterized integer set
		// add an element to an empty set
		assertTrue(defaultIntegerSet.add(new Integer(1)));
		// add an element to a set of one that does not contain the element
		assertTrue(defaultIntegerSet.add(new Integer(2)));
		defaultIntegerSet.remove(new Integer(2));
		// add an element to a set of one that contains the element
		assertFalse(defaultIntegerSet.add(new Integer(1)));
		// add an element to a set of many that is not in the set
		defaultIntegerSet.add(new Integer(2));
		defaultIntegerSet.add(new Integer(3));
		defaultIntegerSet.add(new Integer(4));
		assertTrue(defaultIntegerSet.add(new Integer(5)));
		// add an element to a set of many that is in the set
		assertFalse(defaultIntegerSet.add(new Integer(3)));
		try {
			assertFalse(defaultIntegerSet.add(null));
			fail("");
		}
		catch (NullPointerException e) {}
		
		/// parameterized integer set
		// add an element to an empty set
		assertTrue(parameterizedAscendingIntegerSet.add(new Integer(1)));
		// add an element to a set of one that does not contain the element
		assertTrue(parameterizedAscendingIntegerSet.add(new Integer(2)));
		parameterizedAscendingIntegerSet.remove(new Integer(2));
		// add an element to a set of one that contains the element
		assertFalse(parameterizedAscendingIntegerSet.add(new Integer(1)));
		// add an element to a set of many that is not in the set
		parameterizedAscendingIntegerSet.add(new Integer(2));
		parameterizedAscendingIntegerSet.add(new Integer(3));
		parameterizedAscendingIntegerSet.add(new Integer(4));
		assertTrue(parameterizedAscendingIntegerSet.add(new Integer(5)));
		// add an element to a set of many that is in the set
		assertFalse(parameterizedAscendingIntegerSet.add(new Integer(3)));
		// add null to a set of size many
		try {
			parameterizedAscendingIntegerSet.add(null);
			fail("");
		}
		catch (NullPointerException e) {}
		
		/// unparameterized string set
		// add an element to an empty set
		assertTrue(defaultStringSet.add(new String("a")));
		// add an element to a set of one that does not contain the element
		assertTrue(defaultStringSet.add(new String("b")));
		defaultStringSet.remove(new String("b"));
		// add an element to a set of one that contains the element
		assertFalse(defaultStringSet.add(new String("a")));
		// add an element to a set of many that is not in the set
		defaultStringSet.add(new String("b"));
		defaultStringSet.add(new String("c"));
		defaultStringSet.add(new String("d"));
		assertTrue(defaultStringSet.add(new String("e")));
		// add an element to a set of many that is in the set
		assertFalse(defaultStringSet.add(new String("c")));
		// add null to a set of size many
		try {
			defaultStringSet.add(null);
			fail("");
		}
		catch (NullPointerException e) {}
		
		/// parameterized string set
		// add an element to an empty set
		assertTrue(parameterizedAscendingStringSet.add(new String("a")));
		// add an element to a set of one that does not contain the element
		assertTrue(parameterizedAscendingStringSet.add(new String("b")));
		parameterizedAscendingStringSet.remove(new String("b"));
		// add an element to a set of one that contains the element
		assertFalse(parameterizedAscendingStringSet.add(new String("a")));
		// add an element to a set of many that is not in the set
		parameterizedAscendingStringSet.add(new String("b"));
		parameterizedAscendingStringSet.add(new String("c"));
		parameterizedAscendingStringSet.add(new String("d"));
		assertTrue(parameterizedAscendingStringSet.add(new String("e")));
		// add an element to a set of many that is in the set
		assertFalse(parameterizedAscendingStringSet.add(new String("c")));
		// add null to a set of size many
		try {
			parameterizedAscendingStringSet.add(null);
			fail("");
		}
		catch (NullPointerException e) {}
		
		/// unparameterized custom set
		// add an element to an empty set
		assertTrue(defaultCustomSet.add(new CustomElement(1)));
		// add an element to a set of one that does not contain the element
		assertTrue(defaultCustomSet.add(new CustomElement(2)));
		defaultCustomSet.remove(new CustomElement(2));
		// add an element to a set of one that contains the element
		assertFalse(defaultCustomSet.add(new CustomElement(1)));
		// add an element to a set of many that is not in the set
		defaultCustomSet.add(new CustomElement(2));
		defaultCustomSet.add(new CustomElement(3));
		defaultCustomSet.add(new CustomElement(4));
		assertTrue(defaultCustomSet.add(new CustomElement(5)));
		// add an element to a set of many that is in the set
		assertFalse(defaultCustomSet.add(new CustomElement(3)));
		// add null to a set of size many
		try {
			defaultCustomSet.add(null);
			fail("");
		}
		catch (NullPointerException e) {}
		
		/// parameterized custom set
		// add an element to an empty set
		assertTrue(parameterizedAscendingCustomSet.add(new CustomElement(1)));
		// add an element to a set of one that does not contain the element
		assertTrue(parameterizedAscendingCustomSet.add(new CustomElement(2)));
		parameterizedAscendingCustomSet.remove(new CustomElement(2));
		// add an element to a set of one that contains the element
		assertFalse(parameterizedAscendingCustomSet.add(new CustomElement(1)));
		// add an element to a set of many that is not in the set
		parameterizedAscendingCustomSet.add(new CustomElement(2));
		parameterizedAscendingCustomSet.add(new CustomElement(3));
		parameterizedAscendingCustomSet.add(new CustomElement(4));
		assertTrue(parameterizedAscendingCustomSet.add(new CustomElement(5)));
		// add an element to a set of many that is in the set
		assertFalse(parameterizedAscendingCustomSet.add(new CustomElement(3)));
		// add null to a set of size many
		try {
			parameterizedAscendingCustomSet.add(null);
			fail("");
		}
		catch (NullPointerException e) {}
	}
	
	/**
	 * Test the addAll method of the BinarySearchSet class.
	 * 
	 * Test adding an empty collection to an empty set.
	 * Test adding a collection of one element to an empty set.
	 * Test adding a collection of many to an empty set.
	 * Test adding a collection of one to a set that contains it.
	 * Test adding a collection of many to a set that contains all of it.
	 * Test adding a collection of many to a set that contains some of it.
	 * Test adding a collection of many to a set that contains none of it.
	 * Test all above cases with unparameterized and parameterized instances of Integer, String, and CustomElement.
	 */
	@Test
	public void testAddAll() {
		/// unparameterized integer set
		// empty collection, empty set
		assertFalse(defaultIntegerSet.addAll(new ArrayList<Integer>()));
		// collection of one, empty set
		assertTrue(defaultIntegerSet.addAll(Arrays.asList(1)));
		defaultIntegerSet.remove(1);
		// collection of many, empty set
		assertTrue(defaultIntegerSet.addAll(Arrays.asList(1, 2, 3)));
		// collection of one, set that contains it
		assertFalse(defaultIntegerSet.addAll(Arrays.asList(2)));
		// collection of many, set that contains all of it
		assertFalse(defaultIntegerSet.addAll(Arrays.asList(1, 2, 3)));
		// collection of many, set that contains some of it
		assertTrue(defaultIntegerSet.addAll(Arrays.asList(1, 2, 6)));
		// collection of many, set that contains none of it
		assertTrue(defaultIntegerSet.addAll(Arrays.asList(7, 8, 9)));
		
		/// parameterized integer set
		// empty collection, empty set
		assertFalse(parameterizedAscendingIntegerSet.addAll(new ArrayList<Integer>()));
		// collection of one, empty set
		assertTrue(parameterizedAscendingIntegerSet.addAll(Arrays.asList(1)));
		parameterizedAscendingIntegerSet.remove(1);
		// collection of many, empty set
		assertTrue(parameterizedAscendingIntegerSet.addAll(Arrays.asList(1, 2, 3)));
		// collection of one, set that contains it
		assertFalse(parameterizedAscendingIntegerSet.addAll(Arrays.asList(2)));
		// collection of many, set that contains all of it
		assertFalse(parameterizedAscendingIntegerSet.addAll(Arrays.asList(1, 2, 3)));
		// collection of many, set that contains some of it
		assertTrue(parameterizedAscendingIntegerSet.addAll(Arrays.asList(1, 2, 6)));
		// collection of many, set that contains none of it
		assertTrue(parameterizedAscendingIntegerSet.addAll(Arrays.asList(7, 8, 9)));
		
		/// unparameterized string set
		// empty collection, empty set
		assertFalse(defaultStringSet.addAll(new ArrayList<String>()));
		// collection of one, empty set
		assertTrue(defaultStringSet.addAll(Arrays.asList("a")));
		defaultStringSet.remove("a");
		// collection of many, empty set
		assertTrue(defaultStringSet.addAll(Arrays.asList("a", "b", "c")));
		// collection of one, set that contains it
		assertFalse(defaultStringSet.addAll(Arrays.asList("b")));
		// collection of many, set that contains all of it
		assertFalse(defaultStringSet.addAll(Arrays.asList("a", "b", "c")));
		// collection of many, set that contains some of it
		assertTrue(defaultStringSet.addAll(Arrays.asList("a", "b", "f")));
		// collection of many, set that contains none of it
		assertTrue(defaultStringSet.addAll(Arrays.asList("g", "h", "i")));
		
		/// paramterized string set
		// empty collection, empty set
		assertFalse(parameterizedAscendingStringSet.addAll(new ArrayList<String>()));
		// collection of one, empty set
		assertTrue(parameterizedAscendingStringSet.addAll(Arrays.asList("a")));
		parameterizedAscendingStringSet.remove("a");
		// collection of many, empty set
		assertTrue(parameterizedAscendingStringSet.addAll(Arrays.asList("a", "b", "c")));
		// collection of one, set that contains it
		assertFalse(parameterizedAscendingStringSet.addAll(Arrays.asList("b")));
		// collection of many, set that contains all of it
		assertFalse(parameterizedAscendingStringSet.addAll(Arrays.asList("a", "b", "c")));
		// collection of many, set that contains some of it
		assertTrue(parameterizedAscendingStringSet.addAll(Arrays.asList("a", "b", "f")));
		// collection of many, set that contains none of it
		assertTrue(parameterizedAscendingStringSet.addAll(Arrays.asList("g", "h", "i")));
		
		/// unparameterized custom set
		CustomElement one = new CustomElement(1);
		CustomElement two = new CustomElement(2);
		CustomElement three = new CustomElement(3);
		CustomElement four = new CustomElement(4);
		CustomElement five = new CustomElement(5);
		CustomElement six = new CustomElement(6);
		CustomElement seven = new CustomElement(7);
		// empty collection, empty set
		assertFalse(defaultCustomSet.addAll(new ArrayList<CustomElement>()));
		// collection of one, empty set
		assertTrue(defaultCustomSet.addAll(Arrays.asList(one)));
		defaultCustomSet.remove(one);
		// collection of many, empty set
		assertTrue(defaultCustomSet.addAll(Arrays.asList(one, two, three)));
		// collection of one, set that contains it
		assertFalse(defaultCustomSet.addAll(Arrays.asList(one)));
		// collection of many, set that contains all of it
		assertFalse(defaultCustomSet.addAll(Arrays.asList(one, two, three)));
		// collection of many, set that contains some of it
		assertTrue(defaultCustomSet.addAll(Arrays.asList(one, two, four)));
		// collection of many, set that contains none of it
		assertTrue(defaultCustomSet.addAll(Arrays.asList(five, six, seven)));
		
		/// paramterized custom set
		// empty collection, empty set
		assertFalse(parameterizedAscendingCustomSet.addAll(new ArrayList<CustomElement>()));
		// collection of one, empty set
		assertTrue(parameterizedAscendingCustomSet.addAll(Arrays.asList(one)));
		parameterizedAscendingCustomSet.remove(one);
		// collection of many, empty set
		assertTrue(parameterizedAscendingCustomSet.addAll(Arrays.asList(one, two, three)));
		// collection of one, set that contains it
		assertFalse(parameterizedAscendingCustomSet.addAll(Arrays.asList(one)));
		// collection of many, set that contains all of it
		assertFalse(parameterizedAscendingCustomSet.addAll(Arrays.asList(one, two, three)));
		// collection of many, set that contains some of it
		assertTrue(parameterizedAscendingCustomSet.addAll(Arrays.asList(one, two, four)));
		// collection of many, set that contains none of it
		assertTrue(parameterizedAscendingCustomSet.addAll(Arrays.asList(five, six, seven)));
	}
	
	/**
	 * Test the clear method of the BinarySearchSet class.
	 * 
	 * Test clearing an empty set.
	 * Test clearing a set of one.
	 * Test clearing a set of many.
	 * Test all above cases with unparameterized and parameterized instances of Integer, String, and CustomElement.
	 */
	@Test
	public void testClear() {
		/// unparameterized integer set
		// clear an empty set
		defaultIntegerSet.clear();
		assertTrue(defaultIntegerSet.isEmpty());
		// clear a set of one
		defaultIntegerSet.add(new Integer(1));
		defaultIntegerSet.clear();
		assertTrue(defaultIntegerSet.isEmpty());
		// clear a set of many
		defaultIntegerSet.add(new Integer(1));
		defaultIntegerSet.add(new Integer(2));
		defaultIntegerSet.add(new Integer(3));
		defaultIntegerSet.clear();
		assertTrue(defaultIntegerSet.isEmpty());
		
		/// paramterized integer set
		// clear an empty set
		parameterizedAscendingIntegerSet.clear();
		assertTrue(parameterizedAscendingIntegerSet.isEmpty());
		// clear a set of one
		parameterizedAscendingIntegerSet.add(new Integer(1));
		parameterizedAscendingIntegerSet.clear();
		assertTrue(parameterizedAscendingIntegerSet.isEmpty());
		// clear a set of many
		parameterizedAscendingIntegerSet.add(new Integer(1));
		parameterizedAscendingIntegerSet.add(new Integer(2));
		parameterizedAscendingIntegerSet.add(new Integer(3));
		parameterizedAscendingIntegerSet.clear();
		assertTrue(parameterizedAscendingIntegerSet.isEmpty());
		
		/// unparameterized string set
		// clear an empty set
		defaultStringSet.clear();
		assertTrue(defaultStringSet.isEmpty());
		// clear a set of one
		defaultStringSet.add(new String("a"));
		defaultStringSet.clear();
		assertTrue(defaultStringSet.isEmpty());
		// clear a set of many
		defaultStringSet.add(new String("a"));
		defaultStringSet.add(new String("b"));
		defaultStringSet.add(new String("c"));
		defaultStringSet.clear();
		assertTrue(defaultStringSet.isEmpty());
		
		/// parameterized string set
		// clear an empty set
		parameterizedAscendingStringSet.clear();
		assertTrue(parameterizedAscendingStringSet.isEmpty());
		// clear a set of one
		parameterizedAscendingStringSet.add(new String("a"));
		parameterizedAscendingStringSet.clear();
		assertTrue(parameterizedAscendingStringSet.isEmpty());
		// clear a set of many
		parameterizedAscendingStringSet.add(new String("a"));
		parameterizedAscendingStringSet.add(new String("b"));
		parameterizedAscendingStringSet.add(new String("c"));
		parameterizedAscendingStringSet.clear();
		assertTrue(parameterizedAscendingStringSet.isEmpty());
		
		/// unparameterized custom set
		// clear an empty set
		defaultCustomSet.clear();
		assertTrue(defaultCustomSet.isEmpty());
		// clear a set of one
		defaultCustomSet.add(new CustomElement(1));
		defaultCustomSet.clear();
		assertTrue(defaultCustomSet.isEmpty());
		// clear a set of many
		defaultCustomSet.add(new CustomElement(1));
		defaultCustomSet.add(new CustomElement(2));
		defaultCustomSet.add(new CustomElement(3));
		defaultCustomSet.clear();
		assertTrue(defaultCustomSet.isEmpty());
		
		/// paramterized custom set
		// clear an empty set
		parameterizedAscendingCustomSet.clear();
		assertTrue(parameterizedAscendingCustomSet.isEmpty());
		// clear a set of one
		parameterizedAscendingCustomSet.add(new CustomElement(1));
		parameterizedAscendingCustomSet.clear();
		assertTrue(parameterizedAscendingCustomSet.isEmpty());
		// clear a set of many
		parameterizedAscendingCustomSet.add(new CustomElement(1));
		parameterizedAscendingCustomSet.add(new CustomElement(2));
		parameterizedAscendingCustomSet.add(new CustomElement(3));
		parameterizedAscendingCustomSet.clear();
		assertTrue(parameterizedAscendingCustomSet.isEmpty());
	}
	
	/**
	 * Test the contains method of the BinarySearchSet class.
	 * 
	 * Test an element against an empty set.
	 * Test an element against a set of one that contains it.
	 * Test an element against a set of one that does not contain it.
	 * Test an element against a set of many that contains it.
	 * Test an element against a set of many that does not contain it.
	 * Test that adding a mismatched type throws ClassCastException.
	 * Test all above cases with unparameterized and parameterized instances of Integer, String, and CustomElement.
	 */
	@Test
	public void testContains() {
		/// unparameterized integer set
		// empty set
		assertFalse(defaultIntegerSet.contains(new Integer(1)));
		// set of one that contains it
		defaultIntegerSet.add(new Integer(1));
		assertTrue(defaultIntegerSet.contains(new Integer(1)));
		// set of one that does not contain it
		assertFalse(defaultIntegerSet.contains(new Integer(2)));
		// set of many that contains it
		defaultIntegerSet.add(new Integer(2));
		defaultIntegerSet.add(new Integer(3));
		defaultIntegerSet.add(new Integer(4));
		assertTrue(defaultIntegerSet.contains(new Integer(3)));
		// set of many that does not contain it
		assertFalse(defaultIntegerSet.contains(new Integer(5)));
		// mismatched element type
		try {
			assertFalse(defaultIntegerSet.contains(new String("a")));
			fail ("Successfully added a mismatched type to an array.");
		}
		catch (ClassCastException e) {}
		
		/// parameterized integer set
		// empty set
		assertFalse(parameterizedAscendingIntegerSet.contains(new Integer(1)));
		// set of one that contains it
		parameterizedAscendingIntegerSet.add(new Integer(1));
		assertTrue(parameterizedAscendingIntegerSet.contains(new Integer(1)));
		// set of one that does not contain it
		assertFalse(parameterizedAscendingIntegerSet.contains(new Integer(2)));
		// set of many that contains it
		parameterizedAscendingIntegerSet.add(new Integer(2));
		parameterizedAscendingIntegerSet.add(new Integer(3));
		parameterizedAscendingIntegerSet.add(new Integer(4));
		assertTrue(parameterizedAscendingIntegerSet.contains(new Integer(3)));
		// set of many that does not contain it
		assertFalse(parameterizedAscendingIntegerSet.contains(new Integer(5)));
		// mismatched element type
		try {
			assertFalse(parameterizedAscendingIntegerSet.contains(new String("a")));
			fail ("Successfully added a mismatched type to an array.");
		}
		catch (ClassCastException e) {}
		
		/// unparameterized string set
		// empty set
		assertFalse(defaultStringSet.contains(new String("a")));
		// set of one that contains it
		defaultStringSet.add(new String("a"));
		assertTrue(defaultStringSet.contains(new String("a")));
		// set of one that does not contain it
		assertFalse(defaultStringSet.contains(new String("b")));
		// set of many that contains it
		defaultStringSet.add(new String("b"));
		defaultStringSet.add(new String("c"));
		defaultStringSet.add(new String("d"));
		assertTrue(defaultStringSet.contains(new String("c")));
		// set of many that does not contain it
		assertFalse(defaultStringSet.contains(new String("e")));
		// mismatched element type
		try {
			assertFalse(defaultStringSet.contains(new Integer(1)));
			fail ("Successfully added a mismatched type to an array.");
		}
		catch (ClassCastException e) {}
		
		/// parameterized string set
		// empty set
		assertFalse(parameterizedAscendingStringSet.contains(new String("a")));
		// set of one that contains it
		parameterizedAscendingStringSet.add(new String("a"));
		assertTrue(parameterizedAscendingStringSet.contains(new String("a")));
		// set of one that does not contain it
		assertFalse(parameterizedAscendingStringSet.contains(new String("b")));
		// set of many that contains it
		parameterizedAscendingStringSet.add(new String("b"));
		parameterizedAscendingStringSet.add(new String("c"));
		parameterizedAscendingStringSet.add(new String("d"));
		assertTrue(parameterizedAscendingStringSet.contains(new String("c")));
		// set of many that does not contain it
		assertFalse(parameterizedAscendingStringSet.contains(new String("e")));
		// mismatched element type
		try {
			assertFalse(parameterizedAscendingStringSet.contains(new Integer(1)));
			fail ("Successfully added a mismatched type to an array.");
		}
		catch (ClassCastException e) {}
		
		/// unparameterized custom set
		// empty set
		assertFalse(defaultCustomSet.contains(new CustomElement(1)));
		// set of one that contains it
		defaultCustomSet.add(new CustomElement(1));
		assertTrue(defaultCustomSet.contains(new CustomElement(1)));
		// set of one that does not contain it
		assertFalse(defaultCustomSet.contains(new CustomElement(2)));
		// set of many that contains it
		defaultCustomSet.add(new CustomElement(2));
		defaultCustomSet.add(new CustomElement(3));
		defaultCustomSet.add(new CustomElement(4));
		assertTrue(defaultCustomSet.contains(new CustomElement(3)));
		// set of many that does not contain it
		assertFalse(defaultCustomSet.contains(new CustomElement(5)));
		// mismatched element type
		try {
			assertFalse(defaultCustomSet.contains(new Integer(1)));
			fail ("Successfully added a mismatched type to an array.");
		}
		catch (ClassCastException e) {}
		
		/// parameterized custom set
		// empty set
		assertFalse(parameterizedAscendingCustomSet.contains(new CustomElement(1)));
		// set of one that contains it
		parameterizedAscendingCustomSet.add(new CustomElement(1));
		assertTrue(parameterizedAscendingCustomSet.contains(new CustomElement(1)));
		// set of one that does not contain it
		assertFalse(parameterizedAscendingCustomSet.contains(new CustomElement(2)));
		// set of many that contains it
		parameterizedAscendingCustomSet.add(new CustomElement(2));
		parameterizedAscendingCustomSet.add(new CustomElement(3));
		parameterizedAscendingCustomSet.add(new CustomElement(4));
		assertTrue(parameterizedAscendingCustomSet.contains(new CustomElement(3)));
		// set of many that does not contain it
		assertFalse(parameterizedAscendingCustomSet.contains(new CustomElement(5)));
		// mismatched element type
		try {
			assertFalse(parameterizedAscendingCustomSet.contains(new Integer(1)));
			fail ("Successfully added a mismatched type to an array.");
		}
		catch (ClassCastException e) {}
	}
	
	/**
	 * Test the containsAll method of the BinarySearchSet class.
	 * 
	 * Test an empty collection of elements against an empty set.
	 * Test a collection of one elements against an empty set.
	 * Test a collection of many elements against an empty set.
	 * Test an empty collection of elements against a set of one.
	 * Test a collection of one element against a set of one that contains it.
	 * Test a collection of one element against a set of one that does not contain it.
	 * Test a collection of many elements against a set of one that contains it.
	 * Test a collection of many elements against a set of one that does not contain it.
	 * Test an empty collection of elements against a set of many.
	 * Test a collection of one element against a set of many that contains it.
	 * Test a collection of one element against a set of many that does not contain it.
	 * Test a collection of many elements against a set of many that does not contain any of it.
	 * Test a collection of many elements against a set of many that contains some of it.
	 * Test a collection of many elements against a set of many that contains all of it.
	 * Test the null parameter.
	 * Test all above cases with unparameterized and parameterized instances of Integer, String, and CustomElement.
	 */
	@Test
	public void testContainsAll() {
		/// unparameterized integer set
		// empty collection, empty set
		assertFalse(defaultIntegerSet.containsAll(new ArrayList<Integer>()));
		// collection of one, empty set
		assertFalse(defaultIntegerSet.containsAll(Arrays.asList(1)));
		// collection of many, empty set
		assertFalse(defaultIntegerSet.containsAll(Arrays.asList(1, 2, 3)));
		// empty collection, set of one
		defaultIntegerSet.add(new Integer(1));
		assertFalse(defaultIntegerSet.containsAll(new ArrayList<Integer>()));
		// collection of one, set of one (contains it)
		assertTrue(defaultIntegerSet.containsAll(Arrays.asList(1)));
		// collection of one, set of one (not contains it)
		assertFalse(defaultIntegerSet.containsAll(Arrays.asList(5)));
		// collection of many, set of one (contains one of the elements in the collection)
		assertFalse(defaultIntegerSet.containsAll(Arrays.asList(1, 2, 3)));
		// collection of many, set of one (not contains any)
		assertFalse(defaultIntegerSet.containsAll(Arrays.asList(2, 3, 4)));
		// empty collection, set of many
		defaultIntegerSet.add(new Integer(2));
		defaultIntegerSet.add(new Integer(3));
		defaultIntegerSet.add(new Integer(4));
		assertFalse(defaultIntegerSet.containsAll(new ArrayList<Integer>()));
		// collection of one, set of many (contains it)
		assertTrue(defaultIntegerSet.containsAll(Arrays.asList(2)));
		// collection of one, set of many (not contains it)
		assertFalse(defaultIntegerSet.containsAll(Arrays.asList(5)));
		// collection of many, sets of many (not contains any of it)
		assertFalse(defaultIntegerSet.containsAll(Arrays.asList(5, 6, 7)));
		// collection of many, set of many (contains some)
		assertFalse(defaultIntegerSet.containsAll(Arrays.asList(4, 5, 6)));
		// collection of many, set of many (contains all)
		assertTrue(defaultIntegerSet.containsAll(Arrays.asList(2, 3, 4)));
		// test null
		assertFalse(defaultIntegerSet.containsAll(null));
	}
	
	/**
	 * Test the isEmpty method of the BinarySearchSet class.
	 * 
	 * Test that an empty set is empty.
	 * Test that a set of one is not empty.
	 * Test that a set of many is not empty.
	 * Test all above cases with unparameterized and parameterized instances of Integer, String, and CustomElement.
	 */
	@Test
	public void testIsEmpty() {
		/// unparameterized integer set
		// set of none is empty
		assertTrue(defaultIntegerSet.isEmpty());
		// set of one is not empty
		defaultIntegerSet.add(new Integer(1));
		assertFalse(defaultIntegerSet.isEmpty());
		// set of many is not empty
		defaultIntegerSet.add(new Integer(2));
		defaultIntegerSet.add(new Integer(3));
		defaultIntegerSet.add(new Integer(4));
		assertFalse(defaultIntegerSet.isEmpty());
	}
	
	/**
	 * Test the remove method of the BinarySearchSet class.
	 * 
	 * Test removing an element from an empty set.
	 * Test removing an element from a set of one that contains it.
	 * Test removing an element from a set of one that does not contain it.
	 * Test removing an element from a set of many that contains it.
	 * Test removing an element from a set of many that does not contain it.
	 * Test that removing a mismatched type throws ClassCastException
	 * Test null case.
	 * Test all above cases with unparameterized and parameterized instances of Integer, String, and CustomElement.
	 */
	@Test
	public void testRemove() {
		/// unparameterized integer set
		// remove from set of none
		assertFalse(defaultIntegerSet.remove(new Integer(12)));
		// remove from a set of one (contains)
		defaultIntegerSet.add(new Integer(1));
		assertTrue(defaultIntegerSet.remove(new Integer(1)));
		// remove from a set of one (not contains)
		defaultIntegerSet.add(new Integer(1));
		assertFalse(defaultIntegerSet.remove(new Integer(2)));
		// remove from a set of many (contains)
		defaultIntegerSet.add(new Integer(2));
		defaultIntegerSet.add(new Integer(3));
		defaultIntegerSet.add(new Integer(4));
		assertTrue(defaultIntegerSet.remove(new Integer(3)));
		// remove from a set of many (not contains)
		assertFalse(defaultIntegerSet.remove(new Integer(10)));
		// type mismatch case
		try {
			assertFalse(defaultIntegerSet.remove(new String("aaa")));
			fail ("Successfully removed a mismatched type from a set.");
		}
		catch (ClassCastException e) {}
		// null case
		try {
			defaultIntegerSet.remove(null);
			fail("");
		}
		catch (NullPointerException e) {}
	}
	
	/**
	 * Test the removeAll method of the BinarySearchSet class.
	 * 
	 * Test removing an empty collection from an empty set.
	 * Test removing an empty collection from a set of one.
	 * Test removing an empty collection from a set of many.
	 * Test removing a collection of one from an empty set.
	 * Test removing a collection of one from a set of one that contains it.
	 * Test removing a collection of one from a set of one that does not contain it.
	 * Test removing a collection of one from a set of many that contains it.
	 * Test removing a collection of one from a set of many that does not contain it.
	 * Test removing a collection of many from an empty set.
	 * Test removing a collection of many from a set of one that contains one of the elements.
	 * Test removing a collection of many from a set of one that contains none of the elements.
	 * Test removing a collection of many from a set of many that contains none of it.
	 * Test removing a collection of many from a set of many that contains some of it.
	 * Test removing a collection of many from a set of many that contains all of it.
	 * Test null case.
	 * Test all above cases with unparameterized and parameterized instances of Integer, String, and CustomElement.
	 */
	@Test
	public void testRemoveAll() {
		/// unparameterized integer set
		// empty collection, empty set
		assertFalse(defaultIntegerSet.removeAll(new ArrayList<Integer>()));
		// empty collection, set of one
		defaultIntegerSet.add(new Integer(1));
		assertFalse(defaultIntegerSet.removeAll(new ArrayList<Integer>()));
		// empty collection, set of many
		defaultIntegerSet.add(new Integer(2));
		defaultIntegerSet.add(new Integer(3));
		defaultIntegerSet.add(new Integer(4));
		assertFalse(defaultIntegerSet.removeAll(new ArrayList<Integer>()));
		// collection of one, set of none
		defaultIntegerSet.clear();
		assertFalse(defaultIntegerSet.removeAll(Arrays.asList(1)));
		// collection of one, set of one (contains)
		defaultIntegerSet.add(new Integer(1));
		assertTrue(defaultIntegerSet.removeAll(Arrays.asList(1)));
		// collection of one, set of one (not contains)
		defaultIntegerSet.add(new Integer(1));
		assertFalse(defaultIntegerSet.removeAll(Arrays.asList(2)));
		// collection of one, set of many (contains)
		defaultIntegerSet.add(new Integer(2));
		defaultIntegerSet.add(new Integer(3));
		defaultIntegerSet.add(new Integer(4));
		assertTrue(defaultIntegerSet.removeAll(Arrays.asList(3)));
		// collection of one, set of many (not contains)
		assertFalse(defaultIntegerSet.removeAll(Arrays.asList(5)));
		// collection of many, set of none
		defaultIntegerSet.clear();
		assertFalse(defaultIntegerSet.removeAll(Arrays.asList(1, 2, 3)));
		// collection of many, set of one (contains one)
		defaultIntegerSet.add(new Integer(1));
		assertTrue(defaultIntegerSet.removeAll(Arrays.asList(1, 2, 3)));
		// collection of many, set of one (contains none)
		defaultIntegerSet.add(new Integer(1));
		assertFalse(defaultIntegerSet.removeAll(Arrays.asList(2, 3, 4)));
		// collection of many, set of many (contains none)
		defaultIntegerSet.add(new Integer(2));
		defaultIntegerSet.add(new Integer(3));
		defaultIntegerSet.add(new Integer(4));
		assertFalse(defaultIntegerSet.removeAll(Arrays.asList(5, 6, 7)));
		// collection of many, set of many (contains some)
		assertTrue(defaultIntegerSet.removeAll(Arrays.asList(3, 4, 5)));
		// collection of many, set of many (contains all)
		defaultIntegerSet.add(new Integer(3));
		defaultIntegerSet.add(new Integer(4));
		assertTrue(defaultIntegerSet.removeAll(Arrays.asList(1, 2, 3, 4)));
		// null
		assertFalse(defaultIntegerSet.removeAll(null));
	}
	
	/**
	 * Test the size method of the BinarySearchSet class.
	 * 
	 * Test the size of an empty set.
	 * Test the size of a set of one.
	 * Test the size of a set of many.
	 * Test all above cases with unparameterized and parameterized instances of Integer, String, and CustomElement.
	 */
	@Test
	public void testSize() {
		/// unparameterized integer set
		// set of zero
		assertEquals(defaultIntegerSet.size(), 0);
		// set of one
		defaultIntegerSet.add(new Integer(1));
		assertEquals(defaultIntegerSet.size(), 1);
		// set of many
		defaultIntegerSet.add(new Integer(2));
		defaultIntegerSet.add(new Integer(3));
		defaultIntegerSet.add(new Integer(4));
		assertEquals(defaultIntegerSet.size(), 4);
	}
	
	/**
	 * Test the toArray method of the BinarySearchSet class.
	 * 
	 * Test converting an empty set to an array.
	 * Test converting a set of one to an array.
	 * Test converting a set of many to an array.
	 * Test all above cases with unparameterized and parameterized instances of Integer, String, and CustomElement.
	 */
	@Test
	public void testToArray() {
		/// unparameterized integer set
		// set of none
		assertTrue(Arrays.equals(defaultIntegerSet.toArray(), new Object[0]));
		// set of one
		defaultIntegerSet.add(new Integer(1));
		assertTrue(Arrays.equals(defaultIntegerSet.toArray(), new Object[] {1}));
		// set of many
		defaultIntegerSet.add(new Integer(2));
		defaultIntegerSet.add(new Integer(3));
		defaultIntegerSet.add(new Integer(4));
		assertTrue(Arrays.equals(defaultIntegerSet.toArray(), new Object[] {1, 2, 3, 4}));
	}
}
