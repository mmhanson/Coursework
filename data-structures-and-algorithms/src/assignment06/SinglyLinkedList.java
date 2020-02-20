package assignment06;

import java.util.Iterator;
import java.util.NoSuchElementException;

/**
 * A class implementing the singly-linked list data structure.
 * 
 * @author Maxwell Hanson
 * @since Feb 27, 2018
 * @version 1.1.0
 */
public class SinglyLinkedList<E> implements List<E> {
	/**
	 * The head of the linked list that backs this class.
	 */
	private Node<E> head;
	
	/**
	 * The number of elements in this SinglyLinkedList
	 */
	private int size;
	
	/**
	 * Construct an empty instance of a SinglyLinkedList.
	 */
	SinglyLinkedList() {
		this.head = null;
		this.size = 0;
	}
	
	/**
	 * Insert an element at the beginning of the list.
	 * 
	 * @param element -- the element to add
	 */
	public void addFirst(E element) {
		Node<E> newNode = new Node<E>(element);
		newNode.link(this.head);
		this.head = newNode;
		this.size++;
	}

	/**
	 * Insert an element at a specific position in the list.
	 * 
	 * @param index -- the index that the new element will assume after addition
	 * @param element -- the element to add
	 * @throws IndexOutOfBoundsException -- if index is out of range (index < 0 || index > size())
	 */
	public void add(int index, E element) throws IndexOutOfBoundsException {
		if (index < 0 || index > this.size()) {
			throw new IndexOutOfBoundsException();
		}
		
		Node<E> newNode = new Node<E>(element);
		// retrieve previous and next nodes while avoiding IOBE in edge cases
		Node<E> prevNode = (index == 0) ? null : this.getNode(index - 1);
		Node<E> nextNode = (index == this.size()) ? null : this.getNode(index);
		
		// relink nodes, inserting new element at index
		if (prevNode != null) {
			prevNode.link(newNode);
		}
		else {
			this.head = newNode;
		}
		if (nextNode != null) {
			newNode.link(nextNode);
		}
		
		this.size++;
	}
	
	/**
	 * Retrieve the first element in the list.
	 * 
	 * @return -- first element in the list
	 * @throws NoSuchElementException -- if the list is empty
	 */
	public E getFirst() throws NoSuchElementException {
		if (this.isEmpty()) {
			throw new NoSuchElementException();
		}
		
		return this.get(0);
	}
	
	/**
	 * Retrieve the element at a specific index in the list.
	 * 
	 * @param index -- the specified index
	 * @return -- the element at the position
	 * @throws IndexOutOfBoundsException -- if index is out of range (index < 0 || index >= size())
	 */
	public E get(int index) throws IndexOutOfBoundsException {
		if (index < 0 || index >= this.size()) {
			throw new IndexOutOfBoundsException();
		}
		
		return this.getNode(index).getData();
	}
	
	/**
	 * Remove and returns the first element from the list.
	 * 
	 * @return -- the first element of the list
	 * @throws NoSuchElementException -- if the list is empty
	 */
	public E removeFirst() throws NoSuchElementException {
		if (this.isEmpty()) {
			throw new NoSuchElementException();
		}
		
		// buffer is used to return value after removal
		E retVal = this.head.getData();
		this.head = this.head.getNext();
		this.size--;
		
		return retVal;
	}
	
	/**
	 * Remove and returns the element at a specific index in the list.
	 * 
	 * @param index -- the specified index
	 * @return -- the element at the index
	 * @throws IndexOutOfBoundsException -- if index is out of range (index < 0 || index >= size())
	 */
	public E remove(int index) throws IndexOutOfBoundsException {
		if (index < 0 || index >= this.size()) {
			throw new IndexOutOfBoundsException();
		}
		
		// remove and return elem at `index`
		Node<E> retVal;
		if (index == 0) {
			retVal = head;
			this.head = this.head.getNext();
		}
		else {
			Node<E> prevVal = this.getNode(index - 1);
			retVal = prevVal.getNext();
			prevVal.link(retVal.getNext());
		}
		this.size--;
		
		return retVal.getData();
	}
	
	/**
	 * Determine the index of an element in the list.
	 * 
	 * @param element -- the element to search for
	 * @return -- the index of the first occurrence; -1 if the element is not found
	 */
	public int indexOf(E element) {
		for (int i = 0; i < this.size(); i++) {
			if (this.get(i).equals(element)) {
				return i;
			}
		}
		return -1;
	}
	
	/**
	 * Determine the number of elements in the list.
	 * 
	 * @return -- the number of elements in this list
	 */
	public int size() {
		return this.size;
	}
	
	/**
	 * Determine if this list does not contain any elements.
	 * 
	 * @return -- true if this collection contains no elements; false, otherwise
	 */
	public boolean isEmpty() {
		return (this.size == 0);
	}
	
	/**
	 * Remove all of the elements from this list.
	 */
	public void clear() {
		this.head = null;
		this.size = 0;
	}
	
	/**
	 * Translate this list into an array.
	 * 
	 * @return -- an array containing all of the elements of this list, in order
	 */
	public Object[] toArray() {
		Object[] arr = new Object[this.size()];
		
		for (int index = 0; index < this.size(); index++) {
			arr[index] = this.get(index);
		}
		
		return arr;
	}
	
	/**
	 * Iterate over this list.
	 * 
	 * @return -- an iterator for this list that iterates from start to finish
	 */
	public Iterator<E> iterator() {
		return new Iterator<E>() {
			// cursor represents the last element that the iterator returned
			// initially -1 because no element has been accessed yet
			int cursor = -1;
			// flag to keep track of whether the last call was next()
			boolean lastCallWasNextFlag = false;
			
			@Override
			public boolean hasNext() {
				return ((cursor + 1) < size);
			}
			
			@Override
			public E next() {
				if (!this.hasNext()) {
					throw new NoSuchElementException();
				}
				
				lastCallWasNextFlag = true;
				return SinglyLinkedList.this.get(++cursor);
			}
			
			@Override
			public void remove() {
				// cannot remove element if last call was next
				if (!lastCallWasNextFlag) {
					throw new IllegalStateException();
				}
				
				SinglyLinkedList.this.remove(cursor);
				cursor--;
				lastCallWasNextFlag = false;
			}
		};
	}
	
	/**
	 * Retrieve the node of a specific index.
	 * 
	 * @param index -- index to retrieve node at, assumed in bounds
	 * @return -- node at the index
	 */
	private Node<E> getNode(int index) {
		Node<E> focusNode = head;
		for (int i = 0; i < index; i++) {
			focusNode = focusNode.getNext();
		}
		return focusNode;
	}
}
