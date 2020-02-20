//
// chat_server.cpp
// ~~~~~~~~~~~~~~~
//
// Copyright (c) 2003-2013 Christopher M. Kohlhoff (chris at kohlhoff dot com)
//
// Distributed under the Boost Software License, Version 1.0. (See accompanying
// file LICENSE_1_0.txt or copy at http://www.boost.org/LICENSE_1_0.txt)
//

#include <cstdlib>
#include <deque>
#include <iostream>
#include <list>
#include <memory>
#include <set>
#include <utility>
#include <boost/bind.hpp>
#include <boost/asio.hpp>
#include <vector>

using boost::asio::ip::tcp;

//----------------------------------------------------------------------

typedef std::deque<std::string> message_queue;



//----------------------------------------------------------------------

class client
{
public:
  virtual ~client() {}
  virtual void deliver(std::string& msg) = 0;
};

typedef std::shared_ptr<client> client_ptr;

//----------------------------------------------------------------------

class clientGroup
{
public:
  void join(client_ptr participant)
  {
    participants_.insert(participant);
  }

  void leave(client_ptr participant)
  {
    participants_.erase(participant);
  }

  void deliver(std::string& msg)
  {
    for (auto participant: participants_)
      participant->deliver(msg);
  }

private:
  std::set<client_ptr> participants_;
//enum { max_recent_msgs = 100 };
//message_queue recent_msgs_;
};

std::vector<clientGroup*> sheets;

//----------------------------------------------------------------------

class tcpConnection
  : public client,
    public std::enable_shared_from_this<tcpConnection>
{
public:
   tcpConnection(tcp::socket socket)
    : socket_(std::move(socket))
      // ,group_(NULL)           //, clientGroup& group)
  {
  }

  void start()
  {
group_ = sheets[0];
group_->join(shared_from_this());
start_read();
//getSheet();
  }

  void deliver(std::string& msg)
  {
    do_write(msg);
  }

private:

  void getSheet()
  {
    boost::asio::async_read(socket_, input_buffer_, boost::asio::transfer_at_least(1),
			    boost::bind(&tcpConnection::assignGroup, this,
					boost::asio::placeholders::error));
  }

  bool isValidSheet(const std::string s)
  {
    return !s.empty() && std::find_if(s.begin(), 
      s.end(), [](char c) { return !std::isdigit(c); }) == s.end();
  }

  void assignGroup(const boost::system::error_code& ec)
  {
    if(!ec){
      std::string request;
      std::istream is(&request_buffer_);
      std::getline(is, request);

      if(!request.empty()){
	std::cout << "Request group: " << request << std::endl;
        if(isValidSheet(request)){
          group_ = sheets[0];
          group_->join(shared_from_this());
          start_read();
        }
      }
    }
    else{
      std::cout << "Error receiving group: " << ec.message() << std::endl;
    }
  }


  void start_read()
  {
    boost::asio::async_read(socket_, input_buffer_, boost::asio::transfer_at_least(1),
			    boost::bind(&tcpConnection::handle_read, this,
					boost::asio::placeholders::error));
  }

  void handle_read(const boost::system::error_code& ec)
  {
    if(!ec){
      std::string line;
      std::istream is(&input_buffer_);
      std::getline(is, line);

      if(!line.empty()){
	messageFromClient_ = line;
        group_->deliver(messageFromClient_);
	std::cout << "Received: " << line << std::endl;
      }

      start_read();
    }
    else{
      group_->leave(shared_from_this());
      std::cout << "Error on receive: " << ec.message() << std::endl;
    }
  }

  void do_write(std::string& msg)
  {
    writeMessage_ = msg + "\n";
    auto self(shared_from_this());
    boost::asio::async_write(socket_,
        boost::asio::buffer(writeMessage_),
        [this, self](boost::system::error_code ec, std::size_t )
        {
          if(ec)
          {
            group_->leave(shared_from_this());
          }
        });
  }

  tcp::socket socket_;
  clientGroup* group_;
  boost::asio::streambuf request_buffer_;
  boost::asio::streambuf input_buffer_;
  std::string messageFromClient_;
  std::string writeMessage_;
};

//----------------------------------------------------------------------

class tcp_server
{
public:
  tcp_server(boost::asio::io_service& io_service)
    : acceptor_(io_service, tcp::endpoint(tcp::v4(),2112)),
      socket_(io_service)
  {
    start_accept();
  }

private:

  void start_accept()
  {
    sheets.push_back(&group_);
    do_accept();
  }


  void do_accept()
  {
    acceptor_.async_accept(socket_,
        [this](boost::system::error_code ec)
        {
          if (!ec)
          {
            //std::make_shared<tcpConnection>(std::move(socket_), group_)->start();
            std::make_shared<tcpConnection>(std::move(socket_))->start();
          }

          do_accept();
        });
  }

  tcp::acceptor acceptor_;
  tcp::socket socket_;
  clientGroup group_;
};

//----------------------------------------------------------------------

int main(int argc, char* argv[])
{
  try
  {
    boost::asio::io_service io_service;
    tcp_server server(io_service);
    io_service.run();
  }
  catch (std::exception& e)
  {
    std::cerr << "Exception: " << e.what() << "\n";
  }

  return 0;
}
