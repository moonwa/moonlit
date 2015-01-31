using System;
using System.Collections.Generic;
using System.Linq;

namespace Moonlit.Configuration.ConsoleParameter
{
    public class EnumParameter : Parameter
    {
        private string _argumentDescription = string.Empty;
        private Dictionary<object, string> _description = new Dictionary<object, string>();
        private readonly Type _enumType;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumParameter{T}"/> class.
        /// </summary>
        /// <param name="name">参数名称.</param>
        /// <param name="enumType"></param>
        /// <param name="defaultValue">缺省值.</param>
        /// <param name="prefixs">命令前缀</param>
        /// <param name="description">参数描述</param>
        public EnumParameter(string name, Type enumType, object defaultValue, string description = "",
                             params PrefixEntity[] prefixs)
            : base(name, string.IsNullOrWhiteSpace(description) ? "" : description, false, CreatePrefixEntity(name, prefixs))
        {
            DefaultValue = defaultValue;
            ValueString = defaultValue.ToString();
            _enumType = enumType;
            _description = new Dictionary<object, string>();
            _argumentDescription = description;
        }
         

        /// <summary>
        /// 获取或设置缺省值.
        /// </summary>
        /// <value>The default value.</value>
        public object DefaultValue { private get; set; }

        /// <summary>
        /// 获取或设置所解析的结果值.
        /// </summary>
        /// <value>The value.</value>
        public object Value
        {
            get { return Enum.Parse(_enumType, ValueString, true); }
        }

        private string ValueString { get; set; }

        /// <summary>
        /// 返回当前参数元素的描述
        /// </summary>
        /// <value>描述.</value>
        public override List<string> Description
        {
            get
            {
                var result = new List<string>();
                result.Add(_argumentDescription);
                int len = 0;
                foreach (var item in _description)
                {
                    if (len < item.Key.ToString().Length)
                    {
                        len = item.Key.ToString().Length;
                    }
                }
                foreach (var item in _description)
                {
                    if (item.Key.Equals(DefaultValue))
                    {
                        result.Add(string.Format("{0,-" + len + "} (default){1}", item.Key, item.Value));
                    }
                    else
                    {
                        result.Add(string.Format("{0,-" + len + "}          {1}", item.Key, item.Value));
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 该参数是否使用过
        /// </summary>
        /// <value>如果使用过, 返回 <c>true</c> 否则返回 <c>false</c>.</value>
        public override bool Defined
        {
            get { return true; }
        }

        private static PrefixEntity[] CreatePrefixEntity(string name, PrefixEntity[] prefixs)
        {
            if (prefixs == null || prefixs.Length == 0)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("当没有使用参数时，名称不可为空");
                }
                return new PrefixEntity[] { (LongOrSplitPrefix)name, (ShortPrefix)name[0] };
            }
            return prefixs;
        }
         
        /// <summary>
        /// 当前参数元素验证其值的正确性
        /// </summary>
        protected override void OnValidate()
        {
            string[] names = Enum.GetNames(_enumType);
            IEnumerable<string> matchNames = from name in names
                                             where string.Compare(name, ValueString, true) == 0
                                             select name;

            if (matchNames.Count() == 0)
            {
                throw new ArgumentOutOfRangeException(Name, ValueString, "输入类型错误");
            }
        }

        /// <summary>
        /// 解析输入参数
        /// </summary>
        /// <param name="enumer">包含参数的枚举，子类可自行调用 enumer.MoveNext()</param>
        /// <returns></returns>
        protected override bool OnParse(IParseEnumerator enumer)
        {
            ValueString = enumer.Current;
            enumer.MoveNext();
            return true;
        }
    }

    /// <summary>
    /// 定义包含枚举的参数, 定义格式如 /x=a, --f=xx, /x:a, --f:x
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumParameter<T> : EnumParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumParameter{T}"/> class.
        /// </summary>
        /// <param name="name">参数名称.</param>
        /// <param name="defaultValue">缺省值.</param>
        /// <param name="prefixs">命令前缀</param>
        public EnumParameter(string name, T defaultValue,
                             params PrefixEntity[] prefixs )
            : this(name, defaultValue, null, prefixs)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumParameter{T}"/> class.
        /// </summary>
        /// <param name="name">参数名称.</param>
        /// <param name="defaultValue">缺省值.</param>
        /// <param name="prefixs">命令前缀</param>
        /// <param name="description">参数描述</param>
        private EnumParameter(string name, T defaultValue, string description = "",
                             params PrefixEntity[] prefixs )
            : base(name, typeof(T), defaultValue, description, prefixs)
        {
        }
    }
}