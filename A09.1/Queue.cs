// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Queue.cs
// Custom queue implementation with circular buffer.
// ------------------------------------------------------------------------------------------------

namespace CustomQueue;

#region Class MyQueue -----------------------------------------------------------------------------
/// <summary>Implements a custom queue that adds and removes elements in FIFO order.</summary>
public class MyQueue<T> {
   #region Properties -----------------------------------------------
   /// <summary>Gets the number of elements in the queue.</summary>
   public int Count { get; private set; }
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Returns and removes the top element in the queue.</summary>
   /// <returns>The top element (first inserted value).</returns>
   /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
   public T Dequeue () {
      if (Count == 0) throw new InvalidOperationException ("Queue is empty!");
      T item = mBuffer[mHead];
      mBuffer[mHead] = default;
      mHead = WrapIndex (mHead + 1);
      Count--;
      return item;
   }

   /// <summary>Adds an element to the queue.</summary>
   /// <param name="data">The element to add.</param>
   public void Enqueue (T data) {
      if (Count == mBuffer.Length) Resize ();
      mBuffer[mTail] = data;
      mTail = WrapIndex (mTail + 1);
      Count++;
   }

   /// <summary>Returns the top element in the queue.</summary>
   /// <returns>The top element (first inserted value).</returns>
   /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
   public T Peek () {
      if (Count == 0) throw new InvalidOperationException ("Queue is empty!");
      return mBuffer[mHead];
   }
   #endregion

   #region Implementation -------------------------------------------
   // Resizes the buffer array and re-aligns elements starting from index 0.
   void Resize () {
      var newBuffer = new T[mBuffer.Length * 2];
      for (int i = 0; i < Count; i++) newBuffer[i] = mBuffer[WrapIndex (mHead + i)];
      (mHead, mTail, mBuffer) = (0, Count, newBuffer);
   }

   // Performs circular indexing.
   int WrapIndex (int ptr) => ptr % mBuffer.Length;
   #endregion

   #region Fields ---------------------------------------------------
   T[] mBuffer = new T[4];
   int mHead, mTail;
   #endregion
}
#endregion