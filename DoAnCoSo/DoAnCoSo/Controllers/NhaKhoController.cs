using DoAnCoSo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NhaKhoController : Controller
    {
        private readonly SeafoodDbContext _context;

        public NhaKhoController(SeafoodDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách nhà kho
        public async Task<IActionResult> Index()
        {
            var nhaKhos = await _context.NhaKhos.ToListAsync();
            return View(nhaKhos);
        }

        // Chi tiết nhà kho
        public async Task<IActionResult> Details(int id)
        {
            var nhaKho = await _context.NhaKhos
                .Include(nk => nk.ChiTietKhoHangs)
                .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(nk => nk.MaNhaKho == id);

            if (nhaKho == null)
            {
                return NotFound();
            }
            return View(nhaKho);
        }

        // Thêm nhà kho
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(NhaKho nhaKho)
        {
            if (ModelState.IsValid)
            {
                _context.NhaKhos.Add(nhaKho);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(nhaKho);
        }

        // Sửa nhà kho
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var nhaKho = await _context.NhaKhos.FindAsync(id);
            if (nhaKho == null)
            {
                return NotFound();
            }
            return View(nhaKho);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(NhaKho nhaKho)
        {
            if (ModelState.IsValid)
            {
                _context.Update(nhaKho);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(nhaKho);
        }

        // Xóa nhà kho
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var nhaKho = await _context.NhaKhos
                .Include(nk => nk.ChiTietKhoHangs)
                .FirstOrDefaultAsync(nk => nk.MaNhaKho == id);

            if (nhaKho == null)
            {
                return NotFound();
            }
            if (nhaKho.ChiTietKhoHangs.Any())
            {
                TempData["Error"] = "Không thể xóa nhà kho vì còn sản phẩm trong kho.";
                return RedirectToAction("Index");
            }
            _context.NhaKhos.Remove(nhaKho);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Thêm sản phẩm vào kho
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddProductToWarehouse(int id)
        {
            ViewBag.SanPhams = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham");
            return View(new ChiTietKhoHang { MaNhaKhoID = id });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProductToWarehouse(ChiTietKhoHang chiTietKhoHang)
        {
            var existingChiTiet = await _context.ChiTietKhoHangs
                .FirstOrDefaultAsync(ct => ct.MaNhaKhoID == chiTietKhoHang.MaNhaKhoID && ct.MaSanPhamID == chiTietKhoHang.MaSanPhamID);

            if (existingChiTiet != null)
            {
                ModelState.AddModelError("", "Sản phẩm này đã có trong kho.");
                ViewBag.SanPhams = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham");
                return View(chiTietKhoHang);
            }

            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var sanPham = await _context.SanPhams.FindAsync(chiTietKhoHang.MaSanPhamID);
                    if (sanPham == null)
                    {
                        return NotFound();
                    }

                    // Add to warehouse with product's quantity
                    chiTietKhoHang.SoLuong = sanPham.SoLuong ?? 0;
                    _context.ChiTietKhoHangs.Add(chiTietKhoHang);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    
                    return RedirectToAction("Details", new { id = chiTietKhoHang.MaNhaKhoID });
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            ViewBag.SanPhams = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham");
            return View(chiTietKhoHang);
        }

        // Sửa sản phẩm trong kho
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProductInWarehouse(int id, int maSanPham)
        {
            var chiTiet = await _context.ChiTietKhoHangs
                .FirstOrDefaultAsync(ct => ct.MaNhaKhoID == id && ct.MaSanPhamID == maSanPham);

            if (chiTiet == null)
            {
                return NotFound();
            }

            ViewBag.SanPhams = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham", chiTiet.MaSanPhamID);
            return View(chiTiet);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductInWarehouse(ChiTietKhoHang chiTietKhoHang)
        {
            var existingChiTiet = await _context.ChiTietKhoHangs
                .FirstOrDefaultAsync(ct => ct.MaNhaKhoID == chiTietKhoHang.MaNhaKhoID && ct.MaSanPhamID == chiTietKhoHang.MaSanPhamID);

            if (existingChiTiet != null && ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var sanPham = await _context.SanPhams.FindAsync(chiTietKhoHang.MaSanPhamID);
                    if (sanPham != null)
                    {
                        // Only update the date, keep the quantity synchronized with product
                        existingChiTiet.NgayNhap = chiTietKhoHang.NgayNhap;
                        existingChiTiet.SoLuong = sanPham.SoLuong ?? 0;

                        _context.Update(existingChiTiet);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }

                    return RedirectToAction("Details", new { id = chiTietKhoHang.MaNhaKhoID });
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            ViewBag.SanPhams = new SelectList(_context.SanPhams, "MaSanPham", "TenSanPham", chiTietKhoHang.MaSanPhamID);
            return View(chiTietKhoHang);
        }

        // Xóa sản phẩm khỏi kho
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductFromWarehouse(int id, int maSanPham)
        {
            var chiTiet = await _context.ChiTietKhoHangs
                .FirstOrDefaultAsync(ct => ct.MaNhaKhoID == id && ct.MaSanPhamID == maSanPham);
            if (chiTiet != null)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var sanPham = await _context.SanPhams.FindAsync(chiTiet.MaSanPhamID);
                    if (sanPham != null)
                    {
                        sanPham.SoLuong -= chiTiet.SoLuong; // Giảm tổng số lượng
                        _context.ChiTietKhoHangs.Remove(chiTiet);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return RedirectToAction("Details", new { id });
        }
    }
}