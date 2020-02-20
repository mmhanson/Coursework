package assignment01;

import static org.junit.Assert.assertTrue;
import static org.junit.Assert.assertEquals;
import static org.junit.Assert.fail;
import org.junit.Before;
import org.junit.Test;
import assignment01.Matrix;

/**
 * JUnit tests for the assignment01.Matrix class.
 * 
 * @author Maxwell Hanson
 * @since January 11, 2018
 * @version 1.1.0
 */
public class MatrixTests {
    Matrix oneByOne;
    Matrix twoByThree;
    Matrix twoByThree02;
    Matrix threeByTwo;
    Matrix twoByTwoProduct;
    Matrix twoByThreeSum;
	
    /**
     * Initialize member variables for use with testing.
     */
	@Before
	public void setup() {
		oneByOne = new Matrix(new int[][]
				{{1}});
		twoByThree = new Matrix(new int[][]
                {{1, 2, 3},
				 {2, 5, 6}});
		twoByThree02 = new Matrix(new int[][]
                {{2, 3, 4},
				 {5, 6, 7}});
		threeByTwo = new Matrix(new int[][]
                {{4, 5},
				 {3, 2},
				 {1, 1}});
        // known correct result of multiplying threeByTwo and twoByThree 
		twoByTwoProduct = new Matrix(new int[][]
                {{13, 12},
                 {29, 26}});
		twoByThreeSum = new Matrix(new int[][]
                {{3, 5, 7},
				 {7, 11, 13}});
	}
	
	/**
	 * Test the "equals" method of the "Matrix" class.
	 * 
	 * Testing is done by creating two matrices from the same
	 * parameters and testing if they are equal.
	 */
	@Test
	public void testEquals() {
        // zero case
        int[][] zeroCaseParams = new int[][]{{}};
        Matrix zeroCaseMatrix01 = new Matrix(zeroCaseParams);
        Matrix zeroCaseMatrix02 = new Matrix(zeroCaseParams);
		assertTrue(zeroCaseMatrix01.equals(zeroCaseMatrix02));
		assertTrue(zeroCaseMatrix02.equals(zeroCaseMatrix01));

		// edge case
		int[][] edgeCaseParams = new int[][]{{4}, {3}};
		Matrix edgeCaseMatrix01 = new Matrix(edgeCaseParams);
		Matrix edgeCaseMatrix02 = new Matrix(edgeCaseParams);
		assertTrue(edgeCaseMatrix01.equals(edgeCaseMatrix02));
		assertTrue(edgeCaseMatrix02.equals(edgeCaseMatrix01));
		
		// many case
		int[][] manyCaseParams = new int[][]{{4, 5}, {3, 2}, {1, 1}};
		Matrix manyCaseMatrix01 = new Matrix(manyCaseParams);
		Matrix manyCaseMatrix02 = new Matrix(manyCaseParams);
		assertTrue(manyCaseMatrix01.equals(manyCaseMatrix02));
		assertTrue(manyCaseMatrix02.equals(manyCaseMatrix01));
	}

	/**
	 * Test the "toString" method of the "Matix" class.
	 * 
	 * Testing is done by comparing a constructed matrix to a predetermined,
     * correct string representation of the matrix.
	 */
	@Test
	public void testToString() {
        String res, expRes;

		// edge case
		res = oneByOne.toString();
		expRes = "1\n";
		assertEquals(expRes, res);
		
		// many cases
		res = twoByThree.toString();
		expRes = "1 2 3\n2 5 6\n";
		assertEquals(expRes, res);
		
		res = threeByTwo.toString();
		expRes = "4 5\n3 2\n1 1\n";
		assertEquals(expRes, res);
	}

	/**
	 * Test the "plus" method of the "Matrix" class.
	 * 
     * Testing is done by adding two matrices and comparing the sum with an
     * expected result.
	 */
	@Test
	public void testPlus() {
		// error cases
		// m1 rows > m2 rows, m1 cols = m2 cols
		Matrix errorCase01 = threeByTwo.plus(twoByTwoProduct);
		if (errorCase01 != null) {
			fail("testPlus error case 1 did not return null.");
		}
		// m1 rows < m2 rows, m1 cols = m2 cols
		Matrix errorCase02 = twoByTwoProduct.plus(threeByTwo);
		if (errorCase02 != null) {
			fail("testPlus error case 2 did not return null.");
		}
		// m1 rows = m2 rows, m1 cols > m2 cols
		Matrix errorCase03 = twoByThreeSum.plus(twoByTwoProduct);
		if (errorCase03 != null) {
			fail("testPlus error case 3 did not return null.");
		}
		// m1 rows = m2 rows, m1 cols < m2 cols
		Matrix errorCase04 = twoByTwoProduct.plus(twoByThreeSum);
		if (errorCase04 != null) {
			fail("testPlus error case 4 did not return null.");
		}
		
		// success case
		Matrix sumResult = twoByThree.plus(twoByThree02);
		assertTrue(sumResult.equals(twoByThreeSum));
	}
	
	/**
	 * Test the "times" method of the "Matrix" class.
	 * 
	 * Testing is done by multiplying two matrices and comparing the product
     * with an expected result.
	 */
	@Test
	public void testTimes() {
		// error cases
		// m1 cols > m2 rows
		Matrix errorCase01 = threeByTwo.times(oneByOne);
		if (errorCase01 != null) {
			fail("testTimes() error case 1 did not return null");
		}
		// m1 cols < m2 rows
		Matrix errorCase02 = oneByOne.times(threeByTwo);
		if (errorCase02 != null) {
			fail("testTimes() error case 2s did not return null");
		}
		
		// success case
		Matrix successMatrixProduct = twoByThree.times(threeByTwo);
		assertTrue(twoByTwoProduct.equals(successMatrixProduct));
	}
}
