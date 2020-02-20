package assignment10;

import java.awt.Dimension;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.util.HashMap;
import java.util.List;
import javax.swing.JFrame;
import org.jfree.chart.ChartFactory;
import org.jfree.chart.ChartPanel;
import org.jfree.chart.ChartUtilities;
import org.jfree.chart.JFreeChart;
import org.jfree.chart.plot.PlotOrientation;
import org.jfree.data.category.DefaultCategoryDataset;
import org.jfree.data.statistics.HistogramDataset;
import org.jfree.data.statistics.HistogramType;
import org.jfree.data.xy.XYDataItem;
import org.jfree.data.xy.XYSeries;
import org.jfree.data.xy.XYSeriesCollection;

/**
 * Wrapper around JFreeChart's API. 
 *  
 * @author ryans
 */
public class Charter {
	/**
	 * Reads in a tsv file that holds X and Y values. 
	 * 
	 * @param dataFilePath - Path to the input data
	 * @param outputFile - File to be write the PNG to
	 */
	public void createChart(File dataFilePath, File outputFile, String lineName, String title, String XAxisLabel, String YAxisLabel) {
		// create data set
		XYSeriesCollection data = new XYSeriesCollection();
		data.addSeries(createDataSet(dataFilePath, lineName));
		
		JFreeChart chart = ChartFactory.createXYLineChart(title, XAxisLabel, YAxisLabel, data);
		try {
			ChartUtilities.saveChartAsPNG(outputFile, chart, 500, 500);
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	
	/**
	 * Reads in a tsv file that holds X and Y values. 
	 * 
	 * @param datasets -- hashset of Files that contain graphing data that maps to the name of the line
	 * @param outputFile - File to be write the PNG to
	 */
	public void createChart(HashMap<File, String> datasets, File outputFile, String title, String XAxisLabel, String YAxisLabel) {
		// craete data sets
		XYSeriesCollection data = new XYSeriesCollection();
		
		for (File file: datasets.keySet()) {
			data.addSeries(createDataSet(file, datasets.get(file)));
		}
		
		JFreeChart chart = ChartFactory.createXYLineChart(title, XAxisLabel, YAxisLabel, data);
		try {
			ChartUtilities.saveChartAsPNG(outputFile, chart, 500, 500);
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	
	/**
	 * Constructs a histogram from data
	 * 
	 * @param title - title of the chart
	 * @param data - data for the chart
	 * @param bins - how many bins to put the data into.
	 * @return - JFreeChart instance
	 */
	public JFreeChart buildHistogram(String title, List<Number> data, int bins) {
		HistogramDataset dataset = new HistogramDataset();
		dataset.addSeries("Histogram", convertData(data), bins);
		
		dataset.setType(HistogramType.FREQUENCY);
		
		JFreeChart chart = ChartFactory.createHistogram(title, "Bins", "Count", dataset, PlotOrientation.VERTICAL, false, false, false);
		return chart;
	}
	
	/**
	 * Create a bar chart
	 * @param title - Title of chart
	 * @param data - DefualtCategoryDataset 
	 * @return
	 */
	public JFreeChart buildBarChart(String title, DefaultCategoryDataset data) {
		return ChartFactory.createBarChart(title, "Bins", "Count", data);
	}

	private double[] convertData(List<Number> data) {
		double[] returnValues = new double[data.size()];
		for(int dataIdx = 0; dataIdx < data.size(); dataIdx++) {
			returnValues[dataIdx] = data.get(dataIdx).doubleValue();
		}
		return returnValues;
	}

	/**
	 * Display the chart to the screen
	 * @param chart
	 */
	public void showChart(JFreeChart chart) {
		JFrame frame = new JFrame();
	    frame.setTitle(chart.getTitle().toString());
	    ChartPanel chartPanel = new ChartPanel(chart);
	    frame.setPreferredSize(new Dimension(800, 400));
	    frame.add(chartPanel);
	    frame.pack();
	    frame.setVisible(true);
	}
	
	/**
	 * Turns a .tsv file into an XYSeries object to put into an XYSeriesCollection dataset.
	 * 
	 * @param dataFile - .tsv file. Can only have two columns.
	 * @return
	 */
	private XYSeries createDataSet(File dataFile, String lineName) {
		XYSeries series = new XYSeries(lineName);
		
		try(FileReader reader = new FileReader(dataFile); BufferedReader br = new BufferedReader(reader)) {
			String line;
			while((line = br.readLine()) != null) {
				String[] split = line.split("\t");
				series.add(new XYDataItem(Double.parseDouble(split[0]), Double.parseDouble(split[1])));
			}
		} catch (IOException e) {
			e.printStackTrace();
		}
		
		return series;
	}
}
