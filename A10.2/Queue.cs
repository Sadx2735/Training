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
   #region Constructors ---------------------------------------------
   /// <summary>Initializes the buffer array.</summary>
   public MyDeQueue () => mBuffer = new T[mCapacity];
   #endregion

   #region Properties -----------------------------------------------
   /// <summary>Gets the current capacity of the dequeue buffer.</summary>
   public int Capacity => mCapacity;

   /// <summary>Gets the total number of elements contained in the deque.</summary>
   public int Count => (mTail - mHead + mCapacity) % mCapacity;
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Inserts an element at the front of the deque.</summary>
   /// <param name="element">The item to push to the front.</param>
   public void PushFront (T element) {
      mHead = WrapIndex (mHead - 1);
      mBuffer[mHead] = element;
      if (mHead == mTail) Resize (mCapacity * 2);
   }

   /// <summary>Inserts an element at the back of the deque.</summary>
   /// <param name="element">The item to push to the back.</param>
   public void PushBack (T element) {
      mBuffer[mTail] = element;
      mTail = WrapIndex (mTail + 1);
      if (mTail == mHead) Resize (mCapacity * 2);
   }

   /// <summary>Removes and returns the element at the front of the deque.</summary>
   /// <returns>The element removed from the front.</returns>
   /// <exception cref="InvalidOperationException">Thrown when the dequeue is empty.</exception>
   public T PopFront () {
      if (Count == 0) throw new InvalidOperationException ("Deque is empty!");
      T value = mBuffer[mHead];
      mHead = WrapIndex (mHead + 1);
      if (Count < mCapacity / 4 && mCapacity > MINSIZE) Resize (mCapacity / 2);
      return value;
   }

   /// <summary>Removes and returns the element at the back of the deque.</summary>
   /// <returns>The element removed from the back.</returns>
   /// <exception cref="InvalidOperationException">Thrown when the dequeue is empty.</exception>
   public T PopBack () {
      if (Count == 0) throw new InvalidOperationException ("Deque is empty!");
      mTail = WrapIndex (mTail - 1);
      T value = mBuffer[mTail];
      if (Count < mCapacity / 4 && mCapacity > MINSIZE) Resize (mCapacity / 2);
      return value;
   }

   /// <summary>Returns the element at the front of the deque without removing it.</summary>
   public T PeekLeft () {
      if (Count == 0) throw new InvalidOperationException ("Deque is empty!");
      return mBuffer[mHead];
   }

   /// <summary>Returns the element at the back of the deque without removing it.</summary>
   public T PeekRight () {
      if (Count == 0) throw new InvalidOperationException ("Deque is empty!");
      return mBuffer[WrapIndex (mTail - 1)];
   }
   #endregion

   #region Implementation -------------------------------------------
   // Determines whether the deque contains no elements.
   public bool IsEmpty () => Count == 0;

   // Resizes the buffer array and re-aligns elements starting from index 0.
   void Resize (int newCapacity) {
      int count = newCapacity > mCapacity ? mCapacity : Count;
      var newBuffer = new T[newCapacity];
      for (int i = 0; i < count; i++) newBuffer[i] = mBuffer[WrapIndex (mHead + i)];
      (mHead, mTail, mBuffer, mCapacity) = (0, count, newBuffer, newCapacity);
   }

   // Performs circular indexing.
   int WrapIndex (int ptr) => (ptr + mCapacity) % mCapacity;
   #endregion

   #region Fields ---------------------------------------------------
   T[] mBuffer;
   int mCapacity = 4;
   int mHead, mTail;
   const int MINSIZE = 4;
   #endregion
}
#endregion