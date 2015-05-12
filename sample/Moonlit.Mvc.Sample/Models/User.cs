using System;
using Moonlit.Mvc.Sample.Controllers;

namespace Moonlit.Mvc.Sample.Models
{
    public class User
    {
        public string UserName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

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
        public static string ToDisplayString(this Gender  gender)
        {
            switch (gender)
            {
                case Gender.Male:
                    return "ÄÐ";
                case Gender.Female:
                    return "Å®";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    public enum Gender
    {
        Male,
        Female
    }
    public class UserListQueryModel
    {
        public string UserName { get; set; }
    }
}