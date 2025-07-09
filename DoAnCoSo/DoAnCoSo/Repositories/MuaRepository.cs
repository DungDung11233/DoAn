using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class MuaRepository : IMuaRepository
    {
        private readonly SeafoodDbContext _context;

        public MuaRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Mua>> GetAllAsync()
        {
            return await _context.Muas
                                 .Include(m => m.ChiTietMuas)
                                 .ToListAsync();
        }

        public async Task<Mua> GetByIdAsync(int maMua)
        {
            return await _context.Muas
                                 .Include(m => m.ChiTietMuas)
                                 .FirstOrDefaultAsync(m => m.MaMua == maMua);
        }

        public async Task AddAsync(Mua mua)
        {
            await _context.Muas.AddAsync(mua);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Mua mua)
        {
            _context.Muas.Update(mua);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maMua)
        {
            var mua = await _context.Muas.FindAsync(maMua);
            if (mua != null)
            {
                _context.Muas.Remove(mua);
                await _context.SaveChangesAsync();
            }
        }
    }
}
