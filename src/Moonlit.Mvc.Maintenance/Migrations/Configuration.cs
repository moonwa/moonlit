using System.Data.Entity.Migrations;
using System.Linq;
using System.Transactions;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Models;

namespace Moonlit.Mvc.Maintenance.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MaintDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            //            AutomaticMigrationDataLossAllowed = true;
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
        }
        protected override void Seed(MaintDbContext context)
        {

            SiteModel model = new SiteModel(context.SystemSettings);
            if (model.DBVersion == SiteModel.VersionFirst)
            {
                using (var tran = new TransactionScope())
                {
                    var adminUser = new User()
                    {
                        LoginName = "admin",
                        IsSuper = true,
                        IsEnabled = true,
                        IsBuildIn = true,
                    };

                    adminUser.Password = adminUser.HashPassword("123456");
                    context.Users.Add(adminUser);

                    //var defaultCulture = new Culture()
                    //{
                    //    DisplayName = "¼òÌåÖÐÎÄ",
                    //    IsEnabled = true,
                    //    Name = "zh-cn",
                    //};

                    //context.Cultures.Add(defaultCulture);
                    //context.SaveChanges();

                    //model.DefaultCulture = defaultCulture.CultureId;
                    //model.DBVersion = "0.2";
                    //model.Save(new MaintDbContextMaintDbRepository(context));


                    //context.SaveChanges();
                    tran.Complete();
                }
            }
        }
    }



}
