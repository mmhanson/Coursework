package assignment05;

import java.io.File;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.NoSuchElementException;
import org.jfree.data.xy.XYSeries;
import org.jfree.data.xy.XYSeriesCollection;
import timer.Timeable;
import timer.Timer;
import timer.Visualizer;
import timer.nIterator;

/**
 * Analyze the asymptotic behavior of key methods in the SortUtil class.
 * 
 * @author Maxwell Hanson
 * @since Jul 14, 2018
 * @version 1.0.0
 */
public class SortUtilAnalyzer {
	/**
	 * Switch for what type of array to generate for sorting.
	 * 
	 * This switch is used to generate ascending, descending, or permuted arrays
	 * and feed them into the algorithms to be sorted.
	 * 
	 * Possible types are as follows:
	 *  0: ascending
	 *  1: descending
	 *  2: permuted
	 */
	private static int arrType;
	
	public static void main(String[] args) {
		// output chart filenames
		String chartFname = "Quicksort vs Mergesort";
		
		// define timeable interfaces for quicksort and mergesort methods
		Timeable quicksortTimeable = new Timeable() {
			public int time(int n) {
				// setup input
				ArrayList<Integer> arr = getArr(n);
				
				// time performance
				long startTime = System.nanoTime();
				SortUtil.quicksort(arr, (lhs, rhs) -> lhs.compareTo(rhs));
				long endTime = System.nanoTime();
				
				return (int) (endTime - startTime);
			}
		};
		Timeable mergesortTimeable = new Timeable() {
			public int time(int n) {
				// setup input
				ArrayList<Integer> arr = getArr(n);
				
				// time performance
				long startTime = System.nanoTime();
				SortUtil.mergesort(arr, (lhs, rhs) -> lhs.compareTo(rhs));
				long endTime = System.nanoTime();
				
				return (int) (endTime - startTime);
			}
		};
		
		nIterator iter = new nIterator() {
			List<Integer> values = Arrays.asList(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
					11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
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
		Timer mergesortTimer = new Timer(mergesortTimeable, iter, 10);
		Timer quicksortTimer = new Timer(quicksortTimeable, iter, 10);
		
		// run calculations for method performance
		arrType = 0;
		XYSeries mergesortBestCaseSeries = mergesortTimer.analyze("Mergesort Best Case");
		XYSeries quicksortBestCaseSeries = quicksortTimer.analyze("Quicksort Best Case");
		arrType = 1;
		XYSeries mergesortWorstCaseSeries = mergesortTimer.analyze("Mergesort Worst Case");
		XYSeries quicksortWorstCaseSeries = quicksortTimer.analyze("Quicksort Worst Case");
		arrType = 2;
		XYSeries mergesortAvgCaseSeries = mergesortTimer.analyze("Mergesort Avg Case");
		XYSeries quicksortAvgCaseSeries = quicksortTimer.analyze("Quicksort Avg Case");
		
		// create collections for parameterization
		XYSeriesCollection sortingAlgoData = new XYSeriesCollection();
		sortingAlgoData.addSeries(mergesortBestCaseSeries);
		sortingAlgoData.addSeries(quicksortBestCaseSeries);
		sortingAlgoData.addSeries(mergesortWorstCaseSeries);
		sortingAlgoData.addSeries(quicksortWorstCaseSeries);
		sortingAlgoData.addSeries(mergesortAvgCaseSeries);
		sortingAlgoData.addSeries(quicksortAvgCaseSeries);
		
		// chart and save results
		Visualizer.linechart(sortingAlgoData, "Quicksort vs Mergesort", "Input Array Length (elems)",
				"Execution Time (milliseconds)", new File(chartFname));
	}

	/**
	 * Generate an arraylist based on the arrType variable.
	 * 
	 * @see arrType
	 * @param arrSize -- the size of the array to generate
	 * @return -- arraylist based on 'arrType's value
	 */
	private static ArrayList<Integer> getArr(int arrSize) {
		switch (arrType) {
		case 0:
			return SortUtil.generateAscending(arrSize);
		case 1:
			return SortUtil.generateDescending(arrSize);
		case 2:
			return SortUtil.generateShuffled(arrSize);
		}
		
		return null; // should never be reached
	}
}
