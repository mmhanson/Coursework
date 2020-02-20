package assignment09;

import java.util.Collection;

/**
 * An implementation of a hash table.
 * 
 * This hash table uses quadratic probing to resolve hash collisions.
 * 
 * @author Maxwell Hanson
 * @since Mar 27, 2018
 * @version 1.0.0
 *
 */
public class QuadProbeHashTable implements Set<String> {
	/**
	 * The function that is used for hashing in the hash table.
	 */
	private HashFunctor functor;
	/**
	 * The storage for each of the elements in the hash table
	 */
	private String[] storage;
	/**
	 * The amount of non-null elements in storage
	 */
	private int size;
	/**
	 * The amount of slots in the storage that need to be filled before rehashing.
	 */
	private double lambda;
	/**
	 * The factor by which to multiply the storage capacity when the storage is full.
	 * 
	 * Note that the capacity will always be prime. The next largest prime number will
	 * be calculated if the new capacity is not prime.
	 */
	private int scalar;
	/**
	 * The initial, prime capacity of the storage array. Used for clearing the table.
	 */
	private int initCap;
	
	int collCount;
	
	public void setLambda(double lambda) {
		this.lambda = lambda;
	}
	
	/**
	 * Construct a hash table with a given capacity and hashing function.
	 * 
	 * The size of the hash table is always a prime number. If a non-prime
	 * number is parameterized, then the next largest prime number is used.
	 * 
	 * @param capacity -- the size of the hash table
	 * @param functor -- the hashing function to use
	 */
	public QuadProbeHashTable(int capacity, HashFunctor functor) {
		this.functor = functor;
		
		int primeCapa = findNextPrime(capacity);
		storage = new String[primeCapa];
		
		initCap = primeCapa;
		size = 0;
		scalar = 2;
		lambda = 0.7;
		collCount = 0;
	}
	
	/**
	 * Add an item to the hash table.
	 * 
	 * If an item is already in the table, then it is not added and
	 * false is returned.
	 * 
	 * @param item -- the item to add to the table.
	 * @return -- true if the item was added, false otherwise
	 */
	@Override
	public boolean add(String item) {
		int hash = hash(item, storage.length);
		int index = hash;
		
		// collision handling
		if (storage[hash] != null) {
			collCount++;
			index = quadProbe(index, item);
			
			if (index < 0) {
				return false;
			}
		}
		
		storage[index] = item;
		size++;
		
		// resize if necessary
		if ((size / storage.length) >= lambda) {
			rehash();
		}
		
		return true;
	}

	/**
	 * Add a collection of items to the table.
	 * 
	 * @param items -- the items to add
	 * @return -- true if any item was added, false otherwise
	 */
	@Override
	public boolean addAll(Collection<? extends String> items) {
		int sizeBefore = size;
		
		for (String item: items) {
			add(item);
		}
		
		if (sizeBefore == size) {
			return false;
		}
		return true;
	}

	/**
	 * Clear the hash table (delete all items).
	 * 
	 * The table will be empty after this call.
	 */
	@Override
	public void clear() {
		storage = new String[initCap];
		size = 0;
	}

	/**
	 * Determine if the table contains an item.
	 * 
	 * @param item -- the item to test against
	 * @return -- true if the item is in the hash table, false otherwise
	 */
	@Override
	public boolean contains(String item) {
		int hash = hash(item, storage.length);
		int index = hash;
		
		if (storage[index] != item) {
			index = quadProbe(index, item);
			
			// item is in storage
			if (index < 0) {
				return true;
			}
			// item is not in storage, new index was returned
			return false;
		}
		
		return true;
	}

	/**
	 * Determine if the table contains a collection of items.
	 * 
	 * @param items -- the items to test against
	 * @return -- true if all items are in the hash table, false otherwise
	 */
	@Override
	public boolean containsAll(Collection<? extends String> items) {
		for (String item: items) {
			if (!contains(item)) {
				return false;
			}
		}
		return true;
	}

	/**
	 * Determine if the table is empty.
	 * 
	 * @return -- true if the hash table has no elements, false otherwise
	 */
	@Override
	public boolean isEmpty() {
		return (size == 0);
	}

	/**
	 * Determine the size of the hash table.
	 * 
	 * @return -- the number of elements in the hash table
	 */
	@Override
	public int size() {
		return size;
	}
	
	/**
	 * Calculate the largest next prime number from a number.
	 *   
	 * If the parameter is prime, then the parameter will be returned.
	 *   
	 * @param num -- the number to find a prime number relative to
	 * @return -- the smallest prime number that is greater than, or
	 *   equal to the parameter.
	 */
	private int findNextPrime(int num) {
		if (!isPrime(num)) {
			// bertrand's theorem guarantees that for n > 1, there exists
			// a prime number p s.t. n < p < 2n
			if (num <= 1) {
				num = 2;
			}
			
			for (int primCand = num; primCand < 2*num; primCand++) {
				if (isPrime(primCand)) {
					return primCand;
				}
			}
		}
		return num;
	}
	
	/**
	 * Determine if a number is prime.
	 * 
	 * @param num -- the number to check
	 * @return -- true if the number is prime, false otherwise
	 */
	private boolean isPrime(int num) {
		// factor += 2 because all even numbers are not prime
        for (int factor = 1; factor <= Math.sqrt(num); factor += 2) {
            if (num % factor == 0) {
                return false;
            }
        }

        return true;
    }
	
	/**
	 * Hash and compress a string.
	 * 
	 * Return values are guaranteed to be i s.t. 0 <= i < ran.
	 * 
	 * @param str -- string to hash
	 * @param ran -- the range in which the compressed hash must fall
	 * @return -- the hash of the string
	 */
	private int hash(String str, int ran) {
		return Math.abs(functor.hash(str)) % ran;
	}
	
	/**
	 * Rehash the hash table.
	 * 
	 * In rehashing, a new storage array is created with a capacity
	 * approximately i: i = (scaler)j where j is the original storage 
	 * capacity and 'scaler' is the class attribute. Then each
	 * non-null entry in the original storage is re-added to the new
	 * storage. Then the original storage is replaced with the new
	 * storage.
	 */
	private void rehash() {
		int newCapacity = findNextPrime(scalar * storage.length);
		String[] newStorage = new String[newCapacity];
		
		// storage is replaced so elems from old storage can be added
		// to the new storage via the 'add' method
		String[] oldStorage = storage.clone();
		storage = newStorage;
		
		for (int curs = 0; curs < oldStorage.length; curs++) {
			if (oldStorage[curs] != null) {
				add(oldStorage[curs]);
			}
		}
	}
	
	/**
	 * Find the next available index for an item via quadratic probing.
	 * 
	 * It is assumed that the parameter (index) supplied follows these rules:
	 *   * storage[index] != null
	 *   * 0 <= index < storage.length
	 * 
	 * @param index -- the index to start at, see above restrictions
	 * @param item -- the item to find an index for
	 * @return -- i s.t. if i is positive, then i is the next available index, 
	 *   and if i is negative, then there is no available index or the item is
	 *   already in the hash table
	 */
	private int quadProbe(int index, String item) {
		for (int base = 1; base < storage.length; base++) {
			if (storage[index] == item) {
				return -1;
			}
			else if (storage[index] == null) {
				return index;
			}
			
			index += (int) Math.round(Math.pow(base, 2));
			// wrap around when over range
			index %= storage.length;
		}
		
		return -1;
	}
}
