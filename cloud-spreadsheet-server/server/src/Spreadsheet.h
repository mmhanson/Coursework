/*A spreadsheet is a list of cells all associated with the same document.
 *Edits, undos, reverts, and JSON formatting can be done from here
 *editOrder is a list of cell pointers organized by the time of cell edits.
 *name represents the name of the sheet
 *graph is the DependencyGraph that ensures that no circular dependencies exist. It can also find cells by their name.
 *
 *Samuel Englert, April 16 2019
 */
#ifndef SPREADSHEET_H
#define SPREADSHEET_H

#include "Graph.h"
class Spreadsheet
{
 private:
  std::vector<Cell*> editOrder;
 public:
  std::string name;
  Graph graph;
  Spreadsheet(std::string name);
  ~Spreadsheet();
  Cell* undo();
  Cell* revert(std::string cellName);
  std::string toString();
  bool edit(std::string cellName,std::string contents, std::vector<std::string> dependents);
  void read();
  void write();
};

#endif
