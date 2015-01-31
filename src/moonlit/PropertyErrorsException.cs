using System;
using System.Runtime.Serialization;

namespace Moonlit
{
    [Serializable]
    public class PropertyErrorsException : Exception
    {
        private readonly PropertyErrors _propertyErrors;

        public PropertyErrorsException(string propertyName, params string[] errors):this(new PropertyError(propertyName, errors))
        {
            
        }
        protected PropertyErrorsException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
            this._propertyErrors = (PropertyErrors)info.GetValue("_propertyErrors", typeof(PropertyErrors));
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_propertyErrors", _propertyErrors);
            base.GetObjectData(info, context);
        }
        public PropertyErrorsException(PropertyErrors propertyErrors)
        {
            _propertyErrors = propertyErrors;
        }
        public PropertyErrorsException(PropertyError propertyError)
        {
            _propertyErrors = new PropertyErrors();
            _propertyErrors.Add(propertyError);
        }

        public PropertyErrorsException(PropertyErrors propertyErrors, Exception inner)
            : base("", inner)
        {
            _propertyErrors = propertyErrors;
        }
        public PropertyErrorsException(PropertyError propertyError, Exception inner)
            : base("", inner)
        {
            _propertyErrors = new PropertyErrors();
            _propertyErrors.Add(propertyError);
        }

        public PropertyErrors Errors
        {
            get { return _propertyErrors; }
        }
    }

}