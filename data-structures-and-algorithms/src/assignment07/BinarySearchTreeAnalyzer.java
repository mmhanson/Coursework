package assignment07;

import java.io.File;
import java.util.Arrays;
import java.util.List;
import java.util.NoSuchElementException;
import java.util.Random;
import java.util.TreeSet;

import org.jfree.data.xy.XYSeries;
import org.jfree.data.xy.XYSeriesCollection;
import timer.Timeable;
import timer.Timer;
import timer.Visualizer;
import timer.nIterator;

/**
 * Analyze the asymptotic behavior of key methods in the BinarySearchTree class.
 * 
 * @author Maxwell Hanson
 * @since Jul 17, 2018
 * @version 1.0.0
 */
public class BinarySearchTreeAnalyzer {
	public static void main(String[] args) {
		final int ITER_COUNT = 100;
		Random rand = new Random();
		
		// output chart filenames
		String sortedVsRandomChartFname = "sorted_vs_random.png";
		String sortedVsTreesetChartFname = "sorted_vs_treeset.png";
		
		// define timeable interfaces for quicksort and mergesort methods
		Timeable sortedBstTimeable = new Timeable() {
			public int time(int n) {
				// setup input
				BinarySearchTree<Integer> bst = new BinarySearchTree<Integer>();
				for (int count = 0; count < n; count++) {
					bst.add(count);
				}
				
				// time the time it takes to find each element and average out
				long startTime, stopTime, totalTime = 0;
				for (int cnt = 0; cnt < bst.size(); cnt++) {
					startTime = System.nanoTime();
					bst.contains(cnt);
					stopTime = System.nanoTime();
					totalTime += stopTime - startTime;
				}
				
				return (int) (totalTime / bst.size());
			}
		};
		Timeable randoBstTimeable = new Timeable() {
			public int time(int n) {
				// setup input
				BinarySearchTree<Integer> bst = new BinarySearchTree<Integer>();
				for (int count = 0; count < n; count++) {
					bst.add(rand.nextInt());
				}
				
				// time the time it takes to find each element and average out
				long startTime, stopTime, totalTime = 0;
				for (int cnt = 0; cnt < bst.size(); cnt++) {
					startTime = System.nanoTime();
					bst.contains(cnt);
					stopTime = System.nanoTime();
					totalTime += stopTime - startTime;
				}
				
				return (int) (totalTime / bst.size());
			}
		};
		Timeable treeSetTimeable = new Timeable() {
			public int time(int n) {
				// setup input
				TreeSet<Integer> tree = new TreeSet<Integer>();
				for (int count = 0; count < n; count++) {
					tree.add(count);
				}
				
				// time the time it takes to find each element and average out
				long startTime, stopTime, totalTime = 0;
				for (int cursor = 0; cursor < tree.size(); cursor++) {
					startTime = System.nanoTime();
					tree.contains(cursor);
					stopTime = System.nanoTime();
					totalTime += stopTime - startTime;
				}
				
				return (int) (totalTime / tree.size());
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
		Timer sortedBstTimer = new Timer(sortedBstTimeable, iter, ITER_COUNT);
		Timer randoBstTimer = new Timer(randoBstTimeable, iter, ITER_COUNT);
		Timer treeSetTimer = new Timer(treeSetTimeable, iter, ITER_COUNT);
		
		// run calculations for method performance
		XYSeries sortedBstSeries = sortedBstTimer.analyze("Sorted BST");
		XYSeries randoBstSeries = randoBstTimer.analyze("Random BST");
		XYSeries treeSetSeries = treeSetTimer.analyze("TreeSet");
		
		// create collections for parameterization
		XYSeriesCollection sortedVsRandomSeriesColl = new XYSeriesCollection();
		sortedVsRandomSeriesColl.addSeries(sortedBstSeries);
		sortedVsRandomSeriesColl.addSeries(randoBstSeries);
		XYSeriesCollection sortedVsTreesetSeriesColl = new XYSeriesCollection();
		sortedVsTreesetSeriesColl.addSeries(sortedBstSeries);
		sortedVsTreesetSeriesColl.addSeries(treeSetSeries);
		
		// chart and save results
		Visualizer.linechart(sortedVsRandomSeriesColl, "Sorted vs Random BST", "Input Tree Size (elems)",
				"Execution Time (milliseconds)", new File(sortedVsRandomChartFname));
		Visualizer.linechart(sortedVsTreesetSeriesColl, "Sorted BST vs TreeSet", "Input Tree Size (elems)",
				"Execution Time (milliseconds)", new File(sortedVsTreesetChartFname));
	}
}
