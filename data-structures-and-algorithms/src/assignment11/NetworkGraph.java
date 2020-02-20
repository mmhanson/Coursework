package assignment11;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.PriorityQueue;

/**
 * <p>This class represents a graph of flights and airports along with specific
 * data about those flights. It is recommended to create an airport class and a
 * flight class to represent nodes and edges respectively. There are other ways
 * to accomplish this and you are free to explore those.</p>
 * 
 * <p>Testing will be done with different criteria for the "best" path so be
 * sure to keep all information from the given file. Also, before implementing
 * this class (or any other) draw a diagram of how each class will work in
 * relation to the others. Creating such a diagram will help avoid headache and
 * confusion later.</p>
 * 
 * <p>Be aware also that you are free to add any member variables or methods
 * needed to completed the task at hand</p>
 * 
 * @author CS2420 Teaching Staff - Spring 2018
 * @author Maxwell Hanson
 * @since Apr 10, 2018
 * @version 1.0.0
 */
public class NetworkGraph {
	/**
	 * A set of all the airports (nodes) of the graph
	 */
	private HashSet<Airport> airports;
	
	/**
	 * <p>Constructs a NetworkGraph object and populates it with the information
	 * contained in the given file. See the sample files or a randomly generated
	 * one for the proper file format.</p>
	 * 
	 * <p>You will notice that in the given files there are duplicate flights with
	 * some differing information. That information should be averaged and stored
	 * properly. For example some times flights are canceled and that is
	 * represented with a 1 if it is and a 0 if it is not. When several of the same
	 * flights are reported totals should be added up and then reported as an
	 * average or a probability (value between 0-1 inclusive).</p>
	 * 
	 * @param flightInfo - The inputstream to the flight data. The format is a
	 * *.csv(comma separated value) file
	 */
	public NetworkGraph(InputStream flightInfo)  {
		airports = new HashSet<Airport>();
		
		BufferedReader buff = null;
		
		// parse each flight and add it to the corresponding airport
		try {
			buff = new BufferedReader(new InputStreamReader(flightInfo));
			
			String line;
			buff.readLine(); // skip header
			while ( (line = buff.readLine()) != null) {
				parseFlight(line);
			}
			
			buff.close();
		}
		catch (IOException e) {
			throw new RuntimeException(e);
		}
	}
	
	/**
	 * Construct a flight from the line of a csv file.
	 * 
	 * @param line -- the line of the csv to construct the flight from
	 * @return -- flight from the line of a CSV file
	 */
	private Flight parseFlight(String line) {
		Flight flight;
		String[] splitLine = line.split(",");
		
		// create objects for attributes of the flight
		Airport origPort;
		Airport destPort;
		String carrier;
		double delay, cancelled, dur, dist, price;
		
		// parse values
		origPort = getAirport(splitLine[0]);
		destPort = getAirport(splitLine[1]);
		carrier = splitLine[2];
		delay = Double.parseDouble(splitLine[3]);
		cancelled = Double.valueOf(splitLine[4]);
		dur = Double.parseDouble(splitLine[5]);
		dist = Double.parseDouble(splitLine[6]);
		price = Double.parseDouble(splitLine[7]);
		
		flight = new Flight(origPort, destPort, carrier, delay, cancelled, dur, dist, price);
		origPort.addFlight(flight);
		
		return flight;
		
	}

	/**
	 * This method returns a BestPath object containing information about the best
	 * way to fly to the destination from the origin. "Best" is defined by the
	 * FlightCriteria parameter <code>enum</code>. This method should throw no
	 * exceptions and simply return a BestPath object with information dictating
	 * the result. If the destination or origin is not contained in this instance of
	 * NetworkGraph, simply return a BestPath object with an empty path
	 * (not a <code>null</code> path) and a path cost of 0. If origin or destination
	 * are <code>null</code>, also return a BestPath object with an empty path and a
	 * path cost of 0 .  If there is no path in this NetworkGraph from origin to
	 * destination, also return a BestPath with an empty path and a path cost of 0.
	 * 
	 * @param origin - The starting location to find a path from. This should be a
	 *   3 character long string denoting an airport.
	 * @param destination - The destination location from the starting airport.
	 *   Again, this should be a 3 character long string denoting an airport.
	 * @param criteria - This enum dictates the definition of "best". Based on this
	 *   value a path should be generated and return.
	 * @return - An object containing path information including origin, destination,
	 *   and everything in between.
	 */
	public BestPath getBestPath(String origin, String destination, FlightCriteria criteria) {
		return getBestPath(origin, destination, criteria, null);
	}
	
	/**
	 * <p>This overloaded method should do the same as the one above only when looking for paths
	 * skip the ones that don't match the given airliner.</p>
	 * 
	 * @param origin - The starting location to find a path from. This should be a
	 * 3 character long string denoting an airport.
	 * 
	 * @param destination - The destination location from the starting airport.
	 * Again, this should be a 3 character long string denoting an airport.
	 * 
	 * @param criteria - This enum dictates the definition of "best". Based on this
	 * value a path should be generated and return.
	 * 
	 * @param airliner - a string dictating the airliner the user wants to use exclusively. Meaning
	 * no flights from other airliners will be considered.
	 * 
	 * @return - An object containing path information including origin, destination,
	 * and everything in between.
	 */
	public BestPath getBestPath(String origin, String destination, FlightCriteria criteria, String airliner) {
		// null case
		if (origin == null || destination == null) {
			return new BestPath(new ArrayList<String>(), 0.0);
		}
		
		// convert airport names to airport objects
		Airport originAirport = getAirport(origin);
		Airport destAirport = getAirport(destination);
		// case in which airports arent in network
		if (originAirport == null || destAirport == null) {
			return new BestPath(new ArrayList<String>(), 0.0);
		}
		
		return getBestPath(originAirport, destAirport, criteria, airliner);
	}
	
	/**
	 * Determine the cheapest way to get from one airport to another.
	 * 
	 * "Cheapness" is described in the criteria parameter.
	 * Exclusive use of an airline can be handled with the airline
	 * parameter. If this parameter is null, then any airline is
	 * considered valid. If it is not null, then only airlines
	 * with two-letter codes matching this string will be used
	 * in the path. If the code does not match any airline, then
	 * an empty path will be returned.
	 * 
	 * @param origAirport -- the airport to generate path from
	 * @param destAirport -- the airport to generate the path to
	 * @param criteria -- the definition of "cheapest" flight
	 * @param airline -- the code of the airline to exclusively use
	 *   on our path. If null, then all airlines can be used.
	 * @return -- the cheapest path from the origin to the destination
	 *   if no path exists, then an empty path.
	 */
	private BestPath getBestPath(Airport origAirport, Airport destAirport,
			FlightCriteria criteria, String airline) {
		Airport cursor;
		// total path cost, initialized to zero to cover the case in which
		// there is no path.
		double pathCost = 0.0;
		// priority queue handles the cost. Elements with lowest cost are dequeued
		PriorityQueue<Airport> portQ = new PriorityQueue<Airport>(100, 
				(lhs, rhs) -> Double.compare(lhs.getCost(), rhs.getCost()));
		
		// reset markers on airport classes
		for (Airport port: airports) {
			port.setCost(Double.MAX_VALUE);
			port.unsetPerm();
			port.setParent(null);
		}
		origAirport.setCost(0);
		
		// perform Dijkstra's algorithm to find shortest paths
		portQ.offer(origAirport);
		// while there is a "temporary" node
		while( (cursor = portQ.poll()) != null ) {
			if (cursor.equals(destAirport)) {
				// handle case when origin and dest are equal
				if (cursor.equals(origAirport)) {
					break;
				}
				
				pathCost = cursor.getCost();
				
				// build shortest path by starting at the goal and going backwards by parents
				ArrayList<Airport> revShortestPath = new ArrayList<Airport>();
				cursor = destAirport;
				while (cursor != null) {
					revShortestPath.add(cursor);
					cursor = cursor.getParent();
				}
				// reverse list so origin is first
				// list is also converted to a list of airport names rather than objects
				ArrayList<String> shortestPath = new ArrayList<String>();
				for (int curs = revShortestPath.size() - 1; curs >= 0; curs--) {
					shortestPath.add(revShortestPath.get(curs).getName());
				}
				
				return new BestPath(shortestPath, pathCost);
			}
			cursor.setPerm();
			
			// assign costs to adjacent, temporary airports
			for (Airport connAirport: getConnAirports(cursor)) {
				if (connAirport.isPerm()) {
					continue;
				}
				
				double costTo = cost(cursor, connAirport, criteria, airline);
				double costFromOrig;
				if (criteria == FlightCriteria.CANCELED) {
					costFromOrig = (cursor.getCost() + costTo) / 2;
				}
				else {
					costFromOrig = cursor.getCost() + costTo;
				}
				
				if (costFromOrig < connAirport.getCost()) {
					connAirport.setCost(costFromOrig);
					connAirport.setParent(cursor);
					portQ.offer(connAirport);
				}
			}
		}
		
		// this line is only reached when the origin cannot be connected
		// to the destination--return empty path
		return new BestPath(new ArrayList<String>(), 0.0);
	}
	
	/**
	 * Return the Airport instance corresponding to the name.
	 * 
	 * If the instance is not in the list of airports, then one is created
	 * , added to the master list of airports, and returned
	 * 
	 * @param name -- code of the airport to search for
	 * @return -- instance representing that airport
	 */
	private Airport getAirport(String name) {
		for (Airport airport: airports) {
			if (airport.getName().equals(name)) {
				return airport;
			}
		}
		
		Airport newPort = new Airport(name);
		airports.add(newPort);
		
		return newPort;
	}
	
	/**
	 * Determine all the airports connected to a given airport.
	 * 
	 * @param airport -- the airport to operate on
	 * @return -- a list of airports that this airport is connected to via its flights
	 */
	private List<Airport> getConnAirports(Airport airport) {
		ArrayList<Flight> flights = (ArrayList<Flight>) airport.getFlights();
		ArrayList<Airport> connAirports = new ArrayList<Airport>();
		
		for (Flight flight: flights) {
			Airport destAirport = flight.dest();

			if (!connAirports.contains(destAirport)) {
				connAirports.add(destAirport);
			}
		}
		
		return connAirports;
	}
	
	/**
	 * Calculate the minimal cost to get from on airport to another with certain criteria.
	 * 
	 * @param orig -- airport calculate the cost of leaving
	 * @param dest -- airport to calculate the cost of arriving
	 * @param criteria -- the criteria for defining the cost
	 * @param airline -- the airline to restrict results to. If null, then all airlines
	 *   are considered valid. If not null, then it is considered a two-letter code
	 *   corresponding to an airline. In which case, only flights from that airline are
	 *   considered. If the string does not match an airline, then Double.MAX_VALUE 
	 *   (considered infinity) is returned.
	 * @return -- the minimal cost to get from {@param from} to {@param to}
	 */
	private double cost(Airport orig, Airport dest, FlightCriteria criteria, String airline) {
		// compile a list of flights going from the origin to dest
		ArrayList<Flight> flights = (ArrayList<Flight>) orig.getFlights();
		ArrayList<Flight> flightsClone = new ArrayList<Flight>(); // cloned to prevent modification of attribute
		for (Flight flight: flights) {
			// check cost as well to ensure negative costs are not counted
			if (flight.dest().equals(dest) && flight.price() >= 0.0) {
				flightsClone.add(flight);
			}
		}
		// narrow list for airline if applicable
		if (airline != null) {
			flightsClone.removeIf(flight -> !(flight.carr().equals(airline)));
		}
		
		// if not connected
		if (flightsClone.isEmpty()) {
			return Double.MAX_VALUE;
		}
		
		// average cost and return
		double totalCost = 0.0;
		for (Flight flight: flightsClone) {
			totalCost += cost(flight, criteria);
		}
		
		return (totalCost / flightsClone.size());
	}
	
	/**
	 * Calculate the cost of a flight with a criteria.
	 * 
	 * @param flight -- the flight to calculate the cost of
	 * @param criteria -- the criteria to use to determine the cost of the flight
	 * @return -- cost of the flight
	 */
	private double cost(Flight flight, FlightCriteria criteria) {
		switch (criteria) {
		case PRICE:
			return flight.price();
		case DELAY:
			return flight.delay();
		case DISTANCE:
			return flight.dist();
		case CANCELED:
			return flight.cancelled();
		case TIME:
			return flight.dur();
		default:
			return Double.MAX_VALUE; // reached if no criteria set
		}
	}
}
