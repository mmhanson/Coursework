package streamio;

import java.io.ByteArrayInputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.io.PrintWriter;
import java.net.Socket;
import java.net.UnknownHostException;

import streamio.evillair.EvilLairServer;

public class StreamLab {
	private PasswordCracker passwordCracker;
	
	public static void main(String[] args) throws Exception {
		String secretPhrase = "u0985911";
		
		EvilLairServer.startEvilLairServer();
		GuyOnTheInside guyOnTheInside = GuyOnTheInside.makeContact(secretPhrase);
		
		StreamLab lab = new StreamLab();
		
		lab.getPasswordFrom(guyOnTheInside);
		
		lab.getPasswordFrom(new File("deaddrop.txt"));
		
		lab.getPasswordFromEvilLair();
		System.out.println("Your password is: " + lab.crackPassword());
	}
	
	
	public StreamLab() throws Exception {
		 
		 passwordCracker = new PasswordCracker();
	}
	
	private void getPasswordFrom(GuyOnTheInside guyOnTheInside) throws IOException {
		String passwordPart = guyOnTheInside.getPasswordPart();
		InputStream streamFromString = new ByteArrayInputStream(passwordPart.getBytes());
		passwordCracker.updatePassword(streamFromString);
	}

	private void getPasswordFrom(File file) throws FileNotFoundException, IOException {
		InputStream streamFromFile = new FileInputStream(file);
		passwordCracker.updatePassword(streamFromFile);
	}

	private void getPasswordFromEvilLair() throws UnknownHostException, IOException {
		try(Socket evilLairHack = new Socket("localhost", 8080);
				PrintWriter out = new PrintWriter(evilLairHack.getOutputStream(), true)) {
			//Send secret phrase to the Evil Lair Server.
			out.println("u0985911");
			
			// Receive response
			InputStream streamFromSocket = evilLairHack.getInputStream();
			passwordCracker.updatePassword(streamFromSocket);
			
			evilLairHack.close();
		}
	}
	
	public String crackPassword() {
		return passwordCracker.crackPassword();
	}
}
