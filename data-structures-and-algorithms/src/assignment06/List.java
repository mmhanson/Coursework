package assignment06;

import java.util.Iterator;
import java.util.NoSuchElementException;

/**
 * A generic interface for an ordered list.
 * 
 * @author Maxwell Hanson
 * @since July 16, 2018
 * @version 1.1.0
 * 
 * @param <E> - the type of elements contained in the list
 */
public interface List<E> extends Iterable<E> {

	/**
	 * Insert an element at the beginning of the list.
	 * 
	 * @param element -- element to add
	 */
	void addFirst(E element);

	/**
	 * Insert an element at a specific index in the list.
	 * 
	 * @param index -- index element will assume after addition
	 * @param element -- the element to add
	 * @throws IndexOutOfBoundsException -- if (index < 0 || index > size())
	 */
	void add(int index, E element) throws IndexOutOfBoundsException;
	
	/**
	 * Retrieve the first element in the list.
	 * 
	 * @return -- element at the start of the list
	 * @throws NoSuchElementException -- if the list is empty
	 */
	E getFirst() throws NoSuchElementException;
	
	/**
	 * Get the element of a specific index in the list.
	 * 
	 * @param index -- the index of the element to retrieve
	 * @return -- the element at the position
	 * @throws IndexOutOfBoundsException -- if (index < 0 || index >= size())
	 */
	E get(int index) throws IndexOutOfBoundsException;
	
	/**
	 * Remove and returns the first element from the list.
	 * 
	 * @return -- the first element of the list
	 * @throws NoSuchElementException -- if the list is empty
	 */
	E removeFirst() throws NoSuchElementException;
	
	/**
	 * Remove and returns the element at a specific position in the list.
	 * 
	 * @param index -- the specified position
	 * @return -- the element at the position
	 * @throws IndexOutOfBoundsException -- if (index < 0 || index >= size())
	 */
	E remove(int index) throws IndexOutOfBoundsException;
	
	/**
	 * Determine the index of an element in the list.
	 * 
	 * @param element -- the element to search for
	 * @return -- the index of the first occurrence; -1 if the element is not found
	 */
	int indexOf(E element);
	
	/**
	 * Determine the count of elements in the list.
	 * 
	 * @return -- the number of elements in this list
	 */
	int size();
	
	/**
	 * Determine if the list is empty.
	 * 
	 * @return -- true if this collection contains no elements, false otherwise
	 */
	boolean isEmpty();
	
	/**
	 * Remove all elements from the list.
	 */
	void clear();
	
	/**
	 * Translate the list to an array.
	 * 
	 * @return an array containing all of the elements in this list, in order
	 */
	Object[] toArray();
	
	/**
	 * Construct an iterator for the list.
	 * 
	 * @return -- an iterator that iterates the list from start to finish
	 */
	public Iterator<E> iterator();
}
