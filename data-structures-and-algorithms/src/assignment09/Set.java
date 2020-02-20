package assignment09;

import java.util.Collection;


/**
 * An interface for a set of statically-typed items. This set will not
 * contain any duplicates. Similar to Java's set, but simpler.
 *
 * @author Maxwell Hanson
 * @since March 15, 2018
 * @version 1.1.0
 */
public interface Set<Type> {
  /**
   * Add an item to the set.
   * 
   * @param item -- the element to add
   * @return -- true if the size of the set chagned after the call, false otherwise
   */
  public boolean add(Type item);

  /**
   * Add many items to the set.
   * 
   * @param items -- the collection of items whose presence is ensured in this set
   * @return -- true if the size of the set changed after the call, false otherwise
   */
  public boolean addAll(Collection<? extends Type> items);

  /**
   * Remove all items from the set.
   */
  public void clear();

  /**
   * Determine if the set contains an item.
   * 
   * @param item -- the item sought in this set
   * @return -- true if the set contains the item, false otherwise
   */
  public boolean contains(Type item);

  /**
   * Determine if the set contains many items.
   * 
   * @param items -- the collection of items to check the set against
   * @return -- true if every item in the collection is in the set, false otherwise
   */
  public boolean containsAll(Collection<? extends Type> items);

  /**
   * Determine if this set is empty
   * 
   * @return -- true if the set contains no items, false otherwise
   */
  public boolean isEmpty();
  
  /**
   * Calculate the number of items in the set.
   * 
   * @return -- the size of the set (number of elements)
   */
  public int size();
}
