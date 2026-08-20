using static State;

/// See documentation in <see href="Diagram.png">the local readme file</see>
namespace FileParser {
   class Parser {
      static void Main () {
         var testPaths = new[] {
             @"Cz:\abc\def\r.txt", @"C:\abc\def\readme.txt", @"C:\Readme.txt", @"C:\abc\.bcf",
             @"C:\abc\bcf.", @"Readme.txt", @"C:\abc\def", @"C:\abc d", @"\abcd\Readme.txt", " ",
             @"C:\ab.c\def\r.txt", @"C:\abc:d", @".\abc", ".abc", "abc", @"C:\abc6\def\r.txt",
             @"C:\abc\def\r.txt.txt", @"C:\work\r.txt", @"C:\\work~\\r.txt~",  @"C:\\work~\\r..txt~"
         };

         for(int i=0;i<testPaths.Length;i++) {
            State st = A;
            Action<char> None = (char a) => { };
            Action<char> todo;
            string Drive = "";
            string Directory = "";
            string Extension = "";
            foreach (var ch in testPaths[i].ToUpper()+'~') {
               todo = None;
               (st, todo) = (st, ch) switch {
                  // Starting at Drive
                  (A, >= 'A' and <= 'Z') => (B, (x) => Drive += x),
                  // State 2 getting to column 
                  (B, ':') => (C, None),
                  // State 3 getting to first slash 
                  (C, '\\') => (D, None),
                  // State 4 getting to DirName
                  (D or E, >= 'A' and <= 'Z') => (E, (x) => Directory += x),
                  // State 5 moving to something acceptable dir 
                  (E, '\\') => (F, (x) => Directory += x),
                  // State 6 looping through as dir 
                  (F or G, >= 'A' and <= 'Z') => (G, (x) => Directory += x),
                  (G,'\\') => (F, (x) => Directory += x),
                  // State 6 getting to the extention part 
                  (G, '.') => (H, None),
                  // State 7 getting the extension part accumulated
                  (H or I, >= 'A' and <= 'Z') => (I, (x) => Extension += x),
                  // State 8 getting out : )
                  (I, '~') => (J, None),
                  _ => (Z, None)
               };
               todo (ch);
            }
            Console.WriteLine ($"For Path... {i}, {st}");
            if (st is J) {
               var items = Directory.Split('\\');
               string Filename = items[items.Length - 1];
               Directory = string.Join ("/", items.SkipLast (1));
               Console.WriteLine ($"{Drive,-10},{Directory,-35},{Filename},{Extension,-5}");
            } else Console.WriteLine ("Wrong Path");
         }
      }
   }
}
enum State { A, B, C, D, E, F, G, H, I, J, Z }