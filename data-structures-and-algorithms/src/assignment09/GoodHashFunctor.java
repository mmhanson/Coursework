package assignment09;

/**
 * An example of a good hash function that results in little/no collisions.
 * 
 * @author Maxwell Hanson
 * @since Mar 27, 2018
 * @version 1.1.0
 */
public class GoodHashFunctor implements HashFunctor {
	/**
	 * Hash an item.
	 * 
	 * This hash code is derived using the product-sum algorithm used by java 1.2+
	 * and described {@link https://en.wikipedia.org/wiki/Java_hashCode()#The_java.lang.String_hash_function here}
	 * 
	 * @param item -- the item to has
	 * @return -- the hash of the item
	 */
	@Override
	public int hash(String item) {
		int hash = 0;
		
		for (int ind = 0; ind < item.length(); ind++) {
			hash += (item.charAt(ind) * (Math.pow(31.0, item.length() - 1 - ind)));
		}
		
		return hash;
	}

}
