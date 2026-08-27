using System.Text;

namespace Wordle;

class Program {
   static int mRow = 0;
   static int mCursor = 0;
   const int WORDSIZE = 5;
   const int TRIES = 6;

   const string EXPECTED = "MANGO";

   static char[][] MemBuffer = new char[6][];
   static int[] MemBufferColor = new int[30];
   static int[] KeyBuffer = new int[26];

   static void Main () {
      Console.OutputEncoding = Encoding.UTF8;
      Console.CursorVisible = false;

      for (int i = 0; i < TRIES; i++)
         MemBuffer[i] = new char[5];
      for (; ; ) {
         if (mRow <= 6) {
            ProcessRow (mRow);
            mRow++;
         }
      }
   }

   static void ProcessRow (int rval) {
      for (; ; ) {
         DisplayBoard ();
         var key = Console.ReadKey (true);
         var output = ProcessKey (key);
         switch (output) {
            case State.IDLE: continue;
            case State.NEXTROW: return;
            case State.OVER: PrintWon ();return;
            default: return;
         }
      }

      void PrintWon() { 
      }

      State ProcessKey (ConsoleKeyInfo k) {
         if ((k.Key is >= ConsoleKey.A and <= ConsoleKey.Z)
            && mCursor >= 0 && mCursor < (rval + 1) * WORDSIZE) {
            (int row, int col) = (mCursor / WORDSIZE, mCursor % WORDSIZE);
            MemBuffer[row][col] = char.ToUpper (k.KeyChar);
            mCursor++;
         } else if (k.Key is ConsoleKey.Backspace && mCursor > rval * WORDSIZE) {
            if((mCursor == ((rval + 1) * WORDSIZE))) {
               mCursor--;
               (int row, int col) = (mCursor / WORDSIZE, mCursor % WORDSIZE);
               MemBuffer[row][col] = default;
            }
            else {
               (int row, int col) = (mCursor / WORDSIZE, mCursor % WORDSIZE);
               MemBuffer[row][col] = default;
               mCursor--;
            }
         } else if (k.Key is ConsoleKey.Enter && mCursor == (rval + 1) * WORDSIZE) {
            Console.WriteLine ($"User Guess is {string.Join('.', MemBuffer[rval])}");
            return (ProcessGuess ()) ? State.OVER:State.NEXTROW;
         }
         return State.IDLE;
      }

      bool ProcessGuess() {
         HashSet<char> Seen = [];
         int[] cBuffer = Enumerable.Repeat (1, 5).ToArray ();
         for (int i = 0; i < WORDSIZE; i++) {
            if (MemBuffer[rval][i] == EXPECTED[i]) {
               cBuffer[i] = 3;
               Seen.Add (MemBuffer[rval][i]);
            }
         }
         for(int i=0;i<WORDSIZE;i++) {
            if (cBuffer[i]!=3 && !Seen.Contains(MemBuffer[rval][i])) {
               cBuffer[i] = (EXPECTED.Contains (MemBuffer[rval][i])) ? 2 : 1;
               Seen.Add (MemBuffer[rval][i]);
            }
         }
         for (int i = 0; i < WORDSIZE; i++) {
            MemBufferColor[i+(mRow*WORDSIZE)] = cBuffer[i];
         }
         return cBuffer.Count (3) == WORDSIZE;
      }

   }

   static void DisplayBoard () {
      Console.SetCursorPosition(0,0);
      DisplayMainboard ();
      Console.WriteLine (string.Join ("*", Enumerable.Range (1, 12).Select (ch => '-')));
      Console.Write ('\n');
      DisplayKeyboard ();
   }

   static void DisplayMainboard () {
      for (int row = 0; row < 6; row++) {
         for (int col = 0; col < 5; col++) {
            if (mCursor / 5 == row && mCursor % 5 == col && mCursor<((mRow+1)*WORDSIZE)) 
               DrawUnAllocPt ('◌');
            else if (MemBuffer[row][col] == default) 
               DrawUnAllocPt ('·');
            else DrawAllocPt (row, col);
         }
         Console.WriteLine ("\n");
      }

      void DrawAllocPt (int rval, int cval) {
         if(rval<mRow) {
            switch (MemBufferColor[rval*WORDSIZE+cval]) {
               case 0: Console.ForegroundColor = ConsoleColor.White; break;
               case 1: Console.ForegroundColor = ConsoleColor.Red; break;
               case 2: Console.ForegroundColor = ConsoleColor.Blue; break;
               case 3: Console.ForegroundColor = ConsoleColor.Green; break;
               default: break;
            }
         }
         else Console.ForegroundColor = ConsoleColor.White;
         Console.Write ($"{MemBuffer[rval][cval],-5}");
         Console.ResetColor ();
      }

      void DrawUnAllocPt (char inp) {
         Console.ForegroundColor = ConsoleColor.White;
         Console.Write ($"{inp,-5}");
         Console.ResetColor ();
      }
   }
   static void DisplayKeyboard () {
      for (int i = 1; i <= 26; i++) {
         switch (KeyBuffer[i - 1]) {
            case 0: Console.ForegroundColor = ConsoleColor.White; break;
            case 1: Console.ForegroundColor = ConsoleColor.Red; break;
            case 2: Console.ForegroundColor = ConsoleColor.Blue; break;
            case 3: Console.ForegroundColor = ConsoleColor.Green; break;
            default: break;
         }
         Console.Write ($"{(char)(i + 64),-5}");
         Console.ResetColor ();
         if (i % 5 == 0) { Console.Write ("\n\n"); }
      }
   }

   public enum State { IDLE,NEXTROW,OVER};
}