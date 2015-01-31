using System.Collections.Generic;

namespace Moonlit.Arithmetic
{
    public class ConsecutiveAnalyzer
    {
        public IEnumerable<int[]> Analyze(int value)
        {
            List<int[]> results = new List<int[]>();
            for (int n = 2; n < value; n++)
            {
                int quotient = (value - (n + 1) * n / 2);
                if (quotient < 0) break;
                if (quotient % n == 0)
                {
                    List<int> ints = new List<int>();
                    var x = quotient / n;
                    for (int i = 0; i < n; i++)
                    {
                        ints.Add(x + i + 1);
                    }
                    results.Add(ints.ToArray());
                }
            }
            return results;
        }
    }
}
