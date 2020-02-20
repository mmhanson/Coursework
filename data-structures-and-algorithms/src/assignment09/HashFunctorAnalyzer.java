package assignment09;

import java.io.File;
import java.util.Arrays;
import java.util.List;
import java.util.NoSuchElementException;
import java.util.Random;
import org.jfree.data.xy.XYSeries;
import org.jfree.data.xy.XYSeriesCollection;
import timer.Timeable;
import timer.Timer;
import timer.Visualizer;
import timer.nIterator;

/**
 * Analyze the asymptotic behavior of key methods in the HashFunctor classes.
 * 
 * @author Maxwell Hanson
 * @since Jul 17, 2018
 * @version 1.0.0
 */
public class HashFunctorAnalyzer {
	/**
	 * Hash functor to be used in hash timings.
	 * 
	 * This flag is alternated instead of creating a timeable interface for
	 * each hash functor for each scenario. Quick n dirty.
	 */
	private static HashFunctor hasher;
	
	public static void main(String[] args) {
		// iterations per measurement for all timers
		int iter_count = 25;
		
		// output chart filenames
		String collisionsChartFname = "hash functor collision.png";
		String functorChartFname = "hash functor time.png";
		
		// define timeable interfaces for hash functors
		Timeable hasherCollisionTimeable = new Timeable() {
			public int time(int n) {
				// setup stack
				ChainingHashTable hashTable = new ChainingHashTable(100, hasher);
				for (int count = 0; count <= n; count++) {
					hashTable.add(createRandString(10));
				}
				
				// count collisions (time not used)
				return hashTable.collCount;
			}
		};
		Timeable hasherAdditionTimeable = new Timeable() {
			public int time(int n) {
				// setup stack
				ChainingHashTable hashTable = new ChainingHashTable(100, hasher);
				for (int curs = 0; curs < n; curs++) {
					hashTable.add(createRandString(10));
				}
				
				// time performance
				long startTime = System.nanoTime();
				for (int count = 0; count < 1000; count++) {
					hashTable.add(createRandString(10));
				}
				long stopTime = System.nanoTime();
				
				return (int) (stopTime - startTime);
			}
		};

		nIterator iter = new nIterator() {
			List<Integer> values = Arrays.asList(10, 11, 12, 13, 14, 15);
			private int nextIdx = 0;
			
			@Override
			public void reset() {
				nextIdx = 0;
			}
			
			@Override
			public boolean hasNext() {
				return nextIdx < values.size();
			}

			@Override
			public Integer next() {
				if (nextIdx >= values.size()) {
					throw new NoSuchElementException();
				}
				
				// raise 2 to the 'n'th power where 'n' is the next value
				Integer val = (int) Math.round(Math.pow(2, values.get(nextIdx)));
				nextIdx++;
				
				return val;
			}
		};
		
		// create method timers
		Timer hasherCollisionTimer = new Timer(hasherCollisionTimeable, iter, iter_count);
		Timer hasherAdditionTimer = new Timer(hasherAdditionTimeable, iter, iter_count);
		
		// run calculations for method performance
		hasher = new BadHashFunctor();
		XYSeries badFuncCollSeries = hasherCollisionTimer.analyze("BadHashFunc Collisions");
		hasher = new MediocreHashFunctor();
		XYSeries medFuncCollSeries = hasherCollisionTimer.analyze("MediocreHashFunc Collisions");
		hasher = new GoodHashFunctor();
		XYSeries goodFuncCollSeries = hasherCollisionTimer.analyze("GoodHashFunc Collisions");
		hasher = new BadHashFunctor();
		XYSeries badFuncTimeSeries = hasherAdditionTimer.analyze("BadHashFunc Time");
		hasher = new MediocreHashFunctor();
		XYSeries medFuncTimeSeries = hasherAdditionTimer.analyze("MediocreHashFunc Time");
		hasher = new GoodHashFunctor();
		XYSeries goodFuncTimeSeries = hasherAdditionTimer.analyze("GoodHashFunc Time");
		
		// compile collections from data series
		XYSeriesCollection collisionComparisonSeriesColl = new XYSeriesCollection();
		collisionComparisonSeriesColl.addSeries(badFuncCollSeries);
		collisionComparisonSeriesColl.addSeries(medFuncCollSeries);
		collisionComparisonSeriesColl.addSeries(goodFuncCollSeries);
		XYSeriesCollection timeComparisonSeriesColl = new XYSeriesCollection();
		timeComparisonSeriesColl.addSeries(badFuncTimeSeries);
		timeComparisonSeriesColl.addSeries(medFuncTimeSeries);
		timeComparisonSeriesColl.addSeries(goodFuncTimeSeries);
		
		// chart and save results
		Visualizer.linechart(collisionComparisonSeriesColl, "Hash Functor Collision Count Comparison", "Map Size (elems)",
				"Collision Count", new File(collisionsChartFname));
		Visualizer.linechart(timeComparisonSeriesColl, "Hash Functor Addition Time Comparison", "Map Size (elems)",
				"Time to Add 1,000 Elems (milliseconds)", new File(functorChartFname));
	}
	
	private static String createRandString(int strSize) {
        String SALTCHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        StringBuilder salt = new StringBuilder();
        Random rnd = new Random();
        while (salt.length() < strSize) {
            int index = (int) (rnd.nextFloat() * SALTCHARS.length());
            salt.append(SALTCHARS.charAt(index));
        }
        String saltStr = salt.toString();
        return saltStr;
	}
}
