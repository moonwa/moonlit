using System;

namespace Moonlit.Mvc
{
    public class IeVersionCriteria : IHtmlElementCriteria
    {
        public IeVersionCriteria(int version, IeVersionCriteriaOperator @operator)
        {
            Version = version;
            Operator = @operator;
        }

        public IeVersionCriteriaOperator Operator { get; set; }
        public int Version { get; set; }

        public string BeginTag
        {
            get { return string.Format("<!--[if {0} IE {1}]><!-->", GetOperatorText(), Version); }
        }

        private string GetOperatorText()
        {
            switch (Operator)
            {
                case IeVersionCriteriaOperator.Lt:
                    return "lt";
                case IeVersionCriteriaOperator.Lte:
                    return "lte";
                case IeVersionCriteriaOperator.Gt:
                    return "gt";
                case IeVersionCriteriaOperator.Gte:
                    return "gte";
                case IeVersionCriteriaOperator.Eq:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string EndTag { get { return "<![endif]-->"; } }
    }
}