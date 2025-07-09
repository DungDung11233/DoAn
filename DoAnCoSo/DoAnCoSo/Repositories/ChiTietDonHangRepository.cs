using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class ChiTietDonHangRepository : IChiTietDonHangRepository
    {
        private readonly SeafoodDbContext _context;

        public ChiTietDonHangRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChiTietDonHang>> GetAllAsync()
        {
            return await _context.ChiTietDonHangs
                                 .Include(c => c.SanPham)
                                 .Include(c => c.DonHang)
                                 .ToListAsync();
        }

        public async Task<ChiTietDonHang> GetByIdAsync(int maSanPham, int maDonHang)
        {
            return await _context.ChiTietDonHangs
                                 .Include(c => c.SanPham)
                                 .Include(c => c.DonHang)
                                 .FirstOrDefaultAsync(c => c.MaSanPhamID == maSanPham && c.MaDonHangID == maDonHang);
        }

        public async Task AddAsync(ChiTietDonHang chiTietDonHang)
        {
            await _context.ChiTietDonHangs.AddAsync(chiTietDonHang);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChiTietDonHang chiTietDonHang)
        {
            _context.ChiTietDonHangs.Update(chiTietDonHang);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maSanPham, int maDonHang)
        {
            var chiTietDonHang = await _context.ChiTietDonHangs
                                                .FirstOrDefaultAsync(c => c.MaSanPhamID == maSanPham && c.MaDonHangID == maDonHang);
            if (chiTietDonHang != null)
            {
                _context.ChiTietDonHangs.Remove(chiTietDonHang);
                await _context.SaveChangesAsync();
            }
        }
    }
}
