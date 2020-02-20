package assignment11;

/**
 * This class represents a flight that travels between 2 airports.
 * It stores all the data for the flight and acts as an edge in a 
 * graph data structure.
 * 
 * @author Maxwell Hanson
 * @since Apr 10, 2018
 */
public class Flight {
	/**
	 * Origin airport of the flight.
	 */
	private Airport orig;
	
	/**
	 * Destination airport of the flight.
	 */
	private Airport dest;
	
	/**
	 * Flight carrier.
	 */
	private String carr;
	
	/**
	 * Flight delay time.
	 */
	private double delay;
	
	/**
	 * Cancellation status of flight.
	 * 
	 * if 1, then cancelled. Else not cancelled. No other values used.
	 */
	private double cancelled;
	
	/**
	 * Duration of the flight
	 */
	private double dur;
	
	/**
	 * distance the flight goes.
	 */
	private double dist;
	
	/**
	 * Cost of the ticket to take the flight.
	 */
	private double cost;
	
	
	/**
	 * Construct a new flight.
	 * 
	 * @param orig -- the starting point of the flight
	 * @param dest -- the ending point of the flight
	 * @param carr -- the carrier of the flight, a two-letter code
	 * @param delay -- the number of minutes that the flight was delayed for
	 * @param cancelled -- whether or not the flight was cancelled or not
	 * @param dur -- how long the flight lasts
	 * @param dist -- how far the flight will travel
	 * @param price -- how expensive the flight is to purchase a ticket for
	 */
	public Flight(Airport orig, Airport dest, String carr,
			double delay, double cancelled, double dur, double dist, double price) {
		this.orig = orig;
		this.dest = dest;
		this.carr = carr;
		this.delay = delay;
		this.cancelled = cancelled;
		this.dur = dur;
		this.dist = dist;
		this.cost = price;
	}
	
	/**
	 * Retrieve the origin of the flight.
	 * 
	 * @return -- the airport of flight origin
	 */
	public Airport orig() {
		return orig;
	}
	
	/**
	 * Retrieve the destination of the flight.
	 * 
	 * @return -- the airport of flight destination
	 */
	public Airport dest() {
		return dest;
	}
	
	/**
	 * Retrieve the carrier of the flight.
	 * 
	 * @return -- carrier fo the flight
	 */
	public String carr() {
		return carr;
	}
	
	/**
	 * Retrieve the flight departure delay.
	 * 
	 * @return -- flight delay time
	 */
	public double delay() {
		return delay;
	}
	
	/**
	 * Retrieve the cancellation status of the flight.
	 * 
	 * @return -- 1 if flight cancelled, 0 otherwise
	 */
	public double cancelled() {
		return cancelled;
	}
	
	/**
	 * Retrieve the duration of the flight.
	 * 
	 * @return -- flight duration time
	 */
	public double dur() {
		return dur;
	}
	
	/**
	 * Retrieve the distance of the flight.
	 * 
	 * @return -- distance of flight
	 */
	public double dist() {
		return dist;
	}
	
	/**
	 * Retrieve the price of the flight.
	 * 
	 * @return -- ticket price
	 */
	public double price() {
		return cost;
	}
}
