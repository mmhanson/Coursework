package assignment05;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;

/**
 * A collection of sorting utilities.
 * 
 * @author Maxwell Hanson
 * @since Jul 13, 2018
 * @version 1.1.0
 */
public class SortUtil {
	/**
	 * Sort an array with mergesort.
	 * 
	 * Sorting is done with the mergesort algorithm. Insertionsort is used once the recursion
	 * reaches a certain depth since insertionsort is quicker on smaller arrays.
	 * 
	 * @param array -- array to sort
	 * @param comp-- definition of sorting to use
	 */
	public static <T> void mergesort(ArrayList<T> array, Comparator<? super T> comp) {
		if (array == null || comp == null || array.isEmpty()) {
			return;
		}
		
		Mergesort.mergesortRecursive(array, comp, 0, array.size() - 1);
	}

	/**
	 * Sort an array with quicksort.
	 * 
	 * Sorting is done witht he quicksort algorithm. Insertionsort is used once the recursion
	 * reaches a certain depth since insertionsort is quicker on smaller arrays.
	 * 
	 * @param array -- array to sort
	 * @param comp -- definition of sorting to use
	 */
	public static <T> void quicksort(ArrayList<T> array, Comparator<? super T> comp) {
		if (array == null || comp == null || array.isEmpty()) {
			return;
		}
		
		Quicksort.quicksort(array, comp);
	}

	/**
	 * Generate an ascending list.
	 * 
	 * Ascending numbers start at one. Each entry is one more than the previous.
	 * 
	 * @param size -- size of list to be generated
	 * @return -- ascending list
	 */
	public static ArrayList<Integer> generateAscending(int size) {
		ArrayList<Integer> ascList = new ArrayList<>();
		
		if (size < 1) {
			return ascList;
		}
		
		for (int idx = 1; idx < size + 1; idx++) {
			ascList.add(idx);
		}
		
		return ascList;
	}

	/**
	 * Generate a random list of all integers 1 .. size
	 * 
	 * @param size -- size of ArrayList to be returned
	 * @return -- rnadom list of all integers 1..size
	 */
	public static ArrayList<Integer> generateShuffled(int size) {
		ArrayList<Integer> result = generateAscending(size);
		Collections.shuffle(result);
		
		return result;
	}

	/**
	 * Generate a descending list.
	 * 
	 * Last entry of list is 1. Each entry is one less than the previous.
	 * 
	 * @param size -- size of ArrayList to be returned
	 * @return -- descending list
	 */
	public static ArrayList<Integer> generateDescending(int size) {
		ArrayList<Integer> result = new ArrayList<>();
		
		if (size < 1) {
			return result;
		}
		
		for (int i = size; i > 0; i--) {
			result.add(i);
		}
		
		return result;
	}

}
