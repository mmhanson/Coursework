using SpaceWars;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Model
{
    /// <summary>
    /// This class encapsulates all information about a SpaceWars game.
    /// 
    /// <remarks>
    /// The world class itself contains information about the world that is
    /// not specific to individual ships, stars, or projectiles. This is
    /// information like world size, projectile firing delay, ship engine
    /// strength, projectile velocity, etc...
    /// The world class also contains sets of ships, stars, and projectiles.
    /// These sets contain all ships, stars, and projectiles present in the
    /// world. These sets can be modified directly, but it is discouraged to
    /// do so. Instead, the world provides many methods for modifying the
    /// world state.
    /// </remarks>
    /// </summary>
    public class SpaceWarsWorld : IWorld
    {
        /// <summary>
        /// Create a default world.
        /// 
        /// <remarks>
        /// This uses the size contructor with a size of 750.
        /// <see cref="SpaceWarsWorld.SpaceWarsWorld(int)"/>
        /// </remarks>
        /// </summary>
        /// <param name="size"></param>
        public SpaceWarsWorld() : this(750)
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
        public SpaceWarsWorld(int WorldSize)
        {
            this.Size = WorldSize;
            NextProjectileId = 0;       // counter of which id number to assign the next projectile

            this.Ships = new HashSet<Ship>();
            this.Projectiles = new HashSet<Projectile>();
            this.Stars = new HashSet<Star>();

            // default settings
            this.StartingHP = 5;
            this.EngineStrength = 0.08;
            this.TurnRate = 2.0;
            this.ShipSize = 20;
            this.StarSize = 35;
            this.MsPerFrame = 15;
            this.ProjectileFiringDelay = 6;
            this.ProjectileVelocity = 15;
            this.RespawnDelay = 300;
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
        public SpaceWarsWorld(string settingsXmlFilePath) : this()
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
            if(!ReferenceEquals(null, XmlWorldSize))
            {
                this.Size = Convert.ToInt32(XmlWorldSize);
            }

            String XmlShipSize = GetXmlValue(settings, "ShipSize");
            if (!ReferenceEquals(null, XmlShipSize))
            {
                this.ShipSize = Convert.ToDouble(XmlShipSize);
            }

            String XmlStarSize = GetXmlValue(settings, "StarSize");
            if (!ReferenceEquals(null, XmlStarSize))
            {
                this.StarSize = Convert.ToDouble(XmlStarSize);
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

            // add stars
            int nextStarId = 0;
            foreach (XElement star in settings.Elements("Star"))
            {
                double xCoord = Convert.ToDouble(star.Element("x").Value);
                double yCoord = Convert.ToDouble(star.Element("y").Value);
                double mass = Convert.ToDouble(star.Element("mass").Value);
                Star newStar = new Star(nextStarId, xCoord, yCoord, mass);

                Stars.Add(newStar);
                nextStarId++;
            }
        }
        
        // === Sets of Entities ===

        /// <summary>
        /// All the ships in the world.
        /// </summary>
        public HashSet<Ship> Ships { get; set; }

        /// <summary>
        /// All the projectiles in the world.
        /// </summary>
        public HashSet<Projectile> Projectiles { get; set; }

        /// <summary>
        /// All the stars in the world.
        /// </summary>
        public HashSet<Star> Stars { get; set; }

        // === World Settings ===

        /// <summary>
        /// The size of the world.
        /// 
        /// <remarks>
        /// The world is square, this is the size of an edge.
        /// </remarks>
        /// </summary>
        public Int32 Size { get; }

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
        /// The size of every star.
        /// 
        /// <remarks>
        /// This attribute does not define the teture size of a star, but defines
        /// the hitbox of the star. A stas's hitbox is a circle around the center
        /// of the star with radius `StarSize`.
        /// </remarks>
        /// </summary>
        private readonly Double StarSize;

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
        /// A counter to keep track of which ID number to give the next projectile.
        /// 
        /// <remarks>
        /// Every time a projectile is made (a ship fires), it is giving the ID
        /// number defined here and this value is incremented.
        /// </remarks>
        /// </summary>
        private Int32 NextProjectileId;

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
            Ship newShip = new Ship(playerId, playerName, newShipLocation,
                newShipDirection, false, this.StartingHP, 0, this.RespawnDelay, this.ProjectileFiringDelay);

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
        /// Determine if a projectile has collided with a ship.
        /// 
        /// <remarks>
        /// If a projectile collides with its owner, then nothing happens. Does not return true.
        /// </remarks>
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="collidedShipId">
        /// The id of the ship that the projectile collided with. Undefined if no collision.
        /// </param>
        /// <returns>true if projectile has collided with a ship, false otherwise</returns>
        private Boolean ProjCollidedWithShip(Projectile projectile, out int collidedShipId)
        {
            lock (this.Ships)
            {
                collidedShipId = -1;
                foreach (Ship ship in Ships)
                {
                    Vector2D distance = projectile.Location - ship.Location;
                    if (distance.Length() <= ShipSize)
                    {
                        // projectiles cant collide with their shooters
                        // projectiles cant collide with dead ships
                        if (ship.ShipID != projectile.Owner && !ship.IsDead)
                        {
                            collidedShipId = ship.ShipID;
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Determine if a projectile has collided with any stars
        /// </summary>
        /// <param name="projectile"></param>
        /// <returns>true if the projectile has collided with a star, false otherwise</returns>
        private Boolean ProjCollidedWithStar(Projectile projectile)
        {
            lock (this.Stars)
            {
                foreach (Star star in Stars)
                {
                    Vector2D distance = star.Location - projectile.Location;
                    if (distance.Length() <= StarSize)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Determine if a ship has collided with any stars
        /// </summary>
        /// <param name="projectile"></param>
        /// <returns>true if the ship has collided with a star, false otherwise</returns>
        private Boolean ShipCollidedWithStar(Ship ship)
        {
            lock (this.Stars)
            {
                foreach (Star star in Stars)
                {
                    Vector2D distance = star.Location - ship.Location;
                    if (distance.Length() <= StarSize)
                    {
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
        private void DestroyShip(Ship ship)
        {
            ship.HitPoints = 0;
            ship.Destroy();
        }

        /// <summary>
        /// Move each ship in the world forward one frame.
        /// 
        /// <remarks>
        /// Wraparounds, turning, thrusting, shooting timers, and ship input flags are
        /// all handled in this method.
        /// This method should only be called inside a lock of the ships or the world
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
                            RespawnShip(ship);
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

                    // logic for projectile-ship collisions
                    int collidedShipId = -1;
                    if (ProjCollidedWithShip(projectile, out collidedShipId))
                    {
                        Ship victim = GetShip(collidedShipId);
                        Ship owner = GetShip(projectile.Owner);
                        
                        victim.HitPoints--;
                        owner.Score++;
                        projectile.IsActive = false;

                        if (victim.HitPoints <= 0)
                        {
                            DestroyShip(victim);
                        }
                    }

                    // logic for projectile-star collisions
                    if (ProjCollidedWithStar(projectile))
                    {
                        projectile.IsActive = false;
                    }
                }
            }

            lock (this.Ships)
            {
                // Then check if any ships have collided with stars
                foreach (Ship ship in Ships)
                {
                    // dont destroy already-dead ships
                    if (ship.IsDead)
                    {
                        continue;
                    }

                    if (ShipCollidedWithStar(ship))
                    {
                        DestroyShip(ship);
                    }
                }
            }
        }
    }
}
