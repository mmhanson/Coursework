package assignment08;

import java.util.ArrayList;

/**
 * Node objects are used by the Graph class. Each is a cell. Walls are null.
 * Each links to the 4 nodes adjacent to it. Each node contains a marker
 * to signal that it has been 'visited' in the pathfinding algorithm.
 * 
 * @author Maxwell Hanson
 * @date March 13, 2018
 * @version 1.0.0
 */

public class Node {
	/**
	 * List of all the nodes linked to this one
	 */
	private ArrayList<Node> linked;
	/**
	 * Cost to get to this node. Starts at max value
	 */
	private int cost;
	/**
	 * Is true if the cost is the smallest to get to this node from the start
	 */
	private boolean permanent;
	/**
	 * Is true if the node is on the shortest path to the goal
	 */
	private boolean onPath;

	/**
	 * Default constructor
	 */
	public Node() {
		cost = Integer.MAX_VALUE;
		permanent = false;
		onPath = false;
		linked = new ArrayList<>();
	}

	/**
	 * Return an ArrayList with all the linked nodes to this node.
	 * 
	 * @return An ArrayList containing all the nodes linked to this node
	 */
	public ArrayList<Node> getLinkedNodes() {
		return linked;
	}

	/**
	 * Alter the value of the node's cost if the cost has not been marked as permanent
	 * 
	 * @param newCost -- The value which the cost shall be set to
	 */
	public void setCost(int newCost) {
		if (!permanent) {
			cost = newCost;
		}
	}

	/**
	 * Retrieve the cost of the node.
	 * 
	 * @return -- the cost of getting to this node
	 */
	public int getCost() {
		return cost;
	}

	/**
	 * Mark this node as being on the tentative shortest path to the goal.
	 */
	public void mark() {
		this.onPath ^= true;
	}

	/**
	 * Determine if this node is on the shortest path to the goal.
	 * 
	 * @return - whether or not the node is on the shortest path to the goal node
	 */
	public boolean isOnPath() {
		return onPath;
	}

	/**
	 * Mark this node as permanent.
	 * 
	 * Cannot be undone.
	 */
	public void makePermanent() {
		permanent = true;
	}

	/**
	 * Determine if this node is permanent.
	 * 
	 * @return -- true if permanent, false otherwise.
	 */
	public boolean isPerm() {
		return permanent;
	}

	/**
	 * Link this node to another node
	 * 
	 * @param node -- the node to be linked to this node
	 */
	public void link(Node node) {
		linked.add(node);
	}
}
