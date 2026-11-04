// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// MsTest.cs
// Tests the custom double-ended queue implementation.
// ------------------------------------------------------------------------------------------------

using CustomDeQue;

[TestClass]
public class MyDeQueTests {
   // 1. All four operations throw on an empty deque
   [TestMethod]
   public void Test1 () {
      var myDeQue = new MyDeQue<int> ();
      Assert.ThrowsExactly<InvalidOperationException> (() => myDeQue.PopFront ());
      Assert.ThrowsExactly<InvalidOperationException> (() => myDeQue.PopBack ());
      Assert.ThrowsExactly<InvalidOperationException> (() => myDeQue.PeekFront ());
      Assert.ThrowsExactly<InvalidOperationException> (() => myDeQue.PeekBack ());
   }

   // 2. Push at the back, pop from the front (FIFO order)
   [TestMethod]
   public void Test2 () {
      var myDeQue = new MyDeQue<int> ();
      for (int i = 0; i < 4; i++) myDeQue.PushBack (i);
      var res = new List<int> ();
      while (myDeQue.Count > 0) res.Add (myDeQue.PopFront ());
      Assert.AreSequenceEqual (Enumerable.Range (0, 4), res);
   }

   // 3. Push at the front, pop from the back (FIFO order, wrapped buffer)
   [TestMethod]
   public void Test3 () {
      var myDeQue = new MyDeQue<int> ();
      for (int i = 0; i < 4; i++) myDeQue.PushFront (i);
      var res = new List<int> ();
      while (myDeQue.Count > 0) res.Add (myDeQue.PopBack ());
      Assert.AreSequenceEqual (Enumerable.Range (0, 4), res);
   }

   // 4. Push at the back, pop from the back (LIFO order)
   [TestMethod]
   public void Test4 () {
      var myDeQue = new MyDeQue<int> ();
      for (int i = 0; i < 4; i++) myDeQue.PushBack (i);
      var res = new List<int> ();
      while (myDeQue.Count > 0) res.Add (myDeQue.PopBack ());
      Assert.AreSequenceEqual (Enumerable.Range (0, 4).Reverse (), res);
   }

   // 5. Push at the front, pop from the front (LIFO order)
   [TestMethod]
   public void Test5 () {
      var myDeQue = new MyDeQue<int> ();
      for (int i = 0; i < 4; i++) myDeQue.PushFront (i);
      var res = new List<int> ();
      while (myDeQue.Count > 0) res.Add (myDeQue.PopFront ());
      Assert.AreSequenceEqual (Enumerable.Range (0, 4).Reverse (), res);
   }

   // 6. Pushing past the initial capacity resizes the buffer and keeps Count correct
   [TestMethod]
   public void Test6 () {
      var myDeQue = new MyDeQue<int> ();
      for (int i = 0; i < 12; i++) myDeQue.PushBack (i);
      Assert.AreEqual (12, myDeQue.Count);
   }

   // 7. Count updates correctly with mixed pushes, including after resize
   [TestMethod]
   public void Test7 () {
      bool iPassed = true;
      var myDeQue = new MyDeQue<int> ();
      myDeQue.PushFront (1); myDeQue.PushFront (2);
      iPassed &= (myDeQue.Count == 2);
      myDeQue.PushBack (3); myDeQue.PushBack (4);
      iPassed &= (myDeQue.Count == 4);
      myDeQue.PushFront (5); myDeQue.PushBack (6);
      iPassed &= (myDeQue.Count == 6);
      Assert.IsTrue (iPassed);
   }

   // 8. Peek, push and pop interleaved keep the front and back correct
   [TestMethod]
   public void Test8 () {
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
      Assert.IsTrue (iPassed);
   }
}