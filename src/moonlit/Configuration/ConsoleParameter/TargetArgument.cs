using System.Collections.Generic;

namespace Moonlit.Configuration.ConsoleParameter
{
    /// <summary>
    /// Ŀ�����ʵ��
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
            /// �����������
            /// </summary>
            /// <param name="enumer">����������ö�٣���������е��� enumer.MoveNext()</param>
            /// <returns></returns>
            protected override bool OnParse(IParseEnumerator enumer)
            {
                return true;
            }
        }
        List<string> _targets = new List<string>();
        /// <summary>
        /// ��ȡĿ��.
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
        /// ��ǰ����Ԫ����֤��ֵ����ȷ��
        /// </summary>
        protected override void OnValidate()
        {
        }
        public bool OrderTarget { get; set; }
        /// <summary>
        /// �ò����Ƿ�ʹ�ù�
        /// </summary>
        /// <value>���ʹ�ù�, ���� <c>true</c> ���򷵻� <c>false</c>.</value>
        public override bool Defined
        {
            get { return true; }
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="enumer">����������ö�٣���������е��� enumer.MoveNext()</param>
        /// <returns></returns>
        protected override bool OnParse(IParseEnumerator enumer)
        {
            _targets.Add(enumer.Current);
            enumer.MoveNext();
            if (this.OrderTarget && !enumer.End)        // ���������˳���Ŀ�꣬�򽫺�������ֵ����ΪĿ��ֵ
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
