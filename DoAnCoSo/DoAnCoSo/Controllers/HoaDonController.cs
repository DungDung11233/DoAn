using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnCoSo.Models;
using Microsoft.AspNetCore.Authorization;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font;
using iText.IO.Font.Constants;
using Microsoft.AspNetCore.Identity;
using DoAnCoSo.Repositories;

namespace DoAnCoSo.Controllers
{
    [Authorize(Roles = "Admin,Nhân viên")]
    public class HoaDonController : Controller
    {
        private readonly SeafoodDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IThongKeDoanhThuRepository _thongKeRepository;

        public HoaDonController(
            SeafoodDbContext context, 
            UserManager<ApplicationUser> userManager,
            IThongKeDoanhThuRepository thongKeRepository)
        {
            _context = context;
            _userManager = userManager;
            _thongKeRepository = thongKeRepository;
        }

        public async Task<IActionResult> Index()
        {
            var hoaDons = await _context.HoaDons
                .Include(h => h.NhanVien)
                .Include(h => h.KhachHang)
                .Include(h => h.DonHang)
                    .ThenInclude(d => d.TrangThaiThanhToan)
                .OrderByDescending(h => h.NgayTaoHoaDon)
                .ToListAsync();

            // Tự động tạo thống kê cho các hóa đơn chưa có
            foreach (var hoaDon in hoaDons)
            {
                var thongKeExists = await _context.ThongKeDoanhThus
                    .AnyAsync(t => t.MaHoaDonID == hoaDon.MaHoaDon);

                if (!thongKeExists)
                {
                    var thongKe = new ThongKeDoanhThu
                    {
                        ThoiGian = hoaDon.NgayTaoHoaDon,
                        DoanhThu = hoaDon.TongSoTien,
                        SoLuong = 1,
                        MaHoaDonID = hoaDon.MaHoaDon
                    };

                    await _thongKeRepository.AddAsync(thongKe);
                }
            }

            return View(hoaDons);
        }

        public async Task<IActionResult> Details(int id)
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
                    .ThenInclude(dh => dh.TrangThaiThanhToan)
                .Include(hd => hd.NhanVien)
                .FirstOrDefaultAsync(hd => hd.MaHoaDon == id);

            if (hoaDon == null)
            {
                TempData["Error"] = "Hóa đơn không tồn tại!";
                return RedirectToAction(nameof(Index));
            }

            return View(hoaDon);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadInvoice(int id)
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
                    .ThenInclude(dh => dh.TrangThaiThanhToan)
                .Include(hd => hd.NhanVien)
                .FirstOrDefaultAsync(hd => hd.MaHoaDon == id);

            if (hoaDon == null)
            {
                TempData["Error"] = "Hóa đơn không tồn tại!";
                return RedirectToAction(nameof(Index));
            }

            // Lấy thông tin nhân viên đang đăng nhập
            var user = await _userManager.GetUserAsync(User);
            if (user != null && User.IsInRole("Nhân viên"))
            {
                var nhanVien = await _context.NhanViens
                    .FirstOrDefaultAsync(nv => nv.UserID == user.Id);
                
                if (nhanVien != null)
                {
                    // Cập nhật mã nhân viên vào hóa đơn
                    hoaDon.MaNhanVienID = nhanVien.MaNhanVien;
                    await _context.SaveChangesAsync();
                }
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
                return RedirectToAction(nameof(Index));
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
            document.Add(new Paragraph($"Nhân Viên Phụ Trách: {hoaDon.NhanVien?.TenNhanVien ?? "N/A"}").SetFont(font).SetFontSize(12));
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

        // Thêm phương thức để tạo thống kê cho hóa đơn mới
        private async Task CreateThongKeForHoaDon(HoaDon hoaDon)
        {
            var thongKe = new ThongKeDoanhThu
            {
                ThoiGian = hoaDon.NgayTaoHoaDon,
                DoanhThu = hoaDon.TongSoTien,
                SoLuong = 1,
                MaHoaDonID = hoaDon.MaHoaDon
            };

            await _thongKeRepository.AddAsync(thongKe);
        }
    }
} 