using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Eval;

class EvalException : Exception {
   public EvalException (string message) : base (message) { }
}

class Evaluator {
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
      if (tokens.Count > 2 && tokens[0] is TVariable tvar && tokens[1] is TOpArithmetic { Op: '=' }) {
         tVariable = tvar;
         tokens.RemoveRange (0, 2);
      }
      foreach (var t in tokens) Process (t);
      while (mOperators.Count > 0) ApplyOperator ();
      double f = mOperands.Pop ();
      if (tVariable != null) mVars[tVariable.Name] = f;
      return f;
   }

   public int BasePriority { get; private set; }

   public double GetVariable (string name) {
      if (mVars.TryGetValue (name, out double f)) return f;
      throw new EvalException ($"Unknown variable: {name}");
   }
   readonly Dictionary<string, double> mVars = new ();

   void Process (Token token) {
      switch (token) {
         case TNumber num:
            mOperands.Push (num.Value);
            break;
         case TOperator op:
            op.Priority += BasePriority;
            bool isLeftAssoc = op is TOpArithmetic { Op: '+' or '-' or '*' or '/' };
            while (mOperators.Count > 0 &&
                  (isLeftAssoc ? mOperators.Peek ().Priority >= op.Priority : mOperators.Peek ().Priority > op.Priority)) 
               ApplyOperator ();
            mOperators.Push (op);
            break;
         case TPunctuation p:
            BasePriority += p.Punct == '(' ? 10 : -10;
            break;
         default:
            throw new EvalException ($"Unknown token: {token}");
      }
   }
   readonly Stack<double> mOperands = new ();
   readonly Stack<TOperator> mOperators = new ();

   void ApplyOperator () {
      var op = mOperators.Pop ();
      var f2 = mOperands.Pop ();
      switch (op) {
         case TOpFunction fn:
            mOperands.Push (fn.Evaluate (f2));
            break;
         case TUnary u:
            mOperands.Push (u.Evaluate (f2));
            break;
         case TOpArithmetic arith:
            var f1 = mOperands.Pop ();
            mOperands.Push (arith.Evaluate (f1, f2));
            break;
         default:
            throw new EvalException ($"Unsupported operator type: {op.GetType ().Name}");
      }
   }
}