package streamio;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Base64;

public class GuyOnTheInside {
	private String uid;

	public static GuyOnTheInside makeContact(String secretPhrase) {
		System.out.println("Made Contact with your guy on the inside!");
		return new GuyOnTheInside(secretPhrase);
	}
	
	private GuyOnTheInside(String secretPhrase) { 
		uid = secretPhrase;
	}
	
	public String getPasswordPart() {
		MessageDigest digest = getMD5Hasher();
		digest.update((uid + "inside").getBytes());
		String encodePasswordPart = Base64.getEncoder().encodeToString(digest.digest());
		System.out.println("Password from your inside guy: " + encodePasswordPart);
		return encodePasswordPart;
	}

	private MessageDigest getMD5Hasher() {
		MessageDigest digest;
		try {
			digest = MessageDigest.getInstance("MD5");
		} catch (NoSuchAlgorithmException e) {
			throw new RuntimeException(e);
		}
		return digest;
	}
}
