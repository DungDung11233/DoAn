using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class DonHangRepository : IDonHangRepository
    {
        private readonly SeafoodDbContext _context;

        public DonHangRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<DonHang> GetByIdAsync(int id)
        {
            return await _context.DonHangs
                .Include(dh => dh.KhachHang)
                .Include(dh => dh.PhuongThucThanhToan)
                .Include(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.SanPham)
                .Include(dh => dh.ChiTietTrangThaiDonHangs)
                .ThenInclude(ct => ct.TrangThaiDonHang)
                .FirstOrDefaultAsync(dh => dh.MaDonHang == id);
        }

        public async Task<IEnumerable<DonHang>> GetAllAsync()
        {
            return await _context.DonHangs
                .Include(dh => dh.KhachHang)
                .Include(dh => dh.PhuongThucThanhToan)
                .Include(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.SanPham)
                .Include(dh => dh.ChiTietTrangThaiDonHangs)
                .ThenInclude(ct => ct.TrangThaiDonHang)
                .ToListAsync();
        }

        public async Task AddAsync(DonHang donHang)
        {
            _context.DonHangs.Add(donHang);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DonHang donHang)
        {
            _context.DonHangs.Update(donHang);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang != null)
            {
                _context.DonHangs.Remove(donHang);
                await _context.SaveChangesAsync();
            }
        }
    }
}
