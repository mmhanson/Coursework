package lec12;

import java.util.NoSuchElementException;

/** 
 * Representation of a stack data structure (backed by a basic array).
 * 
 * @author Erin Parker & CS 2420 class
 * @version February 15, 2018
 *
 * @param <E> - the type of elements contained in the stack
 */
public class StackArray<E> implements Stack<E> {
	
	private E[] stack;
	private int top;
	
	@SuppressWarnings("unchecked")
	public StackArray() {
		stack = (E[])new Object[100];    // unsafe cast warning noted
		top = -1;
	}

	@Override
	public void clear() {
		top = -1;
	}

	@Override
	public boolean isEmpty() {
		return top == -1;
	}

	@Override
	public E peek() throws NoSuchElementException {
		if(top == -1)
			throw new NoSuchElementException();
		return stack[top];
	}

	@Override
	public E pop() throws NoSuchElementException {
		if(top == -1)
			throw new NoSuchElementException();
		
		E ret = stack[top];
		top--;
		return ret;    
		
		// one-line option: return stack[top--];
	}

	@SuppressWarnings("unchecked")
	@Override
	public void push(E element) {
		if(top + 1 == stack.length) {
			Object[] temp = new Object[stack.length * 2];
			for(int i = 0; i < stack.length; i++)
				temp[i] = stack[i];
			stack = (E[])temp;    // unsafe cast warning noted
		}
		
		top++;
		stack[top] = element;
		
		// one-line option: stack[++top] = element;
	}

	@Override
	public int size() {
		return top + 1;
	}
}