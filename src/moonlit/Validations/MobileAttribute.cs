using System;
using System.ComponentModel.DataAnnotations;

namespace Moonlit.Validations
{ 
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class MobileAttribute : RegularExpressionAttribute
    {
        public const string Regex = "^1(3|5)\\d{9}$";
        public MobileAttribute()
            : base(Regex)
        {
            ErrorMessage = "有效电话号码为 130 ~ 139 或者 15 开头的11位数字";
        }
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class PhoneAttribute : RegularExpressionAttribute
    {
        public const string Regex = "^\\d{,100}$";
        public PhoneAttribute()
            : base(Regex)
        {
            ErrorMessage = "电话号码应该为数字";
        }
    }
}
