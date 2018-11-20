using System;
using System.Collections.Generic;

namespace ExpressionLib
{
    public class Expression
    {
        enum Priority { LeftBracket, Add, Subtract = Add, Multiply, Divide = Multiply, Power, Function }
        enum Associativity { Left, Right }

        #region Tokens

        /// <summary>
        /// Abstract Token base class which must be passed the Expression Stack which Execute references
        /// A Token represents all elements parsed in the Expression string including numbers, operators, brackets and functions
        /// </summary>
        abstract class Token
        {
            protected Stack<double> stack;

            abstract public void Execute();

            protected Token(Stack<double> stack)
            {
                this.stack = stack;
            }
        }

        /// <summary>
        /// Abstract Operator class representing all operators and functions parsed in the Expression string
        /// </summary>
        abstract class Operator : Token
        {
            public Priority Priority { get; protected set; }
            public Associativity Associativity { get; protected set; }

            // Bracket does not need to pass stack because it doesn't use it in Execute
            protected Operator(Priority priority, Associativity associativity, Stack<double> stack = null) : base(stack)
            {
                Priority = priority;
                Associativity = associativity;
            }
        }

        /// <summary>
        /// Abstract class representing binary operators parsed in the Expression string
        /// </summary>
        abstract class BinaryOperator : Operator
        {
            protected BinaryOperator(Priority priority, Associativity associativity, Stack<double> stack) : base(priority, associativity, stack) { }
        }

        /// <summary>
        /// Abstract Function class representing function operations parsed in the Expression string
        /// </summary>
        abstract class Function : Operator
        {
            // UnaryPlus does not pass the stack
            protected Function(Priority priority, Associativity associativity, Stack<double> stack = null) : base(priority, associativity, stack) { }
        }

        /// <summary>
        /// Concrete Number class representing numbers parsed in the Expression string
        /// </summary>
        class Number : Token
        {
            public double Value { get; private set; }

            public Number(double value, Stack<double> stack) : base(stack)
            {
                Value = value;
            }

            public override void Execute()
            {
                stack.Push(Value);
            }
        }

        /// <summary>
        /// Concrete Variable class representing variables parsed in the Expression string
        /// </summary>
        /// <remarks>Variables are stored in the internal variables Dictionary, the Value is updated when the Variable is <see cref="Set(string, double)"/></remarks>
        class Variable : Token
        {
            public double Value { get; set; }

            public Variable(double val, Stack<double> stack) : base(stack)
            {
                Value = val;
            }

            public override void Execute()
            {
                stack.Push(Value);
            }
        }

        class Add : BinaryOperator
        {
            public Add(Stack<double> stack) : base(Priority.Add, Associativity.Left, stack) { }

            public override void Execute()
            {
                stack.Push(stack.Pop() + stack.Pop());
            }
        }

        class Subtract : BinaryOperator
        {
            public Subtract(Stack<double> stack) : base(Priority.Subtract, Associativity.Left, stack) { }

            public override void Execute()
            {
                stack.Push(-stack.Pop() + stack.Pop());
            }
        }

        class Multiply : BinaryOperator
        {
            public Multiply(Stack<double> stack) : base(Priority.Multiply, Associativity.Left, stack) { }

            public override void Execute()
            {
                stack.Push(stack.Pop() * stack.Pop());
            }
        }

        class Divide : BinaryOperator
        {
            public Divide(Stack<double> stack) : base(Priority.Divide, Associativity.Left, stack) { }

            public override void Execute()
            {
                double b = stack.Pop();
                stack.Push(stack.Pop() / b);
            }
        }

        class Power : BinaryOperator
        {
            public Power(Stack<double> stack) : base(Priority.Power, Associativity.Right, stack) { }

            public override void Execute()
            {
                double b = stack.Pop();
                stack.Push(Math.Pow(stack.Pop(), b));
            }
        }

        class LeftBracket : Operator
        {
            public LeftBracket() : base(Priority.LeftBracket, Associativity.Left) { }

            public override void Execute()
            {
            }
        }

        class RightBracket : Token
        {
            public RightBracket() : base(null) { }

            public override void Execute()
            {
            }
        }

        class UnaryMinus : Function
        {
            public UnaryMinus(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(-stack.Pop());
            }
        }

        class UnaryPlus : Function
        {
            public UnaryPlus() : base(Priority.Function, Associativity.Right) { }

            public override void Execute()
            {
            }
        }

        class Factorial : Function
        {
            public Factorial(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                double d = stack.Pop();
                if (d % 1 != 0 || d < 0)
                {
                    stack.Push(double.NaN);
                    return;
                }
                if (d > int.MaxValue)
                {
                    stack.Push(double.PositiveInfinity);
                    return;
                }
                int n = (int)d;
                double result = 1;
                for (int i = 2; i <= n && !double.IsInfinity(result); i++)
                {
                    result = result * i;
                }
                stack.Push(result);
            }
        }

        class Sqrt : Function
        {
            public Sqrt(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(Math.Sqrt(stack.Pop()));
            }
        }

        class Sin : Function
        {
            public Sin(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(Math.Sin(stack.Pop()));
            }
        }

        class Cos : Function
        {
            public Cos(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(Math.Cos(stack.Pop()));
            }
        }

        class Tan : Function
        {
            public Tan(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(Math.Tan(stack.Pop()));
            }
        }

        class Asin : Function
        {
            public Asin(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(Math.Asin(stack.Pop()));
            }
        }

        class Acos : Function
        {
            public Acos(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(Math.Acos(stack.Pop()));
            }
        }

        class Atan : Function
        {
            public Atan(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(Math.Atan(stack.Pop()));
            }
        }

        class Log : Function
        {
            public Log(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(Math.Log(stack.Pop()));
            }
        }

        #endregion

        const char leftBracketChar = '(';
        const char rightBracketChar = ')';
        const char plusChar = '+';
        const char minusChar = '-';
        const char multiplyChar = '*';
        const char divideChar = '/';
        const char powerChar = '^';
        const char unaryPlusChar = '+';
        const char unaryMinusChar = '-';
        const char factorialChar = '!';
        const string binOpsString = "+-*/^";
        public string ExpressionString { get; private set; }
        private int p;                                              // Current position into the string for reading Tokens
        Dictionary<string, Variable> variables;                     // Dictionary of variables
        Stack<double> workStack;                                    // Stack workspace used when evaluating the RP list (Reverse Polish)
        Stack<Operator> operatorStack;                              // Stack to store the operators during the shunting yard algorithm
        List<Token> tokens;                                         // Final list of Tokens in reverse Polish after parsing the expression
        Dictionary<string, Function> functions;
        Dictionary<char, BinaryOperator> binaryOperators;
        Add opAdd;
        Subtract opSubtract;
        Multiply opMultiply;
        Divide opDivide;
        Power opPower;
        UnaryMinus opUnaryMinus;
        UnaryPlus opUnaryPlus;
        Factorial opFactorial;
        Sqrt opSqrt;
        Sin opSin;
        Cos opCos;
        Tan opTan;
        Asin opAsin;
        Acos opAcos;
        Atan opAtan;
        Log opLog;
        LeftBracket opLeftBracket;
        RightBracket opRightBracket;

        /// <summary>
        /// Create an Expression.
        /// </summary>
        /// <param name="expString">The maths expression to be evaluated which may include variables.</param>
        /// <exception cref="Exception">Many possible exceptions including Syntax Error. Evaluating an Expression after an error returns NaN.</exception>
        public Expression(string expString)
        {
            variables = new Dictionary<string, Variable>();
            workStack = new Stack<double>();
            operatorStack = new Stack<Operator>();
            tokens = new List<Token>();
            CreateOperators();
            set("pi", Math.PI);
            set("e", Math.E);
            set("inf", double.PositiveInfinity);
            set("NaN", double.NaN);
            set("Epsilon", double.Epsilon);
            set("MinValue", double.MinValue);
            set("MaxValue", double.MaxValue);
            ExpressionString = expString;
            try
            {
                parse();
            }
            catch (Exception)
            {
                tokens.Clear();
                tokens.Add(new Number(double.NaN, workStack));
                throw;
            }
        }

        void CreateOperators()
        {
            // Only one of each operation Token needs to be created
            opAdd = new Add(workStack);
            opSubtract = new Subtract(workStack);
            opMultiply = new Multiply(workStack);
            opDivide = new Divide(workStack);
            opPower = new Power(workStack);
            opLeftBracket = new LeftBracket();
            opRightBracket = new RightBracket();
            opUnaryMinus = new UnaryMinus(workStack);
            opUnaryPlus = new UnaryPlus();
            opFactorial = new Factorial(workStack);
            opSqrt = new Sqrt(workStack);
            opSin = new Sin(workStack);
            opCos = new Cos(workStack);
            opTan = new Tan(workStack);
            opLog = new Log(workStack);
            opAsin = new Asin(workStack);
            opAcos = new Acos(workStack);
            opAtan = new Atan(workStack);
            functions = new Dictionary<string, Function>
            {
                {"sqrt", opSqrt },
                {"sin", opSin },
                {"cos", opCos },
                {"tan", opTan },
                {"log", opLog },
                {"asin", opAsin },
                {"acos", opAcos },
                {"atan", opAtan }
            };
            binaryOperators = new Dictionary<char, BinaryOperator>
            {
                {plusChar, opAdd },
                {minusChar, opSubtract },
                {multiplyChar, opMultiply },
                {divideChar, opDivide },
                {powerChar, opPower }
            };
        }

        // Used to set up predefined variables and called from the parser to create new variables with a default Value.
        private void set(string name, double value = double.NaN)
        {
            if (!variables.ContainsKey(name))
            {
                variables.Add(name, new Variable(value, workStack));
            }
        }

        /// <summary>
        /// Sets the Value of a Variable in the expression.
        /// Variables not in the expression are ignored.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        public void Set(string name, double value)
        {
            if (variables.ContainsKey(name))
                variables[name].Value = value;
        }

        /// <summary>
        /// Gets the Value of a variable in the expression.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <exception cref="ArgumentException">No such variable</exception>
        public double Get(string name)
        {
            if (variables.ContainsKey(name))
                return variables[name].Value;
            else
                throw new ArgumentException($"No such variable '{name}'");
        }

        /// <summary>
        /// Evaluate the Expression.
        /// </summary>
        /// <returns>The value of the Expression.</returns>
        public double Evaluate()
        {
            // If parse has done its job correctly there will be no Exceptions and the stack will finish with the evaluation on top which will be popped and returned.
            // Division by zero will return an Infinity.
            // Other errors return NaN.
            for (int i = 0; i < tokens.Count; i++)
            {
                tokens[i].Execute();
            }
            return workStack.Pop();
        }

        // Read tokens until finished or error
        private void parse()
        {
            Token token;
            Operator cmdOp;
            // As we traverse the expression string we are in one of 2 states
            // Either it's legal for next Token to be a BinaryOperator or it isn't
            // Every Token is legal in only one of the modes hence the 2 methods that each read legal Tokens for the context we're in
            // If there's no legal token at the current position then syntax error
            bool expectBinOp = false;
            p = 0;
            while ((token = (expectBinOp ? ReadBinOpToken() : ReadUnaryOpToken())) != null)
            {
                if (token is Operator)
                {
                    if (token is LeftBracket)
                        operatorStack.Push(token as Operator);
                    else
                        PushOperator(token as Operator);
                    if (token is BinaryOperator)
                        expectBinOp = false;
                }
                else
                if (token is RightBracket)
                {
                    cmdOp = null;
                    while (operatorStack.Count > 0 && !((cmdOp = operatorStack.Pop()) is LeftBracket))
                        TokenAdd(cmdOp);
                    if (!(cmdOp is LeftBracket))
                        throw new ArgumentException($"Right bracket mismatch");
                }
                else
                {
                    tokens.Add(token);
                    expectBinOp = true;
                }
            }
            // Check that we end in the right parsing state
            if (!expectBinOp || p < ExpressionString.Length)
                throw new ArgumentException("Syntax error");
            // Pop remaining operators and check for unmatched left bracket
            while (operatorStack.Count > 0)
            {
                if ((cmdOp = operatorStack.Pop()) is LeftBracket)
                    throw new ArgumentException($"Missing close bracket(s)");
                else
                    TokenAdd(cmdOp);
            }
        }

        // Push operator onto the stack according to the shunting yard algorithm.
        private void PushOperator(Operator cmdOp)
        {
            if (cmdOp.Associativity == Associativity.Left || operatorStack.Count == 0 || cmdOp.Priority != operatorStack.Peek().Priority)
            {
                while (operatorStack.Count > 0 && cmdOp.Priority <= operatorStack.Peek().Priority)
                    TokenAdd(operatorStack.Pop());
            }
            operatorStack.Push(cmdOp);
        }

        // Optimize out sequences that evaluate to constants during the parse
        private void TokenAdd(Operator cmdOp)
        {
            if (cmdOp is Function && tokens[tokens.Count - 1] is Number)
            {
                tokens[tokens.Count - 1].Execute();
                tokens.RemoveAt(tokens.Count - 1);
                cmdOp.Execute();
                tokens.Add(new Number(workStack.Pop(), workStack));
            }
            else
            if (cmdOp is BinaryOperator && tokens[tokens.Count - 2] is Number && tokens[tokens.Count - 1] is Number)
            {
                tokens[tokens.Count - 2].Execute();
                tokens[tokens.Count - 1].Execute();
                tokens.RemoveAt(tokens.Count - 1);
                tokens.RemoveAt(tokens.Count - 1);
                cmdOp.Execute();
                tokens.Add(new Number(workStack.Pop(), workStack));
            }
            else
            // Ignore UnaryPlus
            if (!(cmdOp is UnaryPlus))
            {
                tokens.Add(cmdOp);
            }
        }

        // Left bracket, number, variable, function, unary +/-
        private Token ReadUnaryOpToken()
        {
            SkipSpaces(ExpressionString, ref p);
            if (p >= ExpressionString.Length)
                return null;
            char c = ExpressionString[p];
            if (c == leftBracketChar)
            {
                p++;
                return opLeftBracket;
            }
            if (IsStartOfNumber(c))
            {
                return new Number(ReadDouble(ExpressionString, ref p), workStack);
            }
            if (IsASCIILetter(c))
            {
                string letters = ReadLetters(ExpressionString, ref p);
                if (functions.ContainsKey(letters))
                    return functions[letters];
                set(letters);
                return variables[letters];
            }
            if (c == unaryMinusChar)
            {
                p++;
                return opUnaryMinus;
            }
            if (c == unaryPlusChar)
            {
                p++;
                return opUnaryPlus;
            }
            return null;
        }

        // +, -, *, /, ^, !, right bracket
        private Token ReadBinOpToken()
        {
            SkipSpaces(ExpressionString, ref p);
            if (p >= ExpressionString.Length)
                return null;
            char c = ExpressionString[p];
            if (IsBinOperator(c))
                return binaryOperators[ExpressionString[p++]];
            if (c == rightBracketChar)
            {
                p++;
                return opRightBracket;
            }
            if (c == factorialChar)
            {
                p++;
                return opFactorial;
            }
            return null;
        }

        #region Private static helper methods
        static bool IsASCIIDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        static bool IsStartOfNumber(char c)
        {
            return IsASCIIDigit(c) || c == '.';
        }

        static bool IsASCIILetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        static bool IsBinOperator(char c)
        {
            return binOpsString.IndexOf(c) >= 0;
        }

        static void SkipSpaces(string expression, ref int p)
        {
            while (p < expression.Length && expression[p] == ' ') p++;
        }

        // Returns a substring of ASCII letters starting from position p up until the next non-ASCII letter
        // p is passed by ref and on return points to the next non-ASCII letter char in the main string
        static string ReadLetters(string expression, ref int p)
        {
            int a = p;
            while (p < expression.Length && IsASCIILetter(expression[p])) p++;
            return expression.Substring(a, p - a);
        }

        // Parses a double from inside a string starting from position p
        // p is passed by ref and on return points to the character after the double read
        static double ReadDouble(string expression, ref int p)
        {
            int a = p;
            while (p < expression.Length && IsASCIIDigit(expression[p])) p++;
            if (p < expression.Length && expression[p] == '.')
            {
                p++;
                while (p < expression.Length && IsASCIIDigit(expression[p])) p++;
                if (p - a < 2)
                    throw new FormatException("Digit expected after the point");
            }
            if (p < expression.Length && char.ToUpper(expression[p]) == 'E')
            {
                p++;
                if (p < expression.Length && (expression[p] == '+' || expression[p] == '-'))
                    p++;
                while (p < expression.Length && IsASCIIDigit(expression[p])) p++;
            }
            return Double.Parse(expression.Substring(a, p - a));         // Possible exceptions including OverflowException, FormatException
        }
        #endregion
    }
}
