// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joiners at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Program.cs
// For testing the custom file parser program.
// ------------------------------------------------------------------------------------------------
using Parse;

#region Class Program -----------------------------------------------------------------------------
/// <summary> Tests the program against various test cases and prints their output. </summary>
class Program {
   /// <summary> Runs the test to check if the file path is parsed correctly. </summary>
   static void Main () {
      var testPaths = new[] {
         @"Cz:\abc\def\r.txt", @"C:\abc\def\readme.txt", @"C:\Readme.txt", @"C:\abc\.bcf",
         @"C:\abc\bcf.", @"Readme.txt", @"C:\abc\def", @"C:\abc d", @"\abcd\Readme.txt", " ",
         @"C:\ab.c\def\r.txt", @"C:\abc:d", @".\abc", ".abc", "abc", @"C:\abc6\def\r.txt",
         @"C:\DIR\ARCHIVE.TAR.GZ", @"C:\work\r.txt", @"C:\\work~\\r.txt~",
         @"C:\A\B\C\D\E\F\G\FILE.TXT", @"C:\DIR\\FILE.TXT"
      };
      Console.WriteLine ($"{"Input",-38}{"Drive",-12}{"Folder",-20}" +
         $"{"Filename",-12}{"Extension",-11}{"Result"}");
      Console.WriteLine (new string ('_', 100));
      foreach (var path in testPaths) {
         var (drive, directory, filename, ext) = Parser.Evaluate (path);
         // check if every part of the path is present
         bool iPassed = !string.IsNullOrEmpty (drive) || !string.IsNullOrEmpty (directory)
                        || !string.IsNullOrEmpty (filename) || !string.IsNullOrEmpty (ext);
         string driveStr = iPassed && !string.IsNullOrEmpty (drive) ? drive : "-";
         string folderStr = iPassed && !string.IsNullOrEmpty (directory) ? directory : "|-";
         string fileStr = iPassed && !string.IsNullOrEmpty (filename) ? "|" + filename : "|-";
         string extStr = iPassed && !string.IsNullOrEmpty (ext) ? "|." + ext : "|-";
         Console.Write ($"{path,-38}{driveStr,-12}{folderStr,-20}{fileStr,-12}{extStr,-11}");
         if (iPassed) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine ("|Passed");
         } else {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine ("|Failed");
         }
         Console.ResetColor ();
      }
   }
}
#endregion