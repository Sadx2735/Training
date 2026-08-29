namespace CustomDeQueue;

[TestClass]
public class DeQueueTest {
   [TestMethod]
   /// <summary>Checks if all the exceptions work correctly!</summary>
   public void Test1 () {
      var myDeQueue = new MyDeQueue<int> ();
      Assert.ThrowsExactly<InvalidOperationException> (() => myDeQueue.PopFront ());
      Assert.ThrowsExactly<InvalidOperationException> (() => myDeQueue.PopBack ());
      Assert.ThrowsExactly<InvalidOperationException> (() => myDeQueue.PeekFront ());
      Assert.ThrowsExactly<InvalidOperationException> (() => myDeQueue.PeekBack ());
   }

   [TestMethod]
   /// <summary>Checks when mHead<mTail ( Linear Condition )</summary>
   public void Test2 () {
      var myQueue = new MyDeQueue<int> ();
      for (int i = 0; i < 4; i++) myQueue.PushBack (i);
      var res = new List<int> ();
      while (myQueue.Count > 0) res.Add (myQueue.PopFront ());
      CollectionAssert.AreEqual (Enumerable.Range (0, 4).ToList (), res);
   }

   [TestMethod]
   /// <summary>Checks when mHead < mTail ( Linear Condition )</summary>
   public void Test4 () {
      var myQueue = new MyDeQueue<int> ();
      for (int i = 0; i < 4; i++) myQueue.PushBack (i);
      var res = new List<int> ();
      while (myQueue.Count > 0) res.Add (myQueue.PopBack ());
      CollectionAssert.AreEqual (Enumerable.Range (0, 4).Reverse ().ToList (), res);
   }

   [TestMethod]
   /// <summary>Checks when mHead > mTail ( Wrapped condition )</summary>
   public void Test3 () {
      var myQueue = new MyDeQueue<int> ();
      for (int i = 0; i < 4; i++) myQueue.PushFront (i);
      var res = new List<int> ();
      while (myQueue.Count > 0) res.Add (myQueue.PopBack ());
      CollectionAssert.AreEqual (Enumerable.Range (0, 4).ToList (), res);
   }

   [TestMethod]
   /// <summary>Checks when mHead > mTail ( Wrapped Condition )</summary>
   public void Test5 () {
      var myQueue = new MyDeQueue<int> ();
      for (int i = 0; i < 4; i++) myQueue.PushFront (i);
      var res = new List<int> ();
      while (myQueue.Count > 0) res.Add (myQueue.PopFront ());
      CollectionAssert.AreEqual (Enumerable.Range (0, 4).Reverse ().ToList (), res);
   }

   [TestMethod]
   /// <summary>Checks when mHead < mTail & Resized </summary>
   public void Test6 () {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < 12; i++) { myDeQueue.PushFront (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopBack ());
      CollectionAssert.AreEqual (Enumerable.Range (0, 12).ToList (),res);
   }

   [TestMethod]
   /// <summary>Checks when mHead < mTail & Resized </summary>
   public void Test7 () {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < 12; i++) { myDeQueue.PushFront (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopFront ());
      CollectionAssert.AreEqual (Enumerable.Range (0, 12).Reverse().ToList (), res);
   }

   [TestMethod]
   /// <summary>Checks when mHead < mTail & Resized </summary>
   public void Test8() {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < 12; i++) { myDeQueue.PushBack (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopFront ());
      CollectionAssert.AreEqual (Enumerable.Range (0, 12).ToList (), res);
   }

   [TestMethod]
   /// <summary>Checks when mHead < mTail & Resized </summary>
   public void Test9 () {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < 12; i++) { myDeQueue.PushBack (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopBack ());
      CollectionAssert.AreEqual (Enumerable.Range (0, 12).Reverse ().ToList (), res);
   }

   [TestMethod]
   /// <summary>Checks when mHead < mTail & Resized </summary>
   public void Test10 () {
      var myDeQueue = new MyDeQueue<int> ();
      for (int i = 0; i < 12; i++) { myDeQueue.PushBack (i); }
      var res = new List<int> ();
      while (myDeQueue.Count > 0) res.Add (myDeQueue.PopBack ());
      CollectionAssert.AreEqual (Enumerable.Range (0, 12).Reverse ().ToList (), res);
   }

   [TestMethod]
   /// <summary>Checks transition of Count without resize</summary>
   public void Test11 () {
      bool iPassed = true;
      var myDeQueue = new MyDeQueue<int> ();
      myDeQueue.PushFront (1); myDeQueue.PushFront (2);
      iPassed &= (myDeQueue.Count == 2);
      myDeQueue.PushBack (3); myDeQueue.PushBack (4);
      iPassed &= (myDeQueue.Count == 4);
      Assert.AreEqual (true,iPassed);
   }

   [TestMethod]
   /// <summary>Checks transition of Count with resize</summary>
   public void Test12 () {
      bool iPassed = true;
      var myDeQueue = new MyDeQueue<int> ();
      myDeQueue.PushFront (1); myDeQueue.PushFront (2);
      iPassed &= (myDeQueue.Count == 2);
      myDeQueue.PushBack (3); myDeQueue.PushBack (4);
      iPassed &= (myDeQueue.Count == 4);
      myDeQueue.PushFront (5); myDeQueue.PushBack (6);
      iPassed &= (myDeQueue.Count == 6);
      Assert.AreEqual (true, iPassed);
   }

   [TestMethod]
   /// <summary>Mixed-up state transitions</summary>
   public void Test13 () {
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
      Assert.AreEqual (true, iPassed);
   }
}