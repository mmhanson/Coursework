package assignment07;

import java.util.Collection;
import java.util.NoSuchElementException;

/**
 * A set that provides a total ordering on its elements. 
 * 
 * @author Maxwell Hanson
 * @since Jul 17, 2018
 * @version 1.1.0
 * 
 * @param <E> -- the type of elements in the set
 */
public interface SortedSet<E extends Comparable<? super E>> {
	/**
	 * Calculate the first element in the set.
	 * 
	 * @return -- the first (lowest, smallest) element in this set
	 * @throws NoSuchElementException -- if the set is empty
	 */
	public E first() throws NoSuchElementException;

	/**
	 * Calculate the last element in the set.
	 * 
	 * @return -- the last (highest, largest) element in this set
	 * @throws NoSuchElementException -- if the set is empty
	 */
	public E last() throws NoSuchElementException;

	/**
	 * Add an element to the set.
	 * 
	 * @param element -- element to be added to this set
	 * @return -- true if the addition was successful, false otherwise
	 */
	public boolean add(E element);

	/**
	 * Add many elements to the set.
	 * 
	 * @param elements -- collection of elements to be added to this set
	 * @return -- true if this set changed as a result of the call
	 */
	public boolean addAll(Collection<? extends E> elements);

	/**
	 * Remove all elements from the set.
	 */
	public void clear();

	/**
	 * Determine if the set contains an element.
	 * 
	 * @param element -- element whose presence in this set is to be tested
	 * @return -- true if this set contains the element, false otherwise
	 */
	public boolean contains(Object element);

	/**
	 * Determine if the set contains many elements./
	 * 
	 * @param elements -- collection to be checked for containment in this set
	 * @return -- true iff the set contains all elements in the set, false otherwise
	 */
	public boolean containsAll(Collection<?> elements);

	/**
	 * Determine if the set is empty.
	 * 
	 * @return -- true if the set contains no elements, false otherwise
	 */
	public boolean isEmpty();

	/**
	 * Remove the specified element from this set if it is present.
	 * 
	 * @param element -- element to be removed from this set, if present
	 * @return -- true if the element was in the set and removed, false otherwise
	 */
	public boolean remove(Object element);

	/**
	 * Remove many elements from the set.
	 * 
	 * @param elements -- elements to be removed from the set
	 * @return -- true if the set changed after the call, false otherwise
	 */
	public boolean removeAll(Collection<?> elements);

	/**
	 * Determine the number of elements in the set.
	 * 
	 * @return -- the number of elements in this set
	 */
	public int size();

	/**
	 * Translate the set to an array.
	 * 
	 * @return -- an array containing all elements in the set, in order
	 */
	public Object[] toArray();
}