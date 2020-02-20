#include "networking.h"
#include <iostream>
#include <cstdio>
#include <fstream>
#include <map>
#include <set>
#include <vector>
#include <regex>
#include <boost/property_tree/ptree.hpp>
#include <boost/property_tree/json_parser.hpp>
#include <boost/foreach.hpp>
#include <dirent.h>


using boost::property_tree::ptree;
using boost::property_tree::read_json;
using boost::property_tree::write_json;

class SpreadsheetServer
{

private:
  std::map<Spreadsheet*, std::set<client> > clientGroups;
  std::map<std::string, std::string> passwords;
  std::set<client> admins;

  /*
   * Closes sockets and shuts down the server
   */
  void ShutdownServer()
  {
    
    std::map<Spreadsheet*, std::set<client> >::iterator it;
    std::cout << "Client size: " << clientGroups.size() << std::endl;
    for(it = clientGroups.begin(); it != clientGroups.end(); it++){
      std::cout << "Inside for loop" << std::endl;
      std::set<client> clients = it->second;
      std::set<client>::iterator it_c;
      for(it_c = clients.begin(); it_c != clients.end(); it_c++){
	(*it_c)->socket_.shutdown(boost::asio::ip::tcp::socket::shutdown_both);
	(*it_c)->socket_.close();
      }
    }
    exit(0);
    
  }
  
  /*
   * Reads the password file to fill the password map with
   * username-password pairs for password validation
   */
  void FillPasswordMap()
  {
    std::ifstream ifile;
    std::string username, password;
    ifile.open("passwords.txt");
    while(true){
      ifile >> username;
      ifile >> password;
      passwords.insert(std::pair<std::string, std::string>(username, password));
      if(ifile.eof())
	break;
    }
    ifile.close();
  }

  /*
   * Returns a formatted JSON string of all clients and the sheets
   * they are connected to
   */
  std::string GetClientList()
  {
    std::string clientList = "{ \"type\": \"clientlist\",\"list\": { ";

    std::map<Spreadsheet*, std::set<client> >::iterator it;
    for(it = clientGroups.begin(); it != clientGroups.end(); it++){
      std::string name = (it->first)->name;
      std::set<client> clients = it->second;
      std::set<client>::iterator it_c;
      for(it_c = clients.begin(); it_c != clients.end(); it_c++){
	if((*it_c)->connected){
	  std::string username = (*it_c)->username;
	  clientList += "\"" + username + "\": \"" + name + "\", ";
	}
      }
    }

    //Remove the last space and comma
    clientList.pop_back();
    clientList.pop_back();
    clientList += "} }\n\n";

    return clientList;
  }

  void SendClientList()
  {
    if(admins.size() == 0)
      return;

    std::string clientList = GetClientList();
    std::set<client>::iterator it;
    for(it = admins.begin(); it != admins.end(); it++){
      Networking::SendData(*it, clientList);
    }
  }

  void SendUpdatesToAdmin(std::string username, std::string status, std::string cell)
  {
    std::string message = "{ \"type\": \"update\", \"username\": \"" + username + "\",";
    message += "\"status\": \"" + status + "\"," + "\"cell\": \"" + cell + "\" }\n\n";
    std::set<client>::iterator it;
    for(it = admins.begin(); it != admins.end(); it++){
      Networking::SendData(*it, message);
    }
  }

  /*
   * Given a sheet name and a command type, either creates or deletes
   * a sheet from the system
   */ 
  void ManageSheet(std::string type, std::string sheetName)
  {
    std::string path = "spreadsheets/" + sheetName + ".xml";
    const char* cPath = path.c_str();

    if(type == "create"){
      std::ofstream file;
      file.open(path);
      file.close();
    }
    else if(type == "delete"){
      std::remove(cPath);
    }
  }

  /*
   * Helper method to save the password map to disk
   */
  void RewritePasswords()
  {
    std::map<std::string, std::string>::iterator it;    
    std::ofstream outfile;
    outfile.open("passwords.txt");
    for(it = passwords.begin(); it != passwords.end(); it++){
      outfile << it->first << " " << it->second << std::endl;
    }
    outfile.close();
  }

  /*
   * Given a username and a type of command, either creates
   * or deletes the given user
   */
  void ManageClient(std::string type, std::string username)
  {
    std::map<std::string, std::string>::iterator it = passwords.find(username);

    if(type == "create"){
      if(it == passwords.end()){
	std::ofstream outfile;
	passwords.insert(std::pair<std::string, std::string>(username, "default"));
	outfile.open("passwords.txt", std::ios_base::app);
	outfile << username << " default"  << std::endl;
	outfile.close();
      }
    }
    else if(type == "delete"){
      if(it != passwords.end()){
	passwords.erase(it);
	RewritePasswords();
      }
    }
  }

  /*
   * Given a username and password parameter, updates the password
   * of the user
   */
  void ChangePassword(std::string name, std::string pw)
  {
    std::map<std::string, std::string>::iterator it = passwords.find(name);
    if(it != passwords.end())
      it->second = pw;
    RewritePasswords();
  }
  
  /*
   * Generates the correctly formatted JSON message for an error,
   * given the correct error code and source cell.
   */
  std::string GetErrorMessage(int code, std::string source)
  {
    ptree pt;
    pt.put("type","error");
    pt.put("code", code);
    pt.put("source",source);

    std::stringstream oss;
    write_json(oss,pt);
    std::string serialized(oss.str());

    return serialized;
  }

  /*
   * Generates the correctly formatted JSON message for changes in the
   * sheet. Although the type is "full send", it only sends the most recent
   * change.
   */ 
  std::string GetDeltaMessage(std::string name, std::string contents)
  {
    ptree pt;
    pt.put("type","full send");
    
    ptree cells;
    cells.put(name, contents);

    pt.add_child("spreadsheet", cells);
    
    std::stringstream oss;
    write_json(oss,pt);
    std::string serialized(oss.str());

    return serialized;
  }

  /*
   * Given a JSON ptree, returns the string value of the given
   * property if it exists, and a blank string otherwise
   */
  std::string GetJSONProperty(std::string property, ptree& pt)
  {
    ptree::const_assoc_iterator it = pt.find(property);
    if(it == pt.not_found()){
      return "";
    }
    else
      return pt.get<std::string>(property);
  }

  /*
   * Pulls messages off the buffer of a client, splitting on a double
   * newline.  Returns a vector of all such complete messages
   */
  std::vector<std::string> ProcessMessage(client conn)
  {
    std::vector<std::string> messages;
    std::string totalData = conn->message_;

    size_t current = 0;
    size_t next = std::string::npos;

    do{
      next = totalData.find("\n\n",current);
      if(next < totalData.length()){
	messages.push_back(totalData.substr(current, next-current));
	(conn->message_).erase(0,next+2);
	current = next+2;
      }
    } while (next != std::string::npos);

    return messages;
  }
  
  /*
   * Takes a client and a message as parameters and sends the message
   * to every other client on the same sheet
   */
  void SendUpdates(client conn, std::string message)
  {
    Spreadsheet* group = conn->sheet;

    std::map<Spreadsheet*, std::set<client> >::iterator test = clientGroups.find(group);
    if(test == clientGroups.end())
      return;
          
    std::set<client>::iterator it;
    for(it=clientGroups[group].begin(); it != clientGroups[group].end();it++){
      if((*it)->connected)
	Networking::SendData(*it, message);
    }     
  }

  /*
   * Parses a string into a JSON ptree. Returns true if successful
   */ 
  bool ParseJSON(ptree& pt, std::string message)
  {
    std::istringstream is(message);
    try{
      read_json(is, pt);
    }
    catch(std::exception& e){
      std::cerr << "Invalid JSON message" << std::endl;
      return false;
    }
    return true;
  }

  /*
   * Parses JSON message from client and responds according to the
   * type of message called. Handles both client and administrative
   * messages.
   */
  std::string ProcessRequest(client conn, std::string message)
  {
    std::cout << "Process Request: " << message << std::endl;
    std::string username = conn->username;
    ptree pt;
    if(!ParseJSON(pt, message))
      return "";
    std::string type = GetJSONProperty("type", pt);
    std::cout << "Type: " << type << std::endl;
    if(type == "edit"){
      std::string cellName = GetJSONProperty("cell",pt);
      std::string value = GetJSONProperty("value", pt);
      std::vector<std::string> dependencies;

      ptree::const_assoc_iterator it = pt.find("dependencies");
      if(it == pt.not_found()){
	return "";
      }

      BOOST_FOREACH(ptree::value_type& dep, pt.get_child("dependencies")){
	dependencies.push_back(dep.second.get_value<std::string>());
      }
 
      if((conn->sheet)->edit(cellName, value, dependencies)){
	SendUpdatesToAdmin(username, "edit", cellName);
	(conn->sheet)->write();
	return GetDeltaMessage(cellName, value);
      }
      else
	return GetErrorMessage(2, cellName);
    }
    else if(type == "undo"){
      Cell* c = (conn->sheet)->undo();
      if(c != NULL){
	SendUpdatesToAdmin(username, "undo", "");
	return GetDeltaMessage(c->name, c->contents);
      }
      return "Invalid";
    }
    else if(type == "revert"){
      std::string cellName = GetJSONProperty("cell", pt);
      Cell* c = (conn->sheet)->revert(cellName);
      if(c != NULL){
	SendUpdatesToAdmin(username, "revert", cellName);
	return GetDeltaMessage(c->name, c->contents);
      }
      return "Invalid";
    }
    else if(type == "managesheet"){
      std::string action = GetJSONProperty("action", pt);
      std::string name = GetJSONProperty("spreadsheet",pt);
      ManageSheet(action, name);
      return "managed";
    }
    else if(type == "manageclient"){
      std::string action = GetJSONProperty("action", pt);
      std::string name = GetJSONProperty("username",pt);
      ManageClient(action, name);
      return "managed";
    }
    else if(type == "change"){
      std::string username = GetJSONProperty("username", pt);
      std::string pw = GetJSONProperty("newpw",pt);
      ChangePassword(username, pw);
      return "managed";
    }
    else if(type == "shutdown"){
      ShutdownServer();
      return "";
    }
    else{
      return "";
    }
  }

  /*
   * Callback method that fires when data comes in from a client
   * Pulls messages from the buffer using ProcessMessage and then
   * parses and responds appropriately using ProcessRequest
   */
  void ReceiveMessages(client conn)
  {
    std::cout << "Receiving messages" << std::endl;
    std::vector<std::string> messages = ProcessMessage(conn);
    std::vector<std::string>::iterator it;
    for(it = messages.begin(); it != messages.end(); it++){
      std::string outMessage = ProcessRequest(conn, *it);
      if(outMessage == ""){
	conn->connected = false;
	return;
      }
      else if(outMessage == "managed") {}
      else if(outMessage != "Invalid")
	SendUpdates(conn, outMessage);
    }
    Networking::GetData(conn);
  }

  /*
   * Method to send the entire requested spreadsheet to the client
   * Called after a successful open request
   */
  void SendSpreadsheet(client conn, Spreadsheet* sheet)
  {
    std::string sheetStr = (conn->sheet)->toString() + "\n\n";
    clientGroups[sheet].insert(conn);
    conn->sheetName = sheet->name;
    Networking::SendData(conn, sheetStr);
    conn->CallMe = boost::bind(&SpreadsheetServer::ReceiveMessages, this, _1);
    Networking::GetData(conn);
  }

  /*
   * Helper method to validate a password for a given username
   */
  bool ValidatePassword(std::string username, std::string password)
  {
    std::map<std::string, std::string>::iterator it = passwords.find(username);
    if(it != passwords.end()){
      if(it->second == password)
	return true;
      else 
	return false;
    }
    else{
      passwords.insert(std::pair<std::string, std::string>(username, password));
      std::ofstream outfile;
      outfile.open("passwords.txt", std::ios_base::app);
      outfile << username << " " << password << std::endl;
      outfile.close();
      return true;
    }
  }

  /*
   * Handle the client request of type "open"
   * Parses the JSON message, validates the user, and adds them to the appropriate spreadsheet
   */
  void ProcessOpenRequest(client conn)
  {
    std::cout << "Processing Open Request" << std::endl;
    std::vector<std::string> messages = ProcessMessage(conn);
    std::string name, type, username, password;
    bool isValidJSON = true;

    if(messages.size() != 1){
      std::cout << "Invalid message received from client" << std::endl;
      return;
    }

    std::cout << messages[0] << std::endl;
    ptree pt;
    std::istringstream is(messages[0]);
    try{
      read_json(is, pt);
    }
    catch(std::exception& e){
      std::cerr << "Invalid JSON message" << std::endl;
      return;
    }

    type = GetJSONProperty("type",pt);
    if(type == "admin connect"){
      admins.insert(conn);
      std::string clientList = GetClientList();
      Networking::SendData(conn, clientList);
      conn->CallMe = boost::bind(&SpreadsheetServer::ReceiveMessages, this, _1);
      Networking::GetData(conn);
      return;
    }

    name = GetJSONProperty("name",pt);
    username = GetJSONProperty("username",pt);
    password = GetJSONProperty("password",pt);

    std::cout << "Type: " << type << std::endl;

    if(type != "open"){
      std::cout << "Unexpected message type" << std::endl;
      return;
    }

    if(name == ""  || type == "" || username == "" || password == "")
      isValidJSON = false;

    if(isValidJSON){
      if(!SpreadsheetServer::ValidatePassword(username, password)){
	std::string error = GetErrorMessage(1,"");
	Networking::SendData(conn, error);
	SpreadsheetServer::GotContact(conn);
	return;
      }

      std::cout << "About to loop. Size: " << clientGroups.size() << std::endl;
      bool openSheet = false;
      std::map<Spreadsheet*, std::set<client> >::iterator it;
      std::cout << "Made the iterator" << std::endl;
      for(it = clientGroups.begin(); it != clientGroups.end(); it++){
	std::cout << "Made it inside of loop" << std::endl;
	std::cout << "In loop: " << (it->first)->name << std::endl;
	if((it->first)->name == name){
	  conn->sheet = (it->first);
	  openSheet = true;
	}
      }

      std::cout << "Finished loop" << std::endl;

      if(!openSheet)
	conn->sheet = new Spreadsheet(name);
      conn->connected = true;
      conn->username = username;

      SpreadsheetServer::SendSpreadsheet(conn, conn->sheet);

      SendClientList();
    }
    else{
      std::cout << "Open message was invalid" << std::endl;
      return;
    }
  }

  /*
   * Helper method to return a JSON formatted string containing all of the available spreadsheets
   */
  std::string GetSheets()
  {
    ptree pt;
    pt.put("type","list");

    ptree sheets;
    DIR* dirp;
    struct dirent* directory;

    dirp = opendir("spreadsheets");
    if(dirp){
      while((directory = readdir(dirp)) != NULL){
	std::string name = directory->d_name;
	if(name != "." && name != ".."){
	  if(name.find("#") == std::string::npos && name.find("~") == std::string::npos){
	    name.erase(name.find(".xml"),4);
	    ptree sheet;
	    sheet.put("",name);
	    sheets.push_back(std::make_pair("",sheet));
	  }
	}
      }
      closedir(dirp);
    }

    pt.add_child("spreadsheets", sheets);
    
    std::stringstream oss;
    write_json(oss,pt);
    std::string serialized(oss.str());

    serialized.erase(std::remove(serialized.begin(), serialized.end(), '\n'),serialized.end());
    
    return serialized;
  }

  /*
   * Callback method for when a client connects to the server.  Takes in a TcpConnection pointer
   */
  void GotContact(client conn)
  {
    std::cout << "Got contact" << std::endl;

    std::string sheets = SpreadsheetServer::GetSheets() +"\n\n";

    Networking::SendData(conn, sheets);
    conn->CallMe = boost::bind(&SpreadsheetServer::ProcessOpenRequest, this, _1);
    Networking::GetData(conn);
  }

  public: SpreadsheetServer()
  {
  }

  /*
   * Starts a TcpListener on a loop to wait for client contact,
   * sets GotContact method as a callback on connection
   */
  void StartServer()
  {
    std::cout << "Server waiting for client" << std::endl;

    FillPasswordMap();

    networkAction connectedCallback = 
      boost::bind(&SpreadsheetServer::GotContact, this, _1);

    Networking::ServerAwaitingClientLoop(connectedCallback);
  }

};

int main(int argc, char* argv[])
{
  SpreadsheetServer server = SpreadsheetServer();

  server.StartServer();
  return 0;
}


