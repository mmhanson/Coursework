package assignment08;

import java.util.ArrayList;
import java.util.LinkedList;

/**
 * Generic tree.
 * 
 * @author Maxwell Hanson
 * @version 1.1.0
 * @since Mar 13, 2018
 * 
 * @param <T> -- type of object the tree holds
 */
public class Tree<T> {
	/**
	 * Root of the tree.
	 */
	TreeNode<T> root;
	
	/**
	 * Construct a tree with a root.
	 * 
	 * @param root -- the root of the tree to be
	 */
	public Tree(T root) {
		this.root = new TreeNode<T>(root);
	}
	
	/**
	 * Determine the parent of the a node in the tree.
	 * 
	 * @param node -- the node in question
	 * @return -- parent of parameter, or null if the parameter is the
	 *   root or not in the tree
	 */
	public T getParent(T node) {
		return getNodeOfData(node).getParent().getData();
	}
	
	/**
	 * Compile a list of all the leaves in the tree.
	 * 
	 * @return -- list of all leaves in the tree.
	 */
	public LinkedList<T> getLeaves() {
		return getLeavesRec(root);
	}
	
	private LinkedList<T> getLeavesRec(TreeNode<T> node) {
		LinkedList<T> leaves = new LinkedList<T>();
		ArrayList<TreeNode<T>> links = node.getLinks();
		
		if (links.isEmpty()) {
			leaves.add(node.getData());
			return leaves;
		}
		else {
			for (TreeNode<T> child: node.getLinks()) {
				leaves.addAll(getLeavesRec(child));
			}
		}
		
		return leaves;
	}
	
	/**
	 * Link two nodes in the tree.
	 * 
	 * @param nodeFrom -- node to link from
	 * @param nodeTo -- node to link to
	 */
	public void link(T nodeFrom, T nodeTo) {
		TreeNode<T> nodeFromNode = getNodeOfData(nodeFrom);
		
		TreeNode<T> nodeToNode = getNodeOfData(nodeTo);
		if (nodeToNode == null) {
			nodeToNode = new TreeNode<T>(nodeTo);
		}
		
		nodeFromNode.linkTo(nodeToNode);
	}
	
	/**
	 * Get TreeNode<T> instance with 'data'.
	 * 
	 * @param data -- data to find
	 * @return -- tree node with data, null if not found
	 */
	private TreeNode<T> getNodeOfData(T data) {
		if (root == null) {
			return null;
		}
		
		LinkedList<TreeNode<T>> pathStack = new LinkedList<TreeNode<T>>();
		
		pathStack.push(root);
		
	    // Queue based level order traversal
	    while (pathStack.isEmpty() == false)
	    {
	        // See if current node is same as x
	        TreeNode<T> cursor = pathStack.peek();
	        if (cursor.getData() == data) {
	            return cursor;
	        }
	 
	        // Remove current node and enqueue its children
	        pathStack.pop();
	        
	        for (TreeNode<T> child: cursor.getLinks()) {
	        	pathStack.push(child);
	        }
	    }
	 
	    return null;
	}
}

/**
 * Node of the tree.
 * 
 * @author Maxwell Hanson
 * @version 1.1.0
 * @date Mar 15, 2018
 * 
 * @param <T> -- Type of data the node holds
 */
class TreeNode<T> {
	// reference to each node that this node is 'linked' to
	private ArrayList<TreeNode<T>> links;
	// data this node holds
	T data;
	// node that points to this node
	TreeNode<T> parent;
	
	public TreeNode(T data) {
		this.data = data;
		this.links = new ArrayList<TreeNode<T>>();
		this.parent = null;
	}
	
	public T getData() {
		return this.data;
	}
	
	public ArrayList<TreeNode<T>> getLinks() {
		return this.links;
	}
	
	public void linkTo(TreeNode<T> nodeTo) {
		links.add(nodeTo);
		nodeTo.setParent(this);
	}
	
	public void setParent(TreeNode<T> p) {
		this.parent = p;
	}
	
	public TreeNode<T> getParent() {
		return this.parent;
	}
}