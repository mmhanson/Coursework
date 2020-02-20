#include "network_controller.cpp"
#include <boost/asio.hpp>
#include <iostream>

using namespace std;

void inform_client() {
  cout << "informed client" << endl;
}


int main(int argc, char **argv) {
  cout << "start main" << endl;

  boost::asio::io_context io_ctx;
  network_controller net_conn(inform_client, io_ctx);
  cout << "before listen call in main" << endl;
  net_conn.listen();
  cout << "after listen call in main" << endl;
  io_ctx.run();
  
  cout << "end main" << endl;

  return 0;
}
