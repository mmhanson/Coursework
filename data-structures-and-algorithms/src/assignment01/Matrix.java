package assignment01;

/**
 * Implementation of a 2D Mathematical Matrix.
 *
 * Includes methods for adding and multiplying matrices.
 * 
 * @author Maxwell Hanson
 * @since January 11, 2018
 * @version 1.1.0
 */
public class Matrix {
    /**
     * The count of rows in the matrix.
     * Values are one-based.
     */
	int numRows;
    /**
     * The count of columns in the matrix.
     * Values are one-based.
     */
	int numColumns;
    /**
     * Row-major, ordered representation of the matrix.
     */
	int data[][];
	
	/**
     * Construct a matrix with an array.
	 * 
	 * @param data -- a 2D integer array of data used to initialize the new Matrix object
	 */
	public Matrix(int data[][])
	{
        // data.length is the number of 1D arrays in the 2D array
		numRows = data.length;

		if (numRows == 0) {
			numColumns = 0;
		}
        else {
            // data[0] is the first 1D array
			numColumns = data[0].length;
		}

        // create a new matrix and copy the data
		this.data = new int[numRows][numColumns];
		for(int rCurs = 0; rCurs < numRows; rCurs++) { 
			for(int cCurs = 0; cCurs < numColumns; cCurs++) {
				this.data[rCurs][cCurs] = data[rCurs][cCurs];
			}
		}
	}
	
	/**
	 * Retrieve the number of rows in the matrix.
	 * 
	 * @return -- the number of rows in the matrix.
	 */
	public int getNumRows() {
		return this.numRows;
	}
	
	/**
	 * Retrieve the number of columns of the matrix.
	 * 
	 * @return -- the number of columns in the matrix.
	 */
	public int getNumColumns() {
		return this.numColumns;
	}
	
	/**
	 * Retrieve an element in the matrix.
	 * 
	 * @param row -- row of the element
	 * @param col -- column of the element
	 * @return -- element at (row, col)
	 */
	public int get(int row, int col) {
		return this.data[row][col];
	}
	
	/**
	 * Determine whether two objects are equivalent Matrices.
	 * 
	 * @param other -- an object to compare this to
	 * @return -- true if the Matrices are equivalent and false otherwise
     *      (or if the second object isn't a Matrix)
	 */
	@Override
	public boolean equals(Object other)
	{
        // cast to matrix if appropriate
		if(!(other instanceof Matrix)) {
			return false;
		}
		Matrix matrix = (Matrix)other;
		
        // use toString to determine if they are equal
		if (this.toString().equals(matrix.toString())) {
			return true;
		}
		return false;
	}
	
	/**
	 * Represent this Matrix as a string.
	 * 
	 * Each line of the string represents a row of the matrix, and is separated
     * by '\n'. The last row has a trailing newline character. Each element of
     * the line is separated by a space (' '), the last element of each line
     * does not have a trailing space.
	 * 
	 * @return -- string representation of the matrix
	 */
	@Override
	public String toString()
	{
		String repr = "";
		
		int numRows = this.getNumRows();
		int numCols = this.getNumColumns();
		
		for (int rCurs = 0; rCurs < numRows; rCurs++) {
			for (int cCurs = 0; cCurs < numCols; cCurs++) {
				repr += this.get(rCurs, cCurs);
				
                // if column cursor is not at the last element of the row
				if (cCurs != numCols - 1) {
					repr += " ";
				}
			}
			repr += "\n";
		}
		
	    return repr;
	}
	
	/**
	 * Multiply two matrices.
	 * 
	 * In order for the method to succeed, the matrices must have compliant
     * dimensions. That is, the matrix on the left-hand side of the operator
     * must have the same number of columns as the matrix on the right-hand
     * side of the operator has rows.
     *
     * If this condition is not met, the method returns null.
	 * 
	 * @param matrix -- the right hand side matrix to be multiplied
	 * @return -- the resulting matrix of the multiplication, null if the
     *      matrices' dimensions are not compliant
	 */
	public Matrix times(Matrix matrix) {
		if (this.getNumColumns() != matrix.getNumRows()) {
			return null;
		}
		
		int productRows = this.getNumRows();
		int productCols = matrix.getNumColumns();
		
		int[][] product = new int[productRows][productCols];
		
		for (int rCurs = 0; rCurs < productRows; rCurs++) {
			for (int cCurs = 0; cCurs < productCols; cCurs++) {
				// every entry (rCurs-th row, cCurs-th col) in the product 
                //   matrix is just the dot product of the rCurs-th row of the
                //   first matrix and the cCurs-th col of the second matrix.
				int[] row = new int[this.getNumColumns()];
				int[] col = new int[matrix.getNumRows()];
				
				// retrieve the 'row' vector
				for (int curs = 0; curs < this.getNumColumns(); curs++) {
					row[curs] = this.get(rCurs, curs);
				}
				// retrieve the 'col' vector
				for (int curs = 0; curs < matrix.getNumRows(); curs++) {
					col[curs] = matrix.get(curs, cCurs);
				}
				
				// calculate the dot product and assign to the product matrix's appropriate entry
				product[rCurs][cCurs] = this.dotProduct(row, col);
			}
		}
		
		return new Matrix(product);
	}
	
	/**
	 * Calculate the dot product between two vectors.
	 * 
	 * Returns null if the two vectors arent in the same dimensions.
	 * 
	 * @param vec1 -- first vector to calculate the dot product with
	 * @param vec2 -- second vector to calculate the dot product with
	 * @return -- dot product of the first vector and the second vector
	 */
	private Integer dotProduct(int[] vec1, int[] vec2) {
		if (vec1.length != vec2.length) {
			return null;
		}
		
		Integer dotProduct = 0;
		
        // multiply each entry and add to the dot product
		for (int cursor = 0; cursor < vec1.length; cursor++) {
			dotProduct += (vec1[cursor] * vec2[cursor]);
		}
		
		return dotProduct;
	}
	
	/**
     * Add two matrices.
	 * 
	 * For the addition to succeed, the two matrices must have the same dimensions.
	 * If this condition is not met, the method returns null.
	 * 
	 * @param matrix -- the right hand side matrix to be added
	 * @return -- the resulting matrix of the addition, null if the matrices' dimensions are not compliant
	 */
	public Matrix plus(Matrix matrix)
	{
		if (this.getNumRows() != matrix.getNumRows() || this.getNumColumns() != matrix.getNumColumns()) {
			return null;
		}
		
		int numRows = this.getNumRows();
		int numCols = this.getNumColumns();
		
		int[][] sum = new int[numRows][numCols];
		
		for (int i = 0; i < numRows; i++) {
			for (int j = 0; j < numCols; j++) {
				sum[i][j] = this.get(i, j) + matrix.get(i, j);
			}
		}
		
		return new Matrix(sum);
	}
}
