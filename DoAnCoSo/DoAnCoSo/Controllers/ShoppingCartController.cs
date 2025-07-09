using DoAnCoSo.Extensions;
using DoAnCoSo.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font;
using iText.IO.Font.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using DoAnCoSo.Repositories;
using Microsoft.Extensions.Options;
using PayPal.Api;
using System.Globalization;

namespace DoAnCoSo.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly SeafoodDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITrangThaiDonHangRepository _trangThaiDonHangRepository; // Thêm repository
        private const string CartSessionKey = "Cart";
        private readonly PayPalConfig _payPalConfig;
        private readonly IThongKeDoanhThuRepository _thongKeRepository;


        public ShoppingCartController(SeafoodDbContext context, UserManager<ApplicationUser> userManager, ITrangThaiDonHangRepository trangThaiDonHangRepository, IOptions<PayPalConfig> payPalConfig, IThongKeDoanhThuRepository thongKeRepository)
        {
            _context = context;
            _userManager = userManager;
            _trangThaiDonHangRepository = trangThaiDonHangRepository; // Khởi tạo repository
            _payPalConfig = payPalConfig.Value;
            _thongKeRepository = thongKeRepository;
        }

        // Hiển thị giỏ hàng
        [Authorize(Roles = "Khách hàng")]
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>(CartSessionKey) ?? new ShoppingCart();
            return View(cart);
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpGet]
        public IActionResult AddToCart(int maSanPham, string tenSanPham, decimal giaTheoKG, string? imageUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Error"] = "Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng!";
                return RedirectToAction("Index", "Home");
            }

            if (!User.IsInRole("Khách hàng"))
            {
                TempData["Error"] = "Bạn cần có tài khoản khách hàng để mua hàng!";
                return RedirectToAction("Index", "Home");
            }

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>(CartSessionKey) ?? new ShoppingCart();

            var existingItem = cart.Items.FirstOrDefault(i => i.MaSanPham == maSanPham);
            if (existingItem != null)
            {
                existingItem.SoLuong++;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    MaSanPham = maSanPham,
                    TenSanPham = tenSanPham,
                    GiaTheoKG = giaTheoKG,
                    SoLuong = 1,
                    ImageUrl = imageUrl
                });
            }

            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
            TempData["Success"] = "Đã thêm sản phẩm vào giỏ hàng!";
            return RedirectToAction("Index", "Home");
        }

        // Xóa sản phẩm khỏi giỏ hàng
        [HttpPost]
        [Authorize(Roles = "Khách hàng")]
        public IActionResult RemoveFromCart(int maSanPham)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>(CartSessionKey) ?? new ShoppingCart();
            var itemToRemove = cart.Items.FirstOrDefault(i => i.MaSanPham == maSanPham);
            if (itemToRemove != null)
            {
                cart.Items.Remove(itemToRemove);
            }

            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
            return RedirectToAction("Index");
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        [HttpPost]
        [Authorize(Roles = "Khách hàng")]
        public IActionResult UpdateQuantity(int maSanPham, int soLuong)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>(CartSessionKey) ?? new ShoppingCart();
            var item = cart.Items.FirstOrDefault(i => i.MaSanPham == maSanPham);
            if (item != null && soLuong > 0)
            {
                item.SoLuong = soLuong;
            }

            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
            return Ok();
        }

        // Hiển thị trang đặt hàng (GET)
        [HttpGet]
        [Authorize(Roles = "Khách hàng")] // Chỉ khách hàng mới được đặt hàng
        public async Task<IActionResult> Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>(CartSessionKey);
            if (cart == null || !cart.Items.Any())
            {
                TempData["Error"] = "Giỏ hàng đang trống!";
                return RedirectToAction("Index");
            }

            var danhSachPTTT = await _context.PhuongThucThanhToans.ToListAsync();
            ViewBag.PhuongThucThanhToanList = new SelectList(danhSachPTTT, "MaPTTT", "TenPTTT");

            return View(cart);
        }

        // Xử lý đặt hàng (POST)
        [HttpPost]
        [Authorize(Roles = "Khách hàng")]
        public async Task<IActionResult> Checkout(string diaChiGiaoHang, string ghiChu, string? maGiamGia, int maPTTTID)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>(CartSessionKey);
            if (cart == null || !cart.Items.Any())
            {
                TempData["Error"] = "Giỏ hàng đang trống!";
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "Không xác định được người dùng!";
                return RedirectToAction("Index");
            }

            var nguoiDung = await _context.KhachHangs
                .FirstOrDefaultAsync(nd => nd.UserID == user.Id);
            if (nguoiDung == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin người dùng trong hệ thống!";
                return RedirectToAction("Index");
            }

            // Kiểm tra số lượng tồn kho
            foreach (var item in cart.Items)
            {
                var sanPham = await _context.SanPhams.FindAsync(item.MaSanPham);
                if (sanPham == null)
                {
                    TempData["Error"] = $"Sản phẩm {item.TenSanPham} không tồn tại.";
                    return RedirectToAction("Checkout");
                }

                // Lấy số lượng thực tế từ kho
                var soLuongTrongKho = await _context.ChiTietKhoHangs
                    .Where(ct => ct.MaSanPhamID == item.MaSanPham)
                    .SumAsync(ct => (int?)ct.SoLuong) ?? 0;

                if (soLuongTrongKho < item.SoLuong)
                {
                    TempData["Error"] = $"Sản phẩm {item.TenSanPham} không đủ số lượng trong kho (còn lại: {soLuongTrongKho}).";
                    return RedirectToAction("Checkout");
                }
            }

            // Tính tổng tiền
            var tongTien = cart.Items.Sum(item => item.SoLuong * item.GiaTheoKG);
            decimal soTienGiam = 0;
            GiamGia? giamGia = null;

            if (!string.IsNullOrEmpty(maGiamGia))
            {
                giamGia = await _context.GiamGias.FirstOrDefaultAsync(g =>
                    g.Code == maGiamGia &&
                    g.SoLuong > 0 &&
                    g.NgayApDung <= DateTime.Now &&
                    g.SoLanDungMa < g.SoLanToiDaDungMa);

                if (giamGia != null)
                {
                    if (giamGia.GiamTheoPhanTram.HasValue)
                    {
                        soTienGiam = tongTien * (decimal)(giamGia.GiamTheoPhanTram.Value / 100.0);
                    }
                    else if (giamGia.GiamTheoSoTien.HasValue)
                    {
                        soTienGiam = giamGia.GiamTheoSoTien.Value;
                    }
                    soTienGiam = Math.Min(soTienGiam, tongTien);
                    tongTien -= soTienGiam;

                    giamGia.SoLanDungMa += 1;
                    giamGia.SoLuong -= 1;
                    _context.GiamGias.Update(giamGia);
                }
                else
                {
                    TempData["Error"] = "Mã giảm giá không hợp lệ!";
                    return RedirectToAction("Checkout");
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Tạo đơn hàng
                var donHang = new DonHang
                {
                    NgayDatHang = DateTime.Now,
                    TongSoTien = tongTien,
                    DiaChiGiaoHang = diaChiGiaoHang,
                    GhiChu = ghiChu,
                    MaKhachHangID = nguoiDung.MaKhachHang,
                    MaGiamGiaID = giamGia?.MaGiamGia,
                    MaPTTTID = maPTTTID,
                    MaTrangThaiID = maPTTTID == 1 ? 1 : null, // "Chưa thanh toán" cho COD, null cho PayPal
                    ChiTietDonHangs = cart.Items.Select(item => new ChiTietDonHang
                    {
                        MaSanPhamID = item.MaSanPham,
                        SoLuong = item.SoLuong
                    }).ToList()
                };

                _context.DonHangs.Add(donHang);
                await _context.SaveChangesAsync();

                // Thêm trạng thái "Đã đặt hàng"
                var trangThai = await _context.TrangThaiDonHangs.FindAsync(1);
                if (trangThai == null)
                {
                    await transaction.RollbackAsync();
                    TempData["Error"] = "Không tìm thấy trạng thái 'Đã đặt hàng' trong cơ sở dữ liệu!";
                    return RedirectToAction("Checkout");
                }

                var chiTietTrangThai = new ChiTietTrangThaiDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    MaTrangThai = 1 // Đã đặt hàng
                };
                _context.ChiTietTrangThaiDonHangs.Add(chiTietTrangThai);
                await _context.SaveChangesAsync();

                // Tạo hóa đơn
                var hoaDon = new HoaDon
                {
                    NgayTaoHoaDon = DateTime.Now,
                    TongSoTien = donHang.TongSoTien,
                    MaDonHangID = donHang.MaDonHang,
                    MaKhachHangID = nguoiDung.MaKhachHang,
                    MaNhanVienID = null
                };
                _context.HoaDons.Add(hoaDon);
                await _context.SaveChangesAsync();

                // Tạo thống kê cho hóa đơn mới
                var thongKe = new ThongKeDoanhThu
                {
                    ThoiGian = hoaDon.NgayTaoHoaDon,
                    DoanhThu = hoaDon.TongSoTien,
                    SoLuong = 1,
                    MaHoaDonID = hoaDon.MaHoaDon
                };
                await _thongKeRepository.AddAsync(thongKe);

                // Cập nhật số lượng sản phẩm
                foreach (var item in cart.Items)
                {
                    var sanPham = await _context.SanPhams.FindAsync(item.MaSanPham);
                    if (sanPham != null)
                    {
                        // Cập nhật số lượng trong bảng SanPham
                        sanPham.SoLuong -= item.SoLuong;
                        _context.SanPhams.Update(sanPham);
                    }

                    // Cập nhật số lượng trong kho theo FIFO
                    var chiTietKhoHangs = await _context.ChiTietKhoHangs
                        .Where(ct => ct.MaSanPhamID == item.MaSanPham)
                        .OrderBy(ct => ct.NgayNhap) // Lấy theo FIFO (First In First Out)
                        .ToListAsync();

                    int soLuongCanGiam = item.SoLuong;
                    foreach (var chiTiet in chiTietKhoHangs)
                    {
                        if (soLuongCanGiam <= 0) break;

                        if (chiTiet.SoLuong >= soLuongCanGiam)
                        {
                            chiTiet.SoLuong -= soLuongCanGiam;
                            soLuongCanGiam = 0;
                        }
                        else
                        {
                            soLuongCanGiam -= chiTiet.SoLuong;
                            chiTiet.SoLuong = 0;
                        }

                        if (chiTiet.SoLuong == 0)
                        {
                            _context.ChiTietKhoHangs.Remove(chiTiet);
                        }
                        else
                        {
                            _context.ChiTietKhoHangs.Update(chiTiet);
                        }
                    }

                    if (soLuongCanGiam > 0)
                    {
                        await transaction.RollbackAsync();
                        TempData["Error"] = $"Không đủ số lượng trong kho cho sản phẩm {item.TenSanPham}.";
                        return RedirectToAction("Checkout");
                    }
                }

                await _context.SaveChangesAsync();

                // Nếu chọn PayPal, chuyển hướng đến thanh toán PayPal
                if (maPTTTID == 2)
                {
                    await transaction.CommitAsync();
                    HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
                    HttpContext.Session.SetString("DiaChiGiaoHang", diaChiGiaoHang ?? "");
                    HttpContext.Session.SetString("GhiChu", ghiChu ?? "");
                    HttpContext.Session.SetString("TongTien", tongTien.ToString("N0"));
                    return RedirectToAction("PayWithPaypal", new { maDonHang = donHang.MaDonHang });
                }

                // Nếu chọn COD, hoàn tất đơn hàng
                await transaction.CommitAsync();
                HttpContext.Session.Remove(CartSessionKey);
                TempData["Success"] = "Đặt hàng thành công! Hóa đơn của bạn đã được tạo.";
                TempData["MaHoaDon"] = hoaDon.MaHoaDon;
                return RedirectToAction("OrderCompleted");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["Error"] = $"Có lỗi xảy ra khi đặt hàng: {ex.Message}";
                return RedirectToAction("Checkout");
            }
        }


        // Khởi tạo thanh toán PayPal
        [HttpGet]
        [Authorize(Roles = "Khách hàng")]
        public async Task<IActionResult> PayWithPaypal(int maDonHang)
        {
            var donHang = await _context.DonHangs.FindAsync(maDonHang);
            if (donHang == null)
            {
                TempData["Error"] = "Đơn hàng không tồn tại!";
                return RedirectToAction("Checkout");
            }

            try
            {
                // Configure PayPal
                var apiContext = new APIContext(new OAuthTokenCredential(
                    _payPalConfig.ClientId,
                    _payPalConfig.ClientSecret,
                    new Dictionary<string, string> { { "mode", _payPalConfig.Mode ?? "sandbox" } }
                ).GetAccessToken());

                // Ensure mode is set
                apiContext.Config = new Dictionary<string, string> { { "mode", _payPalConfig.Mode ?? "sandbox" } };

                // Chuyển đổi VND sang USD (tỷ giá cố định: 25000)
                decimal exchangeRate = 25000;
                decimal amountUsd = donHang.TongSoTien / exchangeRate;
                string totalAmountUSD = Math.Round(amountUsd, 2).ToString("F2", System.Globalization.CultureInfo.InvariantCulture);

                // Tạo thanh toán PayPal
                var payment = new Payment
                {
                    intent = "sale",
                    payer = new Payer { payment_method = "paypal" },
                    transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            description = $"Đơn hàng #{donHang.MaDonHang}",
                            amount = new Amount
                            {
                                currency = "USD",
                                total = totalAmountUSD
                            }
                        }
                    },
                    redirect_urls = new RedirectUrls
                    {
                        cancel_url = Url.Action("Cancel", "ShoppingCart", null, Request.Scheme),
                        return_url = Url.Action("Success", "ShoppingCart", new { maDonHang }, Request.Scheme)
                    }
                };

                // Tạo thanh toán
                var createdPayment = payment.Create(apiContext);

                // Lấy link chuyển hướng đến PayPal
                var approvalUrl = createdPayment.links
                    .FirstOrDefault(l => l.rel.ToLower() == "approval_url")?.href;

                if (approvalUrl != null)
                {
                    return Redirect(approvalUrl);
                }

                TempData["Error"] = "Không thể tạo thanh toán PayPal!";
                return RedirectToAction("Checkout");
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? "No inner exception";
                Console.WriteLine($"PayPal Error: {ex.Message}, Inner: {innerMessage}, StackTrace: {ex.StackTrace}");
                TempData["Error"] = $"Lỗi khi khởi tạo thanh toán PayPal: {ex.Message}";
                return RedirectToAction("Checkout");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Khách hàng")]
        public async Task<IActionResult> Success(int maDonHang, string paymentId, string token, string PayerID)
        {
            var donHang = await _context.DonHangs.FindAsync(maDonHang);
            if (donHang == null)
            {
                TempData["Error"] = "Đơn hàng không tồn tại!";
                return RedirectToAction("Checkout");
            }

            try
            {
                // Configure PayPal
                var apiContext = new APIContext(new OAuthTokenCredential(
                    _payPalConfig.ClientId,
                    _payPalConfig.ClientSecret,
                    new Dictionary<string, string> { { "mode", _payPalConfig.Mode ?? "sandbox" } }
                ).GetAccessToken());

                // Ensure mode is set
                apiContext.Config = new Dictionary<string, string> { { "mode", _payPalConfig.Mode ?? "sandbox" } };

                // Lấy thông tin thanh toán
                var payment = Payment.Get(apiContext, paymentId);
                var execution = new PaymentExecution { payer_id = PayerID };

                // Thực hiện thanh toán
                var executedPayment = payment.Execute(apiContext, execution);

                if (executedPayment.state.ToLower() == "approved")
                {
                    // Cập nhật trạng thái đơn hàng
                    donHang.MaTrangThaiID = 2; // Đã thanh toán
                    _context.DonHangs.Update(donHang);

                    // Thêm chi tiết trạng thái mới
                    var chiTietTrangThai = new ChiTietTrangThaiDonHang
                    {
                        MaDonHang = donHang.MaDonHang,
                        MaTrangThai = 2, // Đã thanh toán
                        NgayCapNhat = DateTime.Now
                    };
                    _context.ChiTietTrangThaiDonHangs.Add(chiTietTrangThai);

                    await _context.SaveChangesAsync();

                    // Lấy MaHoaDon từ bảng HoaDons
                    var hoaDon = await _context.HoaDons
                        .FirstOrDefaultAsync(hd => hd.MaDonHangID == maDonHang);
                    if (hoaDon != null)
                    {
                        TempData["MaHoaDon"] = hoaDon.MaHoaDon;
                    }

                    // Xóa dữ liệu session
                    HttpContext.Session.Remove(CartSessionKey);
                    HttpContext.Session.Remove("DiaChiGiaoHang");
                    HttpContext.Session.Remove("GhiChu");
                    HttpContext.Session.Remove("TongTien");

                    TempData["Success"] = "Thanh toán PayPal thành công! Đơn hàng của bạn đã được xử lý.";
                    return RedirectToAction("OrderCompleted");
                }

                TempData["Error"] = "Thanh toán PayPal không được phê duyệt!";
                return RedirectToAction("Checkout");
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? "No inner exception";
                Console.WriteLine($"PayPal Success Error: {ex.Message}, Inner: {innerMessage}, StackTrace: {ex.StackTrace}");
                TempData["Error"] = $"Lỗi khi xử lý thanh toán PayPal: {ex.Message}";
                return RedirectToAction("Checkout");
            }
        }

        // Xử lý hủy thanh toán PayPal
        [HttpGet]
        [Authorize(Roles = "Khách hàng")]
        public IActionResult Cancel()
        {
            TempData["Error"] = "Thanh toán PayPal đã bị hủy.";
            return RedirectToAction("Checkout");
        }



        // Hiển thị trang đặt hàng thành công
        public IActionResult OrderCompleted()
        {
            return View();
        }

        // Xóa toàn bộ giỏ hàng
        [HttpPost]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CartSessionKey);
            return RedirectToAction("Index");
        }

        // Hiển thị lịch sử đơn hàng
        // Hiển thị lịch sử đơn hàng
        [HttpGet]
        [Authorize(Roles = "Khách hàng, Admin")]
        public async Task<IActionResult> OrderHistory()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "Không thể xác định người dùng!";
                return RedirectToAction("Index", "Home");
            }

            var donHangs = await _context.DonHangs
                .Include(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.SanPham)
                .Include(dh => dh.PhuongThucThanhToan)
                .Include(dh => dh.KhachHang)
                .Include(dh => dh.ChiTietTrangThaiDonHangs)
                .ThenInclude(ct => ct.TrangThaiDonHang)
                .Include(dh => dh.TrangThaiThanhToan) // Thêm Include cho TrangThaiThanhToan
                .Where(dh => dh.KhachHang.UserID == user.Id)
                .ToListAsync();

            return View(donHangs);
        }
        // Xem hóa đơn
        [HttpGet]
        [Authorize(Roles = "Khách hàng,Nhân viên,Admin")]
        public async Task<IActionResult> ViewInvoice(int maHoaDon)
        {
            var hoaDon = await _context.HoaDons
                .Include(hd => hd.DonHang)
                .ThenInclude(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.SanPham)
                .Include(hd => hd.DonHang)
                .ThenInclude(dh => dh.PhuongThucThanhToan)
                .Include(hd => hd.DonHang)
                .ThenInclude(dh => dh.KhachHang)
                .Include(hd => hd.DonHang)
                .ThenInclude(dh => dh.TrangThaiThanhToan) // Thêm Include cho TrangThaiThanhToan
                .FirstOrDefaultAsync(hd => hd.MaHoaDon == maHoaDon);

            if (hoaDon == null)
            {
                TempData["Error"] = "Hóa đơn không tồn tại!";
                return RedirectToAction("OrderHistory");
            }

            var user = await _userManager.GetUserAsync(User);
            if (hoaDon.DonHang.KhachHang?.UserID != user.Id)
            {
                TempData["Error"] = "Bạn không có quyền xem hóa đơn này!";
                return RedirectToAction("OrderHistory");
            }

            return View(hoaDon);
        }

        // Tải hóa đơn PDF
        [HttpGet]
        [Authorize(Roles = "Khách hàng")]
        public async Task<IActionResult> DownloadInvoice(int maHoaDon)
        {
            var hoaDon = await _context.HoaDons
                .Include(hd => hd.DonHang)
                .ThenInclude(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.SanPham)
                .Include(hd => hd.DonHang)
                .ThenInclude(dh => dh.PhuongThucThanhToan)
                .Include(hd => hd.DonHang)
                .ThenInclude(dh => dh.KhachHang)
                .Include(hd => hd.DonHang)
                .ThenInclude(dh => dh.TrangThaiThanhToan) // Thêm Include cho TrangThaiThanhToan
                .FirstOrDefaultAsync(hd => hd.MaHoaDon == maHoaDon);

            if (hoaDon == null)
            {
                TempData["Error"] = "Hóa đơn không tồn tại!";
                return RedirectToAction("OrderHistory");
            }

            var user = await _userManager.GetUserAsync(User);
            if (hoaDon.DonHang.KhachHang.UserID != user.Id)
            {
                TempData["Error"] = "Bạn không có quyền tải hóa đơn này!";
                return RedirectToAction("OrderHistory");
            }

            using var memoryStream = new MemoryStream();
            PdfWriter writer = new PdfWriter(memoryStream);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, iText.Kernel.Geom.PageSize.A4);
            document.SetMargins(25, 25, 30, 30);

            string fontPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/fonts/DejaVuSans.ttf");
            string boldFontPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/fonts/DejaVuSans-Bold.ttf");

            if (!System.IO.File.Exists(fontPath) || !System.IO.File.Exists(boldFontPath))
            {
                TempData["Error"] = "Không tìm thấy file font DejaVuSans.ttf hoặc DejaVuSans-Bold.ttf!";
                return RedirectToAction("OrderHistory");
            }

            PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
            PdfFont boldFont = PdfFontFactory.CreateFont(boldFontPath, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);

            // Tiêu đề
            document.Add(new Paragraph("HÓA ĐƠN")
                .SetFont(boldFont)
                .SetFontSize(16)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(20));

            // Thông tin hóa đơn
            document.Add(new Paragraph($"Mã Hóa Đơn: {hoaDon.MaHoaDon}").SetFont(font).SetFontSize(12));
            document.Add(new Paragraph($"Ngày Tạo: {hoaDon.NgayTaoHoaDon.ToString("dd/MM/yyyy HH:mm:ss")}").SetFont(font).SetFontSize(12));
            document.Add(new Paragraph($"Mã Đơn Hàng: {hoaDon.MaDonHangID}").SetFont(font).SetFontSize(12));
            document.Add(new Paragraph($"Tổng Số Tiền: {hoaDon.TongSoTien.ToString("N0")} VND").SetFont(font).SetFontSize(12));
            document.Add(new Paragraph($"Trạng Thái Thanh Toán: {hoaDon.DonHang.TrangThaiThanhToan?.TenTrangThai ?? "Chưa xác định"}").SetFont(font).SetFontSize(12));
            document.Add(new Paragraph("\n"));

            // Thông tin khách hàng
            document.Add(new Paragraph("Thông Tin Khách Hàng").SetFont(boldFont).SetFontSize(12));
            document.Add(new Paragraph($"Tên Khách Hàng: {hoaDon.DonHang.KhachHang?.TenKhachHang ?? "Không xác định"}").SetFont(font).SetFontSize(12));
            document.Add(new Paragraph($"Địa Chỉ Giao Hàng: {hoaDon.DonHang.DiaChiGiaoHang}").SetFont(font).SetFontSize(12));
            document.Add(new Paragraph($"Phương Thức Thanh Toán: {hoaDon.DonHang.PhuongThucThanhToan?.TenPTTT ?? "Không xác định"}").SetFont(font).SetFontSize(12));
            document.Add(new Paragraph("\n"));

            // Bảng chi tiết đơn hàng
            document.Add(new Paragraph("Chi Tiết Đơn Hàng").SetFont(boldFont).SetFontSize(12));
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 3, 2, 1, 2 })).UseAllAvailableWidth();

            table.AddHeaderCell(new Cell().Add(new Paragraph("Sản Phẩm").SetFont(boldFont).SetFontSize(12)).SetPadding(5));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Giá").SetFont(boldFont).SetFontSize(12)).SetPadding(5));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Số Lượng").SetFont(boldFont).SetFontSize(12)).SetPadding(5));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Tổng").SetFont(boldFont).SetFontSize(12)).SetPadding(5));

            foreach (var item in hoaDon.DonHang.ChiTietDonHangs)
            {
                table.AddCell(new Cell().Add(new Paragraph(item.SanPham.TenSanPham).SetFont(font).SetFontSize(12)).SetPadding(5));
                table.AddCell(new Cell().Add(new Paragraph(item.SanPham.GiaTheoKG.ToString("N0") + " VND").SetFont(font).SetFontSize(12)).SetPadding(5));
                table.AddCell(new Cell().Add(new Paragraph(item.SoLuong.ToString()).SetFont(font).SetFontSize(12)).SetPadding(5));
                table.AddCell(new Cell().Add(new Paragraph((item.SanPham.GiaTheoKG * item.SoLuong).ToString("N0") + " VND").SetFont(font).SetFontSize(12)).SetPadding(5));
            }

            table.AddCell(new Cell(1, 3).Add(new Paragraph("Tổng Cộng").SetFont(boldFont).SetFontSize(12)).SetPadding(5));
            table.AddCell(new Cell().Add(new Paragraph(hoaDon.TongSoTien.ToString("N0") + " VND").SetFont(boldFont).SetFontSize(12)).SetPadding(5));

            document.Add(table);

            document.Close();
            writer.Close();

            var bytes = memoryStream.ToArray();
            return File(bytes, "application/pdf", $"HoaDon_{hoaDon.MaHoaDon}.pdf");
        }

    
        // Xem chi tiết đơn hàng
        [HttpGet]
        [Authorize(Roles = "Khách hàng")]
        public async Task<IActionResult> ViewOrder(int maDonHang)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "Không thể xác định người dùng!";
                return RedirectToAction("OrderHistory");
            }

            var donHang = await _context.DonHangs
                .Include(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.SanPham)
                .Include(dh => dh.PhuongThucThanhToan)
                .Include(dh => dh.KhachHang)
                .Include(dh => dh.ChiTietTrangThaiDonHangs)
                .ThenInclude(ct => ct.TrangThaiDonHang)
                .Include(dh => dh.TrangThaiThanhToan) // Thêm Include cho TrangThaiThanhToan
                .FirstOrDefaultAsync(dh => dh.MaDonHang == maDonHang && dh.KhachHang.UserID == user.Id);

            if (donHang == null)
            {
                TempData["Error"] = "Đơn hàng không tồn tại hoặc bạn không có quyền xem!";
                return RedirectToAction("OrderHistory");
            }

            return View(donHang);
        }
    }
}