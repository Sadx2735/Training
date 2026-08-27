using System.Text;

namespace Wordle;

class Program {
   static int mRow = 0;
   static int mCursor = 0;
   const int WORDSIZE = 5;
   const int TRIES = 6;
   static int GridStart = (Console.WindowWidth - 21) / 2;
   static int KeyStart = (Console.WindowWidth - 36) / 2;

   static Random r = new Random ();
   static string[] available = File.ReadAllLines (@"C:\\Work\\Training\\A12\\puzzle-5.txt");
   static string[] dictionary = File.ReadAllLines (@"C:\\Work\\Training\\A12\\dict-5.txt");

   static int L = available.Length;
   static string EXPECTED = available[r.Next (1, L) - 1];

   static char[][] MemBuffer = new char[6][];
   static int[] MemBufferColor = new int[30];
   static int[] KeyBuffer = new int[26];

   static void Main () {
      Console.OutputEncoding = Encoding.UTF8;
      Console.CursorVisible = false;

      for (int i = 0; i < TRIES; i++)
         MemBuffer[i] = new char[5];
      for (; ; ) {
         if (mRow < 6) {
            ProcessRow (mRow);
            mRow++;
         }
         else {
            DisplayBoard ();
            PrintLoss ($"{EXPECTED} IS THE WORD! , PLEASE TRY AGAIN!");
         }
      }
   }

   static void PrintLoss (string message) {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine ('\n');
      var MesStart = (Console.WindowWidth - message.Length) / 2;
      Console.SetCursorPosition (MesStart, Console.CursorTop);
      Console.WriteLine (message);
      Console.ResetColor ();
      Console.ReadKey (true);
      Environment.Exit (0);
   }

   static void PrintWon (string message) {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine ('\n');
      var MesStart = (Console.WindowWidth - message.Length) / 2;
      Console.SetCursorPosition (MesStart, Console.CursorTop);
      Console.WriteLine (message);
      Console.ResetColor ();
      Console.ReadKey (true);
      Environment.Exit (0);
   }

   static void Dosomething (string message) {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine ('\n');
      var MesStart = (Console.WindowWidth - message.Length) / 2;
      Console.SetCursorPosition (MesStart, Console.CursorTop);
      Console.WriteLine (message);
      Console.ResetColor ();
   }

   static void ProcessRow (int rval) {
      DisplayBoard ();
      for (; ; ) {
         var key = Console.ReadKey (true);
         var output = ProcessKey (key);
         DisplayBoard ();
         switch (output) {
            case State.IDLE: continue;
            case State.NOTINROW: Dosomething ("WORD IS NOT IN THE DICTIONARY!"); continue;
            case State.NEXTROW: return;
            case State.OVER: mRow++; DisplayBoard (); PrintWon ("YOU GUESSED IT CORRECTLY!"); return;
            default: return;
         }
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
            return ProcessGuess ();
         }
         return State.IDLE;
      }

      State ProcessGuess() {
         HashSet<char> Seen = [];
         int[] cBuffer = Enumerable.Repeat (1, 5).ToArray ();
         for (int i = 0; i < WORDSIZE; i++) {
            if (MemBuffer[rval][i] == EXPECTED[i]) {
               cBuffer[i] = 3;
               Seen.Add (MemBuffer[rval][i]);
            }
         }

         var guessed = new string(MemBuffer[rval]);
         if (guessed == EXPECTED) { FillColorInfo (); return State.OVER; }
         else if (dictionary.Contains (guessed)) { FillColorInfo (); return State.NEXTROW; } 
         else if (!dictionary.Contains (guessed)) { return State.NOTINROW; } else return State.IDLE;

         void FillColorInfo () {
            for (int i = 0; i < WORDSIZE; i++) {
               if (cBuffer[i] != 3 && !Seen.Contains (MemBuffer[rval][i])) {
                  cBuffer[i] = (EXPECTED.Contains (MemBuffer[rval][i])) ? 2 : 1;
                  Seen.Add (MemBuffer[rval][i]);
               }
            }
            for (int idx = 0; idx < WORDSIZE; idx++) {
               MemBufferColor[idx + (mRow * WORDSIZE)] = cBuffer[idx];
               var index = MemBuffer[rval][idx] - 'A';
               KeyBuffer[index] = int.Max (KeyBuffer[index], cBuffer[idx]);
            }
         }
      }

   }

   static void DisplayBoard () {
      Console.Clear ();
      DisplayMainboard ();
      Console.SetCursorPosition (GridStart, Console.CursorTop);
      Console.WriteLine (string.Join ("*", Enumerable.Range (1, 12).Select (ch => '-')));
      Console.Write ('\n');
      DisplayKeyboard ();
   }

   static void DisplayMainboard () {
      for (int row = 0; row < 6; row++) {
         Console.SetCursorPosition (GridStart, Console.CursorTop);
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
      Console.SetCursorPosition (KeyStart, Console.CursorTop);
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
         if (i % 8 == 0) { 
            Console.Write ("\n\n");
            Console.SetCursorPosition (KeyStart, Console.CursorTop); 
         }
      }
   }

   public enum State { IDLE,NOTINROW,NEXTROW,OVER};
}