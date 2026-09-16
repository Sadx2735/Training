// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) TRUMPF Metamation India.
// ------------------------------------------------------------------------------------------------
// PriorityQueue.cs
// Custom PriorityQueue implementation.
// ------------------------------------------------------------------------------------------------

namespace CustomPriorityQueue;

#region Class MyPriorityQueue ---------------------------------------------------------------------
/// <summary>Implements a custom min-heap priority queue.</summary>
class MyPriorityQueue<T> where T : IComparable<T> {
   #region Properties -----------------------------------------------
   /// <summary>Returns true if the heap is empty; otherwise, false.</summary>
   public bool IsEmpty => mHeap.Count == 0;
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Removes and returns the smallest element from the priority queue.</summary>
   /// <returns>The minimum element in the priority queue.</returns>
   /// <exception cref="InvalidOperationException">Thrown if priority queue is empty.</exception>
   public T Dequeue () {
      if (IsEmpty) throw new InvalidOperationException ("Priority queue is empty.");
      var minValue = mHeap[0];
      SiftDown ();
      return minValue;
   }

   /// <summary>Adds an element to the priority queue.</summary>
   /// <param name="value">The item to add.</param>
   public void Enqueue (T value) {
      mHeap.Add (value);
      SiftUp ();
   }

   #endregion

   #region Implementation -------------------------------------------
   // Restores heap property by moving the root element down.
   void SiftDown () {
      mHeap[0] = mHeap[^1];
      mHeap.RemoveAt (mHeap.Count - 1);
      var (currentIndex, leftChild) = (0, 1);
      while (leftChild < mHeap.Count) {
         var (minIndex, rightChild) = (leftChild, 2 * currentIndex + 2);
         if (rightChild < mHeap.Count && mHeap[rightChild].CompareTo (mHeap[leftChild]) < 0)
            minIndex = rightChild;
         if (mHeap[currentIndex].CompareTo (mHeap[minIndex]) <= 0) break;
         (mHeap[currentIndex], mHeap[minIndex]) = (mHeap[minIndex], mHeap[currentIndex]);
         (currentIndex , leftChild) = (minIndex, 2 * minIndex + 1);
      }
   }

   // Restores heap property by moving the last element up.
   void SiftUp () {
      int childIndex = mHeap.Count - 1;
      while (childIndex > 0) {
         int parentIndex = (childIndex - 1) / 2;
         if (mHeap[childIndex].CompareTo (mHeap[parentIndex]) >= 0) break;
         (mHeap[childIndex], mHeap[parentIndex]) = (mHeap[parentIndex], mHeap[childIndex]);
         childIndex = parentIndex;
      }
   }
   #endregion

   #region Fields ---------------------------------------------------
   List<T> mHeap = [];
   #endregion
}
#endregion