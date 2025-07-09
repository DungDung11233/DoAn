using System;
using System.Linq;
using System.Threading.Tasks;
using DoAnCoSo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DoAnCoSo.Repositories
{
    public class VanChuyenService : IVanChuyenService
    {
        private readonly SeafoodDbContext _context;
        private readonly IVanChuyenRepository _vanChuyenRepository;
        private readonly IChiTietTrangThaiDonHangRepository _chiTietTrangThaiDonHangRepository;
        private readonly IDonHangRepository _donHangRepository;

        public VanChuyenService(
            SeafoodDbContext context,
            IVanChuyenRepository vanChuyenRepository,
            IChiTietTrangThaiDonHangRepository chiTietTrangThaiDonHangRepository,
            IDonHangRepository donHangRepository)
        {
            _context = context;
            _vanChuyenRepository = vanChuyenRepository;
            _chiTietTrangThaiDonHangRepository = chiTietTrangThaiDonHangRepository;
            _donHangRepository = donHangRepository;
        }

        public async Task<bool> ProcessShippingOrderAsync(List<int> maDonHangs, int maPhuongTien, int maNhanVien)
        {
            if (maDonHangs == null || !maDonHangs.Any())
            {
                return false;
            }

            // Kiểm tra tính hợp lệ của maPhuongTien
            var phuongTien = await _context.Set<PhuongTienVanChuyen>().FindAsync(maPhuongTien);
            if (phuongTien == null)
            {
                return false;
            }

            // Kiểm tra tính hợp lệ của maNhanVien
            var nhanVien = await _context.Set<NhanVien>().FindAsync(maNhanVien);
            if (nhanVien == null)
            {
                return false;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Kiểm tra tất cả đơn hàng có tồn tại không và có đang trong trạng thái phù hợp không
                var donHangs = await _context.DonHangs
                    .Include(d => d.ChiTietTrangThaiDonHangs)
                    .Where(d => maDonHangs.Contains(d.MaDonHang))
                    .ToListAsync();

                if (donHangs.Count != maDonHangs.Count)
                {
                    await transaction.RollbackAsync();
                    return false; // Có đơn hàng không tồn tại
                }

                // Kiểm tra xem có đơn hàng nào đã được vận chuyển chưa
                foreach (var donHang in donHangs)
                {
                    var trangThaiHienTai = donHang.ChiTietTrangThaiDonHangs
                        .OrderByDescending(c => c.NgayCapNhat)
                        .FirstOrDefault();

                    if (trangThaiHienTai != null && trangThaiHienTai.MaTrangThai >= 5)
                    {
                        await transaction.RollbackAsync();
                        return false; // Có đơn hàng đã được vận chuyển
                    }
                }

                // Tạo phiếu vận chuyển
                var phieuVanChuyen = new PhieuVanChuyen
                {
                    MaPhuongTienID = maPhuongTien,
                    MaNhanVienID = maNhanVien,
                    NgayVanChuyen = DateTime.Now
                };

                await _context.PhieuVanChuyens.AddAsync(phieuVanChuyen);
                await _context.SaveChangesAsync(); // Lưu để có MaVanChuyen

                // Tạo chi tiết vận chuyển cho từng đơn hàng
                foreach (var maDonHang in maDonHangs)
                {
                    var chiTietVanChuyen = new ChiTietVanChuyen
                    {
                        MaDonHang = maDonHang,
                        MaVanChuyen = phieuVanChuyen.MaVanChuyen,
                        TrangThai = "Đang xử lý"
                    };

                    await _context.ChiTietVanChuyens.AddAsync(chiTietVanChuyen);

                    // Cập nhật trạng thái đơn hàng
                    var chiTietTrangThai = new ChiTietTrangThaiDonHang
                    {
                        MaDonHang = maDonHang,
                        MaTrangThai = 5, // Trạng thái "Đang giao hàng"
                        NgayCapNhat = DateTime.Now
                    };

                    await _context.ChiTietTrangThaiDonHangs.AddAsync(chiTietTrangThai);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw; // Throw exception để có thể xử lý ở controller
            }
        }

        public async Task<bool> UpdateDeliveryStatusAsync(int maDonHang)
        {
            try
            {
                // Get shipping details for the order
                var chiTietVanChuyen = await _context.ChiTietVanChuyens
                    .FirstOrDefaultAsync(c => c.MaDonHang == maDonHang);

                if (chiTietVanChuyen == null)
                {
                    return false;
                }

                // Get order details to check payment method
                var donHang = await _context.DonHangs
                    .FirstOrDefaultAsync(d => d.MaDonHang == maDonHang);

                if (donHang == null)
                {
                    return false;
                }

                // Update shipping status
                chiTietVanChuyen.TrangThai = "Đã giao hàng";
                _context.ChiTietVanChuyens.Update(chiTietVanChuyen);

                // Update order status to "Đã giao hàng"
                var trangThaiDonHang = new ChiTietTrangThaiDonHang
                {
                    MaDonHang = maDonHang,
                    MaTrangThai = 6, // Đã giao hàng
                    NgayCapNhat = DateTime.Now
                };

                _context.ChiTietTrangThaiDonHangs.Add(trangThaiDonHang);

                // If payment method is "Thanh toán khi nhận hàng" (COD, MaPTTTID == 1)
                // Update payment status to "Đã thanh toán"
                if (donHang.MaPTTTID == 1)
                {
                    donHang.MaTrangThaiID = 2; // Assuming 2 is the ID for "Đã thanh toán"
                    _context.DonHangs.Update(donHang);
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<DateTime> CalculateDeliveryDateAsync(int maDonHang)
        {
            string productType = await GetProductTypeAsync(maDonHang);
            DateTime shippingDate = DateTime.Now;

            // Calculate delivery date based on product type
            switch (productType.ToLower())
            {
                case "hải sản tươi sống":
                    return shippingDate.AddHours(24); // 24 hours for fresh seafood
                case "đông lạnh":
                    return shippingDate.AddDays(2); // 2 days for frozen products
                case "sấy khô":
                    return shippingDate.AddDays(4); // Average of 3-5 days for dried products
                default:
                    return shippingDate.AddDays(3); // Default: 3 days
            }
        }

        public async Task<string> GetProductTypeAsync(int maDonHang)
        {
            // Get order details
            var chiTietDonHang = await _context.ChiTietDonHangs
                .Where(c => c.MaDonHangID == maDonHang)
                .Include(c => c.SanPham)
                .ThenInclude(s => s.LoaiSanPham)
                .FirstOrDefaultAsync();

            if (chiTietDonHang?.SanPham?.LoaiBaoQuan != null)
            {
                return chiTietDonHang.SanPham.LoaiBaoQuan;
            }

            return "Không xác định";
        }
    }
} 