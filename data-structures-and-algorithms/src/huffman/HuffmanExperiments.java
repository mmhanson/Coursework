package huffman;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.nio.file.attribute.BasicFileAttributes;
import java.util.Arrays;

public class HuffmanExperiments {

	private static final int PRINTABLE_RANGE = 95;
	HuffmanTree huffmanTree = new HuffmanTree();
	public HuffmanExperiments() {
		// TODO Auto-generated constructor stub
	}
	
	public void runEnterpriseExperiment() {
		String original = "enterprise";
		ByteArrayInputStream bais = new ByteArrayInputStream(original.getBytes());
		ByteArrayOutputStream baos = new ByteArrayOutputStream();
		huffmanTree.compressData(bais, baos);
		bais = copyFromOutputToInput(baos);
		baos = new ByteArrayOutputStream();
		huffmanTree.decompressData(bais, baos);
		
		String decompressed = new String(baos.toByteArray());
		System.out.println("Original: " + original + ", decompressed: " + decompressed);
	}
	
	public void runAliceExperiment() throws FileNotFoundException, IOException {
		long compressedSize= compressAlice();
		long decompressedSize = decompressAlice();
		
		System.out.println("For alice_in_worderland.txt");
		System.out.println(String.format("Compressed   Size: %8d",compressedSize));
		System.out.println(String.format("Decompressed Size: %8d", decompressedSize));
		System.out.println(compressionRatio(decompressedSize, compressedSize));
		linebreak();
	}

	private void linebreak() {
		System.out.println("-----------------------------");
	}
	
	public void runEvenDistribution() {
		byte[] data = fakeEvenDistribution(1_000_000);
		ByteArrayOutputStream output = new ByteArrayOutputStream();
		huffmanTree.compressData(new ByteArrayInputStream(data), output);
		byte[] compressedData = output.toByteArray();
		System.out.println("Even distribution of one million printable ASCII");
		System.out.println(String.format("Compressed Size: %8d", compressedData.length));
		System.out.println(compressionRatio(1_000_000, compressedData.length));
		linebreak();
	}
	
	public void runSingleCharacterExperiment() {
		byte[] data = new byte[1_000_000];
		Arrays.fill(data, (byte)'a');
		ByteArrayOutputStream output = new ByteArrayOutputStream();
		huffmanTree.compressData(new ByteArrayInputStream(data), output);
		byte[] compressedData = output.toByteArray();
		System.out.println("One Million Repeated characters");
		System.out.println(String.format("Compressed Size: %8d", compressedData.length));
		System.out.println(compressionRatio(1_000_000, compressedData.length));
		linebreak();
	}
	
	public void runDecompressEnterpriseAsFile() throws FileNotFoundException, IOException {
		try(FileOutputStream fos = new FileOutputStream(new File("enterprise.huf"))) {
			huffmanTree.compressData(new ByteArrayInputStream("enterprise".getBytes()),fos);
		}
		System.out.println("\"enterprise\" successfully decompressed");
	}

	private String compressionRatio(long original, long compressed) {
		return String.format("Compression ratio: %.3f", compressed / (double)original);
	}

	private byte[] fakeEvenDistribution(int size) {
		byte[] data = new byte[size];
		for(int idx = 0; idx < size; idx++) {
			data[idx] = (byte)(32 + (idx % PRINTABLE_RANGE));
		}
		return data;
	}

	private long decompressAlice() throws IOException, FileNotFoundException {
		try(FileInputStream fis = new FileInputStream(new File("alice.huf"));
				FileOutputStream fos = new FileOutputStream(new File("alice.out.txt"))) {
			huffmanTree.decompressData(fis, fos);
			return Files.readAttributes(Paths.get("alice.out.txt"), BasicFileAttributes.class).size();
		}
	}

	private long compressAlice() throws IOException, FileNotFoundException {
		try(FileInputStream fis = new FileInputStream(new File("alice_in_wonderland.txt"));
				FileOutputStream fos = new FileOutputStream(new File("alice.huf"))) {
			huffmanTree.compressData(fis, fos);
			return Files.readAttributes(Paths.get("alice.huf"), BasicFileAttributes.class).size();
		}
	}

	private ByteArrayInputStream copyFromOutputToInput(ByteArrayOutputStream baos) {
		return new ByteArrayInputStream(baos.toByteArray());
	}
	
	public static void main(String[] args) throws FileNotFoundException, IOException {
		HuffmanExperiments experiments = new HuffmanExperiments();
		experiments.runAliceExperiment();
		experiments.runEvenDistribution();
		experiments.runSingleCharacterExperiment();
		experiments.runDecompressEnterpriseAsFile();
	}
}
