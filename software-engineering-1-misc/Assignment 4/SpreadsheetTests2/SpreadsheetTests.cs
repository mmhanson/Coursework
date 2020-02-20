using System;
using SS;
using SpreadsheetUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        // Constructor Tests
        [TestMethod]
        public void TestLoadingStandardSpreadsheet()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("c3", "2.91");
            s.SetContentsOfCell("t35", "Eye candy");
            s.SetContentsOfCell("b5", "12");
            s.SetContentsOfCell("e4", "31");
            s.SetContentsOfCell("a1", "=c3 + 4");
            s.Save("TestStandardSpreadsheetForLoading.xml");
            AbstractSpreadsheet sCopy = new Spreadsheet("TestStandardSpreadsheetForLoading.xml", t => true, t => t, "default");
            Assert.AreEqual(12.0, sCopy.GetCellValue("b5"));
            Assert.IsTrue(sCopy.GetCellContents("a1") is Formula);
            Assert.IsTrue(sCopy.GetCellValue("t35") is string);
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestNotASpreadsheetLoad()
        {
            AbstractSpreadsheet s = new Spreadsheet("NotASpreadSheet.xml", t => true, t => t, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestNoVersionSpreadsheet()
        {
            AbstractSpreadsheet s = new Spreadsheet("NoVersionSpreadsheet.xml", t => true, t => t, "default");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestNotNonExistantSpreadsheet()
        {
            AbstractSpreadsheet s = new Spreadsheet("ThisSpreadsheetDoesNotExist.xml", t => true, t => t, "default");
        }

        // GetCellValue and GetCellContents tests
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetNullValueCell()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetCellValue(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetNullContentsCell()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }

        [TestMethod]
        public void TestGetCellValueBasicSum()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "=3 + c3");
            s.SetContentsOfCell("c3", "2.91");
            s.SetContentsOfCell("t35", "1003.2");
            s.SetContentsOfCell("b5", "12");
            s.SetContentsOfCell("e4", "31");
            Assert.AreEqual(5.91, (double)s.GetCellValue("a1"));
            s.SetContentsOfCell("c3", "10");
            Assert.AreEqual(13, (double)s.GetCellValue("a1"));
        }

        // Although there isn't a dedicated SetContentsOfCell Section
        // SetContentsOfCell is tested comprehensively in each test section (since it is used to build nonempty spreadsheets)
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellInvalidName()
        {
            AbstractSpreadsheet s = new Spreadsheet(t => Regex.IsMatch(t, "^[A-Z]+[0-9]+"), t => t.ToLower(), "ContradictorySpreadsheetVersion");
            s.SetContentsOfCell("A5", "Nothing lasts.... But nothing is lost");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetContentsOfCellNullArgument()
        {
            AbstractSpreadsheet s = new Spreadsheet(t => Regex.IsMatch(t, "^[A-Z]+[0-9]+"), t => t.ToUpper(), "UppercaseCellName-ZeroIndexedVersion");
            s.SetContentsOfCell("a5", null);
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetContentsOfCellCircularDependency()
        {
            AbstractSpreadsheet s = new Spreadsheet(t => Regex.IsMatch(t, "^[A-Z]+[0-9]+"), t => t.ToUpper(), "UppercaseCellName-ZeroIndexedVersion");

            s.SetContentsOfCell("a5", "25");
            s.SetContentsOfCell("n3", "2");
            s.SetContentsOfCell("a2", "=n3 + a5");
            s.SetContentsOfCell("b1", "=1.5 + c3");
            s.SetContentsOfCell("a3", "=a2 + b1");
            s.SetContentsOfCell("c3", "=a3 * n3");
        }

        [TestMethod]
        public void TestMultipleDependencyFormulaSpreadsheet()
        {
            // Since the title is somewhat hard to understand,
            // The goal of this test is to make a spreadsheet with
            // Many formulas that depend on each other and make sure it evaluates okay
            AbstractSpreadsheet s = new Spreadsheet(t => Regex.IsMatch(t, "^[A-Z]+[0-9]+$"), t => t.ToUpper(), "UppercaseCellName-ZeroIndexedVersion");
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("b1", "1");
            s.SetContentsOfCell("c1", "0");
            s.SetContentsOfCell("d1", "-5");
            s.SetContentsOfCell("e1", "DO NOT USE FOR ARITHMETIC");
            s.SetContentsOfCell("a2", "=4 * (a1 * a1)");
            s.SetContentsOfCell("a3", "= a2 + 3 * b1 + 4 * (d1 / 5)");
            s.SetContentsOfCell("a4", "=    a3/11 + (c1    * a2)");
            s.SetContentsOfCell("a5", "=c1 + e1");
            Assert.IsTrue((double)s.GetCellValue("A2") == 100.0);
            Assert.AreEqual((double)s.GetCellValue("A3"), 99.0);
            Assert.AreEqual((double)s.GetCellValue("a4"), 9.0);
            Assert.IsTrue(s.GetCellValue("a5") is FormulaError);
        }

        // Save tests
        [TestMethod]
        public void StandardSpreadsheetSaveDoubles()
        {
            // Nothing should throw an exception
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("c3", "2.91");
            s.SetContentsOfCell("t35", "1003.2");
            s.SetContentsOfCell("b5", "12");
            s.SetContentsOfCell("e4", "31");
            s.Save("TestStandardSpreadsheetSaveDoubles.xml");
            s.SetContentsOfCell("a1", "=c3 + 4");
            s.SetContentsOfCell("t35", "");
            s.SetContentsOfCell("b5", "=a1 + c3");
            s.SetContentsOfCell("b5", "=c3 * 3");
            s.SetContentsOfCell("b5", "");
            s.SetContentsOfCell("e4", "Basically covering old features with these changes");
            s.SetContentsOfCell("e4", "31");
            s.Save("TestStandardSpreadsheetSaveDoubles.xml");
        }

        // GetSavedVersion tests
        [TestMethod]
        public void StandardSpreadsheetGetVersion()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("c3", "2.91");
            s.SetContentsOfCell("t35", "1003.2");
            s.SetContentsOfCell("b5", "12");
            s.SetContentsOfCell("e4", "31");
            s.Save("TestStandardSpreadsheetSaveDoubles.xml");
            Assert.AreEqual("default", s.GetSavedVersion("TestStandardSpreadsheetSaveDoubles.xml"));
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestNoVersionGetVersionSpreadsheet()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetSavedVersion("NoVersionSpreadsheet.xml");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestGetVersionNonExistantSpreadSheet()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetSavedVersion("ThisSpreadsheetDoesNotExist.xml");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestGetVersionNotASpreadSheet()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.GetSavedVersion("NotASpreadSheet.xml");
        }

        [TestMethod]
        public void SpreadsheetGetNonStandardVersion()
        {
            AbstractSpreadsheet s = new Spreadsheet(t => Regex.IsMatch(t, "^[A-Z]+[0-9]+$"), t => t.ToUpper(), "AllCapsVersion");
            s.Save("TestAllCapsEmptySpreadsheetVersion.xml");
            Assert.AreEqual("AllCapsVersion", s.GetSavedVersion("TestAllCapsEmptySpreadsheetVersion.xml"));
        }

        // GetNamesOfAllNonemptyCells tests
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCellsStandard()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("c3", "2.91");
            s.SetContentsOfCell("t35", "Eye candy");
            s.SetContentsOfCell("b5", "12");
            s.SetContentsOfCell("e4", "31");
            s.SetContentsOfCell("a1", "=c3 + 4");
            s.SetContentsOfCell("t35", "");
            foreach(string cellname in s.GetNamesOfAllNonemptyCells())
            {
                if (cellname != "c3" && cellname != "e4" && cellname != "b5" && cellname != "a1")
                {
                    Assert.Fail();
                }
            }
        }
    }
}
