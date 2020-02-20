/**
 *
 * A program to send and recieve strings over a network, functioning as the network controller in the MVC architecture.
 * Written by Maxwell Hanson and Colin Dunn, April 2019
 *
 */

#include <boost/asio.hpp>
#include <boost/function.hpp>
#include <boost/bind.hpp>
#include <iostream>

using boost::asio::ip::tcp;
using namespace std;
typedef boost::function<void()> network_action; // TODO: modify to pass in socket state


class network_controller {

  public:

    //
    // Construct a network controller.
    // 
    // inform_client: invoked when 
    // io_context: the io context to use for asynchronous method calls
    //
    network_controller(network_action inform_client, boost::asio::io_context &io_context):
      io_ctx(io_context), tcp_acceptor(io_ctx, tcp::endpoint(tcp::v4(), 2112)) {
      // initializer copies io context reference and creates an acceptor listening for ipv4 conns on 2112
      this->inform_client = inform_client;
    }

    //
    // Asynchronously start listening for and accepting new clients on the default port.
    //
    // Once a client is accepted, the 'inform_client' function given at construction is invoked
    // 
    void listen() {
      // NOTE there may be issues with 'listen socket' being local not dynamic
      cout << "start listen func" << endl;
      
      // a tcp acceptor listening for ipv4 connections on port 2112
      tcp::socket listen_socket(this->io_ctx);
      // binding done to pipe client-supplied callback to the handler
      boost::function<void(const boost::system::error_code &error)> accept_callback = 
        boost::bind(&network_controller::handle_client_accepted, this, this->inform_client, _1);
      this->tcp_acceptor.async_accept(listen_socket, accept_callback);

      cout << "end listen func" << endl;
    }

    private:

    //
    // Handle a new client trying to connect.
    // 
    // error: client was successfully accepted if no error here
    //
    void handle_client_accepted(network_action conn_callback, const boost::system::error_code &error) {
      cout << "start handler" << endl;

      conn_callback();

      cout << "end handler" << endl;
      return;
    }

    boost::asio::io_context &io_ctx;
    tcp::acceptor tcp_acceptor;
    network_action inform_client;
};
