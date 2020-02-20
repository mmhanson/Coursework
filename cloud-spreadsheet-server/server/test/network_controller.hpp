#include <boost/function:

typedef boost::function<void()> network_action;

class network_controller {
  network_controller(network_action inform_client);
