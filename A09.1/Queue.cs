// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Queue.cs
// Custom queue implementation with circular buffer.
// ------------------------------------------------------------------------------------------------
namespace Cqueue;

#region Class CustomQ -----------------------------------------------------------------------------
/// <summary>Implements a custom queue that adds and removes elements in FIFO order.</summary>
public class CustomQ<T> {
   #region Constructors ---------------------------------------------
   /// <summary>Initializes the buffer array.</summary>
   public CustomQ () => mBuffer = new T[mCapacity];
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Returns and removes the top element in the queue.</summary>
   /// <returns>The top element (first inserted value).</returns>
   public T Dequeue () {
      if (IsEmpty) throw new InvalidOperationException ("Queue is empty!");
      T item = mBuffer[mHead];
      mHead = WrapIndex (mHead);
      if (Count < mCapacity / 2 && mCapacity > MINSIZE) Shrink ();
      return item;
   }

   /// <summary>Adds an element to the queue.</summary>
   /// <param name="data">The element to add.</param>
   public void Enqueue (T data) {
      mBuffer[mTail] = data;
      mTail = WrapIndex (mTail);
      if (mTail == mHead) Extend ();
   }

   /// <summary>Returns the top element in the queue without removing it.</summary>
   /// <returns>The top element (first inserted value).</returns>
   public T Peek () {
      if (IsEmpty) throw new InvalidOperationException ("Queue is empty!");
      return mBuffer[mHead];
   }
   #endregion

   #region Implementations ------------------------------------------
   /// <summary>Creates a larger array and copies the content of the old buffer into it.</summary>
   void Extend () {
      var tempArray = new T[mCapacity * 2];
      Array.Copy (mBuffer, mHead, tempArray, 0, mCapacity - mHead);
      Array.Copy (mBuffer, 0, tempArray, mCapacity - mHead, mTail);
      (mHead, mTail, mBuffer, mCapacity) = (0, mCapacity, tempArray, mCapacity * 2);
   }

   /// <summary>Reduces array capacity by half when element count drops significantly.</summary>
   void Shrink () {
      int count = Count;
      var tempArray = new T[mCapacity / 2];
      if (mHead < mTail) Array.Copy (mBuffer, mHead, tempArray, 0, count);
      else {
         Array.Copy (mBuffer, mHead, tempArray, 0, mCapacity - mHead);
         Array.Copy (mBuffer, 0, tempArray, mCapacity - mHead, mTail);
      }
      (mHead, mTail, mBuffer, mCapacity) = (0, count, tempArray, mCapacity / 2);
   }

   /// <summary>Performs circular indexing.</summary>
   /// <param name="ptr">The index of the pointer.</param>
   /// <returns>The wrapped value of the index.</returns>
   int WrapIndex (int ptr) => (ptr + 1) & (mCapacity - 1);
   #endregion

   #region Properties -----------------------------------------------
   /// <summary>Gets the current capacity of the queue buffer.</summary>
   public int Capacity => mCapacity;

   /// <summary>Gets the number of elements in the queue.</summary>
   public int Count => (mTail - mHead + mCapacity) % mCapacity;

   /// <summary>Gets a value indicating whether the queue is empty.</summary>
   public bool IsEmpty => mHead == mTail;
   #endregion

   #region Fields ---------------------------------------------------
   const int MINSIZE = 4;
   int mHead, mTail;
   int mCapacity = MINSIZE;
   T[] mBuffer;
   #endregion
}
#endregion