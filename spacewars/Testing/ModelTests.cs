using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace Testing
{
    /// <summary>
    /// Some very basic unit tests for the model.
    /// </summary>
    [TestClass]
    public class ModelTests
    {
        /// <summary>
        /// Tests the constructor integrity of the world
        /// </summary>
        [TestMethod]
        public void TestWorldInitialization()
        {
            IWorld w = new SpaceWarsWorld(1000);
            Assert.AreEqual(w.Size, 1000);

            w = new SpaceWarsWorld();
            Assert.AreEqual(w.Size, 750);

            w = new SpaceWarsWorld(@"..\..\..\Resources\test.xml");
            Assert.AreEqual(w.Size, 1250);
            PrivateObject pw = new PrivateObject(w, new PrivateType(typeof(SpaceWarsWorld)));
            // Verify world properties
            Assert.AreEqual(10, (int)pw.GetFieldOrProperty("StartingHP"));
            Assert.AreEqual(5, (int)pw.GetFieldOrProperty("ProjectileFiringDelay"));
            Assert.AreEqual(200, (int)pw.GetFieldOrProperty("RespawnDelay"));
            Assert.IsTrue(Math.Abs((double)pw.GetFieldOrProperty("EngineStrength") - 0.1) < 0.000001);
            Assert.IsTrue(Math.Abs((double)pw.GetFieldOrProperty("TurnRate") - 3) < 0.000001);
            Assert.IsTrue(Math.Abs((double)pw.GetFieldOrProperty("ProjectileVelocity") - 18) < 0.000001);
            Assert.AreEqual(w.Stars.Count, 3);

            try
            {
                w = new SpaceWarsWorld("rubbish");
            }
            catch (System.IO.FileNotFoundException) { }
        }

        /// <summary>
        /// Examines the functionality of the DetectCollisions method
        /// </summary>
        [TestMethod]
        public void TestCollisions()
        {
            IWorld w = new SpaceWarsWorld();
            Ship s1 = new Ship(0, "a", new SpaceWars.Vector2D(0, 0), new SpaceWars.Vector2D(0, -1), false, 5, 0, 200, 6);
            Ship s2 = new Ship(1, "b", new SpaceWars.Vector2D(0, -45), new SpaceWars.Vector2D(0, -1), false, 5, 0, 200, 6);
            w.Ships.Add(s1);
            w.Ships.Add(s2);
            for (int j = 0; j < 5; j++)
            {
                w.Fire(s1);
                for (int i = 0; i < 6; i++)
                {
                    w.Advance();
                }
            }
            Assert.IsTrue(s2.IsDead);
            w.Stars.Add(new Star(0, 100, 100, 5));
            // Test Star collisions
            for (int i = 0; i < 150; i++)
            {
                w.Advance();
            }
            Assert.IsTrue(s1.IsDead);
        }

        [TestMethod]
        public void TestMotionControls()
        {
            IWorld w = new SpaceWarsWorld();
            Ship s = new Ship();
            s = new Ship(6);
            s = new Ship(5, "a", new SpaceWars.Vector2D(0, 0), new SpaceWars.Vector2D(0, -1), false, 5, 0, 200, 6);
            w.SpawnNewShip(0, "jim");
            w.Ships.Add(s);
            w.Turn(s, true);
            w.Thrust(s);
            w.Advance();
            s.CanShoot = false;
            w.Fire(s);
            w.Advance();
            s.CanShoot = true;
            w.Fire(s);
            w.Advance();
            Assert.AreNotEqual(s.Direction, new SpaceWars.Vector2D(0, -1));
            s.TurningClockwise = false;
            w.Turn(s, false);
            w.Advance();
            w.Advance();
            w.Advance();
            Assert.AreEqual(s.Direction, new SpaceWars.Vector2D(0, -1));
        }
    }
}
