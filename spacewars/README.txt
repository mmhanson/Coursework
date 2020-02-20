## Notes

This program is a recreation of the 1977 arcade game 'Space Wars' in C# with full multiplayer support.

I worked on this project in a team of 2, including myself, for the CS 3500 (Software Engineering I) class at the University of Utah in Fall 2018.

## =====================
This spacewars solution was developed by Maxwell Hanson (u0985911)
and Joshua Cragun (). We followed the prescribed MVC architecture
to the best of our abilities. The view does include a reference to
the model, however it does not modify or construct it in any way.
This is left to the controller.

Quickstart
To start our spacewars client, compile the solution and start the
GameForm project. This will bring up the connection screen. Enter
the address of the server and your name and, optionally, select
a custom skin to upload. The lattermost feature is our chosen 
"extra polish" described in the assignment. The user can choose
any image to display in place of their ship. The image will be
scaled to 35x35 pixels like all the ships, but it can be anything.
Of course, all the other clients will not see this image, only
this client will see it.
Once you connect, the world and scoreboard will show up. The controls
are:
W, Up: thrust
A, Left: turn left
D, Right: turn right
Space, S, Down: fire

Testing
The 'Testing' project in our solution contains some miscellaneous classes.
Throughout development, several classes were put here such as
TestNetworkController and TestController. These classes were useful
for the development of prototype-stage projects, but are not used in the
final product you see now. This project was also intended to contain
unit tests, but as of now we did not get around to that.

Special Notes
Though our resources project contains the 'OldSpacewarsLibraries'
executables, these are never used in the project. My partner and I
decided to just put them in the solution to keep things all together
for future reference.
