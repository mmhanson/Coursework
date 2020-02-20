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
 * Analyze the behavior of HashTable classes with change in lambda.
 * 
 * @author Maxwell Hanson
 * @since Jul 17, 2018
 * @version 1.0.0
 */
public class HashTableLambdaAnalyzer {
	public static void main(String[] args) {
		// iterations per measurement for all timers
		int iter_count = 25;
		
		// output chart filenames
		String chartFname = "hashtable_performance.png";
		
		// define timeable interfaces for hash functors
		Timeable quadHashTableTimeable = new Timeable() {
			public int time(int n) {
				// setup hash table
				double lambda = n * 0.1;
				QuadProbeHashTable hashTable = new QuadProbeHashTable(100, new GoodHashFunctor());
				hashTable.setLambda(lambda);
				
				// time performance to add 1000 elems
				String toAdd = createRandString(10);
				long startTime = System.nanoTime();
				for (int count = 0; count < 1000; count++) {
					hashTable.add(toAdd);
				}
				long stopTime = System.nanoTime();
				
				return (int) (stopTime - startTime);
			}
		};
		Timeable linearHashTableTimeable = new Timeable() {
			public int time(int n) {
				// setup hash table
				double lambda = n * 0.1;
				ChainingHashTable hashTable = new ChainingHashTable(100, new GoodHashFunctor());
				hashTable.setLambda(lambda);
				
				// time performance to add 1000 elems
				String toAdd = createRandString(10);
				long startTime = System.nanoTime();
				for (int count = 0; count < 1000; count++) {
					hashTable.add(toAdd);
				}
				long stopTime = System.nanoTime();
				
				return (int) (stopTime - startTime);
			}
		};

		nIterator iter = new nIterator() {
			List<Integer> values = Arrays.asList(1, 2, 3, 4, 5, 6, 7, 8, 9);
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
		Timer quadHashTableTimer = new Timer(quadHashTableTimeable, iter, iter_count);
		Timer linearHashTableTimer = new Timer(linearHashTableTimeable, iter, iter_count);
		
		// run calculations for method performance
		XYSeries quadHashTableLambdaSeries = quadHashTableTimer.analyze("Quadratic Probing Hash Table Lambda Performance");
		XYSeries linearHashTableLambdaSeries = linearHashTableTimer.analyze("Linear Chaining Hash Table Lambda Performance");
		
		// compile collections from data series
		XYSeriesCollection hashTableComparisonSeriesColl = new XYSeriesCollection();
		hashTableComparisonSeriesColl.addSeries(quadHashTableLambdaSeries);
		hashTableComparisonSeriesColl.addSeries(linearHashTableLambdaSeries);
		
		// chart and save results
		Visualizer.linechart(hashTableComparisonSeriesColl, "Hash Table Collision Handling Comparisons", "Lambda Value",
				"Time to Add 1,000 elems (milliseconds)", new File(chartFname));
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
