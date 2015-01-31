using System;

namespace Moonlit.ObjectConverts
{
    [Serializable]
    public class FieldNotFoundException : Exception
    {
        public FieldNotFoundException(string property)
            : base(string.Format("Property '" + property + "' nonfound."))
        {
        }
    }
}