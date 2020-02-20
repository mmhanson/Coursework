package assignment03;

import java.util.Arrays;
import java.util.Collection;
import java.util.Comparator;
import java.util.NoSuchElementException;

/**
 * A generic, ordered set that does not allow duplicates and provides efficient searching.
 * 
 * Elements are ordered using the default comparator or a custom one.
 * All elements must implement the "Comparable" interface or be accepted by
 * the specified comparator.
 * 
 * This class is unchecked. If a non-comparable element type is parameterized,
 * then a ClassCastException will be thrown when an element is added to a set of one.
 * 
 * @author Maxwell Hanson
 * @version 1.1.0
 * @param <E> -- the type of element managed by this set
 */
public class BinarySearchSet<E> implements SortedSet<E> {
	/**
	 * the array to store the objects in
	 */
	private Object[] array;
	
	/**
	 * the initial size of the array following construction
	 */
	private static final int INITIAL_ARRAY_SIZE = 100;
	
	/**
	 * the amount of elements added to the array when it fills
	 */
	private static final int ARRAY_INCRIMENT_INTERVAL = 100;
	
	/**
	 * the comparator to sort the array. If no comparator is specified at
	 * construction, then this value is null.
	 */
	private Comparator<? super E> comp;
	
	/**
	 * the current number of elements in the set
	 */
	private int size;
	
	/**
	 * Construct an empty BinarySearchSet instance without custom ordering.
	 * 
	 * This constructor uses the "compareTo" method of the elements to order them.
	 */
	public BinarySearchSet() {
		array = new Object[INITIAL_ARRAY_SIZE];
		size = 0;
		comp = null;
	}
	
	/**
	 * Construct an empty BinarySearchSet with custom ordering.
	 * 
	 * @param comparator -- the definition of the custom ordering
	 */
	public BinarySearchSet(Comparator<? super E> comparator) {
		this();
		comp = comparator;
	}
	
	/**
	 * Retrieve the comparator used to order the set's elements.
	 * 
	 * @return -- the comparator of the set, or null if using default ordering
	 */
	public Comparator<? super E> getComparator() {
		return this.comp;
	}
	
	/**
	 * Retrieve the first element in the set.
	 * 
	 * @return -- firsts element in the set
	 * @throws -- NoSuchElementException if the set is empty
	 */
	@SuppressWarnings("unchecked")
	public E first() throws NoSuchElementException {
		if (size == 0) {
			throw new NoSuchElementException();
		}
		
		return (E)array[0];
	}
	
	/**
	 * Retrieve the last element in the set.
	 * 
	 * @return -- the last element of the set
	 * @throws NoSuchElementException -- if the set is empty
	 */
	@SuppressWarnings("unchecked")
	public E last() throws NoSuchElementException {
		if (size == 0) {
			throw new NoSuchElementException();
		}
		
		return (E)array[size - 1];
	}
	
	/**
	 * Add an element to the set.
	 * 
	 * If the element is already in the set, then it is not added.
	 * 
	 * @param element -- element to be added to the set
	 * @return -- true if the element was added, false otherwise
	 */
	public boolean add(E element) {
		int index = binarySearch(element);
		
		// do not add duplicates
		if (index >= 0) {
			return false;
		}
		
		// make room
		index = (index * -1) - 1;
		shiftArray(index);
		
		// add element
		array[index] = element;
		size++;
		
		return true;
	}
	
	/**
	 * Add a collection of elements to the set.
	 * 
	 * If any elements are already in the set, they are not added.
	 * 
	 * @param elements -- the collection of elements to add to the set
	 * @return -- true if at least one element was added to the set, false otherwise
	 */
	@SuppressWarnings("unchecked")
	public boolean addAll(Collection<? extends E> elements) {
		// array is compared to before to see if modified
		Object[] arrayCopy = array.clone();
		
		for (Object element: elements) {
			add((E)element);
		}
		
		if (Arrays.equals(array, arrayCopy)) {
			return false;
		}
		else {
			return true;
		}
	}
	
	/**
	 * Remove all the elements from the set.
	 * 
	 * The set will be empty after this call.
	 */
	public void clear() {
		this.array = new Object[INITIAL_ARRAY_SIZE];
		this.size = 0;
	}
	
	/**
	 * Test that an element is in the set.
	 * 
	 * @param element -- the element to test the set against
	 * @return -- true if the set contains the specified element
	 */
	@SuppressWarnings("unchecked")
	public boolean contains(Object element) {
		int index = binarySearch((E)element);
		if (index >= 0) {
			return true;
		}
		else {
			return false;
		}
	}
	
	/**
	 * Test that the set contains a collection of elements.
	 * 
	 * @param elements -- the collection of elements to test the set against
	 * @return -- true if the set contains all of the elements in the collection, false otherwise
	 */
	public boolean containsAll(Collection<?> elements) {
		// if the collection is empty or null, then the set cannot contain any of it
		if (elements == null || elements.isEmpty()) {
			return false;
		}
		
		for (Object element: elements) {
			if (!contains(element)) {
				return false;
			}
		}
		return true;
	}
	
	/**
	 * Test that the set is empty.
	 * 
	 * @return -- true if the set contains no elements
	 */
	public boolean isEmpty() {
		return (size == 0);
	}
	
	/**
	 * Remove an element from the set.
	 * 
	 * If the element is not in the set, nothing is done.
	 * 
	 * @param element -- the element to be removed from the set, if present
	 * @return -- true if the element was removed, false otherwise
	 */
	@SuppressWarnings("unchecked")
	public boolean remove(Object element) {
		int index = binarySearch((E)element);
		
		// if not in array
		if (index < 0) {
			return false;
		}
		
		// move all elements above the target down one index, overwriting the target in the process
		array[index] = null;
		for (int curs = index + 1; curs < size; curs++) {
			array[curs - 1] = array[curs];
		}
		size--;
		
		return true;
	}
	
	/**
	 * Remove a collection of elements from the set.
	 * 
	 * @param elements -- the collection of elements to be removed from the set
	 * @return -- true if any element was removed from the set, false otherwise
	 */
	public boolean removeAll(Collection<?> elements) {
		// if elements is null, they cannot be removed
		if (elements == null) {
			return false;
		}
		
		// array is compared to before to see if modified
		Object[] arrayCopy = this.array.clone();
		
		for (Object element: elements) {
			this.remove(element);
		}
		
		if (Arrays.equals(array, arrayCopy)) {
			return false;
		}
		else {
			return true;
		}
	}
	
	/**
	 * Calculate the size of the set
	 * 
	 * @return -- the count of elements in the set
	 */
	public int size() {
		return this.size;
	}
	
	/**
	 * Convert the set to an array.
	 * 
	 * @return -- an array containing all the elements in the set, sorted in ascending order.
	 */
	public Object[] toArray() {
		Object[] newArray = new Object[size];
		
		for (int curs = 0; curs < size; curs++) {
			newArray[curs] = array[curs];
		}
		
		return newArray;
	}
	
	/**
	 * Find an element with a binary search.
	 * 
	 * If the element is found, returns the index of the element.
	 * If the element is not found, returns the index that the element would have, should it be added.
	 * 
	 * @param element -- element to search against
	 * @return -- index of the element if it is contained in the array. Otherwise, (-(insertion point) - 1)
	 *   where insertion point is defined as the point at which the key would be inserted into the array: the
	 *   index of the first element greater than the key, or a.length if all elements in the array are less than
	 *   the specified key. Note that this guarantees that the return value will be >= 0 if and only if the key
	 *   is found.
	 */
	private int binarySearch(E element) {
		if (this.isEmpty()) {
			return -1;
		}
		else {
			return binarySearch(element, 0, this.size - 1);
		}
	}
	
	/**
	 * Find an element within a range with a binary search.
	 * 
	 * Overload of binarySearch(E element) for recursion.
	 * 
	 * @param element -- search target
	 * @param min -- minimum index to search (inclusive)
	 * @param max -- maximum index to search (inclusive)
	 * @return -- index of the element if it is contained in the array. Otherwise, (-(insertion point) - 1)
	 *   where insertion point is defined as the point at which the key would be inserted into the array: the
	 *   index of the first element greater than the key, or a.length if all elements in the array are less than
	 *   the specified key. Note that this guarantees that the return value will be >= 0 if and only if the key
	 *   is found.
	 */
	@SuppressWarnings("unchecked")
	private int binarySearch(E element, int min, int max) {		
		int mid = (min + max) / 2;
		E midVal = (E)array[mid];
		
		/// base cases
		// element is found
		if (compare(midVal, element) == 0) {
			return mid;
		}
		// element not in array
		if (max <= min) {
			int insrtPt;
			
			if (max < min) {
				insrtPt = min;
			}
			// max == min
			else {
				if (compare(element, midVal) < 0) {
					insrtPt = min;
				}
				else {
					insrtPt = ++min;
				}
			}
			
			return ( (insrtPt * -1) - 1 );
		}
		
		/// recursive cases
		if (compare(midVal, element) < 0) {
			return binarySearch(element, mid + 1, max);
		}
		else {
			return binarySearch(element, min, mid - 1);
		}
	}
	
	/**
	 * Compare two elements.
	 * 
	 * Automatically determines whether to use the comparator object or the comparable
	 * interface depending on the parameters given at construction.
	 * 
	 * Return zero if the elements are equals.
	 * Return a value less than zero if the first parameter is less than the second.
	 * Return a value greater than zero if the first parameter is greater than the second.
	 * 
	 * @return -- num > 0 if e1 > e1. 0 if e1 == e2. num < 0 if e1 < e2.
	 */
	@SuppressWarnings("unchecked")
	private int compare(E e1, E e2) {
		if (comp == null) {
			return ((Comparable<? super E>)e1).compareTo(e2);
		}
		else {
			return comp.compare(e1, e2);
		}
	}
	
	/**
	 * Shift all elements from a certain point forward.
	 * 
	 * @param index -- point in array to start shifting forward
	 */
	private void shiftArray(int index) {
		// increase size of array if run out of space
		if (size == array.length) {
			Object[] newArray = new Object[size + ARRAY_INCRIMENT_INTERVAL];
			for (int curs = 0; curs < size; curs++) {
				newArray[curs] = array[curs];
			}
			array = newArray;
		}
		
		// shit all elements from 'index' onward forward
		for (int curs = size - 1; curs >= index; curs--) {
			array[curs + 1] = array[curs];
		}
	}
}
