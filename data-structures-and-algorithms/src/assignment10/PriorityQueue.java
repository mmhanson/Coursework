package assignment10;

import java.util.Comparator;
import java.util.NoSuchElementException;

/**
 * A priority queue of generically-typed items. 
 * 
 * The queue is implemented as a min heap. 
 * The min heap is implemented implicitly as an array.
 * 
 * @author Maxwell Hanson
 * @since Apr 4, 2018
 * @version 1.0.0
 * 
 * @param E -- the type contained in the queue. Unchecked.
 */
@SuppressWarnings("unchecked")
public class PriorityQueue<E> {
	/**
	 * The amount of elements in the priority queue.
	 */
	private int currentSize;
	/**
	 * Representation of the priority queue.
	 * @see {@link https://www.cs.cmu.edu/~adamchik/15-121/lectures/Binary%20Heaps/heaps.html BinaryHeaps}
	 */
	private E[] array;
	/**
	 * Definition of the order of the elements in the queue.
	 * If null, then the natural ordering of the elements is used.
	 */
	private Comparator<? super E> cmp;

	/**
	 * Construct an empty priority queue with natural ordering.
	 */
	public PriorityQueue() {
		currentSize = 0;
		cmp = null;
		array = (E[]) new Object[10];
	}

	/**
	 * Construct an empty priority queue with explicit ordering.
	 * 
	 * @param comp -- definition of ordering
	 */
	public PriorityQueue(Comparator<? super E> comp) {
		currentSize = 0;
		cmp = comp;
		array = (E[]) new Object[10]; // safe to ignore warning
	}

	/**
	 * Determine the number of items in the priority queue.
	 * 
	 * @return -- the number of items in this priority queue.
	 */
	public int size() {
		return currentSize;
	}

	/**
	 * Remove all items from the priority queue.
	 */
	public void clear() {
		currentSize = 0;
	}

	/**
	 * Determine the smallest element in the priority queue.
	 * 
	 * @return -- the minimum item in this priority queue
	 * @throws NoSuchElementException -- if this priority queue is empty
	 */
	public E findMin() throws NoSuchElementException {
		if (currentSize == 0) {
			throw new NoSuchElementException();
		}
		
		return array[0];
	}

	/**
	 * Removes and returns the minimum item in this priority queue.
	 * 
	 * @return -- the minimum item in this priority queue.
	 * @throws NoSuchElementException -- if this priority queue is empty.
	 */
	public E deleteMin() throws NoSuchElementException {
		E min = findMin();

		array[0] = array[currentSize - 1];
		currentSize--;
		heapify(0);

		return min;
	}

	/**
	 * Add an item to this priority queue.
	 * 
	 * @param item -- the item to be inserted
	 */
	public void add(E item) {
		// reallocate storage if necessary
		if (currentSize == array.length) {
			reallocate();
		}

		// add item to end
		array[currentSize] = item;
		currentSize++;
		heapifyUp();

	}

	/**
	 * Generate dot code for visualizing this priority queue.
	 * 
	 * @return -- dot code for visualizing this priority queue
	 */
	public String generateDot() {
		String result = "digraph Heap {\n\tnode [shape=record]\n";

			for(int i = 0; i < currentSize; i++) {
				result += "\tnode" + i + " [label = \"<f0> |<f1> " + array[i] + "|<f2> \"]\n";
				if(((i*2) + 1) < currentSize)
					result += "\tnode" + i + ":f0 -> node" + ((i*2) + 1) + ":f1\n";
				if(((i*2) + 2) < currentSize)
					result += "\tnode" + i + ":f2 -> node" + ((i*2) + 2) + ":f1\n";
			}
			return result + "}\n";
	}
	
	/**
	 * Restore the heap property starting at an index.
	 * 
	 * @param index -- the index of the element to start restoring the heap at.
	 */
	private void heapify(int index) {
		int leftChildInd = getLeftChildIndex(index);
		int rightChildInd = getRightChildIndex(index);
		int minInd = index;
		
		// assign minInd to the index of the minimum of parent and children
		if (leftChildInd >= 0 && compare(array[leftChildInd], array[minInd]) < 0) {
			minInd = leftChildInd;
		}
		if (rightChildInd >= 0 && compare(array[rightChildInd], array[minInd]) < 0) {
			minInd = rightChildInd;
		}
		
		swap(index, minInd);
		if (minInd != index) {
			heapify(minInd);
		}
	}
	
	/**
	 * Restore the heap property starting at the last node and going up.
	 */
	private void heapifyUp() {
		int index = currentSize - 1;
		
		// while the parent node exists and is greater than the current node
		while (getParentIndex(index) >= 0 &&
				compare(array[getParentIndex(index)], array[index]) > 0) {
			swap(getParentIndex(index), index);
			index = getParentIndex(index);
		}
	}
	
	/**
	 * Swap two elements in the array.
	 * 
	 * Indexes are assumed to be in the range of the array.
	 * 
	 * @param index1 -- first index to swap
	 * @param index2 -- second index to swap
	 */
	private void swap(int index1, int index2) {
		E buff = array[index1];
		
		array[index1] = array[index2];
		array[index2] = buff;
	}
	
	/**
	 * Double the size of the array.
	 */
	private void reallocate() {
		E[] newArr = (E[]) new Object[array.length * 2];
		
		// copy contents
		for (int curs = 0; curs < currentSize; curs++) {
			newArr[curs] = array[curs];
		}
		
		array = newArr;
	}
	
	/**
	 * Get the index of the parent of an element.
	 * 
	 * @param index -- the index of the element to find the parent of
	 * @return -- the index of the parent of the element, or a negative
	 *   number of the element is the root
	 */
	private int getParentIndex(int index) {
		if (index <= 0) {
			return -1;
		}
		
		return (index - 1) / 2;
	}
	
	/**
	 * Get the index of the left child of an element.
	 * 
	 * @param index -- the index of the element to find the left child of
	 * @return -- the index of the left child of the element,
	 *   or a negative number of the element does not have a left child
	 */
	private int getLeftChildIndex(int index) {
		int retVal = (2 * index) + 1;
		
		if (retVal >= currentSize) {
			return -1;
		}
		
		return retVal;
	}
	
	/**
	 * Get the index of the right child of an element.
	 * 
	 * @param index -- the index of the element to find the right child of
	 * @return -- the index of the right child of the element, or a negative
	 *   number of the element does not have a right child
	 */
	private int getRightChildIndex(int index) {
		int retVal = (2 * index) + 2;
		
		if (retVal >= currentSize) {
			return -1;
		}
		
		return retVal;
	}
	
	/**
	 * Internal method for comparing lhs and rhs using Comparator if provided by the
	 * user at construction time, or Comparable, if no Comparator was provided.
	 * 
	 * @param lhs - the left-hand-side item being compared
	 * @param rhs - the right-hand-side item being compared
	 * @return a negative integer if lhs < rhs, 0 if lhs == rhs, a positive integer if lhs > rhs
	 */
	private int compare(E lhs, E rhs) {
		if (cmp == null) {
			return ((Comparable<? super E>) lhs).compareTo(rhs); // safe to ignore warning
		}
		// We won't test your code on non-Comparable types if we didn't supply a Comparator

		return cmp.compare(lhs, rhs);
	}

	//LEAVE IN for grading purposes
	public Object[] toArray() {    
		Object[] ret = new Object[currentSize];
		for(int i = 0; i < currentSize; i++) {
			ret[i] = array[i];
		}
		return ret;
	}
}
