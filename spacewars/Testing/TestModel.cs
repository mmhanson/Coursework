// A C# library for representing the active virtual entities in the game. This project is effectively a large data wrapper and contains no methods.
// 
// Written by Joshua Cragun (u1025691) for CS 3500 at the University of Utah on November 10th, 2018
using Newtonsoft.Json;
using SpaceWars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModel
{
    /// <summary>
    /// This class represents the world of the game.
    /// Effectively it is a container for all the objects in the game.
    /// 
    /// The world will contain IEnumerables for each type of object in the world.
    /// This is largely done of abstraction purposes.
    /// </summary>
    public class World
    {
        public HashSet<Ship> Ships { get; set; }

        public HashSet<Projectile> Projectiles { get; set; }

        public HashSet<Star> Stars { get; set; }
    }

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
        [JsonProperty(PropertyName = "ship")]
        public int ShipID { set; get; }

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
    }

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
        [JsonProperty(PropertyName = "proj")]
        public int ProjID { set; get; }

        [JsonProperty(PropertyName = "loc")]
        public Vector2D Location { set; get; }

        [JsonProperty(PropertyName = "dir")]
        public Vector2D Direction { set; get; }

        [JsonProperty(PropertyName = "alive")]
        public bool IsActive { set; get; }

        [JsonProperty(PropertyName = "owner")]
        public string Owner { set; get; }
    }

    /// <summary>
    /// An object that represents a start in the game. It has the following properties:
    /// 
    /// "star" - an int representing the star's unique ID.
    ///"loc" - a Vector2D representing the star's location.
    ///"mass" - a double representing the star's mass. Note that the sample client does not use this information, but you may choose to display stars differently based on their mass.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Star
    {
        [JsonProperty(PropertyName = "star")]
        public int StarID { set; get; }

        [JsonProperty(PropertyName = "loc")]
        public Vector2D Location { set; get; }

        [JsonProperty(PropertyName = "mass")]
        public double Mass { set; get; }
    }

}
