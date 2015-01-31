using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonlit.Data.Migrations.Migrators;

namespace Moonlit.Data.Migrations
{
    public class MigrateDatabaseInitializer<T> : IDatabaseInitializer<T> where T : DbContext
    {
        private readonly string _name;

        public MigrateDatabaseInitializer()
        {

        }
        public MigrateDatabaseInitializer(string name)
        {
            _name = name;
        }

        public void InitializeDatabase(T context)
        {
            var moduleMigration = GetModuleMigration(context);
            var migrationTypes = typeof(T).Assembly.GetTypes().Where(x => typeof(DbMigration).IsAssignableFrom(x));
            List<DbMigration> migrations = new List<DbMigration>();
            foreach (var migrationType in migrationTypes)
            {
                DbMigration dbMigration = (DbMigration)Activator.CreateInstance(migrationType);
                migrations.Add(dbMigration);

            }
            foreach (var dbMigration in migrations.Where(x => x.Version > moduleMigration.Version).OrderBy(x => x.Version))
            {
                try
                {
                    dbMigration.Up();
                    moduleMigration.Version = dbMigration.Version;
                    try
                    {
                        context.Database.Connection.Open();
                        using (var trans = context.Database.Connection.BeginTransaction())
                        {
                            foreach (var migrator in dbMigration.Migrators)
                            {
                                var sqlStatement = migrator.CreateStatement();
                                var command = context.Database.Connection.CreateCommand();

                                command.Connection = context.Database.Connection;
                                if (!sqlStatement.SuppressTransaction)
                                    command.Transaction = trans;
                                command.CommandText = sqlStatement.Sql;
                                command.ExecuteNonQuery();

                            }
                            var cmd = context.Database.Connection.CreateCommand();
                            cmd.Transaction = trans;
                            cmd.CommandText = "delete from __moduleMigration where name = '" + moduleMigration.Name + "'";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = string.Format("insert __moduleMigration(name, version, lastUpdate) " +
                                                             "values('{0}', '{1}', '{2}')",
                                                             moduleMigration.Name,
                                                             moduleMigration.Version,
                                                             DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.ExecuteNonQuery();
                            trans.Commit();
                        }

                    }
                    finally
                    {
                        context.Database.Connection.Close();
                    }
                }
                finally
                {
                    dbMigration.Clean();
                }
            }
        }

        private ModuleMigration GetModuleMigration(T context)
        {
            ModuleMigration moduleMigration = new ModuleMigration();
            moduleMigration.Version = new Version(0, 0, 0, 0);
            var connection = context.Database.Connection;

            try
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "select count(*) from sysobjects where xtype='U' and Name = '__moduleMigration'";
                cmd.Connection = connection;
                var count = (int)cmd.ExecuteScalar();
                if (count == 0)
                {
                    cmd.CommandText = @"CREATE TABLE [dbo].[__moduleMigration](
	[Name] [nvarchar](200) NOT NULL,
	[Version] [nvarchar](50) NOT NULL,
	[LastUpdate] [datetime] NOT NULL
) ON [PRIMARY] ";
                    cmd.ExecuteNonQuery();
                }
                cmd.CommandText = "select * from __moduleMigration";
                var reader = cmd.ExecuteReader();
                var name = _name ?? context.GetType().Assembly.GetName().Name;
                moduleMigration.Name = name;
                while (reader.Read())
                {
                    if (name.EqualsIgnoreCase(reader["name"] as string))
                    {
                        moduleMigration.Version = new Version(reader["Version"].ToString());
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return moduleMigration;
        }
    }
}
