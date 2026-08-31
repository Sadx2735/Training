// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// TestCases.cs
// For testing the custom double-ended queue implementation.
// ------------------------------------------------------------------------------------------------

namespace CustomDeQueue;

#region Class Test --------------------------------------------------------------------------------
/// <summary>Tests the custom double-ended queue implementation over various edge cases.</summary>
class Test {
   #region Methods --------------------------------------------------
   /// <summary>Runs all unit test cases sequentially.</summary>
   public static void Run () {
      // Exception tests 
      Test1 (); Test2 ();
      // When resize does not happen
      Test3 (4); Test4 (4);
      Test5 (4); Test6 (4);
      // When resize occurs
      Test7 (12); Test8 (12);
      Test9 (12); Test10 (12);
      // Check for Peek , Count
      Test11 (); Test12 ();
      Test13 (); Test14 ();
      // Mixed up States
      Test15 ();
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

   // 1. PopFront over empty Deque
   static void Test1 () {
      bool iPassed = true;
      var myDeQueue = new MyDeQueue<int> ();
      try {
         myDeQueue.PopFront ();
         iPassed = false;
      } catch (InvalidOperationException) {
         iPassed = true;
      }
      PrintStatus ("Empty Dequeue exception when PopFront", iPassed);
   }

   // 2. PopBack over empty Deque
   static void Test2 () {
      bool iPassed = true;
      var myDeQueue = new MyDeQueue<int> ();
      try {
         myDeQueue.PopBack ();
         iPassed = false;
      } catch (InvalidOperationException) {
         iPassed = true;
      }
      PrintStatus ("Empty Dequeue exception when PopBack", iPassed);
   }

   // 3. Order when mHead < mTail ( Linear Condition )
   static void Test3 (int N) {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < N; i++) { myDeQueue.PushBack (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopFront ());
      PrintStatus ("(mHead < mTail) Push: Back, Pop: Front",
         res.SequenceEqual ([0, 1, 2, 3]));
   }

   // 4. Order when mHead > mTail ( Wrapped Condition )
   static void Test4 (int N) {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < N; i++) { myDeQueue.PushFront (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopBack ());
      PrintStatus ("(mHead > mTail) Push: Front, Pop: Back",
         res.SequenceEqual (Enumerable.Range (0, N).ToList ()));
   }

   // 5. Order when mHead < mTail ( Linear Condition )
   static void Test5 (int N) {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < N; i++) { myDeQueue.PushBack (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopBack ());
      PrintStatus ("(mHead < mTail) Push: Back, Pop: Back",
         res.SequenceEqual (Enumerable.Range (0, N).Reverse ().ToList ()));
   }

   // 6. Order when mHead > mTail ( Wrapped Condition )
   static void Test6 (int N) {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < N; i++) { myDeQueue.PushFront (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopFront ());
      PrintStatus ("(mHead > mTail) Push: Front, Pop: Front",
         res.SequenceEqual (Enumerable.Range (0, N).Reverse ().ToList ()));
   }

   // 7. Order when mHead < mTail & Resized ( Linear Condition )
   static void Test7 (int N) {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < N; i++) { myDeQueue.PushBack (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopFront ());
      PrintStatus ("(mHead < mTail) Resized Push: Back, Pop: Front",
         res.SequenceEqual (Enumerable.Range (0, N).ToList ()));
   }

   // 8. Order when mHead > mTail & Resized ( Wrapped Condition )
   static void Test8 (int N) {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < N; i++) { myDeQueue.PushFront (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopBack ());
      PrintStatus ("(mHead > mTail) Resized Push: Front, Pop: Back",
         res.SequenceEqual (Enumerable.Range (0, N).ToList ()));
   }

   // 9. Order when mHead < mTail & Resized ( Linear Condition )
   static void Test9 (int N) {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < N; i++) { myDeQueue.PushBack (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopBack ());
      PrintStatus ("(mHead < mTail) Resized Push: Back, Pop: Back",
         res.SequenceEqual (Enumerable.Range (0, N).Reverse ().ToList ()));
   }

   // 10. Order when mHead > mTail & Resized ( Wrapped Condition )
   static void Test10 (int N) {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < N; i++) { myDeQueue.PushFront (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopFront ());
      PrintStatus ("(mHead > mTail) Resized Push: Front, Pop: Front",
         res.SequenceEqual (Enumerable.Range (0, N).Reverse ().ToList ()));
   }

   // 11. PeekFront on empty deque
   static void Test11 () {
      bool iPassed = true;
      var myDeQueue = new MyDeQueue<int> ();
      try {
         myDeQueue.PeekFront ();
         iPassed = false;
      } catch (InvalidOperationException) {
         iPassed = true;
      }
      PrintStatus ("PeekLeft on empty deque exception", iPassed);
   }

   // 12. PeekBack on empty deque
   static void Test12 () {
      bool iPassed = true;
      var myDeQueue = new MyDeQueue<int> ();
      try {
         myDeQueue.PeekBack ();
         iPassed = false;
      } catch (InvalidOperationException) {
         iPassed = true;
      }
      PrintStatus ("PeekRight on empty deque exception", iPassed);
   }

   // 13. Transition of Count without resize
   static void Test13 () {
      bool iPassed = true;
      var myDeQueue = new MyDeQueue<int> ();
      myDeQueue.PushFront (1); myDeQueue.PushFront (2);
      iPassed &= (myDeQueue.Count == 2);
      myDeQueue.PushBack (3); myDeQueue.PushBack (4);
      iPassed &= (myDeQueue.Count == 4);
      PrintStatus ("Count transition when not resized", iPassed);
   }

   // 14. Transition of Count with resize
   static void Test14 () {
      bool iPassed = true;
      var myDeQueue = new MyDeQueue<int> ();
      myDeQueue.PushFront (1); myDeQueue.PushFront (2);
      iPassed &= (myDeQueue.Count == 2);
      myDeQueue.PushBack (3); myDeQueue.PushBack (4);
      iPassed &= (myDeQueue.Count == 4);
      myDeQueue.PushFront (5); myDeQueue.PushBack (6);
      iPassed &= (myDeQueue.Count == 6);
      PrintStatus ("Count transition when resized", iPassed);
   }

   // 15. Mixed-up state transitions
   static void Test15 () {
      bool iPassed = true;
      var myDeQueue = new MyDeQueue<int> ();
      myDeQueue.PushFront (1); myDeQueue.PushBack (2);
      iPassed &= (myDeQueue.Count == 2);
      iPassed &= (myDeQueue.PeekBack () == 2);
      iPassed &= (myDeQueue.PeekFront () == 1);
      myDeQueue.PushBack (3); myDeQueue.PopFront ();
      iPassed &= (myDeQueue.Count == 2);
      iPassed &= (myDeQueue.PeekBack () == 3);
      iPassed &= (myDeQueue.PeekFront () == 2);
      myDeQueue.PopBack (); myDeQueue.PushBack (5);
      iPassed &= (myDeQueue.Count == 2);
      iPassed &= (myDeQueue.PeekBack () == 5);
      iPassed &= (myDeQueue.PeekFront () == 2);
      PrintStatus ("Mixed-up state transitions", iPassed);
   }
   #endregion
}
#endregion