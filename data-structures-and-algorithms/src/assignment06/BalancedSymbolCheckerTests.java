package assignment06;

import static org.junit.Assert.assertTrue;

import java.io.FileNotFoundException;

import org.junit.Test;

/**
 * Test the functionality of the BalancedSymbolChecker class with test source code files.
 * Test files are located in "`proj_root`/testClasses"
 * 
 * @author Maxwell Hanson
 * @since Mar 1, 2018
 * @version 1.0.0
 */
public class BalancedSymbolCheckerTests {
	/**
	 * Test the BalancedSymbolChecker against files with known flaws.
	 */
	@Test
	public void testFlawed() throws FileNotFoundException {
		String expRes, res;
		
		// 1
		expRes = "ERROR: Unmatched symbol at line 6 and column 1. Expected ), but read } instead.";
		res = BalancedSymbolChecker.checkFile("testClasses/Class1.java");
		assertTrue (expRes.equals(res));
		
		// 2		
		expRes = "ERROR: Unmatched symbol at line 7 and column 1. Expected  , but read } instead.";
		res = BalancedSymbolChecker.checkFile("testClasses/Class2.java");
		assertTrue (expRes.equals(res));
		
		// 3		
		expRes = "No errors found. All symbols match.";
		res = BalancedSymbolChecker.checkFile("testClasses/Class3.java");
		assertTrue (expRes.equals(res));
		
		// 4		
		expRes = "ERROR: File ended before closing comment.";
		res = BalancedSymbolChecker.checkFile("testClasses/Class4.java");
		assertTrue (expRes.equals(res));
		
		// 5		
		expRes = "ERROR: Unmatched symbol at line 3 and column 18. Expected ], but read } instead.";
		res = BalancedSymbolChecker.checkFile("testClasses/Class5.java");
		assertTrue (expRes.equals(res));
		
		// 6		
		expRes = "No errors found. All symbols match.";
		res = BalancedSymbolChecker.checkFile("testClasses/Class6.java");
		assertTrue (expRes.equals(res));
		
		// 7		
		expRes = "ERROR: Unmatched symbol at line 3 and column 33. Expected ], but read ) instead.";
		res = BalancedSymbolChecker.checkFile("testClasses/Class7.java");
		assertTrue (expRes.equals(res));
		
		// 8		
		expRes = "ERROR: Unmatched symbol at line 5 and column 30. Expected }, but read ) instead.";
		res = BalancedSymbolChecker.checkFile("testClasses/Class8.java");
		assertTrue (expRes.equals(res));
		
		// 9		
		expRes = "ERROR: Unmatched symbol at line 3 and column 33. Expected ), but read ] instead.";
		res = BalancedSymbolChecker.checkFile("testClasses/Class9.java");
		assertTrue (expRes.equals(res));
		
		// 10		
		expRes = "ERROR: Unmatched symbol at line 5 and column 10. Expected }, but read ] instead.";
		res = BalancedSymbolChecker.checkFile("testClasses/Class10.java");
		assertTrue (expRes.equals(res));
		
		// 11		
		expRes = "ERROR: Unmatched symbol at the end of file. Expected }.";
		res = BalancedSymbolChecker.checkFile("testClasses/Class11.java");
		assertTrue (expRes.equals(res));
		
	}
	
	/**
	 * Test the BalancedSymbolChecker against files known to be clean.
	 */
	@Test
	public void testPassing() throws FileNotFoundException {
		String expRes, res;
		
		// 12
		expRes = "No errors found. All symbols match.";
		res = BalancedSymbolChecker.checkFile("testClasses/Class12.java");
		assertTrue(expRes.equals(res));
		
		// 13
		expRes = "No errors found. All symbols match.";
		res = BalancedSymbolChecker.checkFile("testClasses/Class13.java");
		assertTrue(expRes.equals(res));
		
		// 14
		expRes = "No errors found. All symbols match.";
		res = BalancedSymbolChecker.checkFile("testClasses/Class14.java");
		assertTrue(expRes.equals(res));
	}
}
