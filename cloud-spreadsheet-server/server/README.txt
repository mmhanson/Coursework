This is the server portion of the spreadsheet final project.

## How to build
### Install boost locally
1. Download boost 1.69: `wget "https://dl.bintray.com/boostorg/release/1.69.0/source/boost_1_69_0.zip"`

2. Unzip boost: `unzip boost_1_69_0.zip`

3. Configure and install boost: `cd boost_1_69_0 && ./bootstrap.sh --prefix=/home/`whoami`/usr && ./b2`

### Make the server
1. `make all`


The server consists of a model and a controller. The controller is split into two parts, the networking code
and the server code. Networking code is located in 'networking.h' and includes logic to listen for connections,
accept connections, and send/receive string-form information from clients. The networking code here functions
very similarly to the network controller from SpaceWars. Server code is located in 'ss_server.cpp', this is
the main program for the server. Here is all the core logic for starting the server, receiving messages, parsing
json, authenticating users, etc...

The model consists of the 'Spreadsheet.h', 'Graph.h', and 'Cell.h' classes. The spreadsheet class is the only
class that needs to be accessed by the Server class, the graph and cell classes are managed by the spreadsheet
class. Here is the logic for reading/writing spreadsheets, detecting circular dependencies, managing cell
history, etc...

To build the project, boost must be installed locally. See the makefile for specific instructions on how to
do so.
