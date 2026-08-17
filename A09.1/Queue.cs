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
   public CustomQ () {
      mBuffer = new T[mCapacity];
   }
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Returns and removes the top element in the queue.</summary>
   /// <returns>The top element (first inserted value).</returns>
   public T Dequeue () {
      if (mPtr1 == mPtr2) throw new InvalidOperationException ("Queue is empty!");
      int current = mPtr1;
      mPtr1 = (mPtr1 + 1) & (mCapacity - 1);
      return mBuffer[current];
   }

   /// <summary>Adds an element to the queue.</summary>
   /// <param name="data">The element to add.</param>
   public void Enqueue (T data) {
      mBuffer[mPtr2] = data;
      mPtr2 = (mPtr2 + 1) & (mCapacity - 1);
      if (mPtr2 == mPtr1) Reallocate ();
   }
   #endregion

   #region Implementations ------------------------------------------
   /// <summary>Creates a larger array and copies the content of the old buffer into it.</summary>
   void Reallocate () {
      var tempArray = new T[mCapacity * 2];
      Array.Copy (mBuffer, mPtr1, tempArray, 0, mCapacity - mPtr1);
      Array.Copy (mBuffer, 0, tempArray, mCapacity - mPtr1, mPtr2);
      mPtr1 = 0;
      mPtr2 = mCapacity;
      mBuffer = tempArray;
      mCapacity *= 2;
   }
   #endregion

   #region Properties -----------------------------------------------
   /// <summary>Gets the current capacity of the queue buffer.</summary>
   public int Capacity => mCapacity;

   /// <summary>Gets the number of elements in the queue.</summary>
   public int Count => (mPtr2 - mPtr1 + mCapacity) % mCapacity;

   /// <summary>Gets a value indicating whether the queue is empty.</summary>
   public bool IsEmpty => mPtr1 == mPtr2;
   #endregion

   #region Fields ---------------------------------------------------
   int mPtr1, mPtr2;
   int mCapacity = 4;
   T[] mBuffer;
   #endregion
}
#endregion