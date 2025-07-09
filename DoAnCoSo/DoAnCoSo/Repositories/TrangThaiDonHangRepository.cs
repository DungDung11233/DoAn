using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class TrangThaiDonHangRepository : ITrangThaiDonHangRepository
    {
        private readonly SeafoodDbContext _context;

        public TrangThaiDonHangRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TrangThaiDonHang>> GetAllAsync()
        {
            return await _context.TrangThaiDonHangs
                                 .Include(t => t.ChiTietTrangThaiDonHangs)
                                 .ToListAsync();
        }

        public async Task<TrangThaiDonHang?> GetByIdAsync(int maTrangThai)
        {
            return await _context.TrangThaiDonHangs
                                 .Include(t => t.ChiTietTrangThaiDonHangs)
                                 .FirstOrDefaultAsync(t => t.MaTrangThai == maTrangThai);
        }

        public async Task AddAsync(TrangThaiDonHang trangThaiDonHang)
        {
            await _context.TrangThaiDonHangs.AddAsync(trangThaiDonHang);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TrangThaiDonHang trangThaiDonHang)
        {
            _context.TrangThaiDonHangs.Update(trangThaiDonHang);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maTrangThai)
        {
            var trangThaiDonHang = await _context.TrangThaiDonHangs.FindAsync(maTrangThai);
            if (trangThaiDonHang != null)
            {
                _context.TrangThaiDonHangs.Remove(trangThaiDonHang);
                await _context.SaveChangesAsync();
            }
        }
    }
}
