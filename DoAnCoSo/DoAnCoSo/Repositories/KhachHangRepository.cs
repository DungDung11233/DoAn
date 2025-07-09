using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class KhachHangRepository : IKhachHangRepository
    {
        private readonly SeafoodDbContext _context;

        public KhachHangRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KhachHang>> GetAllAsync()
        {
            return await _context.KhachHangs
                                 .Include(n => n.DonHangs)
                                 .Include(n => n.DanhGias)
                                 .ToListAsync();
        }

        public async Task<KhachHang> GetByIdAsync(int maNguoiDung)
        {
            return await _context.KhachHangs
                                 .Include(n => n.DonHangs)
                                 .Include(n => n.DanhGias)
                                 .FirstOrDefaultAsync(n => n.MaKhachHang == maNguoiDung);
        }

        public async Task AddAsync(KhachHang nguoiDung)
        {
            await _context.KhachHangs.AddAsync(nguoiDung);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(KhachHang nguoiDung)
        {
            _context.KhachHangs.Update(nguoiDung);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maNguoiDung)
        {
            var nguoiDung = await _context.KhachHangs.FindAsync(maNguoiDung);
            if (nguoiDung != null)
            {
                _context.KhachHangs.Remove(nguoiDung);
                await _context.SaveChangesAsync();
            }
        }
    }
}
