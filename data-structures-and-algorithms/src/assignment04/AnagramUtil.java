package assignment04;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.HashMap;
import java.util.Scanner;

/**
 * A collection of utilities related to anagrams.
 * 
 * @author Maxwell Hanson
 * @version 1.1.0
 * @since Feb 9, 2018
 */
public class AnagramUtil {
	/**
	 * Sort a string lexicographically.
	 * 
	 * Sorting is accomplished with insertion sort.
	 * 
	 * @param str -- string to sort
	 * @return -- sorted string
	 */
	public static String sort(String str) {
		Character[] strArr = stringToArray(str);
		insertionSort(strArr, (lhs, rhs) -> lhs.compareTo(rhs));

		return arrayToString(strArr);
	}

	/**
	 * Sort an array with a comparator.
	 * 
	 * Sorting is accomplished with an insertion sort algorithm.
	 * 
	 * @param arr -- array to sort
	 * @param comp -- comparator to sort the array with
	 * @param <T> -- the type of element in the array
	 */
	public static <T> void insertionSort(T[] arr, Comparator<? super T> comp) {
		// sort the array with insertion sort
		for (int unsorted = 1; unsorted < arr.length; unsorted++) {
			T unsortedElement = arr[unsorted];
			int sorted = unsorted - 1;
			while (sorted >= 0 && comp.compare(arr[sorted], unsortedElement) > 0) {
				arr[sorted + 1] = arr[sorted];
				sorted--;
			}
			arr[sorted + 1] = unsortedElement;
		}
	}

	/**
	 * Determine if two strings are anagrams.
	 * 
	 * @param str0 -- first anagram candidate to check
	 * @param str1 -- second anagram candidate to check
	 * @return -- true if the strings are anagrams, false otherwise
	 */
	public static boolean areAnagrams(String str0, String str1) {
		if (sort(str0).equals(sort(str1))) {
			return true;
		}
		return false;
	}

	/**
	 * Find the largest group of anagrams in the input array.
	 * 
	 * The returned group of anagrams is not ordered.
	 * An empty array is returned if there are no anagrams in the array.
	 * It is assumed that there is only one largest group of anagrams. If there are more,
	 *   an arbitrary group is chosen.
	 * The input array is not guaranteed to be maintained.
	 * 
	 * @param wordSet -- the set of words to check for the largest group of anagrams within
	 * @return -- an array containing the largest group of anagrams, see above stipulations
	 */
	public static String[] getLargestAnagramGroup(String[] wordSet) {
		String[] largestAnagramGroup = new String[] {};

		// clone and sort (each entry in) the parameter
		String[] sortedWordSet = wordSet.clone();
		for (int curs = 0; curs < sortedWordSet.length; curs++) {
			sortedWordSet[curs] = sort(sortedWordSet[curs]);
		}

		/// find group of largest anagrams
		// Each key in the hashmap is a sorted string, each value is
		// how many times the string occurs in the wordSetClone array.
		HashMap<String, Integer> freqMap = new HashMap<String, Integer>();
		for (String sortedWord : sortedWordSet) {
			// if the word has no key, create one
			if (freqMap.get(sortedWord) == null) {
				freqMap.put(sortedWord, 1);
			}
			// if word has key, increment value
			else {
				Integer currValue = freqMap.get(sortedWord);
				freqMap.replace(sortedWord, ++currValue);
			}
		}

		// optimize the frequency map
		String maxKey = "";
		for (String key: freqMap.keySet()) {
			Integer iterVal = freqMap.get(key);
			Integer maxVal = freqMap.get(maxKey);
			
			// to avoid npe with initial maxKey of empty string
			if (maxVal == null) {
				maxVal = 0;
			}
			
			if (iterVal > maxVal) {
				maxKey = key;
			}
		}

		// if optimization is 1, no anagrams
		if (freqMap.get(maxKey) == 1) {
			return largestAnagramGroup;
		}

		// find all anagrams of maxKey (in orig array) and add to largestAnagramGroup
		largestAnagramGroup = new String[freqMap.get(maxKey)];
		// setIdx iterates over wordSet and groupIdx iterates of largestAnagramGroup
		for (int setIdx = 0, groupIdx = 0; setIdx < wordSet.length; setIdx++) {
			if (areAnagrams(wordSet[setIdx], maxKey)) {
				largestAnagramGroup[groupIdx] = wordSet[setIdx];
				groupIdx++; // only needs to be incremented when anagram is found
			}
		}

		return largestAnagramGroup;
	}

	/**
	 * Find the largest group of anagrams in a scanner.
	 * 
	 * It is assumed that the file contains one word per line.
	 * If the file is empty, then it returns an empty array.
	 * 
	 * @param scanner -- the scanner to read words from
	 * @return -- the largest group of anagrams in the scanner
	 */
	public static String[] getLargestAnagramGroup(Scanner scanner) {
		if (!scanner.hasNext())
			return new String[] {};
		String[] scannerArray = scannerToArray(scanner);

		return getLargestAnagramGroup(scannerArray);
	}

	/**
	 * Converts a string to an array of characters
	 * 
	 * @param str -- string to convert to an array
	 * @return -- array representation of the string
	 */
	private static Character[] stringToArray(String str) {
		str = str.toLowerCase();
		Character[] result = new Character[str.length()];
		for (int curs = 0; curs < str.length(); curs++) {
			result[curs] = str.charAt(curs);
		}
		
		return result;
	}

	/**
	 * Convert an array of characters into a string.
	 * 
	 * @param arr -- array to convert to a string
	 * @return -- a string representation of the array
	 */
	private static String arrayToString(Character[] arr) {
		String result = "";
		for (int curs = 0; curs < arr.length; curs++) {
			result += arr[curs];
		}
		
		return result;
	}

	/**
	 * Convert the contents of a scanner to an array.
	 * 
	 * @param scanner -- the scanner to convert
	 * @return -- array representation of the scanner
	 */
	private static String[] scannerToArray(Scanner scanner) {
		ArrayList<String> tempStorage = new ArrayList<>();
		while (scanner.hasNext()) {
			tempStorage.add(scanner.next());
		}
		String[] result = new String[tempStorage.size()];
		for (int curs = 0; curs < tempStorage.size(); curs++) {
			result[curs] = tempStorage.get(curs);
		}
		
		return result;
	}
}
