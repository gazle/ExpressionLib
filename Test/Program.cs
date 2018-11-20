using System;
using ExpressionLib;
using System.Diagnostics;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            const int trials = 10000;
            Stopwatch timer = new Stopwatch();
            string expString;
            Expression exp;
            double result = 0;
            while ((expString = Console.ReadLine()) != string.Empty)
            {
                try
                {
                    exp = new Expression(expString);
                    exp.Set("a", 1);
                    exp.Set("b", 2);
                    exp.Set("c", 3);
                    exp.Set("d", 4);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                timer.Reset();
                timer.Start();
                for (int i = 0; i < trials; i++)
                {
                    result = exp.Evaluate();
                }
                timer.Stop();
                Console.WriteLine(result);
                Console.WriteLine("{0} calculations per second", trials / timer.Elapsed.TotalSeconds);
            }
        }
    }
}
