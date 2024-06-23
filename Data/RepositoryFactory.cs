using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class RepositoryFactory
    {
        public static IRepository<T> CreateRepository<T>() where T : BaseModel
        {
            var options = new DbContextOptionsBuilder<BookstoreDBContext>()
                .UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=Bookstore;Trusted_Connection=true;TrustServerCertificate=true")
                .Options;

            var context = new BookstoreDBContext(options);

            return new Repository<T>(context);
        }
    }
}
