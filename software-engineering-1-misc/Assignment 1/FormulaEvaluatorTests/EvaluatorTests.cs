using Microsoft.VisualStudio.TestTools.UnitTesting;
//using FormulaEvaluator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaEvaluator.Tests
{
    [TestClass()]
    public class EvaluatorTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void testUndefinedVariable()
        {
            // test looking up a varaible that is undefined
            String expr = "6 + F7";
            int res;
            Evaluator.Lookup varResolver = new Evaluator.Lookup(varLookup);

            res = Evaluator.Evaluate(expr, varResolver);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void testMalformedVariableNumberBeforeLetter()
        {
            // test looking up a malformed variable with a number before the letter
            String expr = "6 + 7F";
            int res;
            Evaluator.Lookup varResolver = new Evaluator.Lookup(varLookup);

            res = Evaluator.Evaluate(expr, varResolver);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void testMalformedVariableSpecialCharInterloper()
        {
            // test evaluating a malformed variable (non-alphanumeric symbol in betweeen)
            String expr = "6 + A%8";
            int res;
            Evaluator.Lookup varResolver = new Evaluator.Lookup(varLookup);

            res = Evaluator.Evaluate(expr, varResolver);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void testMalformedVariableSpecialCharBefore()
        {
            // test evaluating a malformed variable (non-alphanumeric symbol at start)
            String expr = "6 + #A8";
            int res;
            Evaluator.Lookup varResolver = new Evaluator.Lookup(varLookup);

            res = Evaluator.Evaluate(expr, varResolver);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void testMalformedVariableSpecialCharAfter()
        {
            // test evaluating a malformed variable (non-alphanumeric symbol at end)
            String expr = "6 + A8?";
            int res;
            Evaluator.Lookup varResolver = new Evaluator.Lookup(varLookup);

            res = Evaluator.Evaluate(expr, varResolver);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void testIllegalToken()
        {
            // test passing an illegal token into the expression
            String expr = "5 + ~";
            int res;
            Evaluator.Lookup varResolver = new Evaluator.Lookup(varLookup);

            res = Evaluator.Evaluate(expr, varResolver);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void testAllIllegalTokens()
        {
            // test passing a string of illegal tokens
            String expr = "друг";
            int res;
            Evaluator.Lookup varResolver = new Evaluator.Lookup(varLookup);

            res = Evaluator.Evaluate(expr, varResolver);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void testNoClosingParenthesis()
        {
            // test not closing a parenthesis
            String expr = "(6 + 8";
            int res;
            Evaluator.Lookup varResolver = new Evaluator.Lookup(varLookup);

            res = Evaluator.Evaluate(expr, varResolver);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void testNoOpenParenthesis()
        {
            // test close parenthesis before open parenthesis
            String expr = "7 + 8)";
            int res;
            Evaluator.Lookup varResolver = new Evaluator.Lookup(varLookup);

            res = Evaluator.Evaluate(expr, varResolver);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void testOperatorBeforeOperand()
        {
            // test operator before operand
            String expr = "* 7";
            int res;
            Evaluator.Lookup varResolver = new Evaluator.Lookup(varLookup);

            res = Evaluator.Evaluate(expr, varResolver);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void testMissingOperand()
        {
            // test no operand after operator
            String expr = "7 *";
            int res;
            Evaluator.Lookup varResolver = new Evaluator.Lookup(varLookup);

            res = Evaluator.Evaluate(expr, varResolver);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void testTwoConsecutiveOperators()
        {
            // test two consecutive operators
            String expr = "2 + + 5";
            int res;
            Evaluator.Lookup varResolver = new Evaluator.Lookup(varLookup);

            res = Evaluator.Evaluate(expr, varResolver);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void testTwoConsecutveOperands()
        {
            // test two tokens consecutive operands
            String expr = "2 + 2 2";
            int res;
            Evaluator.Lookup varResolver = new Evaluator.Lookup(varLookup);

            res = Evaluator.Evaluate(expr, varResolver);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void testEmptyExpression()
        {
            // test two tokens consecutive operands
            String expr = "";
            int res;
            Evaluator.Lookup varResolver = new Evaluator.Lookup(varLookup);

            res = Evaluator.Evaluate(expr, varResolver);
        }

        [TestMethod()]
        // test cases that do not throw exceptions
        public void EvaluateTest()
        {
            string expr, expr2;
            int res, res2, exp_res;
            Evaluator.Lookup varResolver = new Evaluator.Lookup(varLookup);

            /// test order of operations
            // test that parenthesis evaluated over m/d
            expr = "(3 + 3) / 3";
            res = Evaluator.Evaluate(expr, varResolver);
            exp_res = 2;
            Assert.AreEqual(res, exp_res);
            // test that m/d evaluated over a/s
            expr = "2 + 3 * 4";
            res = Evaluator.Evaluate(expr, varResolver);
            exp_res = 14;
            Assert.AreEqual(res, exp_res);
            // test that m and d have same precedence (2*3 = 3*2)
            expr = "2 * 3";
            expr2 = "3 * 2";
            res = Evaluator.Evaluate(expr, varResolver);
            res2 = Evaluator.Evaluate(expr2, varResolver);
            Assert.AreEqual(res, res2);
            // test that a and s have same precedence (2+3 = 3+2)
            expr = "2 + 3";
            expr2 = "3 + 2";
            res = Evaluator.Evaluate(expr, varResolver);
            res2 = Evaluator.Evaluate(expr2, varResolver);
            Assert.AreEqual(res, res2);

            /// test operators
            // test addition
            expr = "2 + 3";
            res = Evaluator.Evaluate(expr, varResolver);
            exp_res = 5;
            Assert.AreEqual(res, exp_res);
            // test subtraction
            expr = "3 - 2";
            res = Evaluator.Evaluate(expr, varResolver);
            exp_res = 1;
            Assert.AreEqual(res, exp_res);
            // test multiplication
            expr = "5 * 5";
            res = Evaluator.Evaluate(expr, varResolver);
            exp_res = 25;
            Assert.AreEqual(res, exp_res);
            // test division
            expr = "9 / 3";
            res = Evaluator.Evaluate(expr, varResolver);
            exp_res = 3;
            Assert.AreEqual(res, exp_res);
            // test reversed operators
            expr = "2 -3";
            expr2 = "3 - 2";
            res = Evaluator.Evaluate(expr, varResolver);
            res2 = Evaluator.Evaluate(expr2, varResolver);
            Assert.AreNotEqual(res, res2);

            /// test variable lookup
            // test looking up a variable
            expr = "3 + A6";
            res = Evaluator.Evaluate(expr, varResolver);
            exp_res = 8;
            Assert.AreEqual(res, exp_res);

            /// test malformed expressions
            // test leading whitespace in expression
            expr = "  2 + 3";
            res = Evaluator.Evaluate(expr, varResolver);
            exp_res = 5;
            Assert.AreEqual(res, exp_res);
            // test trailing whitespace in expression
            expr = "2 + 3  ";
            res = Evaluator.Evaluate(expr, varResolver);
            exp_res = 5;
            Assert.AreEqual(res, exp_res);
            // test two tokens separated by more than one space
            expr = "3  + 3";
            res = Evaluator.Evaluate(expr, varResolver);
            exp_res = 6;
            Assert.AreEqual(res, exp_res);
            // test integer overflow
            expr = "2147483647 + 200";
            res = Evaluator.Evaluate(expr, varResolver);
            Assert.IsTrue(res < 0);
            // test an invalid formula followed by a valid formula (to test if stacks clear)
            expr = "3 **asdf 5";
            expr2 = "5 + 5";
            try
            {
                Evaluator.Evaluate(expr, varResolver);
            }
            catch (ArgumentException) {}
            res2 = Evaluator.Evaluate(expr2, varResolver);
            exp_res = 10;
            Assert.AreEqual(res2, exp_res);
            // test two tokens not separated by space
            expr = "2+ 2";
            res = Evaluator.Evaluate(expr, varResolver);
            exp_res = 4;
            Assert.AreEqual(res, exp_res);
            // test order of subtraction
            expr = "5 - 3";
            expr2 = "3 - 5";
            res = Evaluator.Evaluate(expr, varResolver);
            res2 = Evaluator.Evaluate(expr2, varResolver);
            Assert.AreNotEqual(res, res2);
            // test order of division
            expr = "9 / 3";
            expr2 = "3 / 9";
            res = Evaluator.Evaluate(expr, varResolver);
            res2 = Evaluator.Evaluate(expr2, varResolver);
            Assert.AreNotEqual(res, res2);
        }

        public int varLookup(String s)
        {
            if (s.Equals("A6"))
            {
                return 5;
            }   
            throw new ArgumentException();
        }
    }
}