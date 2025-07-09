using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class PhuongThucThanhToanRepository : IPhuongThucThanhToanRepository
    {
        private readonly SeafoodDbContext _context;

        public PhuongThucThanhToanRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PhuongThucThanhToan>> GetAllAsync()
        {
            return await _context.PhuongThucThanhToans
                                 .ToListAsync();
        }

        public async Task<PhuongThucThanhToan> GetByIdAsync(int maPTTT)
        {
            return await _context.PhuongThucThanhToans
                                 .FirstOrDefaultAsync(p => p.MaPTTT == maPTTT);
        }

        public async Task AddAsync(PhuongThucThanhToan phuongThucThanhToan)
        {
            await _context.PhuongThucThanhToans.AddAsync(phuongThucThanhToan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PhuongThucThanhToan phuongThucThanhToan)
        {
            _context.PhuongThucThanhToans.Update(phuongThucThanhToan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maPTTT)
        {
            var phuongThucThanhToan = await _context.PhuongThucThanhToans.FindAsync(maPTTT);
            if (phuongThucThanhToan != null)
            {
                _context.PhuongThucThanhToans.Remove(phuongThucThanhToan);
                await _context.SaveChangesAsync();
            }
        }
    }
}
