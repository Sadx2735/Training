// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch- July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// DeQueue.cs
// Custom double-ended queue implementation using a circular dynamic array.
// ------------------------------------------------------------------------------------------------

namespace DQueue;

#region Class DeQueue -----------------------------------------------------------------------------
/// <summary>Represents a generic double-ended queue (deque) backed by a circular buffer.</summary>
/// <typeparam name="T">Specifies the element type of the deque.</typeparam>
public class DeQueue<T> {
   #region Constructors ---------------------------------------------
   /// <summary>Initializes a new instance of the DeQueue class with default initial capacity.</summary>
   public DeQueue () {
      mBuffer = new T[Capacity];
   }
   #endregion

   #region Properties -----------------------------------------------
   /// <summary>Gets the current total storage capacity of the underlying buffer.</summary>
   public int Capacity => mCapacity;

   /// <summary>Gets the total number of elements contained in the deque.</summary>
   public int Count => ((mTail - mHead - 1) + mCapacity) % mCapacity;
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Inserts an element at the front of the deque.</summary>
   /// <param name="element">The item to push to the front.</param>
   public void PushFront (T element) {
      mBuffer[mHead] = element;
      if (mHead == mTail) {
         Expand (false);
      } else {
         mHead = WrapIndex (mHead - 1);
      }
   }

   /// <summary>Inserts an element at the back of the deque.</summary>
   /// <param name="element">The item to push to the back.</param>
   public void PushBack (T element) {
      mBuffer[mTail] = element;
      if (mHead == mTail) {
         Expand (true);
      } else {
         mTail = WrapIndex (mTail + 1);
      }
   }

   /// <summary>Removes and returns the element at the front of the deque.</summary>
   /// <returns>The element removed from the front.</returns>
   /// <exception cref="Exception">Thrown when attempting to pop from an empty deque.</exception>
   public T PopFront () {
      int index = WrapIndex (mHead + 1);
      if (index == mTail) throw new Exception ("IndexError: pop from an empty deque");
      T value = mBuffer[index];
      mHead = index;
      if (Count < (mCapacity / 2) + 2 && mCapacity > MINIMUMSIZE) Shrink ();
      return value;
   }

   /// <summary>Removes and returns the element at the back of the deque.</summary>
   /// <returns>The element removed from the back.</returns>
   /// <exception cref="Exception">Thrown when attempting to pop from an empty deque.</exception>
   public T PopBack () {
      int index = WrapIndex (mTail - 1);
      if (index == mHead) throw new Exception ("IndexError: pop from an empty deque");
      T value = mBuffer[index];
      mTail = index;
      if (Count < (mCapacity / 2) + 2 && mCapacity > MINIMUMSIZE) Shrink ();
      return value;
   }

   /// <summary>Returns the element at the front of the deque without removing it.</summary>
   /// <returns>The element at the front of the deque.</returns>
   /// <exception cref="Exception">Thrown when the deque is empty.</exception>
   public T PeekLeft () {
      int index = WrapIndex (mHead + 1);
      if (index == mTail) throw new Exception ("IndexError: pop from an empty deque");
      return mBuffer[index];
   }

   /// <summary>Returns the element at the back of the deque without removing it.</summary>
   /// <returns>The element at the back of the deque.</returns>
   /// <exception cref="Exception">Thrown when the deque is empty.</exception>
   public T PeekRight () {
      int index = WrapIndex (mTail - 1);
      if (index == mHead) throw new Exception ("IndexError: pop from an empty deque");
      return mBuffer[index];
   }

   /// <summary>Determines whether the deque contains no elements.</summary>
   /// <returns>True if the deque is empty; otherwise, false.</returns>
   public bool IsEmpty () => WrapIndex (mTail - 1) == mHead;
   #endregion

   #region Implementations ------------------------------------------
   /// <summary>Reduces capacity by half when element count drops below threshold.</summary>
   void Shrink () {
      int count = Count;
      int newCapacity = (mCapacity / 2) + 2;
      var newBuffer = new T[newCapacity];
      for (int i = 0; i < count; i++)
         newBuffer[1 + i] = mBuffer[WrapIndex (mHead + 1 + i)];
      (mHead, mTail, mBuffer, mCapacity) = (0, (count + 1) % newCapacity, newBuffer, newCapacity);
   }

   /// <summary>Doubles the capacity of the buffer when full.</summary>
   /// <param name="fromBack">True if triggered by PushBack; false if by PushFront.</param>
   void Expand (bool fromBack) {
      int oldCapacity = mCapacity;
      var newBuffer = new T[oldCapacity * 2];
      int start = mHead + (fromBack ? 1 : 0);
      for (int i = 0; i < oldCapacity; i++)
         newBuffer[1 + i] = mBuffer[WrapIndex (start + i)];
      (mHead, mTail, mBuffer, mCapacity) = (0, oldCapacity + 1, newBuffer, oldCapacity * 2);
   }

   /// <summary>Computes the wrapped circular index for internal buffer access.</summary>
   /// <param name="ptr">The raw index offset.</param>
   /// <returns>The adjusted index bounded within buffer capacity.</returns>
   int WrapIndex (int ptr) => (ptr + Capacity) % Capacity;
   #endregion

   #region Fields ---------------------------------------------------
   int mHead = 0;
   int mTail = 1;
   int mCapacity = 4;
   const int MINIMUMSIZE = 4;
   T[] mBuffer;
   #endregion
}
#endregion