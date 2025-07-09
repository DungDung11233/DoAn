using System;
using DoAnCoSo.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSo.Repositories
{
    public class EFChiTietTrangThaiDonHangRepository : IChiTietTrangThaiDonHangRepository
    {
        private readonly SeafoodDbContext _context;

        public EFChiTietTrangThaiDonHangRepository(SeafoodDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChiTietTrangThaiDonHang>> GetAllAsync()
        {
            return await _context.ChiTietTrangThaiDonHangs
                .Include(c => c.DonHang)
                .Include(c => c.TrangThaiDonHang)
                .ToListAsync();
        }

        public async Task<ChiTietTrangThaiDonHang> GetByIdAsync(int maDonHang, int maTrangThai)
        {
            return await _context.ChiTietTrangThaiDonHangs
                .Include(c => c.DonHang)
                .Include(c => c.TrangThaiDonHang)
                .FirstOrDefaultAsync(c => c.MaDonHang == maDonHang && c.MaTrangThai == maTrangThai);
        }

        public async Task AddAsync(ChiTietTrangThaiDonHang entity)
        {
            _context.ChiTietTrangThaiDonHangs.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChiTietTrangThaiDonHang entity)
        {
            _context.ChiTietTrangThaiDonHangs.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int maDonHang, int maTrangThai)
        {
            var entity = await GetByIdAsync(maDonHang, maTrangThai);
            if (entity != null)
            {
                _context.ChiTietTrangThaiDonHangs.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }

}
