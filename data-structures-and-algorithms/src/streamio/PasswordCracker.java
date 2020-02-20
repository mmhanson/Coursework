package streamio;

import java.io.IOException;
import java.io.InputStream;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Base64;

import org.apache.commons.io.IOUtils;

public class PasswordCracker {

	private MessageDigest hasher;

	public PasswordCracker() throws NoSuchAlgorithmException {
		hasher = MessageDigest.getInstance("SHA-256");
	}
	
	public void updatePassword(InputStream inputStream) throws IOException {
		// inputStream is extracted to bytes and fed to hasher
		byte[] bytes = IOUtils.toByteArray(inputStream);
		hasher.update(bytes);
	}
	
	public String crackPassword() {
		return new String(Base64.getEncoder().encode(hasher.digest()));
	}
}
