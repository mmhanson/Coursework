/*A Graph is a collection of cells related like a DAG.
 *Any edit that creates a cycle will not go through
 *cells stores all the important cells in the graph
 *
 *Samuel Englert, April 16 2019
 */
#ifndef GRAPH_H
#define GRAPH_H

#include "Cell.h"
class Graph
{
 public:
  ~Graph();
  std::vector<Cell*> cells;
  bool edit(Cell *c, std::string contents,std::vector<std::string> deps);
  bool edit(Cell *c, std::string contents, std::vector<Cell*> deps);
  Cell* findCellByName(std::string name);
};

#endif
