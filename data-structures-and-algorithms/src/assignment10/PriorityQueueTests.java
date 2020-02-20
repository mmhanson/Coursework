package assignment10;

import static org.junit.Assert.assertTrue;
import static org.junit.Assert.fail;

import java.util.ArrayList;
import java.util.NoSuchElementException;

import org.junit.Before;
import org.junit.Test;

/**
 * Automated tests for the PriorityQueue class.
 * 
 * @author Maxwell Hanson
 * @since Apr 4, 2018
 * @version 1.1.0
 */
public class PriorityQueueTests {
	private PriorityQueue<Integer> emptyQ;
	private PriorityQueue<Integer> oneQ;
	private Integer oneQElem;
	private PriorityQueue<Integer> manyQMinFirst;
	private PriorityQueue<Integer> manyQMinMid;
	private PriorityQueue<Integer> manyQMinLast;
	private ArrayList<Integer> manyQElems;
	private Integer manyQElemsMin;
	private Integer manyQMin;
	
	/**
	 * Setup attributes for testing
	 */
	@Before
	public void setup() {
		emptyQ = new PriorityQueue<Integer>();
		
		oneQ = new PriorityQueue<Integer>();
		oneQElem = 5;
		oneQ.add(oneQElem);
		
		manyQElems = new ArrayList<Integer>();
		manyQElems.add(541);
		manyQElems.add(733);
		manyQElems.add(322);
		manyQElems.add(921);
		manyQElems.add(2323);
		manyQElems.add(2123);
		manyQElems.add(661);
		manyQElems.add(2277);
		manyQElems.add(1232);
		manyQElemsMin = 322;
		manyQMin = 5;
		
		manyQMinFirst = new PriorityQueue<Integer>();
		manyQMinFirst.add(manyQMin);
		for(Integer elem: manyQElems) {
			manyQMinFirst.add(elem);
		}
		
		manyQMinMid = new PriorityQueue<Integer>();
		for (int i = 0; i < (manyQElems.size() / 2); i++) {
			manyQMinMid.add(manyQElems.get(i));
		}
		manyQMinMid.add(manyQMin);
		for (int i = (manyQElems.size() / 2) + 1; i < manyQElems.size(); i++) {
			manyQMinMid.add(manyQElems.get(i));
		}
		
		manyQMinLast = new PriorityQueue<Integer>();
		for (int i = 0; i < manyQElems.size(); i++) {
			manyQMinLast.add(manyQElems.get(i));
		}
		manyQMinLast.add(manyQMin);
	}
	
	/**
	 * Test the "findMin" method.
	 * 
	 * Test that NoSuchElementException is thrown with empty priority queue.
	 * Test that the sole element is returned with an instance of one element.
	 * Test that min is returned when an instance of many is created when min is inserted first.
	 * Test that min is returned when an instance of many is created when min is inserted last.
	 * Test that min is returned when an instance of many is created when min is inserted in the middle.
	 */
	@Test
	public void testFindMin() {
		Integer res, expRes;
		
		/// empty queue
		try {
			emptyQ.findMin();
			fail("");
		} catch (NoSuchElementException e) {}
		
		/// one queue
		res = oneQ.findMin();
		expRes = oneQElem;
		assertTrue(expRes.equals(res));
		
		/// many queue
		expRes = manyQMin;
		res = manyQMinFirst.findMin();
		assertTrue(expRes.equals(res));
		res = manyQMinMid.findMin();
		assertTrue(expRes.equals(res));
		res = manyQMinLast.findMin();
		assertTrue(expRes.equals(res));
	}
	
	/**
	 * Test the "deleteMin" method.
	 * 
	 * Test that NoSuchElementException is thrown with an empty instance.
	 * Test that the sole element is deleted with an instance of one.
	 * Test that min is deleted with an instance of many created when min is inserted first.
	 * Test that the heap property is maintained for the previous instance.
	 * Test that the new min element is returned after deleting the previous min.
	 * Test that min is deleted with an instance of many created when min is inserted last.
	 * Test that the heap property is maintained for the previous instance.
	 * Test that the new min element is returned after deleting the previous min.
	 * Test that min is deleted with an instance of many created when min is inserted in the middle.
	 * Test that the heap property is maintained for the previous instance.
	 * Test that the new min element is returned after deleting the previous min.
	 */
	@Test
	public void testDeleteMin() {
		Integer expRes, res;
		
		/// empty queue
		try {
			emptyQ.deleteMin();
			fail("");
		} catch (NoSuchElementException e) {}
		
		/// one queue
		res = oneQ.deleteMin();
		expRes = oneQElem;
		assertTrue(expRes.equals(res));
		assertTrue(isEmpty(oneQ));
		
		/// many queue
		expRes = manyQMin;
		res = manyQMinFirst.deleteMin();
		assertTrue(expRes.equals(res));
		assertTrue(isHeap(manyQMinFirst.toArray()));
		expRes = manyQElemsMin;
		res = manyQMinFirst.findMin();
		assertTrue(expRes.equals(res));
		
		expRes = manyQMin;
		res = manyQMinMid.deleteMin();
		assertTrue(expRes.equals(res));
		assertTrue(isHeap(manyQMinMid.toArray()));
		expRes = manyQElemsMin;
		res = manyQMinMid.findMin();
		assertTrue(expRes.equals(res));
		
		expRes = manyQMin;
		res = manyQMinLast.deleteMin();
		assertTrue(expRes.equals(res));
		assertTrue(isHeap(manyQMinLast.toArray()));
		expRes = manyQElemsMin;
		res = manyQMinLast.findMin();
		assertTrue(expRes.equals(res));
	}
	
	/**
	 * Test the "add" method.
	 * 
	 * --- for an empty instance ---
	 * Test that adding an element results in that element being the new min.
	 * Test that adding an element increments the size.
	 * Test adding 10000 elements to ensure reallocation is working.
	 * 
	 * --- for an instance of one ---
	 * Test that adding an element that is less than the og elem results in the
	 *   new elem being the new min.
	 * Test that adding an elem that is greater than the og elem results in the
	 *   old elem being the new min.
	 * Test that adding an elem that is equal to the og elem results in either
	 *   being the new min.
	 * For all previous test, test that adding the element increments the size.
	 * 
	 * --- for an instance of many ---
	 * Test that adding a new min results in the new elem being the new min.
	 * Test that adding a new non-min, non-max value results in the min being unchanged.
	 * Test that adding a new max results in the old min being unchanged.
	 * For all previous tests, test that the heap property is maintained.
	 * For all previous test, test that the size is incremented.
	 */
	@Test
	public void testAdd() {
		Integer expRes, res;
		Integer sizeBefore;
		
		/// empty queue
		sizeBefore = emptyQ.size();
		emptyQ.add(3);
		expRes = 3;
		res = emptyQ.findMin();
		assertTrue(expRes.equals(res));
		assertTrue(emptyQ.size() == (sizeBefore + 1));
		for (int i = 0; i < 10000; i++) {
			emptyQ.add(i);
		}
		
		/// one queue
		sizeBefore = oneQ.size();
		oneQ.add(1);
		expRes = 1;
		res = oneQ.findMin();
		assertTrue(expRes.equals(res));
		assertTrue(oneQ.size() == (sizeBefore + 1));
		setup(); // reset oneQ
		sizeBefore = oneQ.size();
		oneQ.add(7);
		expRes = 5;
		res = oneQ.findMin();
		assertTrue(expRes.equals(res));
		assertTrue(oneQ.size() == (sizeBefore + 1));
		setup();
		sizeBefore = oneQ.size();
		oneQ.add(5);
		expRes = 5;
		res = oneQ.findMin();
		assertTrue(expRes.equals(res));
		assertTrue(oneQ.size() == (sizeBefore + 1));
		
		/// many queue
		Integer manyQNewMin = 1;
		Integer manyQNonMin = 700;
		Integer manyQNewMax = Integer.MAX_VALUE;
		
		// manyQMinFirst
		sizeBefore = manyQMinFirst.size();
		manyQMinFirst.add(manyQNewMin);
		expRes = manyQNewMin;
		res = manyQMinFirst.findMin();
		assertTrue(expRes.equals(res));
		assertTrue(manyQMinFirst.size() == (sizeBefore + 1));
		assertTrue(isHeap(manyQMinFirst.toArray()));
		setup();
		sizeBefore = manyQMinFirst.size();
		manyQMinFirst.add(manyQNonMin);
		expRes = manyQMin;
		res = manyQMinFirst.findMin();
		assertTrue(expRes.equals(res));
		assertTrue(manyQMinFirst.size() == (sizeBefore + 1));
		assertTrue(isHeap(manyQMinFirst.toArray()));
		setup();
		sizeBefore = manyQMinFirst.size();
		manyQMinFirst.add(manyQNewMax);
		expRes = manyQMin;
		res = manyQMinFirst.findMin();
		assertTrue(expRes.equals(res));
		assertTrue(manyQMinFirst.size() == (sizeBefore + 1));
		assertTrue(isHeap(manyQMinFirst.toArray()));
		
		// manyQMinMid
		sizeBefore = manyQMinMid.size();
		manyQMinMid.add(manyQNewMin);
		expRes = manyQNewMin;
		res = manyQMinMid.findMin();
		assertTrue(expRes.equals(res));
		assertTrue(manyQMinMid.size() == (sizeBefore + 1));
		assertTrue(isHeap(manyQMinMid.toArray()));
		setup();
		sizeBefore = manyQMinMid.size();
		manyQMinMid.add(manyQNonMin);
		expRes = manyQMin;
		res = manyQMinMid.findMin();
		assertTrue(expRes.equals(res));
		assertTrue(manyQMinMid.size() == (sizeBefore + 1));
		assertTrue(isHeap(manyQMinMid.toArray()));
		setup();
		sizeBefore = manyQMinMid.size();
		manyQMinMid.add(manyQNewMax);
		expRes = manyQMin;
		res = manyQMinMid.findMin();
		assertTrue(expRes.equals(res));
		assertTrue(manyQMinMid.size() == (sizeBefore + 1));
		assertTrue(isHeap(manyQMinMid.toArray()));
		
		// manyQMinLast
		sizeBefore = manyQMinLast.size();
		manyQMinLast.add(manyQNewMin);
		expRes = manyQNewMin;
		res = manyQMinLast.findMin();
		assertTrue(expRes.equals(res));
		assertTrue(manyQMinLast.size() == (sizeBefore + 1));
		assertTrue(isHeap(manyQMinLast.toArray()));
		setup();
		sizeBefore = manyQMinLast.size();
		manyQMinLast.add(manyQNonMin);
		expRes = manyQMin;
		res = manyQMinLast.findMin();
		assertTrue(expRes.equals(res));
		assertTrue(manyQMinLast.size() == (sizeBefore + 1));
		assertTrue(isHeap(manyQMinLast.toArray()));
		setup();
		sizeBefore = manyQMinLast.size();
		manyQMinLast.add(manyQNewMax);
		expRes = manyQMin;
		res = manyQMinLast.findMin();
		assertTrue(expRes.equals(res));
		assertTrue(manyQMinLast.size() == (sizeBefore + 1));
		assertTrue(isHeap(manyQMinLast.toArray()));
	}
	
	/**
	 * Determine if an array is a binary heap.
	 * 
	 * @param array -- the array to determine binary heap property of
	 * @return -- true if the array is a binary heap
	 */
	private boolean isHeap(Object[] array) {
		return isHeapRec(array, 0);
	}
	
	/**
	 * Determine if an array is a binary heap starting at an index.
	 * 
	 * @param array -- the array to determine heap property of
	 * @param ind -- index to start
	 * @return -- true if the array is a binary heap
	 */
	private boolean isHeapRec(Object[] array, int ind) {
		int leftChildInd = (2 * ind) + 1;
		int rightChildInd = (2 * ind) + 2;
		Integer curr = (Integer) array[ind];
		
		if (leftChildInd < array.length) {
			Integer leftChild = (Integer) array[leftChildInd];
			if (leftChild < curr) {
				return false;
			}
			if (!isHeapRec(array, leftChildInd)) {
				return false;
			}
		}
		
		if (rightChildInd < array.length) {
			Integer rightChild = (Integer) array[rightChildInd];
			if (rightChild < curr) {
				return false;
			}
			if (!isHeapRec(array, rightChildInd)) {
				return false;
			}
		}
		
		return true;
	}
	
	private boolean isEmpty(PriorityQueue<Integer> q) {
		try {
			q.findMin();
			return false;
		}
		catch (NoSuchElementException e) {
			return true;
		}
	}
}
