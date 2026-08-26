// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Tokenizer.cs
// Tokenizes input text into discrete tokens.
// ------------------------------------------------------------------------------------------------

namespace Eval;

#region Class Tokenizer ---------------------------------------------------------------------------
/// <summary>Tokenizes an input expression string into individual tokens.</summary>
class Tokenizer {
   #region Constructors ---------------------------------------------
   public Tokenizer (Evaluator eval, string text) {
      mText = text; mN = 0; mEval = eval;
   }
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Advances the pointer and retrieves the next token from the input.</summary>
   /// <returns>The next parsed token.</returns>
   public Token Next () => mPrev = FetchNext ();
   #endregion

   #region Implementations ------------------------------------------
   // Fetches the next available token from the current position in the text.
   Token FetchNext () {
      while (mN < mText.Length) {
         char ch = char.ToLower (mText[mN++]);
         switch (ch) {
            case ' ' or '\t': continue;
            case (>= '0' and <= '9') or '.': return GetNumber ();
            case '(' or ')': return new TPunctuation (ch);
            case '+' or '-' or '*' or '/' or '^' or '=':
               bool iOperandEnd = mPrev is TNumber
                  || (mPrev is TPunctuation tp && tp.Punct == ')');
               bool iUnary = (ch is '+' or '-') && !iOperandEnd;
               string oper = (ch == '+') ? "u+" : "u-";
               if (iUnary) return new TOpUnary (mEval, oper);
               return new TOpArithmetic (mEval, ch);
            case >= 'a' and <= 'z': return GetIdentifier ();
            default: return new TError ($"Unknown symbol: {ch}");
         }
      }
      return new TEnd ();
   }

   // Parses a variable name or built-in function identifier.
   Token GetIdentifier () {
      int start = mN - 1;
      while (mN < mText.Length) {
         char ch = char.ToLower (mText[mN++]);
         if (ch is >= 'a' and <= 'z') continue;
         mN--; break;
      }
      string sub = mText[start..mN];
      if (mFuncs.Contains (sub)) return new TOpFunction (mEval, sub);
      else return new TVariable (mEval, sub);
   }

   // Parses a numeric literal from the current position.
   Token GetNumber () {
      int start = mN - 1;
      while (mN < mText.Length) {
         char ch = mText[mN++];
         if (ch is (>= '0' and <= '9') or '.') continue;
         mN--; break;
      }
      string sub = mText[start..mN];
      if (double.TryParse (sub, out double f)) return new TLiteral (f);
      return new TError ($"Invalid number: {sub}");
   }
   #endregion

   #region Fields ---------------------------------------------------
   readonly Evaluator mEval;
   readonly string[] mFuncs = { "sin", "cos", "tan", "sqrt", "log",
                                "exp", "asin", "acos", "atan" };
   int mN;
   Token? mPrev = null;
   readonly string mText;
   #endregion
}
#endregion