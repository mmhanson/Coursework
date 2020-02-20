package assignment08;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.LinkedList;

/**
 * A class to find the shortest path between two points in a graph.
 * 
 * @author Maxwell Hanson
 * @since Mar 15, 2018
 * @version 1.1.0
 */
public class PathFinder {
	/**
	 * Find the shortest path from point A to point B in a graph.
	 * 
	 * This method implements Dijkstra's Algorithm for its pathfinding logic.
	 * 
	 * Graphs are parameterized as an input file with conventions as follows... *
	 * The first line consists of a number, a space, followed by another number. The
	 * first number is the number of rows in the graph, the second number is the
	 * number of columns in the graph. * The character 'X' represents a wall, i.e. a
	 * node in which one cannot traverse into * The character 'S' represents the
	 * starting point for the path finder * The character 'G' represents the goal,
	 * or ending point for the path finder. Example: 5 5 XXXXX XS X X X X GX XXXXX
	 * 
	 * The shortest path from the starting point is returned as a file representing
	 * the shortest path. The file is formatted with conventison as follows... * All
	 * the conventions of the graph input file are followed * Every node that the
	 * shortest path crosses are specified with a period. Example: 5 5 XXXXX XS..X X
	 * .X X GX XXXXX
	 * 
	 * @param inputFile
	 *            -- input file representing graph as per restrictions above
	 * @param outputFile
	 *            -- output representing the shortest path from the starting ending
	 *            points specified in the graph.
	 */
	public static void solveMaze(String inputFile, String outputFile) {
		// FNFEs are discarded, files are assumed to exist
		Graph graph = null;
		try {
			graph = new Graph(inputFile);
		}
		catch (FileNotFoundException e) {
			System.out.println("FileNotFoundException thrown.");
			e.printStackTrace();
		}
		catch (IOException e) {
			System.out.println("IOException thrown.");
			e.printStackTrace();
		}
		
		Node start = graph.getStart();
		Node cursor = new Node();
		// the paths used in the process of dijkstra's algo are modelled
		// with a tree data structure in which the path from any node (u)
		// in the tree to any descendent (v) is the shortest path from
		// u to v in the graph (shortest-path tree)
		Tree<Node> pathTree = new Tree<Node>(start);

		/*
		 * Perform Dijkstra's Algorithm. Overview of steps below.
		 * 
		 * STEP 0: Mark all nodes' cost as infinity except the starting node, whose cost
		 * is marked as zero.
		 * 
		 * STEP 1: Go to the temporary (non-permanent) node with the smallest cost, mark
		 * this node as permanent and make this node as the current node. If there are
		 * no non-permanent nodes, then stop the algo
		 * 
		 * STEP 2: Assign costs to all nodes connected to the current node according to
		 * the following equation: C(x) = C(curr) + W(curr, x) s.t. C(X): "cost of x",
		 * W(x, y): "weight of the edge connecting x and y" If the cost of a node is
		 * changed, the parent is changed this is done by adding it as a leaf of the
		 * node whose parent it is being changed to
		 * 
		 * STEP 3: Return to step 1.
		 */

		// STEP 0
		// Note all nodes are marked with cost of infinity in the
		// construction of the graph.
		start.setCost(0);

		// STEP 1 (part 1)
		while (hasTempLeaf(pathTree)) {
			// STEP 1 (part 2)
			// note that all nodes in the graph that are temporary and
			// whose cost is not infinity are leaves in the path tree
			cursor = getSmallestTmpLeaf(pathTree.getLeaves());
			cursor.makePermanent();

			// if the current node is the end node, we can stop the algorithmn
			if (cursor.equals(graph.getGoal())) {
				break;
			}

			// STEP 2
			for (Node connNode : cursor.getLinkedNodes()) {
				// weight of all edges is set to one
				if (cursor.getCost() + 1 < connNode.getCost()) {
					connNode.setCost(cursor.getCost() + 1);

					// add as leaf to current (equivalent to changing parent to
					// current) if cost is changed
					pathTree.link(cursor, connNode);
				}
			}

			// STEP 3 (implicit)
		}

		// After this point, the shortest path from start to end is the
		// path from the root of the pathTree to the element of the pathTree
		// equivalent to Graph.getGoal().

		// The path is now transcribed to a linked list.
		LinkedList<Node> path = new LinkedList<Node>();
		// add from finish to start, then reverse
		try {
			cursor = graph.getGoal();
			while (cursor != start) {
				cursor = pathTree.getParent(cursor);
				path.add(cursor);
			}
			path = reverse(path);
		}
		catch (NullPointerException e) {
			path = new LinkedList<Node>();
		}
		
		// each node in the path is marked as 'path' on the graph
		for (Node pathNode: path) {
			pathNode.mark();
		}

		// the solved maze is written to the output file through
		// the graph class
		graph.toFile(outputFile);
	}
	
	private static Node getSmallestTmpLeaf(LinkedList<Node> leaves) {
		// get all tmp leaves
		LinkedList<Node> tmpLeaves = new LinkedList<Node>();
		for (Node leaf: leaves) {
			if (!leaf.isPerm()) {
				tmpLeaves.add(leaf);
			}
		}
		
		// minimize tmp leaves
		Node smallest = tmpLeaves.getFirst();
		for (Node tmpLeaf: tmpLeaves) {
			if (tmpLeaf.getCost() < smallest.getCost()) {
				smallest = tmpLeaf;
			}
		}
		
		return smallest;
	}

	/**
	 * Determine if a tree has any leaf that is not marked as permanent.
	 * 
	 * @param pathTree -- tree to use for calculations
	 * @return -- true if tree has a leaf that is not permanent, false otherwise.
	 */
	private static boolean hasTempLeaf(Tree<Node> pathTree) {
		for (Node leaf : pathTree.getLeaves()) {
			if (!leaf.isPerm()) {
				return true;
			}
		}

		return false;
	}

	/**
	 * Reverse a linked list
	 * 
	 * @param list -- list to reverse
	 * @return -- reverse of list
	 */
	private static LinkedList<Node> reverse(LinkedList<Node> list) {
		LinkedList<Node> revList = new LinkedList<Node>();

		for (int index = list.size() - 1; index >= 0; index--) {
			revList.add(list.get(index));
		}

		return revList;
	}
}
