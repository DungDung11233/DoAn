using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class ChiTietMuaRepository : IChiTietMuaRepository
    {
        private readonly SeafoodDbContext _context;

        public ChiTietMuaRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChiTietMua>> GetAllAsync()
        {
            return await _context.ChiTietMuas
                                 .Include(c => c.SanPham)
                                 .Include(c => c.Mua)
                                 .ToListAsync();
        }

        public async Task<ChiTietMua> GetByIdAsync(int maSanPham, int maMua)
        {
            return await _context.ChiTietMuas
                                 .Include(c => c.SanPham)
                                 .Include(c => c.Mua)
                                 .FirstOrDefaultAsync(c => c.MaSanPhamID == maSanPham && c.MaMuaID == maMua);
        }

        public async Task AddAsync(ChiTietMua chiTietMua)
        {
            await _context.ChiTietMuas.AddAsync(chiTietMua);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChiTietMua chiTietMua)
        {
            _context.ChiTietMuas.Update(chiTietMua);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maSanPham, int maMua)
        {
            var chiTietMua = await _context.ChiTietMuas
                                            .FirstOrDefaultAsync(c => c.MaSanPhamID == maSanPham && c.MaMuaID == maMua);
            if (chiTietMua != null)
            {
                _context.ChiTietMuas.Remove(chiTietMua);
                await _context.SaveChangesAsync();
            }
        }
    }
}
