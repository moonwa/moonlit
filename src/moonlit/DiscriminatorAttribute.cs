using System;

namespace Moonlit
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class DiscriminatorAttribute : Attribute
    {
        private readonly string _discriminator;
        private readonly Type _discriminatorType;

        public DiscriminatorAttribute(string discriminator, Type discriminatorType)
        {
            _discriminator = discriminator;
            _discriminatorType = discriminatorType;
        }

        public Type DiscriminatorType
        {
            get { return _discriminatorType; }
        }

        public string Discriminator
        {
            get { return _discriminator; }
        }
    }
}