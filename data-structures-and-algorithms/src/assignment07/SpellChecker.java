package assignment07;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.Scanner;

/**
 * A class for spell-checking text corpus.
 * 
 * A list of words and a binary search tree are used internally to
 * cross-reference words.
 * 
 * @author Maxwell Hanson
 * @since Mar 6, 2018
 * @version 1.1.0
 */
public class SpellChecker {
	/**
	 * Set of words cross-reference the corpus against.
	 */
	private BinarySearchTree<String> dictionary;

	/**
	 * Construct a spell-checker with no dictionary.
	 */
	public SpellChecker() {
		dictionary = new BinarySearchTree<String>();
	}

	/**
	 * Construct a spell-checker with a list of words.
	 * 
	 * @param words -- the List of Strings used to build the dictionary
	 */
	public SpellChecker(List<String> words) {
		this();
		buildDictionary(words);
	}

	/**
	 * Construct a spell-checker with a file of words.
	 * 
	 * If file does not exist, an empty dictionary is created.
	 * 
	 * @param dictionary_file -- the File that contains Strings used to build the dictionary
	 */
	public SpellChecker(File dictionaryFile) {
		this();
		buildDictionary(readFromFile(dictionaryFile));
	}

	/**
	 * Add a word to the dictionary.
	 * 
	 * @param word -- the String to be added to the dictionary
	 */
	public void addToDictionary(String word) {
		dictionary.add(word.toLowerCase());
	}

	/**
	 * Remove a word from the dictionary.
	 * 
	 * @param word -- the String to be removed from the dictionary
	 */
	public void removeFromDictionary(String word) {
		dictionary.remove(word.toLowerCase());
	}

	/**
	 * Spell-checks a document (file) against the dictionary.
	 * 
	 * @param document_file -- the File that contains Strings to be looked up in the dictionary
	 * @return -- a List of misspelled words
	 */
	public List<String> spellCheck(File documentFile) {
		List<String> wordsToCheck = readFromFile(documentFile);
		List<String> wordsNotInDict = new LinkedList<String>();

		for (String word: wordsToCheck) {
			if (!dictionary.contains(word.toLowerCase())) {
				wordsNotInDict.add(word);
			}
		}
		
		return wordsNotInDict;
	}

	/**
	 * Fills in the dictionary with the input list of words.
	 * 
	 * @param words -- the List of Strings to be added to the dictionary
	 */
	private void buildDictionary(List<String> words) {
		for (String word: words) {
			addToDictionary(word);
		}
	}

	/**
	 * Tokenize a file into a list of words (strings).
	 * 
	 * @param file -- the File to be read
	 * @return -- a List of the Strings in the input file
	 */
	private List<String> readFromFile(File file) {
		ArrayList<String> words = new ArrayList<String>();
		
		try (Scanner fileInput = new Scanner(file)) {
			/*
			 * Java's Scanner class is a simple lexer for Strings and primitive
			 * types (see the Java API, if you are unfamiliar).
			 */

			/*
			 * The scanner can be directed how to delimit (or divide) the input.
			 * By default, it uses whitespace as the delimiter. The following
			 * statement specifies anything other than alphabetic characters as
			 * a delimiter (so that punctuation and such will be ignored). The
			 * string argument is a regular expression that specifies "anything
			 * but an alphabetic character". You need not understand any of this
			 * for the assignment.
			 */
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
