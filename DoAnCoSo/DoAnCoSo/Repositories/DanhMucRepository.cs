using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAnCoSo.Models;

namespace DoAnCoSo.Repositories
{
    public class DanhMucRepository : IDanhMucRepository
    {
        private readonly SeafoodDbContext _context;

        public DanhMucRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DanhMuc>> GetAllAsync()
        {
            return await _context.DanhMucs                               
                                 .Include(dm => dm.SanPhams)
                                 .ToListAsync();
        }

        public async Task<DanhMuc> GetByIdAsync(int maDanhMuc)
        {
            return await _context.DanhMucs
                                 .Include(dm => dm.SanPhams)
                                 .FirstOrDefaultAsync(dm => dm.MaDanhMuc == maDanhMuc);
        }

        public async Task AddAsync(DanhMuc danhMuc)
        {
            await _context.DanhMucs.AddAsync(danhMuc);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DanhMuc danhMuc)
        {
            _context.DanhMucs.Update(danhMuc);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maDanhMuc)
        {
            var danhMuc = await _context.DanhMucs.FirstOrDefaultAsync(dm => dm.MaDanhMuc == maDanhMuc);
            if (danhMuc != null)
            {
                _context.DanhMucs.Remove(danhMuc);
                await _context.SaveChangesAsync();
            }
        }
    }
}
