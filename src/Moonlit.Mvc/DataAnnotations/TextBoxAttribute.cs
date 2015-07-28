using System;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LiteralAttribute : ControlAttribute
    {
        public override Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext)
        {
            return new Literal
            {
                Name = metadata.PropertyName,
                Text = model == null ? "" : model.ToString(),
            };
        }
    } 
    [AttributeUsage(AttributeTargets.Property)]
    public class MultiLineTextBoxAttribute : ControlAttribute
    {
        public string PlaceHolder { get; set; }
        public int? MaxLength { get; set; }
        public override Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext)
        {
            return new MultiLineTextBox
            {
                Name = metadata.PropertyName,
                Value = model == null ? "" : model.ToString(),
                Enabled = !metadata.IsReadOnly,
                PlaceHolder = PlaceHolder,
                MaxLength = MaxLength
            };
        }
    } 
    [AttributeUsage(AttributeTargets.Property)]
    public class TextBoxAttribute : ControlAttribute
    {
        public string Icon { get; set; }
        public string PlaceHolder { get; set; }
        public override Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext)
        {
            return new TextBox
            {
                Enabled = !metadata.IsReadOnly,
                Name = metadata.PropertyName,
                Icon = Icon,
                Value = model == null ? "" : model.ToString(),
                PlaceHolder = this.PlaceHolder,
            };
        }
    } 
    [AttributeUsage(AttributeTargets.Property)]
    public class CheckBoxAttribute : ControlAttribute
    {
        public override Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext)
        {
            return new CheckBox 
            {
                Checked = model != null && Convert.ToBoolean(model),
                Enabled = !metadata.IsReadOnly,
                Name = metadata.PropertyName,
                Text = metadata.DisplayName,
                Value = "true",
            };
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class DatePickerAttribute : ControlAttribute
    {
        public override Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext)
        {
            return new DatePicker 
            {
                Name = metadata.PropertyName,
                Value = model == null ? (DateTime?) null : Convert.ToDateTime(model)
            };
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class PasswordBoxAttribute : ControlAttribute
    {
        public string Icon { get; set; }
        public string PlaceHolder { get; set; }

        public override Control CreateControl(ModelMetadata metadata, object model, ControllerContext controllerContext)
        {
            return new PasswordBox
            {
                Enabled = !metadata.IsReadOnly,
                Icon = Icon,
                PlaceHolder = PlaceHolder,
                Name = metadata.PropertyName,
            };
        }
    }
}