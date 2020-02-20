using System;
using System.Collections.Generic;

namespace Model
{
    public interface IWorld
    {
        // === Sets of Entities ===

        /// <summary>
        /// All the ships in the world.
        /// </summary>
        HashSet<Ship> Ships
        {
            get;
            set;
        }

        /// <summary>
        /// All the projectiles in the world.
        /// </summary>
        HashSet<Projectile> Projectiles
        {
            get;
            set;
        }

        /// <summary>
        /// All the stars in the world.
        /// </summary>
        HashSet<Star> Stars
        {
            get;
            set;
        }

        // === World Settings ===

        /// <summary>
        /// The size of the world.
        /// 
        /// <remarks>
        /// The world is square, this is the size of an edge.
        /// </remarks>
        /// </summary>
        Int32 Size
        {
            get;
        }

        /// <summary>
        /// The hitpoints every ship starts with.
        /// </summary>
        Int32 StartingHP
        {
            get;
        }

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
        void Advance();

        /// <summary>
        /// Thrust a ship.
        /// </summary>
        /// <param name="ship">the ship to thrust</param>
        void Thrust(Ship ship);

        /// <summary>
        /// Turn a ship.
        /// </summary>
        /// <param name="ship">the ship to turn.</param>
        /// <param name="clockwise">The direction to turn. True for clockwise, false for counter-clockwise.</param>
        void Turn(Ship ship, bool clockwise);

        /// <summary>
        /// Fire a ship.
        /// 
        /// <remarks>
        /// This will only fire a ship if it can fire. I.e., if it has waited
        /// the defined amount of frames since it last fired.
        /// </remarks>
        /// </summary>
        /// <param name="ship">Ship to fire</param>
        void Fire(Ship ship);

        /// <summary>
        /// Spawn a new ship onto the board.
        /// </summary>
        /// <param name="playerId">The ID of the ship</param>
        Ship SpawnNewShip(int playerId, String playerName);

        /// <summary>
        /// Remove a player from the world not to respawn.
        /// 
        /// <remarks>
        /// This is meant to be done when a client disconnects from the server.
        /// </remarks>
        /// </summary>
        /// <param name="playerId">ID of player to remove</param>
        void RemovePlayer(Int32 playerId);
    }
}
