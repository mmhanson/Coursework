package assignment02;

/**
 * Representation of a phone number
 * 
 * @author Maxwell Hanson
 * @version 1.1.0
 */
public class PhoneNumber {
	/**
	 * 3-digit area code of the phone number
	 */
	private String areaCode;

	/**
	 * First three digits of the phone number
	 */
	private String trunk;

	/**
	 * Last four digits of the phone number
	 */
	private String rest;

	/**
	 * Construct a phone number from a string.
	 * 
	 * Strings are to be formatted as such:
	 * "***-***-****"
	 * 
	 * @param phoneNum -- string representation of the phone number
	 */
	public PhoneNumber(String phoneNum) {
		// strip separators from the number
		phoneNum = phoneNum.replaceAll("-|\\s|\\.|\\(|\\)", "");

		// determine the validity of the string
		boolean isValid = true;
		if (phoneNum.length() != 10)
			isValid = false;
		for (int index = 0; isValid && index < 10; index++) {
			if (!Character.isDigit(phoneNum.charAt(index))) {
				isValid = false;
			}
		}

		if (isValid) {
			areaCode = phoneNum.substring(0, 3);
			trunk = phoneNum.substring(3, 6);
			rest = phoneNum.substring(6, 10);
		} else {
			areaCode = "000";
			trunk = "000";
			rest = "000";
			System.err.println("Phone number \"" + phoneNum + "\" is not formatted correctly, initializing as "
					+ toString() + ".");
		}
	}

	/**
	 * Represent this phone number as a string.
	 * 
	 * Strings are formatted as such:
	 * "(<areacode>)<trunk>-<rest>"
	 * 
	 * @return -- string of this
	 */
	public String toString() {
		return "(" + areaCode + ") " + trunk + "-" + rest;
	}

	/**
	 * Determine if two phone numbers are equal.
	 * 
	 * Two phone numbers are equal iff all their digits are equal.
	 * 
	 * @param other -- other object to compare to this
	 * @return -- true if other is equal to this
	 */
	public boolean equals(Object other) {
		if (!(other instanceof PhoneNumber))
			return false;

		PhoneNumber rhs = (PhoneNumber) other;
		PhoneNumber lhs = this;

		return lhs.areaCode.equals(rhs.areaCode) && lhs.trunk.equals(rhs.trunk) && lhs.rest.equals(rhs.rest);
	}

	@Override
	public int hashCode() {
		return areaCode.hashCode() + trunk.hashCode() + rest.hashCode();
	}
}
