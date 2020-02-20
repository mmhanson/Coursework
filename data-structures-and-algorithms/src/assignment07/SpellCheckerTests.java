package assignment07;

import static org.junit.Assert.assertTrue;
import static org.junit.Assert.fail;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.Scanner;

import org.junit.Before;
import org.junit.Test;

/**
 * Automated tests for the SpellChecker class.
 * 
 * @author Maxwell Hanson
 * @version 1.1.0
 * @since Mar 6, 2018
 */
public class SpellCheckerTests {
	File emptyDoc;
	File oneLineDoc;
	File oneWordDoc;
	File manyLineDoc;
	File nonExistentFile;
	File dictionary;

	/**
	 * Setup attributes for testing.
	 */
	@Before
	public void setup() {
		emptyDoc = new File("");
		oneLineDoc = new File("hello_world.txt");
		oneWordDoc = new File("hello.txt");
		manyLineDoc = new File("good_luck.txt");
		nonExistentFile = new File("this_file_does_not_exist.txt");
		dictionary = new File("dictionary.txt");
	}
	
	/**
	 * Test the default constructor of the SpellChecker class.
	 * 
	 * Test that the dictionary is empty.
	 *   i.e. Test that calling SpellCheck on a document returns
	 *   a list of all the words in the document.
	 * Test the above test for an empty document, a document of one word, and a
	 *   document of many words on one line, and a document of many words on
	 *   multiple lines.
	 */
	@Test
	public void testDefaultConstructor() {
		SpellChecker sc = new SpellChecker();
		List<String> docWordList, scWordList;
		
		/// test dictionary is empty
		// empty file
		docWordList = readFromFile(emptyDoc);
		scWordList = sc.spellCheck(emptyDoc);
		assertTrue(docWordList.equals(scWordList));
		// one word
		docWordList = readFromFile(oneWordDoc);
		scWordList = sc.spellCheck(oneWordDoc);
		assertTrue(docWordList.equals(scWordList));
		// many word, one line
		docWordList = readFromFile(oneLineDoc);
		scWordList = sc.spellCheck(oneLineDoc);
		assertTrue(docWordList.equals(scWordList));
		// many word, many line
		docWordList = readFromFile(manyLineDoc);
		scWordList = sc.spellCheck(manyLineDoc);
		assertTrue(docWordList.equals(scWordList));
	}
	
	/**
	 * Test the list constructor of the SpellChecker class.
	 * 
	 * Test that supplying an empty list results in checking documents
	 *   returning a list of all words in the document.
	 * Test that supplying one word results in checking documents returning
	 *   all words in the document except that word.
	 * Test that supplying the entire dictionary results in checking documents
	 *   returning no words (if doc is clean).
	 * Test that supplying the entire dictionary results in checking documents
	 *   returning mispelled words (if doc is dirty).
	 * Test a null parameter, should throw NPE.
	 */
	@Test
	public void testListConstructor() {
		SpellChecker sc;
		List<String> docWordList, scWordList;
		List<String> dict;
		
		/// empty list
		sc = new SpellChecker(new LinkedList<String>());
		// empty file
		docWordList = readFromFile(emptyDoc);
		scWordList = sc.spellCheck(emptyDoc);
		assertTrue(docWordList.equals(scWordList));
		// one word
		docWordList = readFromFile(oneWordDoc);
		scWordList = sc.spellCheck(oneWordDoc);
		assertTrue(docWordList.equals(scWordList));
		// many word, one line
		docWordList = readFromFile(oneLineDoc);
		scWordList = sc.spellCheck(oneLineDoc);
		assertTrue(docWordList.equals(scWordList));
		// many word, many line
		docWordList = readFromFile(manyLineDoc);
		scWordList = sc.spellCheck(manyLineDoc);
		assertTrue(docWordList.equals(scWordList));
		
		/// one word
		dict = new LinkedList<String>();
		dict.add("hello");
		sc = new SpellChecker(dict);
		// emtpy file
		docWordList = readFromFile(emptyDoc);
		scWordList = sc.spellCheck(emptyDoc);
		assertTrue(docWordList.equals(scWordList));
		// one word
		docWordList = readFromFile(oneWordDoc);
		scWordList = sc.spellCheck(oneWordDoc);
		docWordList.remove("hello");
		assertTrue(docWordList.equals(scWordList));
		// many word, one line
		docWordList = readFromFile(oneLineDoc);
		scWordList = sc.spellCheck(oneLineDoc);
		docWordList.remove("hello");
		assertTrue(docWordList.equals(scWordList));
		// many word, many line
		docWordList = readFromFile(manyLineDoc);
		scWordList = sc.spellCheck(manyLineDoc);
		docWordList.remove("hello");
		assertTrue(docWordList.equals(scWordList));
		
		/// dictionary, clean docs
		dict = readFromFile(dictionary);
		sc = new SpellChecker(dict);
		// emtpy file
		docWordList = readFromFile(emptyDoc);
		scWordList = sc.spellCheck(emptyDoc);
		assertTrue(docWordList.equals(scWordList));
		// one word
		scWordList = sc.spellCheck(oneWordDoc);
		assertTrue(docWordList.isEmpty());
		// many word, one line
		scWordList = sc.spellCheck(oneLineDoc);
		assertTrue(docWordList.isEmpty());
		
		/// dictionary, dirty docs
		// many word, many line
		scWordList = sc.spellCheck(manyLineDoc);
		assertTrue(docWordList.size() == 1);
		assertTrue(docWordList.get(0).equals("BST"));
		
		/// null
		try {
			sc.spellCheck(null);
			fail ("Successfully parameterized null to spellCheck method.");
		}
		catch (NullPointerException e) {}
	}
	
	/**
	 * Test the file constructor of the SpellChecker class.
	 * 
	 * Test a nonexistent file, dictionary should be empty.
	 * Test an empty file, should result in checking doucments returning a list
	 *   of all the words in the document.
	 * Test a file with on word, should result in checking documents returning a list
	 *   of all words except that word.
	 * Test supplying the entire dictionary results in checking documents returning
	 *   an empty list (if doc is clean), and a list of misspelled words (if doc is dirty).
	 * Test null, should throw NPE.
	 */
	@Test
	public void testFileConstructor() {
		SpellChecker sc;
		List<String> docWordList, scWordList;
		
		/// nonexistent file
		sc = new SpellChecker(nonExistentFile);
		docWordList = readFromFile(emptyDoc);
		scWordList = sc.spellCheck(emptyDoc);
		assertTrue(docWordList.equals(scWordList));
		// one word
		docWordList = readFromFile(oneWordDoc);
		scWordList = sc.spellCheck(oneWordDoc);
		assertTrue(docWordList.equals(scWordList));
		// many word, one line
		docWordList = readFromFile(oneLineDoc);
		scWordList = sc.spellCheck(oneLineDoc);
		assertTrue(docWordList.equals(scWordList));
		// many word, many line
		docWordList = readFromFile(manyLineDoc);
		scWordList = sc.spellCheck(manyLineDoc);
		assertTrue(docWordList.equals(scWordList));
		
		/// empty file
		sc = new SpellChecker(emptyDoc);
		docWordList = readFromFile(emptyDoc);
		scWordList = sc.spellCheck(emptyDoc);
		assertTrue(docWordList.equals(scWordList));
		// one word
		docWordList = readFromFile(oneWordDoc);
		scWordList = sc.spellCheck(oneWordDoc);
		assertTrue(docWordList.equals(scWordList));
		// many word, one line
		docWordList = readFromFile(oneLineDoc);
		scWordList = sc.spellCheck(oneLineDoc);
		assertTrue(docWordList.equals(scWordList));
		// many word, many line
		docWordList = readFromFile(manyLineDoc);
		scWordList = sc.spellCheck(manyLineDoc);
		assertTrue(docWordList.equals(scWordList));
		
		/// file of one word
		sc = new SpellChecker(oneWordDoc);
		// emtpy file
		docWordList = readFromFile(emptyDoc);
		scWordList = sc.spellCheck(emptyDoc);
		assertTrue(docWordList.equals(scWordList));
		// one word
		docWordList = readFromFile(oneWordDoc);
		scWordList = sc.spellCheck(oneWordDoc);
		docWordList.remove("hello");
		assertTrue(docWordList.equals(scWordList));
		// many word, one line
		docWordList = readFromFile(oneLineDoc);
		scWordList = sc.spellCheck(oneLineDoc);
		docWordList.remove("hello");
		assertTrue(docWordList.equals(scWordList));
		// many word, many line
		docWordList = readFromFile(manyLineDoc);
		scWordList = sc.spellCheck(manyLineDoc);
		docWordList.remove("hello");
		assertTrue(docWordList.equals(scWordList));
		
		/// dictionary, clean docs
		sc = new SpellChecker(dictionary);
		// emtpy file
		docWordList = readFromFile(emptyDoc);
		scWordList = sc.spellCheck(emptyDoc);
		assertTrue(docWordList.equals(scWordList));
		// one word
		scWordList = sc.spellCheck(oneWordDoc);
		assertTrue(docWordList.isEmpty());
		// many word, one line
		scWordList = sc.spellCheck(oneLineDoc);
		assertTrue(docWordList.isEmpty());
		
		/// dictionary, dirty docs
		scWordList = sc.spellCheck(manyLineDoc);
		assertTrue(docWordList.size() == 1);
		assertTrue(docWordList.get(0).equals("BST"));
		
		/// null
		try {
			sc.spellCheck(null);
			fail ("Successfully parameterized null to spellCheck method.");
		}
		catch (NullPointerException e) {}
	}
	
	/**
	 * Test the "addToDictionary" method of the SpellChecker class.
	 * 
	 * Test that if a word is returned on a spellcheck call, adding it to the dictionary
	 *   results in it not being returned.
	 */
	@Test
	public void testAddToDictionary() {
		SpellChecker sc;
		List<String> docWordList;
		
		/// one word doc
		sc = new SpellChecker();
		docWordList = sc.spellCheck(oneWordDoc);
		assertTrue(docWordList.contains("hello"));
		sc.addToDictionary("hello");
		docWordList = sc.spellCheck(oneWordDoc);
		assertTrue(!docWordList.contains("hello"));
		
		/// many word, one line
		sc = new SpellChecker();
		docWordList = sc.spellCheck(oneLineDoc);
		assertTrue(docWordList.contains("hello"));
		sc.addToDictionary("hello");
		docWordList = sc.spellCheck(oneLineDoc);
		assertTrue(!docWordList.contains("hello"));
	}
	
	/**
	 * Test the "removeFromDictionary" method of the SpellChecker class.
	 * 
	 * Test that if a document returns no words on a spellcheck call, removing a word
	 *   from the dictionary that is in the document results in the call returning a
	 *   list containing only that word.
	 */
	@Test
	public void testRemoveFromDictionary() {
		SpellChecker sc;
		List<String> docWordList;
		
		/// one word doc
		sc = new SpellChecker(dictionary);
		docWordList = sc.spellCheck(oneWordDoc);
		assertTrue(docWordList.isEmpty());
		sc.removeFromDictionary("hello");
		docWordList = sc.spellCheck(oneWordDoc);
		assertTrue(docWordList.contains("hello"));
		
		/// many word, one line
		sc = new SpellChecker(dictionary);
		docWordList = sc.spellCheck(oneLineDoc);
		assertTrue(docWordList.isEmpty());
		sc.removeFromDictionary("hello");
		docWordList = sc.spellCheck(oneLineDoc);
		assertTrue(docWordList.contains("hello"));
	}
	
	/**
	 * Returns a list of the words contained in the specified file. (Note that
	 * symbols, digits, and spaces are ignored.)
	 * 
	 * @param file -- the File to be read
	 * @return a List of the Strings in the input file
	 */
	private List<String> readFromFile(File file) {
		ArrayList<String> words = new ArrayList<String>();
		
		try (Scanner fileInput = new Scanner(file)) {
			fileInput.useDelimiter("\\s*[^a-zA-Z]\\s*");

			while (fileInput.hasNext()) {
				String s = fileInput.next();
				if (!s.equals("")) {
					words.add(s.toLowerCase());
				}
			}

		} catch (FileNotFoundException e) {
			System.err.println("File " + file + " cannot be found.");
		}
		return words;
	}
}
