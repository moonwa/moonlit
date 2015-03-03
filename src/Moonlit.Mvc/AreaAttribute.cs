using System;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed class AreaAttribute : Attribute
    {
        readonly string _area;
    
        // This is a positional argument
        public AreaAttribute (string area) 
        { 
            this._area = area;
        
        }

        public string Area
        {
            get { return _area; }
        }
   
        // This is a named argument
        public int NamedInt { get; set; } 
    }
}