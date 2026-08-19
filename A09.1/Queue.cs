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
   /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
   public T Dequeue () {
      if (Count == 0) throw new InvalidOperationException ("Queue is empty!");
      T item = mBuffer[mHead];
      mHead = WrapIndex (mHead + 1);
      if (Count < mCapacity / 4 && mCapacity > MINSIZE) Resize (mCapacity / 2);
      return item;
   }

   /// <summary>Adds an element to the queue.</summary>
   /// <param name="data">The element to add.</param>
   public void Enqueue (T data) {
      mBuffer[mTail] = data;
      mTail = WrapIndex (mTail + 1);
      if (mTail == mHead) Resize (mCapacity * 2);
   }

   /// <summary>Returns and removes the top element in the queue.</summary>
   /// <returns>The top element (first inserted value).</returns>
   /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
   public T Peek () {
      if (Count == 0) throw new InvalidOperationException ("Queue is empty!");
      return mBuffer[mHead];
   }
   #endregion

   #region Implementation -------------------------------------------
   // Resizes the buffer array and re-aligns elements starting from index 0.
   void Resize (int newCapacity) {
      int count = newCapacity > mCapacity ? mCapacity : Count;
      var newBuffer = new T[newCapacity];
      for (int i = 0; i < count; i++) newBuffer[i] = mBuffer[WrapIndex (mHead + i)];
      (mHead, mTail, mBuffer, mCapacity) = (0, count, newBuffer, newCapacity);
   }

   // Performs circular indexing.
   int WrapIndex (int ptr) => ptr & (mCapacity - 1);
   #endregion

   #region Properties -----------------------------------------------
   /// <summary>Gets the current capacity of the queue buffer.</summary>
   public int Capacity => mCapacity;

   /// <summary>Gets the number of elements in the queue.</summary>
   public int Count => (mTail - mHead + mCapacity) % mCapacity;
   #endregion

   #region Fields ---------------------------------------------------
   const int MINSIZE = 4;
   int mHead, mTail;
   int mCapacity = MINSIZE;
   T[] mBuffer;
   #endregion
}
#endregion