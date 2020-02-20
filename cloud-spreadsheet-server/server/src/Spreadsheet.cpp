// NOTE: c++11 needed to compile since this file makes use of std::to_string from string.hpp

#include <iostream>
#include "Spreadsheet.h"

// for reading/writing xml
#include <vector>
#include <string>
#include <boost/property_tree/ptree.hpp>
#include <boost/property_tree/xml_parser.hpp>
#include <boost/foreach.hpp>

namespace pt = boost::property_tree;

/*A spreadsheet is a list of cells all associated with the same document.
 *Edits, undos, reverts, and JSON formatting can be done from here
 *editOrder is a list of cell pointers organized by the time of cell edits.
 *name represents the name of the sheet
 *graph is the DependencyGraph that ensures that no circular dependencies exist. It can also find cells by their name.
 *
 * Automatically reads spreadsheet from storage if it has been written.
 * Spreadsheets are assumed to be stored as 'spreadsheets/<spreadsheet name>.xml'
 *
 * Samuel Englert, April 16 2019
 * Maxwell Hanson, April 2019
 */
Spreadsheet::Spreadsheet(std::string name)
{
  this -> name = name;
  this->graph = *(new Graph);

  // === handle reading/writing the spreadsheet ===
  // determine if the spreadsheet's corresponding file exists
  std::string read_path = 
    "spreadsheets/" + std::string(this->name) + ".xml";
  std::ifstream file_tester;
  file_tester.open(read_path);
  if (file_tester) {
    // the spreadsheet's file exists, read it in
    this->read();
  }
  else {
    // the spreadsheet's file does not exist, create one
    this->write();
  }
}

/*Destructor, clears all data
 */
Spreadsheet::~Spreadsheet()
{
  this->name = "";
  delete &editOrder;
  delete &graph;
}

/*
 * Attempts to undo the most recent edit to this Spreadsheet. 
 *
 * Returns a pointer to the undone cell if successful or Null otherwise.
 */
Cell* Spreadsheet::undo()
{
  if(editOrder.size()==0)
    return NULL;
  //Find the last edited cell
  Cell *c = editOrder.back();

  //Attempt a reversion on this cell, remove it from editOrder if successful
  if(c->undo(this->graph))
    {
      editOrder.pop_back();
      return c;
    }
  return NULL;
}

/*
 * Attempts to revert a cell.
 *
 * Returns a pointer to the undone cell or Null if the attempt failed.
 */
Cell* Spreadsheet::revert(std::string cellName)
{
  //Find the cell
  Cell *c = graph.findCellByName(cellName);

  //Attempts a reversion, updates editOrder if successful
  if(c->revert(this->graph))
    {
     editOrder.push_back(c);
      return c;
    }

  return NULL;
}

/*Attempts to edit a cell, returns true if the edit was successful
 */
bool Spreadsheet::edit(std::string cellName, std::string contents, std::vector<std::string> dependents)
{
  //Find the cell
  Cell *c = graph.findCellByName(cellName);

  //Attempt an edit, update editOrder if successful
  if(graph.edit(c,contents,dependents))
    {
      editOrder.push_back(c);
      c->revertProgress = c->history.size()-1;
      return true;
    }
  return false;
}

/*Returns the string representation of the Spreadsheet compliant with the protocol
 */
std::string Spreadsheet::toString()
{
  //First line
  std::string str = "{ \"type\": \"full send\", \n\"spreadsheet\": {\n";

  std::cout << "In the Spreadsheet to String function" << std::endl;
  //Iterates through every cell
  std::vector<Cell*>::iterator it = this->graph.cells.begin();
  std::cout << "Got the iterator" << std::endl;
  for( ;it!=this->graph.cells.end(); )
    {
      //Add a line representing this cell
      Cell *c = (*it);
      it++;
      str += c->toString();
      std::cout << c->toString() << std::endl;
      if(it!=this->graph.cells.end())
	str+=",";
      str+="\n";
    }

  //Last line
  str+= "}\n";
  str+= "}";

  std::cout << "Finishing the toString function" << std::endl;

  return str;
}

/**
 * Read the spreadsheet from storage.
 *
 * Assumes that this spreadsheet is empty.
 * Assumes that 'spreadsheets/<spreadsheet name>.xml' exists.
 * Assumes that the xml file does not have circular dependencies.
 * Spreadsheet will be populated with cells from the XML file.
 *
 * All spreadsheets are stored in the 'spreadsheets' directory
 * in an xml file named '<spreadsheet name>.xml'
 *
 * Maxwell Hanson, April 2019
 */
void Spreadsheet::read() {
  std::cout << "Reading the file" << std::endl;
  std::string read_path = 
    "spreadsheets/" + std::string(this->name) + ".xml";

  pt::ptree tree;
  pt::xml_parser::read_xml(read_path, tree);
  std::cout << "Parsed the file" << std::endl;

  pt::ptree::const_assoc_iterator it = tree.find("spreadsheet");
  if(it == tree.not_found()){
    return;
  }

  std::cout << "About to loop" << std::endl;

  BOOST_FOREACH(pt::ptree::value_type &val, tree.get_child("spreadsheet")) {
    if (val.first == "cell") {
 
      // grab name and contents from xml tags and set them in the cell
      std::string name = val.second.get<std::string>("name");
      std::string contents = val.second.get<std::string>("contents");

      Cell *new_cell = new Cell(name, contents);
      graph.cells.push_back(new_cell);

      pt::ptree::const_assoc_iterator hist = val.second.find("history");
      if(hist != val.second.not_found()){

	// grab the entire history of the cell and set it
	BOOST_FOREACH(pt::ptree::value_type &history, val.second.get_child("history")) {
	  // NOTE cell history goes from oldest to newest
	  new_cell->history.push_back(history.second.data());
	}
      }

      pt::ptree::const_assoc_iterator dep = val.second.find("dependents");
      if(dep != val.second.not_found()){

	// grab all cell dependents and set them
	BOOST_FOREACH(pt::ptree::value_type &dependent, val.second.get_child("dependents")) {
	  std::string dep_name = dependent.second.data();
	  Cell *dep_cell = graph.findCellByName(dep_name);
	  new_cell->dependents.push_back(dep_cell);
	}
      }

      // add the cell to the spreadsheet dependency graph directly
      
    }
  }
}

/**
 * Write the spreadsheet to storage.
 *
 * All spreadsheets are stored in the 'spreadsheets' directory
 * in an xml file named '<spreadsheet name>.xml'. Note calling
 * this multiple times will not concatenate, previous file
 * is overwritten each time."
 *
 * Maxwell Hanson, April 2019
 */
void Spreadsheet::write() {
  // tree is created then written out
  pt::ptree write_tree;

  // populate write tree
  for (int cell_idx = 0; cell_idx < graph.cells.size(); cell_idx++) {
    Cell *cell = graph.cells[cell_idx];
    
    // create empty cell tag under spreadsheet tag
    pt::ptree &cell_tag = write_tree.add("spreadsheet.cell", "");

    // populate name and contents
    cell_tag.add("name", cell->name);
    cell_tag.add("contents", cell->contents);
    // populate cell history
    for (int hist_idx = 0; hist_idx < cell->history.size(); hist_idx++) {
      std::string tag_name = "history." + std::to_string(hist_idx);
      cell_tag.add(tag_name, cell->history[hist_idx]);
    }
    // populate cell dependents
    for (int dep_idx = 0; dep_idx < cell->dependents.size(); dep_idx++) {
      std::string dep_name = cell->dependents[dep_idx]->name;
      std::string tag_name = "dependents.dependent";
      cell_tag.add(tag_name, dep_name);
    }
  }

  // write the tree to file
  std::string write_path = 
    "spreadsheets/" + this->name + ".xml";
  pt::write_xml(write_path, write_tree);
}
