package assignment11;

import static org.junit.Assert.*;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.Arrays;

import org.junit.Before;
import org.junit.Test;

/**
 * Test the "Network Graph" class.
 * 
 * @author Max Hanson
 * @since Apr 17, 2018
 * @version 1.3.0
 */
public class NetworkGraphTests {
	private NetworkGraph graph;
	private NetworkGraph smallGraph;
	
	/**
	 * Setup attributes for testing
	 * 
	 * @throws FileNotFoundException -- if "flights-2017-q3.csv" or 
	 * 		"smallListOfFLights.csv" are not found
	 */
	@Before
	public void setup() throws FileNotFoundException {
		graph = new NetworkGraph(new FileInputStream(new File("flights-2017-q3.csv")));
		smallGraph = new NetworkGraph(new FileInputStream(new File("smallListOfFlights.csv")));
	}
	
	// === LARGE GRAPH TESTS ===
	// note these are the same cases from the "FindBestPathTester.java"
	// class.
	
	/**
	 * Test a random flights based on cost.
	 * 
	 * NIL 2 HIB
	 * CDV 2 YAK
	 * SAV 2 BQK
	 * IMT 2 RHI
	 * FCA 2 MSO
	 * RHI 2 IMT
	 */
	@Test
	public void testRandFlightsCost() {
		BestPath res;
		
		res = graph.getBestPath("DVL", "JMS", FlightCriteria.PRICE);
		System.out.println(res.toString());
		
		res = graph.getBestPath("NIL", "HIB", FlightCriteria.PRICE);
		System.out.println(res.toString());
		
		res = graph.getBestPath("CDV", "YAK", FlightCriteria.PRICE);
		System.out.println(res.toString());
		
		res = graph.getBestPath("SAV", "BQK", FlightCriteria.PRICE);
		System.out.println(res.toString());
		
		res = graph.getBestPath("IMT", "RHI", FlightCriteria.PRICE);
		System.out.println(res.toString());
		
		res = graph.getBestPath("FCA", "MSO", FlightCriteria.PRICE);
		System.out.println(res.toString());
		
		res = graph.getBestPath("RHI", "IMT", FlightCriteria.PRICE);
		System.out.println(res.toString());
	}
	
	/**
	 * Test path-finding based on distance.
	 */
	@Test
	public void testDistance() {
		ArrayList<String> expPath = new ArrayList<String>();
		expPath.add("MOB");
		expPath.add("DFW");
		expPath.add("SFO");
		expPath.add("ACV");
		BestPath expRes = new BestPath(expPath, 2253.0);
		
		BestPath res = graph.getBestPath("MOB", "ACV", FlightCriteria.DISTANCE);
		
		// check that paths are equal and cost is within 0.0001 error
		assertTrue(Arrays.equals(expRes.getPath().toArray(), res.getPath().toArray()));
		assertTrue(Math.abs(expRes.getPathCost() - res.getPathCost()) <= 0.0001);
	}
	
	/**
	 * Test path-finding based on distance with a carrier.
	 */
	@Test
	public void testDistanceCarrier() {
		ArrayList<String> expPath = new ArrayList<String>();
		expPath.add("SFO");
		expPath.add("SLC");
		expPath.add("DFW");
		BestPath expRes = new BestPath(expPath, 1588.0);
		
		BestPath res = graph.getBestPath("SFO", "DFW", FlightCriteria.DISTANCE, "DL");
		
		// check that paths are equal and cost is within 0.0001 error
		assertTrue(Arrays.equals(expRes.getPath().toArray(), res.getPath().toArray()));
		assertTrue(Math.abs(expRes.getPathCost() - res.getPathCost()) <= 0.0001);
	}
	
	/**
	 * Test path-finding based on time.
	 */
	@Test
	public void testTime() {
		ArrayList<String> expPath = new ArrayList<String>();
		expPath.add("MOB");
		expPath.add("DFW");
		expPath.add("SLC");
		BestPath expRes = new BestPath(expPath, 269.2534);
		
		BestPath res = graph.getBestPath("MOB", "SLC", FlightCriteria.TIME);
		
		// check that paths are equal and cost is within 0.0001 error
		assertTrue(Arrays.equals(expRes.getPath().toArray(), res.getPath().toArray()));
		assertTrue(Math.abs(expRes.getPathCost() - res.getPathCost()) <= 0.0001);
	}
	
	/**
	 * Test path-finding based on price.
	 */
	@Test
	public void testPrice() {
		ArrayList<String> expPath = new ArrayList<String>();
		expPath.add("LAS");
		expPath.add("LAX");
		BestPath expRes = new BestPath(expPath, 138.39);
		
		BestPath res = graph.getBestPath("LAS", "LAX", FlightCriteria.PRICE);
		
		// check that paths are equal and cost is within 0.0001 error
		assertTrue(Arrays.equals(expRes.getPath().toArray(), res.getPath().toArray()));
		assertTrue(Math.abs(expRes.getPathCost() - res.getPathCost()) <= 0.0001);
	}
	
	/**
	 * Test multiple consecutive calls.
	 * 
	 * This is tested to ensure proper resetting of markers.
	 */
	@Test
	public void testMultipleCalls() {
		BestPath res, expRes;
		ArrayList<String> expPath;
		
		expPath = new ArrayList<String>();
		expPath.add("LAS");
		expPath.add("LAX");
		expRes = new BestPath(expPath, 138.39);
		
		res = graph.getBestPath("LAS", "LAX", FlightCriteria.PRICE);
		
		// check that paths are equal and cost is within 0.0001 error
		assertTrue(Arrays.equals(expRes.getPath().toArray(), res.getPath().toArray()));
		assertTrue(Math.abs(expRes.getPathCost() - res.getPathCost()) <= 0.0001);
		
		expPath = new ArrayList<String>();
		expPath.add("MOB");
		expPath.add("DFW");
		expPath.add("SLC");
		expRes = new BestPath(expPath, 269.2534);
		
		res = graph.getBestPath("MOB", "SLC", FlightCriteria.TIME);
		
		// check that paths are equal and cost is within 0.0001 error
		assertTrue(Arrays.equals(expRes.getPath().toArray(), res.getPath().toArray()));
		assertTrue(Math.abs(expRes.getPathCost() - res.getPathCost()) <= 0.0001);
		
		expPath = new ArrayList<String>();
		expPath.add("SFO");
		expPath.add("SLC");
		expPath.add("DFW");
		expRes = new BestPath(expPath, 1588.0);
		
		res = graph.getBestPath("SFO", "DFW", FlightCriteria.DISTANCE, "DL");
		
		// check that paths are equal and cost is within 0.0001 error
		assertTrue(Arrays.equals(expRes.getPath().toArray(), res.getPath().toArray()));
		assertTrue(Math.abs(expRes.getPathCost() - res.getPathCost()) <= 0.0001);
	}
	
	// === SMALL GRAPH TESTS ===
	
	/**
	 * Test path-finding based on distance,
	 */
	@Test
	public void testShortestDistancePath() {
		BestPath shortestDistance = smallGraph.getBestPath("SLC", "CWA", FlightCriteria.DISTANCE);
		ArrayList<String> path = new ArrayList<String>();
		path.add("SLC");
		path.add("MSP");
		path.add("CWA");
		BestPath expected = new BestPath(path, 1162);
		assertEquals(expected, shortestDistance);
	}
	
	/**
	 * Test path-finding based on cost.
	 */
	@Test
	public void testLowestCostPath() {
		BestPath lowestCost = smallGraph.getBestPath("SLC", "CWA", FlightCriteria.PRICE);
		ArrayList<String> path = new ArrayList<String>();
		path.add("SLC");
		path.add("DEN");
		path.add("ORD");
		path.add("CWA");
		BestPath expected = new BestPath(path, 33);
		assertEquals(expected, lowestCost);
	}
	
	/**
	 * Test path-finding based on time.
	 */
	@Test
	public void testShortestTimePath() {
		BestPath shortestTime = smallGraph.getBestPath("SLC", "CWA", FlightCriteria.TIME);
		ArrayList<String> path = new ArrayList<String>();
		path.add("SLC");
		path.add("MSP");
		path.add("CWA");
		BestPath expected = new BestPath(path, 210);
		assertEquals(expected, shortestTime);
	}
	
	/**
	 * Test path-finding based on cancellations.
	 */
	@Test
	public void testLeastChanceOfBeingCanceledPath() {
		BestPath res, expRes;
		ArrayList<String> path;
		
		/// cancellation chance when best path has a 0% chance
		res = smallGraph.getBestPath("SLC", "CWA", FlightCriteria.CANCELED);
		path = new ArrayList<String>();
		path.add("SLC");
		path.add("DEN");
		path.add("CWA");
		expRes = new BestPath(path, 0);
		assertEquals(expRes, res);
		
		/// cancellation chance when path has greater than
		//    a zero-percent chance of being cancelled
		// this is to ensure proper averaging of cancellation values
		res = smallGraph.getBestPath("SLC", "JFK", FlightCriteria.CANCELED);
		path = new ArrayList<String>();
		path.add("SLC");
		path.add("ATL");
		path.add("JFK");
		expRes = new BestPath(path, 0.5);
		assertEquals(expRes, res);
	}
	
	/**
	 * Test path-finding based on delay.
	 */
	@Test
	public void testShortestDelayPath() {
		BestPath shortestDelay = smallGraph.getBestPath("SLC", "CWA", FlightCriteria.DELAY);
		ArrayList<String> path = new ArrayList<String>();
		path.add("SLC");
		path.add("MSP");
		path.add("DEN");
		path.add("ORD");
		path.add("CWA");
		BestPath expected = new BestPath(path, 15);
		assertEquals(expected, shortestDelay);
	}
	
	/// === EDGE CASES ===
	
	/**
	 * Test path-finding when there is no path.
	 */
	@Test
	public void testNoPath() {
		BestPath noPath = smallGraph.getBestPath("SLC", "LAX", FlightCriteria.PRICE); // An impossible path should return an empty path
		ArrayList<String> path = new ArrayList<String>();
		BestPath expected = new BestPath(path, 0);
		assertEquals(expected, noPath);
	}
	
	/**
	 * Test path-finding when there is no such origin airport.
	 */
	@Test
	public void testNoOriginAirport() {
		BestPath noOrigin = smallGraph.getBestPath("BOI", "SLC", FlightCriteria.PRICE); // An origin airport that doesn't exist should return an empty path
		ArrayList<String> path = new ArrayList<String>();
		BestPath expected = new BestPath(path, 0);
		assertEquals(expected, noOrigin);
	}
	
	/**
	 * Test path-finding when there is no such destination airport.
	 */
	@Test
	public void testNoDestinationAirport() {
		BestPath noDestination = smallGraph.getBestPath("SLC", "BOI", FlightCriteria.PRICE); // A destination airport that doesn't exist should return an empty path
		ArrayList<String> path = new ArrayList<String>();
		BestPath expected = new BestPath(path, 0);
		assertEquals(expected, noDestination);
	}
	
	/**
	 * Test path-finding for a specific carrier when no path exists
	 *   with that carrier.
	 */
	@Test
	public void testNoPathCarrier() {
		BestPath noCarrier = smallGraph.getBestPath("SLC", "BOI", FlightCriteria.PRICE, "DA"); // An airline that doesn't exist should return an empty path
		ArrayList<String> path = new ArrayList<String>();
		BestPath expected = new BestPath(path, 0);
		assertEquals(expected, noCarrier);
	}
	
	/**
	 * Test path-finding when the origin and destination are the same.
	 */
	@Test
	public void testSameOriginDestinationAirport() {
		BestPath sameAirport = smallGraph.getBestPath("SLC", "SLC", FlightCriteria.PRICE); // A path between the same origin and destination airport should return an empty path
		ArrayList<String> path = new ArrayList<String>();
		BestPath expected = new BestPath(path, 0);
		assertEquals(expected, sameAirport);
	}
}
