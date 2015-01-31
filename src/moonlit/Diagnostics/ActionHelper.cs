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

        public static TimeSpan Elapsed<T>(this Action<T> action, T obj, int times, bool ignoreFirst)
        {
            if (ignoreFirst)
                action(obj);
            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                action(obj);
            }
            watch.Stop();
            return watch.Elapsed;
        }


        public static TimeSpan Elapsed<T1, T2>(this Action<T1, T2> action, T1 obj1, T2 obj2, int times, bool ignoreFirst)
        {
            if (ignoreFirst)
                action(obj1, obj2);
            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                action(obj1, obj2);
            }
            watch.Stop();
            return watch.Elapsed;
        }

        public static TimeSpan Elapsed<T1, T2, T3>(this Action<T1, T2, T3> action, T1 obj1, T2 obj2, T3 obj3, int times, bool ignoreFirst)
        {
            if (ignoreFirst)
                action(obj1, obj2, obj3);
            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                action(obj1, obj2, obj3);
            }
            watch.Stop();
            return watch.Elapsed;
        }

        public static TimeSpan Elapsed<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 obj1, T2 obj2, T3 obj3, T4 obj4, int times, bool ignoreFirst)
        {
            if (ignoreFirst)
                action(obj1, obj2, obj3, obj4);
            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < times; i++)
            {
                action(obj1, obj2, obj3, obj4);
            }
            watch.Stop();
            return watch.Elapsed;
        }
    }
}