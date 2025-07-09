using DoAnCoSo.Models;
using DoAnCoSo.Repositories;
using DoAnCoSo.Repositories.Interfaces;
using DoAnCoSo.Services.Interfaces;
using DoAnCoSo.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Cấu hình DbContext
builder.Services.AddDbContext<SeafoodDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Cấu hình PayPal từ appsettings.json
builder.Services.Configure<PayPalConfig>(builder.Configuration.GetSection("PayPalConfig"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<PayPalConfig>>().Value);

// 🔹 Cấu hình Gemini từ appsettings.json
builder.Services.Configure<GeminiConfig>(builder.Configuration.GetSection("Json:Gemini"));

builder.Services.AddHttpClient();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<SeafoodDbContext>();

// Đặt trước AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
 .AddDefaultTokenProviders()
 .AddDefaultUI()
 .AddEntityFrameworkStores<SeafoodDbContext>();
builder.Services.AddRazorPages();

// 🔹 Cấu hình cookie xác thực
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";         // Trang đăng nhập
    options.LogoutPath = "/Identity/Account/Logout";       // Trang đăng xuất
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Trang từ chối truy cập
});

// Register synchronization service
builder.Services.AddScoped<ISynchronizationService, SynchronizationService>();

// Register repositories
builder.Services.AddScoped<ISanPhamRepository, SanPhamRepository>();
builder.Services.AddScoped<IChiTietKhoHangRepository, ChiTietKhoHangRepository>();
builder.Services.AddScoped<IChiTietMuaRepository, ChiTietMuaRepository>();
builder.Services.AddScoped<IHoaDonRepository, HoaDonRepository>();
builder.Services.AddScoped<IDanhGiaRepository, DanhGiaRepository>();
builder.Services.AddScoped<IPhieuNhapHangRepository, PhieuNhapHangRepository>();
builder.Services.AddScoped<IPhuongThucThanhToanRepository, PhuongThucThanhToanRepository>();
builder.Services.AddScoped<IDanhMucRepository, DanhMucRepository>();
builder.Services.AddScoped<IGiamGiaRepository, GiamGiaRepository>();
builder.Services.AddScoped<IChiTietPhieuNhapRepository, ChiTietPhieuNhapRepository>();
builder.Services.AddScoped<IChiTietDonHangRepository, ChiTietDonHangRepository>();
builder.Services.AddScoped<INhaKhoRepository, NhaKhoRepository>();
builder.Services.AddScoped<IHinhAnhSanPhamRepository, HinhAnhSanPhamRepository>();
builder.Services.AddScoped<IDonHangRepository, DonHangRepository>();
builder.Services.AddScoped<IMuaRepository, MuaRepository>();
builder.Services.AddScoped<IVanChuyenRepository, PhieuVanChuyenRepository>();
builder.Services.AddScoped<IVanChuyenService, VanChuyenService>();
builder.Services.AddScoped<IPhuongTienVanChuyenRepository, PhuongTienVanChuyenRepository>();
builder.Services.AddScoped<ILoaiSanPhamRepository, LoaiSanPhamRepository>();
builder.Services.AddScoped<ITrangThaiDonHangRepository, TrangThaiDonHangRepository>();
builder.Services.AddScoped<IChiTietTrangThaiDonHangRepository, EFChiTietTrangThaiDonHangRepository>();
builder.Services.AddScoped<ITrangThaiThanhToanRepository, EFTrangThaiThanhToanRepository>();
builder.Services.AddScoped<INhanVienRepository, NhanVienRepository>();
builder.Services.AddScoped<IKhachHangRepository, KhachHangRepository>();
builder.Services.AddScoped<INhaCungCapRepository, EFNhaCungCapRepository>();
builder.Services.AddScoped<IGeminiService, GeminiService>();
builder.Services.AddScoped<IThongKeDoanhThuRepository, ThongKeDoanhThuRepository>();
// Register other repositories similarly

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseSession();
app.UseRouting(); 
app.UseStaticFiles();

app.UseAuthorization();
app.MapRazorPages();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Admin",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();