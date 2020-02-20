using SpaceWars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class EnemyShip: Ship
    {
        /// <summary>
        /// Create an enemy ship.
        /// 
        /// <remarks>
        /// Score of ship is zero. Score is never meant to increase even if the
        /// enemy kills a player.
        /// 
        /// Enemy ships are dead when they spawn. They will respawn after the parameterized
        /// amount of frames.
        /// </remarks>
        /// </summary>
        /// <param name="id">id number of the ship</param>
        /// <param name="enemyName">name of the ship</param>
        /// <param name="location">location of the ship</param>
        /// <param name="direction">direction of the ship</param>
        /// <param name="thrust">thrust of the ship</param>
        /// <param name="hp">hitpoints of the ship.</param>
        /// <param name="respawnFrames">The amount of frames the ship must wait to respawn</param>
        /// <param name="shootDelay">The amount of frames the enemy must wait to shoot again</param>
        public EnemyShip(int id, string enemyName, Vector2D location, Vector2D direction, bool thrust, int hp, int respawnFrames, int shootDelay) :
            base(id, enemyName, location, direction, thrust, hp, 0, respawnFrames, shootDelay)
        {
            // enemy-specific settings
            this.IsEnemy = true;

            // kill the ship to respawn later
            this.IsDead = true;
            this.HitPoints = 0;
        }
    }
}
