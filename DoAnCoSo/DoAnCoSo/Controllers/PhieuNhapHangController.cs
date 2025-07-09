using DoAnCoSo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSo.Controllers
{
    public class PhieuNhapHangController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SeafoodDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PhieuNhapHangController(UserManager<ApplicationUser> userManager, SeafoodDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: PhieuNhapHang
        [Authorize(Roles = "Admin,Nhà cung cấp")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            IQueryable<PhieuNhapHang> query = _context.PhieuNhapHangs
                .Include(p => p.ChiTietPhieuNhaps)
                .ThenInclude(c => c.SanPham)
                .Include(p => p.NhaCungCap);

            if (!isAdmin)
            {
                var nhaCungCap = await _context.NhaCungCaps.FirstOrDefaultAsync(n => n.UserID == user.Id);
                if (nhaCungCap == null)
                {
                    return NotFound();
                }
                query = query.Where(p => p.MaNhaCungCapID == nhaCungCap.MaNhaCungCap);
            }

            var phieuNhapHangs = await query.ToListAsync();
            return View(phieuNhapHangs);
        }

        // GET: PhieuNhapHang/Create
        [Authorize(Roles = "Nhà cung cấp")]
        public async Task<IActionResult> Create()
        {
            // Load danh mục và loại sản phẩm để nhà cung cấp chọn
            ViewBag.DanhMucs = new SelectList(await _context.DanhMucs.ToListAsync(), "MaDanhMuc", "TenDanhMuc");
            ViewBag.LoaiSanPhams = new SelectList(await _context.LoaiSanPhams.ToListAsync(), "MaLoai", "TenLoai");
            return View(new PhieuNhapHang());
        }

        // POST: PhieuNhapHang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Nhà cung cấp")]
        public async Task<IActionResult> Create(PhieuNhapHang phieuNhapHang, List<SanPham> sanPhams, List<int> soLuongSanPhams, List<decimal> giaNhapHangs, List<IFormFile> images)
        {
            try
            {
                if (!ModelState.IsValid || sanPhams == null || !sanPhams.Any() || soLuongSanPhams == null || giaNhapHangs == null)
                {
                    ViewBag.DanhMucs = new SelectList(await _context.DanhMucs.ToListAsync(), "MaDanhMuc", "TenDanhMuc");
                    ViewBag.LoaiSanPhams = new SelectList(await _context.LoaiSanPhams.ToListAsync(), "MaLoai", "TenLoai");
                    ModelState.AddModelError("", "Vui lòng nhập ít nhất một sản phẩm và thông tin liên quan.");
                    return View(phieuNhapHang);
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                var nhaCungCap = await _context.NhaCungCaps.FirstOrDefaultAsync(n => n.UserID == user.Id);
                if (nhaCungCap == null)
                {
                    return NotFound();
                }

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Tạo phiếu nhập hàng
                        phieuNhapHang.MaNhaCungCapID = nhaCungCap.MaNhaCungCap;
                        _context.PhieuNhapHangs.Add(phieuNhapHang);
                        await _context.SaveChangesAsync();

                        decimal tongTien = 0;
                        int tongSoLuong = 0;

                        // Thêm từng sản phẩm và chi tiết phiếu nhập
                        for (int i = 0; i < sanPhams.Count; i++)
                        {
                            var sanPham = sanPhams[i];
                            string? imagePath = null;
                            
                            // Đảm bảo các trường bắt buộc không null
                            sanPham.TenSanPham = sanPham.TenSanPham ?? "Sản phẩm mới";
                            sanPham.LoaiBaoQuan = sanPham.LoaiBaoQuan ?? "Tiêu chuẩn";
                            sanPham.TinhTrang = sanPham.TinhTrang ?? "Còn hàng";
                            sanPham.NguonGoc = sanPham.NguonGoc ?? "Nội địa";
                            sanPham.MoTa = sanPham.MoTa ?? "Chưa có mô tả";
                            
                            sanPham.SoLuong = soLuongSanPhams[i];
                            sanPham.GiaTheoKG = giaNhapHangs[i] * 1.2m; // Giá bán = giá nhập * 1.2

                            // Xử lý upload ảnh
                            if (images != null && i < images.Count && images[i] != null && images[i].Length > 0)
                            {
                                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                                if (!Directory.Exists(uploadsFolder))
                                {
                                    Directory.CreateDirectory(uploadsFolder);
                                }

                                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(images[i].FileName);
                                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await images[i].CopyToAsync(fileStream);
                                }

                                imagePath = "/images/" + uniqueFileName;
                                sanPham.ImageUrl = imagePath;
                            }

                            _context.SanPhams.Add(sanPham);
                            await _context.SaveChangesAsync();

                            // Tạo chi tiết phiếu nhập
                            var chiTiet = new ChiTietPhieuNhap
                            {
                                MaPhieuNhap = phieuNhapHang.MaPhieuNhap,
                                MaSanPham = sanPham.MaSanPham,
                                SoLuongSanPham = soLuongSanPhams[i],
                                GiaNhapHang = giaNhapHangs[i],                             
                            };
                            _context.ChiTietPhieuNhaps.Add(chiTiet);

                            tongTien += chiTiet.SoLuongSanPham * chiTiet.GiaNhapHang;
                            tongSoLuong += chiTiet.SoLuongSanPham;
                        }

                        // Cập nhật tổng tiền và tổng số lượng
                        phieuNhapHang.TongTien = tongTien;
                        phieuNhapHang.TongSoLuong = tongSoLuong;
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        TempData["SuccessMessage"] = "Tạo phiếu nhập hàng thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                ViewBag.DanhMucs = new SelectList(await _context.DanhMucs.ToListAsync(), "MaDanhMuc", "TenDanhMuc");
                ViewBag.LoaiSanPhams = new SelectList(await _context.LoaiSanPhams.ToListAsync(), "MaLoai", "TenLoai");
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo phiếu nhập hàng.");
                return View(phieuNhapHang);
            }
        }

        // GET: PhieuNhapHang/Details/5
        [Authorize(Roles = "Admin,Nhà cung cấp")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phieuNhapHang = await _context.PhieuNhapHangs
                .Include(p => p.NhaCungCap)
                .Include(p => p.ChiTietPhieuNhaps)
                .ThenInclude(c => c.SanPham)
                .FirstOrDefaultAsync(m => m.MaPhieuNhap == id);

            if (phieuNhapHang == null)
            {
                return NotFound();
            }

            // Kiểm tra quyền truy cập
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (!isAdmin)
            {
                var nhaCungCap = await _context.NhaCungCaps.FirstOrDefaultAsync(n => n.UserID == user.Id);
                if (nhaCungCap == null || phieuNhapHang.MaNhaCungCapID != nhaCungCap.MaNhaCungCap)
                {
                    return Forbid();
                }
            }

            ViewBag.IsAdmin = isAdmin;
            return View(phieuNhapHang);
        }

        // POST: PhieuNhapHang/Confirm/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Confirm(int id)
        {
            var phieuNhapHang = await _context.PhieuNhapHangs
                .Include(p => p.ChiTietPhieuNhaps)
                .ThenInclude(c => c.SanPham)
                .FirstOrDefaultAsync(p => p.MaPhieuNhap == id);

            if (phieuNhapHang == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy phiếu nhập hàng.";
                return NotFound();
            }

            if (phieuNhapHang.DaXacNhan)
            {
                TempData["ErrorMessage"] = "Phiếu nhập hàng này đã được xác nhận trước đó.";
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin người dùng.";
                return NotFound();
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Cập nhật trạng thái phiếu nhập
                    phieuNhapHang.DaXacNhan = true;
                    phieuNhapHang.ThoiGianXacNhan = DateTime.Now;

                    // Get or create default warehouse
                    var defaultWarehouse = await _context.NhaKhos.FirstOrDefaultAsync();
                    if (defaultWarehouse == null)
                    {
                        defaultWarehouse = new NhaKho
                        {
                            TenNhaKho = "Kho mặc định",
                            DiaChi = "Địa chỉ mặc định",
                            LoaiKho = "Kho chung"
                        };
                        _context.NhaKhos.Add(defaultWarehouse);
                        await _context.SaveChangesAsync();
                    }

                    // Cập nhật số lượng sản phẩm và chi tiết kho
                    foreach (var chiTiet in phieuNhapHang.ChiTietPhieuNhaps)
                    {
                        var sanPham = await _context.SanPhams.FindAsync(chiTiet.MaSanPham);
                        if (sanPham != null)
                        {
                            // Cập nhật số lượng sản phẩm
                            sanPham.SoLuong = (sanPham.SoLuong ?? 0) + chiTiet.SoLuongSanPham;
                            _context.Update(sanPham);

                            // Kiểm tra và cập nhật chi tiết kho
                            var chiTietKho = await _context.ChiTietKhoHangs
                                .FirstOrDefaultAsync(ct => ct.MaNhaKhoID == defaultWarehouse.MaNhaKho && ct.MaSanPhamID == chiTiet.MaSanPham);

                            if (chiTietKho != null)
                            {
                                // Cập nhật số lượng nếu đã tồn tại trong kho
                                chiTietKho.SoLuong += chiTiet.SoLuongSanPham;
                                _context.Update(chiTietKho);
                            }
                            else
                            {
                                // Tạo mới chi tiết kho nếu chưa tồn tại
                                chiTietKho = new ChiTietKhoHang
                                {
                                    MaNhaKhoID = defaultWarehouse.MaNhaKho,
                                    MaSanPhamID = chiTiet.MaSanPham,
                                    SoLuong = chiTiet.SoLuongSanPham,
                                    NgayNhap = DateTime.Now
                                };
                                await _context.ChiTietKhoHangs.AddAsync(chiTietKho);
                            }
                        }
                    }

                    _context.Update(phieuNhapHang);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    TempData["SuccessMessage"] = "Xác nhận phiếu nhập hàng thành công!";
                    return RedirectToAction(nameof(Details), new { id = phieuNhapHang.MaPhieuNhap });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    TempData["ErrorMessage"] = $"Lỗi khi xác nhận phiếu nhập hàng: {ex.Message}";
                    return RedirectToAction(nameof(Details), new { id = phieuNhapHang.MaPhieuNhap });
                }
            }
        }
    }
}