using System;
using System.Threading.Tasks;
using DoAnCoSo.Models;
using DoAnCoSo.Repositories;
using DoAnCoSo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace DoAnCoSo.Controllers
{
    [Authorize]
    public class VanChuyenController : Controller
    {
        private readonly IVanChuyenRepository _vanChuyenRepository;
        private readonly IVanChuyenService _vanChuyenService;
        private readonly IDonHangRepository _donHangRepository;
        private readonly IPhuongTienVanChuyenRepository _phuongTienRepository;
        private readonly INhanVienRepository _nhanVienRepository;
        private readonly SeafoodDbContext _context;

        public VanChuyenController(
            IVanChuyenRepository vanChuyenRepository,
            IVanChuyenService vanChuyenService,
            IDonHangRepository donHangRepository,
            IPhuongTienVanChuyenRepository phuongTienRepository,
            INhanVienRepository nhanVienRepository,
            SeafoodDbContext context)
        {
            _vanChuyenRepository = vanChuyenRepository;
            _vanChuyenService = vanChuyenService;
            _donHangRepository = donHangRepository;
            _phuongTienRepository = phuongTienRepository;
            _nhanVienRepository = nhanVienRepository;
            _context = context;
        }

        // GET: VanChuyen
        [Authorize(Roles = "Admin,Nhân viên")]
        public async Task<IActionResult> Index()
        {
            var vanChuyens = await _vanChuyenRepository.GetAllAsync();
            return View(vanChuyens);
        }

        // GET: VanChuyen/Details/5
        [Authorize(Roles = "Admin,Nhân viên")]
        public async Task<IActionResult> Details(int id)
        {
            var vanChuyen = await _vanChuyenRepository.GetByIdAsync(id);
            if (vanChuyen == null)
            {
                return NotFound();
            }

            // Get order details associated with this shipping record, including NguoiDung and order details
            var chiTietVanChuyens = await _context.ChiTietVanChuyens
                .Where(c => c.MaVanChuyen == id)
                .Include(c => c.DonHang)
                .ThenInclude(d => d.KhachHang) // Include NguoiDung data
                .Include(c => c.DonHang.ChiTietDonHangs) // Include order details to get quantities
                .ToListAsync();

            // Calculate the total quantity of items across all orders
            int totalQuantity = chiTietVanChuyens
                .SelectMany(ct => ct.DonHang.ChiTietDonHangs) // Flatten the related order details
                .Sum(ctdh => ctdh.SoLuong); // Sum the quantities (assuming SoLuong exists)

            ViewBag.ChiTietVanChuyens = chiTietVanChuyens;
            ViewBag.TotalQuantity = totalQuantity; // Add total quantity to ViewBag

            return View(vanChuyen);
        }

        // GET: VanChuyen/Create
        [Authorize(Roles = "Admin,Nhân viên")]
        public async Task<IActionResult> Create()
        {
            // Get pending orders (those that are confirmed but not yet shipped)
            var pendingOrders = await _context.DonHangs
                .Include(d => d.ChiTietTrangThaiDonHangs)
                .ThenInclude(c => c.TrangThaiDonHang)
                .Include(d => d.KhachHang)
                .Where(d => d.ChiTietTrangThaiDonHangs.Any(c => c.MaTrangThai == 2 || c.MaTrangThai == 3)) // Đã xác nhận hoặc đang gói hàng
                .Where(d => !d.ChiTietTrangThaiDonHangs.Any(c => c.MaTrangThai >= 4)) // Chưa bàn giao vận chuyển
                .Select(d => new
                {
                    MaDonHang = d.MaDonHang,
                    ThongTin = $"Đơn #{d.MaDonHang} - {d.KhachHang.TenKhachHang} - {d.DiaChiGiaoHang}"
                })
                .ToListAsync();

            ViewBag.MaDonHangs = new MultiSelectList(pendingOrders, "MaDonHang", "ThongTin");
            ViewBag.MaPhuongTienID = new SelectList(await _phuongTienRepository.GetAllAsync(), "MaPhuongTien", "LoaiPhuongTien");
            ViewBag.MaNhanVienID = new SelectList(await _nhanVienRepository.GetAllAsync(), "MaNhanVien", "TenNhanVien");

            return View();
        }

        // POST: VanChuyen/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Nhân viên")]
        public async Task<IActionResult> Create(CreateShippingViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool result = await _vanChuyenService.ProcessShippingOrderAsync(
                    model.MaDonHangs,
                    model.MaPhuongTienID,
                    model.MaNhanVienID);

                if (result)
                {
                    TempData["Success"] = "Đã tạo phiếu vận chuyển thành công!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi tạo phiếu vận chuyển");
                }
            }

            // If we got to here, something went wrong - repopulate the dropdowns
            var pendingOrders = await _context.DonHangs
                .Include(d => d.KhachHang)
                .Where(d => d.ChiTietTrangThaiDonHangs.Any(c => c.MaTrangThai == 2 || c.MaTrangThai == 3))
                .Where(d => !d.ChiTietTrangThaiDonHangs.Any(c => c.MaTrangThai >= 4))
                .Select(d => new
                {
                    MaDonHang = d.MaDonHang,
                    ThongTin = $"Đơn #{d.MaDonHang} - {d.KhachHang.TenKhachHang} - {d.DiaChiGiaoHang}"
                })
                .ToListAsync();

            ViewBag.MaDonHangs = new MultiSelectList(pendingOrders, "MaDonHang", "ThongTin", model.MaDonHangs);
            ViewBag.MaPhuongTienID = new SelectList(await _phuongTienRepository.GetAllAsync(), "MaPhuongTien", "LoaiPhuongTien", model.MaPhuongTienID);
            ViewBag.MaNhanVienID = new SelectList(await _nhanVienRepository.GetAllAsync(), "MaNhanVien", "TenNhanVien", model.MaNhanVienID);

            return View(model);
        }

        // GET: VanChuyen/PendingDeliveries
        [Authorize(Roles = "Admin,Nhân viên")]
        public async Task<IActionResult> PendingDeliveries()
        {
            var pendingDeliveries = await _context.ChiTietVanChuyens
                .Where(c => c.TrangThai == "Đang xử lý")
                .Include(c => c.DonHang)
                .Include(c => c.PhieuVanChuyen)
                .ThenInclude(p => p.PhuongTienVanChuyen)
                .Include(c => c.PhieuVanChuyen.NhanVien)
                .ToListAsync();

            var viewModels = new List<PendingDeliveryViewModel>();

            foreach (var delivery in pendingDeliveries)
            {
                string productType = await _vanChuyenService.GetProductTypeAsync(delivery.MaDonHang);
                DateTime estimatedDelivery = await _vanChuyenService.CalculateDeliveryDateAsync(delivery.MaDonHang);

                viewModels.Add(new PendingDeliveryViewModel
                {
                    MaDonHang = delivery.MaDonHang,
                    MaVanChuyen = delivery.MaVanChuyen,
                    NgayVanChuyen = delivery.PhieuVanChuyen.NgayVanChuyen,
                    TrangThai = delivery.TrangThai,
                    LoaiSanPham = productType,
                    NgayDuKienGiaoHang = estimatedDelivery,
                    PhuongTien = delivery.PhieuVanChuyen.PhuongTienVanChuyen?.LoaiPhuongTien,
                    NhanVien = delivery.PhieuVanChuyen.NhanVien?.TenNhanVien
                });
            }

            return View(viewModels);
        }

        // POST: VanChuyen/CompleteDelivery/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Nhân viên")]
        public async Task<IActionResult> CompleteDelivery(int maDonHang)
        {
            bool result = await _vanChuyenService.UpdateDeliveryStatusAsync(maDonHang);
            
            if (result)
            {
                // Check if the order used COD payment method
                var donHang = await _context.DonHangs
                    .Include(d => d.PhuongThucThanhToan)
                    .FirstOrDefaultAsync(d => d.MaDonHang == maDonHang);

                if (donHang != null && donHang.MaPTTTID == 1)
                {
                    TempData["Success"] = "Đã cập nhật trạng thái giao hàng thành công và đã cập nhật thanh toán sang trạng thái Đã thanh toán!";
                }
                else
                {
                    TempData["Success"] = "Đã cập nhật trạng thái giao hàng thành công!";
                }
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra khi cập nhật trạng thái giao hàng";
            }
            
            return RedirectToAction(nameof(PendingDeliveries));
        }

        // GET: VanChuyen/DeliveryHistory
        [Authorize(Roles = "Admin,Nhân viên")]
        public async Task<IActionResult> DeliveryHistory()
        {
            var deliveryHistory = await _context.ChiTietVanChuyens
                .Where(c => c.TrangThai == "Đã giao hàng")
                .Include(c => c.DonHang)
                .Include(c => c.PhieuVanChuyen)
                .ThenInclude(p => p.PhuongTienVanChuyen)
                .Include(c => c.PhieuVanChuyen.NhanVien)
                .ToListAsync();

            return View(deliveryHistory);
        }
    }
} 