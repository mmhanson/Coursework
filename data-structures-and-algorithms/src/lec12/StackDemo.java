package lec12;

/**
 * Simple program to demonstrate the use of a stack.
 * 
 * @author Erin Parker
 * @version February 15, 2018
 */
public class StackDemo {

	public static void main(String[] args) {
		Stack<Integer> numbers = new StackArray<Integer>();
		numbers.push(4);
		numbers.push(8);
		numbers.push(15);
		numbers.push(16);
		numbers.push(23);
		numbers.push(42);

		while(!numbers.isEmpty()) 
			System.out.println(numbers.pop());
		
		numbers.clear();
		// System.out.println(numbers.peek());    // uncomment to force exception
	}
}