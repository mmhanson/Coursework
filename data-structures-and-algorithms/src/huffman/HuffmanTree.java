package huffman;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Map.Entry;
import java.util.PriorityQueue;

/**
 * Implements file compression and decompression using Huffman's algorithm,
 * which encodes and decodes each character of a file using a binary trie.
 * 
 * @author Maxwell Hanson
 * @version 1.0.0
 */
public class HuffmanTree {

	/**
	 * Represents a single node of a Huffman Tree.
	 */
	static class Node implements Comparable<Node> {
		public byte symbol; // character to be encoded/decoded
		public int frequency; // number of occurrences of the character
		public Node leftChild, rightChild, parent;

		/**
		 * Constructs a leaf node.
		 */
		public Node(byte symbol, int weight) {
			this.symbol = symbol;
			this.frequency = weight;
			leftChild = rightChild = parent = null;
		}

		/**
		 * Constructs an internal node. Note that an internal node has a weight
		 * (sum of the weights of its children) but no character
		 * (INCOMPLETE_CODE).
		 * 
		 * @param left
		 *            - left child of the new node
		 * @param right
		 *            - right child of the new node
		 */
		public Node(Node left, Node right) {
			symbol = UNFINISHED_ENCODING;
			frequency = left.frequency + right.frequency;
			leftChild = left;
			rightChild = right;
			parent = null;
		}

		/**
		 * W Returns a string containing all of the edges in the tree rooted at
		 * "this" node, in DOT format.
		 */
		public String generateDot() {
			String atNode = Character.toString((char) symbol);
			if (symbol == UNFINISHED_ENCODING) {
				atNode = " ";
			} else if (symbol == EOF) {
				atNode = "EOF";
			} else if (symbol == '\n') {
				atNode = "newline";
			} else if (symbol == '\t') {
				atNode = "tab";
			} else if (symbol == ' ') {
				atNode = "space";
			}
			atNode += " " + frequency;
			String ret = "\tnode" + hashCode() + " [label = \"<f0> |<f1> " + atNode + "|<f2> \"]\n";
			if (leftChild != null) {
				ret += "\tnode" + hashCode() + ":f0 -- node" + leftChild.hashCode() + ":f1\n" + leftChild.generateDot();
			}
			if (rightChild != null) {
				ret += "\tnode" + hashCode() + ":f2 -- node" + rightChild.hashCode() + ":f1\n"
						+ rightChild.generateDot();
			}

			return ret;
		}
		
		public Node getLeftmost() {
			Node cursor = this;
			
			while (cursor.leftChild != null) {
				cursor = cursor.leftChild;
			}
			return cursor;
		}

		/**
		 * Compares two Huffman nodes, using weight.
		 * 
		 * @param rhs
		 *            - right-hand side node
		 * @return a value > 0 if this node is larger than rhs, a value < 0 if
		 *         this node is smaller than rhs, 0 if the nodes are equal
		 */
		public int compareTo(Node rhs) {
			int ret = this.frequency - rhs.frequency;
			if (ret == 0) {
				return this.getLeftmost().symbol - rhs.getLeftmost().symbol;
			}
			
			return ret;
		}
		
		@Override
		public String toString() {
			return Character.toString((char)symbol) + ": " + frequency;
		}
	}

	Node root; // root of the Huffman tree
	Map<Byte, Node> symbols; // set of characters in the file mapped to their corresponding Huffman nodes
	List<Byte> data; // list of characters in the file
	final static int ERROR = -3; // used to detect errors in Huffman encoding
	final static byte UNFINISHED_ENCODING = -2; // used to detect internal Huffman nodes (i.e., not a leaf node and has no character)
	final static byte EOF = 4; // used to detect the end of a file compressed using Huffman's algorithm

	/**
	 * Generates a compressed version of the input data.
	 */
	public void compressData(InputStream input, OutputStream output) {
		symbols = new HashMap<>();
		data = new ArrayList<Byte>();

		try (DataOutputStream out = new DataOutputStream(output)) {
			// read characters and counts frequency of each
			readData(input);

			// build Huffman tree using frequency information
			root = createTrie(symbols);

			// write character and frequencies to beginning of compressed file
			writeEncodingInfo(out);

			// for each character in the input file, encodes it using the
			// Huffman tree
			// and writes the bit code
			try (BitOutputStream bout = new BitOutputStream(out)) {
				for (byte ch : data) {
					bout.writeBits(getCode(symbols, ch));
				}
			}
		} catch (IOException e) {
			System.err.println(e);
		}
	}

	/**
	 * Generates a decompressed version of the input file
	 * 
	 * @param input
	 *            - input file of (compressed) data
	 * @param output
	 *            - output file of decompressed data
	 */
	public void decompressData(InputStream input, OutputStream output) {
		symbols = new HashMap<>();
		data = new ArrayList<Byte>();

		try (DataInputStream in = new DataInputStream(input)) {
			// read characters and frequency information
			readEncodingInfo(in);
			// build Huffman tree using frequency information
			root = createTrie(symbols);
			// read Huffman codes corresponding to each character in original file
			try (BitInputStream bin = new BitInputStream(in)) {
				readCompressedData(bin);
			}

			// use the codes to find each character in the Huffman tree and print the characters
			for (int i : data) {
				output.write(i);
			}
		} catch (IOException e) {
			System.err.println(e);
		}
	}

	/**
	 * COMPRESSION
	 * 
	 * Reads all characters, while maintaining a count of the occurrences of
	 * each character
	 * 
	 * @param in
	 *            - stream for the input file
	 * @throws IOException
	 */
	private void readData(InputStream in) throws IOException {
		int readByte;
		// read characters until end of file
		while ((readByte = in.read()) != -1) {
			byte symbol  = (byte)(readByte & 0xFF); 
			// if character had not yet been seen, put it in the tree (weight = 1)
			if (symbols.get(symbol) == null) {
				symbols.put(symbol, new Node(symbol, 1));
			}
			// if character has already been seen, increment weight
			else {
				symbols.get(symbol).frequency++;

			}
			// keep track of all characters (in order)
			data.add(symbol);
		}

		// add end-of-file marker to tree and list of characters
		// this lets use know when to stop during decompression
		symbols.put(EOF, new Node(EOF, 1));
		data.add(EOF);
	}

	/**
	 * DECOMPRESSION
	 * 
	 * Reads the Huffman codes corresponding to each character in original file.
	 * 
	 * @param in
	 *            - stream for the input file
	 * @throws IOException
	 */
	private void readCompressedData(BitInputStream in) throws IOException {
		String bits = "";
		int bit;
		byte decode = 0;

		// read bits until end of file
		while ((bit = in.readBit()) != -1) {
			bits += bit == 0 ? "0" : "1";

			// follow the path in the Huffman tree indicated by the bit code and get
			// the character encoded
			decode = getChar(root, bits);

			// if the path leads to an internal node, not a complete code; get next bit
			if (decode == UNFINISHED_ENCODING) { 
				continue;
			} else if (decode == ERROR) {
				throw new IOException("Decoding error");
			} else if (decode == EOF) {
				return;
			}

			// keep track of each character decoded
			data.add(decode);
			bits = "";
		}
	}

	/**
	 * COMPRESSION
	 * 
	 * Writes the character and weight information to the output file, so that
	 * the Huffman tree can reconstructed at the time of decompression.
	 * 
	 * @param out
	 *            - stream for the output file
	 * @throws IOException
	 */
	private void writeEncodingInfo(DataOutputStream out) throws IOException {
		for (Entry<Byte, Node> e : symbols.entrySet()) {
			out.writeByte(e.getKey());
			out.writeInt(e.getValue().frequency);
		}

		// special code to indicate end of file
		out.writeByte(0);
		out.writeInt(0);
	}

	/**
	 * DECOMPRESSION
	 * 
	 * Reads the character and weight information at the beginning of the
	 * compressed file, so that the Huffman tree can reconstructed.
	 * 
	 * @param in
	 *            - stream for the input file
	 * @throws IOException
	 */
	private void readEncodingInfo(DataInputStream in) throws IOException {
		byte ch;
		int frequency;
		while (true) {
			ch = in.readByte();
			frequency = in.readInt();
			if (frequency == 0) { // EOF
				return;
			}
			symbols.put(ch, new Node(ch, frequency));
		}
	}

	/**
	 * COMPRESSION and DECOMPRESSION
	 * Constructs a Huffman tree to represent bit codes for each character. (See
	 * algorithm and examples in Lecture 22.)
	 */
	Node createTrie(Map<Byte, Node> symbols) {
		PriorityQueue<Node> pq = new PriorityQueue<Node>();
		pq.addAll(symbols.values());
		while (pq.size() > 1) {
			Node left = pq.poll(), right = pq.poll();
			Node parent = new Node(left, right);
			
			left.parent = parent;
			right.parent = parent;
			
			pq.offer(parent);
		}
		
		return pq.poll();
	}

	/**
	 * COMPRESSION
	 * 
	 * Returns the bit code for a character, by traversing the path from the
	 * character's leaf node up to the root of the tree. Encountering a left
	 * child causes a 0 to be pre-appended to the bit code, and encountering a
	 * right child causes a 1 to be pre-appended.
	 * 
	 * @param character
	 *            - character to be encoded
	 */
	
	byte[] getCode(Map<Byte, Node> symbols, byte character) {
		ArrayList<Integer> bits = new ArrayList<Integer>();
		Node cursor = symbols.get(character);
		while (cursor.parent != null) {
			if (cursor.parent.leftChild == cursor) {
				bits.add(0, 0);
			}
			else {
				bits.add(0, 1);
			}
			cursor = cursor.parent;
		}
		
		byte[] code = new byte[bits.size()];
		for (int idx = 0; idx < code.length; idx++) {
			code[idx] = (byte) bits.get(idx).byteValue();
		}
		return code;
	}

	/**
	 * DECOMPRESSION
	 * 
	 * Returns the character for the bit code, by traversing the path from the
	 * root of the tree to the character's leaf node. A 0 in the code causes the
	 * path to go through a left child, and a 1 in the code causes the path to
	 * go through a right child.
	 * 
	 * @param code
	 *            - the bit code indicating the path from the root
	 * @return the character encoded or ERROR if the path is not valid
	 */
	byte getChar(Node root, String code) {
		if (root == null) {
			return ERROR;
		}
		
		Node cursor = root;
		for (int ind = 0; ind < code.length(); ind++) {
			char currChar = code.charAt(ind);
			
			if (currChar == '0') {
				cursor = cursor.leftChild;
			}
			else {
				cursor = cursor.rightChild;
			}
			
			if (cursor == null) {
				return ERROR;
			}
		}
		
		return cursor.symbol;
	}

	/**
	 * Generates a DOT file for visualizing the Huffman tree.
	 * 
	 * @param dotFilename
	 *            - filename of DOT file to generate
	 */
	public void huffmanToDot(String dotFilename) {
		try (PrintWriter out = new PrintWriter(dotFilename)) {
			out.println("graph Tree {\n\tnode [shape=record]\n");

			if (root == null)
				out.println("");
			else
				out.print(root.generateDot());

			out.println("}");
		} catch (IOException e) {
			System.out.println(e);
		}
	}
}