using DoAnCoSo.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSo.Repositories
{
    public class EFPhuongTienVanChuyenRepository : IPhuongTienVanChuyenRepository
    {
        private readonly SeafoodDbContext _context;

        public EFPhuongTienVanChuyenRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PhuongTienVanChuyen>> GetAllAsync()
        {
            return await _context.PhuongTienVanChuyens.ToListAsync();
        }

        public async Task<PhuongTienVanChuyen?> GetByIdAsync(int id)
        {
            return await _context.PhuongTienVanChuyens.FindAsync(id);
        }

        public async Task AddAsync(PhuongTienVanChuyen entity)
        {
            _context.PhuongTienVanChuyens.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PhuongTienVanChuyen entity)
        {
            _context.PhuongTienVanChuyens.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.PhuongTienVanChuyens.FindAsync(id);
            if (entity != null)
            {
                _context.PhuongTienVanChuyens.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
