using System;
using System.Globalization;
using System.Reflection;

namespace Moonlit
{
    public class LocalizableString
    {
        // Fields
        private Func<string> _cachedResult;
        private string _propertyName;
        private string _propertyValue;
        private Type _resourceType;

        public LocalizableString(string propertyName)
        {
            this._propertyName = propertyName;
        }

        private void ClearCache()
        {
            this._cachedResult = null;
        }

        public string GetLocalizableValue()
        {
            if (this._cachedResult == null)
            {
                if ((this._propertyValue == null) || (this._resourceType == null))
                {
                    this._cachedResult = () => this._propertyValue;
                }
                else
                {
                    PropertyInfo property = this._resourceType.GetProperty(this._propertyValue);
                    bool noFoundPropertyInResource = false;
                    if ((!this._resourceType.IsVisible || (property == null)) || (property.PropertyType != typeof(string)))
                    {
                        noFoundPropertyInResource = true;
                    }
                    else
                    {
                        MethodInfo getMethod = property.GetGetMethod();
                        if (((getMethod == null) || !getMethod.IsPublic) || !getMethod.IsStatic)
                        {
                            noFoundPropertyInResource = true;
                        }
                    }
                    if (noFoundPropertyInResource)
                    {
                        string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "Nonfound Property {0} of Type {1}, the Resource Property is {2}", new object[] { this._propertyName, this._resourceType.FullName, this._propertyValue });
                        this._cachedResult = delegate
                                                 {
                                                     throw new InvalidOperationException(exceptionMessage);
                                                 };
                    }
                    else
                    {
                        this._cachedResult = () => (string)property.GetValue(null, null);
                    }
                }
            }
            return this._cachedResult();
        }

        // Properties
        public Type ResourceType
        {
            get
            {
                return this._resourceType;
            }
            set
            {
                if (this._resourceType != value)
                {
                    this.ClearCache();
                    this._resourceType = value;
                }
            }
        }

        public string Value
        {
            get
            {
                return this._propertyValue;
            }
            set
            {
                if (this._propertyValue != value)
                {
                    this.ClearCache();
                    this._propertyValue = value;
                }
            }
        }
    }
}