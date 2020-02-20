#include <iostream>
#include <vector>
#include <string>


void connect();
void disconnect();
void connectToLog();
void disconnectFromLog();
void shutDown();
bool connected, readingLog;

int main(int argc, char **argv)
{
  std::cout << "CS3505 Administrative Backend\n";
  std::cout << "Type \"help\" for a list of commands\n";
  std::vector<std::string> commands;
  std::vector<std::string> descriptions;
  {
    commands.push_back("help");
    descriptions.push_back("                  list all commands");
    commands.push_back("connect");
    descriptions.push_back("               connect to the server, if active");
    commands.push_back("disconnect");
    descriptions.push_back("            disconnect from the server");
    commands.push_back("start_log");
    descriptions.push_back("             start receiving real-time updates about the server");
    commands.push_back("stop_log");
    descriptions.push_back("              stop receiving updates");
    commands.push_back("shut_down");
    descriptions.push_back("             cleanly stop the server");
    commands.push_back("delete_sheet_(sheet)");
    descriptions.push_back("  delete the spreadsheet, if it exists");
    commands.push_back("delete_user_(username)");
    descriptions.push_back("delete the user, if it exists");
    commands.push_back("exit");
    descriptions.push_back("                  stop the backend");
  }
  std::string input;
  while(true)
    {
      std::cin >> input;
      if(input == "exit")
	{
	  if(readingLog)
	    disconnectFromLog();
	  if(connected)
	    disconnect();
	  break;
	}
      else if(input == "help")
	{
	  for(int i=0;i<commands.size();i++)
	    std::cout << commands.at(i)<< ": "<<descriptions.at(i)<<std::endl;
	}
      else if(input == "connect")
	{
	  if(!connected)
	    {
	      connect();
	    }
	  else
	    std::cout << "You already appear to be connected\n";
	}
      else if(input == "disconnect")
	{
	  if(readingLog)
	    {
	      disconnectFromLog();
	    }
	  if(connected)
	    {
	      disconnect();
	    }
	  else
	    std::cout << "You already appear to be disconnected\n";
	}
       else if(input == "start_log")
	{
	  if(!readingLog)
	    {
	      connectToLog();
	    }
	  else
	    std::cout << "You already appear to be connected to the log\n";
	}
       else if(input == "stop_log")
	{
	  if(readingLog)
	    {
	      disconnectFromLog();
	    }
	  else
	    std::cout << "You don't appear to be connected to the log\n";
	}  
       else if(input == "shut_down")
	 {
	   if(readingLog)
	     {
	       disconnectFromLog();
	     }
	   shutDown();
	   disconnect();
	 } 
       else if(input.substr(0,13) == "delete_sheet_")
	{
	  if(connected)
	    {
	      std::string name = input.substr(13);

	    }
	  else
	    std::cout << "You don't appear to be connected\n";
	} 
else if(input.substr(0,12) == "delete_user_")
	{
	  if(connected)
	    {
	      std::string name = input.substr(12);

	    }
	  else
	    std::cout << "You don't appear to be connected\n";
	} 
       else
	 {
	   std::cout << "Command not recognized, type \"help\" for a list of commands.\n";
	 }
    }
}

void connect()
{
  std::cout << "Connecting...\n";

  connected = true;

  std::cout << "Connected\n";
}

void connectToLog()
{
  std::cout << "Connecting to the log...\n";

  readingLog = true;

  std::cout << "Connected to the log\n";
}

void disconnect()
{
  std::cout << "Disconnecting...\n";

  connected = false;

  std::cout << "Disconnected\n";
}

void disconnectFromLog()
{
  std::cout << "Disconnecting from the log...\n";

  readingLog = false;

  std::cout << "Disconnected from the log\n";
}

void shutDown()
{
  std::cout << "Shutting down...\n";

  std::cout<< "Finished shutting down\n";
}
