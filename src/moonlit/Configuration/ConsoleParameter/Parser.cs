using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// 解析参数类
    /// </summary>
    public class Parser : IEnumerable<IParameterEntity>
    {
        /// <summary>
        /// 参数目标是否信赖顺序，如果是，在第一个 target 出现以后所有的内容均视为 target
        /// </summary>
        public bool OrderTarge { get; set; }
        /// <summary>
        /// 参数列表
        /// </summary>
        private OptionArgumentEntityCollection Arguments = new OptionArgumentEntityCollection();

        /// <summary>
        /// 目标列表
        /// </summary>
        public List<string> Targets { get; private set; }

        #region ctor

        /// <summary>
        /// 构造一个参数解析器
        /// </summary>
        public Parser()
        {
        }
        #endregion

        #region index
        /// <summary>
        /// 根据长参数名获取参数
        /// </summary>
        /// <param name="key">长参数名</param>
        /// <returns></returns>
        protected IParseEntity this[string key]
        {
            get
            {
                return this.Arguments.Find(key);
            }
        }
        /// <summary>
        /// 获取实体.
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
        /// 解析参数
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
        /// 添加参数元素.
        /// </summary>
        /// <param name="args">被添加的参数.</param>
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
        /// 将字符串解析为参数
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

