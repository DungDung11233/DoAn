using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoAnCoSo.Models;
using DoAnCoSo.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnCoSo.Controllers
{
    [Authorize(Roles = "Admin")] // Chỉ quản trị viên có quyền truy cập
    public class DonHangController : Controller
    {
        private readonly IDonHangRepository _donHangRepository;
        private readonly ITrangThaiDonHangRepository _trangThaiDonHangRepository;
        private readonly SeafoodDbContext _context;

        public DonHangController(
            IDonHangRepository donHangRepository,
            ITrangThaiDonHangRepository trangThaiDonHangRepository,
            SeafoodDbContext context)
        {
            _donHangRepository = donHangRepository;
            _trangThaiDonHangRepository = trangThaiDonHangRepository;
            _context = context;
        }

        // Hiển thị danh sách đơn hàng
        public async Task<IActionResult> Index()
        {
            var donHangs = await _context.DonHangs
                .Include(dh => dh.KhachHang)
                .Include(dh => dh.ChiTietTrangThaiDonHangs)
                .ThenInclude(ct => ct.TrangThaiDonHang)
                .ToListAsync();
            return View(donHangs);
        }

        // Hiển thị form thêm đơn hàng
        public IActionResult Add()
        {
            return View();
        }

        // Xử lý thêm đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    donHang.NgayDatHang = DateTime.Now;
                    await _donHangRepository.AddAsync(donHang);

                    // Thêm trạng thái ban đầu: Đã đặt hàng (MaTrangThai = 1)
                    var chiTietTrangThai = new ChiTietTrangThaiDonHang
                    {
                        MaDonHang = donHang.MaDonHang,
                        MaTrangThai = 1 // Đã đặt hàng
                    };
                    _context.ChiTietTrangThaiDonHangs.Add(chiTietTrangThai);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Thêm đơn hàng thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Không thể thêm đơn hàng: " + ex.Message);
                }
            }
            return View(donHang);
        }

        public async Task<IActionResult> Details(int id)
        {
            var donHang = await _donHangRepository.GetByIdAsync(id);
            if (donHang == null)
            {
                TempData["Error"] = "Đơn hàng không tồn tại!";
                return NotFound();
            }

            // Kiểm tra trạng thái để debug
            if (donHang.ChiTietTrangThaiDonHangs == null || !donHang.ChiTietTrangThaiDonHangs.Any())
            {
                TempData["Warning"] = "Đơn hàng chưa có trạng thái nào!";
            }

            return View(donHang);
        }
        //[Authorize(Roles = "Admin")] // Chỉ quản trị viên có quyền truy cập

        // Hiển thị form cập nhật trạng thái
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var donHang = await _context.DonHangs
                .Include(dh => dh.KhachHang)
                .Include(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.SanPham)
                .Include(dh => dh.PhuongThucThanhToan)
                .Include(dh => dh.ChiTietTrangThaiDonHangs)
                .ThenInclude(ct => ct.TrangThaiDonHang)
                .FirstOrDefaultAsync(dh => dh.MaDonHang == id);

            if (donHang == null)
            {
                return NotFound();
            }

            var trangThaiList = await _trangThaiDonHangRepository.GetAllAsync();
            ViewBag.TrangThaiList = new SelectList(trangThaiList, "MaTrangThai", "TenTrangThai");
            return View(donHang);
        }

        // Xử lý cập nhật trạng thái
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int maDonHang, int maTrangThai)
        {
            var donHang = await _context.DonHangs.FindAsync(maDonHang);
            if (donHang == null)
            {
                TempData["Error"] = "Đơn hàng không tồn tại!";
                return NotFound();
            }

            // Kiểm tra trạng thái hợp lệ
            var trangThai = await _trangThaiDonHangRepository.GetByIdAsync(maTrangThai);
            if (trangThai == null)
            {
                TempData["Error"] = "Trạng thái không hợp lệ!";
                return RedirectToAction("UpdateStatus", new { id = maDonHang });
            }

            // Kiểm tra thứ tự trạng thái (ngăn quay lại trạng thái trước đó)
            var currentStatus = await _context.ChiTietTrangThaiDonHangs
                .Where(ct => ct.MaDonHang == maDonHang)
                .OrderByDescending(ct => ct.MaTrangThai)
                .Select(ct => ct.MaTrangThai)
                .FirstOrDefaultAsync();

            if (currentStatus >= maTrangThai)
            {
                TempData["Error"] = "Không thể quay lại trạng thái trước đó!";
                return RedirectToAction("UpdateStatus", new { id = maDonHang });
            }

            // Thêm trạng thái mới
            var chiTietTrangThai = new ChiTietTrangThaiDonHang
            {
                MaDonHang = maDonHang,
                MaTrangThai = maTrangThai
            };
            _context.ChiTietTrangThaiDonHangs.Add(chiTietTrangThai);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật trạng thái thành công!";
            return RedirectToAction(nameof(Index));
        }

        // Hiển thị form cập nhật đơn hàng
        public async Task<IActionResult> Update(int id)
        {
            var donHang = await _donHangRepository.GetByIdAsync(id);
            if (donHang == null)
            {
                return NotFound();
            }
            return View(donHang);
        }

        // Xử lý cập nhật đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, DonHang donHang)
        {
            if (id != donHang.MaDonHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _donHangRepository.UpdateAsync(donHang);
                    TempData["Success"] = "Cập nhật đơn hàng thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Không thể cập nhật đơn hàng: " + ex.Message);
                }
            }
            return View(donHang);
        }

        // Hiển thị form xóa đơn hàng
        public async Task<IActionResult> Delete(int id)
        {
            var donHang = await _donHangRepository.GetByIdAsync(id);
            if (donHang == null)
            {
                return NotFound();
            }
            return View(donHang);
        }

        // Xử lý xóa đơn hàng
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _donHangRepository.DeleteAsync(id);
                TempData["Success"] = "Xóa đơn hàng thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Không thể xóa đơn hàng: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}