using AltaSoft.Notifications.DAL;
using AltaSoft.Notifications.DAL.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Database.SetInitializer(new DropCreateDatabaseAlways<MainDbContext>());

                using (var db = new MainDbContext())
                {
                    Console.WriteLine("Location: {0}, Database: {1}", db.Database.Connection.DataSource, db.Database.Connection.Database);


                    // Test Providers
                    db.Providers.Add(new Provider { Key = "sendgrid", Name = "SendGrid", RegDate = DateTime.Now });
                    db.Providers.Add(new Provider { Key = "sms", Name = "SMS", RegDate = DateTime.Now });
                    db.Providers.Add(new Provider { Key = "signalr", Name = "SignalR", RegDate = DateTime.Now });
                    db.Providers.Add(new Provider { Key = "email", Name = "Smtp Mail", RegDate = DateTime.Now });

                    // Test Applications
                    var application = db.Applications.Add(new Application { SecretKey = "TEST", Username = "EZ", Password = "123", RegDate = DateTime.Now, EmailFromAddress = "notofication@altasoft.ge", EmailFromFullName = "Notification System", Name = "Test Project", Description = "For testing purposes only", IsTestMode = true });

                    // Test Users
                    db.Users.Add(new User { FirstName = "Test", FullName = "Test Tester", ExternalUserId = 1, MobileNumber = "995593159115", Email = "e.zibzibadze@altasoft.ge", ApplicationId = application.Id, RegDate = DateTime.Now });


                    db.SaveChanges();


                    Console.WriteLine();
                    Console.WriteLine("Database Generated - Successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error Message:");
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine("Error Details:");
                Console.WriteLine(ex.ToString());
            }

            Console.ReadKey();
        }
    }
}
