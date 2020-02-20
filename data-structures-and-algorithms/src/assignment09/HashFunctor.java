package assignment09;

/**
 * A functional interface for hashing.
 * 
 * @author Maxwell Hanson
 * @since March 15, 2018
 * @version 1.0.0
 */
@FunctionalInterface
public interface HashFunctor {
  public int hash(String item);

}