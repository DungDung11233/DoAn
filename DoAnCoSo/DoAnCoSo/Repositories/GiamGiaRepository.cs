using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class GiamGiaRepository : IGiamGiaRepository
    {
        private readonly SeafoodDbContext _context;

        public GiamGiaRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GiamGia>> GetAllAsync()
        {
            return await _context.GiamGias
                                 .Include(g => g.DonHangs)
                                 .ToListAsync();
        }

        public async Task<GiamGia> GetByIdAsync(int maGiamGia)
        {
            return await _context.GiamGias
                                 .Include(g => g.DonHangs)
                                 .FirstOrDefaultAsync(g => g.MaGiamGia == maGiamGia);
        }

        public async Task AddAsync(GiamGia giamGia)
        {
            await _context.GiamGias.AddAsync(giamGia);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GiamGia giamGia)
        {
            _context.GiamGias.Update(giamGia);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maGiamGia)
        {
            var giamGia = await _context.GiamGias.FindAsync(maGiamGia);
            if (giamGia != null)
            {
                _context.GiamGias.Remove(giamGia);
                await _context.SaveChangesAsync();
            }
        }
    }
}
