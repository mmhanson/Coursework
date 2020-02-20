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
 * Analyze the performance of HashTable classes when different
 * collision handling strategies are used.
 * 
 * @author Maxwell Hanson
 * @since Jul 17, 2018
 * @version 1.0.0
 */
public class HashTableAnalyzer {
	public static void main(String[] args) {
		// iterations per measurement for all timers
		int iter_count = 25;
		
		// output chart filenames
		String collisionsChartFname = "hash table collisions.png";
		String timesChartFname = "hash table times.png";
		
		// define timeable interfaces for hash functors
		Timeable quadCollisionTimeable = new Timeable() {
			public int time(int n) {
				// setup hash table
				QuadProbeHashTable hashTable = new QuadProbeHashTable(100, new GoodHashFunctor());
				for (int count = 0; count <= n; count++) {
					hashTable.add(createRandString(10));
				}
				
				// count collisions (time not used)
				return hashTable.collCount;
			}
		};
		Timeable linearCollisionTimeable = new Timeable() {
			public int time(int n) {
				// setup hash table
				ChainingHashTable hashTable = new ChainingHashTable(100, new GoodHashFunctor());
				for (int count = 0; count <= n; count++) {
					hashTable.add(createRandString(10));
				}
				
				// count collisions (time not used)
				return hashTable.collCount;
			}
		};
		Timeable quadAdditionTimeTimeable = new Timeable() {
			public int time(int n) {
				// setup hash table
				QuadProbeHashTable hashTable = new QuadProbeHashTable(100, new GoodHashFunctor());
				for (int i = 0; i < n; i++) {
					hashTable.add(createRandString(10));
				}
				
				// time how long it takes to add 1000 elems
				
				long startTime = System.nanoTime();
				for (int count = 0; count < 1000; count++) {
					hashTable.add(createRandString(10));
				}
				long stopTime = System.nanoTime();
				
				return (int) (stopTime - startTime);
			}
		};
		Timeable linearAdditionTimeTimeable = new Timeable() {
			public int time(int n) {
				// setup hash table
				ChainingHashTable hashTable = new ChainingHashTable(100, new GoodHashFunctor());
				for (int i = 0; i < n; i++) {
					hashTable.add(createRandString(10));
				}
				
				// time how long it takes to add 1000 elems
				
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
		Timer quadCollisionTimer = new Timer(quadCollisionTimeable, iter, iter_count);
		Timer linearCollisionTimer = new Timer(linearCollisionTimeable, iter, iter_count);
		Timer quadAdditionTimeTimer = new Timer(quadAdditionTimeTimeable, iter, iter_count);
		Timer linearAdditionTimeTimer = new Timer(linearAdditionTimeTimeable, iter, iter_count);
		
		// run calculations for method performance
		XYSeries quadCollSeries = quadCollisionTimer.analyze("Quadratic Probing Collisions");
		XYSeries linearCollSeries = linearCollisionTimer.analyze("Linear Chaining Collisions");
		XYSeries quadAddTimeSeries = quadAdditionTimeTimer.analyze("Quadratic Probing Addition Time");
		XYSeries linearAddTimeSeries = linearAdditionTimeTimer.analyze("Linear Chaining Addition Time");
		
		// compile collections from data series
		XYSeriesCollection collisionSeriesColl = new XYSeriesCollection();
		collisionSeriesColl.addSeries(quadCollSeries);
		collisionSeriesColl.addSeries(linearCollSeries);
		XYSeriesCollection additionSeriesColl = new XYSeriesCollection();
		collisionSeriesColl.addSeries(quadAddTimeSeries);
		collisionSeriesColl.addSeries(linearAddTimeSeries);
		
		// chart and save results
		Visualizer.linechart(collisionSeriesColl, "Quadratic Probing vs Linear Chaining Collision Count", "Map Size (elems)",
				"Collision Count", new File(collisionsChartFname));
		Visualizer.linechart(additionSeriesColl, "Quadratic Probing vs Linear Chaining Addition Time", "Map Size (elems)",
				"Time to Add 1,000 Elems (milliseconds)", new File(timesChartFname));
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
