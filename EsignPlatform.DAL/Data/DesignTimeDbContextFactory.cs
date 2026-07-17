using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.DAL.Data
{
    // Add-Migration / Update-Database üçün design-time factory.
    // Startup layihəsi UI olmadan da işləyir.
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=Leyla-Comp;Database=SmartContractDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True")
                .Options;
            return new AppDbContext(options);
        }
    }

}
