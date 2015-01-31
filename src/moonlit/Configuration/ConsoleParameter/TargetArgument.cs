using System.Collections.Generic;

namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// 目标参数实体
    /// </summary>
    public class TargetArgument : Parameter
    {
        /// <summary>
        /// 
        /// </summary>
        class EmptyPrefix : PrefixEntity
        {
            public override string Key
            {
                get { return string.Empty; }
            }
            /// <summary>
            /// 解析输入参数
            /// </summary>
            /// <param name="enumer">包含参数的枚举，子类可自行调用 enumer.MoveNext()</param>
            /// <returns></returns>
            protected override bool OnParse(IParseEnumerator enumer)
            {
                return true;
            }
        }
        List<string> _targets = new List<string>();
        /// <summary>
        /// 获取目标.
        /// </summary>
        /// <value>The targets.</value>
        public List<string> Targets
        {
            get { return _targets; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TargetArgument"/> class.
        /// </summary>
        public TargetArgument(bool orderTarget)
            : base("", "", false, new EmptyPrefix())
        {
            this.OrderTarget = orderTarget;
        }
        /// <summary>
        /// 当前参数元素验证其值的正确性
        /// </summary>
        protected override void OnValidate()
        {
        }
        public bool OrderTarget { get; set; }
        /// <summary>
        /// 该参数是否使用过
        /// </summary>
        /// <value>如果使用过, 返回 <c>true</c> 否则返回 <c>false</c>.</value>
        public override bool Defined
        {
            get { return true; }
        }

        /// <summary>
        /// 解析输入参数
        /// </summary>
        /// <param name="enumer">包含参数的枚举，子类可自行调用 enumer.MoveNext()</param>
        /// <returns></returns>
        protected override bool OnParse(IParseEnumerator enumer)
        {
            _targets.Add(enumer.Current);
            enumer.MoveNext();
            if (this.OrderTarget && !enumer.End)        // 如果是信赖顺序的目标，则将后面所有值都视为目标值
            {
                do
                {
                    _targets.Add(enumer.Current);
                } while (enumer.MoveNext());
            }
            return true;
        }
    }
}
