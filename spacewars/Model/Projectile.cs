using Newtonsoft.Json;
using SpaceWars;
using System;

namespace Model
{
    /// <summary>
    /// An object that represents a projectile in the game. It has the following properties:
    /// 
    /// "proj" - an int representing the projectile's unique ID.
    ///"loc" - a Vector2D representing the projectile's location.
    ///"dir" - a Vector2D representing the projectile's orientation.
    ///"alive" - a bool representing if the projectile is active or not.The server will send the deactivated projectiles only once.
    ///"owner" - an int representing the ID of the ship that created the projectile.You can use this to draw the projectiles with a different color or image.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        /// <summary>
        /// The backing attribute for the Direction property.
        /// 
        /// <remarks>
        /// The direction property was moved away from auto-generated properties
        /// so it can auto-normalize the direction vectors.
        /// </remarks>
        /// </summary>
        private Vector2D direction;

        /// <summary>
        /// Flag to determine if this is an enemy projectile.
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

        /*
        * The info about these properties can be found in the method description
        */

        [JsonProperty(PropertyName = "proj")]
        public int ProjID { set; get; }

        [JsonProperty(PropertyName = "loc")]
        public Vector2D Location { set; get; }

        [JsonProperty(PropertyName = "dir")]
        public Vector2D Direction
        {
            set
            {
                this.direction = value;
                direction.Normalize();
            }
            get
            {
                return this.direction;
            }
        }

        [JsonProperty(PropertyName = "alive")]
        public bool IsActive { set; get; }

        [JsonProperty(PropertyName = "owner")]
        public int Owner { set; get; }

        /// <summary>
        /// Create a projectile.
        /// 
        /// <remarks>
        /// Directionn vector is normalized.
        /// </remarks>
        /// </summary>
        /// <param name="idNum">Id number of the projectile</param>
        /// <param name="locXCoord">X component of location of projectile</param>
        /// <param name="locYCoord">Y component of location of projectile</param>
        /// <param name="dirX">X component of direction vector of projectile</param>
        /// <param name="dirY">Y component of direction vector of projectile</param>
        /// <param name="ownerName">name of the owner of the projectile</param>
        public Projectile(int idNum, Vector2D location, Vector2D direction, int ownerID)
        {
            this.ProjID = idNum;
            this.Location = location;
            this.Direction = direction;
            this.Direction.Normalize();
            this.IsActive = true;
            this.Owner = ownerID;

            this.IsEnemy = false;
        }

        public override int GetHashCode()
        {
            return ProjID;
        }

        public override bool Equals(object obj)
        {
            if (obj is Projectile)
            {
                Projectile otherProjectile = (Projectile)obj;
                return ProjID == otherProjectile.ProjID;
            }
            return false;
        }

        /// <summary>
        /// Default Projectile constructor
        /// </summary>
        public Projectile()
        {
            this.ProjID = -1;
        }
    }
}
