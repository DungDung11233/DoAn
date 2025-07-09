using Microsoft.EntityFrameworkCore;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class ChiTietPhieuNhapRepository : IChiTietPhieuNhapRepository
    {
        private readonly SeafoodDbContext _context;

        public ChiTietPhieuNhapRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChiTietPhieuNhap>> GetAllAsync()
        {
            return await _context.ChiTietPhieuNhaps
                                 .Include(c => c.PhieuNhapHang)
                                 .Include(c => c.SanPham)
                                 .ToListAsync();
        }

        public async Task<ChiTietPhieuNhap?> GetByIdAsync(int maPhieuNhap, int maSanPham)
        {
            return await _context.ChiTietPhieuNhaps
                                 .Include(c => c.PhieuNhapHang)
                                 .Include(c => c.SanPham)
                                 .FirstOrDefaultAsync(c => c.MaPhieuNhap == maPhieuNhap && c.MaSanPham == maSanPham);
        }

        public async Task AddAsync(ChiTietPhieuNhap entity)
        {
            await _context.ChiTietPhieuNhaps.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChiTietPhieuNhap entity)
        {
            _context.ChiTietPhieuNhaps.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maPhieuNhap, int maSanPham)
        {
            var entity = await _context.ChiTietPhieuNhaps
                .FirstOrDefaultAsync(c => c.MaPhieuNhap == maPhieuNhap && c.MaSanPham == maSanPham);
            if (entity != null)
            {
                _context.ChiTietPhieuNhaps.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
