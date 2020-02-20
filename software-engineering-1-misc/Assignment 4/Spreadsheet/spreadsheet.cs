// Written by Maxwell Hanson (u0985911) on October 2018
//
// Changed in PS5 (version 1.1):
//   new:
//     exception: SpreadsheetReadWriteException
//     property to determine if it has been changed since it was created or saved
//     property added to determine if a string is a valid variable name
//     property to normalize cell names
//     property to get version information
//     method to retrieve saved version string
//     method to save Spreadsheet as XML
//     method to retrieve cell value
//     SetContentsOfCell method overload with argument 'contents'
//   modified:
//     cell names must be validated by the isValid functor as well as be valid cell names
//     cell names must be normalized before they are used at all
//     superconstructor changed to have more arguments
//     all other SetContentsOfCell overloads are set to protected
//
// Changed in PS6 (version 1.2):
//  modified:
//   restoration constructor changed to be more readable and correct
//   save method chagned to be more readable and correct
//   cell name validation regex changed to fit critera 
//   added normalization to methods where it was absent

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using SpreadsheetUtilities;

namespace SS
{
    // TODO validate change in value, save, validation, etc...
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// A spreadsheet is considered a collection of nonempty cells in this implementation.
        /// 
        /// Dictionary was chosen since it uses a hash table and the operations add, remove, contains will be constant.
        /// Furthermore, a dictionary was chosen over a hashset since we need to take strings (cell names) and get cells (Cell class instances.)
        /// </summary>
        private Dictionary<String, Cell> cells;

        /// <summary>
        /// A description of the dependencies each cell has with another.
        /// 
        /// This data structure tracks every cell that references the values of other cells.
        /// </summary>
        private DependencyGraph dependencies;

        /// <summary>
        /// A regex to determine if a cell name is valid.
        /// 
        /// Cell names are valid if, and only if they consist of one or more letters followed by one or more digits.
        /// Note that this is only the base test for a valid cell name. Further restrictions are applied by
        /// the validator provided at construction time.
        /// </summary>
        private String validCellNameRegex = "^[A-Za-z]+[0-9]+$";

        /// <summary>
        /// Keep track if this spreadsheet has been changed since it was created or saved.
        /// </summary>
        private Boolean isChanged;

        /// <summary>
        /// Determine whether or not the spreadsheet has changed since it was created or saved.
        /// 
        /// True if it has changed, false otherwise.
        /// </summary>
        public override bool Changed
        {
            get => this.isChanged;
            protected set => this.isChanged = value;
        }

        // === Constructor(s) ===

        /// <summary>
        /// Create an empty spreadsheet with default version and no validator or normalizer.
        /// 
        /// <remarks>
        /// With no validator or normalizer, all variables and cell names (that meet base requirements) are valid
        /// and variables and cell names are not changed during normalization. Version string is "default"
        /// </remarks>
        /// </summary>
        public Spreadsheet() : base(x => true, x => x, "default")
        {
            this.cells = new Dictionary<String, Cell>();
            this.dependencies = new DependencyGraph();
            this.isChanged = false;
        }

        /// <summary>
        /// Create an empty spreadsheet with validation and normalization and version.
        /// 
        /// <remarks>
        /// Validation supplies extra restraints beyond the base requirements for variables and cell names
        /// Variables/Cell names must:
        ///   1. Consist of one or more letters followed by one or more digits
        ///   2. Return true when passed to isValid()
        /// Every variable in every formula is replaced with normalizer(variable). Every cell name is also replaced with normalizer(name)
        /// Version string is 'version'.
        /// </remarks>
        /// </summary>
        /// <param name="isValid">The validator delegate to use to validate cell names</param>
        /// <param name="normalizer">The normalizer delegate to use to normalize cell names</param>
        /// <param name="version">The version of the spreadsheet</param>
        public Spreadsheet(Func<String, bool> isValid, Func<String, String> normalizer, String version) : base(isValid, normalizer, version)
        {
            this.cells = new Dictionary<String, Cell>();
            this.dependencies = new DependencyGraph();
            this.isChanged = false;
        }

        /// <summary>
        /// Create a spreadsheet from a saved file with validation and normalization and version.
        /// 
        /// <remarks>
        /// Spreadsheet is restored from the saved file whose path is given as an argument.
        /// Validation supplied extra restraints beyond the base requirements for variables and cell names
        /// Variables/Cell names must:
        ///   1. Consist of one or more letters followed by one or more digits
        ///   2. Return true when passed to isValid()
        /// Every variable in every formula is replaced with normalizer(variable). Every cell name is also replaced with normalizer(name)
        /// Version string is 'version'.
        /// </remarks>
        /// </summary>
        /// <param name="xmlFilePath">The path to the xml file representing a saved spreadsheet</param>
        /// <param name="isValid">The validator delegate to use to validate cell names</param>
        /// <param name="normalizer">The normalizer delegate to use to normalize cell names</param>
        /// <param name="version">The version of the spreadsheet</param>
        /// <exception cref="SpreadsheetReadWriteException">
        /// Thrown when:
        ///     The version of the saved spreadsheet does not match the version parameter
        ///     There is an invalid cell name in the saved spreadsheet
        ///     There are invalid formulas or circular dependencies in the spreadsheet file
        ///     There are any problems opening, reading, or closing the file
        /// </exception>
        public Spreadsheet(String xmlFilePath, Func<String, bool> isValid, Func<String, String> normalizer, String version) : base(isValid, normalizer, version)
        {
            // Beginning setup
            this.cells = new Dictionary<String, Cell>();
            this.dependencies = new DependencyGraph();

            // Begin trying to read the filepath
            try
            {
                // Start by trying to read the sheet data
                XDocument sheetData = XDocument.Load(xmlFilePath);

                XElement spreadsheet = sheetData.Element("spreadsheet");

                XAttribute sheetVersion = spreadsheet.Attribute("version");
                if (ReferenceEquals(sheetVersion, null))
                {
                    throw new SpreadsheetReadWriteException("Spreadsheet version could not be found");
                }

                // Get the cells
                IEnumerable<XElement> cells = spreadsheet.Elements("cell");

                // For each cell in the xml, set the corresponding cell to its contents
                foreach (XElement cell in cells)
                {
                    // Get the cell elements corresponding to its name and contents
                    XElement name = cell.Element("name");
                    XElement content = cell.Element("contents");
                    SetContentsOfCell(name.Value, content.Value);
                }
                // Once everything has been added, set isChanged to false
                isChanged = false;
            }
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException(e.Message);
            }
        }

        // === Cell Content Modifier Methods ===

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        public override object GetCellContents(string name)
        {
            // normalize cell name
            name = this.Normalize(name);

            // throw exception if cellname is null or invalid
            if (name == null || !isValidCellName(name))
            {
                throw new InvalidNameException();
            }

            // return contents if cell exists
            if (this.cells.ContainsKey(name))
            {
                Cell thisCell = this.cells[name];
                return thisCell.GetContents();
            }

            // cell does not exist, then it is 'empty', return an empty string
            return "";
        }

        /// <summary>
        /// Get the color of a cell
        /// </summary>
        /// <param name="cellName">Cell name to get color of</param>
        /// <returns></returns>
        public Color GetCellColor(String cellName)
        {
            // if nonempty, return color
            if (cells.ContainsKey(cellName))
            {
                return cells[cellName].GetColor();
            }
            // if empty, return white
            else
            {
                return Color.White;
            }
        }

        /// <summary>
        /// Set the color of a cell.
        /// 
        /// Cellname is assumed valid and all args are assumed nonnull
        /// </summary>
        /// <param name="cellName">Cell name to set color of</param>
        /// <returns>true if cell exists, false otherwise</returns>
        public bool SetCellColor(String cellName, Color color)
        {
            String normalizedCellName = this.Normalize(cellName);

            // if nonempty, return color
            if (cells.ContainsKey(normalizedCellName))
            {
                cells[normalizedCellName].SetColor(color);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override string GetSavedVersion(string filename)
        {
            try
            {
                XDocument sheetData = XDocument.Load(filename);

                // Try to find the 'spreadsheet' element, if it exists
                XElement sheetElement = sheetData.Element("spreadsheet");
                if (ReferenceEquals(sheetElement, null))        // if xml file does not have 'spreadsheet' element, then it is not valid
                {
                    throw new SpreadsheetReadWriteException("Filepath does not correspond to spreadsheet file");
                }

                // Try to find the version attribute, if it exists
                XAttribute version = sheetElement.Attribute("version");
                if (ReferenceEquals(version, null))
                {
                    throw new SpreadsheetReadWriteException("Spreadsheet version could not be found");
                }

                return version.Value;
            }
            catch (Exception e)
            {
                // If anything else goes wrong throw a SpreadsheetReadWriteException with the message explaining what went wrong.
                throw new SpreadsheetReadWriteException(e.Message);
            }
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>
        /// cell name goes here
        /// </name>
        /// <contents>
        /// cell contents goes here
        /// </contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(string filename)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                // Should default to two spaces, which is fine
                settings.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    // Begin document
                    writer.WriteStartDocument();

                    // Initialize spreadsheet wrapper
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);

                    foreach (Cell cell in cells.Values)
                    {
                        // Cell wrapper
                        writer.WriteStartElement("cell");

                        // Name wrapper
                        writer.WriteStartElement("name");
                        writer.WriteString(cell.GetName());
                        writer.WriteEndElement();

                        // Contents wrapper
                        writer.WriteStartElement("contents");
                        string content = cell.GetContents().ToString();
                        // Make the necessary change to the formula string if it is one
                        if (cell.GetContents() is Formula)
                        {
                            content = "=" + content;
                        }
                        writer.WriteString(content);
                        writer.WriteEndElement();

                        // End the cell
                        writer.WriteEndElement();
                    }
                    // End the spreadsheet
                    writer.WriteEndElement();
                    // End the file
                    writer.WriteEndDocument();
                    // Make the document no longer changed
                    Changed = false;
                }
            }
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException(e.Message);
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            // normalize cell name
            name = this.Normalize(name);

            // if null or invalid, then throw InvalidNameException
            if (name == null || !IsValid(name))
            {
                throw new InvalidNameException();
            }

            // check if cell name is in list of cell names
            if (this.cells.ContainsKey(name))
            {
                Cell cell = this.cells[name];       // get the cell instance referred to by 'name'
                object cellVal = cell.GetValue();   // determine the value

                return cellVal;
            }
            // return empty string if cell is empty (i.e. not found in list of cell names)
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            // the keys of the cell dictionary are all of their names.
            // constructs a separate list instead of just returning the `keys` property
            // of the cells dictionary since we need to return an empty enumerable, not null
            // when there are no nonempty cells
            List<String> nonEmptyCells = new List<string>();

            foreach (String key in this.cells.Keys)
            {
                nonEmptyCells.Add(key);
            }

            return nonEmptyCells;
        }

        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            // normalize cell name
            name = this.Normalize(name);

            // parameter sanitiation
            if (ReferenceEquals(content, null))
            {
                throw new ArgumentNullException("null content parameter");
            }
            if (ReferenceEquals(name, null) || !isValidCellName(name))
            {
                throw new InvalidNameException();
            }

            // delegate work to specific helper methods
            if (Double.TryParse(content, out double tmpDouble))
            {
                return this.SetCellContents(name, Convert.ToDouble(content));
            }
            if (content.StartsWith("="))
            {
                // set to formula of contents (minus first character, which is '=')
                Formula contentFormula = new Formula(content.Substring(1), this.Normalize, this.IsValid);
                return this.SetCellContents(name, contentFormula);
            }
            // delegates to String overload if contents is not double or formula
            return this.SetCellContents(name, content);
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            return HandleCellModification(name, number);
        }

        /// <summary>
        /// the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        //
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, string text)
        {
            return HandleCellModification(name, text);
        }

        /// <summary>
        /// if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            return HandleCellModification(name, formula);
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("Null name of cell to calculate dependents of.");
            }
            if (!isValidCellName(name))
            {
                throw new InvalidNameException();
            }

            // use the dependency graph object to determine the direct dependents of the cell
            // note that, despite the ambiguous name, this method only returns direct dependents
            return this.dependencies.GetDependents(name);
        }

        /// <summary>
        /// Determine if a cell name is valid.
        /// 
        /// The definition of a valid cell name is determined by the regex attribute, validCellNameRegex
        /// </summary>
        /// <param name="cellName">The name of the cell to validate.</param>
        /// <returns>True if the cell name is valid, false otherwise</returns>
        private Boolean isValidCellName(String cellName)
        {
            // must be a valid cell according to the spreadsheet and the validator
            return (Regex.IsMatch(cellName, validCellNameRegex) && this.IsValid(cellName));
        }

        /// <summary>
        /// Lookup the (double) value of a cell.
        /// 
        /// <remarks>
        /// This method is different from GetCellValue in that it throws ArgumentException if there is an error and
        ///   it returns a double, not an object.
        /// This method is meant to be used as a lookup delegate to evaluate a cell
        /// </remarks>
        /// </summary>
        /// <param name="cellName">The name of the cell to get the value of</param>
        /// <returns>object representing value of cell (Double, String, Formulaexception</returns>
        /// <exception cref="ArgumentException">
        /// Throws ArgumentException if cellName is invalid or if the value of the cell is 
        /// text, empty, or a FormulaError
        /// </exception>
        public double lookup(String cellName)
        {
            object value;
            try
            {
                value = this.GetCellValue(cellName);
            }
            catch (InvalidNameException)
            {
                // throw argument exception if cellName is invalid
                throw new ArgumentException("invalid cell name");
            }

            // if value is text or empty or FormulaError, then also throw ArgumentException
            if (value is String || value is FormulaError)
            {
                throw new ArgumentException();
            }

            return Convert.ToDouble(value);
        }

        /// <summary>
        /// Move spreadsheetReader to the next element.
        /// 
        /// Helper method for reading and restoring spreadsheet xml files.
        /// </summary>
        /// <param name="spreadsheetReader"></param>
        /// <returns>True if spreadsheetReader is at the next element, false otherwise (if no other elments in file)</returns>
        private Boolean ReadNextElement(XmlReader spreadsheetReader)
        {
            while (spreadsheetReader.Read())
            {
                if (spreadsheetReader.NodeType == XmlNodeType.Element)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Throw SpreadsheetReadWriteException if name not equal to expectedName.
        /// 
        /// Helper method for reading and restoring spreadsheet xml files.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="expectedName"></param>
        private void assertCellName(String name, String expectedName)
        {
            if (name != expectedName)
            {
                throw new SpreadsheetReadWriteException("unexpected xml element. Expected '" + expectedName + "', found '" + name + "'");
            }
        }

        /// <summary>
        /// This method handles the changes of the contents of a cell, regardless of type.
        /// Effectively it is a single method that can perform the functionality of all varients of the SetCellContent methods.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="newContent"></param>
        /// <returns></returns>
        private ISet<string> HandleCellModification(string name, object newContent)
        {
            // Normalize the name
            name = Normalize(name);

            // old content saved in case of circular exception (will be used to restore state)
            Object oldContent = GetCellContents(name);
            // TODO may need modification
            if (!oldContent.Equals(newContent))
            {
                Changed = true;
            }

            // This method and others like it will be split into four sections
            // 1.) Adding a formula to an empty cell
            // 2.) Updating the contents of a nonempty cell
            // 3.) Updating the dependency graph (if necessary)
            // 4.) returning the set of cells affected by change

            // Part 1.) Changing an empty cell into a nonempty one (i.e. "adding" it)
            if (!cells.ContainsKey(name) && newContent.ToString() != "")    // add cell if cell is empty
            {
                cells.Add(name, new Cell(name, newContent));
            }
            // Part 2.) Updating the value of a nonempty cell
            else     // update the value of the cell if it is nonempty
            {
                if (newContent.ToString() == "")    // setting value to empty
                {
                    // cell is being made empty, must be removed from nonempty cells and depndencies updated
                    cells.Remove(name);
                    foreach (string cell in dependencies.GetDependees(name))
                    {
                        dependencies.RemoveDependency(cell, name);
                    }
                }
                else
                {
                    // If a formula is switched to something other than a formula and is not an empty string,
                    // Then we need to update the dependency graph
                    if (oldContent is Formula)
                    {
                        // Make sure that the cell does not depend on any cells in the old formula
                        foreach (string cell in dependencies.GetDependees(name))
                        {
                            dependencies.RemoveDependency(cell, name);
                        }
                    }
                    // set Cell instance referred to by 'name' in the cells dictionary to a new cell with the updated contents
                    cells[name] = new Cell(name, newContent);
                }
            }

            // Part 3.) Updating the dependency graph, if necessary
            if (newContent is Formula)
            {
                Formula formula = (Formula)newContent;
                foreach (string cell in formula.GetVariables())
                {
                    // Each variable in the formula should indicate a valid cell name
                    // Add this cell as a dependent of each variable in the formula
                    dependencies.AddDependency(cell, name);
                }
            }

            // Part 4.) Finding directly and indirectly dependent cells of the given cell.
            // Find the cells that depend of the given cell both directly and indirectly and update their value
            // Note that this will throw if there exists a cirular depencency in the graph
            try
            {
                IEnumerable<string> dependents = GetCellsToRecalculate(name);
                // update all the values of the cell's dependents
                // note that the cell directly changed will apprear in the 'dependents' enumeration, so it will be updated
                foreach (string dependentCell in dependents)
                {
                    // Any cell that was emptied will still appear in the list of dependents
                    // So we need to make sure to skip over those.
                    // We don't need to update the value because those cells will be deleted by the garbage collector
                    if (cells.ContainsKey(dependentCell))   // if dependentCell is nonempty, then update the value
                    {
                        cells[dependentCell].Update(this);
                    }
                }
                return new HashSet<string>(dependents);
            }
            catch (CircularException)
            {
                // revert changes and throw exception
                HandleCellModification(name, oldContent);
                throw new CircularException();
            }
        }
    }
}