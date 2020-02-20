This spacewars solution was developed by Maxwell Hanson (u0985911)
and Joshua Cragun (u1025691). We followed the prescribed MVC architecture
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

======= Custom Ship =======
We went through the efforts of adding our own special feature! In 
our version of SpaceWars, you can add your own custom skin locally
if you so desire. To do so is simple; simply click the "upload skin"
button and upload a bitmap file of your chosing. If you don't, the game will ask if
you want upload one, since you can't change your appearance in the middle
of a game. Further, inside the of the "Images" folder in the "Resources"
directory, we've provided three different custom skins to choose from
if you'd like to experiment with this feature but don't have any bitmaps of
your own on hand.

===========
=== PS8 ===
===========
Server:
The added server functionality is centered around the Server project, as mandated by the
assignment. In this project there is a Server class. This is the class that is run when
a user starts the server. It will create an instance of itself and update itself on an
infinite loop. On a high level, it will accept clients as they come and periodically
send information about the world to each connected client. Clients are continuously
listened for and accepted, but the world is sent out on a schedule.
When the server is created, it reads a couple XML settings from an xml file named
'settings.xml' in the base of the Resources project. More details about this settings file
can be found in the 'XML' section. The important thing to note is the server determines
which gamemode to use depending on this file. To control the two gamemodes, two models
are used. One is used for one gamemode, and the other is used for the other gamemode.
For more information abou the custom gamemode, see the Custom gamemode section.

Model:
The model is created and updated in the server. The model itself handles the mechanical
details of exactly what to do with each entity while the server takes input from clients
and sends it down to the model. The API of the model is defined in the IWorld interface.
This interface is implemented to create a model for each gamemode. The two models are are
the SpaceWars model of the SpaceWarsWorld class and the SpaceInvaders model of the
SpaceInvadersWorld class. See the IWorld interface for details about the API and see
each model for details about how they work.

=== Custom Gamemode ===

Overview:
For my custom gamemode I created an implementation of 'SpaceInvaders'. In this gamemode there
are no stars and fellow players cannot shoot each other. Ships have been divided into two
categories, there are AI enemies and player allies. One enemy will spawn with the player
and try to shoot it. Every time an AI enemy is killed, a certain number of them will spawn
after that. The default is 2 more enemies for every one killed, but this can be changed
in the settings. Fellow players can connect and help kill the enemies, but they will not be able to hurt
other players. The player will lose 1 hitpoint per enemy hit just as in SpaceWars.
Originally I wanted the player to have three lives indicated by stars in the corner. Players
and enemies would not collide with the stars, they would just indicate the lives. The player
would then not respawn after the three lives ran out. However, the custom gamemode already
took me several considerable hours to implement and I feel it is sufficient to earn the credit for it.

Settings:
There are several settings related to this gamemode in the XML section. To change the gamemode,
rewrite the XML settings file to have a value of "SpaceInvaders" for the "GameMode" tag. To
implement this gamemode, I created a new world class and changed the server to read the gamemode
from the xml and use the alternative model if the settings specify SpaceInvaders.

Caveats:
I've noticed some bugs while playing. Gameplay seems to go smooth for a very long time with up to about
10 AI players and around 20 enemies. If there is a ton of enemies and AI players, however, I've
noticed an exception while iterating over the Projectiles of the world. I tried fixing it but its a
little hard to get at apparently. I don't feel this is game-breaking but it is something to consider.
Also, the example client leaves dead enemies in the scoreboard. This is a little bit off-putting and
makes the gamemode less viable in my opinion, but I feel this is part of the client and not the server.
I don't think theres anything I could have done on the server-side to get this worked out so I did not
break a sweat over it.

=== XML ===

XML Layout:
The server will initially create a default world with default settings. The server then tries
to read an XML file in the base directory of the Resources project named 'settings.xml'. If
this file cannot be read, then the server keeps the default settings. If the file can be read,
then the server goes through the appropriate tag for each setting and replaces the default
setting with the one read from the file if the setting can be read from the file. If any setting
cannot be read from the file, the corresponding default setting is kept. All setting tags are
to be directly under the root tag called 'settings'. For a description of each setting, its default,
and its XML tag, see the table below.
Stars may also be defined in the XML document using the 'Star' tag under the settings tag. The
star tag must contain a 'x', 'y', and 'mass' tags under it defining the size and location of the
star.

SpaceWars (default gamemode) XML Settings:
+------------------------------------------------+-----------------+----------+-----------+
|              Setting Description               |     Tagname     | Datatype |  Default  |
+------------------------------------------------+-----------------+----------+-----------+
| A ships starting hp                            | StartHP         | Int32    | 5         |
| Millseconds to wait between calculating frames | MsPerFrame      | Int32    | 15        |
| Frames to wait between ships firing            | ProjFiringDelay | Int32    | 6         |
| Frames to wait before respawn                  | RespawnDelay    | Int32    | 300       |
| World Size (square edge length)                | WorldSize       | Int32    | 750       |
| A ships engine strength                        | EngineStrength  | Double   | 0.08      |
| A ships turning rate                           | TurnRate        | Double   | 2.0       |
| A ships hitbox radius                          | ShipSize        | Double   | 20.0      |
| A stars hitbox radius                          | StarSize        | Double   | 35.0      |
| Velocity (units/second) of projectiles         | ProjVelocity    | Double   | 15.0      |
| Game Mode (SpaceWars or SpaceInvaders)         | GameMode        | String   | SpaceWars |
+------------------------------------------------+-----------------+----------+-----------+
The MsPerFrame and GameMode settings are read by the server, all others are read by the model.

SpaceInvaders (custom gamemode) XML Settings:
If the 'SpaceInvaders' gamemode is chosen in the xml settings, then these additional settings are relevant:
+------------------------------------------+-------------------------+----------+---------+
|           Setting Description            |         Tagname         | Datatype | Default |
+------------------------------------------+-------------------------+----------+---------+
| Enemy Firing Delay (frames)              | EnemyProjFiringDelay    | Int32    |     200 |
| Count of ships that spawn after one dies | EnemyRespawnRate        | Int32    |       2 |
| Enemy Respawn Delay (frames)             | EnemyRespawnDelay			 | Int32    |     300 |
| Life Count                               | LifeCount               | Int32    |       3 |
+------------------------------------------+-------------------------+----------+---------+
All other settings for SpaceWars are relevant except 'StarSize' and 'Star' settings since there
is no stars in SpaceInvaders. If these settings are found they will be ignored.