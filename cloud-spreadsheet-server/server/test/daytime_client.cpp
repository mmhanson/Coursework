#include <iostream>
#include <boost/asio.hpp>
#include <boost/array.hpp>

using std::cout;
using boost::asio::ip::tcp;
using namespace std;


int main(int argc, char **argv) {
  // assumed argv[1] is the ??

  // interface with the OS io services
  boost::asio::io_context io_context;

  tcp::resolver resolver(io_context);
  // TODO what does this do?
  tcp::resolver::results_type endpoints = resolver.resolve("localhost", "2112");

  tcp::socket socket(io_context);

  cout << "connecting..." << endl;

  boost::asio::connect(socket, endpoints);

  cout << "connected" << endl;

  while (true) {
    boost::array<char, 128> buffer;
    boost::system::error_code error;

    size_t msg_len = socket.read_some(boost::asio::buffer(buffer), error);

    if (error == boost::asio::error::eof) {
      // peer cleanly exited
      break;
    }
    else if (error) {
      // some other error occurred
      throw boost::system::system_error(error);
    }

    // write only the length of the message to stdout
    cout.write(buffer.data(), msg_len);
  }
}
