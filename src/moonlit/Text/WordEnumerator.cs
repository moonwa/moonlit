using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Moonlit.Text
{
    public class CamelEnumerator : IEnumerable<string>
    {
        private readonly string _text;
        private readonly int _minWordLength;
        private readonly string[] _words;

        public CamelEnumerator(string text, int minWordLength, params string[] words)
        {
            _text = text;
            _minWordLength = minWordLength;
            _words = words;
        }

        public IEnumerator<string> GetEnumerator()
        {
            if (string.IsNullOrEmpty(_text))
            {
                yield break;
            }
            int start = 0, end = 0;
            int sum = 0;
            for (int i = 0; i < _text.Length; i++)
            {
                var c = _text[i];
                end = i;
                sum += Char.IsUpper(c) ? 1 : 0;
                var word = _text.Substring(start, end - start);
                if (_words.Any(x => x == word))
                {
                    yield return word;
                    start = end;
                    i = start - 1;
                    sum = 0;
                }
                if (end - start <= _minWordLength)
                {
                    continue;
                }
                // end - start + 1 : count of word
                // sum : count of upper char 

                int charCountOfWord = end - start + 1;
                // lg, lt, lg ..., sum = 1, count = 3
                if ((sum != charCountOfWord) && char.IsUpper(c))
                {
                    yield return _text.Substring(start, end - start);
                    start = end;
                    i = start - 1;
                    sum = 0;
                }
                // lg, lg, lg, lt, sum = 3, count = 4
                else if (sum + 1 == charCountOfWord && char.IsLower(c))
                {
                    yield return _text.Substring(start, end - start - 1);
                    start = end - 1;
                    i = start - 1;
                    sum = 0;
                }
            }
            yield return _text.Substring(start, _text.Length - start);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class WordEnumerator : IEnumerable<string>
    {
        private readonly string _text;
        private readonly List<WordSpliter> _spliters;

        public WordEnumerator(string text, char[] excludeSpliters = null, char[] includeSpliters = null)
        {
            _spliters = new List<WordSpliter>();

            _text = text;
            if (excludeSpliters != null)
                foreach (var spliter in excludeSpliters)
                {
                    _spliters.Add(new WordSpliter(spliter, false));
                }
            if (includeSpliters != null)
                foreach (var spliter in includeSpliters)
                {
                    _spliters.Add(new WordSpliter(spliter, true));
                }
        }

        public IEnumerator<string> GetEnumerator()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var c in _text)
            {
                var spliter = GetSpliter(c);
                if (spliter != null)
                {
                    string s = stringBuilder.ToString();
                    if (s != string.Empty)
                        yield return s;
                    stringBuilder.Clear();
                    if (spliter.Include)
                        yield return spliter.Character.ToString();
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }
            string result = stringBuilder.ToString();
            if (result != string.Empty)
                yield return result;
        }

        private WordSpliter GetSpliter(char c)
        {
            foreach (var spliter in _spliters)
            {
                if (spliter.Character == c) return spliter;
            }
            return null;
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void RemoveSpliter(char c)
        {
            var spliter = GetSpliter(c);
            if (spliter != null)
                _spliters.Remove(spliter);
        }
    }
}