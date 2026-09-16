// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// TestCases.cs
// For testing the custom CustomPriorityQueue implementation.
// ------------------------------------------------------------------------------------------------

using CustomPriorityQueue;

namespace CustomQueue;

#region Class Test --------------------------------------------------------------------------------
/// <summary>Tests the Custom PriorityQueue implementation over various edge cases.</summary>
class Test {
   #region Methods --------------------------------------------------
   /// <summary>Runs all unit test cases sequentially.</summary>
   public static void Run () {
      Test1 ();
   }
   #endregion

   #region Implementation -------------------------------------------
   // Formats and prints based on the result
   static void PrintStatus (string name, bool passed) {
      if (passed) {
         Console.ForegroundColor = ConsoleColor.Green;
         Console.WriteLine ($"[PASS] {name}");
      } else {
         Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine ($"[FAIL] {name}");
      }
      Console.ResetColor ();
   }

   // 1. Checks for Dequeue on empty Priority Queue throwing exception
   static void Test1 () {
      var priorityQueue = new MyPriorityQueue<int> ();
      bool iPassed = false;
      try {
         priorityQueue.Dequeue ();
      } catch (InvalidOperationException) {
         iPassed = true;
      }
      PrintStatus ("Dequeue from empty priority queue", iPassed);
   }
   #endregion
}
#endregion