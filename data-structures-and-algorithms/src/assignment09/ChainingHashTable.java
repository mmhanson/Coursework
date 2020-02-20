package assignment09;

import java.util.Collection;
import java.util.LinkedList;

/**
 * An implementation of a hash table.
 * 
 * This hash table uses chaining to resolve hash collisions.
 * 
 * @author Maxwell Hanson
 * @since Mar 27, 2018
 * @version 1.1.0
 */
public class ChainingHashTable implements Set<String> {
	/**
	 * The array in which the elements of the set are stored
	 */
	private LinkedList<String>[] storage;
	/**
	 * The function that is used for hashing in the hash table.
	 */
	private HashFunctor functor;
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
	 * Construct a hash table of a given capacity and hash function.
	 * 
	 * @param capacity -- the initial size of the hash table
	 * @param functor -- the hashing function to use with the table
	 */
	@SuppressWarnings("unchecked")
	public ChainingHashTable(int capacity, HashFunctor functor) {
		storage = (LinkedList<String>[]) new LinkedList[capacity];
		
		initCap = capacity;
		this.functor = functor;
		lambda = 0.7;
		size = 0;
		scalar = 2;
		collCount = 0;
	}

	/**
	 * Add an item to the hash table.
	 * 
	 * @param item -- the item to add to the table.
	 * @return -- true if the item was added, false otherwise
	 */
	@Override
	public boolean add(String item) {
		int index = hash(item, storage.length);
		
		if (storage[index] == null) {
			storage[index] = new LinkedList<String>();
		}
		// collision
		else {
			collCount++;
		}
		
		if (!storage[index].contains(item)) {
			storage[index].add(item);
			size++;
		}
		else {
			return false;
		}
		
		// rehash if too full
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
	@SuppressWarnings("unchecked")
	@Override
	public void clear() {
		storage = new LinkedList[initCap];
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
		
		if (storage[hash] == null) {
			return false;
		}
		
		if (storage[hash].contains(item)) {
			return true;
		}
		return false;
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
	 * Rehash the hash table
	 * 
	 * In rehashing, a new storage array is created with a capacity
	 * twice that of the original storage. Then each non-null element
	 * of the original storage is re-added to the new storage.
	 */
	@SuppressWarnings("unchecked")
	private void rehash() {
		LinkedList<String>[] oldStorage = storage.clone();
		LinkedList<String>[] newStorage = new LinkedList[scalar * oldStorage.length];
		// storage is replaced so old entries can be rehashed with the 'add' method
		storage = newStorage;
		
		for (int curs = 0; curs < oldStorage.length; curs++) {
			if (oldStorage[curs] != null) {
				for (String entry: oldStorage[curs]) {
					add(entry);
				}
			}
		}
	}
}
