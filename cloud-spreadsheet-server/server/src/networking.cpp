#include "networking.h"

using boost::asio::ip::tcp;

std::string make_daytime_string()
{
  using namespace std; // For time_t, time and ctime;
  time_t now = time(0);
  return ctime(&now);
}

client tcpConnection::create(boost::asio::io_service& io_service)
{
  return client(new tcpConnection(io_service));
}

void tcpConnection::startRead()
{
  //std::cout << "StartRead()" << std::endl;
  boost::asio::async_read(socket_, input_buffer_, boost::asio::transfer_at_least(1),
        boost::bind(&tcpConnection::handleRead, shared_from_this(), _1));
  //std::cout << "Finished StartRead" << std::endl;
}

void tcpConnection::handleRead(const boost::system::error_code& ec)
{
  //std::cout << "HandleRead()" << std::endl;
  if(!ec){
    
    std::istreambuf_iterator<char> eos;
    std::istream is(&input_buffer_);
    std::string data(std::istreambuf_iterator<char>(is),eos);

    if(data.length() > 0){
      message_ += data;
      CallMe(shared_from_this());
    }
    
  }
  else{
    std::cout << "Error on receive: " << ec.message() << std::endl;
    this->connected = false;
  }
  //std::cout << "Finished HandleRead()" << std::endl;
}

void tcpConnection::startWrite(std::string data)
{
  boost::asio::async_write(socket_, boost::asio::buffer(data, data.length()),
        boost::bind(&tcpConnection::handleWrite, shared_from_this(),
          boost::asio::placeholders::error,
          boost::asio::placeholders::bytes_transferred));
}


  void tcpConnection::start()
  {
    /*
    //message_ = make_daytime_string();
    message_ = "THIS IS ONLY A TEST\n";

    boost::asio::async_write(socket_, boost::asio::buffer(message_),
        boost::bind(&tcpConnection::handleWrite, shared_from_this(),
          boost::asio::placeholders::error,
          boost::asio::placeholders::bytes_transferred));
    */
  }


  tcpConnection::tcpConnection(boost::asio::io_service& io_service)
    : socket_(io_service)
  {
  }

  void tcpConnection::handleWrite(const boost::system::error_code& ec,
      size_t bytes)
  {
    if(!ec){
      //std::cout << "Handlewrite()" << std::endl;
      //CallMe(shared_from_this());
    }
  }


TcpListener::TcpListener(boost::asio::io_service& io_service, networkAction connectedCallback)
  : acceptor_(io_service, tcp::endpoint(tcp::v4(), 2112))
{
  this->callback = connectedCallback;
  startAccept();
}

void TcpListener::CloseListener()
{

}

void TcpListener::startAccept()
{    
  tcpConnection::client new_connection =
    tcpConnection::create(acceptor_.get_io_service());

  acceptor_.async_accept(new_connection->socket_,
    boost::bind(&TcpListener::handleAccept, this, new_connection,
      boost::asio::placeholders::error));
}

void TcpListener::handleAccept(tcpConnection::client new_connection,
   const boost::system::error_code& error)
{
  if (!error)
  {
    new_connection->start();
    callback(new_connection);
    startAccept();
  }
}

void Networking::ServerAwaitingClientLoop(networkAction connectedCallback)
{
  std::cout << "Starting the client loop" << std::endl;
  try
  {
    boost::asio::io_service io_service;
    TcpListener* server = new TcpListener(io_service, connectedCallback);
    io_service.run();
  }
  catch (std::exception& e)
  {
    std::cerr << e.what() << std::endl;
  }
}

void Networking::GetData(client conn)
{
  //read data
  std::cout << "Get Data" << std::endl;
  conn->startRead();
  
}

void Networking::SendData(client conn, std::string data)
{
  std::cout << "Sending Data" << std::endl;
  conn->startWrite(data);
}

void Networking::ShutdownServer(TcpListener* listener)
{

}
