package timer;

/**
 * A functional interface for timing methods.
 * 
 * This interface provides a standardized way for analytics tools to time
 * java methods. The interface is intended to be implemented for each
 * method and all setup and timing functionality is to be enclosed in the
 * singular function.
 * 
 * @author Maxwell Hanson
 * @since July 11, 2018
 * @version 1.0.0
 */
@FunctionalInterface
public interface Timeable {
	/**
	 * Time how long it takes a method to return given a value of 'n'.
	 * 
	 * @param n -- abstracted 'input' for the method. Parameters for method are calculated when
	 *     the interface is implemented on a method-to-method basis.
	 * @return -- milliseconds it took method to exit
	 */
	public int time(int n);
}
