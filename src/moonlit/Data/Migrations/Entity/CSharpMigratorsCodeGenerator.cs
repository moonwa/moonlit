using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Moonlit.Data.Migrations;
using Moonlit.Data.Migrations.Migrators;
using Moonlit.Text;

namespace Moonlit.Data.Design.Entity
{
    internal class CSharpMigratorsCodeGenerator : MigratorsCodeGenerator
    {
        public CSharpMigratorsCodeGenerator(IEnumerable<DbMigrator> migrators, string target)
            : base(migrators, target)
        {
        }

        public override string Generate(string className, string @namespace, Version version)
        {
            var buffer = new StringBuilder();
            var indentWriter = new IndentWriter(new StringWriter(buffer), 4);
            indentWriter.WriteLine("namespace {0} {{", @namespace);
            using (indentWriter.BeginIndent())
            {
                indentWriter.WriteLine("using System;");
                indentWriter.WriteLine("using System.Data.Entity;");
                indentWriter.WriteLine("public class " + className + " : Moonlit.Data.Migrations.DbMigration {");
                using (indentWriter.BeginIndent())
                {
                    indentWriter.WriteLine("private Version _version = new Version(\"{0}\");", version);
                    indentWriter.WriteLine("public override Version Version { get { return _version; } }");
                    indentWriter.WriteLine("public override string Target {{ get {{ return @\"{0}\"; }} }}",
                                           Target.Replace("\"", "\"\""));
                    indentWriter.WriteLine("public override void Up() {");
                    using (indentWriter.BeginIndent())
                    {
                        foreach (DbMigrator dbMigrator in Migrators)
                        {
                            Write(dbMigrator as CreateTableDbMigrator, indentWriter);
                            Write(dbMigrator as RenameTableDbMigrator, indentWriter);
                            Write(dbMigrator as DropTableDbMigrator, indentWriter);
                            Write(dbMigrator as AddColumnDbMigrator, indentWriter);
                            Write(dbMigrator as DropColumnDbMigrator, indentWriter);
                            Write(dbMigrator as AddPrimaryKeyDbMigrator, indentWriter);
                            Write(dbMigrator as DropPrimaryKeyDbMigrator, indentWriter);
                            Write(dbMigrator as AlterColumnDbMigrator, indentWriter);
                            Write(dbMigrator as RenameColumnDbMigrator, indentWriter);
                            Write(dbMigrator as CreateIndexDbMigrator, indentWriter);
                            Write(dbMigrator as DropIndexDbMigrator, indentWriter);
                            Write(dbMigrator as SqlDbMigrator, indentWriter);
                        }
                    }
                    indentWriter.WriteLine("}");
                }
                indentWriter.WriteLine("}");
            }
            indentWriter.WriteLine("}");
            indentWriter.Flush();
            return buffer.ToString();
        }

        private void Write(SqlDbMigrator migrator, IndentWriter indentWriter)
        {
            if (migrator == null) return;
            indentWriter.WriteLine("Sql(\"{0}\");", migrator.Sql);
        }

        private void Write(DropIndexDbMigrator migrator, IndentWriter indentWriter)
        {
            if (migrator == null) return;
            var columns = string.Join(",", migrator.Columns.Select(x => "\"" + x + "\""));
            indentWriter.WriteLine("DropIndex(\"{0}\", new []{{{1}}});", migrator.TableName, columns);
        }

        private void Write(CreateIndexDbMigrator migrator, IndentWriter indentWriter)
        {
            if (migrator == null) return;
            var columns = string.Join(",", migrator.Columns.Select(x => "\"" + x + "\""));
            indentWriter.WriteLine("CreateIndex(\"{0}\", new []{{{1}}}, unique:{2});", migrator.TableName, columns, ToString(migrator.Unique));
        }

        private void Write(RenameTableDbMigrator migrator, IndentWriter indentWriter)
        {
            if (migrator == null) return;
            indentWriter.WriteLine("RenameTable(\"{0}\", \"{1}\");", migrator.TableName, migrator.NewTableName);
        }

        private void Write(RenameColumnDbMigrator migrator, IndentWriter indentWriter)
        {
            if (migrator == null) return;
            indentWriter.WriteLine("RenameColumn(\"{0}\", \"{1}\", \"{2}\");", migrator.TableName, migrator.ColumnName, migrator.NewColumnName);
        }

        private void Write(AlterColumnDbMigrator migrator, IndentWriter indentWriter)
        {
            if (migrator == null) return;
            indentWriter.WriteLine("AlterColumn(\"{0}\", \"{1}\", x=> {2});", migrator.TableName, migrator.ColumnName, ToString(migrator.Column, "x"));
        }

        private void Write(DropPrimaryKeyDbMigrator migrator, IndentWriter indentWriter)
        {
            if (migrator == null) return;
            indentWriter.WriteLine("DropPrimaryKey(\"{0}\");", migrator.TableName);
        }

        private void Write(AddPrimaryKeyDbMigrator migrator, IndentWriter indentWriter)
        {
            if (migrator == null) return;
            indentWriter.WriteLine("AddPrimaryKey(\"{0}\", \"{1}\");", migrator.TableName, migrator.ColumnName);
        }

        private void Write(DropColumnDbMigrator migrator, IndentWriter indentWriter)
        {
            if (migrator == null) return;
            indentWriter.WriteLine("DropColumn(\"{0}\", \"{1}\");", migrator.TableName, migrator.ColumnName);
        }

        private void Write(AddColumnDbMigrator migrator, IndentWriter indentWriter)
        {
            if (migrator == null) return;
            indentWriter.WriteLine("AddColumn(\"{0}\", \"{1}\", x=> {2});", migrator.TableName, migrator.ColumnName, ToString(migrator.Column, "x"));
        }

        private void Write(DropTableDbMigrator migrator, IndentWriter indentWriter)
        {
            if (migrator == null) return;
            indentWriter.WriteLine("DropTable(\"{0}\" );", migrator.TableName);
        }

        private void Write(CreateTableDbMigrator migrator, IndentWriter indentWriter)
        {
            if (migrator == null) return;

            indentWriter.WriteLine("CreateTable(\"{0}\", x => new {{", migrator.TableName);
            using (indentWriter.BeginIndent())
            {
                foreach (ColumnModel column in migrator.ColumnModels)
                {
                    indentWriter.WriteLine("{0} = {1},", column.Name, ToString(column, "x"));
                }
            }
            indentWriter.WriteLine("});");
        }

        private object ToString(bool b)
        {
            return b.ToString().ToLower();
        }

        private object ToString(bool? b)
        {
            return b.HasValue ? ToString(b.Value) : "null";
        }

        private object ToString(int? b)
        {
            return b.HasValue ? b.Value.ToString() : "null";
        }

        private object ToString(ColumnModel property, string builderName)
        {
            var builder = new ColumnBuilder();
            switch (property.StoreType)
            {
                case PrimitiveTypeName.TypeBoolean:
                    // return builder.Boolean(nullable: property.Nullable, name: property.Name);
                    return string.Format("{0}.Boolean(nullable: {1})"
                                         , builderName
                                         , ToString(property.Nullable));
                case PrimitiveTypeName.TypeInt:
                    //return builder.Int(nullable: property.Nullable, identity: property.Identity, name: property.Name);
                    return string.Format("{0}.Int(nullable: {1}, identity: {2})"
                                         , builderName
                                         , ToString(property.Nullable)
                                         , ToString(property.Identity));
                case PrimitiveTypeName.TypeString:
                    //builder.String(nullable: property.Nullable, maxLength: property.MaxLength,
                    //               fixedLength: property.FixedLength, isMaxLength: property.IsMaxLength, name: property.Name);
                    return
                        string.Format(
                            "{0}.String(nullable: {1}, maxLength: {2}, fixedLength: {3}, isMaxLength: {4})",
                            builderName,
                            ToString(property.Nullable),
                            ToString(property.MaxLength),
                            ToString(property.FixedLength),
                            ToString(property.IsMaxLength)
                            );
                case PrimitiveTypeName.TypeDateTime:
                    //builder.DateTime(nullable: property.Nullable, name: property.Name); 
                    return string.Format("{0}.DateTime(nullable: {1})",
                                         builderName,
                                         ToString(property.Nullable)
                        );
                case PrimitiveTypeName.TypeDecimal:
                    builder.Decimal(nullable: property.Nullable, precision: property.Precision, name: property.Name);
                    return string.Format("{0}.Decimal(nullable: {1}, precision:{2})",
                                         builderName,
                                         ToString(property.Nullable),
                                         ToString(property.Precision)
                        );
                default:
                    return string.Empty;
            } 
        }
    }
}