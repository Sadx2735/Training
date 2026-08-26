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
      if (tokens.Count > 2 && tokens[0] is TVariable tvar
                           && tokens[1] is TOpArithmetic { Op: '=' }) {
         tVariable = tvar;
         tokens.RemoveRange (0, 2);
      }
      foreach (var t in tokens) Process (t);
      while (mOperators.Count > 0) ApplyOperator ();
      double f = mOperands.Pop ();
      // Ideal case must have 0 for all the three that are down.
      if (mOperators.Count > 0) throw new EvalException ("Too few operands.");
      if (mOperands.Count > 0) throw new EvalException ("Too few operators.");
      if (BasePriority > 0) throw new EvalException ("Missing closing parenthesis.");
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
   readonly Stack<double> mOperands = new ();
   readonly Stack<TOperator> mOperators = new ();

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
}