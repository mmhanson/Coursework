using Newtonsoft.Json;
using SpaceWars;
using System;

namespace Model
{
    /// <summary>
    /// An object that represents a player in the game. It has the following properties:
    /// 
    /// "ship" - an int representing the ship's unique ID. Note: ship IDs can be the same as Projectile IDs, they are only unique relative to other Ships (same applies for Projectiles and Stars).
    /// "name" - a string representing the player's name.
    ///"loc" - a Vector2D representing the ship's location. (See below for description of Vector2D).
    ///"dir" - a Vector2D representing the ship's orientation.
    ///"thrust" - a bool representing whether or not the ship was firing engines on that frame.This can be used by the client to draw a different representation of the ship, e.g., showing engine exhaust.
    ///"hp" - and int representing the hit points of the ship.This value ranges from 0 - 5. If it is 0, then this ship is temporarily destroyed, and waiting to respawn. If the player controlling this ship disconnects, the server will discontinue sending this ship.
    ///"score" - an int representing the ship's score.
    ///
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Ship
    {
        /// <summary>
        /// The amount of frames until the ship can shoot again.
        /// 
        /// <remarks>
        /// This is related to the shootDelay attribute. They are different
        /// in that the shootTimer attribute counts down to zero and lets
        /// the ship shoot when it is zero. The shootDelay attribute is
        /// what this attribute is set to when the ship shoots.
        /// </remarks>
        /// </summary>
        private int shootTimer;

        /// <summary>
        /// The amount of frames the ship must wait to shoot again.
        /// 
        /// <remarks>
        /// This is related to the shootDelay attribute. They are different
        /// in that the shootTimer attribute counts down to zero and lets
        /// the ship shoot when it is zero. The shootDelay attribute is
        /// what this attribute is set to when the ship shoots.
        /// </remarks>
        /// </summary>
        private readonly int shootDelay;

        /// <summary>
        /// The amount of frames until the ship will respawn.
        /// </summary>
        private int respawnTimer;

        /// <summary>
        /// Whether or not this ship is dead
        /// </summary>
        private bool isDead;

        private int ID;
        [JsonProperty(PropertyName = "ship")]
        public int ShipID
        {
            set
            {
                // The ship's color is determined by it's ID
                ID = value;
                switch (value % 8)
                {
                    case 0:
                        Color = "red";
                        break;
                    case 1:
                        Color = "green";
                        break;
                    case 2:
                        Color = "blue";
                        break;
                    case 3:
                        Color = "yellow";
                        break;
                    case 4:
                        Color = "violet";
                        break;
                    case 5:
                        Color = "brown";
                        break;
                    case 6:
                        Color = "grey";
                        break;
                    case 7:
                        Color = "white";
                        break;
                }
            }
            get
            {
                return ID;
            }
        }

        /*
         * The info about these properties can be found in the method description
         */

        [JsonProperty(PropertyName = "name")]
        public string PlayerName { set; get; }

        [JsonProperty(PropertyName = "loc")]
        public Vector2D Location { set; get; }

        [JsonProperty(PropertyName = "dir")]
        public Vector2D Direction { set; get; }

        [JsonProperty(PropertyName = "thrust")]
        public bool IsThrusting { set; get; }

        [JsonProperty(PropertyName = "hp")]
        public int HitPoints { set; get; }

        [JsonProperty(PropertyName = "score")]
        public int Score { set; get; }

        /// <summary>
        /// The ship's velocity
        /// </summary>
        public Vector2D Velocity { set; get; }

        /// <summary>
        /// Indicates if the ship is turning clockwise
        /// </summary>
        public bool TurningClockwise { set; get; }

        /// <summary>
        /// Indicates if the ship is turning counter-clockwise
        /// </summary>
        public bool TurningCounterClockwise { set; get; }

        /// <summary>
        /// This is string is used to determine what color the ship should be rendered as.
        /// </summary>
        public string Color { internal set; get; }

        /// <summary>
        /// Indicates if the ship can shoot or not
        /// </summary>
        public bool CanShoot
        {
            set
            {
                // if setting canshoot to false
                if (!value)
                {
                    // must wait this amount of frames until ship can shoot again
                    this.shootTimer = this.shootDelay;
                }
                else
                {
                    this.shootTimer = 0;
                }
            }
            get
            {
                return (this.shootTimer <= 0 && !this.IsDead);
            }
        }

        public bool CanRespawn
        {
            internal set
            {
                // can only be set through the IsDead property
            }
            get
            {
                return (this.respawnTimer <= 0);
            }
        }

        public bool IsDead
        {
            internal set
            {
                this.isDead = value;        // only set to true when respawnTimer is zero or less

                // if killing ship, then set the respawn countdown
                if (this.isDead)
                {
                    this.respawnTimer = 200;        // TODO use attribute instead of hard code
                }
            }
            get
            {
                return this.isDead;
            }
        }

        /// <summary>
        /// Flag to determine if this ship is an enemy.
        /// 
        /// <remarks>
        /// Only used in the space invaders game mode.
        /// </remarks>
        /// </summary>
        public Boolean IsEnemy
        {
            get;
            internal set;
        }

        /// <summary>
        /// Whether this ship will be removed.
        /// 
        /// See the 'remove' method.
        /// </summary>
        public Boolean MarkedForRemoval
        {
            get; internal set;
        }

        /// <summary>
        /// The number of frames before the ship is considered "timed out" and no longer rendered.
        /// </summary>
        private int frameCount;

        /// <summary>
        /// Default ship constructor
        /// </summary>
        public Ship()
        {
            this.frameCount = 30;
        }

        /// <summary>
        /// Dummy ship constructor
        /// </summary>
        /// <param name="id"></param>
        public Ship(int id)
        {
            ID = id;
            this.frameCount = 30;
            this.shootTimer = 0;
        }

        /// <summary>
        /// Parameterized ship constructor
        /// </summary>
        /// <param name="id">Ship's ID</param>
        /// <param name="playername">Player's name</param>
        /// <param name="location">Location of the ship</param>
        /// <param name="direction">Direction of the ship</param>
        /// <param name="thrust">If the ship is thursting</param>
        /// <param name="hp">Number of hitpoints the ship has</param>
        /// <param name="score">Player's score</param>
        /// <param name="respawnFrames">The amount of frames the ship must wait to respawn</param>
        /// <param name="shootDelay">The amount of frames the ship must wait to shoot again</param>
        public Ship(int id, string playername, Vector2D location, Vector2D direction, bool thrust, int hp, int score, int respawnFrames, int shootDelay)
        {
            ShipID = id;
            PlayerName = playername;
            Location = location;
            Direction = direction;
            Velocity = new Vector2D(0, 0);
            IsThrusting = thrust;
            HitPoints = hp;
            Score = score;
            this.frameCount = 30;
            this.shootTimer = 0;
            this.shootDelay = shootDelay;

            this.IsEnemy = false;
        }

        /// <summary>
        /// The hash code for SpaceWars objects is their ID.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ShipID;
        }

        /// <summary>
        /// Remove this ship.
        /// 
        /// <remarks>
        /// Removal is different from destruction in that a removed ship does not respawn
        /// and a destroy ship does. Ships are to be removevd after a client disconnects.
        /// The removal process will broadcast the dead ship to each client and then
        /// stop broadcasting the ship after that.
        /// </remarks>
        /// </summary>
        public void Remove()
        {
            this.MarkedForRemoval = true;
            this.IsDead = true;
            this.HitPoints = 0;
        }

        /// <summary>
        /// Destroy this ship.
        /// 
        /// <remarks>
        /// The ship object is not destroyed, it is only marked as dead and a respawn
        /// counter is set in motion.
        /// </remarks>
        /// </summary>
        public void Destroy()
        {
            this.IsDead = true;
        }
        
        /// <summary>
        /// Remove all velocity of ship.
        /// </summary>
        public void ResetVelocity()
        {
            this.Velocity = new Vector2D(0, 0);
        }

        /// <summary>
        /// Ships are considered equal if they have the same ID.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Ship)
            {
                Ship otherShip = (Ship)obj;
                return ShipID == otherShip.ShipID;
            }
            return false;
        }

        /// <summary>
        /// Increases the timeout counter for this object
        /// </summary>
        public void CountDown()
        {
            --frameCount;
        }

        /// <summary>
        /// Decrement the amount of frames the ship must wait to shoot.
        /// </summary>
        public void DecrementShootTimer()
        {
            if (shootTimer <= 0)
            {
                shootTimer = 0;
            }
            else
            {
                shootTimer--;
            }
        }

        public void DecrementRespawnTimer()
        {
            // decrement first so that when it hits zero, ship is not dead anymore
            respawnTimer--;
            if (respawnTimer <= 0)
            {
                respawnTimer = 0;
                this.IsDead = false;
            }
        }

        /// <summary>
        /// Indicates if the object timed out or not
        /// </summary>
        /// <returns></returns>
        public bool TimedOut()
        {
            return frameCount <= 0;
        }
    }
}
