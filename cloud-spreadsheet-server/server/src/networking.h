#include <ctime>
#include <iostream>
#include <string>
#include <boost/bind.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/enable_shared_from_this.hpp>
#include <boost/asio.hpp>
#include <boost/function.hpp>
#include "Spreadsheet.h"

using boost::asio::ip::tcp;

class tcpConnection;
typedef boost::shared_ptr<tcpConnection> client;

typedef boost::function<void(client)> networkAction;

class tcpConnection
  : public boost::enable_shared_from_this<tcpConnection>
{
public:
  typedef boost::shared_ptr<tcpConnection> client;

  static client create(boost::asio::io_service& io_service);

  void start();

  tcpConnection(boost::asio::io_service& io_service);

  void handleWrite(const boost::system::error_code& /*error*/,
		    size_t /*bytes_transferred*/);


  void startRead();

  void handleRead(const boost::system::error_code&);

  void startWrite(std::string data);


  tcp::socket socket_;
  std::string message_;
  boost::asio::streambuf input_buffer_;
  boost::asio::streambuf outputBuffer_;
  bool connected;
  networkAction CallMe;
  std::string sheetName;
  Spreadsheet* sheet; 
  std::string username;
};

class TcpListener
{
public:
  TcpListener(boost::asio::io_service& io_service, networkAction);
  void CloseListener();

private:
  void startAccept();

  void handleAccept(tcpConnection::client, const boost::system::error_code&);

  tcp::acceptor acceptor_;
  boost::asio::io_service ioService;
  networkAction callback;
};

class Networking
{
 public: 
  static void ServerAwaitingClientLoop(networkAction);

  static void GetData(client);

  static void SendData(client, std::string);

  static void ShutdownServer(TcpListener*);

};

