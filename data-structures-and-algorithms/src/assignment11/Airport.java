package assignment11;

import java.util.ArrayList;
import java.util.List;

/**
 * This class represents an airport, which is used to connect flights.
 * 
 * It acts a Node in a graph data structure. 
 * 
 * @author Maxwell Hanson
 * @since Apr 10, 2018
 * @version 1.1.0
 */
public class Airport {
	/**
	 * Departing flights
	 */
	private List<Flight> flights;
	
	/**
	 * Name of the airport.
	 */
	private String name;
	
	/**
	 * Marker for performing dijkstra's algorithm on a network of airports.
	 */
	private Airport parent;
	
	/**
	 * cost to get to the airport, used in dijkstra's
	 */
	private double cost;
	
	/**
	 * Flag for permanent cost in dijkstra's
	 */
	private boolean perm;
	
	/**
	 * Construct an Airport.
	 * 
	 * @param name -- the three letter, unique code of the airport
	 */
	public Airport(String name) {
		this.name = name;
		flights = new ArrayList<Flight>();
		parent = null;
		perm = false;
	}
	
	/**
	 * Add a departing flight to the airport.
	 * 
	 * @param flight -- the flight to add to the airport
	 */
	public void addFlight(Flight flight) {
		flights.add(flight);
	}
	
	/**
	 * Retrieve all departing flights of the airport.
	 * 
	 * @return -- all flights leaving this airport
	 */
	public List<Flight> getFlights() {
		return flights;
	}
	
	/**
	 * Retrieve the name of the airport.
	 * 
	 * @return -- the airport's name
	 */
	public String getName() {
		return name;
	}
	
	/**
	 * Retrieve the cost of the airport.
	 * 
	 * @return The airport's cost
	 */
	public double getCost() {
		return cost;
	}
	
	
	/**
	 * Set the cost of the airport.
	 * 
	 * @param cost -- The cost to be set
	 * @return -- The new cost
	 */
	public double setCost(double cost) {
		this.cost = cost;
		return cost;
	}
	
	/**
	 * Get the parent of the airport.
	 * 
	 * @return -- the airport's parent airport
	 */
	public Airport getParent() {
		return parent;
	}
	
	/**
	 * Set the parent airport.
	 * 
	 * @param newParent -- The parent to be set
	 * @return -- The new parent airport
	 */
	public Airport setParent(Airport newParent) {
		parent = newParent;
		return parent;
	}
	
	/**
	 * Set the airport's cost to permanent.
	 */
	public void setPerm() {
		perm = true;
	}
	
	/**
	 * Set the permanence status to false.
	 */
	public void unsetPerm() {
		perm = false;
	}
	
	/**
	 * Determine the permanence status of the airport.
	 * 
	 * @return -- true if the cost is permanent, false otherwise
	 */
	public boolean isPerm() {
		return perm;
	}
}
