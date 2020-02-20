package assignment06;

/**
 * A node of a linked list.
 * 
 * This class is used in the implementation of the SinglyLinkedList class.
 * 
 * @author Maxwell Hanson
 * @since Dec 27, 2017
 * @version 1.1.0
 * 
 * @param <E> -- What data type is stored in the node.
 */
public class Node<E> {
	/**
	 * Reference to the next node in the chain.
	 * Null if this node does not point to another node
	 */
	private Node<E> next;
	/**
	 * The data stored in this node.
	 */
	private E data;
	
	/**
	 * Construct an empty node.
	 */
	Node() {
		this.data = null;
		this.next = null;
	}
	
	/**
	 * Construct a node with data.
	 * 
	 * @param data -- the data to put in this node
	 */
	Node(E data) {
		this.data = data;
		this.next = null;
	}
	
	/**
	 * Get the node that this node points to.
	 * 
	 * @return -- node that this node points to.
	 */
	public Node<E> getNext() {
		return next;
	}
	
	/**
	 * Determine if this node points to another node.
	 * 
	 * @return -- true if this node points to another, false otherwise
	 */
	public boolean hasNext() {
		return !(this.next == null);
	}
	
	/**
	 * Point this node to a new node.
	 * 
	 * @param newNode -- the node to make this node reference
	 */
	public void link(Node<E> newNode) {
		this.next = newNode;
	}
	
	/**
	 * Replace the data in this node with new data.
	 * 
	 * @param newData -- the data to put in this node
	 */
	public void replaceData(E newData) {
		this.data = newData;
	}
	
	/**
	 * Retrieve the data in this node.
	 * 
	 * @return -- data in this node
	 */
	public E getData() {
		return this.data;
	}
}
