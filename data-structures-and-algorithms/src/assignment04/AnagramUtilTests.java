package assignment04;

import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertTrue;
import static org.junit.Assert.fail;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Scanner;
import org.junit.Before;
import org.junit.Test;

public class AnagramUtilTests {
	// attributes for use with tests
	private Integer[] oneIntArray;
	private Integer[] randomIntArray;
	private Integer[] ascendingIntArray;
	private Integer[] descendingIntArray;
	private String[] oneStringArray;
	private String[] randomStringArray;
	private String[] ascendingStringArray;
	private String[] descendingStringArray;
	private String randomString;
	private String ascendingString;
	private String descendingString;
	private String oneCharString;
	private String emptyString;
	private String lowerAnagram1;
	private String lowerAnagram2;
	private String lowerNonAnagram1;
	private String lowerNonAnagram2;
	private String upperAnagram1;
	private String upperAnagram2;
	private String upperNonAnagram1;
	private String upperNonAnagram2;
	private String mixedAnagram1;
	private String mixedAnagram2;
	private String mixedNonAnagram1;
	private String mixedNonAnagram2;
	private String miscCharAnagram1;
	private String miscCharAnagram2;
	private String miscCharNonAnagram1;
	private String miscCharNonAnagram2;
	private String[] twoAnagramArray;
	private String[] twoAnagramGroup;
	private String[] manyAnagramArray;
	private String[] manyAnagramGroup;
	private String[] allAnagramArray;
	private String[] noAnagramArray;
	private Scanner noAnagramScanner;
	private Scanner twoAnagramScanner;
	private Scanner manyAnagramScanner;
	private Scanner allAnagramScanner;
	private Scanner oneStringScanner;

	/**
	 * Setup attributes for use in testing.
	 */
	@Before
	public void setup() {
		/// integer arrays
		oneIntArray = new Integer[] { 1 };
		randomIntArray = new Integer[] { 58, 17, 14, 37, 35, 70, 53, 39, 19, 99, 65, 75, 12, 36, 73, 86, 1, 5, 95, 9,
				68, 74, 24, 50, 76, 23, 51, 82, 44, 2, 38, 15, 48, 84, 94, 77, 90, 49, 8, 67, 3, 59, 11, 54, 7, 18, 79,
				100, 60, 20, 6, 83, 16, 13, 21, 62, 33, 55, 97, 63, 28, 22, 81, 92, 29, 31, 10, 64, 87, 34, 88, 27, 61,
				89, 71, 57, 91, 4, 41, 47, 85, 25, 72, 26, 80, 56, 32, 96, 42, 45, 30, 69, 46, 78, 66, 52, 43, 40, 98,
				93 };
		ascendingIntArray = new Integer[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21,
				22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47,
				48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73,
				74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99,
				100 };
		descendingIntArray = new Integer[] { 100, 99, 98, 97, 96, 95, 94, 93, 92, 91, 90, 89, 88, 87, 86, 85, 84, 83,
				82, 81, 80, 79, 78, 77, 76, 75, 74, 73, 72, 71, 70, 69, 67, 66, 65, 64, 63, 62, 61, 60, 59, 58, 57, 56,
				55, 54, 53, 52, 51, 50, 49, 48, 47, 46, 45, 44, 43, 42, 41, 40, 39, 38, 68, 37, 36, 35, 34, 33, 32, 31,
				30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3,
				2, 1 };

		/// string arrays
		oneStringArray = new String[] { "purple" };
		randomStringArray = new String[] { "module", "wolf", "referral", "object", "baseball", "minute", "casualty",
				"plain", "need", "treat", "pump", "club", "front", "dough", "clay", "feign", "trend", "belief",
				"frozen", "tiger", "weed", "district", "shallow", "duty", "hostage", "mention", "funny", "weight",
				"element", "past", "gradient", "dose", "innocent", "clique", "tire", "eliminate", "hardware",
				"possible", "can", "conflict", "interest", "broccoli", "reward", "miner", "rich", "rub", "reaction",
				"broken", "europe", "border", "ballet", "forward", "ideology", "chop", "wash", "profit", "surround",
				"fold", "instal", "plastic", "advertise", "monster", "galaxy", "note", "year", "racism", "misplace",
				"ride", "tendency", "belly", "board", "tumour", "swim", "grind", "filter", "mail", "physics",
				"perforate", "minimize", "weave", "speed", "dream", "cope", "get", "cause", "mother", "formal",
				"cluster", "affect", "emergency", "pleasant", "blue", "coma", "remedy", "muggy", "overwhelm", "screen",
				"unfair", "penny", "honor" };
		ascendingStringArray = new String[] { "advertise", "affect", "ballet", "baseball", "belief", "belly", "blue",
				"board", "border", "broccoli", "broken", "can", "casualty", "cause", "chop", "clay", "clique", "club",
				"cluster", "coma", "conflict", "cope", "district", "dose", "dough", "dream", "duty", "element",
				"eliminate", "emergency", "europe", "feign", "filter", "fold", "formal", "forward", "front", "frozen",
				"funny", "galaxy", "get", "gradient", "grind", "hardware", "honor", "hostage", "ideology", "innocent",
				"instal", "interest", "mail", "mention", "miner", "minimize", "minute", "misplace", "module", "monster",
				"mother", "muggy", "need", "note", "object", "overwhelm", "past", "penny", "perforate", "physics",
				"plain", "plastic", "pleasant", "possible", "profit", "pump", "racism", "reaction", "referral",
				"remedy", "reward", "rich", "ride", "rub", "screen", "shallow", "speed", "surround", "swim", "tendency",
				"tiger", "tire", "treat", "trend", "tumour", "unfair", "wash", "weave", "weed", "weight", "wolf",
				"year" };
		descendingStringArray = new String[] { "year", "wolf", "weight", "weed", "weave", "wash", "unfair", "tumour",
				"trend", "treat", "tire", "tiger", "tendency", "swim", "surround", "speed", "shallow", "screen", "rub",
				"ride", "rich", "reward", "remedy", "referral", "reaction", "racism", "pump", "profit", "possible",
				"pleasant", "plastic", "plain", "physics", "perforate", "penny", "past", "overwhelm", "object", "note",
				"need", "muggy", "mother", "monster", "module", "misplace", "minute", "minimize", "miner", "mention",
				"mail", "interest", "instal", "innocent", "ideology", "hostage", "honor", "hardware", "grind",
				"gradient", "get", "galaxy", "funny", "frozen", "front", "forward", "formal", "fold", "filter", "feign",
				"europe", "emergency", "eliminate", "element", "duty", "dream", "dough", "dose", "district", "cope",
				"conflict", "coma", "cluster", "club", "clique", "clay", "chop", "cause", "casualty", "can", "broken",
				"broccoli", "border", "board", "blue", "belly", "belief", "baseball", "ballet", "affect", "advertise" };

		/// strings
		randomString = "broccoli";
		ascendingString = "bcciloor";
		descendingString = "rooliccb";
		emptyString = "";

		/// anagrams
		lowerAnagram1 = "note";
		lowerAnagram2 = "tone";
		lowerNonAnagram1 = "computer";
		lowerNonAnagram2 = "strange";
		upperAnagram1 = "NOTE";
		upperAnagram2 = "TONE";
		upperNonAnagram1 = "SCRIBBLE";
		upperNonAnagram2 = "ELECTRODYNAMICS";
		mixedAnagram1 = "nOtE";
		mixedAnagram2 = "tOnE";
		mixedNonAnagram1 = "SpEcTrUm";
		mixedNonAnagram2 = "MeDdLe";
		miscCharAnagram1 = "?.aBcD>(*$12";
		miscCharAnagram2 = "$?*.21a(B>cD";
		miscCharNonAnagram1 = "^&*Dbsdfe341_-+";
		miscCharNonAnagram2 = "??ksdf%&*923";

		/// anagram arrays
		twoAnagramArray = new String[] { "spectrum", "slamander", "cosmos", "creationism", "orange", "note", "nalgene",
				"constellation", "tone" };
		twoAnagramGroup = new String[] { "note", "tone" };
		manyAnagramArray = new String[] { "spectrum", "slamander", "cosmos", "creationism", "orange", "note", "nalgene",
				"constellation", "tone", "cosmography", "xenophobia", "california", "neot", "toen", "calcium",
				"crapify", "oent" };
		manyAnagramGroup = new String[] { "note", "tone", "neot", "toen", "oent" };
		allAnagramArray = new String[] { "note", "tone", "neot", "toen", "oent", "ento", "eotn", "eont" };
		noAnagramArray = new String[] { "spectrum", "slamander", "cosmos", "creationism", "orange", "note", "nalgene",
				"constellation", "detone", "cosmography", "xenophobia", "california", "frog", "crap", "calcium",
				"crapify", "pool" };

		/// string scanners
		noAnagramScanner = new Scanner("spectrum slamander cosmos creationism orange note "
				+ "nalgene constellation detone cosmography xenophobia california frog crap " + "calcium crapify pool");
		twoAnagramScanner = new Scanner("spectrum slamander cosmos creationism orange note nalgene constellation tone");
		manyAnagramScanner = new Scanner("spectrum slamander cosmos creationism orange note nalgene constellation "
				+ "tone cosmography xenophobia california neot toen calcium crapify oent");
		allAnagramScanner = new Scanner("note tone neot toen oent ento eotn eont");
		oneStringScanner = new Scanner("yeeeboii");
	}

	/**
	 * Test the 'sort' method of the 'AnagramUtil' class.
	 * 
	 * Test sorting an unsorted string.
	 * Test sorting a sorted (ascending) string.
	 * Test sorting a sorted (descending) string.
	 * Test sorting a string of one character.
	 * Test sorting an empty string.
	 * Test null case
	 */
	@Test
	public void testSort() {
		String expRes, res;

		// unsorted string
		expRes = ascendingString;
		res = AnagramUtil.sort(randomString);
		assertTrue(expRes.equals(res));

		// ascending string
		expRes = ascendingString;
		res = AnagramUtil.sort(ascendingString);
		assertTrue(expRes.equals(res));

		// descending string
		expRes = ascendingString;
		res = AnagramUtil.sort(descendingString);
		assertTrue(expRes.equals(res));

		// one character string
		oneCharString = "a";
		expRes = oneCharString;
		res = AnagramUtil.sort(oneCharString);
		assertTrue(expRes.equals(res));

		// empty string
		expRes = emptyString;
		res = AnagramUtil.sort(emptyString);
		assertTrue(expRes.equals(res));

		// null case
		try {
			AnagramUtil.sort(null);
			fail("");
		}
		catch (NullPointerException e) {}
	}

	/**
	 * Test the 'insertionSort' method of the 'AnagramUtil' class on Integers.
	 * 
	 * Test sorting a random array of integers.
	 * Test sorting a sorted (ascending) array of integers.
	 * Test sorting a sorted (descending) array of integers. Test
	 * sorting an array of one integer. Test sorting a random array of strings.
	 * Test sorting a sorted (ascending) array of strings.
	 * Test sorting a sorted (descending) array of strings.
	 * Test sorting an array of one strings.
	 * Test parameterizing null.
	 */
	@Test
	public void testInsertionSortIntegers() {
		Integer[] expRes;

		// one integer
		expRes = oneIntArray.clone();
		AnagramUtil.<Integer>insertionSort(oneIntArray, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(Arrays.equals(expRes, oneIntArray));

		// random array of integers
		expRes = ascendingIntArray.clone();
		AnagramUtil.<Integer>insertionSort(randomIntArray, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(Arrays.equals(expRes, randomIntArray));

		// sorted (ascending) array of integers
		expRes = ascendingIntArray.clone();
		AnagramUtil.<Integer>insertionSort(ascendingIntArray, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(Arrays.equals(expRes, ascendingIntArray));

		// sorted (descending) array of integers
		expRes = ascendingIntArray.clone();
		AnagramUtil.<Integer>insertionSort(descendingIntArray, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(Arrays.equals(expRes, descendingIntArray));

		// null case
		try {
			AnagramUtil.<Integer>insertionSort(null, (lhs, rhs) -> lhs.compareTo(rhs));
			fail("Parameterizing null failed to throw NullPointerExcpetion.");
		} catch (NullPointerException e) {
		}
	}

	/**
	 * Test the 'insertionSort' method of the 'AnagramUtil' class on Strings.
	 * 
	 * Test sorting a random array.
	 * Test sorting a sorted (ascending) array.
	 * Test sorting a sorted (descending) array.
	 * Test sorting an array of one strings.
	 * Test parameterizing null.
	 */
	@Test
	public void testInsertionSortStrings() {
		String[] expRes;

		// one string
		expRes = oneStringArray.clone();
		AnagramUtil.<String>insertionSort(oneStringArray, (lhs, rhs) -> lhs.compareTo(rhs));
		assertTrue(Arrays.equals(expRes, oneStringArray));

		// random strings
		expRes = ascendingStringArray.clone();
		AnagramUtil.<String>insertionSort(randomStringArray,
				(lhs, rhs) -> lhs.toLowerCase().compareTo(rhs.toLowerCase()));
		assertTrue(Arrays.equals(expRes, randomStringArray));

		// ascending strings
		expRes = ascendingStringArray.clone();
		AnagramUtil.<String>insertionSort(ascendingStringArray,
				(lhs, rhs) -> lhs.toLowerCase().compareTo(lhs.toLowerCase()));
		assertTrue(Arrays.equals(expRes, ascendingStringArray));

		// descending strings
		expRes = ascendingStringArray.clone();
		AnagramUtil.<String>insertionSort(descendingStringArray,
				(lhs, rhs) -> lhs.toLowerCase().compareTo(rhs.toLowerCase()));
		assertTrue(Arrays.equals(expRes, descendingStringArray));

		// null case
		try {
			AnagramUtil.<String>insertionSort(null, (lhs, rhs) -> lhs.compareTo(rhs));
			fail("");
		} catch (NullPointerException e) {}
	}

	/**
	 * Test the 'areAnagrams' method of the 'AnagramUtil' class.
	 * 
	 * Test that two lowercase anagrams return true.
	 * Test that two lowercase non-anagrams return false.
	 * Test that two uppercase anagrams return true.
	 * Test that two uppercase non-anagrams return false.
	 * Test that two mixed-case anagrams return true.
	 * Test that two mixed-case non-anagrams return false.
	 * Test two strings with digits and special characters that are anagrams.
	 * Test two strings with digits and special characters that are not anagrams.
	 * Test first parameter null, second not.
	 * Test second parameter null, first not.
	 * Test both parameters null.
	 */
	@Test
	public void testAreAnagrams() {
		// lowercase anagrams
		assertTrue(AnagramUtil.areAnagrams(lowerAnagram1, lowerAnagram2));

		// lowercase non-anagrams
		assertFalse(AnagramUtil.areAnagrams(lowerNonAnagram1, lowerNonAnagram2));

		// uppercase anagrams
		assertTrue(AnagramUtil.areAnagrams(upperAnagram1, upperAnagram2));

		// uppercase non-anagrams
		assertFalse(AnagramUtil.areAnagrams(upperNonAnagram1, upperNonAnagram2));

		// mixed-case anagrams
		assertTrue(AnagramUtil.areAnagrams(mixedAnagram1, mixedAnagram2));

		// mixed-case non-anagrams
		assertFalse(AnagramUtil.areAnagrams(mixedNonAnagram1, mixedNonAnagram2));

		// anagrams digits and special characters
		assertTrue(AnagramUtil.areAnagrams(miscCharAnagram1, miscCharAnagram2));

		// non-anagrams digits and special characters
		assertFalse(AnagramUtil.areAnagrams(miscCharNonAnagram1, miscCharNonAnagram2));

		// null cases
		try {
			AnagramUtil.areAnagrams(lowerAnagram1, null);
			fail("");
		}
		catch (NullPointerException e) {}
		try {
			AnagramUtil.areAnagrams(null, lowerAnagram1);
			fail("");
		}
		catch (NullPointerException e) {}
		try {
			AnagramUtil.areAnagrams(null, null);
			fail("");
		}
		catch (NullPointerException e) {}
	}

	/**
	 * Test the 'getLargestAnagramGroup' method of the 'AnagramUtil' class with
	 * String parameters.
	 * 
	 * Test a string array with no anagrams.
	 * Test a string array with two anagrams.
	 * Test a string array with many anagrams.
	 * Test a string array with all anagrams.
	 * Test a string array of size one.
	 * Test null case.
	 */
	@Test
	public void testGetLargestAnagramGroupString() {
		String[] expRes, res;
		ArrayList<String> expResList, resList;

		// string array with no anagrams
		expRes = new String[] {};
		res = AnagramUtil.getLargestAnagramGroup(noAnagramArray);
		printArray(expRes);
		printArray(res);
		assertTrue(Arrays.equals(expRes, res));

		// string array with two anagrams
		expRes = twoAnagramGroup;
		res = AnagramUtil.getLargestAnagramGroup(twoAnagramArray);
		// using the Collection.containsAll method ensures ordering is not required
		expResList = new ArrayList<String>(Arrays.asList(expRes));
		resList = new ArrayList<String>(Arrays.asList(res));
		assertTrue(resList.containsAll(expResList));
		// ensure that expRes is not a subset of res
		assertTrue(expRes.length == res.length);

		// string array with many anagrams
		expRes = manyAnagramGroup;
		res = AnagramUtil.getLargestAnagramGroup(manyAnagramArray);
		// using the Collection.containsAll method ensures ordering is not required
		expResList = new ArrayList<String>(Arrays.asList(expRes));
		resList = new ArrayList<String>(Arrays.asList(res));
		assertTrue(resList.containsAll(expResList));
		// ensure that expRes is not a subset of res
		assertTrue(expRes.length == res.length);

		// string array with all anagrams
		expRes = allAnagramArray.clone();
		res = AnagramUtil.getLargestAnagramGroup(allAnagramArray);
		// using the Collection.containsAll method ensures ordering is not required
		expResList = new ArrayList<String>(Arrays.asList(expRes));
		resList = new ArrayList<String>(Arrays.asList(res));
		assertTrue(resList.containsAll(expResList));
		// ensure that expRes is not a subset of res
		assertTrue(expRes.length == res.length);

		// string array of size one
		expRes = new String[] {};
		res = AnagramUtil.getLargestAnagramGroup(oneStringArray);
		assertTrue(Arrays.equals(expRes, res));

		// null case
		expRes = new String[] {};
		String[] param = null;
		res = AnagramUtil.getLargestAnagramGroup(param);
		assertTrue(Arrays.equals(expRes, res));
	}

	@Test
	public void testsGetLargestAnagramGroup() {
		String[] input = new String[] { "carets", "Caller", "eat", "cellar", "recall", "Caters", "Ate", "caster",
				"aspired", "allergy", "despair", "asp", "pas", "least", "sap", "spa", "diapers", "praised", "crates",
				"Reacts", "bats", "tea", "Stab", "stale", "tabs", "recast", "darters", "Gallery", "retards", "star",
				"red", "code", "Coed", "deco", "traders", "traces" };
		String[] expRes = new String[] {"carets", "Caters", "caster", "crates", "Reacts", "recast", "traces"};
		String[] res = AnagramUtil.getLargestAnagramGroup(input);
		System.out.println(res.length);
		assertTrue(AnagramUtil.areAnagrams("Caters", "carets"));
		ArrayList<String> expResList = new ArrayList<String>(Arrays.asList(expRes));
		ArrayList<String> resList = new ArrayList<String>(Arrays.asList(res));
		assertTrue(resList.containsAll(expResList));
		// ensure that expRes is not a subset of res
		assertTrue(expRes.length == res.length);
	}

	/**
	 * Test the 'getLargestAnagramGroup' method of the 'AnagramUtil' class with
	 * Scanner parameters.
	 * 
	 * Test a scanner of strings with no anagrams. Test a scanner of strings with
	 * two anagrams. Test a scanner of strings with many anagrams. Test a scanner of
	 * strings with all anagrams. Test a scanner of strings of size one. Test an
	 * empty scanner. Test null case.
	 */
	@Test
	public void testGetLargestAnagramGroupScanner() {
		String[] expRes, res;
		ArrayList<String> expResList, resList;

		// no anagrams
		expRes = new String[] {};
		res = AnagramUtil.getLargestAnagramGroup(noAnagramScanner);
		printArray(expRes);
		printArray(res);
		assertTrue(Arrays.equals(expRes, res));

		// two anagrams
		expRes = twoAnagramGroup;
		res = AnagramUtil.getLargestAnagramGroup(twoAnagramScanner);
		// using the Collection.containsAll method ensures ordering is not required
		expResList = new ArrayList<String>(Arrays.asList(expRes));
		resList = new ArrayList<String>(Arrays.asList(res));
		assertTrue(resList.containsAll(expResList));
		// ensure that expRes is not a subset of res
		assertTrue(expRes.length == res.length);

		// many anagrams
		expRes = manyAnagramGroup;
		res = AnagramUtil.getLargestAnagramGroup(manyAnagramScanner);
		// using the Collection.containsAll method ensures ordering is not required
		expResList = new ArrayList<String>(Arrays.asList(expRes));
		resList = new ArrayList<String>(Arrays.asList(res));
		assertTrue(resList.containsAll(expResList));
		// ensure that expRes is not a subset of res
		assertTrue(expRes.length == res.length);

		// all anagrams
		expRes = allAnagramArray.clone();
		res = AnagramUtil.getLargestAnagramGroup(allAnagramScanner);
		// using the Collection.containsAll method ensures ordering is not required
		expResList = new ArrayList<String>(Arrays.asList(expRes));
		resList = new ArrayList<String>(Arrays.asList(res));
		assertTrue(resList.containsAll(expResList));
		// ensure that expRes is not a subset of res
		assertTrue(expRes.length == res.length);

		// scanner of one string
		expRes = new String[] {};
		res = AnagramUtil.getLargestAnagramGroup(oneStringScanner);
		// using the Collection.containsAll method ensures ordering is not required
		expResList = new ArrayList<String>(Arrays.asList(expRes));
		resList = new ArrayList<String>(Arrays.asList(res));
		assertTrue(resList.containsAll(expResList));
		// ensure that expRes is not a subset of res
		assertTrue(expRes.length == res.length);

		// empty scanner
		expRes = new String[] {};
		res = AnagramUtil.getLargestAnagramGroup(new Scanner(""));
		assertTrue(Arrays.equals(expRes, res));

		// null case
		expRes = new String[] {};
		Scanner param = null;
		res = AnagramUtil.getLargestAnagramGroup(param);
		assertTrue(Arrays.equals(expRes, res));
	}

	public static <T> void printArray(T[] arg) {
		for (int i = 0; i < arg.length; i++) {
			System.out.print(arg[i] + ", ");
		}
		System.out.println();
	}
}
