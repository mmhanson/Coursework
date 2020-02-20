// <author> Maxwell Hanson </author>
// <date> Sep 12, 2018 </date>
// <summary> A class representing a DependencyGraph </summary>

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;


namespace DevelopmentTests
{
    /// <summary>
    /// This is a test class for DependencyGraphTest and is intended
    /// to contain all DependencyGraphTest Unit Tests
    /// </summary>
    [TestClass()]
    public class DependencyGraphTest
    {
        // ===== TESTING THE SIZE OF THE GRAPH ===

        /// <summary>
        /// Test that a newly-created empty graph contains no relationships.
        /// </summary>
        [TestMethod()]
        public void EmptyGraphTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        /// Test that adding relationships to a graph increments the size.
        /// 
        /// Adds 4 relationships, testing the size after each addition.
        /// </summary>
        [TestMethod()]
        public void IncrementSizeTest()
        {
            DependencyGraph graph = new DependencyGraph();

            graph.AddDependency("a", "b");
            Assert.AreEqual(1, graph.Size);

            graph.AddDependency("a", "c");
            Assert.AreEqual(2, graph.Size);

            graph.AddDependency("c", "b");
            Assert.AreEqual(3, graph.Size);

            graph.AddDependency("b", "d");
            Assert.AreEqual(4, graph.Size);
        }

        /// <summary>
        /// Test that removing relationships to a graph decrements the size.
        /// 
        /// Adds 4 relationships and removes 4, testing the size before and after each removal.
        /// </summary>
        [TestMethod()]
        public void DecrementSizeTest()
        {
            DependencyGraph graph = new DependencyGraph();

            graph.AddDependency("a", "b");
            graph.AddDependency("a", "c");
            graph.AddDependency("c", "b");
            graph.AddDependency("b", "d");

            Assert.AreEqual(4, graph.Size);

            graph.RemoveDependency("a", "b");
            Assert.AreEqual(3, graph.Size);

            graph.RemoveDependency("a", "c");
            Assert.AreEqual(2, graph.Size);

            graph.RemoveDependency("c", "b");
            Assert.AreEqual(1, graph.Size);

            graph.RemoveDependency("b", "d");
            Assert.AreEqual(0, graph.Size);
        }

        /// <summary>
        /// Test that many interchanged add and remove statements correctly track the size.
        /// </summary>
        [TestMethod()]
        public void InterchangedAddRemoveSizeTest()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("x", "y");
            graph.AddDependency("a", "b");
            graph.AddDependency("a", "c");
            graph.AddDependency("a", "d");
            graph.AddDependency("c", "b");
            graph.RemoveDependency("a", "d");
            graph.AddDependency("e", "b");
            graph.AddDependency("b", "d");
            graph.RemoveDependency("e", "b");
            graph.RemoveDependency("x", "y");

            Assert.AreEqual(4, graph.Size);
        }

        // ===== TESTING DEPENDENT AND DEPENDEE FUNCTIONALITY OF THE GRAPH ===

        /// <summary>
        /// Test that adding duplicate dependencies does not affect the list of dependents and dependees.
        /// </summary>
        [TestMethod()]
        public void DuplicateDependentDependeesListTest()
        {
            // setup
            DependencyGraph DG = new DependencyGraph();
            DG.AddDependency("x", "y");
            DG.AddDependency("a", "b");
            DG.AddDependency("x", "y");

            // test states
            IEnumerator<String> xDependentsEnum = DG.GetDependents("x").GetEnumerator();
            IEnumerator<String> yDependeesEnum = DG.GetDependees("y").GetEnumerator();
            Assert.IsTrue(xDependentsEnum.MoveNext());
            Assert.IsTrue(yDependeesEnum.MoveNext());
            Assert.AreEqual("y", xDependentsEnum.Current);
            Assert.AreEqual("x", yDependeesEnum.Current);
        }

        /// <summary>
        /// Test that the bracket operator of the dependency graph class operates correctly.
        /// 
        /// The bracket operator takes a string and returns the count of dependees of that string
        /// </summary>
        [TestMethod()]
        public void BracketOperatorTest()
        {
            DependencyGraph DG = new DependencyGraph();

            // test one dependency
            DG.AddDependency("x", "y");
            Assert.AreEqual(1, DG["y"]);

            // test multiple (pairwise disjoint) dependencies
            DG.AddDependency("a", "b");
            DG.AddDependency("i", "j");
            Assert.AreEqual(1, DG["y"]);
            Assert.AreEqual(1, DG["b"]);
            Assert.AreEqual(1, DG["j"]);

            // test multiple (overlapping) dependencies
            DG = new DependencyGraph();
            DG.AddDependency("x", "y");
            DG.AddDependency("a", "y");
            DG.AddDependency("b", "y");
            Assert.AreEqual(3, DG["y"]);

            // test multiple (some overlapping others not) dependencies
            DG = new DependencyGraph();
            DG.AddDependency("x", "y");
            DG.AddDependency("a", "y");
            DG.AddDependency("i", "j");
            Assert.AreEqual(2, DG["y"]);
            Assert.AreEqual(1, DG["j"]);

            // test removing a dependency
            DG = new DependencyGraph();
            DG.AddDependency("x", "y");
            DG.RemoveDependency("x", "y");
            Assert.AreEqual(0, DG["y"]);

            // test a nonexistent dependency
            DG = new DependencyGraph();
            Assert.AreEqual(0, DG["y"]);

            // test that replacing the dependees of a dependent updates the data
            DG = new DependencyGraph();
            DG.AddDependency("x", "y");
            HashSet<String> yDependees = new HashSet<string>();
            yDependees.Add("a");
            yDependees.Add("b");
            yDependees.Add("c");
            DG.ReplaceDependees("y", yDependees);
            Assert.AreEqual(3, DG["y"]);
        }

        /// <summary>
        /// <para>Test that adding a (sole) relationship (x, y) to a DG causes:</para>
        ///     * 'y' is the sole dependent of 'x'
        ///     * 'y' is the sole dependee of 'x'
        ///     * 'x' has no dependees
        ///     * 'y' has no dependents
        /// </summary>
        /// <remarks>Dependents and Dependees are tested both with GetDependent/Dependee and HasDependent/Dependee methods</remarks>
        [TestMethod()]
        public void AddingDependencyTest()
        {
            DependencyGraph DG = new DependencyGraph();
            DG.AddDependency("x", "y");
            
            // test that 'y' has 'x' as its sole dependee with the GetDependees method
            IEnumerator<string> yDependeeEnum = DG.GetDependees("y").GetEnumerator();
            Assert.IsTrue(yDependeeEnum.MoveNext());       // test to see if e1 has at least one element (y has a dependee)
            Assert.AreEqual("x", yDependeeEnum.Current);   // test that the one element is x (y's dependee is x)
            Assert.IsFalse(yDependeeEnum.MoveNext());      // test that e1 has no other elements (y has no other dependees)

            // test that 'y' has no dependents with the GetDependents method
            IEnumerator<string> yDependentEnum = DG.GetDependents("y").GetEnumerator();
            Assert.IsFalse(yDependentEnum.MoveNext());

            // test that 'x' has 'y' as its sole dependent with the GetDependent method
            IEnumerator<string> xDependentEnum = DG.GetDependents("x").GetEnumerator();
            Assert.IsTrue(xDependentEnum.MoveNext());       // test that 'x' has a dependent
            Assert.AreEqual("y", xDependentEnum.Current);   // test that 'y' is x's sole dependent
            Assert.IsFalse(xDependentEnum.MoveNext());      // test that 'x' has no other dependents

            // test that 'x' has no dependees with the GetDependents method
            IEnumerator<string> xDependeeEnum = DG.GetDependees("x").GetEnumerator();
            Assert.IsFalse(xDependeeEnum.MoveNext());

            // test that 'y' has a dependee, but not a dependant with the HasDependent method
            Assert.IsTrue(DG.HasDependees("y"));
            Assert.IsFalse(DG.HasDependents("y"));

            // test that 'x' has a dependant, but not a dependee with the HasDependent method
            Assert.IsTrue(DG.HasDependents("x"));
            Assert.IsFalse(DG.HasDependees("x"));
        }

        /// <summary>
        /// Test that removing (x, y) from a DG causes:
        ///   * x to not have any dependencies
        ///   * y to not have any dependencies
        ///   * x to not have any dependees
        ///   * x to not have any dependencies
        /// </summary>
        /// <remarks>Dependents and Dependees are tested both with GetDependent/Dependee and HasDependent/Dependee methods</remarks>
        [TestMethod()]
        public void RemovingDependencyTest()
        {
            DependencyGraph DG = new DependencyGraph();
            DG.AddDependency("x", "y");
            DG.RemoveDependency("x", "y");

            Assert.IsFalse(DG.GetDependees("y").GetEnumerator().MoveNext());    // check y no dependees (GetDependents/GetDependees methods)
            Assert.IsFalse(DG.GetDependents("y").GetEnumerator().MoveNext());   // check y no dependents
            Assert.IsFalse(DG.HasDependees("y"));                               // check y no dependees (HasDependees/HasDependents methods)
            Assert.IsFalse(DG.HasDependents("y"));                              // check y no dependents

            Assert.IsFalse(DG.GetDependees("x").GetEnumerator().MoveNext());    // check x no dependees (GetDependents/GetDependees methods)
            Assert.IsFalse(DG.GetDependents("x").GetEnumerator().MoveNext());   // check x no dependents
            Assert.IsFalse(DG.HasDependees("x"));                               // check x no dependees (HasDependees/HasDependents methods)
            Assert.IsFalse(DG.HasDependents("x"));                              // check x no dependents

            Assert.AreEqual(DG.Size, 0);                                        // check that DG has no relationships, note this is redundant but also i dont really care
        }

        /// <summary>
        /// Test that removing a dependency that is not in the graph doesn't throw an error and doesnt affect the size.
        /// </summary>
        [TestMethod()]
        public void RemovingNonexistentDependencyTest()
        {
            DependencyGraph DG = new DependencyGraph();
            DG.RemoveDependency("x", "y");
            Assert.AreEqual(0, DG.Size);
        }

        /// <summary>
        /// Test that add a dependency already in the graph doesn't throw an error and doesnt affect the size.
        /// </summary>
        [TestMethod()]
        public void AddingExistentDependencyTest()
        {
            DependencyGraph DG = new DependencyGraph();
            DG.AddDependency("x", "y");
            DG.AddDependency("x", "y");
            Assert.AreEqual(1, DG.Size);
        }

        /// <summary>
        /// Test that replacing the dependents with a set of one of a dependency in a DG
        /// with that dependency causes the expected outcome.
        /// </summary>
        [TestMethod()]
        public void ReplaceOneDependentsExistentRelationship()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("x", "y");

            // Change the dependee of x.
            // Dependency graph before:
            // x -> y
            HashSet<String> dependentSet = new HashSet<string>();
            dependentSet.Add("z");
            graph.ReplaceDependents("x", dependentSet);
            // Dependency graph now:
            // x -> z

            // test that 'z' is the dependent of 'x'
            IEnumerator<String> xDependents = graph.GetDependents("x").GetEnumerator();
            Assert.IsTrue(xDependents.MoveNext());
            Assert.AreEqual("z", xDependents.Current);

            // test that 'x' is the dependee of 'z'
            IEnumerator<String> zDependees = graph.GetDependees("z").GetEnumerator();
            Assert.IsTrue(zDependees.MoveNext());
            Assert.AreEqual("x", zDependees.Current);
        }

        /// <summary>
        /// Test that replacing the dependents with a set of many of a dependency in a DG
        /// with that dependency causes the expected outcome.
        /// </summary>
        [TestMethod()]
        public void ReplaceManyDependentsExistentRelationship()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("x", "y");

            // Change the dependee of x.
            // Dependency graph before:
            // x -> y
            HashSet<String> dependentSet = new HashSet<string>();
            dependentSet.Add("z");
            dependentSet.Add("a");
            graph.ReplaceDependents("x", dependentSet);
            // Dependency graph now:
            // x -> z
            // |
            // +--> a

            // test that 'z' and "a" is the dependent of 'x'
            IEnumerator<String> xDependents = graph.GetDependents("x").GetEnumerator();
            Assert.IsTrue(xDependents.MoveNext());
            String xDependent0 = xDependents.Current; // note down dependents for testing
            Assert.IsTrue(xDependents.MoveNext());
            String xDependent1 = xDependents.Current;
            Assert.IsTrue((xDependent0.Equals("a") && xDependent1.Equals("z")) || (xDependent0.Equals("z") && xDependent1.Equals("a"))); // test dependents in either order

            // test that 'x' is the dependee of 'z'
            IEnumerator <String> zDependees = graph.GetDependees("z").GetEnumerator();
            Assert.IsTrue(zDependees.MoveNext());
            Assert.AreEqual("x", zDependees.Current);

            // test that 'x' is the dependee of 'a
            IEnumerator<String> aDependees = graph.GetDependees("a").GetEnumerator();
            Assert.IsTrue(aDependees.MoveNext());
            Assert.AreEqual("x", aDependees.Current);
        }

        /// <summary>
        /// Test that replacing the dependees with a set of one of a dependency in a DG
        /// with that dependency causes the expected outcome.
        /// </summary>
        [TestMethod()]
        public void ReplaceOneDependeesExistentRelationship()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("x", "y");

            // Change the dependee of y.
            // Dependency graph before:
            // x -> y
            HashSet<String> dependeeSet = new HashSet<string>();
            dependeeSet.Add("z");
            graph.ReplaceDependees("y", dependeeSet);
            // Dependency graph now:
            // z -> y

            // test that 'y' is the dependent of 'z'
            IEnumerator<String> zDependents = graph.GetDependents("z").GetEnumerator();
            Assert.IsTrue(zDependents.MoveNext());
            Assert.AreEqual("y", zDependents.Current);

            // test that 'z' is the dependee of 'y'
            IEnumerator<String> yDependees = graph.GetDependees("y").GetEnumerator();
            Assert.IsTrue(yDependees.MoveNext());
            Assert.AreEqual("z", yDependees.Current);
        }

        /// <summary>
        /// Test that replacing the dependees with a set of many of a dependency in a DG
        /// with that dependency causes the expected outcome.
        /// </summary>
        [TestMethod()]
        public void ReplaceManyDependeesExistentRelationship()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("x", "y");

            // Change the dependees of y.
            // Dependency graph before:
            // x -> y
            HashSet<String> dependeeSet = new HashSet<string>();
            dependeeSet.Add("z");
            dependeeSet.Add("a");
            graph.ReplaceDependees("y", dependeeSet);
            // Dependency graph now:
            // z -> y
            //      ^
            // a ---+

            // test that 'y' is the dependent of 'z'
            IEnumerator<String> zDependents = graph.GetDependents("z").GetEnumerator();
            Assert.IsTrue(zDependents.MoveNext());
            Assert.AreEqual("y", zDependents.Current);;

            // test that 'y' is the dependent of 'a'
            IEnumerator<String> aDependents = graph.GetDependents("a").GetEnumerator();
            Assert.IsTrue(aDependents.MoveNext());
            Assert.AreEqual("y", aDependents.Current);

            // test that 'z' and 'a' is the dependee of 'y'
            IEnumerator<String> yDependees = graph.GetDependees("y").GetEnumerator();
            Assert.IsTrue(yDependees.MoveNext());
            String yDependee0 = yDependees.Current; // note the dependencies in variables
            Assert.IsTrue(yDependees.MoveNext());
            String yDependee1 = yDependees.Current;
            Assert.IsTrue(yDependee0.Equals("a") && yDependee1.Equals("z") || yDependee0.Equals("z") && yDependee1.Equals("a")); // test dependencies in either order
        }

        /// <summary>
        /// Test that replacing the dependents and dependees of a dependency in a DG in which that dependency has been removed
        /// does not throw any errors and does not affect the size of the DG.
        /// 
        /// This should not cause any errors
        /// </summary>
        [TestMethod()]
        public void ReplaceDependentsDependeesRemovedRelationship()
        {
            DependencyGraph graph = new DependencyGraph();

            // add and remove a relationship
            graph.AddDependency("x", "y");
            graph.RemoveDependency("x", "y");

            // test replacing the dependents/dependees of the elements of the relationship
            // these operations should not fail even though the relationship doesn't exist anymore
            Assert.AreEqual(graph.Size, 0);
            graph.ReplaceDependents("x", new HashSet<string>());
            graph.ReplaceDependees("y", new HashSet<string>());
            Assert.AreEqual(graph.Size, 0);

            // do the same but with nonempty replacement dependents, dependees
            Assert.AreEqual(graph.Size, 0);

            HashSet<String> dependentSet = new HashSet<string>();
            dependentSet.Add("a");
            dependentSet.Add("b");
            dependentSet.Add("c");
            HashSet<String> dependeeSet = new HashSet<string>();
            dependentSet.Add("d");
            dependentSet.Add("e");
            dependentSet.Add("f");

            graph.ReplaceDependents("x", dependentSet);
            graph.ReplaceDependees("y", dependeeSet);
            Assert.AreEqual(graph.Size, 6);
        }

        /// <summary>
        /// Test that replacing the dependents and dependees of a dependency in a DG which has never had that dependency
        /// does not throw any errors and does not affect the size of the DG.
        /// 
        /// This should not cause any errors
        /// </summary>
        [TestMethod()]
        public void ReplaceDependentsDependeesNonexistentRelationship()
        {
            DependencyGraph graph = new DependencyGraph();

            // test replacing the dependents/dependees of the elements of the relationship
            // these operations should not fail even though the relationship doesn't exist anymore
            Assert.AreEqual(graph.Size, 0);
            graph.ReplaceDependents("x", new HashSet<string>());
            graph.ReplaceDependees("y", new HashSet<string>());
            Assert.AreEqual(graph.Size, 0);

            // do the same but with nonempty replacement dependents, dependees
            Assert.AreEqual(graph.Size, 0);

            HashSet<String> dependentSet = new HashSet<string>();
            dependentSet.Add("a");
            dependentSet.Add("b");
            dependentSet.Add("c");
            HashSet<String> dependeeSet = new HashSet<string>();
            dependeeSet.Add("d");
            dependeeSet.Add("e");
            dependeeSet.Add("f");

            graph.ReplaceDependents("x", dependentSet);
            graph.ReplaceDependees("y", dependeeSet);
            Assert.AreEqual(graph.Size, 6);
        }

        /// <summary>
        /// Test making two dependency graphs at the same time. The graphs should not be equal.
        /// 
        /// This is tested by adding a relationship to one and not the other, and testing that it is
        /// in one and not the other.
        /// </summary>
        [TestMethod()]
        public void StaticTest()
        {
            DependencyGraph graph0 = new DependencyGraph();
            DependencyGraph graph1 = new DependencyGraph();

            graph0.AddDependency("x", "y");
            Assert.AreEqual(1, graph0.Size);
            Assert.AreEqual(0, graph1.Size);
        }

        /// <summary>
        /// Test that GetDependees returns the correct results for each node in a graph with several relationships.
        /// </summary>
        [TestMethod()]
        public void MultipleDependenciesTest()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            graph.AddDependency("a", "c");
            graph.AddDependency("c", "b");
            graph.AddDependency("b", "d");

            // test that 'a' has no dependees
            IEnumerator<string> e = graph.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            // test that 'b' has three dependees, 'a', 'c' (not necessarily in order)
            e = graph.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            // test that 'c' has one depencency, 'a'
            e = graph.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            // test that 'd' has one dependency, 'b'
            e = graph.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        /// Test that adding redundant relationships do not increment the size.
        /// </summary>
        [TestMethod()]
        public void RedundantDependecyTests()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            graph.AddDependency("a", "c");
            graph.AddDependency("a", "b"); // redundant
            graph.AddDependency("c", "b");
            graph.AddDependency("b", "d");
            graph.AddDependency("c", "b"); // redundant

            Assert.AreEqual(4, graph.Size);
        }

        /// <summary>
        /// Test that using interchanged 'ReplaceDependents' and 'ReplaceDependees' gives the correct results.
        /// </summary>
        [TestMethod()]
        public void InterchangedReplaceSizeTest()
        {
            IEnumerator<String> enumerator;
            DependencyGraph graph = new DependencyGraph();

            // add and remove dependencies in scrambled order
            // ReplaceDependents, ReplaceDependees is used here for rearrangement
            graph.AddDependency("x", "b");
            graph.AddDependency("a", "z");
            graph.ReplaceDependents("b", new HashSet<string>()); // b already has no dependents
            graph.AddDependency("y", "b"); // b now depends on x and y
            graph.ReplaceDependents("a", new HashSet<string>() { "c" }); // c now depends on a
            graph.AddDependency("w", "d"); // new relationship
            graph.ReplaceDependees("b", new HashSet<string>() { "a", "c" }); // b now depends on a, c instead of x, y
            graph.ReplaceDependees("d", new HashSet<string>() { "b" }); // d now depends on 'b' only instead of w

            // graph is now supossed to look like:
            // 
            // (a) ---> (c)
            //  |        |
            //  V        |
            // (b) <-----+
            //  |
            //  V
            // (d)

            // test that 'a' has no dependees
            enumerator = graph.GetDependees("a").GetEnumerator();
            Assert.IsFalse(enumerator.MoveNext());

            // test that 'b' has 'a', 'c' as dependencies (unordered)
            enumerator = graph.GetDependees("b").GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            String s1 = enumerator.Current;
            Assert.IsTrue(enumerator.MoveNext());
            String s2 = enumerator.Current;
            Assert.IsFalse(enumerator.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            // test that 'c' has 'a' as its only dependency
            enumerator = graph.GetDependees("c").GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual("a", enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());

            // test that 'd' has 'b' as its only dependency
            enumerator = graph.GetDependees("d").GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual("b", enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
        }

        /// <summary>
        /// Stress-test the dependency graph by dumping a lot of relationships into it and checking if they are correct.
        /// </summary>
        [TestMethod()]
        public void DependencyGraphStressTest()
        {
            DependencyGraph graph = new DependencyGraph();

            // generate a lot of characters to use as node letters
            const int CHARSIZE = 200;
            string[] chars = new string[CHARSIZE];
            for (int idx = 0; idx < CHARSIZE; idx++)
            {
                chars[idx] = ("" + (char)('a' + idx));
            }
            
            // these variables are a reference of the correct arrangement of dependents/dependees
            // to check the against the graph after loading them all in. This is done with an array
            // of sets for dependents and dependees where each set of an index gives the dependents
            // and dependees for any character.
            HashSet<string>[] dependents = new HashSet<string>[CHARSIZE];
            HashSet<string>[] dependees = new HashSet<string>[CHARSIZE];
            for (int idx = 0; idx < CHARSIZE; idx++)
            {
                dependents[idx] = new HashSet<string>();
                dependees[idx] = new HashSet<string>();
            }

            // Add a bunch of dependencies to the graph and the arrays
            for (int idx0 = 0; idx0 < CHARSIZE; idx0++)
            {
                for (int idx1 = idx0 + 1; idx1 < CHARSIZE; idx1++)
                {
                    graph.AddDependency(chars[idx0], chars[idx1]);
                    dependents[idx0].Add(chars[idx1]);
                    dependees[idx1].Add(chars[idx0]);
                }
            }

            // Remove a bunch of dependencies from the graph and arrays (semirandomly)
            for (int idx0 = 0; idx0 < CHARSIZE; idx0++)
            {
                for (int idx1 = idx0 + 4; idx1 < CHARSIZE; idx1 += 4)
                {
                    graph.RemoveDependency(chars[idx0], chars[idx1]);
                    dependents[idx0].Remove(chars[idx1]);
                    dependees[idx1].Remove(chars[idx0]);
                }
            }

            // Add some dependencies back (semirandomly)
            for (int idx0 = 0; idx0 < CHARSIZE; idx0++)
            {
                for (int idx1 = idx0 + 1; idx1 < CHARSIZE; idx1 += 2)
                {
                    graph.AddDependency(chars[idx0], chars[idx1]);
                    dependents[idx0].Add(chars[idx1]);
                    dependees[idx1].Add(chars[idx0]);
                }
            }

            // Remove some more (semirandomly)
            for (int idx0 = 0; idx0 < CHARSIZE; idx0 += 2)
            {
                for (int idx1 = idx0 + 3; idx1 < CHARSIZE; idx1 += 3)
                {
                    graph.RemoveDependency(chars[idx0], chars[idx1]);
                    dependents[idx0].Remove(chars[idx1]);
                    dependees[idx1].Remove(chars[idx0]);
                }
            }

            // check that the dependency graph kept track of the correct answers
            for (int idx = 0; idx < CHARSIZE; idx++)
            {
                Assert.IsTrue(dependents[idx].SetEquals(new HashSet<string>(graph.GetDependents(chars[idx]))));
                Assert.IsTrue(dependees[idx].SetEquals(new HashSet<string>(graph.GetDependees(chars[idx]))));
            }
        }

    }
}



