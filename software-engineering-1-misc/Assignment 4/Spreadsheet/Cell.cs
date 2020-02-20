/// Written by Maxwell Hanson (u0985911) on September 26th for CS 3500 Project 4

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;
using System.Windows;
using System.Drawing;

namespace SS
{
    /// <summary>
    /// Represents a cell in a spreadsheet.
    /// 
    /// This class was created to further decompose the functionality of a spreadsheet.
    /// </summary>
    class Cell
    {
        /// <summary>
        /// The contents of this cell.
        /// 
        /// Can be a double, string, or formula instance
        /// </summary>
        private object contents;

        /// <summary>
        /// The value of this cell.
        /// 
        /// Can be double, string, or FormulaError.
        /// This is different from contents in that if the contents is a formula, this attribute
        /// will contain its evaluation
        /// </summary>
        private object value;

        /// <summary>
        /// The name of this cell.
        /// </summary>
        private String name;

        /// <summary>
        /// The color of this cell
        /// </summary>
        private Color cellColor;

        /// <summary>
        /// Construct a Cell with a name and contents and color
        /// 
        /// <remarks>
        /// Note that the value of the cell is uninitialized by the constructor. It is assumed that the 'Update'
        /// method will be manually called after instantiating a cell to create the cells value.
        /// </remarks>
        /// </summary>
        /// <param name="name">The name of the cell</param>
        /// <param name="contents">The contents of the cell</param>
        /// <param name="color">The color of the cell</param>
        public Cell(String name, object contents, Color color)
        {
            this.name = name;
            this.contents = contents;
            this.cellColor = color;
        }

        /// <summary>
        /// Construct a Cell with a name and contents.
        /// 
        /// Color is white.
        /// 
        /// <remarks>
        /// Note that the value of the cell is uninitialized by the constructor. It is assumed that the 'Update'
        /// method will be manually called after instantiating a cell to create the cells value.
        /// </remarks>
        /// </summary>
        /// <param name="name">The name of the cell</param>
        /// <param name="contents">The contents of the cell</param>
        public Cell(String name, object contents)
        {
            this.name = name;
            this.contents = contents;
            this.cellColor = Color.White;
        }

        /// <summary>
        /// Return the contents of this Cell
        /// </summary>
        /// <returns>Returns a String, Double, or Formula object</returns>
        public object GetContents()
        {
            return contents;
        }

        /// <summary>
        /// Return the value of this Cell
        /// </summary>
        /// <returns>Returns a string, Double, or FormulaError object</returns>
        public object GetValue()
        {
            return value;
        }

        public Color GetColor()
        {
            return this.cellColor;
        }

        public void SetColor(Color color)
        {
            this.cellColor = color;
        }

        /// <summary>
        /// Return the name of the cell
        /// </summary>
        /// <returns></returns>
        public String GetName()
        {
            return this.name;
        }

        /// <summary>
        /// Update the value of this cell.
        /// 
        /// <remarks>
        /// When cells are dependent on one another, in the event that one cells value changes, the dependent cells' values
        /// are changed at that time as well so that updated information is presented. This method will update the value of
        /// this cell to be the most current. Specifically, it the direct dependents of this cell, retrieve their values,
        /// and recalulate the value of this cell accordingly.
        /// </remarks>
        /// </summary>
        /// <param name="spreadsheet">The spreadsheet to refer to for information needed to update the value</param>
        public void Update(Spreadsheet spreadsheet)
        {
            if (contents is double)
            {
                value = (double)contents;
            }
            else if (contents is string)
            {
                value = contents.ToString();
            }
            else if (contents is Formula)
            {
                Formula f = (Formula)contents;
                value = f.Evaluate(spreadsheet.lookup);
            }
        }
    }
}
