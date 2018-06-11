using DataAcess;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace DataAccess.IntegrationTests
{
    public class DatabaseInitialiser
    {
        [Fact]
        public void Test1()
        {
            string connStringTemplate = "Server=(localdb)\\mssqllocaldb;Database={tenant};Trusted_Connection=True;MultipleActiveResultSets=true";

            // setup TenantA first;
            string tenantName = "TenantA";
            var dbContextOptionsBuilder = new DbContextOptionsBuilder();
            dbContextOptionsBuilder.UseSqlServer(connStringTemplate.Replace("{tenant}", tenantName));
            CRMContext contextA = new CRMContext(dbContextOptionsBuilder.Options);

            //for (int i = 0; i < 5; i++)
            //{
            //    contextA.Customers.Remove(contextA.Customers.FirstOrDefault());
            //}

            //contextA.SaveChanges();

            for (int i = 0; i < 5; i++)
            {
                contextA.Customers.Add(new DataAcess.DomainModel.Customer() { CustomerId = Guid.NewGuid(), Name = $"TenA:XYZ Inc {i + 1}", RegistrationDate = DateTime.Today, Website = $"http://www.{$"XYZ Inc {i + 1}"}.com" });
            }

            contextA.SaveChanges();

            // setup TenantB next;
            tenantName = "TenantB";
            dbContextOptionsBuilder = new DbContextOptionsBuilder();
            dbContextOptionsBuilder.UseSqlServer(connStringTemplate.Replace("{tenant}", tenantName));
            CRMContext contextB = new CRMContext(dbContextOptionsBuilder.Options);

            //for (int i = 0; i < 10; i++)
            //{
            //    contextB.Customers.Remove(contextB.Customers.FirstOrDefault());
            //}

            //contextB.SaveChanges();

            for (int i = 0; i < 10; i++)
            {
                contextB.Customers.Add(new DataAcess.DomainModel.Customer() { CustomerId = Guid.NewGuid(), Name = $"TenB: Super Awesome Co {i + 1}", RegistrationDate = DateTime.Today, Website = $"http://www.{$"Super Awesome Co {i + 1}"}.com" });
            }

            contextB.SaveChanges();
        }
    }
}