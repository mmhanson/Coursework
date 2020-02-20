#include <nlohmann/json.hpp>
#include <iostream>

using json = nlohmann::json;

int main(int argc, char **argv)
{
  json username = json::array({{"username","pajensen"},{"password","doofus"}});

  //Serialize the object
  std::cout << username<<'\n';
}
