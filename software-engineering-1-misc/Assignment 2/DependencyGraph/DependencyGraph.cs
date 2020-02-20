// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)
//
// <author>
// Maxwell Hanson
// </author>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        // The dependency tuples are kept with a HashSet
        HashSet<Tuple<String, String>> dependencies;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            this.dependencies = new HashSet<Tuple<string, string>>();
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// 
        /// This is the number of edges, not the number of nodes in the graph.
        /// </summary>
        public int Size
        {
            get { return dependencies.Count; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                int count = 0;
                // iterate through every dependency tuple in the set
                foreach (Tuple<String, String> dependency in this.dependencies)
                {
                    // if a dependency is found where the dependent is 's', then increment count
                    if (dependency.Item2.Equals(s))
                    {
                        count++;
                    }
                }

                return count;
            }
        }
        
        /// <summary>
        /// Determine if a node in the dependency graph has dependents.
        /// </summary>
        /// <remarks>
        /// The definition of 'dependent' is described in the class summary.
        /// </remarks>
        /// <param name="s">
        /// string denoting the node to determine if it has dependents
        /// </param>
        /// <return>
        /// true if the node referenced by 's' has dependents, false otherwise
        /// </return>
        public bool HasDependents(string s)
        {
            // iterate through every dependency tuple in the set
            foreach (Tuple<String, String> dependency in this.dependencies)
            {
                // if a dependency is found where the dependent is 's', then return true
                if (dependency.Item1.Equals(s))
                {
                    return true;
                }
            }

            // loop finished, no matching dependencies found
            return false;
        }


        /// <summary>
        /// Determine if a node in the dependency graph has dependees.
        /// </summary>
        /// <remarks>
        /// The definition of 'dependee' is described in the class summary
        /// </remarks>
        /// <param name="s">
        /// string denoting the node to determine if it has dependees
        /// </param>
        /// <return>
        /// true if the node referenced by 's' has dependees, false otherwise
        /// </return>
        public bool HasDependees(string s)
        {
            // iterate through every dependency tuple in the set
            foreach (Tuple<String, String> dependency in this.dependencies)
            {
                // if a dependency is found where the dependee is 's', then return true
                if (dependency.Item2.Equals(s))
                {
                    return true;
                }
            }

            // loop finished, no matching dependencies found
            return false;
        }


        /// <summary>
        /// Enumerate all dependents(s) of a node.
        /// </summary>
        /// <remarks>A node 'x' has a dependent 'y' iff the element (x, y) is in the graph</remarks>
        /// <param name="s">String denoting the node to get the dependents of</param>
        /// <return>An enumeration of strings representing nodes which directly depend on 's'</return>
        public IEnumerable<string> GetDependents(string s)
        {
            HashSet < String > dependents = new HashSet<String>();

            // iterate through every dependency tuple in the set
            foreach (Tuple<String, String> dependency in this.dependencies)
            {
                // if a dependency is found where the dependee is 's', then add the dependent to the set
                if (dependency.Item1.Equals(s))
                {
                    dependents.Add(dependency.Item2);
                }
            }

            return dependents;
        }

        /// <summary>
        /// Enumerate all dependees(s) of a node.
        /// </summary>
        /// <remarks>A node 'y' has a dependee 'x' iff the element (x, y) is in the graph</remarks>
        /// <param name="s">String denoting the node to get the dependees of</param>
        /// <returns>An enumeration of strings representing the nodes in which the node 's' directly depends on</returns>
        public IEnumerable<string> GetDependees(string s)
        {
            HashSet<String> dependees = new HashSet<String>();

            // iterate through every dependency tuple in the set
            foreach (Tuple<String, String> dependency in this.dependencies)
            {
                // if a dependency is found where the dependent is 's', then add the dependee to the set
                if (dependency.Item2.Equals(s))
                {
                    dependees.Add(dependency.Item1);
                }
            }
            
            return dependees;
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// <para>The ordered pair (s, t) should be thought of as:</para>   
        ///   t depends on s
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S </param>
        /// <param name="t"> t cannot be evaluated until s is </param>
        public void AddDependency(string s, string t)
        {
            // turn the strings 's' and 't' into a tuple '(s, t)' and add to the set
            dependencies.Add(new Tuple<String, String>(s, t));
            // note that the size does not need to be incremented as the hashset keeps track of its own size
            // note that duplicates are not added since the HashSet does not add them
        }

        /// <summary>
        /// Remove the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"> The dependee node </param>
        /// <param name="t"> The dependant node </param>
        public void RemoveDependency(string s, string t)
        {
            // create a tuple from s and t then attempt remove
            dependencies.Remove(new Tuple<String, String>(s, t));
            // note that the size does not need to be incremented as the hashset keeps track of its own size
            // note that if the element is not present, the method exits cleanly since the HashSet will just return false
        }

        /// <summary>
        /// Remove all existing ordered pairs of the form (s,r).
        /// Then, for each t in newDependents, adds the ordered pair (s,t).
        ///     <para>
        ///         i.e., it removes all dependents for 's' and replaces them with the dependents
        ///         enumerated by 'newDependents'.
        ///     </para>
        /// </summary>
        /// <param name="s">String referencing node to replace dependents of</param>
        /// <param name="newDependents">Enumeration of dependents to replace dependents of 's'</param>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            // remove all dependencies (s, a)
            // to avoid removing elements while iterating, a set of dependents is compiled, then they are removed after iteration
            HashSet<String> dependents = new HashSet<string>();
            // compile the list
            foreach (Tuple<String, String> dependency in dependencies)
            {
                // if the dependee is 's', then add to set
                if (dependency.Item1.Equals(s))
                {
                    dependents.Add(dependency.Item2);
                }
            }
            // remove each matching dependency
            foreach (String dependent in dependents)
            {
                this.RemoveDependency(s, dependent);
            }

            // add the dependency (s, b) for all b in newDependents
            foreach (String newDependent in newDependents)
            {
                this.AddDependency(s, newDependent);
            }
        }
        
        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// 
        ///     <para>
        ///         i.e., removes all dependees for 's' and repalces them with the
        ///         dependees enumerated in 'newDependees'
        ///     </para>
        /// </summary>
        /// <param name="s">String referending node to remove all dependees of</param>
        /// <param name="newDependees">Enumeration of dependees to replace dependees of 's'</param>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            // remove all dependencies (a, s)
            // to avoid removing elements while iterating, a set of dependents is compiled, then they are removed after iteration
            HashSet<String> dependees = new HashSet<string>();
            // compile the list
            foreach (Tuple<String, String> dependency in dependencies)
            {
                // if the dependent is 's', then add to set
                if (dependency.Item2.Equals(s))
                {
                    dependees.Add(dependency.Item1);
                }
            }
            // remove each matching dependency
            foreach (String dependee in dependees)
            {
                this.RemoveDependency(dependee, s);
            }

            // add the dependency (b, s) for all b in newDependees
            foreach (String newDependee in newDependees)
            {
                this.AddDependency(newDependee, s);
            }
        }

    }

}

