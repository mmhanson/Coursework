package assignment06;

import java.util.NoSuchElementException;

/**
 * A stack backed by a singly-linked list.
 * 
 * @author Maxwell Hanson
 * @since Jul 17, 2018
 * @version 1.1.0
 *
 * @param <E>
 */
public class StackLinkedList<E> implements Stack<E> {
	/**
	 * Linked-list data structure that backs this class.
	 */
	private SinglyLinkedList<E> list;

	
	/**
	 * Create an empty StackLinkedList.
	 */
	StackLinkedList() {
		this.list = new SinglyLinkedList<E>();
	}
	
	/**
	 * Remove all of the elements from the stack.
	 */
	@Override
	public void clear() {
		list.clear();
	}

	/**
	 * Determine if the stack is empty.
	 * 
	 * @return -- true if the stack is empty, false otherwise
	 */
	@Override
	public boolean isEmpty() {
		return list.isEmpty();
	}

	/**
	 * Return, but does not remove, the element at the top of the stack.
	 * 
	 * @return -- the element at the top of the stack
	 * @throws NoSuchElementException -- if the stack is empty
	 */
	@Override
	public E peek() throws NoSuchElementException {
		return list.getFirst();
	}

	/**
	 * Return and removes the item at the top of the stack.
	 * 
	 * @return -- the element at the top of the stack
	 * @throws NoSuchElementException -- if the stack is empty
	 */
	@Override
	public E pop() throws NoSuchElementException {
		return list.removeFirst();
	}

	/**
	 * Add an element to the top of the stack.
	 * 
	 * @param element -- the element to be added
	 */
	@Override
	public void push(E element) {
		list.addFirst(element);
	}
	
	/**
	 * Determine the number of elements in the stack.
	 * 
	 * @return -- the number of elements in the stack
	 */
	@Override
	public int size() {
		return list.size();
	}
}
