using EsignPlatform.DAL.Data;
using EsignPlatform.DAL.Entities;
using EsignPlatform.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.DAL.Repositories
{

    public class TemplateRepository : Repository<Template>, ITemplateRepository
    {
        public TemplateRepository(AppDbContext context) : base(context) { }

        public async Task<List<Template>> GetActiveAsync() =>
            await _dbSet.Where(t => t.IsActive).OrderBy(t => t.Category).ToListAsync();
    }
}
