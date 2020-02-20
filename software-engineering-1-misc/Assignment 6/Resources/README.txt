Last Updated: 10/22/18

Authors:
Joshua Cragun
Max Hanson

All the models used in the backend of this project are debugged and pass all tests, they should work as expected.

GUI Design Decisions
=================
The 'File' menu, we decided, was implemented as a toolbar of icons on the top, explanations of each button can
be found by clicking the 'help' icon on the toolbar. Its the icon with the question mark.

Below the toolbar is information about the cell. There is a label containting the name of the currently selected cell.
To the right of that label is another label containing the contents of the currently selected cell. Downward from there,
we have to more boxes. The left one is a text box displaying the value of the current cell. This box is editable, if you
edit a new value, then press set, the current cell will adopt that value.

Below that is the spreadsheet grid, which is standard across assignments.

Extra feature
=============
For the extra feature we added the ability to change the color of cells. Note there are some caveats here. For one, the 
color of a cell can only be changed if the cell is nonempty. Due to the way I implemented the coloring, the cell color
is tied in with the cell class in the spreadsheet class, so to set to colors of empty cells would require a lot of
restructuring of the spreadsheet class. Also, when spreadsheets are saved, the cell colorings are not saved. This one
would require less work to fix, however the feature has already required about two hours of work to implement and I don't
think it is really required here.
To change the color of a nonempty cell, press the 'Set color' button to get a generic dialog to set the cell color. If
the cell is empty, you will get a helpful message explaining the caveat.

