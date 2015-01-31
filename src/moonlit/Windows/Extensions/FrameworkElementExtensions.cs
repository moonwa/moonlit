using System.Collections.Generic;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Windows
{
    public static class FrameworkElementExtensions
    {
        public static IEnumerable<DependencyProperty> GetDependencyProperties(this FrameworkElement element)
        {
            var dependencyProperties = new List<DependencyProperty>();

            Type type = element.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(DependencyProperty))
                {
                    dependencyProperties.Add((DependencyProperty)field.GetValue(null));
                }
            }

            return dependencyProperties;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "obj")]
        public static bool AreHandlersSuspended(this DependencyObject obj)
        {
            return _areHandlersSuspended;
        }

        public static bool ContainsFocusedElement(this FrameworkElement element)
        {
            if (element != null)
            {
                DependencyObject focusedDependencyObject = FocusManager.GetFocusedElement() as DependencyObject;
                while (focusedDependencyObject != null)
                {
                    if (focusedDependencyObject == element)
                    {
                        return true;
                    }

                    // Walk up the visual tree.  If we hit the root, try using the framework element's
                    // parent.  We do this because Popups behave differently with respect to the visual tree,
                    // and it could have a parent even if the VisualTreeHelper doesn't find it.
                    DependencyObject parent = VisualTreeHelper.GetParent(focusedDependencyObject);
                    if (parent == null)
                    {
                        FrameworkElement focusedElement = focusedDependencyObject as FrameworkElement;
                        if (focusedElement != null)
                        {
                            parent = focusedElement.Parent;
                        }
                    }
                    focusedDependencyObject = parent;
                }
            }
            return false;
        }
        public static Binding CloneBinding(Binding source)
        {
            Binding clone = new Binding();

            if (source == null)
            {
                return clone;
            }

            clone.Converter = source.Converter;
            clone.ConverterCulture = source.ConverterCulture;
            clone.ConverterParameter = source.ConverterParameter;
            clone.Mode = source.Mode;
            clone.NotifyOnValidationError = source.NotifyOnValidationError;
            clone.Path = source.Path;
            clone.UpdateSourceTrigger = source.UpdateSourceTrigger;
            clone.ValidatesOnExceptions = source.ValidatesOnExceptions;

            // Binding keeps track of which of the three setters for
            // ElementName, RelativeSource, and Source have been called.
            // Calling any two of the setters, even if the value passed in is null,
            // will raise an exception.  For that reason, we must check for null
            // for these properties to ensure that we only call the setter when we should.
            if (source.ElementName != null)
            {
                clone.ElementName = source.ElementName;
            }
            else if (source.RelativeSource != null)
            {
                clone.RelativeSource = source.RelativeSource;
            }
            else if (source.Source != null)
            {
                clone.Source = source.Source;
            }

            return clone;
        }
        private static bool _areHandlersSuspended;
        public static void SetValueNoCallback(this DependencyObject obj, DependencyProperty property, object value)
        {
            _areHandlersSuspended = true;
            try
            {
                obj.SetValue(property, value);
            }
            finally
            {
                _areHandlersSuspended = false;
            }
        }
    }
}