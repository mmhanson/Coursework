// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta)
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens

// (Maxwell Hanson)
// Version 1.3 (9/21/18)

// Change log:
// Implemented all features required by PS3


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax; variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        /// <summary>
        /// A list of tokens is how the formula is stored in the backend.
        /// </summary>
        private List<String> formulaTokens;
        private Func<String, bool> isValidVarname;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true. 
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            // create a list to store each token
            this.formulaTokens = new List<string>();
            // assign validator to attribute for use in other methods
            this.isValidVarname = isValid;

            // normalize (variables) and store each token
            foreach(String token in GetTokens(formula))
            {
                if (isVariable(normalize(token)))
                {
                    this.formulaTokens.Add(normalize(token));
                    continue;
                }
                this.formulaTokens.Add(token);
            }

            // validate each token
            foreach (String token in this.formulaTokens)
            {
                // if it is not a valid variable name...
                if (!isValid(token))
                {
                    // and it is not an operator, parenthesis, or number
                    if (isOperator(token) || isOpenParenthesis(token) || isCloseParenthesis(token) || isDouble(token))
                    {
                        continue;
                    }

                    // then it is illegal
                    throw new FormulaFormatException("Illegal term name: '" + token + "'");
                }
            }

            // validate syntax rules
            // must be one token
            if (this.formulaTokens.Count == 0)
            {
                throw new FormulaFormatException("Empty formula");
            }

            String prevToken = formulaTokens.First(); // initially assigned to first element for second iteration
            String currToken;
            // counters for the number of parenthesis seen
            int OpenParenCnt = 0;
            int CloseParenCnt = 0;
            for (int idx = 0; idx < this.formulaTokens.Count; idx++)
            {
                currToken = this.formulaTokens.ElementAt(idx);
                // increment parenthesis count
                if (isOpenParenthesis(currToken))
                {
                    OpenParenCnt++;
                }
                if (isCloseParenthesis(currToken))
                {
                    CloseParenCnt++;
                }

                // if first token
                if (idx == 0)
                {
                    checkFirstToken(currToken);
                    continue;
                }

                // check the right-parenthesis rule
                if (CloseParenCnt > OpenParenCnt)
                {
                    throw new FormulaFormatException("More close parenthesis than open parenthesis");
                }

                // check the parenthesis-following rule
                if (isOpenParenthesis(prevToken) || isOperator(prevToken))
                {
                    checkParenthesisFollowingRule(currToken);
                }

                // check the extra-following rule
                if ( isDouble(prevToken) || isVariable(prevToken) || isCloseParenthesis(prevToken) )
                {
                    checkExtraFollowingRule(currToken);
                }

                // if last token
                if (idx == formulaTokens.Count - 1)
                {
                    checkLastToken(currToken);
                }

                // retrieve element again instead of assigning prevToken to currToken to avoid reference collision
                prevToken = this.formulaTokens.ElementAt(idx);
            }

            // check the balanced parenthesis rule
            if ( OpenParenCnt > CloseParenCnt ) {
                throw new FormulaFormatException("More open parenthesis than close parenthesis");
            }
            else if (OpenParenCnt < CloseParenCnt)
            {
                throw new FormulaFormatException("More close parenthesis than open parenthesis");
            }
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            try
            {
                object retVal = FormulaEvaluator.Evaluator.Evaluate(this.ToString(), lookup);
                return retVal;
            }
            catch (ArgumentException e)
            {
                return new FormulaError(e.Message);
            }
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            List<String> varList = new List<string>();

            foreach (String token in this.formulaTokens)
            {
                // if is variable and not already in the list (duplicates not counted twice)
                if (isVariable(token) && !varList.Contains(token))
                {
                    varList.Add(token);
                }
            }

            return varList;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            // just concatenates every token in the list of tokens
            String str = "";

            foreach (String token in this.formulaTokens)
            {
                // if they are a number, they need to be parsed as a double and then back to a string
                // this is done to ensure equal numbers are reprsented the same
                //  for example: 2.000 == 2.0 and 002 == 2.0
                if (isDouble(token))
                {
                    str += Double.Parse(token).ToString();
                    continue;
                }

                str += token;
            }

            return str;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            // parmaeter handling
            if (obj == null || !(obj is Formula))
            {
                return false;
            }

            // this goes off of the property that if two formula's 
            // string representations are the same, then they are equal
            return (this.ToString().Equals(obj.ToString()));
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            // null checking
            // note cannot use == operator since infinite recursion will occur as it is overloaded here
            if (object.ReferenceEquals(f1, null) && object.ReferenceEquals(f2, null))
            {
                return true;
            }
            if (object.ReferenceEquals(f1, null) || object.ReferenceEquals(f2, null))
            {
                return false;
            }

            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !(f1.Equals(f2));
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            // just uses the hash of the string representation since two
            // fomrulas are considered equal if their strings are equal
            return this.ToString().GetHashCode();
        }

        // === Helper Methods ===

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }

        /// <summary>
        /// Perform appropriate checks on the first token of the formula
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private void checkFirstToken(String token)
        {
            // if number or variable or open parenthesis, its fine
            if (isDouble(token) || isVariable(token) || isOpenParenthesis(token))
            {
                return;
            }

            // if not, the token is illegal as the first token
            throw new FormulaFormatException("Illegal first term: " + token);
        }

        /// <summary>
        /// Perform appropriate checks on the last token of the formula
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private void checkLastToken(String token)
        {
            // if number or variable or close parenthesis, its fine
            if (isDouble(token) || isVariable(token) || isCloseParenthesis(token))
            {
                return;
            }

            // if not, the token is illegal as the last token
            throw new FormulaFormatException("Illegal last term: " + token);
        }

        /// <summary>
        /// Perform appropriate checks for the extra-following rule.
        /// 
        /// The extra-following rule is that only a close parenthesis or an operator can follow
        /// an operator or a close parenthesis.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private void checkExtraFollowingRule(String token)
        {
            if (isOperator(token) || isCloseParenthesis(token))
            {
                return;
            }

            throw new FormulaFormatException("Only an operator or close parenthesis can follow a number, variable, or close parenthesis. Not '" + token + "'.");
        }

        /// <summary>
        /// Perform appropriate checks for the parenthesis following rule
        /// 
        /// The parenthesis-following rule is that only a number, varialbe, or open parenthesis can follow an open parenthesis.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private void checkParenthesisFollowingRule(String token)
        {
            if (isDouble(token) || isVariable(token) || isOpenParenthesis(token))
            {
                return;
            }

            throw new FormulaFormatException("Only a number, variable, or open parenthesis can follow an open parenthesis, not '" + token + "'");
        }

        /// <summary>
        /// Determine if a token is a double
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool isDouble(String token)
        {
            double retVal;
            return Double.TryParse(token, out retVal);
        }

        /// <summary>
        /// Determine if a token is a variable.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool isVariable(String token)
        {
            // if it is an operator, parenthesis, or number, then it is not a variable
            if (isOperator(token) || isOpenParenthesis(token) || isCloseParenthesis(token) || isDouble(token))
            {
                return false;
            }

            // if it is anything else, then check if a variable
            return this.isValidVarname(token);
        }

        /// <summary>
        /// Determine if a token is an open parenthesis
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static bool isOpenParenthesis(String token)
        {
            return token.Equals("(");
        }

        /// <summary>
        /// Determine if a token is a close parenthesis
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static bool isCloseParenthesis(String token)
        {
            return token.Equals(")");
        }

        /// <summary>
        /// Determine if a token is an operator.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool isOperator(String token)
        {
            return (token.Equals("+") || token.Equals("-") || token.Equals("*") || token.Equals("/"));
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// 
    /// This is not an exception since cells should display errors, not throw exceptions if there are errorsftoSt
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}
