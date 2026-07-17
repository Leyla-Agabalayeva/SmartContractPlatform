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
    public class ContractRepository : Repository<Contract>, IContractRepository
    {
        public ContractRepository(AppDbContext context) : base(context) { }

        public async Task<List<Contract>> GetByUserAsync(string userId) =>
            await _dbSet
                .Include(c => c.Template)
                .Include(c => c.Parties)
                .Where(c => c.CreatedByUserId == userId
                            || c.Parties.Any(p => p.UserId == userId))
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

        public async Task<Contract?> GetFullAsync(int id) =>
            await _dbSet
                .Include(c => c.Template)
                .Include(c => c.CreatedByUser)
                .Include(c => c.Parties)
                .Include(c => c.Signatures)
                .Include(c => c.Document)
                .FirstOrDefaultAsync(c => c.Id == id);
    }

}
