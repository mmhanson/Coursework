package timer;

import java.awt.Dimension;
import java.io.File;
import java.io.IOException;

import javax.swing.JFrame;

import org.jfree.chart.ChartFactory;
import org.jfree.chart.ChartPanel;
import org.jfree.chart.ChartUtilities;
import org.jfree.chart.JFreeChart;
import org.jfree.data.xy.XYSeriesCollection;

/**
 * An abstract toolkit for visualizing asymptotic analyses of java methods.
 * 
 * @author Maxwell Hanson
 * @since July 11, 2018
 * @version 1.0.0
 */
public class Visualizer {
	/**
	 * Create a line chart out of a set of x-y series and write image to a file.
	 * 
	 * @param data -- data to create line(s) from
	 * @param title -- title of the chart
	 * @param XAxisLabel -- x axis label for the chart
	 * @param YAxisLabel -- y axis label for the chart
	 * @param chartFile -- output file to write chcart to
	 */
	public static void linechart(XYSeriesCollection data, String title, String XAxisLabel, String YAxisLabel, File chartFile) {
		JFreeChart chart = ChartFactory.createXYLineChart(title, XAxisLabel, YAxisLabel, data);
		try {
			ChartUtilities.saveChartAsPNG(chartFile, chart, 500, 500);
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	
	/**
	 * Create a line chart out of a set of x-y series and show the image in a field.
	 * 
	 * @param data -- data to create line(s) from
	 * @param title -- title of the chart
	 * @param XAxisLabel -- x axis label for the chart
	 * @param YAxisLabel -- y axis label for the chart
	 */
	public static void linechart(XYSeriesCollection data, String title, String XAxisLabel, String YAxisLabel) {
		JFreeChart chart = ChartFactory.createXYLineChart(title, XAxisLabel, YAxisLabel, data);
		showChart(chart);
	}
	
	/**
	 * Display a chart in a window.
	 * 
	 * @param chart -- chart to display
	 */
	public static void showChart(JFreeChart chart) {
		JFrame frame = new JFrame();
	    frame.setTitle(chart.getTitle().toString());
	    ChartPanel chartPanel = new ChartPanel(chart);
	    frame.setPreferredSize(new Dimension(800, 400));
	    frame.add(chartPanel);
	    frame.pack();
	    frame.setVisible(true);
	}
}
