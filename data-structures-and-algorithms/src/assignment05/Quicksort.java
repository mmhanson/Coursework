package assignment05;

import java.util.ArrayList;
import java.util.Comparator;

/**
 * Sort an ArrayList with quicksort.
 * 
 * @author Maxwell Hanson
 * @since Jul 13, 2018
 * @version 1.1.0
 */
public class Quicksort {
	/**
	 * Theshold that determines when to switch from quicksort to insertion-sort.
	 * 
	 * When the arrays are divided into sub-arrays of this length or smaller, then
	 * insertion sort is used. The idea is that insertion sort is more efficient on
	 * smaller arrays than quicksort.
	 */
	private static int INSERTIONSORT_THRESHOLD = 450;
	
	/**
	 * The pivot type used in the quicksort algorithm. 
	 * 
	 * Modify this variable to change how the class picks the pivot.
	 * Possible values are 0, 1, and 2.
	 * 
	 * 0: pivot is always median
	 * 1: pivot is always the first element
	 * 2: pivot is always the last element
	 */
	private static int PIVOT_TYPE = 0;
	
	/**
	 * Sort an array with quicksort.
	 * 
	 * @param masterArr -- the array to sort
	 * @param comp -- the the definition of the sorting
	 * @param pivotType -- how pivots will be chosen. Can be 0, 1, or 2
	 */
	public static <T> void quicksort(ArrayList<T> masterArr, Comparator<? super T> comp) {
		// only driver method for recursive method
		quicksortRecursive(masterArr, comp, 0, masterArr.size() - 1);
	}
	
	/**
	 * Recursively sort a subarray with quicksort.
	 * 
	 * @param masterArr -- master array to sort
	 * @param comp -- definintion of sorting
	 * @param lIdx -- lower-bound of the sub-array to sort (inclusive)
	 * @param rIdx -- upper-bound of the sub-array to sort (inclusive)
	 */
	private static <T> void quicksortRecursive(ArrayList<T> masterArr, Comparator<? super T> comp, int lIdx, int rIdx) {
		// base case is when the sub-array is less than two elements
		if (rIdx <= (lIdx)) {
			return;
		}
		
		// use insertionsort on the subarray if it is sufficiently small
		if (masterArr.size() <= INSERTIONSORT_THRESHOLD) {
			insertionsort(masterArr, comp, lIdx, rIdx);
		}
		
		/// sort the sub-array
		int pivotIndex = partition(masterArr, comp, lIdx, rIdx);
		quicksortRecursive(masterArr, comp, lIdx, pivotIndex - 1);
		quicksortRecursive(masterArr, comp, pivotIndex + 1, rIdx);
		
	}
	
	/**
	 * Partition a subarray.
	 * 
	 * The subarray is organized such that all elements less than the pivot are to its left
	 * and all elements greater than the pivot are to its right.
	 * 
	 * @param masterArr -- master array containing the subarray to partition
	 * @param comp --  definition of sorting to use
	 * @param lIdx -- lower bound of the subarray to sort (inclusive).
	 * @param rIdx -- higher bound of the subarray to sort (inclusive).
	 * @return -- index of the pivot after the list has been organized
	 */
	private static <T> int partition(ArrayList<T> masterArr, Comparator<? super T> comp, int lIdx, int rIdx) {
		int pivotIndex = getPivotIndex(masterArr, lIdx, rIdx);
		
		// move pivot to the end
		swap(masterArr, pivotIndex, rIdx);
		
		// move from the ends of the array inward, swapping all elements that are on the wrong side
		int leftIndex = lIdx;
		int rightIndex = rIdx - 1;
		
		while (leftIndex <= rightIndex) {
			// put leftIndex at the next highest element that is greater than the pivot
			while ( leftIndex < rIdx && comp.compare(masterArr.get(leftIndex), masterArr.get(rIdx)) <= 0 ) {
				leftIndex++;
			}
			
			// put rightIndex at the next lowest element that is less than the pivot
			while ( rightIndex > lIdx && comp.compare(masterArr.get(rightIndex), masterArr.get(rIdx)) >= 0 ) {
				rightIndex--;
			}
			
			// swap them if the indices are not crossed
			if (leftIndex <= rightIndex) {
				swap(masterArr, leftIndex, rightIndex);
				rightIndex--;
			}
		}
		
		// put pivot back in correct position
		swap(masterArr, leftIndex, rIdx);
		
		// return new pivot index
		return leftIndex;
	}
	
	/**
	 * Swap two elements in a list.
	 * 
	 * @param array -- the list in which the elements to swap are contained
	 * @param index0 -- the index of the first element to swap
	 * @param index1 -- the index of the second element to swap
	 */
	@SuppressWarnings("unchecked")
	private static <T> void swap(ArrayList<T> array, int index0, int index1) {
		Object buffer = new Object();
		buffer = array.get(index0);
		array.set(index0, array.get(index1));
		array.set(index1, (T)buffer);
	}
	
	/**
	 * Get the index of a pivot of a sub-array.
	 * 
	 * The behavior of this method is determined by the hard-coded pivot choosing strategy.
	 * See the PIVOT_TYPE attribute to modify the pivot-choosing strategy of the method.
	 * 
	 * @param array -- array to get pivot from
	 * @param lowIndex -- lower-bound of possible indexes for the pivot (inclusive).
	 * @param highIndex -- higher-bound of possible indexes for the pivot (inclusive).
	 * @return -- index of the calculated pivot
	 */
	private static <T> int getPivotIndex(ArrayList<T> array, int lowIndex, int highIndex) {
		// pivots are determined from the PIVOT_TYPE variable
		switch (PIVOT_TYPE) {
		case 0:
			return (lowIndex + highIndex) / 2;
		case 1:
			return lowIndex;
		case 2:
			return highIndex;
		}
		
		// to make eclipse shut up, in practice, this is never reached
		return 0;
	}
	
	/**
	 * Sort a subarray with Insertionsort.
	 * 
	 * @param masterArr -- list to be sorted
	 * @param comp -- definition of the sorting
	 * @param lIdx -- starting index (inclusive)
	 * @param rIdx  -- stopping index (inclusive)
	 */
	private static <T> void insertionsort(ArrayList<T> masterArr, Comparator<? super T> comp, int lIdx, int rIdx) {
		// element at unsortedIdx is the element to be inserted into the sorted section
		for (int unsortedIdx = lIdx; unsortedIdx <= rIdx; unsortedIdx++) {
			T unsortedElem = masterArr.get(unsortedIdx);
			
			// move every element that is 'greater' than unsortedElem up one index
			int idx = unsortedIdx - 1;
			while (idx >= 0 && comp.compare(masterArr.get(idx), unsortedElem) > 0) {
				masterArr.set(idx + 1, masterArr.get(idx));
				idx--;
			}
			// insert unsoredElem in its sorted position
			masterArr.set(idx + 1, unsortedElem);
		}
	}
}