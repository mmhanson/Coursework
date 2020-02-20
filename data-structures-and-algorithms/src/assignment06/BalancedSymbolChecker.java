package assignment06;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.Reader;

/**
 * Class containing the checkFile method for checking if the (, [, and { symbols
 * in an input file are correctly matched.
 * 
 * @author Maxwell Hanson
 * @since Jul 16, 2018
 * @version 1.0.0
 */
public class BalancedSymbolChecker {
	/**
	 * Buffer for the previous character read
	 */
	private static Character prevChar;
	/**
	 * Flag for determining if the current character is in a singled line comment.
	 */
	private static boolean singleLineCommentFlag;
	/**
	 * Flag for determining if the current character is in a multi-lined comment.
	 */
	private static boolean multiLineCommentFlag;
	/**
	 * Flag for determining if the current character is in a string literal.
	 */
	private static boolean stringLiteralFlag;
	/**
	 * Flag for determining if the current character is in a character literal.
	 */
	private static boolean charLiteralFlag;
	/**
	 * Flag for determining if the current character is in a backlash-quote.
	 */
	private static boolean charQuoteFlag;
	
	/**
	 * Determine if the symbols in a file are matched.
	 * 
	 * If an illegal symbol is found in the file, a descriptive message is returned
	 * 
	 * @param fname -- name of the input file to check
	 * @return -- a message indicating whether the input file has legal symbols or not
	 * @throws FileNotFoundException -- if the file does not exist
	 */
	public static String checkFile(String fname) throws FileNotFoundException {
		int currLine = 1;
		int currCol = 0;
		prevChar = null; // initially null, processed after first char
		singleLineCommentFlag = false;
		multiLineCommentFlag = false;
		stringLiteralFlag = false;
		charLiteralFlag = false;
		charQuoteFlag = false;
		
		// a stack is used to keep track of symbol balances
		StackLinkedList<Character> stack = new StackLinkedList<Character>();
		File file = new File(fname);
		Reader buff = new BufferedReader(new InputStreamReader(new FileInputStream(file)));
		
		// file is read character by character and the processing of each character is passed to a helper method
		int readRes;
		try {
			while ((readRes = buff.read()) != -1) {
				Character currChar = Character.valueOf( (char) readRes );
				// increment position counters
				currCol++;
				if (currChar.equals('\n')) {
					currLine++;
					currCol = 0;
				}
				
				// helper method returns false if character is illegal
				if ( !handleChar(currChar, stack) ) {
					buff.close();
					// if stack is empty, no expected symbol, prevent npe
					Character expSym = stack.isEmpty() ? ' ' : stack.pop();
					return unmatchedSymbol(currLine, currCol, currChar, inverse(expSym));
				}
				prevChar = currChar;
			}
			buff.close();
		}
		catch (IOException e) {
			return "Unexpected IO error in BufferedReader.";
		}
		
		// eof reached
		// open comment errors are returned before unmatched symbol errors
		// illegal if multiline comment still open
		if ( multiLineCommentFlag ) {
			return unfinishedComment();
		}
		// if stack is still populated, there are unmatched symbols in the file
		if ( !stack.isEmpty() ) {
			return unmatchedSymbolAtEOF(inverse(stack.peek()));
		}
		
		return allSymbolsMatch();
	}
	
	/**
	 * Handle a character being read in the file.
	 * 
	 * If a character is an opening symbol ('(', '{', or '['), then it is pushed to the stack.
	 * If a character is a closing symbol (')', '}', ']'), then the stack is popped and examined.
	 *   If the popped character matches the closing symbol, true is returned.
	 *   If the popped character does not match, false is returned.
	 * 
	 * @param currChar -- the character being handled
	 * @param stack -- the stack to handle the character with
	 * @return -- true if the character is legal, false otherwise
	 */
	private static boolean handleChar(Character currChar, StackLinkedList<Character> stack) {
		// comment, literal, and quote logic
		// first it is checked that we are not inside a literal (string or char) or a comment (single or multilined)
		// then we check if a literal or a comment was opened
		// a system of intuitively named helper methods and flags are used for this process
		if (inCharQuote()) {
			// closeCharQuote helper method not needed since only last one char
			charQuoteFlag = false;
			return true;
		}
		if (inLiteral()) {
			openCharQuotes(currChar); // quotes only used in literals/comments
			closeLiterals(currChar);
			return true;
		}
		if (inComment()) {
			openCharQuotes(currChar); // quotes only used in literals/comments
			closeComments(currChar);
			return true;
		}
		openComments(currChar);
		openLiterals(currChar);
		
		// push opening chars
		if (isOpeningSym(currChar)) {
			stack.push(currChar);
		}
		
		// pop closing chars/handle errors
		else if (isClosingSym(currChar)) {
			// if stack is empty, there is extra symbol
			if ( stack.isEmpty() ) {
				return false;
			}
			
			// if closing char is found, it must match the last opening char (top of stack)
			if (symMatch(currChar, stack.peek())) {
				stack.pop(); // only popped if legal since if illegal char needs be popped in caller
				return true;
			}
			else {
				return false;
			}
		}
		return true;
	}
	
	/**
	 * Determine if a comment was opened with the current char and set global flags accordingly.
	 * 
	 * @param currChar -- current char
	 */
	private static void openComments(Character currChar) {
		// case when first char is being analyzed
		if (prevChar == null) {
			return;
		}
		
		if (prevChar.equals('/') && currChar.equals('/')) {
			singleLineCommentFlag = true;
		}
		
		if (prevChar.equals('/') && currChar.equals('*')) {
			multiLineCommentFlag = true;
		}
		
		return;
	}
	
	/**
	 * Determine if a comment was closed with the current char and set flags accordingly.
	 * 
	 * @param currChar -- current char
	 */
	private static void closeComments(Character currChar) {
		if (currChar.equals('\n') && singleLineCommentFlag) {
			singleLineCommentFlag = false;
		}
		
		if (prevChar.equals('*') && currChar.equals('/') && multiLineCommentFlag) {
			multiLineCommentFlag = false;
		}
	}
	
	/**
	 * Determine if a string literal was closed with the current char and set flags accordingly.
	 * 
	 * @param currChar -- current char
	 */
	private static void closeLiterals(Character currChar) {
		if (currChar == '"' && stringLiteralFlag) {
			stringLiteralFlag = false;
		}
		
		if (currChar == '\'' && charLiteralFlag) {
			charLiteralFlag = false;
		}
	}
	
	/**
	 * Determine if a literal is opened with the current char and set flags accordingly.
	 * 
	 * @param currChar -- current character
	 */
	private static void openLiterals(Character currChar) {
		if (currChar == '"') {
			stringLiteralFlag = true;
		}
		
		if (currChar == '\'') {
			charLiteralFlag = true;
		}
	}
	
	/**
	 * Determine if the current character opens a backslash-quote.
	 * 
	 * @param currChar -- current char
	 */
	private static void openCharQuotes(Character currChar) {
		if (currChar == '\\') {
			charQuoteFlag = true;
		}
	}
	
	/**
	 * Determine if current char is in a comment.
	 * 
	 * @return -- true if in comment, false otherwise
	 */
	private static boolean inComment() {
		return singleLineCommentFlag || multiLineCommentFlag;
	}
	
	/**
	 * Determine if the current char is in a string literal.
	 * 
	 * @return -- true if in a literal, false otherwise
	 */
	private static boolean inLiteral() {
		return stringLiteralFlag || charLiteralFlag;
	}
	
	/**
	 * Determine if the current char is in a backslash-quote.
	 * 
	 * @return -- true if in a backslash-induced quote, false otherwise
	 */
	private static boolean inCharQuote() {
		return charQuoteFlag;
	}
	
	/**
	 * Determine if a symbol is an 'opening symbol'.
	 * 
	 * An opening symbol is (, {, or [
	 * 
	 * @param sym -- symbol to check
	 * @return -- true if (, {, or [; false otherwise
	 */
	private static boolean isOpeningSym(Character sym) {
		if (sym.equals('(') || sym.equals('{') || sym.equals('[')) {
			return true;
		}
		return false;
	}

	/**
	 * Determine if a symbol is a 'closing symbol'.
	 * 
	 * A closing symbol is ), } or ]
	 * 
	 * @param sym -- symbol to check
	 * @return -- true if ), }, ]; false otherwise
	 */
	private static boolean isClosingSym(Character sym) {
		if (sym.equals(')') || sym.equals('}') || sym.equals(']')) {
			return true;
		}
		return false;
	}
	
	/**
	 * Determine if two symbols 'match'.
	 * 
	 * Matching symbols are inverse symbols. Examples:
	 *  * ( and )
	 *  * { and }
	 *  * [ and ]
	 *  
	 * Parameter order does not matter as long as both symbols are inverses.
	 * 
	 * @param sym0 -- first symbol to analyze
	 * @param sym1 -- second symbol to analyze
	 * @return -- true if symbols match, false otherwise
	 */
	private static boolean symMatch (Character sym0, Character sym1) {
		if ( (sym0.equals('(') && sym1.equals(')')) || (sym0.equals(')') && sym1.equals('(')) ) {
			return true;
		}
		
		if ( (sym0.equals('{') && sym1.equals('}')) || (sym0.equals('}') && sym1.equals('{')) ) {
			return true;
		}
		
		if ( (sym0.equals('[') && sym1.equals(']')) || (sym0.equals(']') && sym1.equals('[')) ) {
			return true;
		}
		
		return false;
	}
	
	/**
	 * Inverse a character
	 * 
	 * "(" -> ")"
	 * "[" -> "]"
	 * "{" -> "}"
	 * Assumes only inputs are the three above
	 * 
	 * @param chr -- char to inverse
	 * @return -- inverse of char
	 */
	private static Character inverse(Character chr) {
		if (chr.equals('(')) {
			return ')';
		}
		if (chr.equals('{')) {
			return '}';
		}
		if (chr.equals('[')) {
			return ']';
		}
		
		// in case of no expected symbol
		return ' ';
	}
	
	/**
	 * Use this error message in the case of an unmatched symbol.
	 * 
	 * @param lineNumber - the line number of the input file where the matching symbol was expected
	 * @param colNumber - the column number of the input file where the matching symbol was expected
	 * @param symbolRead - the symbol read that did not match
	 * @param symbolExpected - the matching symbol expected
	 * @return the error message
	 */
	private static String unmatchedSymbol(int lineNumber, int colNumber, char symbolRead, char symbolExpected) {
		return "ERROR: Unmatched symbol at line " + lineNumber + " and column " + colNumber + ". Expected " + symbolExpected
				+ ", but read " + symbolRead + " instead.";
	}

	/**
	 * Use this error message in the case of an unmatched symbol at the end of the file.
	 * 
	 * @param symbolExpected - the matching symbol expected
	 * @return the error message
	 */
	private static String unmatchedSymbolAtEOF(char symbolExpected) {
		return "ERROR: Unmatched symbol at the end of file. Expected " + symbolExpected + ".";
	}

	/**
	 * Use this error message in the case of an unfinished comment
	 * (i.e., a file that ends with an open /* comment).
	 * 
	 * @return the error message
	 */
	private static String unfinishedComment() {
		return "ERROR: File ended before closing comment.";
	}

	/**
	 * Use this message when no unmatched symbol errors are found in the entire file.
	 * 
	 * @return the success message
	 */
	private static String allSymbolsMatch() {
		return "No errors found. All symbols match.";
	}
}