package streamio.evillair;

import java.io.BufferedOutputStream;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.ServerSocket;
import java.net.Socket;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;

public class EvilLairServer extends Thread {

	public static void startEvilLairServer() throws IOException {
		new EvilLairServer().start();
	}

	private ServerSocket socket;
	
	public EvilLairServer() throws IOException {
		setDaemon(true);
		this.socket = new ServerSocket(8080);
	}
	
	@Override
	public void run() {
		safeRun();
		System.out.println("Evil Lair has closed!");
	}
	
	public void safeRun() throws RuntimeException{
		try(Socket studentSocket = this.socket.accept();
				BufferedReader reader = new BufferedReader(new InputStreamReader(studentSocket.getInputStream()));
				BufferedOutputStream out = new BufferedOutputStream(studentSocket.getOutputStream())) {
			String uid = readUidFrom(reader);
			System.out.println("Recieved hack passcode: " + uid);
			byte[] output = hashUid(uid);
			out.write(output);
			out.flush();
		} catch (Exception e) {
			throw new RuntimeException(e);
		}
	}
	
	private byte[] hashUid(String uid) throws NoSuchAlgorithmException {
		MessageDigest md = MessageDigest.getInstance("MD5");
		md.update((uid + "lair").getBytes());
		return md.digest();
	}

	private String readUidFrom(BufferedReader inputStream) throws IOException {
		String uid = inputStream.readLine();
		return uid;
	}
}
