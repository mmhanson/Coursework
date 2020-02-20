# admin_client_final_project

To connect the client will make a TCP connection request to the server. To connect, the following message will be sent to the server after the spreadsheets are sent to the admin-client. 

{
  “Type”: “admin connect”
}

After receiving this message, the server should set apart the connection to the admin client in a special variable for use later and immediately send the list of active clients. Whenever a non-admin client connects or disconnects from the server (or is removed), the server should send out the full client list as specified below. Whenever an update happens on the server (edit, undo, revert), the “update” message is sent to the admin-client. The server should expect to receive any of the admin -> server messages at anytime and act appropriately. 

Each message is terminated by "\n\n". All communication take place on port 2112.

Server ---------------> Admin
Message Type
Message Example
Description

“clientlist”
{
 “type”: “clientlist”,
 “list”: {
   ”pajensen”: “spreadsheet 1”,
   “Keenan”: “spreadsheet 2”
  }
}

Anytime a client connects or disconnects, the server will send the admin-client a list of all active clients as a dictionary mapped to the spreadsheets they are on. This message will also be sent when an admin client connects. 

“update”
{
“type”: “update”,
“username”:”pajensen”,
“status”: ”edit”
“cell”: “A1”
}

Server will tell admin in real time what actions are taking place as an update. 
 

 
Admin ---------------> Server
Message Type
Message Example
Description

“managesheet”
{
 “type” : “managesheet”,
 “action”: “create”,
 “spreadsheet”: “sheet 1”
}

Sends either the action “create” or “delete depending on what action the server should take on the specified spreadsheet.

“manageclient”
{
 “type” : “manageclient”,
 “action”: “create”,
 “username”: “pajenson”
}

Sends either the action “create” or “delete depending on what action the server should take.

“change”
{
“type”: “change”,
“username”:”pajensen”,
“newpw”:”234"
}

To be send when a client's password needs changing. 

“shutdown”
{
“type”: “shutdown”
}

Admin have the ability to ask server to shut down, after that admin will clean all users and spreadsheets stored in the admin client.


