// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// TestCases.cs
// For testing the custom Double ended Queue implementation.
// ------------------------------------------------------------------------------------------------

using System.Collections;

namespace CustomDeQueue;

#region Class Test --------------------------------------------------------------------------------
/// <summary>Tests the custom Double ended Queue implementation over various edge cases.</summary>
class Test {
   #region Methods --------------------------------------------------
   /// <summary>Runs all unit test cases sequentially.</summary>
   public static void Run () {
      // Exception tests 
      Test1 (); Test2 ();
      
      // When resize doesnt happen
      Test3 (4); Test4 (4); 
      Test5 (4); Test6 (4);

      // When resize occurs
      Test7 (12); Test8 (12);
      Test9 (12); Test10 (12);
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
      PrintStatus ("( mHead < mTail ) Push : Back, Ret : Front", res.SequenceEqual ([0, 1, 2, 3]));
   }

   // 4. Order when mHead > mTail ( Wrapped Condition )
   static void Test4 (int N) {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < N; i++) { myDeQueue.PushFront (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopBack ());
      PrintStatus ("( mHead > mTail ) Push : Front, Ret : Back", res.SequenceEqual (Enumerable.Range (0, N).ToList ()));
   }

   // 5. Order when mHead < mTail ( Linear Condition )
   static void Test5 (int N) {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < N; i++) { myDeQueue.PushBack (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopBack ());
      PrintStatus ("( mHead < mTail ) Push : Back, Ret : Back", res.SequenceEqual (Enumerable.Range (0, N).Reverse().ToList ()));
   }

   // 6. Order when mHead > mTail ( Wrapped Condition )
   static void Test6 (int N) {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < N; i++) { myDeQueue.PushFront (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopFront ());
      PrintStatus ("( mHead > mTail ) Push : Front, Ret : Front", res.SequenceEqual (Enumerable.Range (0, N).Reverse ().ToList ()));
   }

   // 7. Order when mHead < mTail & Resized ( Linear Condition )
   static void Test7 (int N) {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < N; i++) { myDeQueue.PushBack (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopFront ());
      PrintStatus ("( mHead < mTail ) Push : Back, Ret : Front", res.SequenceEqual (Enumerable.Range (0, N).ToList ()));
   }

   // 8. Order when mHead > mTail & Resized ( Wrapped Condition )
   static void Test8 (int N) {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < N; i++) { myDeQueue.PushFront (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopBack ());
      PrintStatus ("( mHead > mTail ) & Resized Push : Front, Ret : Back", res.SequenceEqual (Enumerable.Range (0, N).ToList ()));
   }

   // 9. Order when mHead < mTail & Resized ( Linear Condition )
   static void Test9 (int N) {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < N; i++) { myDeQueue.PushBack (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopBack ());
      PrintStatus ("( mHead < mTail ) & Resized Push : Back, Ret : Back", res.SequenceEqual (Enumerable.Range (0, N).Reverse ().ToList ()));
   }

   // 10. Order when mHead > mTail & Resized ( Wrapped Condition )
   static void Test10 (int N) {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < N; i++) { myDeQueue.PushFront (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopFront ());
      PrintStatus ("( mHead > mTail ) & Resized Push : Front, Ret : Front", res.SequenceEqual (Enumerable.Range (0, N).Reverse ().ToList ()));
   }

   // 11. Mixed Up operation without Resize


   // 12. Mixed Up operations with Resize

   // 13. Consistancy of Peek for Resized

   // 14. Consistency of Peek for UnResized

   // 15. Consistancy for Count..
   #endregion
}
#endregion