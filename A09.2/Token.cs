// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Token.cs
// Defines token types representing numbers, operators, punctuation, and errors.
// ------------------------------------------------------------------------------------------------

namespace Eval;

#region Class TEnd --------------------------------------------------------------------------------
/// <summary>Represents the end-of-input token.</summary>
class TEnd : Token {
   #region Methods --------------------------------------------------
   public override string ToString () => "end";
   #endregion
}
#endregion

#region Class TError ------------------------------------------------------------------------------
/// <summary>Represents a token error during evaluation.</summary>
class TError : Token {
   #region Constructors ---------------------------------------------
   public TError (string message) => Message = message;
   #endregion

   #region Properties -----------------------------------------------
   public string Message { get; private set; }
   #endregion

   #region Methods --------------------------------------------------
   public override string ToString () => $"error:{Message}";
   #endregion
}
#endregion

#region Class TLiteral ----------------------------------------------------------------------------
/// <summary>Represents a numeric literal token.</summary>
class TLiteral : TNumber {
   #region Constructors ---------------------------------------------
   public TLiteral (double f) => Value = f;
   #endregion

   #region Properties -----------------------------------------------
   public override double Value { get; }
   #endregion

   #region Methods --------------------------------------------------
   public override string ToString () => $"literal:{Value}";
   #endregion
}
#endregion

#region abstract Class TNumber --------------------------------------------------------------------
/// <summary>Abstract base class for numeric tokens.</summary>
abstract class TNumber : Token {
   #region abstract Properties --------------------------------------
   public abstract double Value { get; }
   #endregion
}
#endregion

#region Class Token -------------------------------------------------------------------------------
/// <summary>Abstract base class for all token types.</summary>
abstract class Token {
}
#endregion

#region Class TOpArithmetic -----------------------------------------------------------------------
/// <summary>Represents arithmetic operator tokens.</summary>
class TOpArithmetic : TOperator {
   #region Constructors ---------------------------------------------
   public TOpArithmetic (Evaluator eval, char ch) : base (eval) {
      Op = ch;
      Priority = sPriority[ch];
   }
   #endregion

   #region Properties -----------------------------------------------
   public char Op { get; private set; }
   public override int Priority { get; set; }
   #endregion

   #region Methods --------------------------------------------------
   public double Evaluate (double a, double b) {
      return Op switch {
         '+' => a + b,
         '-' => a - b,
         '*' => a * b,
         '/' => a / b,
         '^' => Math.Pow (a, b),
         _ => throw new EvalException ($"Unknown operator: {Op}")
      };
   }

   public override string ToString () => $"op:{Op}:{Priority}";
   #endregion

   #region Fields ---------------------------------------------------
   static Dictionary<char, int> sPriority = new () {
      ['+'] = 1, ['-'] = 1, ['*'] = 2, ['/'] = 2, ['^'] = 3, ['='] = 4
   };
   #endregion
}
#endregion

#region Class TOpFunction -------------------------------------------------------------------------
/// <summary>Represents mathematical function operator tokens.</summary>
class TOpFunction : TOperator {
   #region Constructors ---------------------------------------------
   public TOpFunction (Evaluator eval, string name) : base (eval) {
      Func = name;
      Priority = 4;
   }
   #endregion

   #region Properties -----------------------------------------------
   public string Func { get; private set; }
   public override int Priority { get; set; }
   #endregion

   #region Methods --------------------------------------------------
   public double Evaluate (double f) {
      return Func switch {
         "sin" => Math.Sin (D2R (f)),
         "cos" => Math.Cos (D2R (f)),
         "tan" => Math.Tan (D2R (f)),
         "sqrt" => Math.Sqrt (f),
         "log" => Math.Log (f),
         "exp" => Math.Exp (f),
         "asin" => R2D (Math.Asin (f)),
         "acos" => R2D (Math.Acos (f)),
         "atan" => R2D (Math.Atan (f)),
         _ => throw new EvalException ($"Unknown function: {Func}")
      };

      double D2R (double f) => f * Math.PI / 180;
      double R2D (double f) => f * 180 / Math.PI;
   }

   public override string ToString () => $"func:{Func}:{Priority}";
   #endregion
}
#endregion

#region abstract Class TOperator ------------------------------------------------------------------
/// <summary>Abstract base class for operator tokens.</summary>
abstract class TOperator : Token {
   #region Constructors ---------------------------------------------
   protected TOperator (Evaluator eval) => mEval = eval;
   #endregion

   #region abstract Properties --------------------------------------
   public abstract int Priority { get; set; }
   #endregion

   #region Fields ---------------------------------------------------
   protected readonly Evaluator mEval;
   #endregion
}
#endregion

#region Class TOpUnary ----------------------------------------------------------------------------
/// <summary>Represents unary operator tokens.</summary>
class TOpUnary : TOperator {
   #region Constructors ---------------------------------------------
   public TOpUnary (Evaluator eval, string name) : base (eval) {
      Uname = name;
      Priority = 4;
   }
   #endregion

   #region Properties -----------------------------------------------
   public override int Priority { get; set; }
   public string Uname { get; private set; }
   #endregion

   #region Methods --------------------------------------------------
   public double Evaluate (double f) {
      return Uname switch {
         "u+" => +f,
         "u-" => -f,
         _ => throw new EvalException ($"Unknown function: {Uname}")
      };
   }

   public override string ToString () => $"func:{Uname}:{Priority}";
   #endregion
}
#endregion

#region Class TPunctuation ------------------------------------------------------------------------
/// <summary>Represents punctuation tokens like parentheses.</summary>
class TPunctuation : Token {
   #region Constructors ---------------------------------------------
   public TPunctuation (char ch) => Punct = ch;
   #endregion

   #region Properties -----------------------------------------------
   public char Punct { get; private set; }
   #endregion

   #region Methods --------------------------------------------------
   public override string ToString () => $"Punct:{Punct}";
   #endregion
}
#endregion

#region Class TVariable ---------------------------------------------------------------------------
/// <summary>Represents variable tokens.</summary>
class TVariable : TNumber {
   #region Constructors ---------------------------------------------
   public TVariable (Evaluator eval, string name) => (Name, mEval) = (name, eval);
   #endregion

   #region Properties -----------------------------------------------
   public string Name { get; private set; }
   public override double Value { get => mEval.GetVariable (Name); }
   #endregion

   #region Methods --------------------------------------------------
   public override string ToString () => $"var:{Name}";
   #endregion

   #region Fields ---------------------------------------------------
   readonly Evaluator mEval;
   #endregion
}
#endregion