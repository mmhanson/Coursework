#include <iostream>
#include "Cell.h"
#include "Graph.h"

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
Cell::Cell(std::string name, std::string contents)
{
  this->name = name;
  this->contents = contents;
  this->revertProgress = -1;
}
/*Destructor, clears all data in the cell
 */
Cell::~Cell()
{
  this->name = "";
  this->contents = "";
  this->revertProgress = 0;
  delete &dependents;
  delete &history;
  delete &dependencyHistory;
}

/*Attempts to revert the cell's contents to the last version in history
 *If reversion would cause a circular dependency, nothing happens
 *Returns true if the reversion happened, and false otherwise
 */
bool Cell::revert(Graph &g)
{
  if(this->revertProgress==-1)
    return false;
  std::string newConts = this->history.at(this->revertProgress);
  std::vector<Cell*> newDeps = this->dependencyHistory.at(this->revertProgress);
  if(g.edit(this,newConts,newDeps))
    {
      revertProgress--;
      return true;
    }
  
  return false;
}

bool Cell::undo(Graph &g)
{
   //If there is no reversion to be made, return false
  if(this->history.size()==0&&this->contents =="")
    return false;
  else if(this->history.size()==0&&this->contents !="")
    {
      this->contents = "";
      this->dependents.clear();
      this->revertProgress = -1;
      return true;
    }

  //Access the last contents and dependents
  std::string newConts = history.back();
  std::vector<Cell*> newDeps = dependencyHistory.back();

  //If the graph can successfully edit the cell, history and dependencyHistory will gain something instead of losing something, in compliance with the Graph class. Removing two elements from both arrays will solve this.
  if(g.edit(this,newConts,newDeps))
    {
      this->history.pop_back();
      this->dependencyHistory.pop_back();
      this->history.pop_back();
      this->dependencyHistory.pop_back();
      if(this->revertProgress >= this->history.size())
	this->revertProgress= this->history.size()-1;
      else
	this->revertProgress++;
      return true;
    }

  //If the edit was unsuccessful, the cell will not be changed, so nothing more needs to be done
  return false;
}

/*Returns a format of the cell compliant with the protocol and JSON
 */
std::string Cell::toString()
{
  return "\""+this->name+"\": \""+this->contents+"\"";
}
