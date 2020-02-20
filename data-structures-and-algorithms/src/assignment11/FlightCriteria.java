/**
 *
 */
package assignment11;

/**
 * <p>FlightCriteria are used to designate what type of "best" is
 * looked for while path finding. Usage of enumerations are very
 * simple; see the java documentation.<br><p>
 *
 * Your code should support the following flight path criteria.
 *
 *<li>{@link #PRICE}</li>
 *<li>{@link #DELAY}</li>
 *<li>{@link #DISTANCE}</li>
 *<li>{@link #CANCELED}</li>
 *<li>{@link #TIME}</li>
 *
 *@author CS2420 Teaching Staff - Spring 2018
 */
public enum FlightCriteria {
	/**
	 * Designates the shortest path by price of the flight.
	 */
	PRICE,
	
	/**
	 * Designates the shortest path by delay of departure.
	 */
	DELAY,
	
	/**
	 * Designates the shortest path by distance in miles.
	 */
	DISTANCE,
	
	/**
	 * Designates the path with the least chance of being canceled.
	 */
	CANCELED,
	
	/**
	 * Designates the path with shortest average actual flight time.
	 */
	TIME
}
