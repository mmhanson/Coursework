// These tests are for private use only
// Redistributing this file is strictly against SoC policy.

using SpreadsheetUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace PS2GradingTests
{
  /// <summary>
  ///This is a test class for DependencyGraphTest and is intended
  ///to contain all DependencyGraphTest Unit Tests
  ///</summary>
  [TestClass()]
  public class DependencyGraphTest
  {

    // ************************** TESTS ON EMPTY DGs ************************* //

    /// <summary>
    ///Empty graph should contain nothing
    ///</summary>
    [TestMethod()]
    public void TestZeroSize()
    {
      DependencyGraph t = new DependencyGraph();
      Assert.AreEqual(0, t.Size);
    }

    /// <summary>
    ///Empty graph should contain nothing
    ///</summary>
    [TestMethod()]
    public void TestNoDepends()
    {
      DependencyGraph t = new DependencyGraph();
      Assert.IsFalse(t.HasDependees("x"));
      Assert.IsFalse(t.HasDependents("x"));
    }

    /// <summary>
    ///Empty graph should contain nothing
    ///</summary>
    [TestMethod()]
    public void TestEmptyEnumerator()
    {
      DependencyGraph t = new DependencyGraph();
      Assert.IsFalse(t.GetDependees("x").GetEnumerator().MoveNext());
      Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
    }

    /// <summary>
    ///Empty graph should contain nothing
    ///</summary>
    [TestMethod()]
    public void TestEmptyIndexer()
    {
      DependencyGraph t = new DependencyGraph();
      Assert.AreEqual(0, t["x"]);
    }

    /// <summary>
    ///Removing from an empty DG shouldn't fail
    ///</summary>
    [TestMethod()]
    public void TestRemoveFromEmpty()
    {
      DependencyGraph t = new DependencyGraph();
      t.RemoveDependency("x", "y");
    }

    /// <summary>
    ///Replace on an empty DG shouldn't fail
    ///</summary>
    [TestMethod()]
    public void TestReplaceEmpty()
    {
      DependencyGraph t = new DependencyGraph();
      t.ReplaceDependents("x", new HashSet<string>());
      t.ReplaceDependees("y", new HashSet<string>());
    }


    // ************************ MORE TESTS ON EMPTY DGs *********************** //

    /// <summary>
    ///Empty graph should contain nothing
    ///</summary>
    [TestMethod()]
    public void TestAddRemoveEmpty()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "y");
      Assert.AreEqual(1, t.Size);
      t.RemoveDependency("x", "y");
      Assert.AreEqual(0, t.Size);
    }

    /// <summary>
    ///Empty graph should contain nothing
    ///</summary>
    [TestMethod()]
    public void TestAddRemoveEmpty2()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "y");
      Assert.IsTrue(t.HasDependees("y"));
      Assert.IsTrue(t.HasDependents("x"));
      t.RemoveDependency("x", "y");
      Assert.IsFalse(t.HasDependees("y"));
      Assert.IsFalse(t.HasDependents("x"));
    }

    /// <summary>
    ///Empty graph should contain nothing
    ///</summary>
    [TestMethod()]
    public void TestComplexAddRemoveEmpty()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "y");
      IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
      Assert.IsTrue(e1.MoveNext());
      Assert.AreEqual("x", e1.Current);
      IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
      Assert.IsTrue(e2.MoveNext());
      Assert.AreEqual("y", e2.Current);
      t.RemoveDependency("x", "y");
      Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
      Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
    }

    /// <summary>
    ///Empty graph should contain nothing
    ///</summary>
    [TestMethod()]
    public void TestAddRemoveIndexerEmpty()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "y");
      Assert.AreEqual(1, t["y"]);
      t.RemoveDependency("x", "y");
      Assert.AreEqual(0, t["y"]);
    }

    /// <summary>
    ///Removing from an empty DG shouldn't fail
    ///</summary>
    [TestMethod()]
    public void TestRemoveTwice()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "y");
      Assert.AreEqual(t.Size, 1);
      t.RemoveDependency("x", "y");
      t.RemoveDependency("x", "y");
    }

    /// <summary>
    ///Replace on an empty DG shouldn't fail
    ///</summary>
    [TestMethod()]
    public void TestRemoveReplace()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "y");
      Assert.AreEqual(t.Size, 1);
      t.RemoveDependency("x", "y");
      t.ReplaceDependents("x", new HashSet<string>());
      t.ReplaceDependees("y", new HashSet<string>());
    }


    // ********************** Making Sure that Static Variables Weren't Used ****************** //
    ///<summary>
    ///It should be possibe to have more than one DG at a time.  This test is
    ///repeated because I want it to be worth more than 1 point.
    ///</summary>
    [TestMethod()]
    public void TestStatic1()
    {
      DependencyGraph t1 = new DependencyGraph();
      DependencyGraph t2 = new DependencyGraph();
      t1.AddDependency("x", "y");
      Assert.AreEqual(1, t1.Size);
      Assert.AreEqual(0, t2.Size);
    }

    // Increase the weight of this test
    [TestMethod()]
    public void TestStatic2()
    {
      TestStatic1();
    }

    [TestMethod()]
    public void TestStatic3()
    {
      TestStatic1();
    }

    [TestMethod()]
    public void TestStatic4()
    {
      TestStatic1();
    }

    [TestMethod()]
    public void TestStatic5()
    {
      TestStatic1();
    }

    /**************************** SIMPLE NON-EMPTY TESTS ****************************/

    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestSimpleSize()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("c", "b");
      t.AddDependency("b", "d");
      Assert.AreEqual(4, t.Size);
    }


    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestSimpleIndexer()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("c", "b");
      t.AddDependency("b", "d");
      Assert.AreEqual(2, t["b"]);
    }

    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestSimpleHasDeps()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("c", "b");
      t.AddDependency("b", "d");
      Assert.IsTrue(t.HasDependents("a"));
      Assert.IsFalse(t.HasDependees("a"));
      Assert.IsTrue(t.HasDependents("b"));
      Assert.IsTrue(t.HasDependees("b"));
    }

    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestSimpleEnumerator()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("c", "b");
      t.AddDependency("b", "d");

      IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependees("b").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      String s1 = e.Current;
      Assert.IsTrue(e.MoveNext());
      String s2 = e.Current;
      Assert.IsFalse(e.MoveNext());
      Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

      e = t.GetDependees("c").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("a", e.Current);
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependees("d").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("b", e.Current);
      Assert.IsFalse(e.MoveNext());
    }

    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestSimpleEnumerator2()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("c", "b");
      t.AddDependency("b", "d");

      IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      String s1 = e.Current;
      Assert.IsTrue(e.MoveNext());
      String s2 = e.Current;
      Assert.IsFalse(e.MoveNext());
      Assert.IsTrue(((s1 == "b") && (s2 == "c")) || ((s1 == "c") && (s2 == "b")));

      e = t.GetDependents("b").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("d", e.Current);
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependents("c").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("b", e.Current);
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependents("d").GetEnumerator();
      Assert.IsFalse(e.MoveNext());
    }

    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestDuplicatesSize()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("a", "b");
      t.AddDependency("c", "b");
      t.AddDependency("b", "d");
      t.AddDependency("c", "b");
      Assert.AreEqual(4, t.Size);
    }

    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestDuplicatesIndexer()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("a", "b");
      t.AddDependency("c", "b");
      t.AddDependency("b", "d");
      t.AddDependency("c", "b");
      Assert.AreEqual(2, t["b"]);
    }

    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestDuplicatesDeps()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("a", "b");
      t.AddDependency("c", "b");
      t.AddDependency("b", "d");
      t.AddDependency("c", "b");
      Assert.IsTrue(t.HasDependents("a"));
      Assert.IsFalse(t.HasDependees("a"));
      Assert.IsTrue(t.HasDependents("b"));
      Assert.IsTrue(t.HasDependees("b"));
    }

    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestDuplicatesEnumerator()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("a", "b");
      t.AddDependency("c", "b");
      t.AddDependency("b", "d");
      t.AddDependency("c", "b");

      IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependees("b").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      String s1 = e.Current;
      Assert.IsTrue(e.MoveNext());
      String s2 = e.Current;
      Assert.IsFalse(e.MoveNext());
      Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

      e = t.GetDependees("c").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("a", e.Current);
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependees("d").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("b", e.Current);
      Assert.IsFalse(e.MoveNext());
    }

    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestDuplicatesEnumerator2()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("a", "b");
      t.AddDependency("c", "b");
      t.AddDependency("b", "d");
      t.AddDependency("c", "b");

      IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      String s1 = e.Current;
      Assert.IsTrue(e.MoveNext());
      String s2 = e.Current;
      Assert.IsFalse(e.MoveNext());
      Assert.IsTrue(((s1 == "b") && (s2 == "c")) || ((s1 == "c") && (s2 == "b")));

      e = t.GetDependents("b").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("d", e.Current);
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependents("c").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("b", e.Current);
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependents("d").GetEnumerator();
      Assert.IsFalse(e.MoveNext());
    }


    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestComplexAddRemove()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "y");
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("a", "d");
      t.AddDependency("c", "b");
      t.RemoveDependency("a", "d");
      t.AddDependency("e", "b");
      t.AddDependency("b", "d");
      t.RemoveDependency("e", "b");
      t.RemoveDependency("x", "y");
      Assert.AreEqual(4, t.Size);
    }

    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestAddRemoveIndexer()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "y");
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("a", "d");
      t.AddDependency("c", "b");
      t.RemoveDependency("a", "d");
      t.AddDependency("e", "b");
      t.AddDependency("b", "d");
      t.RemoveDependency("e", "b");
      t.RemoveDependency("x", "y");
      Assert.AreEqual(2, t["b"]);
    }

    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestAddRemoveDeps()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "y");
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("a", "d");
      t.AddDependency("c", "b");
      t.RemoveDependency("a", "d");
      t.AddDependency("e", "b");
      t.AddDependency("b", "d");
      t.RemoveDependency("e", "b");
      t.RemoveDependency("x", "y");
      Assert.IsTrue(t.HasDependents("a"));
      Assert.IsFalse(t.HasDependees("a"));
      Assert.IsTrue(t.HasDependents("b"));
      Assert.IsTrue(t.HasDependees("b"));
    }


    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestComplexEnumerator()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "y");
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("a", "d");
      t.AddDependency("c", "b");
      t.RemoveDependency("a", "d");
      t.AddDependency("e", "b");
      t.AddDependency("b", "d");
      t.RemoveDependency("e", "b");
      t.RemoveDependency("x", "y");

      IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependees("b").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      String s1 = e.Current;
      Assert.IsTrue(e.MoveNext());
      String s2 = e.Current;
      Assert.IsFalse(e.MoveNext());
      Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

      e = t.GetDependees("c").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("a", e.Current);
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependees("d").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("b", e.Current);
      Assert.IsFalse(e.MoveNext());
    }

    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestComplexEnumerator2()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "y");
      t.AddDependency("a", "b");
      t.AddDependency("a", "c");
      t.AddDependency("a", "d");
      t.AddDependency("c", "b");
      t.RemoveDependency("a", "d");
      t.AddDependency("e", "b");
      t.AddDependency("b", "d");
      t.RemoveDependency("e", "b");
      t.RemoveDependency("x", "y");

      IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      String s1 = e.Current;
      Assert.IsTrue(e.MoveNext());
      String s2 = e.Current;
      Assert.IsFalse(e.MoveNext());
      Assert.IsTrue(((s1 == "b") && (s2 == "c")) || ((s1 == "c") && (s2 == "b")));

      e = t.GetDependents("b").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("d", e.Current);
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependents("c").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("b", e.Current);
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependents("d").GetEnumerator();
      Assert.IsFalse(e.MoveNext());
    }


    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestComplexReplace()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "b");
      t.AddDependency("a", "z");
      t.ReplaceDependents("b", new HashSet<string>());
      t.AddDependency("y", "b");
      t.ReplaceDependents("a", new HashSet<string>() { "c" });
      t.AddDependency("w", "d");
      t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
      t.ReplaceDependees("d", new HashSet<string>() { "b" });
      Assert.AreEqual(4, t.Size);
    }

    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestComplexReplaceIndexer()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "b");
      t.AddDependency("a", "z");
      t.ReplaceDependents("b", new HashSet<string>());
      t.AddDependency("y", "b");
      t.ReplaceDependents("a", new HashSet<string>() { "c" });
      t.AddDependency("w", "d");
      t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
      t.ReplaceDependees("d", new HashSet<string>() { "b" });
      Assert.AreEqual(2, t["b"]);
    }

    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestComplexReplace2()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "b");
      t.AddDependency("a", "z");
      t.ReplaceDependents("b", new HashSet<string>());
      t.AddDependency("y", "b");
      t.ReplaceDependents("a", new HashSet<string>() { "c" });
      t.AddDependency("w", "d");
      t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
      t.ReplaceDependees("d", new HashSet<string>() { "b" });
      Assert.IsTrue(t.HasDependents("a"));
      Assert.IsFalse(t.HasDependees("a"));
      Assert.IsTrue(t.HasDependents("b"));
      Assert.IsTrue(t.HasDependees("b"));
    }

    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestComplexReplaceEnumerator()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "b");
      t.AddDependency("a", "z");
      t.ReplaceDependents("b", new HashSet<string>());
      t.AddDependency("y", "b");
      t.ReplaceDependents("a", new HashSet<string>() { "c" });
      t.AddDependency("w", "d");
      t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
      t.ReplaceDependees("d", new HashSet<string>() { "b" });

      IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependees("b").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      String s1 = e.Current;
      Assert.IsTrue(e.MoveNext());
      String s2 = e.Current;
      Assert.IsFalse(e.MoveNext());
      Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

      e = t.GetDependees("c").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("a", e.Current);
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependees("d").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("b", e.Current);
      Assert.IsFalse(e.MoveNext());
    }

    /// <summary>
    ///Non-empty graph contains something
    ///</summary>
    [TestMethod()]
    public void TestComplexReplaceEnumerator2()
    {
      DependencyGraph t = new DependencyGraph();
      t.AddDependency("x", "b");
      t.AddDependency("a", "z");
      t.ReplaceDependents("b", new HashSet<string>());
      t.AddDependency("y", "b");
      t.ReplaceDependents("a", new HashSet<string>() { "c" });
      t.AddDependency("w", "d");
      t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
      t.ReplaceDependees("d", new HashSet<string>() { "b" });

      IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      String s1 = e.Current;
      Assert.IsTrue(e.MoveNext());
      String s2 = e.Current;
      Assert.IsFalse(e.MoveNext());
      Assert.IsTrue(((s1 == "b") && (s2 == "c")) || ((s1 == "c") && (s2 == "b")));

      e = t.GetDependents("b").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("d", e.Current);
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependents("c").GetEnumerator();
      Assert.IsTrue(e.MoveNext());
      Assert.AreEqual("b", e.Current);
      Assert.IsFalse(e.MoveNext());

      e = t.GetDependents("d").GetEnumerator();
      Assert.IsFalse(e.MoveNext());
    }


    // ************************** STRESS TESTS REPEATED MULTIPLE TIMES ******************************** //
    /// <summary>
    ///Using lots of data
    ///</summary>
    [TestMethod()]
    public void StressTest1()
    {
      // Dependency graph
      DependencyGraph t = new DependencyGraph();

      // A bunch of strings to use
      const int SIZE = 200;
      string[] letters = new string[SIZE];
      for (int i = 0; i < SIZE; i++)
      {
        letters[i] = ("" + (char)('a' + i));
      }

      // The correct answers
      HashSet<string>[] dents = new HashSet<string>[SIZE];
      HashSet<string>[] dees = new HashSet<string>[SIZE];
      for (int i = 0; i < SIZE; i++)
      {
        dents[i] = new HashSet<string>();
        dees[i] = new HashSet<string>();
      }

      // Add a bunch of dependencies
      for (int i = 0; i < SIZE; i++)
      {
        for (int j = i + 1; j < SIZE; j++)
        {
          t.AddDependency(letters[i], letters[j]);
          dents[i].Add(letters[j]);
          dees[j].Add(letters[i]);
        }
      }

      // Remove a bunch of dependencies
      for (int i = 0; i < SIZE; i++)
      {
        for (int j = i + 4; j < SIZE; j += 4)
        {
          t.RemoveDependency(letters[i], letters[j]);
          dents[i].Remove(letters[j]);
          dees[j].Remove(letters[i]);
        }
      }

      // Add some back
      for (int i = 0; i < SIZE; i++)
      {
        for (int j = i + 1; j < SIZE; j += 2)
        {
          t.AddDependency(letters[i], letters[j]);
          dents[i].Add(letters[j]);
          dees[j].Add(letters[i]);
        }
      }

      // Remove some more
      for (int i = 0; i < SIZE; i += 2)
      {
        for (int j = i + 3; j < SIZE; j += 3)
        {
          t.RemoveDependency(letters[i], letters[j]);
          dents[i].Remove(letters[j]);
          dees[j].Remove(letters[i]);
        }
      }

      // Make sure everything is right
      for (int i = 0; i < SIZE; i++)
      {
        Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
        Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
      }
    }

    [TestMethod()]
    public void StressTest2()
    {
      StressTest1();
    }
    [TestMethod()]
    public void StressTest3()
    {
      StressTest1();
    }


    // ********************************** ANOTHER STESS TEST, REPEATED ******************** //
    /// <summary>
    ///Using lots of data with replacement
    ///</summary>
    [TestMethod()]
    public void StressTest8()
    {
      // Dependency graph
      DependencyGraph t = new DependencyGraph();

      // A bunch of strings to use
      const int SIZE = 400;
      string[] letters = new string[SIZE];
      for (int i = 0; i < SIZE; i++)
      {
        letters[i] = ("" + (char)('a' + i));
      }

      // The correct answers
      HashSet<string>[] dents = new HashSet<string>[SIZE];
      HashSet<string>[] dees = new HashSet<string>[SIZE];
      for (int i = 0; i < SIZE; i++)
      {
        dents[i] = new HashSet<string>();
        dees[i] = new HashSet<string>();
      }

      // Add a bunch of dependencies
      for (int i = 0; i < SIZE; i++)
      {
        for (int j = i + 1; j < SIZE; j++)
        {
          t.AddDependency(letters[i], letters[j]);
          dents[i].Add(letters[j]);
          dees[j].Add(letters[i]);
        }
      }

      // Remove a bunch of dependencies
      for (int i = 0; i < SIZE; i++)
      {
        for (int j = i + 2; j < SIZE; j += 3)
        {
          t.RemoveDependency(letters[i], letters[j]);
          dents[i].Remove(letters[j]);
          dees[j].Remove(letters[i]);
        }
      }

      // Replace a bunch of dependents
      for (int i = 0; i < SIZE; i += 2)
      {
        HashSet<string> newDents = new HashSet<String>();
        for (int j = 0; j < SIZE; j += 5)
        {
          newDents.Add(letters[j]);
        }
        t.ReplaceDependents(letters[i], newDents);

        foreach (string s in dents[i])
        {
          dees[s[0] - 'a'].Remove(letters[i]);
        }

        foreach (string s in newDents)
        {
          dees[s[0] - 'a'].Add(letters[i]);
        }

        dents[i] = newDents;
      }

      // Make sure everything is right
      for (int i = 0; i < SIZE; i++)
      {
        Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
        Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
      }
    }

    [TestMethod()]
    public void StressTest9()
    {
      StressTest8();
    }
    [TestMethod()]
    public void StressTest10()
    {
      StressTest8();
    }


    // ********************************** A THIRD STESS TEST, REPEATED ******************** //
    /// <summary>
    ///Using lots of data with replacement
    ///</summary>
    [TestMethod()]
    public void StressTest15()
    {
      // Dependency graph
      DependencyGraph t = new DependencyGraph();

      // A bunch of strings to use
      const int SIZE = 1000;
      string[] letters = new string[SIZE];
      for (int i = 0; i < SIZE; i++)
      {
        letters[i] = ("" + (char)('a' + i));
      }

      // The correct answers
      HashSet<string>[] dents = new HashSet<string>[SIZE];
      HashSet<string>[] dees = new HashSet<string>[SIZE];
      for (int i = 0; i < SIZE; i++)
      {
        dents[i] = new HashSet<string>();
        dees[i] = new HashSet<string>();
      }

      // Add a bunch of dependencies
      for (int i = 0; i < SIZE; i++)
      {
        for (int j = i + 1; j < SIZE; j++)
        {
          t.AddDependency(letters[i], letters[j]);
          dents[i].Add(letters[j]);
          dees[j].Add(letters[i]);
        }
      }

      for (int i = 0; i < SIZE; i++)
      {
        Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
        Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
      }

      // Remove a bunch of dependencies
      for (int i = 0; i < SIZE; i++)
      {
        for (int j = i + 2; j < SIZE; j += 3)
        {
          t.RemoveDependency(letters[i], letters[j]);
          dents[i].Remove(letters[j]);
          dees[j].Remove(letters[i]);
        }
      }

      for (int i = 0; i < SIZE; i++)
      {
        Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
        Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
      }

      // Replace a bunch of dependees
      for (int i = 0; i < SIZE; i += 2)
      {
        HashSet<string> newDees = new HashSet<String>();
        for (int j = 0; j < SIZE; j += 9)
        {
          newDees.Add(letters[j]);
        }
        t.ReplaceDependees(letters[i], newDees);

        foreach (string s in dees[i])
        {
          dents[s[0] - 'a'].Remove(letters[i]);
        }

        foreach (string s in newDees)
        {
          dents[s[0] - 'a'].Add(letters[i]);
        }

        dees[i] = newDees;
      }

      // Make sure everything is right
      for (int i = 0; i < SIZE; i++)
      {
        Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
        Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
      }
    }

    [TestMethod()]
    public void StressTest16()
    {
      StressTest15();
    }
    [TestMethod()]
    public void StressTest17()
    {
      StressTest15();
    }

    //// This test was not used for grading
    //[TestMethod]
    //public void TestEmptyReplaceDependees()
    //{
    //  DependencyGraph dg = new DependencyGraph();

    //  dg.ReplaceDependees("b", new HashSet<string> { "a" });

    //  Assert.AreEqual(1, dg.Size);
    //  Assert.IsTrue(new HashSet<string> { "b" }.SetEquals(dg.GetDependents("a")));
    //}

  }
}
