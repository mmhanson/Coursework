package assignment06;

import java.io.File;
import java.util.Arrays;
import java.util.List;
import java.util.NoSuchElementException;
import java.util.Random;
import org.jfree.data.xy.XYSeries;
import org.jfree.data.xy.XYSeriesCollection;
import lec12.StackArray;
import timer.Timeable;
import timer.Timer;
import timer.Visualizer;
import timer.nIterator;

/**
 * Analyze the asymptotic behavior of key methods in the SortUtil class.
 * 
 * @author Maxwell Hanson
 * @since Jul 17, 2018
 * @version 1.0.0
 */
public class StackAnalyzer {
	public static void main(String[] args) {
		// to populate stacks in timeable interfaces
		Random rand = new Random();
		// iterations per measurement for all timers
		int iter_count = 10_000;
		
		// output chart filenames
		String SLLvSAChartFname = "SLL vs SA.png";
		String pushChartFname = "SLL vs SA push.png";
		String popChartFname = "SLL vs SA pop.png";
		String peekChartFname = "SLL vs SA peek.png";
		
		// define timeable interfaces for array-based and linked-list-based
		//   stacks for push, pop, and peek operations
		Timeable ListPushTimeable = new Timeable() {
			public int time(int n) {
				// setup stack
				StackLinkedList<Integer> stack = new StackLinkedList<Integer>();
				for (int count = 0; count < n; count++) {
					stack.push(rand.nextInt());
				}
				
				// time performance
				long startTime = System.nanoTime();
				stack.push(0);
				long endTime = System.nanoTime();
				
				return (int) (endTime - startTime);
			}
		};
		Timeable ListPopTimeable = new Timeable() {
			public int time(int n) {
				// setup stack
				StackLinkedList<Integer> stack = new StackLinkedList<Integer>();
				for (int count = 0; count < n; count++) {
					stack.push(rand.nextInt());
				}
				
				// time performance
				long startTime = System.nanoTime();
				stack.pop();
				long endTime = System.nanoTime();
				
				return (int) (endTime - startTime);
			}
		};
		Timeable ListPeekTimeable = new Timeable() {
			public int time(int n) {
				// setup stack
				StackLinkedList<Integer> stack = new StackLinkedList<Integer>();
				for (int count = 0; count < n; count++) {
					stack.push(rand.nextInt());
				}
				
				// time performance
				long startTime = System.nanoTime();
				stack.peek();
				long endTime = System.nanoTime();
				
				return (int) (endTime - startTime);
			}
		};
		Timeable ArrPushTimeable = new Timeable() {
			public int time(int n) {
				// setup stack
				StackArray<Integer> stack = new StackArray<Integer>();
				for (int count = 0; count < n; count++) {
					stack.push(rand.nextInt());
				}
				
				// time performance
				long startTime = System.nanoTime();
				stack.pop();
				long endTime = System.nanoTime();
				
				return (int) (endTime - startTime);
			}
		};
		Timeable ArrPopTimeable = new Timeable() {
			public int time(int n) {
				// setup stack
				StackArray<Integer> stack = new StackArray<Integer>();
				for (int count = 0; count < n; count++) {
					stack.push(rand.nextInt());
				}
				
				// time performance
				long startTime = System.nanoTime();
				stack.push(0);
				long endTime = System.nanoTime();
				
				return (int) (endTime - startTime);
			}
		};
		Timeable ArrPeekTimeable = new Timeable() {
			public int time(int n) {
				// setup stack
				StackArray<Integer> stack = new StackArray<Integer>();
				for (int count = 0; count < n; count++) {
					stack.push(rand.nextInt());
				}
				
				// time performance
				long startTime = System.nanoTime();
				stack.peek();
				long endTime = System.nanoTime();
				
				return (int) (endTime - startTime);
			}
		};
		
		nIterator iter = new nIterator() {
			List<Integer> values = Arrays.asList(10, 11, 12, 13, 14, 15, 16, 17,
					18, 19, 20, 21, 22, 23, 24, 25);
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
		Timer ListPushTimer = new Timer(ListPushTimeable, iter, iter_count);
		Timer ListPopTimer = new Timer(ListPopTimeable, iter, iter_count);
		Timer ListPeekTimer = new Timer(ListPeekTimeable, iter, iter_count);
		Timer ArrPushTimer = new Timer(ArrPushTimeable, iter, iter_count);
		Timer ArrPopTimer = new Timer(ArrPopTimeable, iter, iter_count);
		Timer ArrPeekTimer = new Timer(ArrPeekTimeable, iter, iter_count);
		
		// run calculations for method performance
		XYSeries ListPushDataSeries = ListPushTimer.analyze("SortedLinkedList (SLL) Push");
		XYSeries ListPopDataSeries = ListPopTimer.analyze("SLL Pop");
		XYSeries ListPeekDataSeries = ListPeekTimer.analyze("SLL Peek");
		XYSeries ArrPushDataSeries = ArrPushTimer.analyze("SortedArray (SA) Push");
		XYSeries ArrPopDataSeries = ArrPopTimer.analyze("SA Pop");
		XYSeries ArrPeekDataSeries = ArrPeekTimer.analyze("SA Peek");
		
		// compile collections from data series
		XYSeriesCollection SLLvsSACompAllSeriesColl = new XYSeriesCollection();
		SLLvsSACompAllSeriesColl.addSeries(ListPushDataSeries);
		SLLvsSACompAllSeriesColl.addSeries(ListPopDataSeries);
		SLLvsSACompAllSeriesColl.addSeries(ListPeekDataSeries);
		SLLvsSACompAllSeriesColl.addSeries(ArrPushDataSeries);
		SLLvsSACompAllSeriesColl.addSeries(ArrPopDataSeries);
		SLLvsSACompAllSeriesColl.addSeries(ArrPeekDataSeries);
		XYSeriesCollection SLLvsSACompPushSeriesColl = new XYSeriesCollection();
		SLLvsSACompPushSeriesColl.addSeries(ArrPushDataSeries);
		SLLvsSACompPushSeriesColl.addSeries(ListPushDataSeries);
		XYSeriesCollection SLLvsSACompPopSeriesColl = new XYSeriesCollection();
		SLLvsSACompPopSeriesColl.addSeries(ArrPopDataSeries);
		SLLvsSACompPopSeriesColl.addSeries(ListPopDataSeries);
		XYSeriesCollection SLLvsSACompPeekSeriesColl = new XYSeriesCollection();
		SLLvsSACompPeekSeriesColl.addSeries(ArrPeekDataSeries);
		SLLvsSACompPeekSeriesColl.addSeries(ListPeekDataSeries);
		
		// chart and save results
		Visualizer.linechart(SLLvsSACompAllSeriesColl, "SortedLinkedList vs SortedArray Stacks", "Stack Size (elems)",
				"Execution Time (milliseconds)", new File(SLLvSAChartFname));
		Visualizer.linechart(SLLvsSACompPushSeriesColl, "SortedLinkedList vs SortedArray Stacks (push only)", "Stack Size (elems)",
				"Execution Time (milliseconds)", new File(pushChartFname));
		Visualizer.linechart(SLLvsSACompPopSeriesColl, "SortedLinkedList vs SortedArray Stacks (pop only)", "Stack Size (elems)",
				"Execution Time (milliseconds)", new File(popChartFname));
		Visualizer.linechart(SLLvsSACompPushSeriesColl, "SortedLinkedList vs SortedArray Stacks (peek only)", "Stack Size (elems)",
				"Execution Time (milliseconds)", new File(peekChartFname));
	}
}
