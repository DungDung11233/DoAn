using DoAnCoSo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MuaController : Controller
    {
        private readonly SeafoodDbContext _context;

        public MuaController(SeafoodDbContext context)
        {
            _context = context;
        }

        // Danh sách mùa
        public async Task<IActionResult> Index()
        {
            var muas = await _context.Muas.ToListAsync();
            return View(muas);
        }

        // Chi tiết mùa kèm danh sách sản phẩm
        public async Task<IActionResult> Details(int id)
        {
            var mua = await _context.Muas
                .Include(m => m.ChiTietMuas)
                .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(m => m.MaMua == id);

            if (mua == null)
            {
                return NotFound();
            }
            return View(mua);
        }

        // Thêm mùa
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Mua mua)
        {
            if (ModelState.IsValid)
            {
                _context.Muas.Add(mua);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(mua);
        }

        // Sửa mùa
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var mua = await _context.Muas.FindAsync(id);
            if (mua == null)
            {
                return NotFound();
            }
            return View(mua);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Mua mua)
        {
            if (ModelState.IsValid)
            {
                _context.Update(mua);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(mua);
        }

        // Xóa mùa
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var mua = await _context.Muas
                .Include(m => m.ChiTietMuas)
                .FirstOrDefaultAsync(m => m.MaMua == id);

            if (mua == null)
            {
                return NotFound();
            }
            if (mua.ChiTietMuas != null && mua.ChiTietMuas.Any())
            {
                TempData["Error"] = "Không thể xóa mùa vì có sản phẩm liên quan.";
                return RedirectToAction("Index");
            }
            _context.Muas.Remove(mua);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Thêm sản phẩm vào mùa
        [HttpGet]
        public IActionResult AddProductToSeason(int id)
        {
            ViewBag.SanPhams = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham");
            var chiTietMua = new ChiTietMua { MaMuaID = id };
            return View(chiTietMua);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToSeason(ChiTietMua chiTietMua)
        {
            if (chiTietMua.MaMuaID == null || chiTietMua.MaSanPhamID == null)
            {
                ModelState.AddModelError("", "Phải chọn sản phẩm và mùa.");
            }
            if (chiTietMua.SoLuongTheoMua <= 0)
            {
                ModelState.AddModelError("SoLuongTheoMua", "Số lượng phải lớn hơn 0.");
            }

            var existingChiTiet = await _context.ChiTietMuas
                .FirstOrDefaultAsync(ct => ct.MaMuaID == chiTietMua.MaMuaID && ct.MaSanPhamID == chiTietMua.MaSanPhamID);

            if (ModelState.IsValid)
            {
                if (existingChiTiet != null)
                {
                    existingChiTiet.SoLuongTheoMua = chiTietMua.SoLuongTheoMua;
                    _context.Update(existingChiTiet);
                }
                else
                {
                    _context.ChiTietMuas.Add(chiTietMua);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = chiTietMua.MaMuaID });
            }

            ViewBag.SanPhams = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham");
            return View(chiTietMua);
        }

        // Hiển thị form chỉnh sửa sản phẩm trong mùa
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProductInSeason(int id, int maSanPham)
        {
            var chiTiet = await _context.ChiTietMuas
                .FirstOrDefaultAsync(ct => ct.MaMuaID == id && ct.MaSanPhamID == maSanPham);

            if (chiTiet == null)
            {
                // Nếu không tìm thấy ChiTietMua, trả về lỗi và hiển thị danh sách sản phẩm
                ViewBag.SanPhams = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham");
                return NotFound();
            }

            // Hiển thị thông tin chi tiết sản phẩm trong mùa
            ViewBag.SanPhams = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham", chiTiet.MaSanPhamID);
            return View(chiTiet);
        }

        // Xử lý khi gửi form chỉnh sửa sản phẩm trong mùa
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductInSeason(ChiTietMua chiTietMua)
        {
            // Kiểm tra số lượng sản phẩm có hợp lệ không
            if (chiTietMua.SoLuongTheoMua <= 0)
            {
                ModelState.AddModelError("SoLuongTheoMua", "Số lượng phải lớn hơn 0.");
                ViewBag.SanPhams = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham", chiTietMua.MaSanPhamID);
                return View(chiTietMua);
            }

            // Kiểm tra xem sản phẩm đã có trong mùa chưa
            var existingChiTiet = await _context.ChiTietMuas
                .FirstOrDefaultAsync(ct => ct.MaMuaID == chiTietMua.MaMuaID && ct.MaSanPhamID == chiTietMua.MaSanPhamID);

            if (existingChiTiet == null)
            {
                ModelState.AddModelError("", "Không tìm thấy thông tin sản phẩm trong mùa.");
                ViewBag.SanPhams = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham", chiTietMua.MaSanPhamID);
                return View(chiTietMua);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var sanPham = await _context.SanPhams.FindAsync(chiTietMua.MaSanPhamID);
                if (sanPham == null)
                {
                    ModelState.AddModelError("", "Sản phẩm không tồn tại.");
                    ViewBag.SanPhams = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham", chiTietMua.MaSanPhamID);
                    return View(chiTietMua);
                }

                // Cập nhật số lượng sản phẩm trong mùa
                var chenhLech = chiTietMua.SoLuongTheoMua - existingChiTiet.SoLuongTheoMua;
                sanPham.SoLuong -= chenhLech; // Giảm số lượng tồn kho khi tăng số lượng trong mùa

                if (sanPham.SoLuong < 0)
                {
                    ModelState.AddModelError("SoLuongTheoMua", "Số lượng sản phẩm trong kho không đủ.");
                    ViewBag.SanPhams = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham", chiTietMua.MaSanPhamID);
                    return View(chiTietMua);
                }

                existingChiTiet.SoLuongTheoMua = chiTietMua.SoLuongTheoMua;

                _context.Update(sanPham);
                _context.Update(existingChiTiet);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToAction("Details", new { id = chiTietMua.MaMuaID });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                ViewBag.SanPhams = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham", chiTietMua.MaSanPhamID);
                return View(chiTietMua);
            }
        }

        // Xóa sản phẩm khỏi mùa
        [HttpPost]
        public async Task<IActionResult> DeleteProductFromSeason(int id, int maSanPham)
        {
            var chiTiet = await _context.ChiTietMuas
                .FirstOrDefaultAsync(ct => ct.MaMuaID == id && ct.MaSanPhamID == maSanPham);

            if (chiTiet != null)
            {
                _context.ChiTietMuas.Remove(chiTiet);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id });
        }
    }
}
