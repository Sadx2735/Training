var hq = new Heap<int> ();

hq.Enqueue (99);
hq.Enqueue (9);
hq.Enqueue (24);
hq.Enqueue (18);

hq.PrintHeapQ ();

Console.WriteLine (hq.Dequeue ());
hq.PrintHeapQ ();

class Heap<T> where T : IComparable<T> {
   List<T> mHeap = new();
   public void Enqueue (T value) {
      mHeap.Add (value);
      SiftUp ();
   }

   public void PrintHeapQ() {
      Console.WriteLine ("Printing..");
      foreach(var item in mHeap) {
         Console.Write($"{item} ");
      }
      Console.WriteLine ("- - - - - - -");
   }
   public T Dequeue () {
      if (IsEmpty) throw new Exception ("Empty HeapQ");
      var value = mHeap[0];
      SiftDown ();
      return value;
   }

   public bool IsEmpty => mHeap.Count == 0;

   public void SiftDown () {
      (mHeap[mHeap.Count - 1], mHeap[0]) = (mHeap[0], mHeap[mHeap.Count - 1]);
      mHeap.RemoveAt (mHeap.Count-1);
      int currentPos = 0;
      int leftChild = 2*currentPos+1;
      while(leftChild<=mHeap.Count-1) {
         int minimum = leftChild;
         int rightChild = 2 * currentPos + 2;
         if(rightChild<=mHeap.Count-1 && mHeap[rightChild].CompareTo (mHeap[leftChild])<0) {
            minimum = rightChild;
         }
         if (mHeap[currentPos].CompareTo (mHeap[minimum]) < 1) break;
         (mHeap[currentPos], mHeap[minimum]) = (mHeap[minimum], mHeap[currentPos]);
         currentPos = minimum;
         leftChild = 2 * currentPos + 1;
      }
   }

   public void SiftUp () {
      int Cp = mHeap.Count-1;
      while (Cp > 0 && mHeap[Cp].CompareTo (mHeap[(Cp-1) / 2]) < 0) {
         (mHeap[Cp], mHeap[(Cp - 1)/2]) = (mHeap[(Cp - 1)/ 2], mHeap[Cp]);
         Cp = (Cp - 1) / 2;
      }
   }
}
