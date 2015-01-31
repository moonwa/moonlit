using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// ����������
    /// </summary>
    public class Parser : IEnumerable<IParameterEntity>
    {
        /// <summary>
        /// ����Ŀ���Ƿ�����˳������ǣ��ڵ�һ�� target �����Ժ����е����ݾ���Ϊ target
        /// </summary>
        public bool OrderTarge { get; set; }
        /// <summary>
        /// �����б�
        /// </summary>
        private OptionArgumentEntityCollection Arguments = new OptionArgumentEntityCollection();

        /// <summary>
        /// Ŀ���б�
        /// </summary>
        public List<string> Targets { get; private set; }

        #region ctor

        /// <summary>
        /// ����һ������������
        /// </summary>
        public Parser()
        {
        }
        #endregion

        #region index
        /// <summary>
        /// ���ݳ���������ȡ����
        /// </summary>
        /// <param name="key">��������</param>
        /// <returns></returns>
        protected IParseEntity this[string key]
        {
            get
            {
                return this.Arguments.Find(key);
            }
        }
        /// <summary>
        /// ��ȡʵ��.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T GetEntity<T>(string key)
            where T : IParameterEntity
        {
            return (T)this[key];
        }
        #endregion

        #region privates
        private void Invalidate()
        {
            foreach (IParameterEntity arg in this.Arguments)
            {
                arg.Validate();
            }
        }
        #endregion

        #region Parse
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="argStrings">The arg strings.</param>
        public void Parse(string[] argStrings)
        {
            if (argStrings == null)
                return;
            try
            {
                TargetArgument target = new TargetArgument(this.OrderTarge);
                this.AddArguments(target);
                ParseEnumerator enumer = new ParseEnumerator(new List<string>(argStrings).GetEnumerator());
                if (enumer.MoveNext())
                {
                    this.Arguments.Parse(enumer);
                }
                this.Targets = new List<string>(target.Targets);
            }
            finally
            {
                this.Arguments.RemoveAt(this.Arguments.Count - 1);
            }
            this.Invalidate();
        }
        #endregion

        #region AddArguments
        /// <summary>
        /// ��Ӳ���Ԫ��.
        /// </summary>
        /// <param name="args">����ӵĲ���.</param>
        /// <returns></returns>
        public void AddArguments(params IParameterEntity[] args)
        {
            this.Arguments.AddRange(args);
        }
        #endregion

        #region IEnumerable<IArgumentEntity> Members

        public IEnumerator<IParameterEntity> GetEnumerator()
        {
            return this.Arguments.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Arguments.GetEnumerator();
        }

        #endregion

        #region Parse
        /// <summary>
        /// ���ַ�������Ϊ����
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string[] Parse(string s)
        {
            Match match = regex.Match(s);
            List<string> args = new List<string>();
            while (match.Success)
            {
                args.Add(match.Groups["g"].Value);
                match = match.NextMatch();
            }
            return args.ToArray();
        } 
        #endregion

        static readonly Regex regex = new Regex(@"(?>"")(?<g>[^""""]*)(?>"")|(?<g>[^\s""]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }
}

