package assignment06;

import java.util.NoSuchElementException;

/**
 * This interface specifies the general behavior of a LIFO stack of elements.
 * 
 * @author Maxwell Hanson
 * @since Feb 15, 2018
 * @version 1.0.0
 * 
 * @param <E> - the type of elements contained in the stack
 */
public interface Stack<E> {
	/**
	 * Remove all of the elements from the stack.
	 */
	public void clear();

	/**
	 * Determine if this stack is empty.
	 * 
	 * @return -- true if the stack is empty, false otherwise
	 */
	public boolean isEmpty();

	/**
	 * Return, but do not remove, the element at the top of the stack.
	 * 
	 * @return -- the element at the top of the stack
	 * @throws NoSuchElementException -- if the stack is empty
	 */
	public E peek() throws NoSuchElementException;

	/**
	 * Return, and removes, the item at the top of the stack.
	 * 
	 * @return -- the element at the top of the stack
	 * @throws NoSuchElementException -- if the stack is empty
	 */
	public E pop() throws NoSuchElementException; 

	/**
	 * Add an element to the top of the stack.
	 * 
	 * @param element -- the element to be added
	 */
	public void push(E element); 
	
	/**
	 * Determine the number of elements in the stack.
	 * 
	 * @return -- the number of elements in the stack
	 */
	public int size();
}