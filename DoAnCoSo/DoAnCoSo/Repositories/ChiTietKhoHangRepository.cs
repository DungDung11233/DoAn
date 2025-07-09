using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class ChiTietKhoHangRepository : IChiTietKhoHangRepository
    {
        private readonly SeafoodDbContext _context;

        public ChiTietKhoHangRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChiTietKhoHang>> GetAllAsync()
        {
            return await _context.ChiTietKhoHangs
                                 .Include(c => c.SanPham)
                                 .Include(c => c.NhaKho)
                                 .ToListAsync();
        }

        public async Task<ChiTietKhoHang> GetByIdAsync(int maSanPham, int maNhaKho)
        {
            return await _context.ChiTietKhoHangs
                                 .Include(c => c.SanPham)
                                 .Include(c => c.NhaKho)
                                 .FirstOrDefaultAsync(c => c.MaSanPhamID == maSanPham && c.MaNhaKhoID == maNhaKho);
        }

        public async Task AddAsync(ChiTietKhoHang chiTietKhoHang)
        {
            await _context.ChiTietKhoHangs.AddAsync(chiTietKhoHang);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChiTietKhoHang chiTietKhoHang)
        {
            _context.ChiTietKhoHangs.Update(chiTietKhoHang);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maSanPham, int maNhaKho)
        {
            var chiTietKhoHang = await _context.ChiTietKhoHangs
                                                .FirstOrDefaultAsync(c => c.MaSanPhamID == maSanPham && c.MaNhaKhoID == maNhaKho);
            if (chiTietKhoHang != null)
            {
                _context.ChiTietKhoHangs.Remove(chiTietKhoHang);
                await _context.SaveChangesAsync();
            }
        }
    }
}
