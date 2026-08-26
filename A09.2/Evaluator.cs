// ------------------------------------------------------------------------------------------------
// Training ~ A training program for new joinees at Metamation, Batch - July 2026.
// Copyright (c) Metamation India.
// ------------------------------------------------------------------------------------------------
// Evaluator.cs
// Converts input into tokens and processes them.
// ------------------------------------------------------------------------------------------------

namespace Eval;

#region Class EvalException -----------------------------------------------------------------------
/// <summary>Implements custom evaluation exception.</summary>
class EvalException : Exception {
   #region Constructors ---------------------------------------------
   public EvalException (string message) : base (message) { }
   #endregion
}
#endregion

#region Class Evaluator ---------------------------------------------------------------------------
/// <summary>Evaluates mathematical expressions and variable assignments.</summary>
class Evaluator {
   #region Properties -----------------------------------------------
   public int BasePriority { get; private set; }
   #endregion

   #region Methods --------------------------------------------------
   /// <summary>Breaks the input string into tokens and evaluates the expression.</summary>
   /// <param name="text">The raw mathematical expression or assignment statement.</param>
   /// <returns>The resulting numerical value of the evaluated expression.</returns>
   /// <exception cref="EvalException">Thrown when the input contains errors.</exception>
   public double Evaluate (string text) {
      mOperands.Clear ();
      mOperators.Clear ();
      BasePriority = 0;
      List<Token> tokens = new ();
      var tokenizer = new Tokenizer (this, text);
      for (; ; ) {
         var token = tokenizer.Next ();
         if (token is TEnd) break;
         if (token is TError err) throw new EvalException (err.Message);
         tokens.Add (token);
      }
      // Check if this is a variable assignment
      TVariable? tVariable = null;
      if (tokens.Count > 2 && tokens[0] is TVariable tvar
                           && tokens[1] is TOpArithmetic { Op: '=' }) {
         tVariable = tvar;
         tokens.RemoveRange (0, 2);
      }
      foreach (var t in tokens) Process (t);
      while (mOperators.Count > 0) ApplyOperator ();
      double f = mOperands.Pop ();
      // Ideal case must have 0 for all three stacks/counters below.
      if (mOperators.Count > 0) throw new EvalException ("Too few operands.");
      if (mOperands.Count > 0) throw new EvalException ("Too few operators.");
      if (BasePriority > 0) throw new EvalException ("Missing closing parenthesis.");
      if (tVariable != null) mVars[tVariable.Name] = f;
      return f;
   }

   /// <summary>Retrieves the double value of a previously stored variable.</summary>
   /// <param name="name">The name of the variable to look up.</param>
   /// <returns>The numerical value stored under the variable name.</returns>
   /// <exception cref="EvalException">When variable does not exist error.</exception>
   public double GetVariable (string name) {
      if (mVars.TryGetValue (name, out double f)) return f;
      throw new EvalException ($"Unknown variable: {name}");
   }
   #endregion

   #region Implementation -------------------------------------------
   // Applies operators from stack onto operands.
   void ApplyOperator () {
      if (mOperators.Count == 0) throw new EvalException ("Too few operators.");
      if (mOperands.Count == 0) throw new EvalException ("Too few operands.");
      var op = mOperators.Pop ();
      var f2 = mOperands.Pop ();
      switch (op) {
         case TOpFunction fnFunc:
            mOperands.Push (fnFunc.Evaluate (f2));
            break;
         case TOpUnary fnUnary:
            mOperands.Push (fnUnary.Evaluate (f2));
            break;
         case TOpArithmetic arith:
            if (mOperands.Count == 0)
               throw new EvalException ("Too few mOperands");
            var f1 = mOperands.Pop ();
            mOperands.Push (arith.Evaluate (f1, f2));
            break;
         default:
            throw new EvalException ($"Unsupported operator type: {op.GetType ().Name}");
      }
   }

   // Processes individual tokens and handles precedence.
   void Process (Token token) {
      switch (token) {
         case TNumber num:
            mOperands.Push (num.Value);
            break;
         case TOperator op:
            op.Priority += BasePriority;
            bool iRightAssoc = op is TOpUnary
               || op is TOpFunction || (op is TOpArithmetic opArith && opArith.Op == '^');
            while (mOperators.Count > 0 && (iRightAssoc
                  ? mOperators.Peek ().Priority > op.Priority
                  : mOperators.Peek ().Priority >= op.Priority))
               ApplyOperator ();
            mOperators.Push (op);
            break;
         case TPunctuation p:
            BasePriority += p.Punct == '(' ? 10 : -10;
            if (BasePriority < 0) throw new EvalException ("Too many closing parentheses.");
            break;
         default:
            throw new EvalException ($"Unknown token: {token}");
      }
   }
   #endregion

   #region Fields ---------------------------------------------------
   readonly Stack<double> mOperands = new ();
   readonly Stack<TOperator> mOperators = new ();
   readonly Dictionary<string, double> mVars = new ();
   #endregion
}
#endregion