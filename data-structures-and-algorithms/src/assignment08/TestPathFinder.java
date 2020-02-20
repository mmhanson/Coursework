package assignment08;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.Scanner;
import org.junit.Test;
import static org.junit.Assert.*;

public class TestPathFinder {

	public static boolean verifyDots(File file1, File file2) throws FileNotFoundException {
		int numDots1 = 0;
		int numDots2 = 0;
		Scanner file1Scanner = new Scanner(file1);
		Scanner file2Scanner = new Scanner(file2);
		while (file1Scanner.hasNextLine()) {
			String line = file1Scanner.nextLine();
			for (int i = 0; i < line.length(); i++) {
				if (line.charAt(i) == '.') {
					numDots1++;
				}
			}
		}
		
		while (file2Scanner.hasNextLine()) {
			String line = file2Scanner.nextLine();
			for (int i = 0; i < line.length(); i++) {
				if (line.charAt(i) == '.') {
					numDots2++;
				}
			}
		}

		file1Scanner.close();
		file2Scanner.close();
		return numDots1 == numDots2;
	}
	
	@Test
	public void testFiles() {
		try {
		assertTrue(verifyDots(new File("bigMazeOutput.txt"), new File("bigMazeSol.txt")));
		assertTrue(verifyDots(new File("demoMazeOutput.txt"), new File("demoMazeSol.txt")));
		assertTrue(verifyDots(new File("mediumMazeOutput.txt"), new File("mediumMazeSol.txt")));
		assertTrue(verifyDots(new File("randomMazeOutput.txt"), new File("randomMazeSol.txt")));
		assertTrue(verifyDots(new File("tinyMazeOutput.txt"), new File("tinyMazeSol.txt")));
		PathFinder.solveMaze("classic.txt", "classicOutput.txt");
		assertTrue(verifyDots(new File("classicOutput.txt"), new File("classicSol.txt")));
		PathFinder.solveMaze("unsolvable.txt", "unsolvableO.txt");
		assertTrue(verifyDots(new File("unsolvableO.txt"), new File("unsolvable.txt")));
		PathFinder.solveMaze("straight.txt", "straightO.txt");
		assertTrue(verifyDots(new File("straightO.txt"), new File("straightSol.txt")));
		} catch (Exception e) {
			System.out.println("Exceptions thrown");
		}
	}
}
