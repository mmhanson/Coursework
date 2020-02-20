// Tests for the Formula class written by Maxwell Hanson (u0985911), September 2018.
// Written for CS 3500 Project Set 3

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace FormulaTester
{
    //// <summary>
    //// A class to test the Forumla class.
    //// </summary>
    [TestClass]
    public class FormulaTestser
    {
        /// === test the constructor ===
        // Success cases

        /// <summary>
        /// test that (3 + 2) returns 5
        /// tests that an integer following an open parenthesis is valid (according to the parenthesis following rule)
        /// tests the balanced parenthesis rule with one set of parenthesis
        /// tests passing an open parenthesis as the first token is valid (according to the first token rule)
        /// tests passing a close parenthesis as the last token is valid (according to the last token rule)
        /// test passing an integer in the formula is valid (it is supossed to be converted to double float)
        /// </summary>
        [TestMethod()]
        public void AddditionInParenthesisTest()
        {
            object res;
            object expRes = 5.0;

            Formula f = new Formula("(3 + 2)");
            res = f.Evaluate(x => 0);

            Assert.AreEqual(res, expRes);
        }

        /// <summary>
        /// test that 3 + (3.2 + 2) returns 8.2
        /// tests that an open parenthesis following an operator is valid (according to the parenthesis following rule)
        /// tests that a decimal following an open parentehsis is valid (according to the parenthesis following rule)
        /// tests that an integer as the first token is valid (according to the first token rule)
        /// </summary>
        [TestMethod()]
        public void ParenthesisDecimalAdditionTest()
        {
            object res;
            object expRes = 8.2;

            Formula f = new Formula("3 + (3.2 + 2)");
            res = f.Evaluate(x => 0);

            Assert.AreEqual(res, expRes);
        }

        /// <summary>
        /// test that 3.5 + 8 is 11.5
        /// tests that a decimal as the first token is valid (according to the first token rule)
        /// tests that an integer as the last token is valid (according to the last token rule)
        /// </summary>
        [TestMethod()]
        public void DecimalIntegerAdditionTest()
        {
            object res;
            object expRes = 11.5;

            Formula f = new Formula("3.5 + 8");
            res = f.Evaluate(x => 0);

            Assert.AreEqual(res, expRes);
        }

        /// <summary>
        /// test that A5 + 8.5 is 11.5 where A5 is 3
        /// tests that a varaible as the first token is valid (according to the first token rule)
        /// tests that passing a decimal as the last token is valid (according to the last token rule)
        /// </summary>
        [TestMethod()]
        public void VariableDecimalAdditionTest()
        {
            object res;
            object expRes = 11.5;
            Func<String, Double> lookup = x => 3; // all variables are 3

            Formula f = new Formula("A5 + 8.5");
            res = f.Evaluate(lookup);

            Assert.AreEqual(res, expRes);
        }

        /// <summary>
        /// test that A6 + A7 is 12 where A6 and A7 are 6
        /// tests that a variable as the last token is valid (according to the last token rule)
        /// tests that a formula of only variables is valid
        /// </summary>
        [TestMethod()]
        public void DoubleVariableAdditionTest()
        {
            object res;
            object expRes = 12.0;
            Func<String, Double> lookup = x => 6; // all variables are 3

            Formula f = new Formula("A6 + A7");
            res = f.Evaluate(lookup);

            Assert.AreEqual(res, expRes);
        }

        /// <summary>
        /// test that (((A2 + A3))) returns 9 where A2 is 3 and A3 is 6
        /// tests that an open parenthesis following an open parenthesis is valid (according to the parenthesis following rule)
        /// tests that a variable following an open parenthesis is valid (according to the parenthesis following rule)
        /// tests that a variable following an operator is valid (accroding to the parenthesis following rule)
        /// </summary>
        [TestMethod()]
        public void ManyParenthesisTest()
        {
            object res;
            object expRes = 9.0;
            // A2 is 3, A3 is 6
            Func<String, Double> lookup = x =>
            {
                if (x.Equals("A2"))
                {
                    return 3;
                }
                return 6;
            };

            Formula f = new Formula("(((A2 + A3)))");
            res = f.Evaluate(lookup);

            Assert.AreEqual(res, expRes);
        }

        /// <summary>
        /// test that (3 + (5 * (8 / 2) + 18)) is 41
        /// tests many nested, yet balanced parenthesis is valid (according to the balanced parenthesis rule)
        /// </summary>
        [TestMethod()]
        public void MixedManyParenthesisTest()
        {
            object res;
            object expRes = 41.0;

            Formula f = new Formula(" (3 + (5 * (8 / 2) + 18))");
            res = f.Evaluate(x => 0);

            Assert.AreEqual(res, expRes);
        }

        /// <summary>
        /// test that 1e3 + 4 is 1004
        /// tests that scientific notation is valid according to the constructor
        /// </summary>
        [TestMethod()]
        public void Test()
        {
            object res;
            object expRes = 1004.0;

            Formula f = new Formula("1e3 + 4");
            res = f.Evaluate(x => 0);

            Assert.AreEqual(res, expRes);
        }
        
        // Erroneous cases
        
        /// <summary>
        /// test 3 , 6 throws FFE
        /// tests that the character one ascii-value away from the '+' operator is illegal
        /// note that ascii(+) - 1 is not tested since that character is '*', and is legal
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OperatorAsciiEdgeCaseTest()
        {
            object res;

            Formula f = new Formula("3 , 6");
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test 3 . 6 throws FFE
        /// tests that the character one ascii-value away from the '-' operator is illegal
        /// note that ascii(-) - 1 is not tested since that character is ',', and has already been tested
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OperatorAsciiEdgeCAseTest2()
        {
            object res;

            Formula f = new Formula("3 . 6");
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test 3 ) 6 throws FFE
        /// tests that the character one ascii-value away from the '*' operator is illegal
        /// note that ascii(*) + 1 is not tested since that character is '+', and is legal
        /// also note that ascii(/) +- 1 is not tested since both of those values are {0, .}, which are legal or already tested
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OperatorAsciiEdgeCaseTest3()
        {
            object res;

            Formula f = new Formula("3 ) 6");
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that : + 8 throws FFE
        /// tests that the character one ascii-value away from '9' is illegal
        /// note that the character ascii(0) - 1 is not tested since that character is /, and is legal
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OperatorAsciiEdgeCaseTest4()
        {
            object res;

            // identity normalizer and inverse identity validator used
            Formula f = new Formula(": + 8", x => x, x => false);
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "" throws FFE
        /// tests that an empty formula is invalid
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void EmptyFormulaTest()
        {
            object res;

            Formula f = new Formula("");
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "(3 + 2))" throws FFE
        /// tests that a formula with one open parentehsis and two close parenthesis is invalid (according to the right-parenthesis rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void UnbalancedParenthesisTest()
        {
            object res;

            Formula f = new Formula("(3 + 2))");
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "3 + 2)" throws FFE
        /// test that a formula with no open parenthesis and one close parenthesis is invalid (according to the right-parenthesis rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void UnbalancedParenthesisTest2()
        {
            object res;

            Formula f = new Formula("3 + 2)");
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "(((3 + 2)))))" throws FFE
        /// tests that a formula with several open parenthesis and several more close parenthesis is invalid (according to the right-parenthesis rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void UnbalancedParenthesisTest3()
        {
            object res;

            Formula f = new Formula("(((3 + 2)))))");
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "3 + 2)))" throws FFE
        /// tests that a formula with no open parenthesis and several more close parenthesis is invalid (according to the right-parenthesis rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void UnbalancedParenthesisTest4()
        {
            object res;

            Formula f = new Formula("3 + 2)))");
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "(3 + 2" throws FFE
        /// tests that a formula with one open parenthesis and no close parenthesis is invalid (according to the balanced-parenthesis rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void UnbalancedParenthesisTest5()
        {
            object res;

            Formula f = new Formula("(3 + 2");
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "(((3 + 2" throws FFE
        /// tests that a formula with several open parenthesis and no close parenthesis is invalid (according to the balanced-parenthesis rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void UnbalancedParenthesisTest6()
        {
            object res;

            Formula f = new Formula("(((3 + 2");
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "(((((3 + 2)))" throws FFE
        /// tests that a formula with several open parenthesis and several (less) close parenthesis is invalid (according to the balanced-parenthesis rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void UnbalancedParenthesisTest7()
        {
            object res;

            Formula f = new Formula("(((((3 + 2)))");
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "'" throws FFE
        /// tests that the character -1 ascii valid away from '(' as the first character is invalid (according to the first token rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FirstTokenAsciiEdgeCaseTest()
        {
            object res;

            // lambda functions make identity normalizer and inverse identity validator
            Formula f = new Formula("'", x => x, x => false);
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that ")" throws FFE
        /// tests that the character +1 ascii valid away from '(' as the first character is invalid (according to the first token rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FirstTokenAsciiEdgeCaseTest2()
        {
            object res;

            Formula f = new Formula(")", x => x, x => false);
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "/" throws FFE
        /// tests that the character -1 ascii valid away from '0' as the first character is invalid (according to the first token rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FirstTokenAsciiEdgeCaseTest3()
        {
            object res;

            Formula f = new Formula("/", x => x, x => false);
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that ":" throws FFE
        /// tests that the character +1 ascii valid away from '9' as the first character is invalid (according to the first token rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FirstTokenAsciiEdgeCaseTest4()
        {
            object res;

            Formula f = new Formula(":", x => x, x => false);
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "@" where A is 3 throws FFE
        /// tests that the character -1 ascii valid away from 'A' as the first character is invalid (according to the first token rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FirstTokenAsciiEdgeCaseTest5()
        {
            object res;

            Formula f = new Formula("@", x => x, x => false);
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "B" throws FFE
        /// tests that the character +1 ascii valid away from 'A' as the first character is invalid (according to the first token rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FirstTokenAsciiEdgeCaseTest6()
        {
            object res;

            Formula f = new Formula("B", x => x, x => false);
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "(" throws FFE
        /// tests that the "(" character, while valid, throws FFE when it is the only variable in the expression
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void FirstTokenAsciiEdgeCaseTest7()
        {
            object res;

            Formula f = new Formula("(", x => x, x => false);
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "+ 3" throws FFE
        /// tests that an operator as the first token is invalid.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OperatorFirstTokenErrorTest()
        {
            object res;

            Formula f = new Formula("+ 3", x => x, x => false);
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "3 + (" throws FFE
        /// tests that the character -1 ascii valid away from ')' as the first character is invalid (according to the last token rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void LastTokenAsciiEdgeCaseTest()
        {
            object res;

            Formula f = new Formula("3 + (", x => x, x => false);
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "3 + *" throws FFE
        /// tests that the character +1 ascii valid away from ')' as the first character is invalid (according to the last token rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void LastTokenAsciiEdgeCaseTest2()
        {
            object res;

            Formula f = new Formula("3 + *", x => x, x => false);
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "3 + /" throws FFE
        /// tests that the character -1 ascii valid away from '0' as the first character is invalid (according to the last token rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void LastTokenAsciiEdgeCaseTest3()
        {
            object res;

            Formula f = new Formula("3 + /", x => x, x => false);
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "3 + :" throws FFE
        /// tests that the character +1 ascii valid away from '9' as the first character is invalid (according to the last token rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void LastTokenAsciiEdgeCaseTest4()
        {
            object res;

            Formula f = new Formula("3 + :", x => x, x => false);
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "3 + @" where A is 3 throws FFE
        /// tests that the character -1 ascii valid away from 'A' as the first character is invalid (according to the last token rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void LastTokenAsciiEdgeCaseTest5()
        {
            object res;
            Func<String, double> lookup = x => 3;

            Formula f = new Formula("3 + @", x => x, x => false);
            res = f.Evaluate(lookup);
        }

        /// <summary>
        /// test that "8 + B" throws FFE
        /// tests that the character +1 ascii valid away from 'A' as the first character is invalid (according to the last token rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void LastTokenAsciiEdgeCaseTest6()
        {
            object res;

            Formula f = new Formula("8 + B", x => x, x => false);
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "3 +" throws FFE
        /// tests that an operator as the last token is invalid (according to the last token rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OperatorLastTokenErrorTest()
        {
            object res;

            Formula f = new Formula("3 +", x => x, x => false);
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "3 + + 8" throws FFE
        /// tests that an operator followed by an operator is invalid (accoridng to the parenthesis following rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConsecutiveOperatorsTest()
        {
            object res;

            Formula f = new Formula("3 + + 8");
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "( + 8" throws FFE
        /// tests that an open parenthesis followed by an operator is invalid (accoridng to the parenthesis following rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OpenParenthesisFollowedByOperatorTest()
        {
            object res;

            Formula f = new Formula("( + 8");
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "3 + 8 + ()" throws FFE
        /// tests that a close parenthesis followed by an open parenthesis is invalid (accoridng to the parenthesis following rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void EmptyParenthesisTest()
        {
            object res;

            Formula f = new Formula("3 + 8 + ()");
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "(3 + ) * 8" throws FFE
        /// tests that an operator followed by a close parenthesis is invalid (accoridng to the parenthesis following rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OperatorThenCloseParenthesisTest()
        {
            object res;

            Formula f = new Formula("(3 + ) * 8");
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "3 3" throws FFE
        /// tests that a number followed by a number is invalid (according to the extra following rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConsecutiveOperandsTest()
        {
            object res;

            Formula f = new Formula("3 3");
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test that "A6 A7" where A6, A7 are 8 throws FFE
        /// tests that a variable followed by a variable is invalid (according to the extra following rule)
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ConsecutiveVariablesTest()
        {
            object res;
            Func<String, Double> lookup = x => 8;

            Formula f = new Formula("A6 A7");
            res = f.Evaluate(lookup);
        }

        /// <summary>
        /// test that "+" throws FFE
        /// tests that an operator as the only token is invalid
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SingleOperatorTest()
        {
            object res;

            Formula f = new Formula("+");
            res = f.Evaluate(x => 0);
        }
        
        // Edge cases
        
        /// <summary>
        /// test that "8" is 8
        /// tests that a formula with a single (integer) token is valid
        /// </summary>
        [TestMethod()]
        public void SingleOperandTest()
        {
            object res;
            object expRes = 8.0;

            Formula f = new Formula("8");
            res = f.Evaluate(x => 0);

            Assert.AreEqual(expRes, res);
        }

        /// <summary>
        /// test that "8.3" is 8.3
        /// tests that a formula with a single (decimal) token is valid
        /// </summary>
        [TestMethod()]
        public void SingleDecimalTest()
        {
            object res;
            object expRes = 8.3;

            Formula f = new Formula("8.3");
            res = f.Evaluate(x => 0);

            Assert.AreEqual(expRes, res);
        }

        /// <summary>
        /// test that "A7" where A7 is 3, is 3
        /// tests that a formula with a single (variable) token is valid
        /// </summary>
        [TestMethod()]
        public void SingleVariableTest()
        {
            object res;
            object expRes = 3.0;
            Func<String, Double> lookup = x => 3;

            Formula f = new Formula("A7");
            res = f.Evaluate(lookup);

            Assert.AreEqual(expRes, res);
        }
        
        // Extranuous cases
        
        /// <summary>
        /// test that "1 + 2 + 3 + ... + 10,000" is valid
        /// tests passing an extreme amount of tokens to the constructor in the formula (stress test)
        /// </summary>
        [TestMethod()]
        public void StressOperandTest()
        {
            object res;
            object expRes = 9.0;

            // create very large formula
            String formula = "1";
            for (int i = 2; i < 10000; i++)
            {
                formula += ("+ " + i.ToString() + " ");
            }
            formula += "10000";

            Formula f = new Formula("formula");
            res = f.Evaluate(x => 0);
        }

        /// <summary>
        /// test passing extremely small decimals in the formula (test for precision?)
        /// </summary>
        [TestMethod()]
        public void PrecisionTest()
        {
            object res;
            object expRes = 1.00000000000001;

            Formula f = new Formula("1.00000000000001");
            res = f.Evaluate(x => 0);

            Assert.AreEqual(expRes, res);
        }

        // === Test the evaluator ===
        // Success cases

        /// <summary>
        /// test that "2.24 + 3.5 = 5.74 =/= 5 or 6"
        /// tests the numbers are formatted as double floats and not ints
        /// </summary>
        [TestMethod()]
        public void DoubleDatatypeTest()
        {
            object res;
            object expRes = 5.74;

            Formula f = new Formula("2.24 + 3.5");
            res = f.Evaluate(x => 0);

            Assert.AreEqual(expRes, res);
        }

        /// <summary>
        /// test that "3.111222333444555" is 3.111222333444555
        /// tests that numbers are double floats and not just floats
        /// </summary>
        [TestMethod()]
        public void DoubleFloatTest()
        {
            object res;
            object expRes = 3.11122233344455;

            Formula f = new Formula("3.11122233344455");
            res = f.Evaluate(x => 0);

            Assert.AreEqual(expRes, res);
        }

        /// <summary>
        /// test that "3e4" is 30000
        /// tests that a scientific notation number is the same as the non-scientific notation
        /// </summary>
        [TestMethod()]
        public void ScientificNotationTest()
        {
            object res;
            object expRes = 30000.0;

            Formula f = new Formula("3e4");
            res = f.Evaluate(x => 0);

            Assert.AreEqual(expRes, res);
        }

        // Erroneous cases

        /// <summary>
        /// test that "3 / 0" yields FormulaError
        /// tests that dividing by zero gives an error
        /// </summary>
        [TestMethod()]
        public void DivisionByZeroTest()
        {
            object res;
            object expRes = new FormulaError("Attempted to divide by zero.");

            Formula f = new Formula("3 / 0");
            res = f.Evaluate(x => 0);

            Assert.AreEqual(expRes, res);
        }

        /// <summary>
        /// test that "A2 + 3" where A2 is undefined yeilds FormulaError
        /// tests that undefined variables give an error
        /// </summary>
        [TestMethod()]
        public void UndefineVariableTest()
        {
            object res;
            object expRes = new FormulaError("");
            // A2 is 3, A3 is 6
            Func<String, Double> lookup = x =>
            {
                throw new ArgumentException("");
            };

            Formula f = new Formula("A2 + 3");
            res = f.Evaluate(lookup);

            Assert.AreEqual(res, expRes);
        }
        
        // === test the equality method and operator ===
        
        /// <summary>
        /// test that "2.0" equals "2.000"
        /// tests that decimals with extra zeroes after the decimal are equal
        /// </summary>
        [TestMethod()]
        public void DecimalZeroesTest()
        {
            Formula f = new Formula("2.0");
            Formula f2 = new Formula("2.000");

            Assert.IsTrue(f.Equals(f2));
            Assert.IsTrue(f == f2);
        }

        /// <summary>
        /// test that "2" equals "002"
        /// tests that integers with extra zeros before the digits are equal
        /// </summary>
        [TestMethod()]
        public void IntegerZeroesTest()
        {
            Formula f = new Formula("2");
            Formula f2 = new Formula("002");

            Assert.IsTrue(f.Equals(f2));
            Assert.IsTrue(f == f2);
        }

        /// <summary>
        /// test that "2 + 3" does not equal "2 + 4" with both the '.equals' method and the '==' operator and their hash codes
        /// tests that two formulas with one different token are not equal
        /// tests that two unequal formulas do not test as equal
        /// tests that the equality method and operator return the same result with unequal formulas
        /// </summary>
        [TestMethod()]
        public void ReversedOperandsInequalityTest()
        {
            Formula f = new Formula("2 + 3");
            Formula f2 = new Formula("2 + 4");

            Assert.IsTrue(!f.Equals(f2));
            Assert.IsTrue(!(f == f2));
            Assert.IsTrue(!(f.GetHashCode().Equals(f2.GetHashCode())));
        }

        /// <summary>
        /// test that "3 + 5" has the same hash, and is equal to (using method and oeprator) as (different instance) "3 + 5"
        /// tests that two equal formulas have equal hashes
        /// tests that two equal formulas are evaluated as equal
        /// tests that the equal method and operator return the same result
        /// </summary>
        [TestMethod()]
        public void HashEqualityTest()
        {
            Formula f = new Formula("3 + 5");
            Formula f2 = new Formula("3 + 5");

            Assert.IsTrue(f.Equals(f2));
            Assert.IsTrue(f == f2);
            Assert.AreEqual(f.GetHashCode(), f2.GetHashCode());
        }

        /// <summary>
        /// test that "5 + 3" has a different hash as "3 + 5"
        /// tests that two unequal formulas have different hashes
        /// </summary>
        [TestMethod()]
        public void HashInequalityTest()
        {
            Formula f = new Formula("5 + 3");
            Formula f2 = new Formula("3 + 5");

            Assert.IsTrue(f != f2);
            Assert.IsTrue(!f.Equals(f2));
            Assert.AreNotEqual(f.GetHashCode(), f2.GetHashCode());
        }
        
        // === test the getVariables method ===
        
        /// <summary>
        /// test that "3 + 3".getVariables() enumerates nothing
        /// tests that a formula with no variables enumerates no variables
        /// </summary>
        [TestMethod()]
        public void NothingVariableEnumeratorTest()
        {
            Formula f = new Formula("3 + 3");
            IEnumerator<String> varEnumerator = f.GetVariables().GetEnumerator();

            Assert.IsTrue(!varEnumerator.MoveNext());
        }

        /// <summary>
        /// test that "A6".getVariables() where A6 -> a6 in the normalizer enumerates "a6"
        /// tests that a formula with a normalized variable enumerates the normalized variable and not the original
        /// test that a formula with a single variable enumerates that single variable
        /// </summary>
        [TestMethod()]
        public void SingleVariableEnumerationTest()
        {
            Func<String, String> normalizer = x =>
            {
                return "a6";
            };

            Formula f = new Formula("A6", normalizer, x => true);
            IEnumerator<String> varEnumerator = f.GetVariables().GetEnumerator();

            Assert.IsTrue(varEnumerator.MoveNext());
            Assert.AreEqual("a6", varEnumerator.Current);
        }

        /// <summary>
        /// test that "A6".getVariables() with no normalizer enumerates "A6"
        /// tests that a formula with no nomralized variables enumerates the original variable
        /// </summary>
        [TestMethod()]
        public void SingleVariableEnumerationTest2()
        {
            Formula f = new Formula("A6");
            IEnumerator<String> varEnumerator = f.GetVariables().GetEnumerator();

            Assert.IsTrue(varEnumerator.MoveNext());
            Assert.AreEqual("A6", varEnumerator.Current);
        }

        /// <summary>
        /// test that "3 + (8 * 5) + F5 + (9 / 3) * 4".getVariables() with no normalizer enumerates "F5"
        /// tests that a formula with one variable (in a larger expression) enumerates the variable
        /// </summary>
        [TestMethod()]
        public void ComplexExpressionVariableEnumerationTest()
        {
            Formula f = new Formula("3 + (8 * 5) + F5 + (9 / 3) * 4");
            IEnumerator<String> varEnumerator = f.GetVariables().GetEnumerator();

            Assert.IsTrue(varEnumerator.MoveNext());
            Assert.AreEqual("F5", varEnumerator.Current);
        }

        /// <summary>
        /// test that "3 + (8 * 5 + T5) + F5 + (A6 / 3) * C3".getVariables() with no normalizer enumerates "T5, F5, A6, C3" (in no particular order)
        /// tests that a formula with many variables in a larger expression enumerates the variables correctly
        /// </summary>
        [TestMethod()]
        public void ComplexVariableEnumerationTest2()
        {
            Formula f = new Formula("3 + (8 * 5 + T5) + F5 + (A6 / 3) * C3");
            IEnumerator<String> varEnumerator = f.GetVariables().GetEnumerator();

            Assert.IsTrue(varEnumerator.MoveNext());
            Assert.IsTrue(
                varEnumerator.Current.Equals("T5") ||
                varEnumerator.Current.Equals("F5") ||
                varEnumerator.Current.Equals("A6") ||
                varEnumerator.Current.Equals("C3"));
            Assert.IsTrue(varEnumerator.MoveNext());
            Assert.IsTrue(
                varEnumerator.Current.Equals("T5") ||
                varEnumerator.Current.Equals("F5") ||
                varEnumerator.Current.Equals("A6") ||
                varEnumerator.Current.Equals("C3"));
            Assert.IsTrue(varEnumerator.MoveNext());
            Assert.IsTrue(
                varEnumerator.Current.Equals("T5") ||
                varEnumerator.Current.Equals("F5") ||
                varEnumerator.Current.Equals("A6") ||
                varEnumerator.Current.Equals("C3"));
            Assert.IsTrue(varEnumerator.MoveNext());
            Assert.IsTrue(
                varEnumerator.Current.Equals("T5") ||
                varEnumerator.Current.Equals("F5") ||
                varEnumerator.Current.Equals("A6") ||
                varEnumerator.Current.Equals("C3"));
        }

        // === test the toString method ===

        /// <summary>
        /// test that "3 + 3".toString() is "3+3"
        /// tests that the spaces are removed from the formula
        /// </summary>
        [TestMethod()]
        public void StringRepresentationTest()
        {
            Formula f = new Formula("3 + 3");
            String str = f.ToString();
            
            Assert.AreEqual("3+3", str);
        }

        /// <summary>
        /// test that "A6 + 3".toString() is "a6+3" where A6 -> a6 in the normalizer
        /// tests that variables are normalized in the toString method
        /// </summary>
        [TestMethod()]
        public void VariableNormalizationInStringRepresentationTest()
        {
            Func<String, String> normalizer = x => "a6";

            Formula f = new Formula("A6 + 3", normalizer, x => true);
            String str = f.ToString();

            Assert.AreEqual("a6+3", str);
        }

        /// <summary>
        /// test that "3 + 3".toString() equals "3 + 3".toString()
        /// tests that two equal formulas have equal strings
        /// </summary>
        [TestMethod()]
        public void StringRepresentationEqualityTest()
        {
            Formula f = new Formula("3 + 3");
            Formula f2 = new Formula("3 + 3");

            Assert.AreEqual(f.ToString(), f2.ToString());
        }

        /// <summary>
        /// test that "3 + (8 * 5) + 6 + (9 / 3) * 4".toString() equals "3+(8*5)+6+(9/3)*4"
        /// tests the toString method on a larger expression
        /// </summary>
        [TestMethod()]
        public void LargeExpressionCollapseStringTest()
        {
            Formula f = new Formula("3 + (8 * 5) + 6 + (9 / 3) * 4");
            String str = f.ToString();

            Assert.AreEqual("3+(8*5)+6+(9/3)*4", str);
        }

        // === test the normalizer and validator ===

        /// <summary>
        /// test that "A6 + a6" where a6 is 3 and A6 -> a6 in the nomralizer equals 6
        /// test that two (unequal) variables that normalize to the same value have the same value
        /// </summary>
        [TestMethod()]
        public void VariableNormalizationTest()
        {
            object res;
            object expRes = 6.0;
            Func<String, String> normalizer = x => "a6";
            Func<String, Double> lookup = x =>
            {
                if (x.Equals("a6"))
                {
                    return 3;
                }
                return 0;
            };

            Formula f = new Formula("A6 + a6", normalizer, x => true);
            res = f.Evaluate(lookup);

            Assert.AreEqual(res, expRes);
        }

        /// <summary>
        /// test that "A6" where a6 is 3 and A6 -> a6 in the normalizer equals 3
        /// test that a formula with a single normalizable variable gets normalized before evaluation
        /// </summary>
        [TestMethod()]
        public void SingleVariableExpressionNormalizationTest()
        {
            object res;
            object expRes = 3.0;
            Func<String, String> normalizer = x => "a6";
            Func<String, Double> lookup = x =>
            {
                if (x.Equals("a6"))
                {
                    return 3;
                }
                return 0;
            };

            Formula f = new Formula("A6", normalizer, x => true);
            res = f.Evaluate(lookup);

            Assert.AreEqual(res, expRes);
        }

        /// <summary>
        /// test that "3 + F5 + C3 + (9 / 3) * 8 + A8" where <capital letter><number> -> <lower letter><number> in the normalizer
        ///   and f5 is 3, c3 is 2, a8 is 8 equals 40
        /// tests a formula with many variables all get normalized before evaluation
        /// </summary>
        [TestMethod()]
        public void LargeExpressionVariableNormalizationTest()
        {
            object res;
            object expRes = 40.0;
            Func<String, String> normalizer = x =>
            {
                if (x.Equals("F5"))
                {
                    return "f5";
                }
                if (x.Equals("C3"))
                {
                    return "c3";
                }
                if (x.Equals("A8"))
                {
                    return "a8";
                }
                return x;
            };
            Func<String, Double> lookup = x =>
            {
                if (x.Equals("f5"))
                {
                    return 3;
                }
                if (x.Equals("c3"))
                {
                    return 2;
                }
                if (x.Equals("a8"))
                {
                    return 8;
                }
                throw new ArgumentException("varaible '" + x + "' not found.");
            };

            Formula f = new Formula("3 + F5 + C3 + (9 / 3) * 8 + A8", normalizer, x => true);
            res = f.Evaluate(lookup);

            Assert.AreEqual(res, expRes);
        }

        /// <summary>
        /// test that "A6" is invalid where a6 = 3 and no normalizer
        /// tests that a formula with a variable and no normalizer does not normalize variables
        /// </summary>
        [TestMethod()]
        public void NoNormalizerTest()
        {
            object res;
            object expRes = new FormulaError("Variable 'A6' not found.");

            Func<String, Double> lookup = x =>
            {
                if (x.Equals("a6"))
                {
                    return 3;
                }

                throw new ArgumentException("Variable '" + x + "' not found.");
            };

            Formula f = new Formula("A6");
            res = f.Evaluate(lookup);

            Assert.AreEqual(res, expRes);
        }

        /// <summary>
        /// test that "A6 + (3 * 8) * (9 / 3) + T5" where A6, T5 = 3 is 78 with no normalizer and no validator
        /// tests that a formula with many variables and no normalizer does not change the variables
        /// tests that all variables in a formula are valid when no validator is supplied
        /// </summary>
        [TestMethod()]
        public void NoNormalizerValidatorLargeExpressionTest()
        {
            object res;
            object expRes = 78.0;
            Func<String, Double> lookup = x =>
            {
                return 3;
            };

            Formula f = new Formula("A6 + (3 * 8) * (9 / 3) + T5");
            res = f.Evaluate(lookup);

            Assert.AreEqual(res, expRes);
        }
    }
}
