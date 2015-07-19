using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [MoonlitAuthorize(Roles = MaintModule.PrivilegeCulture)]
    public class DbContextController : MaintControllerBase
    {
        [RequestMapping("dbcontexts", "devtools/dbcontext")]
        [SitemapNode(Parent = "DevTools", Text = "DevToolsDbContextList", ResourceType = typeof(CultureTextResources))]
        public ActionResult Index(DbContextListModel model)
        {
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
        [RequestMapping("dbcontexts_export_nodejs", "devtools/dbcontext")]
        [FormAction("dbcontexts_export_nodejs")]
        [ActionName("Index")]
        [HttpPost]
        public ActionResult ExportNode(DbContextListModel model, string[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                var zipSrcFolder = HttpContext.Server.MapPath("~/tmp/" + new Random().Next(100000, 999999).ToString());
                var zipFileName = zipSrcFolder + ".zip";

                Directory.CreateDirectory(zipSrcFolder);
                Directory.CreateDirectory(Path.Combine(zipSrcFolder, "models"));
                Directory.CreateDirectory(Path.Combine(zipSrcFolder, "boundeds"));
                List<Type> boundedTypes = new List<Type>();
                foreach (var id in ids)
                {
                    var dbContextType = Type.GetType(id);
                    var tables = dbContextType.GetProperties()
                        .Where(x =>
                            x.PropertyType.IsGenericType &&
                            x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

                    foreach (var table in tables)
                    {
                        var tableType = table.PropertyType.GetGenericArguments()[0];
                        StringBuilder builder = new StringBuilder();
                        builder.AppendLine("var uuid = require('uuid');");
                        builder.AppendLine("var requireOptional = require('require-optional');");
                        builder.AppendLine("module.exports = {");
                        builder.AppendLine("    tableName: '" + table.Name.ToLower() + "',");
                        builder.AppendLine("    name: '" + tableType.Name + "',");
                        builder.AppendLine("    schema: {");
                        var columns = tableType.GetProperties().Where(x => x.GetCustomAttribute<DbContextExportAttribute>() == null || !x.GetCustomAttribute<DbContextExportAttribute>().Ignore).ToList();
                        for (int i = 0; i < columns.Count; i++)
                        {
                            var column = columns[i];
                            var attr = column.GetCustomAttribute<DbContextExportAttribute>();
                            var propertyName = attr != null && !string.IsNullOrEmpty(attr.Name) ? GetFieldName(attr.Name) : GetFieldName(column.Name);
                            builder.AppendLine(string.Format("        {0}: {{", propertyName));
                            BuildNodeType(column, builder);
                            if (i == columns.Count - 1)
                            {
                                builder.AppendLine("        }");
                            }
                            else
                            {
                                builder.AppendLine("        },");
                            }

                            var propType = column.PropertyType.ToWithoutNullableType();
                            if (propType.IsEnum)
                            {
                                if (!boundedTypes.Contains(propType))
                                {
                                    boundedTypes.Add(propType);
                                }
                            }
                        }
                        builder.AppendLine("    },");
                        builder.AppendLine(string.Format("    constructor: requireOptional('./{0}.ctor.js') || function(){{}},", tableType.Name.ToLower()));
                        builder.AppendLine(string.Format("    instanceActions: requireOptional('./{0}.instances.js') || {{}},", tableType.Name.ToLower()));

                        builder.AppendLine("}");
                        System.IO.File.WriteAllText(Path.Combine(zipSrcFolder, "models", tableType.Name.ToLower() + ".model.js"), builder.ToString());
                    }
                }
                foreach (var boundedType in boundedTypes)
                {
                    var text = BuildBoundedType(boundedType);
                    System.IO.File.WriteAllText(Path.Combine(zipSrcFolder, "boundeds", boundedType.Name.ToLower() + ".js"), text);
                }
                ZipFile.CreateFromDirectory(zipSrcFolder, zipFileName);
                return File(System.IO.File.ReadAllBytes(zipFileName), "application/zip", "dbcontext.zip");
            }
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }

        private string BuildBoundedType(Type boundedType)
        {
            boundedType = boundedType.ToWithoutNullableType();
            StringBuilder buffer = new StringBuilder();
            buffer.Append("exports = module.exports = {");
            string[] fields = Enum.GetNames(boundedType);
            Array values = Enum.GetValues(boundedType);
            for (int i = 0; i < fields.Length; i++)
            {
                string field = fields[i];
                buffer.AppendFormat("    \"{0}\" : {1},", GetFieldName(field), (int)values.GetValue(i));
                buffer.AppendLine();
            }
            buffer.Append("    _items: [");
            for (int i = 0; i < fields.Length; i++)
            {
                string field = fields[i];
                buffer.AppendLine("        {");
                buffer.AppendLine(string.Format("            \"text\" : \"${{{0}.{1}}}\",", boundedType.Name, field));
                buffer.AppendLine(string.Format("            \"name\" : \"{0}\",", GetFieldName(field)));
                buffer.AppendLine(string.Format("            \"value\" : {0},", (int)values.GetValue(i)));
                buffer.AppendLine(string.Format("            \"sort\" : {0}", i));
                buffer.Append("        }");
                if (i != fields.Length - 1)
                {
                    buffer.Append(",");
                }
                buffer.AppendLine();
            }
            buffer.Append("    ]");

            buffer.Append("}");
            return buffer.ToString();
        }

        string GetFieldName(string s)
        {
            return s[0].ToString().ToLower() + s.Substring(1);
        }
        private void BuildNodeType(PropertyInfo property, StringBuilder builder)
        {
            var attr = property.GetCustomAttribute<DbContextExportAttribute>();
            var propertyType = property.PropertyType;
            if (attr != null)
            {
                propertyType = attr.ExportAsType ?? propertyType;
            }

            if (typeof(string) == propertyType)
            {
                builder.AppendLine(string.Format("            type: 'String',"));
                var stringLength = property.GetCustomAttribute<StringLengthAttribute>();
                if (stringLength != null)
                {
                    builder.AppendLine(string.Format("            maxlength: " + stringLength.MaximumLength + ","));
                    if (stringLength.MinimumLength > 0)
                    {
                        builder.AppendLine(string.Format("            minlength: " + stringLength.MinimumLength + ","));
                    }
                }
                else
                {
                    builder.AppendLine(string.Format("            maxlength: " + 50 + ","));
                }
                if (property.GetCustomAttribute<RequiredAttribute>() != null)
                {
                    builder.AppendLine(string.Format("            required: true,"));
                }
                var exgularAttr = property.GetCustomAttribute<RegularExpressionAttribute>();
                if (exgularAttr != null)
                {
                    var defaultRegularError = string.Format("{0}.{1}.regular", property.DeclaringType.Name, property.Name);
                    builder.AppendLine(string.Format("            regex: {{ expr: '{0}', msg: '${{{1}}}'}},", exgularAttr.Pattern.Replace(@"\", @"\\"), exgularAttr.ErrorMessageResourceName ?? defaultRegularError));
                }
            }
            else if (typeof(DateTime) == propertyType || typeof(DateTime?) == propertyType)
            {
                builder.AppendLine(string.Format("            type: 'Date',"));
                if (typeof(DateTime) == propertyType ||
                    property.GetCustomAttribute<RequiredAttribute>() != null)
                {
                    builder.AppendLine(string.Format("            required: true,"));
                }
            }
            else if (typeof(Boolean) == propertyType || typeof(Boolean?) == propertyType)
            {
                builder.AppendLine(string.Format("            type: 'Boolean',"));
                if (typeof(Boolean) == propertyType ||
                    property.GetCustomAttribute<RequiredAttribute>() != null)
                {
                    builder.AppendLine(string.Format("            required: true,"));
                }
            }
            else if (typeof(int) == propertyType
                || typeof(int?) == propertyType
                || typeof(decimal) == propertyType
                || typeof(decimal?) == propertyType
                )
            {
                builder.AppendLine(string.Format("            type: 'Number',"));
                if (typeof(int) == propertyType ||
                    typeof(decimal) == propertyType ||
                    typeof(bool) == propertyType ||
                    property.GetCustomAttribute<RequiredAttribute>() != null)
                {
                    builder.AppendLine(string.Format("            required: true,"));
                }
            }
            else if (propertyType.ToWithoutNullableType().IsEnum)
            {
                builder.AppendLine(string.Format("            type: 'Number',"));
                if (!propertyType.IsGenericType)
                {
                    builder.AppendLine(string.Format("            required: true,"));
                }
                //                builder.AppendLine(string.Format("        bounded: [{0}],", string.Join(",", Enum.GetNames(property.PropertyType).Select(x => "'" + x + "'"))));
            }
            else
            {
                throw new NotSupportedException("Type: " + propertyType + " is not supported.");
            }
            var columnName = property.Name;
            if (attr != null && !string.IsNullOrEmpty(attr.Name))
            {
                columnName = attr.Name;
            }
            builder.AppendLine(string.Format("            column: '{0}',", columnName.ToLower()));

            if (string.Equals(property.Name, property.DeclaringType.Name + "id", StringComparison.OrdinalIgnoreCase))
            {
                builder.AppendLine(string.Format("            primaryKey: true,"));

                if (typeof(int) == propertyType)
                {
                    builder.AppendLine(string.Format("            autoIncrement: true,"));
                }
            }
            if (property.GetCustomAttribute<DisplayAttribute>() != null)
            {
                var uiculture = Thread.CurrentThread.CurrentUICulture;
                var culture = Thread.CurrentThread.CurrentCulture;
                try
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                    builder.AppendLine(string.Format("            label: '${{{0}}}'", property.GetCustomAttribute<DisplayAttribute>().GetName()));
                }
                finally
                {

                    Thread.CurrentThread.CurrentUICulture = uiculture;
                    Thread.CurrentThread.CurrentCulture = culture;
                }
            }
            else
            {
                builder.AppendLine(string.Format("            label: '${{{0}.{1}}}'", property.DeclaringType.Name, property.Name));
            }
        }
    }
}