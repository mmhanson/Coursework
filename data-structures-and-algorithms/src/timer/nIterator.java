package timer;

/**
 * An interface similar to an iterator, but narrowed to iterate over
 * possible values of 'n' for asymptotic analysis. Includes
 * functionality for reseting iteration so one instance can be used
 * for many analyses.
 * 
 * @author Maxwell Hanson
 * @since Jul 14, 2018
 * @version 1.0.0
 */
public interface nIterator {
	/**
	 * Reset the iterator to the first value
	 */
	public void reset();
	
	/**
	 * Get the value.
	 */
	public Integer next();
	
	/**
	 * Determine if there is a next value.
	 */
	public boolean hasNext();
}
