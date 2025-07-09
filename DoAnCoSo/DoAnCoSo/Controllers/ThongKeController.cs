using Microsoft.AspNetCore.Mvc;
using DoAnCoSo.Models;
using DoAnCoSo.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DoAnCoSo.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly SeafoodDbContext _context;
        private readonly IThongKeDoanhThuRepository _thongKeRepository;

        public ThongKeController(SeafoodDbContext context, IThongKeDoanhThuRepository thongKeRepository)
        {
            _context = context;
            _thongKeRepository = thongKeRepository;
        }

        public async Task<IActionResult> Index()
        {
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            ViewBag.CurrentYear = currentYear;
            
            // Kiểm tra năm trong request (nếu có)
            int requestedYear;
            if (Request.Query.TryGetValue("nam", out var namValue) && 
                int.TryParse(namValue, out requestedYear))
            {
                // Nếu năm được chọn lớn hơn năm hiện tại, sử dụng năm hiện tại
                if (requestedYear > currentYear)
                {
                    return RedirectToAction("Index", new { nam = currentYear });
                }
            }
            
            var hoaDonNamHienTai = await _context.HoaDons
                .Where(h => h.NgayTaoHoaDon.Year == currentYear)
                .ToListAsync();

            // Lưu thống kê cho mỗi hóa đơn nếu chưa có
            foreach (var hoaDon in hoaDonNamHienTai)
            {
                var thongKeExists = await _context.ThongKeDoanhThus
                    .AnyAsync(t => t.MaHoaDonID == hoaDon.MaHoaDon);

                if (!thongKeExists)
                {
                    var thongKe = new ThongKeDoanhThu
                    {
                        ThoiGian = hoaDon.NgayTaoHoaDon,
                        DoanhThu = hoaDon.TongSoTien,
                        SoLuong = 1, // Mỗi hóa đơn tính là 1 đơn
                        MaHoaDonID = hoaDon.MaHoaDon
                    };

                    await _thongKeRepository.AddAsync(thongKe);
                }
            }
            
            ViewBag.TongDoanhThuNam = hoaDonNamHienTai.Sum(h => h.TongSoTien);
            ViewBag.TongDonHangNam = hoaDonNamHienTai.Count();
            ViewBag.TrungBinhDonHang = ViewBag.TongDonHangNam > 0 
                ? ViewBag.TongDoanhThuNam / ViewBag.TongDonHangNam 
                : 0;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetThongKeTheoThang(int nam)
        {
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            // Kiểm tra năm hợp lệ
            if (nam > currentYear)
            {
                nam = currentYear;
            }
            
            // Xác định số tháng cần thống kê
            var soThangCanThongKe = nam < currentYear ? 12 : currentMonth;

            var hoaDonTrongNam = await _context.HoaDons
                .Where(h => h.NgayTaoHoaDon.Year == nam)
                .ToListAsync();

            // Tạo danh sách các tháng đã qua
            var thongKeTheoThang = Enumerable.Range(1, soThangCanThongKe)
                .GroupJoin(
                    hoaDonTrongNam,
                    month => month,
                    hoaDon => hoaDon.NgayTaoHoaDon.Month,
                    (month, hoaDons) => new
                    {
                        Thang = month,
                        SoLuongDonHang = hoaDons.Count(),
                        TongDoanhThu = hoaDons.Sum(h => h.TongSoTien)
                    })
                .OrderBy(t => t.Thang)
                .ToList();

            var labels = thongKeTheoThang.Select(t => "Tháng " + t.Thang).ToList();
            var soLuongData = thongKeTheoThang.Select(t => t.SoLuongDonHang).ToList();
            var doanhThuData = thongKeTheoThang.Select(t => (double)t.TongDoanhThu).ToList();

            // Tính % tăng/giảm so với tháng trước (chỉ cho các tháng đã qua)
            var phanTramThayDoi = new List<double>();
            for (int i = 0; i < doanhThuData.Count; i++)
            {
                if (i == 0)
                {
                    phanTramThayDoi.Add(0); // Tháng đầu tiên không có thay đổi
                }
                else if (doanhThuData[i-1] == 0)
                {
                    // Nếu tháng trước = 0 và tháng này > 0 thì tăng 100%
                    phanTramThayDoi.Add(doanhThuData[i] > 0 ? 100 : 0);
                }
                else
                {
                    var thayDoi = ((doanhThuData[i] - doanhThuData[i-1]) / doanhThuData[i-1]) * 100;
                    phanTramThayDoi.Add(Math.Round(thayDoi, 2));
                }
            }

            return Json(new
            {
                Labels = labels,
                SoLuongDonHang = soLuongData,
                DoanhThu = doanhThuData,
                PhanTramThayDoi = phanTramThayDoi
            });
        }

        [HttpGet]
        public IActionResult GetNamThongKe()
        {
            var currentYear = DateTime.Now.Year;
            // Tạo danh sách 5 năm từ năm hiện tại trở về trước
            var years = Enumerable.Range(0, 5)
                                .Select(i => currentYear - i)
                                .ToList();
            return Json(years);
        }
    }
} 