using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Moonlit.Collections
{
    public class MatrixBuilder<TSource, TLocal> : IEnumerable<TLocal[]>
    {
        public delegate IEnumerable<TLocal> Iterator(TSource target, TLocal[] locals, ref bool continuation);

        private readonly Iterator _iterator;
        private readonly TLocal[] _locals;
        private readonly IEnumerable<IEnumerable<TSource>> _generations;

        public MatrixBuilder(
            IEnumerable<IEnumerable<TSource>> generations,
            Iterator iterator
            ) : this(generations, iterator, new TLocal[0])
        {
            _generations = generations;
            _iterator = iterator;
        }

        private MatrixBuilder(IEnumerable<IEnumerable<TSource>> generations, Iterator iterator, TLocal[] locals)
        {
            _generations = generations;
            _iterator = iterator;
            _locals = locals;
        }

        public IEnumerator<TLocal[]> GetEnumerator()
        {
            if (!_generations.Any())
            {
                if (this._locals.Any())
                {
                    yield return this._locals.ToArray();
                }
                yield break;
            }

            var generation = _generations.First();
            var nextGenerations = _generations.Skip(1);
            foreach (var item in generation)
            {
                var continuation = true;
                var locals = _locals;
                var newLocals = _iterator(item, locals, ref continuation);
                foreach (var newLocal in newLocals)
                {
                    var nextLocals = _locals.Concat(new[] { newLocal }).ToArray();
                    if (continuation)
                    {
                        var nextBuilder = new MatrixBuilder<TSource, TLocal>(nextGenerations, _iterator, nextLocals);
                        foreach (var childItems in nextBuilder)
                        {
                            yield return childItems;
                        }
                    }
                    else
                    {
                        yield return nextLocals;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}