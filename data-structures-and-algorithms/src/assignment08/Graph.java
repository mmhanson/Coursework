package assignment08;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.util.ArrayList;

/**
 * This object will create a working virtual representation of PACMAN map file
 * in the form of a graph. This object can be called up in calculating the
 * fastest route to a given destination.
 * 
 * @author Maxwell Hanson
 * @since March 13, 2018
 * @version 1.0.0
 */

public class Graph {
	/**
	 * starting position on the map
	 */
	private Node start;
	/**
	 * (desired) ending position on the map
	 */
	private Node goal;
	/**
	 * matrix representing the map
	 */
	private Node[][] board;
	/**
	 * height of the map
	 */
	private int height;
	/**
	 * width of the map
	 */
	private int width;

	/**
	 * Construct a graph by reading from a file.
	 * 
	 * @param filename -- path of file to use to create graph
	 * @throws IOException -- if issues reading file
	 * @throws FileNotFoundException -- if the file is not found
	 */
	public Graph(String filename) throws IOException {
		File inputFile = new File(filename);
		BufferedReader buffer = new BufferedReader(new FileReader(inputFile));
		
		// assign height and width
		String firstLine = buffer.readLine();
		String[] splitFirstLine = firstLine.split(" ");
		height = Integer.valueOf(splitFirstLine[0]);
		width = Integer.valueOf(splitFirstLine[1]);
		
		// construct the unlinked array of nodes
		board = new Node[height][width];
		for (int nRow = 0; nRow < height; nRow++) {
			for (int nCol = 0; nCol < width; nCol++) {
				switch ((char)buffer.read()) {
					case 'X':
						// walls are null in the node grid
						board[nRow][nCol] = null;
						break;
					case ' ':
						board[nRow][nCol] = new Node();
						break;
					case 'G':
						board[nRow][nCol] = new Node();
						goal = board[nRow][nCol];
						break;
					case 'S':
						board[nRow][nCol] = new Node();
						start = board[nRow][nCol];
						break;
				}
			}
			buffer.read(); // skip newline
		}
		
		// link the array of nodes
		Node cursor;
		for (int nRow = 0; nRow < height; nRow++) {
			for (int nCol = 0; nCol < width; nCol++) {
				cursor = board[nRow][nCol];
				
				// null nodes (aka walls) are not linked to any adjacent nodes
				// so it is impossible for pacman to traverse to them
				if (cursor == null) {
					continue;
				}
				
				for(Node adjacentNode: getAdjacentNodes(nRow, nCol)) {
					cursor.link(adjacentNode);
				}
			}
		}
		
		buffer.close();
	}
	
	/**
	 * Determine all nodes adjacent to a given node at specified coordinates.
	 * 
	 * @param x -- x-coordinate of the node in question
	 * @param y -- y-cooridnate of the node in question
	 * @return -- list of nodes adjacent to node at (x, y)
	 */
	public ArrayList<Node> getAdjacentNodes(int y, int x) {
		ArrayList<Node> adjacentNodes = new ArrayList<Node>();
		Node cursor = board[y][x];
		
		// top
		if (y - 1 >= 0 && board[y - 1][x] != null) {
			cursor.link(board[y - 1][x]);
		}
		
		// bottom
		if (y + 1 < height && board[y + 1][x] != null) {
			cursor.link(board[y + 1][x]);
		}
		
		// left
		if (x - 1 >= 0 && board[y][x - 1] != null) {
			cursor.link(board[y][x - 1]);
		}
		
		// right
		if (x + 1 < width && board[y][x + 1] != null) {
			cursor.link(board[y][x + 1]);
		}
		
		return adjacentNodes;
	}

	/**
	 * Retrieve the starting node of the graph.
	 * 
	 * @return The starting node of this graph
	 */
	public Node getStart() {
		return start;
	}

	/**
	 * Retrieve the goal of the map.
	 * 
	 * @return -- the goal node of this graph
	 */
	public Node getGoal() {
		return goal;
	}

	/**
	 * Write current state of the graph to file.
	 * 
	 * @param outputFile -- The path to the file to be written. If the file does not exist, it will be created.
	 */
	public void toFile(String outputFile) {
		try (PrintWriter output = new PrintWriter(new FileWriter(outputFile))) {
			output.println(height + " " + width);
			for (int i = 0; i < height; i++) {
				for (int j = 0; j < width; j++) {
					if (board[i][j] == null) {
						output.print('X');
					} else if (board[i][j].equals(start)) {
						output.print('S');
					} else if (board[i][j].equals(goal)) {
						output.print('G');
					} else if (board[i][j].isOnPath()) {
						output.print('.');
					} else {
						output.print(' ');
					}
				}
				output.print('\n');
			}
			output.close();
		} catch (IOException e) {
			System.out.println("File could not be created");
			e.printStackTrace();
		}
	}

	/**
	 * Returns the dimensions of the graph.
	 * 
	 * The first value is the height, the second iss the width.
	 * 
	 * @return -- an array with the dimensions of this graph
	 */
	public int[] getDimensions() {
		return new int[] { height, width };
	}
}
