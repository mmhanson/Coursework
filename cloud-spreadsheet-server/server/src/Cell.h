/* A cell class represents one cell in a spreadsheet.
 *The name represents what position the cell occupies (eg: A2)
 *Contents represent the literal text contained by the cell
 *preNumber and postNumber are used by the Graph to detect cycles during an edit
 *dependents lists all other cells that are referenced by this cell
 *history shows the contents of the cell from the beginning to now
 *dependencyHistory shows the dependent cells of all past contents
 *
 *Samuel Englert, April 16 2019
 */
#ifndef CELL_H
#define CELL_H
#include <string>
#include <vector>

class Graph;
class Cell
{
 public:
 int revertProgress;
  int preNumber;
  int postNumber;
  std::string contents;
  std::vector<Cell*> dependents;
  std::vector<std::string> history;
  std::vector<std::vector<Cell*> > dependencyHistory;
  std::string name;
  Cell(std::string name,std::string contents);
  ~Cell();
  bool revert(Graph &g);
  bool undo(Graph &g);
  std::string toString();
};
#endif
