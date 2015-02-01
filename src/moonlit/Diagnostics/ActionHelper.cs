using System;
using System.Diagnostics;

namespace Moonlit.Diagnostics
{
    public static class ActionHelper
    {
        public static TimeSpan Elapsed(this Action action, int times, bool ignoreFirst)
        {
            if (ignoreFirst)
                action();

            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                action();
            }
            watch.Stop();
            return watch.Elapsed;
        } 
    }
}