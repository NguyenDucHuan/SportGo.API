using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Repository
{
    public class SportGoDbContextFactory : IDesignTimeDbContextFactory<SportGoDbContext>
    {
        public SportGoDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SportGoDbContext>();
            // TODO: Replace with your actual connection string or load from config
            optionsBuilder.UseSqlServer("Server=(local);Database=SportGoDb;User Id=sa;Password=12345;Trusted_Connection=True;TrustServerCertificate=True");

            return new SportGoDbContext(optionsBuilder.Options);
        }
    }
}
