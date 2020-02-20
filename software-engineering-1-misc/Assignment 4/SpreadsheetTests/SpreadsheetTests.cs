using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using SpreadsheetUtilities;
using System.Collections.Generic;

namespace SpreadsheetTests
{
    /// <summary>
    /// === Test Map ===
    /// Creating a new formula with variables when:
    ///   zero-argument constructor is used
    ///     (should accept all (first-degree) valid vars, should not modify var names, should have "default" version)
    ///   three-argument constructor is used with identity validator/normalizer and nondefault version
    ///     (should accept all (first-degree) valid vars, should not modify var names, should have nondefault version)
    ///   three-argument constructor is used with some validator/normalizer
    ///     (should restrict valid vars and modify var names according to parameters)
    ///   three-argument constructor is used with inverse identity function validator (x => false)
    ///     (should reject all var names)
    ///
    /// A new spreadsheet is created when:
    ///   the four-argument constructor is used (exceptional cases):
    ///     four-argument constructor is used when xmlFilePath not valid
    ///     four-arguemnt constructor is used when xml file version does not match parameter 
    ///     four-argument constructor is used when xml file contains invalid cell name 
    ///     four-argument constructor is used when xml file contains a circular dependency 
    ///     four-argument constructor is used when xml file contains an invalid formula 
    ///   the four-argument constructor is used (normal cases):
    ///     four-argument constructor is used when xml file contains a valid, nonempty (no dependent cells) spreadsheet 
    ///     four-argument constructor is used when xml file contains an empty spreadsheet 
    ///     four-argument constructor is used when xml file contains a spreadsheet with dependencies:
    ///       modification to dependee cells cause change in value to the dependent cells
    ///
    /// The saved version of a spreadsheet is retrieved when:
    ///   the spreadsheet is new, unedited, and has default version 
    ///   the spreadsheet is new, unedited, and has nondefault version 
    ///   the spreadsheet is new and has been edited 
    ///   the spreadsheet is restored and has not been edited 
    ///   the spreadsheet is restored and has been edited 
    ///
    /// The spreadsheet is saved when:
    ///  the spreadsheet is empty 
    ///  the spreadsheet is nonempty and contains no dependencies 
    ///    (test restoring the speradsheet by creating a new one and comparing the cell values)
    ///  the spreadsheet is nonempty and contains dependencies 
    ///    (test restoring the speradsheet by creating a new one and comparing the cell values and testing the dependencies)
    ///
    /// The cell value is retrieved when:
    ///   the cell is emtpy 
    ///   the cell contains a string 
    ///   the cell contains a decimal 
    ///   the cell contains an integer 
    ///   the cell contains a formula without variables 
    ///   the cell contains a formula with variables 
    ///   the cell contains a FormulaError 
    ///
    /// Getting contents of cell with cell value:
    ///   complex formula
    ///   complex formula including variables
    ///   formula of constant integer
    ///   formula of constant decimal 
    ///   formula of single variable
    ///   formula with division by zero (FormulaError)
    ///   number (decimal)
    ///   number (integer) 
    ///   empty cell
    ///   string
    /// 
    /// Getting contents of cell with call parameters:
    ///   null parameter
    ///   invalid cell name of one illegal character
    ///   invalid cell name of one legal character (letter)
    ///   invalid cell name with first illegal character, last couple legal
    ///   invalid cell name with first legal character, last illegal
    ///   invalid cell name with first couple legal, last one illegal
    /// 
    /// Setting contents of cell performs as expected when:
    ///   parameters are:
    ///     invalid cell name, valid contents
    ///     valid name, null contents
    ///     null name and null contents 
    ///   new cell value is:
    ///     going to result in circular dependency
    ///     going to result in FormulaError 
    ///   cell is:
    ///     independent (no dependees or dependents)
    ///     has only direct dependents (no dependees, only first-degree dependents)
    ///     has many indirect dependents (no dependees, many n-th degree dependents)
    ///
    /// Getting nonempty cells when:
    ///   all cells are empty
    ///   one cell is nonempty
    ///   many cells are nonempty, none are dependent, all content types
    ///   many cells are nonempty, many are dependent
    /// </summary>
    [TestClass]
    public class SpreadsheetTests
    {
        // === Normalization, Validation, and Version Tests ===

        /// <summary>
        /// Test that a spreadsheet created with the zero-argument constructor performs as expected with formula variabes.
        /// 
        /// Test that it accepts all (valid cell name) vars
        /// Test that it does not modify any variables
        /// Test that version is 'default'
        /// </summary>
        [TestMethod]
        public void zeroArgumentConstructorFormulaVariablesTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet();

            // set A1 to a formula
            spreadsheet.SetContentsOfCell("A1", "=(A2 + ABBC33443)"); // (validator is tested here)

            // test that the variables didn't change (noramlizer)
            Formula expRes = new Formula("(A2 + ABBC33443)");
            Formula a1Formula = spreadsheet.GetCellContents("A1") as Formula;
            Assert.AreEqual(expRes, a1Formula);

            // test that the version is default
            Assert.AreEqual(spreadsheet.Version, "default");
        }

        /// <summary>
        /// Test that a spreadsheet created with the three-arugment constructor (with identity validator and normalziers)
        /// performs as epxected with formula variables
        /// 
        /// Test that it accepts all (valid cell name) vars
        /// Test that it does not modify any variable names
        /// Test that version is whatever was parameterized
        /// </summary>
        [TestMethod]
        public void threeArgumentConstructorIdentntiyFormulaVariablesTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet(x => true, x => x, "thisIsTheVersion");

            // set A1 to a formula
            spreadsheet.SetContentsOfCell("A1", "=(A2 + ABBC33443)"); // (validator is tested here)

            // test that the variables didn't change (noramlizer)
            Formula expRes = new Formula("(A2 + ABBC33443)");
            Formula a1Formula = spreadsheet.GetCellContents("A1") as Formula;
            Assert.AreEqual(expRes, a1Formula);

            // test that the version is correct
            Assert.AreEqual(spreadsheet.Version, "thisIsTheVersion");
        }

        /// <summary>
        /// Test that a spreadsheet created with the three-argument constructor (with restrictive validator
        /// and modifying normalizer) performs as expected with formula variables
        /// 
        /// Test that it rejects unfit varaibles as parameterized
        /// Test that it modifies variables as parameterized
        /// </summary>
        [TestMethod]
        public void threeArgumentConstructorFormulaVariablesTest()
        {
            // function valid iff starts with 'A'
            Func<String, Boolean> validator = x =>
            {
                return (x.ToLower().StartsWith("a"));
            };
            // convert all var names to lower case
            Func<String, String> normalizer = x =>
            {
                return (x.ToLower());
            };
            Spreadsheet spreadsheet = new Spreadsheet(validator, normalizer, "thisIsTheVersion");

            // set A1 to a formula
            spreadsheet.SetContentsOfCell("A1", "=(A2 + AABBC33443)"); // (validator is tested here)
            // set A1 to an invalid formula
            // will throw FormulaFormatException since invalid variable
            try
            {
                spreadsheet.SetContentsOfCell("A2", "=(B2 + XABBC33443)"); // (validator is tested here)
            }
            catch (FormulaFormatException) { }
            
            // test that the variables were changed by the normalizer
            Formula expRes = new Formula("(a2 + aabbc33443)");
            Formula a1Formula = spreadsheet.GetCellContents("A1") as Formula;
            Assert.AreEqual(expRes, a1Formula);

            // test that the version is correct
            Assert.AreEqual(spreadsheet.Version, "thisIsTheVersion");
        }

        /// <summary>
        /// Test that a spreadsheet created with the three-argument constructor (with inverse identity
        /// function for validator, returns false for all strings) performs as expected.
        /// 
        /// Test that it rejects all variables
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void threeArgumentConstructorInverseFormulaVariablesTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet(x => false, x => x, "thisIsTheVersion");

            // set A1 to a formula
            spreadsheet.SetContentsOfCell("A1", "=(A2 + ABBC33443)"); // (validator is tested here)
        }

        // === Constructor Tests ===

        /// <summary>
        /// Test that a spreadsheet created with the four-argument consructor when the xml
        /// path is invalid throws the appropriate excpeption
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void fourArgConstructorInvalidPath()
        {
            Spreadsheet spreadsheet = new Spreadsheet("this\\is\\an\\invalid\\path", x => true, x => x, "version");
        }

        /// <summary>
        /// Test that a spreadsheet created when the four-argument constrcutor is used
        /// when the xml file has a version mismatch throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void fourArgConstructorVersionMismatch()
        {
            Spreadsheet spreadsheet = new Spreadsheet("..\\..\\testXmlFiles\\versionMismatchSpreadsheet.xml", x => true, x => x, "2");
        }

        /// <summary>
        /// Test creating a spreadsheet with xml file containing an invalid cell name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void fourArgConstrcutorInvalidCellName()
        {
            Spreadsheet spreadsheet = new Spreadsheet("..\\..\\testXmlFiles\\invalidCellNameSpreadsheet.xml", x => true, x => x, "2");
        }

        /// <summary>
        /// Test creating a spreadsheet with xml file containing circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void fourArgConstructorCircularDependency()
        {
            // TODO create file with circular dependency and update path
            Spreadsheet spreadsheet = new Spreadsheet("..\\..\\testXmlFiles\\circularDependencySpreadsheet.xml", x => true, x => x, "2");
        }

        /// <summary>
        /// Test creating a spreadsheet with xml file containing an invalid formula
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void fourArgConstructorInvalidFormula()
        {
            // TODO create file with invalid formula and update path
            Spreadsheet spreadsheet = new Spreadsheet("..\\..\\testXmlFiles\\invalidFormulaSpreadsheet.xml", x => true, x => x, "2");
        }

        /// <summary>
        /// Test creating a spreadsheet from file contiang a valid, nonempty spreadsheet with no dependencies
        /// </summary>
        [TestMethod]
        public void fourArgConstructorValidNonemptyXmlFile()
        {
            // TODO create file with valid, nonempty spreadsheet with no dependencies and update path
            Spreadsheet spreadsheet = new Spreadsheet("..\\..\\testXmlFiles\\noDependenciesSpreadsheet.xml", x => true, x => x, "default");

            // TODO test getnamesofnonemptycells
        }

        /// <summary>
        /// Test creating a spreadsheet with xml file containing a valid, empty spreadsheet
        /// </summary>
        [TestMethod]
        public void fourArgConstructorValidEmptyXmlFile()
        {
            Spreadsheet spreadsheet = new Spreadsheet("..\\..\\testXmlFiles\\emptySpreadsheet.xml", x => true, x => x, "default");

            // TODO test getnamesofnonempty cells
        }

        /// <summary>
        /// Test creating a spreadsheet with xml file containing a valid spreadsheet with dependencies
        /// </summary>
        [TestMethod]
        public void fourArgConstructorValidDependenciesXmlFile()
        {
            // TODO create file with valid, nonempty spreadsheet with dependencies and update path
            Spreadsheet spreadsheet = new Spreadsheet("..\\..\\testXmlFiles\\spreadsheetWithDependencies.xml", x => true, x => x, "default");

            // TODO test getnamesofnonempty cells

            // TODO test modifying a dependee cell and assert the new values of the dependent cells
        }

        // === Test Changed property Of a Spreadsheet ===

        /// <summary>
        /// Test Changed property with spreadsheet thats new and unedited
        /// </summary>
        [TestMethod]
        public void getChangedPropertyNewUneditedTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            Assert.IsFalse(spreadsheet.Changed);
        }

        /// <summary>
        /// Test Changed property with spreadsheet thats new and unedited with nondefault version
        /// </summary>
        [TestMethod]
        public void getChangedPropertyNewUneditedNondefaultVersionTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet(x => true, x => x, "nondefault_version");
            Assert.IsFalse(spreadsheet.Changed);
        }

        /// <summary>
        /// Test Changed property with spreadsheet thats new and edited.
        /// </summary>
        [TestMethod]
        public void getChangedPropertyNewEditedTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "3");
            Assert.IsTrue(spreadsheet.Changed);
        }

        /// <summary>
        /// Test Changed property with spreadsheet thats restored and unedited
        /// </summary>
        [TestMethod]
        public void getChangedPropertyRestoredUneditedTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet("..\\..\\testXmlFiles\\testFile.xml", x => true, x => x, "default");
            Assert.IsFalse(spreadsheet.Changed);
        }

        /// <summary>
        /// Test Changed property with spreadsheet thats restored and edited
        /// </summary>
        [TestMethod]
        public void getChangedPropertyRestoredEditedTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet("..\\..\\testXmlFiles\\testFile.xml", x => true, x => x, "default");
            spreadsheet.SetContentsOfCell("A1", "3");
            Assert.IsTrue(spreadsheet.Changed);
        }

        // === Test saving a spreadsheet ===

        /// <summary>
        /// Test saving an empty spreadsheet
        /// </summary>
        [TestMethod]
        public void saveEmptyTest()
        {
            // TODO make place to save spreadsheets for tests and update path
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.Save("..\\..\\testXmlFiles\\emptySpreadsheet.xml");
        }

        /// <summary>
        /// Test saving a nonempty spreadsheet with no dependnecies
        /// </summary>
        [TestMethod]
        public void saveNonemptyNoDependenciesTest()
        {
            // TODO make place to save spreadsheets for tests and update path
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "3");
            spreadsheet.SetContentsOfCell("B1", "2");
            spreadsheet.SetContentsOfCell("C1", "1");
            spreadsheet.Save("..\\..\\testXmlFiles\\savedNonemptyNoDependenciesSpreadsheet.xml");
        }

        /// <summary>
        /// Test saving a nonempty spreadsheet with dependencies
        /// </summary>
        [TestMethod]
        public void saveNonemptyDependenciesTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "3");
            spreadsheet.SetContentsOfCell("B1", "=A1 + 2");
            spreadsheet.SetContentsOfCell("C1", "=B2 + 3");
            spreadsheet.Save("..\\..\\testXmlFiles\\savedDependenciesSpreadsheet.xml");
        }

        // === Test retrieving a cells value ===

        /// <summary>
        /// Test retrieving the value of an empty cell
        /// </summary>
        [TestMethod]
        public void retrieveValueEmptyCellTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            object value = spreadsheet.GetCellValue("A1");
            object expValue = "";
            Assert.AreEqual(value, expValue);
        }

        /// <summary>
        /// Test retrieving the value of a cell with a string
        /// </summary>
        [TestMethod]
        public void retrieveValueStringTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "thisisastring");

            String value = spreadsheet.GetCellValue("A1") as String;
            String expValue = "thisisastring";
            Assert.AreEqual(value, expValue);
        }

        /// <summary>
        /// Test retrieving the value of a cell with a decimal
        /// </summary>
        [TestMethod]
        public void retrieveValueDecimalTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "3.3");

            Double value = Convert.ToDouble(spreadsheet.GetCellValue("A1"));
            Double expValue = 3.3;
            Assert.AreEqual(value, expValue);
        }

        /// <summary>
        /// Test retrieving the value of a cell with an integer
        /// </summary>
        [TestMethod]
        public void retrieveValueIntegerTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "3");

            Double value = Convert.ToDouble(spreadsheet.GetCellValue("A1"));
            Double expValue = 3;
            Assert.AreEqual(value, expValue);
        }

        /// <summary>
        /// Test retrieving the value of a cell with a formula with no variables
        /// </summary>
        [TestMethod]
        public void retrieveValueFormulaNoVarsTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "=3 + (4 * 12)");

            Double value = Convert.ToDouble(spreadsheet.GetCellValue("A1"));
            Double expValue = 51;
            Assert.AreEqual(value, expValue);
        }

        /// <summary>
        /// Test retrieving the value of a cell with variables
        /// </summary>
        [TestMethod]
        public void retrieveValueFormulaWithVarsTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A2", "4");
            spreadsheet.SetContentsOfCell("A1", "=3 + (A2 * 12)");

            Double value = Convert.ToDouble(spreadsheet.GetCellValue("A1"));
            Double expValue = 51;
            Assert.AreEqual(value, expValue);
        }

        /// <summary>
        /// Test retrieving the value of a cell with a formulaerror
        /// </summary>
        [TestMethod]
        public void retrieveValueFormulaErrorTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "=3 / 0");
            Func<String, Double> lookupFunctor = x =>
            {
                throw new ArgumentException("Not found");
            };

            Formula equalFormulaA1 = new Formula("3 / 0");
            object formEval = new FormulaError("Attempted to divide by zero.");

            object A1val = spreadsheet.GetCellContents("A1");
            // try to cast them to formulas, if it does not work then both vars will be null and test will fail later
            Formula A1Formula = A1val as Formula;

            // test evaluation of formula
            object A1Eval = A1Formula.Evaluate(lookupFunctor);
            Assert.AreEqual(A1Eval, formEval);

            // test that formulas are equal to separately constructed formulas
            Assert.AreEqual(A1Formula, equalFormulaA1);
        }

        // === Content modification tests ===

        /// <summary>
        /// Test that GetCellContents("A1") where A1 is defined below returns the correct formula object.
        /// Test that GetCellContents("A1") where A2 is defined below returns the correct formula object.
        /// 
        /// These two test are meant to evaluate the retrieval of cell contents with complex formulas with and without variables.
        /// 
        /// <ppara>A1 = "=6 + 8 * (7 + 3) / 4 + 12"</para>
        /// <para>A2 = "=6 + A6 * (7 + B2) / 4 + 12" where A6 is 8 and B2 is 3</para>
        /// <para>
        /// "Correctness" is determined by two things: the returned formula must be equal to a
        /// separately constructed formula object, and it must evaluate to the correct value.
        /// </para>
        /// </summary>
        [TestMethod]
        public void ComplexFormulaContentRetrievalTest()
        {
            // construct spreadsheet and lookup function
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "=6 + 8 * (7 + 3) / 4 + 12");
            spreadsheet.SetContentsOfCell("A2", "=6 + A6 * (7 + B2) / 4 + 12");
            Func<String, Double> lookupFunctor = x =>
            {
                if (x.Equals("A6"))
                {
                    return 8;
                }
                else if (x.Equals("B2"))
                {
                    return 3;
                }
                throw new ArgumentException("Not found");
            };

            Formula equalFormulaA1 = new Formula("6 + 8 * (7 + 3) / 4 + 12");
            Formula equalFormulaA2 = new Formula("6 + A6 * (7 + B2) / 4 + 12");
            Double formEval = 38;

            object A1val = spreadsheet.GetCellContents("A1");
            object A2val = spreadsheet.GetCellContents("A2");
            // try to cast them to formulas, if it does not work then both vars will be null and test will fail later
            Formula A1Formula = A1val as Formula;
            Formula A2Formula = A2val as Formula;

            // test evaluation of formulas
            Double A1Eval = Convert.ToDouble(A1Formula.Evaluate(lookupFunctor));
            Double A2Eval = Convert.ToDouble(A2Formula.Evaluate(lookupFunctor));
            Assert.AreEqual(A1Eval, formEval);
            Assert.AreEqual(A2Eval, formEval);

            // test that formulas are equal to separately constructed formulas
            Assert.AreEqual(A1Formula, equalFormulaA1);
            Assert.AreEqual(A2Formula, equalFormulaA2);
        }

        /// <summary>
        /// Test that GetCellContents("A1") where A1 is defined below returns the correct formula.
        /// Test that GetCellContents("A2") where A2 is defined below returns the correct formula.
        /// Test that GetCellContents("A3") where A3 is defined below returns the correct formula.
        /// 
        /// This evaluates the retrieval of formulas with only one value for both variables and constants.
        /// 
        /// <para>A1 = "=3"</para>
        /// <para>A2 = "=A3" where A3 is 3</para>
        /// <para>A3 = "=3.3"</para>
        /// <para>
        /// "Correctness" is determined by two things: the returned formula must be equal to a
        /// separately constructed formula object, and it must evaluate to the correct value.
        /// </para>
        /// </summary>
        [TestMethod]
        public void SingleValueFormulaContentRetrievalTest()
        {            
            // construct spreadsheet and lookup function
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "=3");
            spreadsheet.SetContentsOfCell("A2", "=A3");
            spreadsheet.SetContentsOfCell("A3", "=3.3");
            Func<String, Double> lookupFunctor = x =>
            {
                if (x.Equals("A3"))
                {
                    return 3;
                }
                throw new ArgumentException("Not found");
            };

            Formula equalFormulaA1 = new Formula("3");
            Formula equalFormulaA2 = new Formula("A3");
            Formula equalFormulaA3 = new Formula("3.3");
            Double formEval = 3;
            Double formEvalA3 = 3.3;

            object A1val = spreadsheet.GetCellContents("A1");
            object A2val = spreadsheet.GetCellContents("A2");
            object A3val = spreadsheet.GetCellContents("A3");
            // try to cast them to formulas, if it does not work then both vars will be null and test will fail later
            Formula A1Formula = A1val as Formula;
            Formula A2Formula = A2val as Formula;
            Formula A3Formula = A3val as Formula;

            // test evaluation of formulas
            Assert.AreEqual(A1Formula.Evaluate(lookupFunctor), formEval);
            Assert.AreEqual(A2Formula.Evaluate(lookupFunctor), formEval);
            Assert.AreEqual(A3Formula.Evaluate(lookupFunctor), formEvalA3);

            // test that formulas are equal to separately constructed formulas
            Assert.AreEqual(A1Formula, equalFormulaA1);
            Assert.AreEqual(A2Formula, equalFormulaA2);
            Assert.AreEqual(A3Formula, equalFormulaA3);
        }

        /// <summary>
        /// Test that GetCellContents("A1") returns 3.3.
        /// Test that GetCellContents("A2") returns 3.
        /// 
        /// Tests retrieving the value of a cell containing a double.
        /// <remarks>A1 is 3.3</remarks>
        /// <remarks>A2 is 3</remarks>
        /// </summary>
        [TestMethod]
        public void DoubleContentRetrievalTest()
        {
            // construct spreadsheet and lookup function
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "3.3");
            spreadsheet.SetContentsOfCell("A2", "3");
            Func<String, Double> lookupFunctor = x =>
            {
                throw new ArgumentException("Not found");
            };

            Double equalDoubleA1 = 3.3;
            Double equalDoubleA2 = 3;

            object A1val = spreadsheet.GetCellContents("A1");
            object A2val = spreadsheet.GetCellContents("A2");
            // try to cast to double, if it does not work then exception will be thrown
            Double A1Double = Convert.ToDouble(A1val);
            Double A2Double = Convert.ToDouble(A2val);

            // test value
            Assert.AreEqual(A1Double, 3.3);
            Assert.AreEqual(A2Double, 3);

            // test equality with separate double
            Assert.AreEqual(A1val, equalDoubleA1);
            Assert.AreEqual(A2val, equalDoubleA2);
        }

        /// <summary>
        /// Test that GetCellContents("A1") returns an empty string.
        /// 
        /// Tests retrieving the value of an empty cell.
        /// 
        /// <remarks>A1 is empty</remarks>
        /// </summary>
        [TestMethod]
        public void EmptyCellContentRetrievalTest()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            // get contents and attempt to cast to string, if not string then test will fail
            object A1Contents = spreadsheet.GetCellContents("A1");
            String A1String = A1Contents as String;

            Assert.AreEqual(A1String, "");
        }

        /// <summary>
        /// Test that GetCellContents("A1") returns "A string".
        /// 
        /// Tests retrieving the value of a cell containing a string.
        /// 
        /// <remarks>A1 is "A string"</remarks>
        /// </summary>
        [TestMethod]
        public void StringContentRetrievalTest()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "A string");

            object A1Contents = spreadsheet.GetCellContents("A1");
            String A1String = A1Contents as String;

            Assert.AreEqual(A1Contents, "A string");
        }

        /// <summary>
        /// Test that GetCellContents("A1") where A1 is defined below returns the correct formula.
        /// 
        /// This evaluates the retrieval of formulas with FormulaError evaluations
        /// 
        /// <para>A1 = "=3 / 0"</para>
        /// <para>
        /// "Correctness" is determined by two things: the returned formula must be equal to a
        /// separately constructed formula object, and it must evaluate to the correct value.
        /// </para>
        [TestMethod]
        public void FormulaErrorContentRetrievalTest()
        {
            // construct spreadsheet and lookup function
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "=3 / 0");
            Func<String, Double> lookupFunctor = x =>
            {
                throw new ArgumentException("Not found");
            };

            Formula equalFormulaA1 = new Formula("3 / 0");
            object formEval = new FormulaError("Attempted to divide by zero.");

            object A1val = spreadsheet.GetCellContents("A1");
            // try to cast them to formulas, if it does not work then both vars will be null and test will fail later
            Formula A1Formula = A1val as Formula;

            // test evaluation of formula
            object A1Eval = A1Formula.Evaluate(lookupFunctor);
            Assert.AreEqual(A1Eval, formEval);

            // test that formulas are equal to separately constructed formulas
            Assert.AreEqual(A1Formula, equalFormulaA1);
        }

        /// <summary>
        /// Test that GetCellContents(null) throws InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void NullCellContentRetrievalTest()
        {
            // construct spreadsheet
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            spreadsheet.GetCellContents(null);
        }

        /// <summary>
        /// Test that GetCellContents("%") throws InvalidNameException.
        /// 
        /// Tests that the method throws an exception when a (non-edge case) invalid cell name is passed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidCellNameContentRetrievalTest()
        {
            // construct spreadsheet
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            spreadsheet.GetCellContents("%");
        }

        /// <summary>
        /// Test that GetCellContents("A") throws InvalidNameException.
        /// 
        /// Tests that the method throws an exception when an (edge case) invalid cell name is passed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidCellNameSingleCharContentRetrievalTest()
        {
            // construct spreadsheet
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            spreadsheet.GetCellContents("%");
        }

        /// <summary>
        /// Test that GetCellContents("*952") throws InvalidNameException.
        /// 
        /// Tests that the method throws an exception when a (edge case) invalid cell name is passed.
        /// The invalid cell name has only the first character invalid and the rest valid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidFirstCharCellNameContentRetrievalTest()
        {
            // construct spreadsheet
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            spreadsheet.GetCellContents("*952");
        }

        /// <summary>
        /// Test that GetCellContents("A&3_B") throws InvalidNameException.
        /// 
        /// Tests that the method throws an exception when an invalid cell name is passed.
        /// The invalid cell name has only one character in the middle of the name invalid while the rest of the chars are valid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidMidCharCellNameContentRetrievalTest()
        {
            // construct spreadsheet
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            spreadsheet.GetCellContents("A&3_B");
        }

        /// <summary>
        /// Test that GetCellContents("A77!") throws InvalidNameException.
        /// 
        /// Tests that the method throws an exception when an invalid cell name is passed.
        /// The invalid cell name has only the last character of the name invalid while the rest of the chars are valid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidLastCharCellNameContentRetrievalTest()
        {
            // construct spreadsheet
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            spreadsheet.GetCellContents("A77!");
        }

        /// <summary>
        /// Test passing an invalid variable name to the SetContentsOfCell Method.
        /// </summary>
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void InvalidNameSetContentsOfCellStringTest()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("^^^", "contents");
        }

        /// <summary>
        /// Test passing a null contents parameter to the SetContentsOfCell method will throw ArgumentNullException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetValueNullTextTest()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", null);
        }

        /// <summary>
        /// Test passing null contents and name parameters to the SetContentsOfCell method will throw ArgumentNullException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetValueNullBothTest()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell(null, null);
        }

        /// <summary>
        /// Test that the set returned when setting the value of an independent cell is just that cell.
        /// Also test that the cell is changed.
        /// </summary>
        [TestMethod]
        public void SetValueIndependentCellTest()
        {
            // create new spreadsheet and add the independent cell
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "3");
            Func<String, Double> lookup = x =>
            {
                throw new ArgumentException("not found");
            };

            // set the value of a1 and save the set of affected cells it returns
            HashSet<String> affectedCells = spreadsheet.SetContentsOfCell("A1", "4") as HashSet<String>;

            // test that the set of affected cells is only a1
            // construct a separate instance of the expected set to compare with
            HashSet<String> expectedSet = new HashSet<string>();
            expectedSet.Add("A1");
            Assert.IsTrue(affectedCells.SetEquals(expectedSet));

            // test that A1 was changed
            object A1val = spreadsheet.GetCellContents("A1");
            Double A1double = Convert.ToDouble(A1val);
            Assert.IsTrue(A1double == 4);
        }

        /// <summary>
        /// Test that the set returned when setting the value of a cell with only direct dependents returns the correct set
        /// Also test that the cell and its dependents are changed
        /// </summary>
        [TestMethod]
        public void SetValueDirectDependentsCellTest()
        {
            // constsruct the spreadsheet and setup the dependency structure
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "5.5");
            spreadsheet.SetContentsOfCell("B1", "=A1 + 3");
            spreadsheet.SetContentsOfCell("C1", "=A1 + 4");
            spreadsheet.SetContentsOfCell("D1", "=A1 + 5");

            // dependency graph:
            // a --> b
            // +---> c
            // +---> d
            // where:
            //  a: A1
            //  b: B1
            //  c: C1
            //  d: D1

            // change A1 and save the set of affected elements it returns
            HashSet<String> affectedCells = spreadsheet.SetContentsOfCell("A1", "6.6") as HashSet<String>;

            // test that the set contains only A1 and all its dependents
            HashSet<String> expectedCells = new HashSet<string>();
            expectedCells.Add("A1");
            expectedCells.Add("B1");
            expectedCells.Add("C1");
            expectedCells.Add("D1");
            Assert.IsTrue(affectedCells.SetEquals(expectedCells));

            // test that all cells are the correct values
            Assert.AreEqual(6.6, Convert.ToDouble(spreadsheet.GetCellContents("A1")));
        }

        /// <summary>
        /// Test that the set returned when setting the value of a cell with many indirect dependencies returns the correct set.
        /// Also test that the cell and its dependents are changed.
        /// </summary>
        [TestMethod]
        public void SetValueComplexDependentsTest()
        {
            // constsruct the spreadsheet and setup the dependency structure
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "5.5");
            spreadsheet.SetContentsOfCell("B1", "=A1 + 3");
            spreadsheet.SetContentsOfCell("C1", "=A1 + 4");
            spreadsheet.SetContentsOfCell("D1", "=A1 + 5");
            spreadsheet.SetContentsOfCell("E1", "=C1 + 1");
            spreadsheet.SetContentsOfCell("F1", "=D1 + 1");
            spreadsheet.SetContentsOfCell("G1", "=D1 + 2");
            spreadsheet.SetContentsOfCell("H1", "=G1 + 1");

            // dependency graph:
            // a --> b
            // +---> c ---> e
            // +---> d ---> f
            //       + ---> g
            //              +----> h
            // where:
            //  a: A1
            //  b: B1
            //  c: C1
            //  d: D1
            //  ...

            // change A1 and save the set of affected elements it returns
            IEnumerable<String> affectedCells = spreadsheet.SetContentsOfCell("A1", "6.6");
            HashSet<String> affectedCellsSet = affectedCells as HashSet<String>;

            // test that the set contains only A1 and all its dependents
            HashSet<String> expectedCells = new HashSet<string>();
            expectedCells.Add("A1");
            expectedCells.Add("B1");
            expectedCells.Add("C1");
            expectedCells.Add("D1");
            expectedCells.Add("E1");
            expectedCells.Add("F1");
            expectedCells.Add("G1");
            expectedCells.Add("H1");
            Assert.IsTrue(affectedCellsSet.SetEquals(expectedCells));
            Assert.AreEqual(6.6, Convert.ToDouble(spreadsheet.GetCellContents("A1")));
        }

        /// <summary>
        /// Test that attempting to create a circular dependecy with a SetContentsOfCell call results in CircularExcpetion and no changes are made to the spreadsheet.
        /// </summary>
        // TODO test modification to spreadsheet
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetValueCircularDependencyTest()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "3.3");
            spreadsheet.SetContentsOfCell("A2", "=A1 + 3");
            spreadsheet.SetContentsOfCell("A1", "=A2 + 5"); // exception thrown here
        }

        /// <summary>
        /// Test creating a FormulaError with a SetContentsOfCell.
        /// </summary>
        [TestMethod]
        public void SetValueFormualaErrorTest()
        {
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "3.3");
            spreadsheet.SetContentsOfCell("A1", "=3 / 0");
            Func<String, Double> lookupFunctor = x =>
            {
                throw new ArgumentException("Not found");
            };

            Formula equalFormulaA1 = new Formula("3 / 0");
            object formEval = new FormulaError("Attempted to divide by zero.");

            object A1val = spreadsheet.GetCellContents("A1");
            // try to cast them to formulas, if it does not work then both vars will be null and test will fail later
            Formula A1Formula = A1val as Formula;

            // test evaluation of formula
            object A1Eval = A1Formula.Evaluate(lookupFunctor);
            Assert.AreEqual(A1Eval, formEval);

            // test that formulas are equal to separately constructed formulas
            Assert.AreEqual(A1Formula, equalFormulaA1);
        }

        /// <summary>
        /// Get the names of nonempty cells when all cells are empty.
        /// </summary>
        [TestMethod]
        public void EmptyGetNonEmptyCellsTest()
        {
            // construct spreadsheet with one nonempty cell
            AbstractSpreadsheet spreadsheet = new Spreadsheet();

            // get list of all nonempty cells
            List<String> nonEmptyCells = spreadsheet.GetNamesOfAllNonemptyCells() as List<String>;

            // assert results
            Assert.AreEqual(nonEmptyCells.Count, 0);
        }

        /// <summary>
        /// Test getting the names of nonempty cells when there is only one nonempty cell.
        /// </summary>
        [TestMethod]
        public void SingleGetNonEmptyCellsTest()
        {
            // construct spreadsheet with one nonempty cell
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "3");

            // get list of all nonempty cells
            List<String> nonEmptyCells = spreadsheet.GetNamesOfAllNonemptyCells() as List<String>;

            // assert results
            Assert.IsTrue(nonEmptyCells.Contains("A1"));
            Assert.AreEqual(nonEmptyCells.Count, 1);
        }

        /// <summary>
        /// Test getting the name of nonempty cells when there are many, nondependent nonempty cells of all datatypes.
        /// </summary>
        [TestMethod]
        public void MultipleNondependentGetNonEmptyCellsTest()
        {
            // construct spreadsheet with one nonempty cell
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "3");
            spreadsheet.SetContentsOfCell("B1", "=8");
            spreadsheet.SetContentsOfCell("C1", "this is a string");
            spreadsheet.SetContentsOfCell("D1", "100.3");

            // get list of all nonempty cells
            List<String> nonEmptyCells = spreadsheet.GetNamesOfAllNonemptyCells() as List<String>;

            // assert results
            Assert.IsTrue(nonEmptyCells.Contains("A1"));
            Assert.IsTrue(nonEmptyCells.Contains("B1"));
            Assert.IsTrue(nonEmptyCells.Contains("C1"));
            Assert.IsTrue(nonEmptyCells.Contains("D1"));
            Assert.AreEqual(nonEmptyCells.Count, 4);
        }

        /// <summary>
        /// Test getting the name of nonempty cells when there are many, dependent nonempty cells
        /// </summary>
        [TestMethod]
        public void MultipleDependentGetNonEmptyCellsTest()
        {
            // construct spreadsheet with one nonempty cell
            AbstractSpreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "3");
            spreadsheet.SetContentsOfCell("B1", "=A1 + 3");
            spreadsheet.SetContentsOfCell("C1", "=B1 + 8");
            spreadsheet.SetContentsOfCell("D1", "=C1 + 12");

            // get list of all nonempty cells
            List<String> nonEmptyCells = spreadsheet.GetNamesOfAllNonemptyCells() as List<String>;

            // assert results
            Assert.IsTrue(nonEmptyCells.Contains("A1"));
            Assert.IsTrue(nonEmptyCells.Contains("B1"));
            Assert.IsTrue(nonEmptyCells.Contains("C1"));
            Assert.IsTrue(nonEmptyCells.Contains("D1"));
            Assert.AreEqual(nonEmptyCells.Count, 4);
        }
    }
}
