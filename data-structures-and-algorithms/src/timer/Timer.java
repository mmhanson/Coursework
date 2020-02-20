package timer;

import org.jfree.data.xy.XYDataItem;
import org.jfree.data.xy.XYSeries;

/**
 * Measure the asymptotic behavior of a method
 * 
 * @author Maxwell Hanson
 * @since July 11, 2018
 * @version 1.1.0
 */
public class Timer {
	/**
	 * Method to measure behavior of.
	 */
	Timeable subject;
	/**
	 * An iterator that iterates over values of n for the inputs to the timable interface.
	 */
	nIterator nIterator;
	
	/**
	 * Number of times to calculate each measure before averaging
	 */
	int iterations;
	
	/**
	 * Create a new timer.
	 * 
	 * 'n' bounds are used to measure inputs for the method. Inputs are calculated
	 * by raising 2 to the power of n, not by inputting in to the function. Every
	 * n is iterated over and 2 is raised to that power and inputted into the subject.
	 * 
	 * Iterations is meant to average calculations to eliminate anomalies. Supplied
	 * value is the amount of times each input is averaged.
	 * 
	 * @param subject -- method to measure
	 * @param nLowerBound -- lower bound of 'n' (inclusive), see above
	 * @param nUpperBound -- upper bound of 'n' (inclusive), see above
	 * @param iterations -- iterations to average, see above
	 */
	public Timer (Timeable subject, nIterator nIterator, int iterations) {
		this.subject = subject;
		this.nIterator = nIterator;
		this.iterations = iterations;
	}
	
	/**
	 * Create a timer.
	 * 
	 * Overload for default iterations count.
	 */
	public Timer (Timeable subject, nIterator nIterator) {
		this(subject, nIterator, 100);
	}
	
	/**
	 * Calculate the performance of the method given the inputs.
	 * 
	 * @param lineName -- the name of the line for the data series. This is shown
	 *     when the graph is generated as the name of the line of the series.
	 * @return -- series of x-y items corresponding to a line showing the behavior
	 *     of the method.
	 */
	public XYSeries analyze(String lineName) {
		System.out.println("\nstarting asymptotic analysis...");
		
		// warm up JVM for timing accuracy
		long startTime = System.nanoTime();
		while (System.nanoTime() - startTime < 1_000_000_000);
		
		XYSeries dataSeries = new XYSeries(lineName);
		
		nIterator.reset();
		while (nIterator.hasNext()) {
			// get parameter and time method
			int param = nIterator.next();
			XYDataItem results = time(param);
			
			// output results to console for monitoring execution
			System.out.println("(" + results.getXValue() + ", " + results.getYValue() + ")");
			
			dataSeries.add(results);
		}
		
		return dataSeries;
	}
	
	/**
	 * Calculate the average performance of the method given an input.
	 * 
	 * Performance is measured 'iterations' times and averaged.
	 * 
	 * @param n -- the abstracted 'input' for the wrapped method
	 * @return -- data pair indicating the input and corresponding performance
	 */
	private XYDataItem time(int n) {
		// calculate average time
		long totalTime = 0;
		for (int iter = 0; iter < iterations; iter++) {
			totalTime += subject.time(n);
		}
		
		Number x = Integer.valueOf(n);
		Number y = Long.valueOf(totalTime / iterations);
		
		return new XYDataItem(x, y);
	}
}
