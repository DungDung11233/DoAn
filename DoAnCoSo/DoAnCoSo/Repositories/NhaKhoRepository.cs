using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class NhaKhoRepository : INhaKhoRepository
    {
        private readonly SeafoodDbContext _context;

        public NhaKhoRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NhaKho>> GetAllAsync()
        {
            return await _context.NhaKhos
                                 .Include(n => n.ChiTietKhoHangs)
                                 .ToListAsync();
        }

        public async Task<NhaKho> GetByIdAsync(int maNhaKho)
        {
            return await _context.NhaKhos
                                 .Include(n => n.ChiTietKhoHangs)
                                 .FirstOrDefaultAsync(n => n.MaNhaKho == maNhaKho);
        }

        public async Task AddAsync(NhaKho nhaKho)
        {
            await _context.NhaKhos.AddAsync(nhaKho);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NhaKho nhaKho)
        {
            _context.NhaKhos.Update(nhaKho);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maNhaKho)
        {
            var nhaKho = await _context.NhaKhos.FindAsync(maNhaKho);
            if (nhaKho != null)
            {
                _context.NhaKhos.Remove(nhaKho);
                await _context.SaveChangesAsync();
            }
        }
    }
}
