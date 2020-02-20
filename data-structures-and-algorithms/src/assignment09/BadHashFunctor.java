package assignment09;

/**
 * An example of a bad hash function that results in many collisions.
 * 
 * @author Maxwell Hanson
 * @since Mar 27, 2018
 * @version 1.1.0
 */
public class BadHashFunctor implements HashFunctor {
	/**
	 * Hash an item.
	 * 
	 * This hash function uses the integer value of the first
	 * character of the string, or zero for an empty string.
	 * 
	 * @param item -- the item to has
	 * @return -- the hash of the item
	 */
	@Override
	public int hash(String item) {
		int hash;
		
		try {
			hash = item.charAt(0);
		}
		catch (IndexOutOfBoundsException e) {
			hash = 0;
		}
		
		return hash;
	}

}
