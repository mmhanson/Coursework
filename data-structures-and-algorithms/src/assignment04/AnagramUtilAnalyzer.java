package assignment04;

import timer.Visualizer;
import java.io.File;
import java.util.Arrays;
import java.util.Iterator;
import java.util.List;
import java.util.NoSuchElementException;
import java.util.Random;
import org.jfree.data.xy.XYSeries;
import org.jfree.data.xy.XYSeriesCollection;
import timer.Timeable;
import timer.Timer;

/**
 * A class to analyze the asymptotic performance of the AnagramUtil key methods.
 * 
 * @author Maxwell Hanson
 * @since Jul 12, 2018
 * @version 1.0.0
 */
public class AnagramUtilAnalyzer {
	/**
	 * Analyze the asymptotic performance of the AnagramUtil class' methods.
	 */
	public static void main(String[] args) {
		Random rand = new Random();
		
		// chart filenames
		String areAnagramsChartFname = "AnagramUtil.areAnagrams Asymptotic Analysis.png";
		String getLargestAnagramGroupChartFname = "AnagramUtil.getLargestAnagramGroup Asymptotic Analysis.png";
		
		// construct timeable interfaces for methods to analyze
		Timeable areAnagramsTimeable = new Timeable() {
			public int time(int n) {
				// setup input
				String str0 = createRandString(n);
				String str1 = createRandString(n);
				
				// time performance
				long startTime = System.nanoTime();
				AnagramUtil.areAnagrams(str0, str1);
				long endTime = System.nanoTime();
				
				return (int) (endTime - startTime);
			}
		};
		Timeable getLargestAnagramGroupTimeable = new Timeable() {
			public int time(int n) {
				// setup input
				String[] wordSet = new String[n];
				for (int i = 0; i < wordSet.length; i++) {
					wordSet[i] = createRandString(rand.nextInt(6) + 5);
				}
				
				// time performance
				long startTime = System.nanoTime();
				AnagramUtil.getLargestAnagramGroup(wordSet);
				long endTime = System.nanoTime();
				
				return (int) (endTime - startTime);
			}
		};
		
		// construct input iterators for the methods to analyze
		// these objects iterate over all values of 'n' that are to be analyzed for the respective method
		Iterator<Integer> areAnagramsIterator = new Iterator<Integer>() {
			List<Integer> values = Arrays.asList(10, 11, 12, 13, 14, 15);
			private int nextIdx = 0;
			
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
		Iterator<Integer> getLargestAnagramGroupIterator = new Iterator<Integer>() {
			List<Integer> values = Arrays.asList(10, 11, 12, 13, 14, 15);
			private int nextIdx = 0;
			
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
		Timer areAnagramsTimer = new Timer(areAnagramsTimeable, areAnagramsIterator);
		Timer getLargestAnagramGroupTimer = new Timer(getLargestAnagramGroupTimeable, getLargestAnagramGroupIterator);
		
		// run calculations for method performance
		XYSeries areAnagramsSeries = areAnagramsTimer.analyze("areAnagrams");
		XYSeries getLargestAnagramGroupSeries = getLargestAnagramGroupTimer.analyze("getLargestAnagram");
		
		// create collections for parameterization
		XYSeriesCollection areAnagramsData = new XYSeriesCollection();
		areAnagramsData.addSeries(areAnagramsSeries);
		XYSeriesCollection getLargestAnagramGroupData = new XYSeriesCollection();
		getLargestAnagramGroupData.addSeries(getLargestAnagramGroupSeries);
		
		// chart and save results
		Visualizer.linechart(areAnagramsData, "areAnagrams Asymptotic Behavior", "Input String Length (chars)",
				"Execution Time (milliseconds)", new File(areAnagramsChartFname));
		Visualizer.linechart(getLargestAnagramGroupData, "getLargestAnagramGroup Asymptotic Behavior",
				"Input String Array Length (count)", "Execution Time (milliseconds)", new File(getLargestAnagramGroupChartFname));
	}
	
	/**
	 * Create a random string.
	 * 
	 * Helper method for 'time' method to get strings to compare.
	 * 
	 * @param strSize -- size of string to generate
	 * @return -- random string of 'strSize' length
	 */
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