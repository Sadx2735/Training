

namespace Parse;

using static State;
class Parser {
   public static (string, string, string, string) Evaluate (string input) {
      var st = A;
      Action<char> none = (char a) => { };
      Action<char> todo;
      string drive = "";
      string directory = "";
      string extension = "";
      foreach (var ch in input.ToUpper () + '~') {
         todo = none;
         (st, todo) = (st, ch) switch {
            (A, >= 'A' and <= 'Z') => (B, (x) => drive += x),
            (B, ':') => (C, none),
            (C, '\\') => (D, none),
            (D or E, >= 'A' and <= 'Z') => (E, (x) => directory += x),
            (E, '\\') => (F, (x) => directory += x),
            (F or G, >= 'A' and <= 'Z') => (G, (x) => directory += x),
            (G, '\\') => (F, (x) => directory += x),
            (G, '.') => (H, none),
            (H or I, >= 'A' and <= 'Z') => (I, (x) => extension += x),
            (I, '~') => (J, none),
            _ => (Z, none)
         };
         todo (ch);
      }
      if (st is J) {
         var items = directory.Split ('\\');
         string Filename = items[^1];
         directory = string.Join ("/", items.SkipLast (1));
         return (drive, directory, Filename, extension);
      }
      return (string.Empty, string.Empty, string.Empty, string.Empty);
   }
}
enum State { A, B, C, D, E, F, G, H, I, J, Z }