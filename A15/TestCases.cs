// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// TestCases.cs
// Unit tests for MyPriorityQueue.
// ------------------------------------------------------------------------------------------------

namespace CustomPriorityQueue;

#region Class Test --------------------------------------------------------------------------------
/// <summary>Tests the MyPriorityQueue implementation across essential edge cases.</summary>
class Test {
   #region Methods --------------------------------------------------
   /// <summary>Runs all unit test cases sequentially.</summary>
   public static void Run () {
      Test1 ();
      Test2 ();
      Test3 ();
   }
   #endregion

   #region Implementation -------------------------------------------
   static void PrintStatus (string name, bool passed) {
      Console.ForegroundColor = passed ? ConsoleColor.Green : ConsoleColor.Red;
      Console.WriteLine ($"[{(passed ? "PASS" : "FAIL")}] {name}");
      Console.ResetColor ();
   }

   // 1. Exception thrown on empty dequeue
   static void Test1 () {
      var pq = new MyPriorityQueue<int> ();
      bool passed = false;
      try {
         pq.Dequeue ();
      } catch (InvalidOperationException) {
         passed = true;
      }
      PrintStatus ("Dequeue from empty priority queue throws exception", passed);
   }

   // 2. Min-heap sorting.
   static void Test2 () {
      var data = new List<int> { 30, 10, 50, 20, 5, 40, 1 };
      var pq = new MyPriorityQueue<int> ();
      foreach (var item in data) pq.Enqueue (item);
      var result = new List<int> ();
      while (!pq.IsEmpty) result.Add (pq.Dequeue ());
      data.Sort ();
      PrintStatus ("Elements dequeued in ascending order", data.SequenceEqual (result));
   }

   // 3. Handles duplicate priority values correctly
   static void Test3 () {
      var data = new List<int> { 5, 1, 5, 2, 1 };
      var pq = new MyPriorityQueue<int> ();
      foreach (var item in data) pq.Enqueue (item);
      var result = new List<int> ();
      while (!pq.IsEmpty) result.Add (pq.Dequeue ());
      data.Sort ();
      PrintStatus ("Handles duplicate elements correctly", data.SequenceEqual (result));
   }

   // 4. Random elements order.
   static void Test4 () {
      var res1 = new List<int> ();
      var res2 = new MyPriorityQueue<int> ();
      var random = new Random ();
      for (int i = 0; i < 100; i++) {
         var item = random.Next ();
         res1.Add (item); res2.Enqueue (item);
      }
      var result = new List<int> ();
      while (!res2.IsEmpty) { result.Add (res2.Dequeue ()); }
      PrintStatus ("Random elements ordering", res1.SequenceEqual (result));
   }
   #endregion
}
#endregion