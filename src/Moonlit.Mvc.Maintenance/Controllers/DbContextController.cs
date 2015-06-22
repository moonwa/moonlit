using System;
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
    [Authorize(Roles = MaintModule.PrivilegeCulture, Order = 1000)]
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
                            var propertyName = column.Name[0].ToString().ToLower() + column.Name.Substring(1);
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
                        }
                        builder.AppendLine("    },");
                        builder.AppendLine(string.Format("    constructor: requireOptional('./{0}.ctor.js') || function(){{}},", tableType.Name.ToLower()));
                        builder.AppendLine(string.Format("    instanceActions: requireOptional('./{0}.instances.js') || {{}},", tableType.Name.ToLower()));

                        builder.AppendLine("}");
                        System.IO.File.WriteAllText(Path.Combine(zipSrcFolder, tableType.Name.ToLower() + ".model.js"), builder.ToString());
                    }
                }
                ZipFile.CreateFromDirectory(zipSrcFolder, zipFileName);
                return File(System.IO.File.ReadAllBytes(zipFileName), "application/zip", "dbcontext.zip");
            }
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }

        private void BuildNodeType(PropertyInfo property, StringBuilder builder)
        {
            if (typeof(string) == property.PropertyType)
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
            else if (typeof(DateTime) == property.PropertyType || typeof(DateTime?) == property.PropertyType)
            {
                builder.AppendLine(string.Format("            type: 'Date',"));
                if (typeof(DateTime) == property.PropertyType ||
                    property.GetCustomAttribute<RequiredAttribute>() != null)
                {
                    builder.AppendLine(string.Format("            required: true,"));
                }
            }
            else if (typeof(Boolean) == property.PropertyType || typeof(Boolean?) == property.PropertyType)
            {
                builder.AppendLine(string.Format("            type: 'Boolean',"));
                if (typeof(Boolean) == property.PropertyType ||
                    property.GetCustomAttribute<RequiredAttribute>() != null)
                {
                    builder.AppendLine(string.Format("            required: true,"));
                }
            }
            else if (typeof(int) == property.PropertyType
                || typeof(int?) == property.PropertyType
                || typeof(decimal) == property.PropertyType
                || typeof(decimal?) == property.PropertyType
                )
            {
                builder.AppendLine(string.Format("            type: 'Number',"));
                if (typeof(int) == property.PropertyType ||
                    typeof(decimal) == property.PropertyType ||
                    typeof(bool) == property.PropertyType ||
                    property.GetCustomAttribute<RequiredAttribute>() != null)
                {
                    builder.AppendLine(string.Format("            required: true,"));
                }
            }
            else if (property.PropertyType.ToWithoutNullableType().IsEnum)
            {
                builder.AppendLine(string.Format("            type: 'Number',"));
                if (!property.PropertyType.IsGenericType)
                {
                    builder.AppendLine(string.Format("            required: true,"));
                }
                //                builder.AppendLine(string.Format("        bounded: [{0}],", string.Join(",", Enum.GetNames(property.PropertyType).Select(x => "'" + x + "'"))));
            }
            else
            {
                throw new NotSupportedException("Type: " + property.PropertyType + " is not supported.");
            }

            builder.AppendLine(string.Format("            column: '{0}',", property.Name.ToLower()));

            if (string.Equals(property.Name, property.DeclaringType.Name + "id", StringComparison.OrdinalIgnoreCase))
            {
                builder.AppendLine(string.Format("            primaryKey: true,"));
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