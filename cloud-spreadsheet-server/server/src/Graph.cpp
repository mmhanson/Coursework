#include <iostream>
#include "Graph.h"

/*Destructor, cleans up all the data
 */
Graph::~Graph()
{
  delete &cells;
}

/*Returns true if a cycle is found in the graph starting from c
 */
bool hasCycle(Cell *c, int count)
{
  //Increment the counter, and set the preNumber
  count++;
  c->preNumber = count;

  //Iterate through the dependents of c
  for(std::vector<Cell*>::iterator it = c->dependents.begin(); it!=c->dependents.end();it++)
    {
      //If the preNumber of a cell is larger than the post number (default 0), a cycle exists
      if(*it == c)
	continue;
      if((*it)->preNumber > (*it)->postNumber)
	return true;

      //If any of the dependents report a cycle, a cycle exists
      if(hasCycle((*it),count))
	return true;
    }

  //Increment the counter, and assign a post number. No cycles were found.
  count ++;
  c->postNumber = count;
  return false;
}

/*Tries to edit the cell, returns true if an edit was successful
 *This method takes in a string vector as the list of dependents
 */
bool Graph::edit(Cell *c, std::string contents, std::vector<std::string> deps)
{
  //Find all the cells
  std::vector<Cell*> newDeps;

  //Find all the dependent cells
  for(std::vector<std::string>::iterator it = deps.begin();it!=deps.end();++it)
    {
      newDeps.push_back(findCellByName(*it));
    }

  //Call the other edit method
  return edit(c,contents,newDeps);
}

/*Tries to edit the cell, returns true if an edit was successful
 *This method takes in a vector of Cell pointers as the list of dependents
 */
bool Graph::edit(Cell *c, std::string contents, std::vector<Cell*> newDeps)
{
  //Old and new dependencies of the cell
  std::vector<Cell*> oldDeps(c->dependents);

  //Prepare the graph for Cycle checking
  for(std::vector<Cell*>::iterator it = cells.begin();it!=cells.end();++it)
    {
      (*it)->preNumber = 0;
      (*it)->postNumber = 0;
    }

  //Check the graph for cycles
  c->dependents = newDeps;
  if(hasCycle(c,0))
  {
    c->dependents = oldDeps;
    return false;
  }

  //Finish the edit if no cycles are found
  c->history.push_back(c->contents);
  c->contents = contents;
  c->dependencyHistory.push_back(oldDeps);
  return true;
}

/*Returns a pointer to a cell with the following name. Creates a new cell if no such cell exists yet.
 */
Cell* Graph::findCellByName(std::string name)
{
  //See if a cell with this name already exists
  for(std::vector<Cell*>::iterator it = cells.begin();it!=cells.end();++it)
    {
      if((*it)->name == name)
	return *it;
    }

  //If no such cell exists, create one
  Cell *newCell = new Cell(name,"");
  cells.push_back(newCell);
  return newCell;
}
