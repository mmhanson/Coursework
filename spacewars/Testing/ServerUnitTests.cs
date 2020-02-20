using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Testing
{
    [TestClass]
    public class ServerUnitTests
    {
        // === TEST THE getJsonString METHOD ===

        /// <summary>
        /// Test that ids of ships, projectiles, and stars are serialized
        /// under the correct keys and that different instances (with
        /// different ids) have different serialized ids.
        /// </summary>
        [TestMethod]
        public void TestJsonIdSerialization()
        {
            // TODO
        }

        /// <summary>
        /// Test that serializations of ships, projs, and stars have
        /// all the required fields and no more.
        /// </summary>
        [TestMethod]
        public void TestJsonSerializedFieldCount()
        {
            // TODO
        }

        /// <summary>
        /// Test that serializations of ships, projs, and stars have
        /// correct fields for the properties of the class.
        /// </summary>
        [TestMethod]
        public void TestJsonSerializedFieldValues()
        {
            // TODO
        }

        // === TEST THE isValidCommand METHOD ===

        /// <summary>
        /// Test that a command with no parenthesis is invalid.
        /// </summary>
        [TestMethod]
        public void TestCommandNoParenthesis()
        {
            // TODO
        }

        /// <summary>
        /// Test that a command with one parenthesis is invalid.
        /// 
        /// Both sides (one on left, one on right) are tested individually.
        /// </summary>
        [TestMethod]
        public void TestCommandOneParenthesis()
        {
            // TODO
        }

        /// <summary>
        /// Test that a command with reversed parenthesis is invalid.
        /// 
        /// Both sides are tested individually as well as together.
        /// </summary>
        [TestMethod]
        public void TestCommandReversedParenthesis()
        {
            // TODO
        }

        /// <summary>
        /// Test that a regular command "(LFT)" is valid.
        /// </summary>
        [TestMethod]
        public void TestCommandRegularCommand()
        {
            // TODO
        }

        /// <summary>
        /// Test that several permutations of all four actions is a valid command "(FLRT)"
        /// </summary>
        [TestMethod]
        public void TestCommandFullCommand()
        {
            // TODO
        }

        /// <summary>
        /// Test that an empty command "()" is valid
        /// </summary>
        [TestMethod]
        public void TestCommandEmptyCommand()
        {
            // TODO
        }

        /// <summary>
        /// Test that a command with an action twice "(FFR)" is invalid
        /// </summary>
        [TestMethod]
        public void TestCommandRedundantCommand()
        {
            // TODO
        }

        /// <summary>
        /// Test that a random string is invalid.
        /// </summary>
        [TestMethod]
        public void TestCommandRandomString()
        {
            // TODO
        }
    }
}
