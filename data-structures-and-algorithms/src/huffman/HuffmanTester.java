package huffman;

import static huffman.HuffmanTree.UNFINISHED_ENCODING;
import static huffman.HuffmanTree.ERROR;
import static org.junit.Assert.assertArrayEquals;
import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertSame;

import java.util.HashMap;
import java.util.Map;

import org.junit.Before;
import org.junit.Test;

import huffman.HuffmanTree.Node;

public class HuffmanTester {
	
	private HuffmanTree huffman;

	@Before
	public void	 setup() {
		huffman = new HuffmanTree();
	}
	
	@Test
	public void createTree() {
		Map<Byte, Node> symbols = buildHistogramFor("enterprise");
		Node root = huffman.createTrie(symbols);
		assertEquals("Root's frequency incorrect",10, root.frequency);
		assertEquals("Root's symbol incorrect", -2, root.symbol);
		assertEquals("Left child incorrect", 4, root.leftChild.frequency);
		assertEquals("Right child incorrect", 6, root.rightChild.frequency);
		assertEquals("Leftmost child is P", 112, getLeftmostSymbol(root));
		assertEquals("rightmost child is n", 110, getRightmostSymbol(root));
		assertSame("RightChild has correct parent", root, root.rightChild.parent);
		assertSame("LeftChild has correct parent", root, root.leftChild.parent);
		
		symbols = buildHistogramFor("aaaaaaa\n");
		root = huffman.createTrie(symbols);
		assertEquals("Wrong root frequency", 8, root.frequency);
		assertEquals("Wrong left symbol", 10, root.leftChild.symbol);
		assertEquals("Wrong right symbol", 97, root.rightChild.symbol);
		
		symbols = buildHistogramFor("Data Structures and Algorithms");
		root = huffman.createTrie(symbols);
		assertEquals("Wrong root frequency", 30, root.frequency);
		assertEquals("Wrong leftmost symbol", 97, getLeftmostSymbol(root));
		assertEquals("Wrong rightmost symbol", 32, getRightmostSymbol(root));
	}
	
	@Test
	public void getCode() {
		Map<Byte, Node> symbols = buildSymbolMap(handcraftedEnterpriseTree());
		assertArrayEquals(new byte[]{0, 0, 0}, huffman.getCode(symbols, (byte)'p'));
		assertArrayEquals(new byte[]{1, 0}, huffman.getCode(symbols, (byte)'e'));
		assertArrayEquals(new byte[]{1, 1, 1, 1}, huffman.getCode(symbols, (byte)'n'));
		assertArrayEquals(new byte[]{1, 1, 1, 0}, huffman.getCode(symbols, (byte)'i'));
		assertArrayEquals(new byte[]{0, 1}, huffman.getCode(symbols, (byte)'r'));
		assertArrayEquals(new byte[]{1, 1, 0}, huffman.getCode(symbols, (byte)'t'));
		assertArrayEquals(new byte[]{0, 0, 1}, huffman.getCode(symbols, (byte)'s'));
	}
	
	@Test
	public void getChar() {
		Node root = handcraftedEnterpriseTree();
		assertEquals(huffman.getChar(root, "1"), UNFINISHED_ENCODING);
		assertEquals(huffman.getChar(root, "11"), UNFINISHED_ENCODING);
		assertEquals(huffman.getChar(root, "0"), UNFINISHED_ENCODING);
		
		assertEquals((byte)'p', huffman.getChar(root, "000"));
		assertEquals((byte)'e', huffman.getChar(root, "10"));
		assertEquals((byte)'n', huffman.getChar(root, "1111"));
		assertEquals((byte)'i', huffman.getChar(root, "1110"));
		assertEquals((byte)'r', huffman.getChar(root, "01"));
		assertEquals((byte)'t', huffman.getChar(root, "110"));
		assertEquals((byte)'s', huffman.getChar(root, "001"));
		
		assertEquals("If we reach some bit encoding that doesn't match the tree", ERROR, huffman.getChar(root, "111111"));
		
	}
	

	private Map<Byte, Node> buildSymbolMap(Node root) {
		Map<Byte, Node> symbols = new HashMap<>();
		traverse(root, symbols);
		return symbols;
	}

	private void traverse(Node root, Map<Byte, Node> symbols) {
		if(root == null) {
			return;
		}
		if(root.symbol != UNFINISHED_ENCODING) {
			symbols.put(root.symbol, root);
		}
		traverse(root.leftChild, symbols);
		traverse(root.rightChild, symbols);
	}

	private byte getRightmostSymbol(Node root) {
		Node cursor = root;
		while(cursor.rightChild != null) {
			cursor = cursor.rightChild;
		}
		return cursor.symbol;
	}
	
	private byte getLeftmostSymbol(Node root) {
		Node cursor = root;
		while(cursor.leftChild != null) {
			cursor = cursor.leftChild;
		}
		return cursor.symbol;
	}

	private Map<Byte, Node> buildHistogramFor(String string) {
		Map<Byte, Node> symbols = new HashMap<>();
		for(char letter : string.toCharArray()) {
			byte symbol  = (byte)(letter & 0xFF); 
			if (symbols.get(symbol) == null) {
				symbols.put(symbol, new Node(symbol, 1));
			}
			else {
				symbols.get(symbol).frequency++;
			}
		}
		return symbols;
	}
	
	private Node handcraftedEnterpriseTree() {
		Node root = new Node(UNFINISHED_ENCODING, 10);
		createLeftChild(root, UNFINISHED_ENCODING, 4);
		  createLeftChild(root.leftChild, UNFINISHED_ENCODING, 2);
		    createLeftChild(root.leftChild.leftChild, (byte)'p', 1);
		    createRightChild(root.leftChild.leftChild, (byte)'s', 1);
		  createRightChild(root.leftChild, (byte)'r', 2);
		createRightChild(root, UNFINISHED_ENCODING, 6);
		  createLeftChild(root.rightChild, (byte)'e', 3);
		  createRightChild(root.rightChild, UNFINISHED_ENCODING, 3);
		    createLeftChild(root.rightChild.rightChild, (byte)'t', 1);
		    createRightChild(root.rightChild.rightChild, UNFINISHED_ENCODING, 2);
		      createLeftChild(root.rightChild.rightChild.rightChild, (byte)'i', 1);
		      createRightChild(root.rightChild.rightChild.rightChild, (byte)'n', 1);
		return root;
	}
	
	private Node createLeftChild(Node parent, byte symbol, int frequency) {
		parent.leftChild = new Node(symbol, frequency);
		parent.leftChild.parent = parent;
		return parent.leftChild;
	}
	
	private Node createRightChild(Node parent, byte symbol, int frequency) {
		parent.rightChild = new Node(symbol, frequency);
		parent.rightChild.parent = parent;
		return parent.rightChild;
	}
}
