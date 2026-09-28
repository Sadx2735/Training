// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// MTests.cs
// Tests the custom priority queue implementation.
// ------------------------------------------------------------------------------------------------

namespace CustomPriorityQueue;

[TestClass]
public class MyPQTests {

   [TestMethod]
   public void Test1 () {
      var myPQ = new MyPriorityQueue<int> ();
      Assert.ThrowsExactly<InvalidOperationException> (() => myPQ.Dequeue ());
   }

   [TestMethod]
   public void Test2 () {
      var data = new List<int> { 30, 10, 50, 20, 5, 40, 1 };
      var pq = new MyPriorityQueue<int> ();
      foreach (var item in data) pq.Enqueue (item);
      var result = new List<int> ();
      while (!pq.IsEmpty) result.Add (pq.Dequeue ());
      data.Sort ();
      Assert.AreSequenceEqual (data, result);
   }

   [TestMethod]
   public void Test3 () {
      var data = new List<int> { 5, 1, 5, 2, 1 };
      var pq = new MyPriorityQueue<int> ();
      foreach (var item in data) pq.Enqueue (item);
      var result = new List<int> ();
      while (!pq.IsEmpty) result.Add (pq.Dequeue ());
      data.Sort ();
      Assert.AreSequenceEqual (data, result);
   }

   [TestMethod]
   public void Test4 () {
      var res1 = new List<int> ();
      var res2 = new MyPriorityQueue<int> ();
      var random = new Random ();
      for (int i = 0; i < 100; i++) {
         var item = random.Next ();
         res1.Add (item);
         res2.Enqueue (item);
      }
      var result = new List<int> ();
      while (!res2.IsEmpty) { result.Add (res2.Dequeue ()); }
      Assert.AreSequenceEqual (res1.Order (), result);
   }
}