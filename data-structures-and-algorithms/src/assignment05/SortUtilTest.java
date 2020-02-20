package assignment05;

import static org.junit.Assert.assertTrue;
import static org.junit.Assert.fail;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Comparator;
import org.junit.Before;
import org.junit.Test;
import assignment05.SortUtil;

/**
 * Test cases for the SortUtil class.
 * 
 * @author Maxwell Hanson
 * @since Jul 12, 2018
 * @version 1.0.0
 */
public class SortUtilTest {
	private static ArrayList<Integer> posArr;
	private static ArrayList<Integer> negArr;
	private static ArrayList<Integer> mixedSignArr;
	private static ArrayList<String> asciiArr;
	private static ArrayList<String> specialCharArr;
	private static ArrayList<String> mixedCharArr;
	private static ArrayList<Integer> ascendingArr;
	private static ArrayList<Integer> descendingArr;
	private static ArrayList<Integer> permutedArr;
	private static ArrayList<Integer> evenArr;
	private static ArrayList<Integer> oddArr;
	private static ArrayList<Integer> nullArr;
	private static ArrayList<Integer> zeroArr;
	private static ArrayList<Integer> emptyArr;

	/**
	 * Setup the attributes for testing.
	 */
	@Before
	public void setup() {
		/// setup arrays
		posArr = new ArrayList<Integer>(Arrays.asList(new Integer[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }));
		negArr = new ArrayList<Integer>(Arrays.asList(new Integer[] { -1, -2, -3, -4, -5, -6, -7, -8, -9, -10 }));
		mixedSignArr = new ArrayList<Integer>(Arrays.asList(new Integer[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 }));
		asciiArr = new ArrayList<String>(
				Arrays.asList(new String[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o",
						"p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H",
						"I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" }));
		specialCharArr = new ArrayList<String>(Arrays.asList(
				new String[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r",
						"s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K",
						"L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "1", "2", "3", "4",
						"5", "6", "7", "8", "9", "0", "~", "`", "!", "@", "#", "$", "%", "^", "&", "*(", ")", "-", "=",
						"_", "+", "[", "]", "\\", "{", "}", "|", ";", "'", ":", "\"", ",", ".", "/", "<", ">", "?" }));
		mixedCharArr = new ArrayList<String>(Arrays.asList(
				new String[] { "DCZzD", "AU'<i", "40ZD`", "Dopy&", "WVWK%", "Q!</h", "1gCUk", "/VK+V", "3u4[n", "5]Q]N",
						"`3-UH", "Inf,%", "i}1Y_", "4LN#K", "#b/J?", "KlZA`", "m_gq2", "ff;O`", "t1<(1", "aMeM}",
						"7{b!B", "2YeFY", ",Dgh|", "@SA?]", "<n>A'", "~9,>r", "0M-?c", ":O&uE", "oe+Q_", "D&,+K",
						"#6i-)", "]|6xY", "I^jHB", "[L@Li", "3(>H{", "5u=?b", "-ju0(", "wKe9,", "c/d5(", "W+g9K",
						"-oZK#", "\\V@}(", "CHL;Y", "MoZx(", "xyiPx", ";yC\"n", "+H1o8", "dTa6.", "\\|K>G", "%]3k$" }));
		ascendingArr = SortUtil.generateAscending(1000);
		descendingArr = SortUtil.generateDescending(1000);
		permutedArr = SortUtil.generateShuffled(1000);
		evenArr = SortUtil.generateShuffled(1000);
		oddArr = SortUtil.generateShuffled(999);
		nullArr = SortUtil.generateShuffled(100);
		nullArr.add(null);
		zeroArr = SortUtil.generateShuffled(100);
		zeroArr.set(50, 0);
		emptyArr = new ArrayList<Integer>(Arrays.asList(new Integer[] {}));
	}

	/**
	 * Test the mergesort method of the SortUtil class.
	 * 
	 * Test sorting an array of integers with only positive integers.
	 * Test sortingban array of integers with only negative integers.
	 * Test sorting an array of integers with positive, negative, and zero integers.
	 * Test sorting an array of strings with only ascii characters as individual elements.
	 * Test sorting an array of strings with every character on the keyboard as individual elements.
	 * Test sorting an array of strings containing ascii and special characters.
	 * Test sorting an ascending array. Test sorting a descending array.
	 * Test sorting several permuted arrays.
	 * Test sorting an array with an even number of elements.
	 * Test sorting an array with an odd number of elements.
	 * Test sorting an array with null contained in it.
	 * Test sorting an array with zero contained in it.
	 * Test passing an empty array.
	 * Test passing null for array.
	 * Test passing null for the comparator.
	 * Test passing null for the list and the comparator.
	 */
	@Test
	public void testMergesort() {
		// array of integers only positive elems
		SortUtil.mergesort(posArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(posArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// array of integers only negative elems
		SortUtil.mergesort(negArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(negArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// array of integers pos, neg, and zero
		SortUtil.mergesort(mixedSignArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(mixedSignArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// array of strings with ascii characters for each elem
		SortUtil.mergesort(asciiArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(asciiArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// array of strings with every character on keyboard for each elem
		SortUtil.mergesort(specialCharArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(specialCharArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// array of random strings containing every ascii and special char
		SortUtil.mergesort(mixedCharArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(mixedCharArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// ascending array
		SortUtil.mergesort(ascendingArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(ascendingArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// descending array
		SortUtil.mergesort(descendingArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(descendingArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// permuted arrays
		SortUtil.mergesort(permutedArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(permutedArr, (lhs, rhs) -> lhs.compareTo(rhs)));
		permutedArr = SortUtil.generateShuffled(1000);
		SortUtil.mergesort(permutedArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(permutedArr, (lhs, rhs) -> lhs.compareTo(rhs)));
		permutedArr = SortUtil.generateShuffled(1000);
		SortUtil.mergesort(permutedArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(permutedArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// even number of elems
		SortUtil.mergesort(evenArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(evenArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// odd number of elems
		SortUtil.<Integer>mergesort(oddArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(oddArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// array containing null
		try {
		SortUtil.mergesort(nullArr, (lhs, rhs) -> lhs.compareTo(rhs));
		fail("NullPointerException not thrown");
		} catch (NullPointerException e) {
		}

		// array containing zero
		SortUtil.mergesort(zeroArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(zeroArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// empty array
		SortUtil.<Integer>mergesort(emptyArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(emptyArr.equals(new ArrayList<Integer>()));

		// null for array param
		SortUtil.<Integer>mergesort(null, (lhs, rhs) -> lhs.compareTo(rhs));

		// null for comp param
		SortUtil.<Integer>mergesort(emptyArr, null);
		assertTrue(emptyArr.equals(new ArrayList<Integer>()));

		// null for both params
		SortUtil.<Integer>mergesort(null, null);
	}

	/**
	 * Test the quicksort method of the SortUtil class.
	 * 
	 * Test sorting an array of integers with only positive integers.
	 * Test sorting an array of integers with only negative integers.
	 * Test sorting an array of integers with positive, negative, and zero integers.
	 * Test sorting an array of strings with only ascii characters as individual elements.
	 * Test sorting an array of strings with every character on the keyboard as individual elements.
	 * Test sorting an array of strings containing ascii and special characters.
	 * Test sorting an ascending array. Test sorting a descending array.
	 * Test sorting several permuted arrays.
	 * Test sorting an array with an even number of elements.
	 * Test sorting an array with an odd number of elements.
	 * Test sorting an array with null contained in it.
	 * Test sorting an array with zero contained in it.
	 * Test passing an empty array.
	 * Test passing null for array.
	 * Test passing null for the comparator.
	 * Test passing null for the list and the comparator.
	 */
	@Test
	public void testQuicksort() {
		// array of integers only positive elems
		SortUtil.<Integer>quicksort(posArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(posArr, (lhs, rhs) -> lhs.compareTo(rhs)));
		
		// array of integers only negative elems
		SortUtil.<Integer>quicksort(negArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(negArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// array of integers pos, neg, and zero
		SortUtil.<Integer>quicksort(mixedSignArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(mixedSignArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// array of strings with ascii characters for each elem
		SortUtil.quicksort(asciiArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(asciiArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// array of strings with every character on keyboard for each elem
		SortUtil.quicksort(specialCharArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(specialCharArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// array of random strings containing every ascii and special char
		SortUtil.quicksort(mixedCharArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(mixedCharArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// ascending array
		SortUtil.<Integer>quicksort(ascendingArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(ascendingArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// descending array
		SortUtil.<Integer>quicksort(descendingArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(descendingArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// permuted arrays
		SortUtil.<Integer>quicksort(permutedArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(permutedArr, (lhs, rhs) -> lhs.compareTo(rhs)));
		permutedArr = SortUtil.generateShuffled(1000);
		SortUtil.<Integer>quicksort(permutedArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(permutedArr, (lhs, rhs) -> lhs.compareTo(rhs)));
		permutedArr = SortUtil.generateShuffled(1000);
		SortUtil.<Integer>quicksort(permutedArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(permutedArr, (lhs, rhs) -> lhs.compareTo(rhs)));


		// even number of elems
		SortUtil.<Integer>quicksort(evenArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(evenArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// odd number of elems
		SortUtil.<Integer>quicksort(oddArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(oddArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// array containing null (should throw nullpointerexception)
		try {
			SortUtil.<Integer>quicksort(nullArr, (lhs, rhs) -> lhs.compareTo(rhs));
			fail("Array with null did not throw null pointer exception.");
		}
		catch (NullPointerException e) {}

		// array containing zero
		SortUtil.<Integer>quicksort(zeroArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(isAscending(zeroArr, (lhs, rhs) -> lhs.compareTo(rhs)));

		// empty array
		SortUtil.<Integer>quicksort(emptyArr, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(emptyArr.equals(new ArrayList<Integer>()));

		// null for array param
		SortUtil.<Integer>quicksort(null, (lhs, rhs) -> lhs.compareTo(rhs));

		// null for comp param
		SortUtil.<Integer>quicksort(emptyArr, null);
		assertTrue(emptyArr.equals(new ArrayList<Integer>()));

		// null for both params
		SortUtil.<Integer>quicksort(null, null);
	}

	/**
	 * Test the generateAscending method of the SortUtil class.
	 *
	 * Test a negative parameter. Test a parameter of zero. Test that calling the
	 * method with a parameter of a small postive number returns. Test that calling
	 * the method a parameter of a moderately-large, postive number. Test that
	 * calling the method a parameter of a very large, positive number. Test that
	 * every element in the array returned by calling the method with a
	 * moderately-large, postive integer is less than the element after it (except
	 * the last element).
	 */
	@Test
	public void testGenerateAscending() {
		// negative parameter (should return empty list)
		SortUtil.generateAscending(-100);

		// zero parameter (should return empty list)
		SortUtil.generateAscending(0);

		// small pos
		SortUtil.generateAscending(100);

		// med pos
		SortUtil.generateAscending(10000);

		// large pos
		SortUtil.generateAscending(1000000);

		// test sorting of an array
		assertTrue( isAscending(SortUtil.generateAscending(10000), (lhs, rhs) -> lhs.compareTo(rhs)) );
	}

	/**
	 * Test the generateShuffled method of the SortUtil class.
	 *
	 * Test a negative parameter. Test a parameter of zero. Test that calling the
	 * method with a parameter of a small postive number returns. Test that calling
	 * the method a parameter of a moderately-large, postive number. Test that
	 * calling the method a parameter of a very large, positive number. Test that
	 * calling the method with a moderately large, positive parameter returns an
	 * ArrayList that is neither ascending, nor descending.
	 */
	@Test
	public void testGenerateShuffled() {
		// negative parameter (should return empty list)
		SortUtil.generateShuffled(-100);

		// zero parameter (should return empty list
		SortUtil.generateShuffled(0);

		// small pos
		SortUtil.generateShuffled(100);

		// med pos
		SortUtil.generateShuffled(10000);

		// large pos
		SortUtil.generateShuffled(1000000);

		// test array is not sorted
		assertTrue(!isAscending(SortUtil.generateShuffled(10000), (lhs, rhs) -> lhs.compareTo(rhs)));
		assertTrue(!isDescending(SortUtil.generateShuffled(10000), (lhs, rhs) -> lhs.compareTo(rhs)));
	}

	/**
	 * Test the generateDesceding method of the SortUtil class.
	 *
	 * Test a negative parameter. Test a parameter of zero. Test that calling the
	 * method with a parameter of a small postive number returns. Test that calling
	 * the method a parameter of a moderately-large, postive number. Test that
	 * calling the method a parameter of a very large, positive number. Test that
	 * every element in the array returned by calling the method with a
	 * moderately-large, postive integer is greater than the element after it
	 * (except the last element).
	 */
	@Test
	public void testGenerateDescending() {
		// negative parameter (should return empty array)
		SortUtil.generateDescending(-100);

		// zero parameter (should return empty array)
		SortUtil.generateAscending(0);

		// small pos
		SortUtil.generateDescending(100);

		// med pos
		SortUtil.generateDescending(10000);

		// large pos
		SortUtil.generateDescending(1000000);

		// test sorting of an array
		assertTrue(isDescending(SortUtil.generateDescending(10000), (lhs, rhs) -> lhs.compareTo(rhs)));
	}

	/**
	 * Determine if an array is ascending.
	 * 
	 * @param arr
	 *            -- array to determine the status of
	 * @param comp
	 *            -- comparator to determine the status of the array with
	 * @return -- true if the array is ascending, false otherwise
	 */
	private static <T> boolean isAscending(ArrayList<T> arr, Comparator<? super T> comp) {
		for (int index = 0; index < arr.size() - 1; index++) {
			if (comp.compare(arr.get(index), arr.get(index + 1)) > 0) {
				return false;
			}
		}
		return true;
	}

	/**
	 * Determine if an array is descending.
	 * 
	 * @param arr
	 *            -- array to determine the status of
	 * @param comp
	 *            -- comparator to determine the status of the array with
	 * @return -- true if the array is descending, false otherwise
	 */
	private static <T> boolean isDescending(ArrayList<T> arr, Comparator<? super T> comp) {
		for (int index = 0; index < arr.size() - 1; index++) {
			if (comp.compare(arr.get(index), arr.get(index + 1)) < 0) {
				return false;
			}
		}
		return true;
	}
}
