
for (; ; ) {
   Console.Write ("Enter the Number : ");

   string UserInput = Console.ReadLine ().Trim ().ToLower ();
   int sign = 1;

   bool signHasEdited = false;
   bool NumberInputed = false;

   bool HasDot = false;
   bool IsAfterDot = false;

   bool flag = true;
   bool HasE = false;

   int NumberafE = 0;
   int SignE = 1;
   bool SignafterE = false;

   bool HasNumberafterE = false;
   bool HasNumberafterD = false;

   double factor = 10;
   double total = 0;
   foreach (var ch in UserInput) {
      if (!HasE && !NumberInputed && !signHasEdited && (ch is '-' or '+')) {
         sign = (ch == '+') ? 1 : -1;
         signHasEdited = true;
      } else if (!HasE && (ch is >= '0' && ch <= '9')) {
         if (!IsAfterDot) { total = (total * 10) + ch - '0'; } else { total += ((1 / factor) * (ch - '0')); factor *= 10; HasNumberafterD = true; }
         NumberInputed = true;
      } else if (ch == 'e' && NumberInputed) {
         HasE = true;
      } else if (HasE && !SignafterE && !HasNumberafterE && (ch is '-' or '+')) {
         SignE = (ch == '+') ? 1 : -1;
         SignafterE = true;
      } else if (HasE && (ch is >= '0' && ch <= '9')) {
         NumberafE = (NumberafE * 10) + (ch - '0');
         HasNumberafterE = true;
      } else if (!HasE && NumberInputed && !HasDot && ch == '.') {
         HasDot = true;
         IsAfterDot = true;
      } else {
         flag = false;
         break;
      }
   }
   if (flag && NumberInputed && (!HasDot || HasNumberafterD) && (!HasE || HasNumberafterE))
      Console.WriteLine ($"Output of the Double in String is {sign * total * Math.Pow (10, SignE * NumberafE)}");
   else {
      Console.WriteLine ($"Output of the Double in String is {Double.NaN}");
   }
}