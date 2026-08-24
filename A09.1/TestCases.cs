// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// TestCases.cs
// For testing the custom queue implementation.
// ------------------------------------------------------------------------------------------------

namespace CustomQueue;

#region Class Test --------------------------------------------------------------------------------
/// <summary>Tests the Custom Queue implementation over various edge cases.</summary>
class Test {
   #region Methods --------------------------------------------------
   /// <summary>Runs all unit test cases sequentially.</summary>
   public static void Run () {
      Test1 ();
      Test2 ();
      Test3 ();
      Test4 ();
      Test5 ();
      Test6 ();
      Test7 ();
      Test8 ();
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

   // 1. Checks for Dequeue on empty queue throwing exception
   static void Test1 () {
      var queue = new MyQueue<int> ();
      bool iPassed = false;
      try {
         queue.Dequeue ();
      } catch (InvalidOperationException) {
         iPassed = true;
      }
      PrintStatus ("Empty Dequeue exception", iPassed);
   }

   // 2. Checks for Peek on empty queue throwing exception
   static void Test2 () {
      var queue = new MyQueue<int> ();
      bool iPassed = false;
      try {
         queue.Peek ();
      } catch (InvalidOperationException) {
         iPassed = true;
      }
      PrintStatus ("Empty Peek exception", iPassed);
   }

   // 3. Checks for Count property transitions on Enqueue and Dequeue
   static void Test3 () {
      var queue = new MyQueue<int> ();
      bool iPassed = queue.Count == 0;
      queue.Enqueue (1);
      iPassed &= (queue.Count == 1);
      queue.Dequeue ();
      iPassed &= (queue.Count == 0);
      PrintStatus ("Count property transitions", iPassed);
   }

   // 4. Checks for ordering of elements when mTail < mHead (Wrapped state)
   static void Test4 () {
      var queue = new MyQueue<int> ();
      for (int i = 0; i < 4; i++) queue.Enqueue (i);
      queue.Dequeue ();
      queue.Dequeue ();
      queue.Enqueue (4);
      queue.Enqueue (5);

      var res = new List<int> ();
      while (queue.Count > 0) res.Add (queue.Dequeue ());
      PrintStatus ("Order (mTail < mHead)", res.SequenceEqual ([2, 3, 4, 5]));
   }

   // 5. Checks for ordering of elements when mTail > mHead (Linear state)
   static void Test5 () {
      var queue = new MyQueue<int> ();
      for (int i = 0; i < 4; i++) queue.Enqueue (i);
      var res = new List<int> ();
      while (queue.Count > 0) res.Add (queue.Dequeue ());
      PrintStatus ("Order (mTail > mHead)", res.SequenceEqual ([0, 1, 2, 3]));
   }

   // 6. Checks for ordering after resize when mTail < mHead (Wrapped state resize)
   static void Test6 () {
      var queue = new MyQueue<int> ();
      for (int i = 0; i < 4; i++) queue.Enqueue (i);
      queue.Dequeue ();
      queue.Enqueue (4);
      queue.Enqueue (5);
      queue.Enqueue (6);
      var res = new List<int> ();
      while (queue.Count > 0) res.Add (queue.Dequeue ());
      PrintStatus ("Resize order (mTail < mHead)", res.SequenceEqual ([1, 2, 3, 4, 5, 6]));
   }

   // 7. Checks for ordering after resize when mTail > mHead (Linear state resize)
   static void Test7 () {
      var queue = new MyQueue<int> ();
      for (int i = 0; i < 6; i++) queue.Enqueue (i);
      var res = new List<int> ();
      while (queue.Count > 0) res.Add (queue.Dequeue ());
      PrintStatus ("Resize order (mTail > mHead)", res.SequenceEqual ([0, 1, 2, 3, 4, 5]));
   }

   // 8. Checks for Peek return values across state changes
   static void Test8 () {
      var queue = new MyQueue<int> ();
      bool iPassed = true;
      for (int i = 0; i < 4; i++) queue.Enqueue (i);
      iPassed &= (queue.Peek () == 0);
      queue.Enqueue (4);
      queue.Enqueue (5);
      for (int i = 0; i < 5; i++) queue.Dequeue ();
      iPassed &= (queue.Peek () == 5);
      PrintStatus ("Peek value consistency", iPassed);
   }
   #endregion
}
#endregion