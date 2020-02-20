package assignment09;

/**
 * An example of a mediocre hash function that results in some collisions.
 * 
 * @author Maxwell Hanson
 * @since Mar 27, 2018
 * @version 1.0.0
 */
public class MediocreHashFunctor implements HashFunctor {
	/**
	 * Hash an item.
	 * 
	 * This hash function uses the sum of all the characters
	 * in the string, or zero for an empty string.
	 * 
	 * @param item -- the item to has
	 * @return -- the hash of the item
	 */
	@Override
	public int hash(String item) {
		int hash = 0;
		
		for (int ind = 0; ind < item.length(); ind++) {
			hash += item.charAt(ind);
		}
		
		return hash;
	}

}
