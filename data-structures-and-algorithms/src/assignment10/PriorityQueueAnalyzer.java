package assignment10;

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
 * Analyze the asymptotic behavior of key methods in the PriorityQueue class.
 * 
 * @author Maxwell Hanson
 * @since Jul 18, 2018
 * @version 1.0.0
 */
public class PriorityQueueAnalyzer {
	public static void main(String[] args) {
		final int ITER_COUNT = 25;
		Random rand = new Random();
		
		// output chart filenames
		String findMinChartFname = "priorityQueue_findMin_analysis.png";
		String delMinChartFname = "priorityQueue_deleteMin_analysis.png";
		String addElemChartFname = "priorityQueue_add_analysis.png";
		
		// define timeable interfaces for pq methods
		Timeable findMinTimeable = new Timeable() {
			public int time(int n) {
				// setup a priority queue with 'n' ascending elements
				PriorityQueue<Integer> queue = new PriorityQueue<Integer>();
				for (int curs = 0; curs < n; curs++) {
					queue.add(curs);
				}
				
				// time how long it takes to find the min
				long startTime = System.nanoTime();
				queue.findMin();
				long stopTime = System.nanoTime();
				
				return (int) (stopTime - startTime);
			}
		};
		Timeable delMinTimeable = new Timeable() {
			public int time(int n) {
				// setup a priority queue with 'n' ascending elements
				PriorityQueue<Integer> queue = new PriorityQueue<Integer>();
				for (int curs = 0; curs < n; curs++) {
					queue.add(curs);
				}
				
				// time how long it takes to delete the min
				long startTime = System.nanoTime();
				queue.deleteMin();
				long stopTime = System.nanoTime();
				
				return (int) (stopTime - startTime);
			}
		};
		Timeable addElemTimeable = new Timeable() {
			public int time(int n) {
				// setup a priority queue with 'n' ascending elements
				PriorityQueue<Integer> queue = new PriorityQueue<Integer>();
				for (int curs = 0; curs < n; curs++) {
					queue.add(curs);
				}
				
				// time how long it takes to delete the min
				int toAdd = rand.nextInt();
				long startTime = System.nanoTime();
				queue.add(toAdd);
				long stopTime = System.nanoTime();
				
				return (int) (stopTime - startTime);
			}
		};
		
		nIterator iter = new nIterator() {
			List<Integer> values = Arrays.asList(10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
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
		Timer findMinTimer = new Timer(findMinTimeable, iter, ITER_COUNT);
		Timer delMinTimer = new Timer(delMinTimeable, iter, ITER_COUNT);
		Timer addElemTimer = new Timer(addElemTimeable, iter, ITER_COUNT);
		
		// run calculations for method performance
		XYSeries findMinSeries = findMinTimer.analyze("findMin");
		XYSeries delMinSeries = delMinTimer.analyze("deleteMin");
		XYSeries addElemSeries = addElemTimer.analyze("add");
		
		// create collections for parameterization
		XYSeriesCollection findMinSeriesColl = new XYSeriesCollection();
		findMinSeriesColl.addSeries(findMinSeries);
		XYSeriesCollection delMinSeriesColl = new XYSeriesCollection();
		delMinSeriesColl.addSeries(delMinSeries);
		XYSeriesCollection addElemSeriesColl = new XYSeriesCollection();
		addElemSeriesColl.addSeries(addElemSeries);
		
		// chart and save results
		Visualizer.linechart(findMinSeriesColl, "findMin Method Performance", "PriorityQueue Size (elems)",
				"Execution Time (milliseconds)", new File(findMinChartFname));
		Visualizer.linechart(delMinSeriesColl, "delMin Method Performance", "PriorityQueue Size (elems)",
				"Execution Time (milliseconds)", new File(delMinChartFname));
		Visualizer.linechart(addElemSeriesColl, "add Method Performance", "PriorityQueue Size (elems)",
				"Execution Time (milliseconds)", new File(addElemChartFname));
	}
}
