package assignment05;

import java.util.ArrayList;
import java.util.Comparator;

/**
 * Sort an ArrayList with mergesort.
 * 
 * @author Maxwell Hanson
 * @version Jul 12, 2018
 * @version 1.1.0
 */
public class Mergesort {
	/**
	 * Arrays with this amount of elements or less are sorted with insertion sort and not mergesort.
	 * 
	 * Reason being insertion sort is more efficient with smaller array sthan merge sort.
	 */
	private static int INSERTIONSORT_THRESHOLD = 450;

	/**
	 * Recursively sort an array with mergesort.
	 * 
	 * @param array -- the array to sort
	 * @param comparator -- the definition of sorting order
	 */
	public static <T> void mergesortRecursive(ArrayList<T> array, Comparator<? super T> comparator, int left, int right) {
		// use insertionsort to sort rest of array if its size is below the global threshold
		if (Math.abs(right - left) <= INSERTIONSORT_THRESHOLD) {
			insertionsort(array, comparator, left, right);
			return;
		}
		
		// done sorting (sub)array if pointers cross
		if (Math.abs(left - right) <= 0) {
			return;
		}
		
		// recursively sort both halves and combine
		int mid = (left + right) / 2;
		mergesortRecursive(array, comparator, left, mid);
		mergesortRecursive(array, comparator, mid + 1, right);
		merge(array, comparator, left, mid + 1, right);
	}

	/**
	 * Merge two contiguous subarrays of a larger array.
	 * 
	 * @param masterArr -- full array in which two subarrays are contained
	 * @param comp -- definition of the sorting to be used
	 * @param left -- the smallest index of both arrays, inclusive
	 * @param mid -- index separating both arrays, element at mid belongs to the 'right' subarray
	 * @param right -- largest index of both arrays, inclusive
	 */
	public static <T> void merge(ArrayList<T> masterArr, Comparator<? super T> comp, int left, int mid, int right) {
		/// clone subarrays into separate arrays
		// left subarray goes from `left` (inclusive) to `mid` (exclusive)
		// right subarray goes from `mid` (inclusive) to `right` (inclusive)
		ArrayList<T> leftSubarr = new ArrayList<T>();
		ArrayList<T> rightSubarr = new ArrayList<T>();

		for (int curs = left; curs < mid; curs++) {
			leftSubarr.add(masterArr.get(curs));
		}
		for(int curs = mid; curs <= right; curs++) {
			rightSubarr.add(masterArr.get(curs));
		}

		// copy two subarray clones into the master array in sorted form
		// both subarrays are iterated on, the first element of each is compared and the lesser is copied to the master array
		int lIdx = 0; // index of left subarray
		int rIdx = 0; // index of right subarray
		int mIdx = left; // index of master array
		while (lIdx < leftSubarr.size() && rIdx < rightSubarr.size()) {
			int compRes = comp.compare(leftSubarr.get(lIdx), rightSubarr.get(rIdx));
			
			// if leftSubarr's element is 'less' than rightSubarr's...
			if (compRes < 0) {
				masterArr.set(mIdx, leftSubarr.get(lIdx));
				lIdx++;
			}
			else {
				masterArr.set(mIdx, rightSubarr.get(rIdx));
				rIdx++;
			}
			
			mIdx++;
		}
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