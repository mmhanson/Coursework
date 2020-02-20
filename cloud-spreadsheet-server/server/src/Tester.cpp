#include <iostream>
#include "Spreadsheet.h"

int main(int argc, char **argv)
{
  std::cout <<"Test 1: Circular dependency\n";
  {
    Spreadsheet *s = new Spreadsheet("Test1.sprd");
    std::vector<std::string> deps;
    deps.push_back("B1");
    std::cout << s->edit("A1","=B1+5",deps)<<" (Should be 1)\n";
    deps.pop_back();
    deps.push_back("A1");
    std::cout << s->edit("B1","=A1+5",deps)<<" (Should be 0)\n";
  }

  std::cout <<std::endl;

  std::cout<<"Test 2: Cell equality\n";
  {
    Graph *g = new Graph();
    Cell *c1 = g->findCellByName("Jason");
    Cell *c2 = g->findCellByName("Jason");
    std::cout << (c1 == c2) <<" (Should be 1)\n";
  }

  std::cout <<std::endl;

  std::cout << "Test 3: Long chains\n";
  {
    Spreadsheet *s = new Spreadsheet("Test3.sprd");
    std::vector<std::string> deps;
    std::string lastName = "-1";
    for(int i=0;i<10000;i++)
      {
	std::string name = "";
	name += (i%10000);
	name+= ((i%10000)/1000);
	name+= ((i%1000)/100);
	name+= ((i%100)/10);
	name+= (i%10);
	deps.push_back(lastName);
	s->edit(name,lastName,deps);
	lastName = name;
	deps.pop_back();
      }
    std::cout << "Test 3 done (10,000 cells added)\n";
  }

  std::cout <<std::endl;

  std::cout <<"Test 4: Undo on a single cell\n";
  {
    Spreadsheet *s = new Spreadsheet("Test4.sprd");
    std::vector<std::string> deps;
    Cell *c = s->graph.findCellByName("A1");
    std::cout << s->edit("A1","Test",deps)<<" (Should be 1)\n";
    std::cout << s->edit("A1","Test2",deps)<<" (Should be 1)\n";
    s->undo();
    std::cout << (c->contents == "Test")<<" (Should be 1)\n";
    s->undo();
    std::cout << (c->contents == "")<<" (Should be 1)\n";
  }
  std::cout<<"\n";

  std::cout<<"Test 5: Revert on a single cell\n";
  {
    Spreadsheet *s = new Spreadsheet("Test4.sprd");
    std::vector<std::string> deps;
    Cell *c = s->graph.findCellByName("A1");
    s->edit("A1","Test",deps);
    s->edit("A1","Test2",deps);
    s->revert("A1");
    std::cout << (c->contents == "Test")<<" (Should be 1)\n";
    s->revert("A1");
    std::cout << (c->contents == "")<<" (Should be 1)\n";
  }
  std::cout<<"\n";

  std::cout<<"Test 6: Revert on an unmodified cell\n";
  {
    Spreadsheet *s = new Spreadsheet("Test6.sprd");
    Cell *c = s->graph.findCellByName("A1");
    std::cout << s->revert("A1") <<" (Should be 0)\n";
  }
  std::cout<<"\n";

  std::cout<<"Test 7: Revert on a singly-modified cell\n";
  {
    Spreadsheet *s = new Spreadsheet("Test7.sprd");
    Cell *c = s->graph.findCellByName("A1");
    std::vector<std::string> deps;
    std::cout << s->edit("A1","Text",deps)<<" (Should be 1)\n";
    std::cout <<s->revert("A1")<<" (Should be 1)\n";
    std::cout << (c->contents == "")<<" (Should be 1)\n";
  }
  std::cout<<"\n";

  std::cout<<"Test 8: A cell dependent on itself\n";
  {
    Spreadsheet *s = new Spreadsheet("Test8.sprd");
    Cell *c = s->graph.findCellByName("A1");
    std::vector<std::string> deps;
    deps.push_back("A1");
    std::cout << s->edit("A1","Text",deps) << " (Should be 1)\n";
  }
  std::cout<<"\n";

  std::cout<<"Test 9: Undoing a new spreadsheet\n";
  {
    Spreadsheet *s = new Spreadsheet("Test8.sprd");
    std::cout << s->undo() << " (Should be 0)\n";
  }
  std::cout<<"\n";

  std::cout<<"Test 10: Excessive undoes, edits and reverts\n";
  {
    Spreadsheet *s = new Spreadsheet("Test8.sprd");
    std::vector<std::string> deps;
    std::string contentsOverTime = "";
    Cell *c = s->graph.findCellByName("A1");
    s->edit("A1","A",deps);
    contentsOverTime += c->contents;
    s->edit("A1","B",deps);
    contentsOverTime += c->contents;
    s->edit("A1","C",deps);
    contentsOverTime += c->contents;
    s->edit("A1","D",deps);
    contentsOverTime += c->contents;
    s->edit("A1","E",deps);
    contentsOverTime += c->contents;
    s->undo();
    contentsOverTime += c->contents;
    s->undo();
    contentsOverTime += c->contents;
    s->revert("A1");
    contentsOverTime += c->contents;
    s->revert("A1");
    contentsOverTime += c->contents;
    s->revert("A1");
    contentsOverTime += c->contents;
    s->undo();
    contentsOverTime += c->contents;
    s->undo();
    contentsOverTime += c->contents;
    s->edit("A1","F",deps);
    contentsOverTime += c->contents;
    s->undo();
    contentsOverTime += c->contents;
    s->revert("A1");
    contentsOverTime += c->contents;
    std::cout << contentsOverTime<<"\n";
    std::cout << "ABCDEDCBAABFBC (These strings should be the same)\n";
  }
}
