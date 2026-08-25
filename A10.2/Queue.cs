// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// DeQueue.cs
// Custom double-ended queue implementation using a circular buffer.
// ------------------------------------------------------------------------------------------------

namespace CustomDeQueue;

#region Class DeQueue -----------------------------------------------------------------------------
/// <summary>Implements a custom double ended Queue</summary>
public class MyDeQueue<T> {
   #region Properties -----------------------------------------------
   /// <summary>the total number of elements contained in the deque.</summary>
   public int Count { get; private set; }
   #endregion

   #region Methods --------------------------------------------------
   // Determines whether the deque contains no elements.
   public bool IsEmpty () => Count == 0;

   /// <summary>Returns the element at the back of the deque.</summary>
   public T PeekBack () {
      if (Count == 0) throw new InvalidOperationException ("Deque is empty!");
      return mBuffer[WrapIndex (mTail - 1)];
   }

   /// <summary>Returns the element at the front of the deque.</summary>
   public T PeekFront () {
      if (Count == 0) throw new InvalidOperationException ("Deque is empty!");
      return mBuffer[mHead];
   }

   /// <summary>Removes and returns the element at the back of the deque.</summary>
   /// <returns>The element removed from the back.</returns>
   /// <exception cref="InvalidOperationException">Thrown when the dequeue is empty.</exception>
   public T PopBack () {
      if (Count == 0) throw new InvalidOperationException ("Deque is empty!");
      mTail = WrapIndex (mTail - 1);
      T value = mBuffer[mTail];
      mBuffer[mTail] = default!;
      Count--;
      return value;
   }

   /// <summary>Removes and returns the element at the front of the deque.</summary>
   /// <returns>The element removed from the front.</returns>
   /// <exception cref="InvalidOperationException">Thrown when the dequeue is empty.</exception>
   public T PopFront () {
      if (Count == 0) throw new InvalidOperationException ("Deque is empty!");
      T value = mBuffer[mHead];
      mBuffer[mHead] = default!;
      mHead = WrapIndex (mHead + 1);
      Count--;
      return value;
   }

   /// <summary>Inserts an element at the back of the deque.</summary>
   /// <param name="element">The item to push to the back.</param>
   public void PushBack (T element) {
      if (Count == mBuffer.Length) Resize ();
      mBuffer[mTail] = element;
      mTail = WrapIndex (mTail + 1);
      Count++;
   }

   /// <summary>Inserts an element at the front of the deque.</summary>
   /// <param name="element">The item to push to the front.</param>
   public void PushFront (T element) {
      if (Count == mBuffer.Length) Resize ();
      mHead = WrapIndex (mHead - 1);
      mBuffer[mHead] = element;
      Count++;
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
   int WrapIndex (int ptr) => (ptr + mBuffer.Length) % mBuffer.Length;
   #endregion

   #region Fields ---------------------------------------------------
   T[] mBuffer = new T[4];
   int mHead, mTail;
   #endregion
}
#endregion