namespace Eval;

abstract class Token {
}

abstract class TNumber : Token {
   public abstract double Value { get; }
}

class TLiteral : TNumber {
   public TLiteral (double f) => mValue = f;
   public override double Value => mValue;
   public override string ToString () => $"literal:{Value}";
   readonly double mValue;
}

class TVariable : TNumber {
   public TVariable (Evaluator eval, string name) => (Name, mEval) = (name, eval);
   public string Name { get; private set; }
   public override double Value => mEval.GetVariable (Name);
   public override string ToString () => $"var:{Name}";
   readonly Evaluator mEval;
}

abstract class TOperator : Token {
   protected TOperator (Evaluator eval) => mEval = eval;
   public abstract int Priority { get; set; }
   readonly protected Evaluator mEval;
}

class TOpArithmetic : TOperator {
   public TOpArithmetic (Evaluator eval, char ch) : base (eval) {
      Op = ch;
      Priority = sPriority[ch];
   }
   public char Op { get; private set; }
   public override string ToString () => $"op:{Op}:{Priority}";
   public override int Priority { get; set; } 

   static Dictionary<char, int> sPriority = new () {
      ['+'] = 1, ['-'] = 1, ['*'] = 2, ['/'] = 2, ['^'] = 3, ['='] = 4,
   };
   public double Evaluate (double a, double b) {
      return Op switch {
         '+' => a + b,
         '-' => a - b,
         '*' => a * b,
         '/' => a / b,
         '^' => Math.Pow (a, b),
         _ => throw new EvalException ($"Unknown operator: {Op}"),
      };
   }
}

class TOpFunction : TOperator {
   public TOpFunction (Evaluator eval, string name) : base (eval) { Func = name;Priority = 4; }
   public string Func { get; private set; }
   public override string ToString () => $"func:{Func}:{Priority}";
   public override int Priority { get; set; }

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
}

class TOpUnary : TOperator {
   public TOpUnary (Evaluator eval, string name) : base (eval) { Uname = name; Priority = 4; }
   public string Uname { get; private set; }
   public override string ToString () => $"func:{Uname}:{Priority}";
   public override int Priority { get; set; }

   public double Evaluate (double f) {
      return Uname switch {
         "u+" => +f,
         "u-" => -f,
         _ => throw new EvalException ($"Unknown function: {Uname}")
      };
   }
}

class TPunctuation : Token {
   public TPunctuation (char ch) => Punct = ch;
   public char Punct { get; private set; }
   public override string ToString () => $"Punct:{Punct}";
}

class TEnd : Token {
   public override string ToString () => "end";
}

class TError : Token {
   public TError (string message) => Message = message;
   public string Message { get; private set; }
   public override string ToString () => $"error:{Message}";
}