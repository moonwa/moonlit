using System;

namespace Moonlit.Mvc.Sample.Models
{
    public static class ModelHelper
    {
        public static string ToDisplayString(this Gender? gender)
        {
            if (gender == null)
            {
                return string.Empty;
            }
            return gender.Value.ToDisplayString();
        }
        public static string ToDisplayString(this Gender gender)
        {
            switch (gender)
            {
                case Gender.Male:
                    return "��";
                case Gender.Female:
                    return "Ů";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}