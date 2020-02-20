using SpaceWars;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Model
{
    /// <summary>
    /// This class encapsulates all information about a SpaceInvaders game.
    /// 
    /// <remarks>
    /// <para>
    /// The world class itself contains information about the world that is
    /// not specific to individual ships or projectiles. This is information
    /// like world size, ship hitbox radius, life count, etc...
    /// </para>
    /// <para>
    /// The world class also contains sets of ships, enemies, and projectiles.
    /// These sets contain all ships, enemies, and projectiles present in the
    /// world. These sets can be modified directly, but it is discouraged to
    /// do so. Instead, the world provides many methods for modifying the
    /// world state.
    /// </para>
    /// </remarks>
    /// </summary>
    public class SpaceInvadersWorld : IWorld
    {
        /// <summary>
        /// Create a default world.
        /// 
        /// <remarks>
        /// This uses the size contructor with a size of 750.
        /// <see cref="SpaceInvadersWorld.SpaceInvadersWorld(int)"/>
        /// </remarks>
        /// </summary>
        /// <param name="size"></param>
        public SpaceInvadersWorld() : this(750)
        {

        }

        /// <summary>
        /// Create a world with all default settings except size.
        /// 
        /// <remarks>
        /// See the readme for information about default settings.
        /// </remarks>
        /// </summary>
        /// <param name="WorldSize"></param>
        public SpaceInvadersWorld(int WorldSize)
        {
            this.Size = WorldSize;
            this.NextProjectileId = 0;       // counter of which id number to assign the next projectile
            // note that this value is consequentially the maximum guaranteed amount of players that can
            // connect without the server crashing.
            this.nextEnemyId = 100;           // what id to give next enemy. N

            this.Ships = new HashSet<Ship>();
            this.Projectiles = new HashSet<Projectile>();
            this.Stars = new HashSet<Star>();

            // spaceinvaders enemies
            this.enemies = new HashSet<EnemyShip>();

            // default settings
            this.StartingHP = 5;
            this.EngineStrength = 0.08;
            this.TurnRate = 2.0;
            this.ShipSize = 20;
            this.MsPerFrame = 15;
            this.ProjectileFiringDelay = 6;
            this.ProjectileVelocity = 15;
            this.RespawnDelay = 300;

            // settings specific to the space invaders gamemode
            this.EnemyProjFiringDelay = 200;
            this.EnemyRespawnRate = 2;
            this.EnemyRespawnDelay = 300;
            this.LifeCount = 3;

            // create a single enemy
            this.SpawnEnemy();
        }

        /// <summary>
        /// Create a world based off an XML document.
        /// 
        /// <remarks>
        /// This constructor creates a default world, then replaces each setting with the setting
        /// in the XML file, if it can be found. So if no settings can be read, then a default
        /// world is created. If some settings can be read, but not others, then some settings
        /// will have default values but others will not. See the readme for more information.
        /// 
        /// Note that the 'GameMode' tag is not read here, but in the server. It determines
        /// which class to use as the world.
        /// </remarks>
        /// </summary>
        /// <param name="settingsXmlFilePath">path of xml to read settings from</param>
        public SpaceInvadersWorld(string settingsXmlFilePath) : this()
        {
            // a default world is created first, then this constructor attempts
            //   to find each setting in the xml file and replaces the default
            XDocument settingsDoc = XDocument.Load(settingsXmlFilePath);
            XElement settings = settingsDoc.Element("settings");

            String XmlStartHP = GetXmlValue(settings, "StartHP");
            if (!ReferenceEquals(null, XmlStartHP))
            {
                this.StartingHP = Convert.ToInt32(XmlStartHP);
            }

            String XmlEngineStrength = GetXmlValue(settings, "EngineStrength");
            if (!ReferenceEquals(null, XmlEngineStrength))
            {
                this.EngineStrength = Convert.ToDouble(XmlEngineStrength);
            }

            String XmlTurnRate = GetXmlValue(settings, "TurnRate");
            if (!ReferenceEquals(null, XmlTurnRate))
            {
                this.TurnRate = Convert.ToDouble(XmlTurnRate);
            }

            String XmlWorldSize = GetXmlValue(settings, "WorldSize");
            if (!ReferenceEquals(null, XmlWorldSize))
            {
                this.Size = Convert.ToInt32(XmlWorldSize);
            }

            String XmlShipSize = GetXmlValue(settings, "ShipSize");
            if (!ReferenceEquals(null, XmlShipSize))
            {
                this.ShipSize = Convert.ToDouble(XmlShipSize);
            }

            String XmlMsPerFrame = GetXmlValue(settings, "MsPerFrame");
            if (!ReferenceEquals(null, XmlMsPerFrame))
            {
                this.MsPerFrame = Convert.ToInt32(XmlMsPerFrame);
            }

            String XmlProjFiringDelay = GetXmlValue(settings, "ProjFiringDelay");
            if (!ReferenceEquals(null, XmlProjFiringDelay))
            {
                this.ProjectileFiringDelay = Convert.ToInt32(XmlProjFiringDelay);
            }

            String XmlProjVelocity = GetXmlValue(settings, "ProjVelocity");
            if (!ReferenceEquals(null, XmlProjVelocity))
            {
                this.ProjectileVelocity = Convert.ToDouble(XmlProjVelocity);
            }

            String XmlRespawnDelay = GetXmlValue(settings, "RespawnDelay");
            if (!ReferenceEquals(null, XmlRespawnDelay))
            {
                this.RespawnDelay = Convert.ToInt32(XmlRespawnDelay);
            }

            // spaceinvaders-specific settings
            String XmlEnemyProjFiringDelay = GetXmlValue(settings, "EnemyProjFiringDelay");
            if (!ReferenceEquals(null, XmlEnemyProjFiringDelay))
            {
                this.EnemyProjFiringDelay = Convert.ToInt32(XmlEnemyProjFiringDelay);
            }

            String XmlEnemyRespawnRate = GetXmlValue(settings, "EnemyRespawnRate");
            if (!ReferenceEquals(null, XmlEnemyRespawnRate))
            {
                this.EnemyRespawnRate = Convert.ToInt32(XmlEnemyRespawnRate);
            }

            String XmlEnemyRespawnDelay = GetXmlValue(settings, "EnemyRespawnDelay");
            if (!ReferenceEquals(null, XmlEnemyRespawnDelay))
            {
                this.EnemyRespawnDelay = Convert.ToInt32(XmlEnemyRespawnDelay);
            }

            String XmlLifeCount = GetXmlValue(settings, "LifeCount");
            if (!ReferenceEquals(null, XmlLifeCount))
            {
                this.LifeCount = Convert.ToInt32(XmlLifeCount);
            }
        }

        // === Public Entity Sets ===

        /// <summary>
        /// All the ships in the world.
        /// 
        /// <remarks>
        /// Includes both player and enemy ships. Player and enemy ships will
        /// also be encapsulated in separate sets for speed, but they will
        /// have a duplicate added here for a consistent interface.
        /// </remarks>
        /// </summary>
        public HashSet<Ship> Ships { get; set; }

        /// <summary>
        /// All the projectiles in the world.
        /// 
        /// <remarks>
        /// Both enemy and player projectiles will be kept here, there is no need
        /// to separate them as with the ships since no improvment will be made from
        /// that. Projectiles just need to be moved on and, when they collide, the
        /// model will handle them according to whether they are player or enemy
        /// projectiles.
        /// </remarks>
        /// </summary>
        public HashSet<Projectile> Projectiles { get; set; }

        /// <summary>
        /// All the stars in the world.
        /// 
        /// <remarks>
        /// This will always be empty as there is no stars in the world. This is only
        /// here to satisfy the IWorld interface for this model.
        /// </remarks>
        /// </summary>
        public HashSet<Star> Stars { get; set; }

        // === Private Entity Sets ===

        /// <summary>
        /// All enemy ships.
        /// 
        /// <remarks>
        /// This is a subset of the 'Ships' property of this class. These ships
        /// are kept track of separately for quick updating.
        /// </remarks>
        /// </summary>
        private HashSet<EnemyShip> enemies;

        // === World Settings ===

        /// <summary>
        /// The size of the world.
        /// 
        /// <remarks>
        /// The world is square, this is the size of an edge.
        /// </remarks>
        /// </summary>
        public Int32 Size { get; internal set; }

        /// <summary>
        /// The hitpoints every ship starts with.
        /// </summary>
        public Int32 StartingHP { get; internal set; }

        /// <summary>
        /// The acceleration of every ship.
        /// 
        /// <remarks>
        /// The value of this attribute is the amount of units/second^2 a ship
        /// accelerates by every frame that they are thrusting. That is, a ships
        /// velocity increases by `EngineStrength` units/second every frame it
        /// is thrusting.
        /// </remarks>
        /// </summary>
        private readonly Double EngineStrength;

        /// <summary>
        /// The turn rate of every ship.
        /// 
        /// <remarks>
        /// The value of this attriute is the amount of degrees a ships angle
        /// will increase by every frame it is turning. That is, a ships angle
        /// will increase/decrease by `TurnRate` degrees every frame it is
        /// turning clockwise and counter-clockwise, respectively.
        /// </remarks>
        /// </summary>
        private readonly Double TurnRate;

        /// <summary>
        /// The size of every ship.
        /// 
        /// <remarks>
        /// This attribute does not define the teture size of a ship, but defines
        /// the hitbox of the ship. A ship's hitbox is a circle around the center
        /// of the ship with radius `ShipSize`.
        /// </remarks>
        /// </summary>
        private readonly Double ShipSize;

        /// <summary>
        /// Milliseconds between the end of updating one frame and the start of updating the next.
        /// 
        /// <remarks>
        /// This counts the amount of time between the end of computing one frame, and the beginning
        /// of computing the next. So, in other words, it does not include the time to compute each
        /// frame.
        /// </remarks>
        /// </summary>
        private readonly Int32 MsPerFrame;

        /// <summary>
        /// The number of frames a ship must wait between shooting projectiles.
        /// 
        /// <remarks>
        /// If a ship shoots a projectile one frame, it must wait `ProjectileFiringDelay`
        /// more frames to shoot another.
        /// </remarks>
        /// </summary>
        private readonly Int32 ProjectileFiringDelay;

        /// <summary>
        /// The number of frames a ship must wait before it can respawn.
        /// 
        /// <remarks>
        /// If a ship dies one frame, then it will respawn in `RespawnDelay`
        /// more frames.
        /// </remarks>
        /// </summary>
        private readonly Int32 RespawnDelay;

        /// <summary>
        /// The velocity of every projectile.
        /// 
        /// <remarks>
        /// A projectile's position will increase by `ProjectileVelocity`
        /// units every frame.
        /// </remarks>
        /// </summary>
        private readonly Double ProjectileVelocity;

        /// <summary>
        /// The amount of frames an enemy must wait to shoot another prjectile.
        /// </summary>
        private readonly Int32 EnemyProjFiringDelay;

        /// <summary>
        /// The amount of frames an enemy waits to respawn.
        /// 
        /// <remarks>
        /// Enemies don't respawn as much as spawn. When one enemy is killed a certain
        /// number more are spawned. This attribute defines the frame count between one
        /// enemy being killed and several more spawning.
        /// </remarks>
        /// </summary>
        private readonly Int32 EnemyRespawnDelay;

        /// <summary>
        /// How many more enemies spawn after one is killed.
        /// </summary>
        private readonly Int32 EnemyRespawnRate;

        /// <summary>
        /// How many times a player can die before they stop respawning.
        /// </summary>
        private readonly Int32 LifeCount;

        /// <summary>
        /// A counter to keep track of which ID number to give the next projectile.
        /// 
        /// <remarks>
        /// Every time a projectile is made (a ship fires), it is giving the ID
        /// number defined here and this value is incremented. Enemy and player
        /// projectiles share the same id space.
        /// </remarks>
        /// </summary>
        private Int32 NextProjectileId;

        /// <summary>
        /// A counter to keep track of which id to give the next enemy.
        /// 
        /// <remarks>
        /// This value must be incremented every time a new enemy is spawned.
        /// </remarks>
        /// </summary>
        private Int32 nextEnemyId;

        // === Public Methods ===

        /// <summary>
        /// Advances the world by one frame.
        /// 
        /// <remarks>
        /// Ship motion complies with classical mechanics in that acceleration is constant,
        /// velocity is linear, and position is quadratic. These values are computed by instantaneous
        /// (frame-by-frame) accumulation. Instead of using derivatives and integrals, the total
        /// acceleration is calculated for each ship, then added to the velocity. The velocity is
        /// then added to the position. This happens every frame and, over a few seconds, these
        /// actions create approximate derivatives/integrals.
        /// </remarks>
        /// </summary>
        public void Advance()
        {
            // each method has its own locks so a lock is not necessary here
            AdvanceShips();
            AdvanceEnemies();
            AdvanceProjectiles();
            HandleCollisions();
        }

        /// <summary>
        /// Thrust a ship.
        /// </summary>
        /// <param name="ship">the ship to thrust</param>
        public void Thrust(Ship ship)
        {
            // set the 'thrusting' flag in the ship
            //   this flag will be unset when the ship is updated next frame
            ship.IsThrusting = true;
        }

        /// <summary>
        /// Turn a ship.
        /// </summary>
        /// <param name="ship">the ship to turn.</param>
        /// <param name="clockwise">The direction to turn. True for clockwise, false for counter-clockwise.</param>
        public void Turn(Ship ship, bool clockwise)
        {
            // set the 'turning' flag of a ship
            //   this flag will be unset when the ship is updated next frame
            if (clockwise)
            {
                ship.TurningClockwise = true;
            }
            else
            {
                ship.TurningCounterClockwise = true;
            }
        }

        /// <summary>
        /// Fire a ship.
        /// 
        /// <remarks>
        /// This will only fire a ship if it can fire. I.e., if it has waited
        /// the defined amount of frames since it last fired.
        /// </remarks>
        /// </summary>
        /// <param name="ship">Ship to fire</param>
        public void Fire(Ship ship)
        {
            if (ship.CanShoot)
            {
                Vector2D projLocation = new Vector2D(ship.Location);
                Vector2D projDirection = new Vector2D(ship.Direction);
                int projOwnerId = ship.ShipID;
                projLocation = projLocation + (ship.Direction * this.ShipSize);  // adjust location to tip of ship instead of center

                lock (this.Projectiles)
                {
                    Projectile projectile = new Projectile(NextProjectileId, projLocation, projDirection, projOwnerId);
                    // set the projectile to enemy if the shooter is an enemy
                    projectile.IsEnemy = ship.IsEnemy;
                    Projectiles.Add(projectile);
                    NextProjectileId++;
                }

                ship.CanShoot = false; // trigger shooting delay
            }
        }

        /// <summary>
        /// Spawn a new ship onto the board.
        /// </summary>
        /// <param name="playerId">The ID of the ship</param>
        public Ship SpawnNewShip(int playerId, String playerName)
        {
            Vector2D newShipLocation = GetShipSpawnCoordinates();   // generate random coordinates
            Vector2D newShipDirection = new Vector2D(1, 1);         // point up
            newShipDirection.Normalize();
            // new ship is not thrusting and has no score
            Ship newShip = new Ship(playerId, playerName, newShipLocation, newShipDirection,
                false, this.StartingHP, 0, this.RespawnDelay, this.ProjectileFiringDelay);

            lock (this.Ships)
            {
                this.Ships.Add(newShip);
            }

            return newShip;
        }

        /// <summary>
        /// Remove a player from the world not to respawn.
        /// 
        /// <remarks>
        /// This is meant to be done when a client disconnects from the server.
        /// </remarks>
        /// </summary>
        /// <param name="playerId">ID of player to remove</param>
        public void RemovePlayer(Int32 playerId)
        {
            Ship ship = GetShip(playerId);
            ship.Remove();      // mark for removal
        }

        // === Static Helper Methods === 

        /// <summary>
        /// Get the value of a tag from an XML document.
        /// </summary>
        /// <param name="xml">Xml to get the tag from</param>
        /// <param name="tag">name of the tag</param>
        /// <returns>
        /// value of the tag or null
        /// </returns>
        private static String GetXmlValue(XElement xml, String tag)
        {
            XElement element = xml.Element(tag);
            if (element == null)
            {
                return null;
            }
            else
            {
                return element.Value;
            }
        }

        // === General-Purpose Nonstatic Helper Methods ===

        /// <summary>
        /// Helper method that gets a ship in the world by ID.
        /// 
        /// <remarks>
        /// Sometimes the server will search for a ship when it is not in the model. This
        /// happens due to the asynchronous nature of operation e.g. a ship may be deleted because
        /// it collided with a star, then one of its projectiles may collide with another.
        /// In the event that a ship cannot be found with the matching id, a dummy ship will
        /// be returned. In this way, operations performed on the dummy ship will not affect
        /// the mode and it will prevent NPEs and perpetual null-checks every call.
        /// </remarks>
        /// </summary>
        /// <param name="id">id of ship to find</param>
        /// <returns>
        /// The ship with the matching id, or a dummy ship
        /// </returns>
        private Ship GetShip(int id)
        {
            lock (this.Ships)
            {
                foreach (Ship ship in Ships)
                {
                    if (ship.ShipID.Equals(id))
                    {
                        return ship;
                    }
                }
                // return a dummy ship if cannot find owner
                return new Ship(id, "dummy", new Vector2D(10, 10), new Vector2D(1, 0), false, 5, 0, this.RespawnDelay, this.ProjectileFiringDelay);
            }
        }
        /// <summary>
        /// Helper method that gets an enemy ship with an ID.
        /// 
        /// <remarks>
        /// This will only return a ship if it is an enemy ship. If there is a player ship
        /// with the matching id, it will not be returned. A dummy ship will be returned
        /// instead.
        /// </remarks>
        /// </summary>
        /// <param name="id">id of ship to find</param>
        /// <returns>
        /// The ship with the matching id, or a dummy ship
        /// </returns>
        private EnemyShip GetEnemyShip(int id)
        {
            lock (this.Ships)
            {
                foreach (EnemyShip enemyShip in enemies)
                {
                    if (enemyShip.ShipID.Equals(id))
                    {
                        return enemyShip;
                    }
                }
                // return a dummy ship if cannot find owner
                return new EnemyShip(id, "dummy", new Vector2D(10, 10), new Vector2D(1, 0), false, 5, this.RespawnDelay, this.EnemyProjFiringDelay);
            }
        }

        /// <summary>
        /// Generate an enemy direction based off of their ID.
        /// 
        /// <remarks>
        /// The id determines a direction from {up, down, left, right},
        /// then it is offset by a randomly generated number.
        /// </remarks>
        /// </summary>
        /// <param name="enemyId">
        /// The id of the enemy to base the direction off of.
        /// </param>
        /// <returns></returns>
        private Vector2D GetEnemyDirection(int enemyId)
        {
            int xDir = 0;
            int yDir = 0;
            int dirInt = enemyId % 4;
            switch (dirInt)
            {
                // north
                case 0:
                    xDir = 0;
                    yDir = -1;
                    break;
                // south
                case 1:
                    xDir = 0;
                    yDir = 1;
                    break;
                // east
                case 2:
                    xDir = 1;
                    yDir = 0;
                    break;
                // west
                case 3:
                    xDir = -1;
                    yDir = 0;
                    break;
            }
            Vector2D baseVector = new Vector2D(xDir, yDir);

            // TODO randomly offset vector

            return baseVector;
        }

        /// <summary>
        /// Calculate new coordinates for a ship to respawn at.
        /// 
        /// <remarks>
        /// New coordaintes are calculated so that the ship is far enough away
        /// from a star to not immediately die. This is defined as at least 100 units.
        /// </remarks>
        /// </summary>
        /// <returns>spawn coordinates of ship</returns>
        private Vector2D GetShipSpawnCoordinates()
        {
            double minDistance = 100;      // minimum distance a ship can be from a star
            Random rand = new Random();

            while (true)
            {
                // randomly generate coordinates
                int axisLength = this.Size / 2;            // half of one side of the world
                int xCoord = rand.Next(Size) - axisLength; // adjust to be within (-axisLen, axisLen)
                int yCoord = rand.Next(Size) - axisLength;
                Vector2D spawnLocation = new Vector2D(xCoord, yCoord);

                // if its too close to any star, generate new coordinates
                foreach (Star star in Stars)
                {
                    Vector2D distance = spawnLocation - star.Location;
                    if (distance.Length() < minDistance)
                    {
                        continue;
                    }
                }

                return spawnLocation;
            }
        }

        /// <summary>
        /// Respawn a ship.
        /// 
        /// <remarks>
        /// Differs from SpawnShip in that a new ship is not created, instead
        /// an existing ship is exhumed.
        /// </remarks>
        /// </summary>
        /// <param name="ship"></param>
        private void RespawnShip(Ship ship)
        {
            Vector2D newLoc = GetShipSpawnCoordinates();    // random location far enough away from star(s)
            Vector2D newDir = new Vector2D(0, -1);   // pointed up

            ship.Location = newLoc;
            ship.Direction = newDir;
            ship.ResetVelocity();       // so that ship spawns not moving
            ship.HitPoints = this.StartingHP;
        }

        /// <summary>
        /// Respawn an enemy ship.
        /// 
        /// <remarks>
        /// Differs from RespawnShip in that it does not reset the direction and location.
        /// </remarks>
        /// </summary>
        /// <param name="enemy"></param>
        private void RespawnEnemy(EnemyShip enemy)
        {
            enemy.ResetVelocity();       // so that ship spawns not moving
            enemy.HitPoints = this.StartingHP;
        }

        /// <summary>
        /// Spawn a new enemy into the world.
        /// 
        /// <remarks>
        /// Enemies are not respawned, but destroyed and then more are spawned. This
        /// creates a new, dead enemy. The dead enemy will wait the approrpiate amount
        /// of frames to 'respawn' (in this case its just spawning).
        /// </remarks>
        /// </summary>
        private void SpawnEnemy()
        {
            Vector2D newLoc = GetShipSpawnCoordinates();    // random location far enough away from star(s)
            int enemyId = this.nextEnemyId;
            String enemyName = "enemy_" + enemyId;
            Vector2D newDir = GetEnemyDirection(enemyId);   // generate an enemy direction

            // create the new enemy
            EnemyShip enemy = new EnemyShip(nextEnemyId, enemyName, newLoc, newDir, false, this.StartingHP, this.RespawnDelay, this.EnemyProjFiringDelay);
            nextEnemyId++;

            // add them to the appropriate ship sets
            lock (this.Ships)
            {
                this.Ships.Add(enemy);
            }
            lock (this.enemies)
            {
                this.enemies.Add(enemy);
            }
        }

        // === Helper Methods For Advancing Frames ===

        /// <summary>
        /// Sum every acceleration vector acting on a ship.
        /// 
        /// <remarks>
        /// Currently this includes any star(s) and the thrust vector.`
        /// </remarks>
        /// </summary>
        /// <param name="ship"></param>
        /// <returns>Sum of acceleration vectors action on a ship.</returns>
        private Vector2D SumAccelVectors(Ship ship)
        {
            Vector2D a = new Vector2D(0, 0);    // cumulative acceleration

            // add all star vectors
            foreach (Star star in Stars)
            {
                Vector2D gDir = star.Location - ship.Location;  // direction from ship to star
                gDir.Normalize();
                Vector2D g = gDir * star.Mass;    // amplified by stars mass for psuedo-gravity
                a = a + g;
            }

            // add thrust
            if (ship.IsThrusting)
            {
                Vector2D thrust = ship.Direction * EngineStrength;
                a = a + thrust;
            }

            return a;
        }

        /// <summary>
        /// Wrap a ship around the world if needed.
        /// </summary>
        /// <param name="ship"></param>
        private void WrapAround(Ship ship)
        {
            // the length of one axis of world-space coordinates
            // each axis is the same length and the origin is at (0,0)
            double axisLength = this.Size / 2;
            double xCoord = ship.Location.GetX();
            double yCoord = ship.Location.GetY();

            // if pass over east or west boundary, then flip
            if (Math.Abs(xCoord) > axisLength)
            {
                double newXCoord = xCoord * -1;
                ship.Location = new Vector2D(newXCoord, yCoord);
            }
            // if pass over north or south boundary, then flip
            if (Math.Abs(yCoord) > axisLength)
            {
                double newYCoord = yCoord * -1;
                ship.Location = new Vector2D(xCoord, newYCoord);
            }
        }

        /// <summary>
        /// Determine if a projectile is off screen.
        /// </summary>
        /// <param name="projectile"></param>
        private bool IsOffScreen(Projectile projectile)
        {
            // the length of one axis of world-space coordinates
            // each axis is the same length and the origin is at (0,0)
            double axisLength = this.Size / 2;
            double xCoord = projectile.Location.GetX();
            double yCoord = projectile.Location.GetY();

            // if pass over east or west boundary, then flip
            if (Math.Abs(xCoord) > axisLength)
            {
                return true;
            }
            // if pass over north or south boundary, then flip
            if (Math.Abs(yCoord) > axisLength)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determine if a projectile has collided with a player.
        /// 
        /// <remarks>
        /// If a projectile collides with its owner, then nothing happens. Does not return true.
        /// If a player's projectile collides with another player, nothing happens. A collision
        /// is only returned when an enemies projectile collides with a players projectile.
        /// </remarks>
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="collidedShipId">
        /// The id of the ship that the projectile collided with. Undefined if no collision.
        /// </param>
        /// <returns>
        /// true if the projectile is an enemies' and if it has collided with a player ship,
        /// false otherwise
        /// </returns>
        private Boolean ProjCollidedWithPlayer(Projectile projectile, out int collidedShipId)
        {
            collidedShipId = -1;

            // a player projectile cannot collide with another player
            if (!projectile.IsEnemy)
            {
                return false;
            }

            lock (this.Ships)
            {
                foreach (Ship ship in Ships)
                {
                    // only examine player ships since enemy projectiles cannot collide with other enemy projectiles
                    if (ship.IsEnemy)
                    {
                        continue;
                    }

                    Vector2D distance = projectile.Location - ship.Location;
                    if (distance.Length() <= ShipSize)
                    {
                        // the enemy projectile has collided with a player ship
                        
                        // if the ship is the projectiles owner, it is not a collision
                        //   I dont think this will ever be true but ill keep it in for good measure
                        if (ship.ShipID == projectile.Owner)
                        {
                            continue;
                        }

                        // if the ship is 'dead', then it is not a collision
                        if (ship.IsDead)
                        {
                            continue;
                        }

                        // the enemy projectile collided with a non-dead player ship
                        collidedShipId = ship.ShipID;
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Determine if a projectile has collided with an enemy.
        /// 
        /// <remarks>
        /// If the projectile collides with its owner, then nothing happens. Does not return true.
        /// If the projectile is an enemy's, then return false.
        /// If the projectile is a player's and collides with another player, then returns false.
        /// If the projectile is a player's and collides with an enemy, then returns true.
        /// </remarks>
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="collidedShipId">
        /// The id of the ship that the projectile collided with. Undefined if no collision.
        /// </param>
        /// <returns>
        /// true if the projectile is a player's and it has collided with an enemy ship, false otherwise.
        /// </returns>
        private Boolean ProjCollidedWithEnemy(Projectile projectile, out int collidedShipId)
        {
            collidedShipId = -1;

            // a enemy projectile cannot collide with another enemy
            if (projectile.IsEnemy)
            {
                return false;
            }

            lock (this.Ships)
            {
                foreach (Ship ship in Ships)
                {
                    // only examine enemy ships since player projectiles cannot collide with other player projectiles
                    if (!ship.IsEnemy)
                    {
                        continue;
                    }

                    Vector2D distance = projectile.Location - ship.Location;
                    if (distance.Length() <= ShipSize)
                    {
                        // the player projectile has collided with an enemy ship

                        // if the ship is the projectiles owner, it is not a collision
                        //   I dont think this will ever be true but ill keep it in for good measure
                        if (ship.ShipID == projectile.Owner)
                        {
                            continue;
                        }

                        // if the ship is 'dead', then it is not a collision
                        if (ship.IsDead)
                        {
                            continue;
                        }

                        // the player projectile collided with a non-dead enemy ship
                        collidedShipId = ship.ShipID;
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Destroy a ship.
        /// 
        /// <remarks>
        /// The ship is not removed from the ship set, instead it is marked as 'dead' and a countdown
        /// of frames to respawn is set in motion. The number of frames until a ship is respawned is
        /// defined during construction. After this countdown reaches zero, the ship is respawned.
        /// </remarks>
        /// </summary>
        /// <param name="ship"></param>
        private void DestroyPlayer(Ship ship)
        {
            ship.HitPoints = 0;
            ship.Destroy();
        }

        /// <summary>
        /// Destroy an enemy.
        /// 
        /// <remarks>
        /// The enemy ship is removed from the ship sets and a certain number of new enemies are
        /// spawned into the world.
        /// </remarks>
        /// </summary>
        /// <param name="enemy">the enemy to destroy.</param>
        private void DestroyEnemy(EnemyShip enemy)
        {
            // mark it for removal
            //   it will be removed from 'enemies' and 'Ships' in the next call
            //   to 'advance ships' 
            enemy.MarkedForRemoval = true;

            // spawn `EnemyRespawnRate` new (dead) enemies
            for ( int enemyCounter = 0; enemyCounter < this.EnemyRespawnRate; enemyCounter++)
            {
                SpawnEnemy();
            }
        }

        /// <summary>
        /// Get the player nearest to an enemy ship.
        /// </summary>
        /// <param name="enemy"></param>
        /// <returns>
        /// player nearest to enemy. Null if there are no players in the world.
        /// </returns>
        private Ship GetNearestPlayer(EnemyShip enemy)
        {
            Vector2D smallestDistance = new Vector2D(this.Size, this.Size);
            Ship smallestShip = null;
            lock (this.Ships)
            {
                // minmize distance to players and return
                foreach (Ship ship in Ships)
                {
                    // skip enemies, only looking for players
                    if (ship.IsEnemy)
                    {
                        continue;
                    }
                    
                    Vector2D shipDist = ship.Location - enemy.Location;
                    if (shipDist.Length() < smallestDistance.Length())
                    {
                        smallestDistance = shipDist;
                        smallestShip = ship;
                    }
                }
            }

            return smallestShip;
        }

        /// <summary>
        /// Move each ship in the world forward one frame.
        /// 
        /// <remarks>
        /// Wraparounds, turning, thrusting, shooting timers, and ship input flags are
        /// all handled in this method.
        /// Players and enemies are advanced in this method. Enemy behavior is determined
        /// in the 'AdvanceEnemies' method.
        /// </remarks>
        /// </summary>
        private void AdvanceShips()
        {
            lock (this.Ships)
            {
                foreach (Ship ship in Ships)
                {
                    // respawn and countdown dead ships
                    if (ship.IsDead)
                    {
                        ship.DecrementRespawnTimer();
                        if (ship.CanRespawn)
                        {
                            // spawn the ship differently depending on if it is an enemy or not
                            if (ship.IsEnemy)
                            {
                                RespawnEnemy((EnemyShip)ship);
                            }
                            else
                            {
                                RespawnShip(ship);
                            }
                        }
                        continue;
                    }

                    // move the ship forward one frame
                    Vector2D accel = SumAccelVectors(ship);     // total acceleration of ship
                    ship.Velocity = ship.Velocity + accel;
                    ship.Location = ship.Location + ship.Velocity;

                    // wrap the ship around if needed
                    WrapAround(ship);

                    // rotate the ship if needed
                    if (ship.TurningClockwise)
                    {
                        ship.Direction.Rotate(TurnRate);
                    }
                    if (ship.TurningCounterClockwise)
                    {
                        ship.Direction.Rotate(TurnRate * -1);
                    }

                    // switch off thrust/rotate flags
                    //   these flags are switched on when a clients sends the input
                    //   and switched off when the appropriate action is made
                    if (ship.IsThrusting)
                    {
                        ship.IsThrusting = false;
                    }
                    if (ship.TurningClockwise)
                    {
                        ship.TurningClockwise = false;
                    }
                    if (ship.TurningCounterClockwise)
                    {
                        ship.TurningCounterClockwise = false;
                    }

                    // decrease frames until ship can shoot again
                    ship.DecrementShootTimer();
                }

                // remove all ships marked for removal
                //   this is done when a client disconnects
                Ships.RemoveWhere((Ship s) => s.MarkedForRemoval);
                enemies.RemoveWhere((EnemyShip e) => e.MarkedForRemoval);
            }
        }

        /// <summary>
        /// Advance each enemy in the world.
        /// 
        /// <remarks>
        /// Enemies are not moved here, instead they are controller. By this it is
        /// meant that this method determines the behavior of each enemy: if each
        /// enemy should be turning (and if so, in which direction), thrusting, and
        /// firing. If the enemy should be turning or thrusting, then the appropriate
        /// flags are set in their class. If they should be firing, then the Fire
        /// method is called for the enemy.
        /// </remarks>
        /// </summary>
        private void AdvanceEnemies()
        {
            foreach (EnemyShip enemy in this.enemies)
            {
                // dead enemies have no behavior
                if (enemy.IsDead)
                {
                    continue;
                }

                // bring ship to terminal velocity if it is not there
                Double xVel = Math.Abs(enemy.Velocity.GetX());
                Double yVel = Math.Abs(enemy.Velocity.GetY());
                if (xVel < 2 && yVel < 2)
                {
                    // ship turns in a certain direction based off of their id
                    if (enemy.ShipID % 2 == 0)
                    {
                        this.Turn(enemy, true);
                    }
                    else
                    {
                        this.Turn(enemy, false);
                    }
                    Thrust(enemy);
                }
                // turn the ship toward the nearest player if it is at terminal velocity
                else
                {
                    Ship player = GetNearestPlayer(enemy);

                    // happens when no players in world, do not rotate if so
                    if (ReferenceEquals(player, null))
                    {
                        continue;
                    }

                    // turn the enemy toward the player
                    Vector2D enemyToPlayerDir = player.Location - enemy.Location;
                    enemyToPlayerDir.Normalize();
                    if (enemy.Direction.ToAngle() < enemyToPlayerDir.ToAngle())
                    {
                        Turn(enemy, true);
                    }
                    else
                    {
                        Turn(enemy, false);
                    }
                }

                // fire the enemy if it is ready
                //   might configure enemies to only fire when it is pointing toward a player
                //   right now the enemy fires as soon as it can
                if (enemy.CanShoot)
                {
                    Fire(enemy);
                }
            }
        }

        /// <summary>
        /// Advance each projectile in the world.
        /// 
        /// <remarks>
        /// Timeouts are handled here.
        /// </remarks>
        /// </summary>
        private void AdvanceProjectiles()
        {
            lock (this.Projectiles)
            {
                // remove all inactive projectiles
                //   done before advancing so inactive projectiles broadcast have a chance to be sent to
                //   clients before they are deleted. If done after then clients will have frozen projectiles.
                Projectiles.RemoveWhere((Projectile p) => !p.IsActive);

                // Compute projectile motion
                foreach (Projectile projectile in Projectiles)
                {
                    // projectiles have no acceleration
                    Vector2D velocity = projectile.Direction * ProjectileVelocity;
                    projectile.Location = projectile.Location + velocity;

                    // projectiles deleted if off screen
                    if (IsOffScreen(projectile))
                    {
                        projectile.IsActive = false;
                    }
                }
            }
        }

        /// <summary>
        /// Handle all collisions of ships and projectiles/stars
        /// 
        /// <remarks>
        /// In the event of a collision between a ship and a projectile,
        /// the victims's hp is decreased, the projectile is removed, and 
        /// the shooter's score is increased. If the victim's hp is zero or
        /// less, then it is removed.
        /// 
        /// In the event of a collision between a ship and a star, the ship
        /// is removed.
        /// </remarks>
        /// </summary>
        private void HandleCollisions()
        {
            // NOTE for the sake of clarity, this process has been made more expensive
            //   if it is a bottleneck, then it will be modified accordingly
            lock (this.Projectiles)
            {
                foreach (Projectile projectile in Projectiles)
                {
                    if (!projectile.IsActive)
                    {
                        // dont collide inactive projectiles
                        continue;
                    }

                    // logic for projectile-player collisions
                    int collidedShipId = -1;
                    if (ProjCollidedWithPlayer(projectile, out collidedShipId))
                    {
                        Ship playerVictim = GetShip(collidedShipId);
                        
                        playerVictim.HitPoints--;
                        projectile.IsActive = false;

                        if (playerVictim.HitPoints <= 0)
                        {
                            DestroyPlayer(playerVictim);
                        }
                    }
                    // logic for projectile-enemy collisions
                    else if (ProjCollidedWithEnemy(projectile, out collidedShipId))
                    {
                        EnemyShip enemyVictim = GetEnemyShip(collidedShipId);

                        enemyVictim.HitPoints--;
                        projectile.IsActive = false;

                        if (enemyVictim.HitPoints <= 0)
                        {
                            DestroyEnemy(enemyVictim);
                        }
                    }
                }
            }

            // ships dont collide with anything except projectiles (handled above)
        }
    }
}
