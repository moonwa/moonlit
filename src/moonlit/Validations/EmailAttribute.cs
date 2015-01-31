using System;
using System.ComponentModel.DataAnnotations;

namespace Moonlit.Validations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            : base(@"^[a-z][\s\.]+@[\s\.]$")
        { 
        }
    }
}