package assignment07;

import java.util.Collection;
import java.util.NoSuchElementException;

/**
 * An implementation of a sorted set with a binary search tree structure.
 * 
 * @author Maxwell Hanson
 * @since Jul 17, 2018
 * @version 1.1.0
 *
 * @param <E> -- element to be used in the set
 */
public class BinarySearchTree<E extends Comparable<? super E>> implements SortedSet<E> {
	/**
	 * A cursor to keep track of nodes in the tree. Notably for use in traversing the tree
	 */
	private bstNode<E> cursor;
	/**
	 * The root of the tree
	 */
	private bstNode<E> root;
	/**
	 * The number of elements in the tree.
	 */
	private int size;

	/**
	 * Construct a BST with a root.
	 * 
	 * @param element -- the root of the bst
	 */
	public BinarySearchTree(E element) {
		root = new bstNode<>(element);
		cursor = root;
		size = 1;
	}

	/**
	 * Construct an empty BST.
	 */
	public BinarySearchTree() {
		root = null;
		cursor = root;
		size = 0;
	}

	/**
	 * Determine the smallest element of the BST.
	 * 
	 * @return -- smallest (leftmost) element of the BST.
	 */
	@Override
	public E first() throws NoSuchElementException {
		if (this.isEmpty()) {
			throw new NoSuchElementException();
		}
		
		return root.getLeftMost().getData();
	}

	/**
	 * Determine the largest element of the BST.
	 * 
	 * @return -- largest (rightmost) element of the BST.
	 */
	@Override
	public E last() throws NoSuchElementException {
		if (root == null) {
			throw new NoSuchElementException();
		}
		
		return root.getRightMost().getData();
	}

	/**
	 * Add a single item to the BST.
	 * 
	 * @param -- element to add
	 * @return -- true if addition was successful, false otherwise
	 */
	@Override
	public boolean add(E element) {
		// only a driver method for the addRec method
		if (size == 0) {
			root = new bstNode<E>(element);
			cursor = root;
			size = 1;
			return true;
		}
		
		return addRec(element);
	}

	/**
	 * Add many elements to the BST.
	 * 
	 * @param elements -- a collection of elements to add
	 * @return -- true if all elements added, false otherwise
	 */
	@Override
	public boolean addAll(Collection<? extends E> elements) {
		int sizeBefore = size;
		
		for (E element : elements) {
			add(element);
			reset();
		}
		
		// If the size is unchanged then not all elements added
		if (sizeBefore - size == 0) {
			return false;
		}
		return true;
	}

	/**
	 * Remove all elements from the BST.
	 */
	@Override
	public void clear() {
		root = null;
		size = 0;
	}

	/**
	 * Determine if the BST contains an element
	 * 
	 * @param element -- the element to check if the BST contains
	 * @return -- true if the BST contains the element, false otherwise
	 */
	@SuppressWarnings("unchecked")
	@Override
	public boolean contains(Object element) {
		// edge cases
		if (this.isEmpty() || element == null) {
			return false;
		}
		E data;
		// attempt to cast to E
		try {
			data = (E) element;
			data.compareTo(root.getData());
		}
		catch (ClassCastException e) {
			return false;
		}
		
		// driver method for containsRec
		return containsRec(data);
	}

	/**
	 * Determine if the BST contains a collection of elements.
	 * 
	 * @param elements -- the collection of elements to add
	 * @return -- true if all elements are in the list, false otherwise
	 */
	@Override
	public boolean containsAll(Collection<?> elements) {
		for (Object element : elements) {
			if (!this.contains(element)) {
				return false;
			}
		}
		
		return true;
	}

	/**
	 * Determine if the BST is empty.
	 * 
	 * @return -- true if the BST has no elements, false otherwise
	 */
	@Override
	public boolean isEmpty() {
		return (size == 0);
	}

	/**
	 * Remove an element from the BST.
	 * 
	 * @param element -- the element to remove
	 * @return -- true if the element was removed, false otherwise
	 */
	@SuppressWarnings("unchecked")
	@Override
	public boolean remove(Object element) {
		// parameter sanitation
		if (this.isEmpty() || element == null) {
			return false;
		}
		E data;
		try {
			data = (E) element;
			data.compareTo(root.getData());
		} catch (ClassCastException e) {
			return false;
		}
		// Handle root removals
		if (root.getData().equals(data)) {
			performRootRemoval();
			reset();
			return true;
		} else if (!containsNonResettingRec(data)) {
			return false;
		}
		
		// Because containsNonResettingRec was called
		// the pointer should be at the node of interest
		// or, the method has already terminated
		bstNode<E> parent = cursor.getParent();
		// There are three cases when we want to remove a node
		switch (cursor.numChildren()) {
		// There are no children
		case 0:
			// Then simply remove it
			if (parent.getLeft() == cursor) {
				parent.setLeft(null);
			} else {
				parent.setRight(null);
			}
			cursor.setParent(null);
			size--;
			reset();
			return true;
		// There is only one child
		case 1:
			// Then set the child's parent to the pointer's parent
			// And set the parents child to the pointer's child
			// And then reset the pointer
			bstNode<E> child;
			if (cursor.getLeft() == null) {
				child = cursor.getRight();
			} else {
				child = cursor.getLeft();
			}
			if (parent.getLeft() == cursor) {
				parent.setLeft(child);
			} else {
				parent.setRight(child);
			}
			child.setParent(parent);
			size--;
			reset();
			return true;
		// There are two children
		case 2:
			// We must find a successor, copy its data, and then remove the successor
			// Which either has one child, or none
			bstNode<E> snapShot = cursor;
			bstNode<E> successor = findSuccessor();
			E successorData = successor.getData();
			reset();
			remove(successor);
			snapShot.setData(successorData);
			return true;
		}
		// This should not be reached, but if is, then we know something has gone wrong
		return false;
	}

	/**
	 * Remove a collection of elements from the BST.
	 * 
	 * @param elements -- a collection of elements to remove
	 * @return -- true if any element was removed, false otherwise
	 */
	@Override
	public boolean removeAll(Collection<?> elements) {
		if (elements == null) {
			return false;
		}
		
		int sizeBefore = size;
		
		for (Object element: elements) {
			remove(element);
		}
		
		return (sizeBefore != size);
	}

	/**
	 * Determine the count of elements in the tree.
	 *
	 * @return -- the size of the tree
	 */
	@Override
	public int size() {
		return size;
	}

	/**
	 * Translate the BST to an array.
	 * 
	 * @return -- array representation to the tree
	 */
	@Override
	public Object[] toArray() {
		return toArrayRec(root);
	}

	/**
	 * Create a string of dot code that graphs the BST.
	 * 
	 * @return -- contents of dot-file graphing the BST, empty string if tree has no values.
	 */
	public String generateDot() {
		String retVal;
		
		if(root == null) {
			return "";
		}

		retVal = "digraph BinarySearchTree {\n";
		retVal += "  node [shape=record]\n";
		retVal += root.generateDotRecursive();

		return retVal + "}";
	}

	/**
	 * Recursive part to the contains method
	 * 
	 * @param data
	 * @return
	 */
	private boolean containsRec(E data) {
		// If the data matches the data at the pointer at any time, return true
		if (data.compareTo(cursor.getData()) == 0) {
			reset();
			return true;
			// If the value is greater the pointer as the node to the right is null, or if
			// the value is less than the pointer's value and the left node is null, then
			// the value must not be in the set.
		} else if (((data.compareTo(cursor.getData()) > 0) && (cursor.getRight() == null))
				|| ((data.compareTo(cursor.getData()) < 0) && (cursor.getLeft() == null))) {
			reset();
			return false;
			// If we haven't found it or been able to conclude that it is not in the set,
			// move left or right accordingly
		} else if (data.compareTo(cursor.getData()) < 0) {
			moveLeft();
			return containsRec(data);
		} else {
			moveRight();
			return containsRec(data);
		}
	}

	/**
	 * This just like the recursive contains method, except the pointer is not reset
	 * back to root if a match is found. This means that the pointer will be able to
	 * access the node between method calls easily, but complications can occur if
	 * not used properly. This method is necessary to remove nodes efficiently, as
	 * it allows us to locate and remove a node in one try.
	 * 
	 * @param data
	 * @return
	 */
	private boolean containsNonResettingRec(E data) {
		if (data.compareTo(cursor.getData()) == 0) {
			return true;
		} else if (((data.compareTo(cursor.getData()) > 0) && (cursor.getRight() == null))
				|| ((data.compareTo(cursor.getData()) < 0) && (cursor.getLeft() == null))) {
			reset();
			return false;
		} else if (data.compareTo(cursor.getData()) < 0) {
			moveLeft();
			return containsNonResettingRec(data);
		} else {
			moveRight();
			return containsNonResettingRec(data);
		}
	}
	
	/**
	 * Handles node removals if the node being removed is the root
	 */
	private void performRootRemoval() {
		switch (root.numChildren()) {
		case 0:
			root = null;
			size = 0;
			return;
		case 1:
			if (root.getLeft() == null) {
				root = root.getRight();
			} else {
				root = root.getLeft();
			}
			size--;
			return;
		case 2:
			reset();
			bstNode<E> successor = findSuccessor();
			E data = successor.getData();
			remove(successor);
			root.setData(data);
		}
	}

	/**
	 * Returns the smallest (leftmost) node to pointer
	 * 
	 * @return -- smallest node to the pointer
	 */
	private bstNode<E> findSuccessor() {
		// We assume in this case that the pointer is already at the node of interest
		// Since it will only be called in case 2 of remove
		// First move to the right
		moveRight();
		return cursor.getLeftMost();
	}
	
	/**
	 * Recursively Generate an array of a subtree.
	 * 
	 * @param root -- node to be used as the root of the subtree to generate the array representing.
	 * @return -- an array of this subtree.
	 */
	private Object[] toArrayRec(bstNode<E> root) {
		// this happens when the parent of the "root" has no such child
		if (root == null) {
			return new Object[] {};
		}
		
		Object[] leftArray = toArrayRec(root.getLeft());
		Object[] rightArray = toArrayRec(root.getRight());
		Object[] rootArray = new Object[] {root.data};
		
		return concat(concat(leftArray, rootArray), rightArray);
	}
	
	/**
	 * Concatenate two arrays.
	 * 
	 * @param arr0
	 * @param arr1
	 * @return -- arr0 + arr1
	 */
	private Object[] concat(Object[] arr0, Object[] arr1) {
		Object[] bothArr = new Object[arr0.length + arr1.length];
		
		for (int i = 0; i < arr0.length; i++) {
			bothArr[i] = arr0[i];
		}
		
		for (int j = 0; j < arr1.length; j++) {
			bothArr[arr0.length + j] = arr1[j];
		}
		
		return bothArr;
	}

	private class bstNode<T> {
		/**
		 * Data stored in node
		 */
		private T data;
		/**
		 * Left child node
		 */
		private bstNode<T> left;
		/**
		 * Right child node
		 */
		private bstNode<T> right;
		/**
		 * Parent node
		 */
		private bstNode<T> parent;
		
		/**
		 * Generate the dotgraph for the subtree defined with this node as the root.
		 */
		private String generateDotRecursive() {
			String retVal = "";
			
			retVal += "  node" + data + " [label = \"<f0> |<f1> " + data + "|<f2> \"]\n";
			
			if(left != null) {
				retVal += "  node" + data + ":f0 -> node" + left.getData() + ":f1\n" + left.generateDotRecursive();
			}
			
			if(right != null) {
				retVal += "  node" + data + ":f2 -> node" + right.getData() + ":f1\n" + right.generateDotRecursive();
			}

			return retVal;
		}

		/**
		 * Constructs a node with starting data, two children, and a parent
		 * 
		 * @param element
		 * @param left
		 * @param right
		 * @param parent
		 */
		public bstNode(T element, bstNode<T> left, bstNode<T> right, bstNode<T> parent) {
			data = element;
			this.left = left;
			this.right = right;
			this.parent = parent;
		}

		/**
		 * Retrieves the leftmost node
		 * 
		 * @return
		 */
		public bstNode<T> getLeftMost() {
			if (this.left == null) {
				return this;
			}
			return left.getLeftMost();
		}

		/**
		 * Retrieves the rightmost node
		 * 
		 * @return
		 */
		public bstNode<T> getRightMost() {
			if (this.right == null) {
				return this;
			}
			return right.getRightMost();
		}

		/**
		 * Creates a node with starting data only
		 * 
		 * @param element
		 */
		public bstNode(T element) {
			this(element, null, null, null);
		}

		/**
		 * Creates a node with starting data as well as a parent
		 * 
		 * @param element
		 * @param parent
		 */
		public bstNode(T element, bstNode<T> parent) {
			this(element, null, null, parent);
		}

		/**
		 * Returns the data stored in this node
		 * 
		 * @return
		 */
		public T getData() {
			return data;
		}

		/**
		 * Updates the data stored in this node
		 * 
		 * @param element
		 */
		public void setData(T element) {
			data = element;
		}

		/**
		 * Updates the left child of this node
		 * 
		 * @param node
		 */
		public void setLeft(bstNode<T> node) {
			left = node;
		}

		/**
		 * Updates the right child of this node
		 * 
		 * @param node
		 */
		public void setRight(bstNode<T> node) {
			right = node;
		}

		/**
		 * Updates the parent of this node
		 * 
		 * @param node
		 */
		public void setParent(bstNode<T> node) {
			parent = node;
		}

		/**
		 * Retrieves the left child of this node
		 * 
		 * @return
		 */
		public bstNode<T> getLeft() {
			return left;
		}

		/**
		 * Retrieves the right child of this node
		 * 
		 * @return
		 */
		public bstNode<T> getRight() {
			return right;
		}

		/**
		 * Retrieves the parent of this node
		 * 
		 * @return
		 */
		public bstNode<T> getParent() {
			return parent;
		}

		/**
		 * Returns the number of children a node has
		 * 
		 * @return
		 */
		public int numChildren() {
			int sum = 0;
			if (this.left != null) {
				sum++;
			}
			if (this.right != null) {
				sum++;
			}
			return sum;
		}

	}

	/**
	 * Moves the pointer to the left child of the pointer
	 */
	private void moveLeft() {
		cursor = cursor.getLeft();
	}

	/**
	 * Moves the pointer to the right child of the pointer
	 */
	private void moveRight() {
		cursor = cursor.getRight();
	}

	/**
	 * Returns the pointer back to its default state
	 */
	private void reset() {
		cursor = root;
	}

	/**
	 * Recursively add an element.
	 * 
	 * @param element -- element to add
	 * @return -- true if addition was successful, false otherwise
	 */
	private boolean addRec(E element) {
		// If at any point it is found that the item is already in the tree, return
		// false
		if (element.compareTo(cursor.getData()) == 0) {
			return false;
			// Otherwise, if the item is less than the node the pointer is on, and the item
			// to the left is null, add the item there and return true.
		} else if (element.compareTo(cursor.getData()) < 0 && (cursor.getLeft() == null)) {
			cursor.setLeft(new bstNode<E>(element, cursor));
			size++;
			reset();
			return true;
			// Otherwise, if the item is greater than the data at the pointer, and the item
			// to the right of it is null, then put the new element there and return true.
		} else if (element.compareTo(cursor.getData()) > 0 && (cursor.getRight() == null)) {
			cursor.setRight(new bstNode<E>(element, cursor));
			size++;
			reset();
			return true;
			// Lastly then, if we haven't found a duplicate or a place to add the new
			// element, move the pointer to the right or left accordingly, and try again.
		} else if (element.compareTo(cursor.getData()) < 0) {
			moveLeft();
			return addRec(element);
		} else {
			moveRight();
			return addRec(element);
		}
	}
}
