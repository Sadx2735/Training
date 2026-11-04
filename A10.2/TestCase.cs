// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// TestCase.cs
// For testing the custom double-ended queue implementation.
// ------------------------------------------------------------------------------------------------

namespace CustomDeQue;

#region Class Test --------------------------------------------------------------------------------
/// <summary>Tests the custom double-ended queue implementation over various edge cases.</summary>
class Test {
   #region Methods --------------------------------------------------
   /// <summary>Runs all unit test cases sequentially.</summary>
   public static void Run () {
      // Exception tests
      Test1 (); Test2 ();
      Test3 (); Test4 ();
      // Push / pop order
      Test5 (4); Test6 (4);
      Test7 (4); Test8 (4);
      // Resize
      Test9 (12);
      // Count and Peek
      Test10 (); Test11 ();
   }
   #endregion

   #region Implementation -------------------------------------------
   // Formats and prints based on the result
   static void PrintStatus (string name, bool passed) {
      Console.ForegroundColor = passed ? ConsoleColor.Green : ConsoleColor.Red;
      Console.WriteLine (passed ? $"[PASS] {name}" : $"[FAIL] {name}");
      Console.ResetColor ();
   }

   // 1. PopFront on an empty deque throws
   static void Test1 () {
      bool iPassed = true;
      var myDeQue = new MyDeQue<int> ();
      try {
         myDeQue.PopFront ();
         iPassed = false;
      } catch (InvalidOperationException) {
         iPassed = true;
      }
      PrintStatus ("Empty deque exception when PopFront", iPassed);
   }

   // 2. PopBack on an empty deque throws
   static void Test2 () {
      bool iPassed = true;
      var myDeQue = new MyDeQue<int> ();
      try {
         myDeQue.PopBack ();
         iPassed = false;
      } catch (InvalidOperationException) {
         iPassed = true;
      }
      PrintStatus ("Empty deque exception when PopBack", iPassed);
   }

   // 3. PeekFront on an empty deque throws
   static void Test3 () {
      bool iPassed = true;
      var myDeQue = new MyDeQue<int> ();
      try {
         myDeQue.PeekFront ();
         iPassed = false;
      } catch (InvalidOperationException) {
         iPassed = true;
      }
      PrintStatus ("Empty deque exception when PeekFront", iPassed);
   }

   // 4. PeekBack on an empty deque throws
   static void Test4 () {
      bool iPassed = true;
      var myDeQue = new MyDeQue<int> ();
      try {
         myDeQue.PeekBack ();
         iPassed = false;
      } catch (InvalidOperationException) {
         iPassed = true;
      }
      PrintStatus ("Empty deque exception when PeekBack", iPassed);
   }

   // 5. Push at the back, pop from the front (FIFO order, mHead < mTail)
   static void Test5 (int N) {
      var myDeQue = new MyDeQue<int> ();
      for (int i = 0; i < N; i++) myDeQue.PushBack (i);
      var res = new List<int> ();
      while (myDeQue.Count > 0) res.Add (myDeQue.PopFront ());
      PrintStatus ("(mHead < mTail) Push: Back, Pop: Front",
         res.SequenceEqual (Enumerable.Range (0, N)));
   }

   // 6. Push at the front, pop from the back (FIFO order, mHead > mTail wrapped)
   static void Test6 (int N) {
      var myDeQue = new MyDeQue<int> ();
      for (int i = 0; i < N; i++) myDeQue.PushFront (i);
      var res = new List<int> ();
      while (myDeQue.Count > 0) res.Add (myDeQue.PopBack ());
      PrintStatus ("(mHead > mTail) Push: Front, Pop: Back",
         res.SequenceEqual (Enumerable.Range (0, N)));
   }

   // 7. Push at the back, pop from the back (LIFO order, mHead < mTail)
   static void Test7 (int N) {
      var myDeQue = new MyDeQue<int> ();
      for (int i = 0; i < N; i++) myDeQue.PushBack (i);
      var res = new List<int> ();
      while (myDeQue.Count > 0) res.Add (myDeQue.PopBack ());
      PrintStatus ("(mHead < mTail) Push: Back, Pop: Back",
         res.SequenceEqual (Enumerable.Range (0, N).Reverse ()));
   }

   // 8. Push at the front, pop from the front (LIFO order, mHead > mTail wrapped)
   static void Test8 (int N) {
      var myDeQue = new MyDeQue<int> ();
      for (int i = 0; i < N; i++) myDeQue.PushFront (i);
      var res = new List<int> ();
      while (myDeQue.Count > 0) res.Add (myDeQue.PopFront ());
      PrintStatus ("(mHead > mTail) Push: Front, Pop: Front",
         res.SequenceEqual (Enumerable.Range (0, N).Reverse ()));
   }

   // 9. Pushing past the initial capacity resizes the buffer and keeps Count correct
   static void Test9 (int N) {
      var myDeQue = new MyDeQue<int> ();
      for (int i = 0; i < N; i++) myDeQue.PushBack (i);
      PrintStatus ("Resize keeps Count correct", myDeQue.Count == N);
   }

   // 10. Count updates correctly with mixed pushes, including after resize
   static void Test10 () {
      bool iPassed = true;
      var myDeQue = new MyDeQue<int> ();
      myDeQue.PushFront (1); myDeQue.PushFront (2);
      iPassed &= (myDeQue.Count == 2);
      myDeQue.PushBack (3); myDeQue.PushBack (4);
      iPassed &= (myDeQue.Count == 4);
      myDeQue.PushFront (5); myDeQue.PushBack (6);
      iPassed &= (myDeQue.Count == 6);
      PrintStatus ("Count transition with mixed pushes", iPassed);
   }

   // 11. Peek, push and pop interleaved keep the front and back correct
   static void Test11 () {
      bool iPassed = true;
      var myDeQue = new MyDeQue<int> ();
      myDeQue.PushFront (1); myDeQue.PushBack (2);
      iPassed &= (myDeQue.Count == 2);
      iPassed &= (myDeQue.PeekBack () == 2);
      iPassed &= (myDeQue.PeekFront () == 1);
      myDeQue.PushBack (3); myDeQue.PopFront ();
      iPassed &= (myDeQue.Count == 2);
      iPassed &= (myDeQue.PeekBack () == 3);
      iPassed &= (myDeQue.PeekFront () == 2);
      myDeQue.PopBack (); myDeQue.PushBack (5);
      iPassed &= (myDeQue.Count == 2);
      iPassed &= (myDeQue.PeekBack () == 5);
      iPassed &= (myDeQue.PeekFront () == 2);
      PrintStatus ("Mixed-up state transitions", iPassed);
   }
   #endregion
}
#endregion