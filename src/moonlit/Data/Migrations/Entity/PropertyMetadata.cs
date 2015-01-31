using System;
using System.Web.Script.Serialization;
using Moonlit.Data.Migrations;

namespace Moonlit.Data.Design.Entity
{
    [Serializable]
    public class PropertyMetadata
    {
        private Type _type;

        protected bool Equals(PropertyMetadata other)
        {
            return string.Equals(Name, other.Name) && string.Equals(TypeName, other.TypeName) && IsNullable.Equals(other.IsNullable) && IsKey.Equals(other.IsKey) && MaxLength == other.MaxLength;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TypeName != null ? TypeName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsNullable.GetHashCode();
                hashCode = (hashCode * 397) ^ IsKey.GetHashCode();
                hashCode = (hashCode * 397) ^ MaxLength;
                return hashCode;
            }
        }

        public string Name { get; set; }
        public string TypeName { get; set; }
        public bool IsNullable { get; set; }
        public bool IsKey { get; set; }
        public int MaxLength { get; set; }

        [ScriptIgnore]
        public Type Type
        {
            get { return _type; }
            set
            {
                _type = value;
                TypeName = PrimitiveTypeName.GetName(value);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PropertyMetadata)obj);
        }
    }
}