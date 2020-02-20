package assignment11;

import java.io.FileNotFoundException;
import java.io.PrintWriter;
import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Random;

/**
 * <p>This class will generate random flights for you to test. If you want to make
 * your own just ignore this file. You do not need to know anything about the
 * implementation, you will not be tested on it, it is just made to be a help if
 * you are having a hard time generating files.</P>
 *
 * <p>We suggest using this file to generate some random data then modify it
 * slightly to test an edge case. That way you have several other flights your
 * algorithm will have to check as well as the correct one.</p>
 *
 * <p><b>USAGE:</b> You can use this class just by changing the commented global
 * variables at the top of the file. For students interested in what was done in lab
 * with the command line you can also compile it with <code>javac</code> and give it 3
 * command-line parameters: (1)The number of edges you want in the graph, (2)the number
 * of nodes in the graph, and (3)the name of the file for the .csv file to output to.
 * Keep in mind dense graphs will have shorter paths and sparse graphs may not have paths
 * to every node.</p>
 *
 * @author CS2420 Teaching Staff - Spring 2018
 */
public class GenerateFlightData {
	
	// ONLY EDIT THESE THREE VARIABLES
	private static int edgeCount = 15;
	private static int nodeCount = 5;
	private static String filePath = "testfile.csv";

	// EDITING ANYTHING BELOW THIS MAY SCREW UP THE CODE SO DO SO AT YOUR OWN RISK.
	private static final int BASE_COST = 200;
	private static final double TIME_DISTANCE_RATIO = 6.9;
	
	private static ArrayList<String> airports = new ArrayList<String>();
	private static ArrayList<String> addedAirports = new ArrayList<String>();
	private static ArrayList<String> airliners = new ArrayList<String>(Arrays.asList("AA", "DL", "EV", "HA", "MQ", "OO", "UA", "WN"));
	private static Random r = new Random();
	
	private static HashMap<NodeDistance, Integer> flightDistances = new HashMap<NodeDistance, Integer>();
	private static HashMap<String, Double> airlinerCostRatio = new HashMap<String, Double>();

	/**
	 * @param args
	 * @throws FileNotFoundException
	 */
	public static void main(String[] args) {
		
		if (args.length > 0) {
			edgeCount = Integer.parseInt(args[0]);
			nodeCount = Integer.parseInt(args[1]);
			filePath = args[2];
		}
		
		generateRandomAirports(nodeCount);
		generateRandomAirlinerCosts();
		
		try(PrintWriter writer = new PrintWriter(filePath)) {
		writer.write("ORIGIN,DESTINATION,CARRIER,DELAY,CANCELED,TIME,DISTANCE,COST\n");
		
		for (int i = 0; i < edgeCount; i++) {
			
			String origin = getRandomAirportOrigin();
			String destination = getRandomAirportDestination(origin);
			String airliner = getRandomAirLiner();
			String randomDepatureDelay = generateRandomDepatureDelay();
			int randomDistance = generateDistance(origin, destination);
			int estimatedFlightTime = generateEstimatedFlightTime(origin, destination);
			
			String s = origin + "," +
					   destination + "," +
					   airliner + "," +
					   randomDepatureDelay + "," +
					   generateDivertedOrCanceled() + "," +
					   estimatedFlightTime + "," +
					   randomDistance + "," +
					   (new DecimalFormat("#.##")).format(generatePrice(origin,destination, airliner)) + "\n";
			if (i == edgeCount - 1)
				s = s.substring(0, s.length() - 1);
			writer.write(s);
		}
		
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		}
		
		System.out.println("NodeCount: " + nodeCount + "\nEdgeCount: " + edgeCount + "\nFilePath: " + System.getProperty("user.dir") + "\\" + filePath);
	}

	private static int generateEstimatedFlightTime(String origin, String destination) {
		return (int)(flightDistances.get(new NodeDistance(origin, destination)) / TIME_DISTANCE_RATIO);
	}

	private static void generateRandomAirlinerCosts() {
		for (String s : airliners) {
			double ratio = r.nextDouble();
			airlinerCostRatio.put(s, (ratio > .5 ? ratio - .5 : ratio));
		}
	}

	private static String generateDivertedOrCanceled() {
		return r.nextDouble() < .4 ? "1" : "0";
	}

	private static String generateRandomDepatureDelay() {
		return Integer.toString(r.nextInt(500));
	}

	private static String getRandomAirLiner() {
		return airliners.get(r.nextInt(airliners.size()));
	}

	private static String getRandomAirportDestination(String airport) {
		String s = airports.get(r.nextInt(airports.size()));
		
		while (s.equals(airport))
			s = airports.get(r.nextInt(airports.size()));
		
		return s;
	}

	private static String getRandomAirportOrigin() {
		String s = airports.get(r.nextInt(airports.size()));
		
		while (addedAirports.contains(s))
			s = airports.get(r.nextInt(airports.size()));
		
		return s;
	}

	public static void generateRandomAirports(int airportCount)	{
		while (airports.size() < airportCount) {
			String airport = generateAirport();
			
			if (airports.contains(airport))
				continue;
			
			airports.add(airport);
		}
	}

	private static String generateAirport() {
		return generateRandomLetter() + generateRandomLetter() + generateRandomLetter();
	}
	
	private static String generateRandomLetter() {
		return Character.toString(((char) (65 + r.nextInt(26))));
	}
	
	private static double generatePrice(String origin, String destination, String airliner) {
		NodeDistance n = new NodeDistance(origin, destination);
		return BASE_COST + flightDistances.get(n) * airlinerCostRatio.get(airliner);
	}
	
	private static int generateDistance(String origin, String destination) {
		NodeDistance n = new NodeDistance(origin, destination);
		if (flightDistances.containsKey(n))
			return flightDistances.get(n);
		n.distance = 200 + r.nextInt(2800);
		flightDistances.put(n, n.distance);
		return n.distance;
	}
	
	private static class NodeDistance {
		String origin;
		String destination;
		int distance;
		
		private NodeDistance(String origin, String destination) {
			this.origin = origin;
			this.destination = destination;
		}
		
		@Override
		public boolean equals(Object o) {
			if (o instanceof NodeDistance)
				return (this.origin.equals(((NodeDistance)o).origin) &&
					    this.destination.equals(((NodeDistance)o).destination)) ||
					   (this.destination.equals(((NodeDistance)o).origin) &&
						this.origin.equals(((NodeDistance)o).destination));
			return false;
		}
		
		@Override
		public int hashCode() {
			return origin.hashCode() + destination.hashCode();
		}
	}
}
