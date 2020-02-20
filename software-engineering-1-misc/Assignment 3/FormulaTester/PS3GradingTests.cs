using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SpreadsheetUtilities;

namespace GradingTests
{
  [TestClass]
  public class GradingTests
  {

    // Normalizer tests
    [TestMethod()]
    public void TestNormalizerGetVars()
    {
      Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
      HashSet<string> vars = new HashSet<string>(f.GetVariables());

      Assert.IsTrue(vars.SetEquals(new HashSet<string> { "X1" }));
    }

    [TestMethod()]
    public void TestNormalizerEquals()
    {
      Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
      Formula f2 = new Formula("2+X1", s => s.ToUpper(), s => true);

      Assert.IsTrue(f.Equals(f2));
    }

    [TestMethod()]
    public void TestNormalizerToString()
    {
      Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
      Formula f2 = new Formula(f.ToString());

      Assert.IsTrue(f.Equals(f2));
    }

    // Validator tests
    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestValidatorFalse()
    {
      Formula f = new Formula("2+x1", s => s, s => false);
    }

    [TestMethod()]
    public void TestValidatorX1()
    {
      Formula f = new Formula("2+x", s => s, s => (s == "x"));
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestValidatorX2()
    {
      Formula f = new Formula("2+y1", s => s, s => (s == "x"));
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestValidatorX3()
    {
      Formula f = new Formula("2+x1", s => s, s => (s == "x"));
    }


    // Simple tests that return FormulaErrors
    [TestMethod()]
    public void TestUnknownVariable()
    {
      Formula f = new Formula("2+X1");
      Assert.IsInstanceOfType(f.Evaluate(s => { throw new ArgumentException("Unknown variable"); }), typeof(FormulaError));
    }

    [TestMethod()]
    public void TestDivideByZero()
    {
      Formula f = new Formula("5/0");
      Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
    }

    [TestMethod()]
    public void TestDivideByZeroVars()
    {
      Formula f = new Formula("(5 + X1) / (X1 - 3)");
      Assert.IsInstanceOfType(f.Evaluate(s => 3), typeof(FormulaError));
    }


    // Tests of syntax errors detected by the constructor
    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestSingleOperator()
    {
      Formula f = new Formula("+");
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestExtraOperator()
    {
      Formula f = new Formula("2+5+");
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestExtraCloseParen()
    {
      Formula f = new Formula("2+5*7)");
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestExtraOpenParen()
    {
      Formula f = new Formula("((3+5*7)");
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestNoOperator()
    {
      Formula f = new Formula("5x");
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestNoOperator2()
    {
      Formula f = new Formula("5+5x");
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestNoOperator3()
    {
      Formula f = new Formula("5+7+(5)8");
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestNoOperator4()
    {
      Formula f = new Formula("5 5");
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestDoubleOperator()
    {
      Formula f = new Formula("5 + + 3");
    }

    [TestMethod()]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestEmpty()
    {
      Formula f = new Formula("");
    }

    // Some more complicated formula evaluations
    [TestMethod()]
    public void TestComplex1()
    {
      Formula f = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");
      Assert.AreEqual(5.14285714285714, (double)f.Evaluate(s => (s == "x7") ? 1 : 4), 1e-9);
    }

    [TestMethod()]
    public void TestRightParens()
    {
      Formula f = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
      Assert.AreEqual(6, (double)f.Evaluate(s => 1), 1e-9);
    }

    [TestMethod()]
    public void TestLeftParens()
    {
      Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
      Assert.AreEqual(12, (double)f.Evaluate(s => 2), 1e-9);
    }

    [TestMethod()]
    public void TestRepeatedVar()
    {
      Formula f = new Formula("a4-a4*a4/a4");
      Assert.AreEqual(0, (double)f.Evaluate(s => 3), 1e-9);
    }

    // Test of the Equals method
    [TestMethod()]
    public void TestEqualsBasic()
    {
      Formula f1 = new Formula("X1+X2");
      Formula f2 = new Formula("X1+X2");
      Assert.IsTrue(f1.Equals(f2));
    }

    [TestMethod()]
    public void TestEqualsWhitespace()
    {
      Formula f1 = new Formula("X1+X2");
      Formula f2 = new Formula(" X1  +  X2   ");
      Assert.IsTrue(f1.Equals(f2));
    }

    [TestMethod()]
    public void TestEqualsDouble()
    {
      Formula f1 = new Formula("2+X1*3.00");
      Formula f2 = new Formula("2.00+X1*3.0");
      Assert.IsTrue(f1.Equals(f2));
    }

    [TestMethod()]
    public void TestEqualsComplex()
    {
      Formula f1 = new Formula("1e-2 + X5 + 17.00 * 19 ");
      Formula f2 = new Formula("   0.0100  +     X5+ 17 * 19.00000 ");
      Assert.IsTrue(f1.Equals(f2));
    }


    [TestMethod()]
    public void TestEqualsNullAndString()
    {
      Formula f = new Formula("2");
      Assert.IsFalse(f.Equals(null));
      Assert.IsFalse(f.Equals(""));
    }


    // Tests of == operator
    [TestMethod()]
    public void TestEq()
    {
      Formula f1 = new Formula("2");
      Formula f2 = new Formula("2");
      Assert.IsTrue(f1 == f2);
    }

    [TestMethod()]
    public void TestEqFalse()
    {
      Formula f1 = new Formula("2");
      Formula f2 = new Formula("5");
      Assert.IsFalse(f1 == f2);
    }

    [TestMethod()]
    public void TestEqNull()
    {
      Formula f1 = new Formula("2");
      Formula f2 = new Formula("2");
      Assert.IsFalse(null == f1);
      Assert.IsFalse(f1 == null);
      Assert.IsTrue(f1 == f2);
    }


    // Tests of != operator
    [TestMethod()]
    public void TestNotEq()
    {
      Formula f1 = new Formula("2");
      Formula f2 = new Formula("2");
      Assert.IsFalse(f1 != f2);
    }

    [TestMethod()]
    public void TestNotEqTrue()
    {
      Formula f1 = new Formula("2");
      Formula f2 = new Formula("5");
      Assert.IsTrue(f1 != f2);
    }


    // Test of ToString method
    [TestMethod()]
    public void TestString()
    {
      Formula f = new Formula("2*5");
      Assert.IsTrue(f.Equals(new Formula(f.ToString())));
    }


    // Tests of GetHashCode method
    [TestMethod()]
    public void TestHashCode()
    {
      Formula f1 = new Formula("2*5");
      Formula f2 = new Formula("2*5");
      Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
    }

    // Technically the hashcodes could not be equal and still be valid,
    // extremely unlikely though. Check their implementation if this fails.
    [TestMethod()]
    public void TestHashCodeFalse()
    {
      Formula f1 = new Formula("2*5");
      Formula f2 = new Formula("3/8*2+(7)");
      Assert.IsTrue(f1.GetHashCode() != f2.GetHashCode());
    }

    [TestMethod()]
    public void TestHashCodeComplex()
    {
      Formula f1 = new Formula("2 * 5 + 4.00 - _x");
      Formula f2 = new Formula("2*5+4-_x");
      Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
    }


    // Tests of GetVariables method
    [TestMethod()]
    public void TestVarsNone()
    {
      Formula f = new Formula("2*5");
      Assert.IsFalse(f.GetVariables().GetEnumerator().MoveNext());
    }

    [TestMethod()]
    public void TestVarsSimple()
    {
      Formula f = new Formula("2*X2");
      List<string> actual = new List<string>(f.GetVariables());
      HashSet<string> expected = new HashSet<string>() { "X2" };
      Assert.AreEqual(actual.Count, 1);
      Assert.IsTrue(expected.SetEquals(actual));
    }

    [TestMethod()]
    public void TestVarsTwo()
    {
      Formula f = new Formula("2*X2+Y3");
      List<string> actual = new List<string>(f.GetVariables());
      HashSet<string> expected = new HashSet<string>() { "Y3", "X2" };
      Assert.AreEqual(actual.Count, 2);
      Assert.IsTrue(expected.SetEquals(actual));
    }

    [TestMethod()]
    public void TestVarsDuplicate()
    {
      Formula f = new Formula("2*X2+X2");
      List<string> actual = new List<string>(f.GetVariables());
      HashSet<string> expected = new HashSet<string>() { "X2" };
      Assert.AreEqual(actual.Count, 1);
      Assert.IsTrue(expected.SetEquals(actual));
    }

    [TestMethod()]
    public void TestVarsComplex()
    {
      Formula f = new Formula("X1+Y2*X3*Y2+Z7+X1/Z8");
      List<string> actual = new List<string>(f.GetVariables());
      HashSet<string> expected = new HashSet<string>() { "X1", "Y2", "X3", "Z7", "Z8" };
      Assert.AreEqual(actual.Count, 5);
      Assert.IsTrue(expected.SetEquals(actual));
    }

    // Tests to make sure there can be more than one formula at a time
    [TestMethod()]
    public void TestMultipleFormulae()
    {
      Formula f1 = new Formula("2");
      Formula f2 = new Formula("3");
      Assert.IsTrue(f1.ToString().IndexOf("2") >= 0);
      Assert.IsTrue(f2.ToString().IndexOf("3") >= 0);
    }

    // Repeat this test to increase its weight
    [TestMethod()]
    public void TestMultipleFormulaeB()
    {
      Formula f1 = new Formula("2");
      Formula f2 = new Formula("3");
      Assert.IsTrue(f1.ToString().IndexOf("2") >= 0);
      Assert.IsTrue(f2.ToString().IndexOf("3") >= 0);
    }

    [TestMethod()]
    public void TestMultipleFormulaeC()
    {
      Formula f1 = new Formula("2");
      Formula f2 = new Formula("3");
      Assert.IsTrue(f1.ToString().IndexOf("2") >= 0);
      Assert.IsTrue(f2.ToString().IndexOf("3") >= 0);
    }

    [TestMethod()]
    public void TestMultipleFormulaeD()
    {
      Formula f1 = new Formula("2");
      Formula f2 = new Formula("3");
      Assert.IsTrue(f1.ToString().IndexOf("2") >= 0);
      Assert.IsTrue(f2.ToString().IndexOf("3") >= 0);
    }

    [TestMethod()]
    public void TestMultipleFormulaeE()
    {
      Formula f1 = new Formula("2");
      Formula f2 = new Formula("3");
      Assert.IsTrue(f1.ToString().IndexOf("2") >= 0);
      Assert.IsTrue(f2.ToString().IndexOf("3") >= 0);
    }

    // Stress test for constructor
    [TestMethod()]
    public void TestConstructor()
    {
      Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
    }

    // This test is repeated to increase its weight
    [TestMethod()]
    public void TestConstructorB()
    {
      Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
    }

    [TestMethod()]
    public void TestConstructorC()
    {
      Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
    }

    [TestMethod()]
    public void TestConstructorD()
    {
      Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
    }

    // Stress test for constructor
    [TestMethod()]
    public void TestConstructorE()
    {
      Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
    }


  }
}
