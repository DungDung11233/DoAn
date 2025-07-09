using DoAnCoSo.Models;
using DoAnCoSo.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DoAnCoSo.Repositories;
using System.Linq;

namespace DoAnCoSo.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly GeminiConfig _geminiConfig;
        private readonly ISanPhamRepository _sanPhamRepository;
        private readonly IDanhMucRepository _danhMucRepository;
        private readonly IDonHangRepository _donHangRepository;

        public GeminiService(
            HttpClient httpClient, 
            IOptions<GeminiConfig> geminiConfig, 
            ISanPhamRepository sanPhamRepository,
            IDanhMucRepository danhMucRepository,
            IDonHangRepository donHangRepository)
        {
            _httpClient = httpClient;
            _geminiConfig = geminiConfig.Value;
            _sanPhamRepository = sanPhamRepository;
            _danhMucRepository = danhMucRepository;
            _donHangRepository = donHangRepository;
        }

        public async Task<string> GenerateContentAsync(string prompt)
        {
            try
            {
                var request = new GeminiRequest
                {
                    Contents = new List<Content>
                    {
                        new Content
                        {
                            Parts = new List<Part>
                            {
                                new Part { Text = prompt }
                            }
                        }
                    },
                    GenerationConfig = new GenerationConfig
                    {
                        Temperature = 0.7f,
                        MaxOutputTokens = 2048
                    }
                };

                // Fix the API URL if it has a typo
                string apiUrl = _geminiConfig.ApiUrl;
                if (apiUrl.Contains("- pro:"))
                {
                    apiUrl = apiUrl.Replace("- pro:", ":");
                }

                string fullUrl = $"{apiUrl}?key={_geminiConfig.ApiKey}";
                
                var response = await _httpClient.PostAsJsonAsync(fullUrl, request);
                
                if (response.IsSuccessStatusCode)
                {
                    var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
                    
                    if (geminiResponse?.Candidates != null && geminiResponse.Candidates.Count > 0 && 
                        geminiResponse.Candidates[0].Content?.Parts != null && 
                        geminiResponse.Candidates[0].Content.Parts.Count > 0)
                    {
                        return geminiResponse.Candidates[0].Content.Parts[0].Text;
                    }
                    
                    return "No response generated.";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return $"Error: {response.StatusCode}, {errorContent}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> GenerateChatResponseAsync(List<ChatMessage> messages)
        {
            try
            {
                var request = new GeminiRequest
                {
                    Contents = messages.Select(m => new Content
                    {
                        Role = m.Role,
                        Parts = new List<Part> { new Part { Text = m.Content } }
                    }).ToList(),
                    GenerationConfig = new GenerationConfig
                    {
                        Temperature = 0.7f,
                        MaxOutputTokens = 2048
                    }
                };

                // Fix the API URL if it has a typo
                string apiUrl = _geminiConfig.ApiUrl;
                if (apiUrl.Contains("- pro:"))
                {
                    apiUrl = apiUrl.Replace("- pro:", ":");
                }

                string fullUrl = $"{apiUrl}?key={_geminiConfig.ApiKey}";
                
                var response = await _httpClient.PostAsJsonAsync(fullUrl, request);
                
                if (response.IsSuccessStatusCode)
                {
                    var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
                    
                    if (geminiResponse?.Candidates != null && geminiResponse.Candidates.Count > 0 && 
                        geminiResponse.Candidates[0].Content?.Parts != null && 
                        geminiResponse.Candidates[0].Content.Parts.Count > 0)
                    {
                        return geminiResponse.Candidates[0].Content.Parts[0].Text;
                    }
                    
                    return "No response generated.";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return $"Error: {response.StatusCode}, {errorContent}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> GenerateDataAwareResponseAsync(string userQuery)
        {
            try
            {
                // Phân tích truy vấn để xác định loại thông tin cần lấy
                var queryType = AnalyzeQueryType(userQuery);
                string dataContext = "";

                try
                {
                    switch (queryType)
                    {
                        case QueryType.ProductList:
                            dataContext = await GetProductListContext();
                            break;
                        case QueryType.ProductDetail:
                            dataContext = await GetProductDetailContext(userQuery);
                            break;
                        case QueryType.CategoryInfo:
                            dataContext = await GetCategoryContext();
                            break;
                        case QueryType.PriceInfo:
                            dataContext = await GetPriceContext(userQuery);
                            break;
                        case QueryType.OrderInfo:
                            dataContext = await GetOrderContext();
                            break;
                        default:
                            dataContext = await GetGeneralContext();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi chi tiết khi truy cập dữ liệu cụ thể
                    Console.WriteLine($"Lỗi khi truy cập dữ liệu {queryType}: {ex.Message}");
                    Console.WriteLine($"Chi tiết: {ex.StackTrace}");
                    
                    // Trả về một thông báo lỗi chi tiết hơn
                    return $"Tôi xin lỗi, tôi không thể truy cập dữ liệu {queryType} lúc này. Chi tiết lỗi: {ex.Message}";
                }

                // Thêm thông tin truy vấn của người dùng vào context
                dataContext += $"\n\nUser question: {userQuery}\n\nAnswer the question in the same language as the question (Vietnamese or English). Be concise but informative.";

                return await GenerateContentAsync(dataContext);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi tổng thể trong GenerateDataAwareResponseAsync: {ex.Message}");
                Console.WriteLine($"Chi tiết: {ex.StackTrace}");
                return $"Xin lỗi, tôi không thể truy cập dữ liệu hệ thống lúc này. Chi tiết lỗi: {ex.Message}";
            }
        }

        private QueryType AnalyzeQueryType(string query)
        {
            query = query.ToLower();

            // Tìm kiếm chi tiết sản phẩm cụ thể
            if ((query.Contains("chi tiết") || query.Contains("thông tin") || query.Contains("details") || 
                query.Contains("mô tả") || query.Contains("describe")) && 
                (query.Contains("sản phẩm") || query.Contains("product")))
            {
                return QueryType.ProductDetail;
            }

            // Tìm kiếm thông tin về giá
            if ((query.Contains("giá") || query.Contains("price") || query.Contains("cost") || 
                query.Contains("bao nhiêu") || query.Contains("how much")) &&
                (query.Contains("sản phẩm") || query.Contains("product")))
            {
                return QueryType.PriceInfo;
            }

            // Tìm kiếm thông tin về danh mục
            if (query.Contains("danh mục") || query.Contains("category") || query.Contains("loại"))
            {
                return QueryType.CategoryInfo;
            }

            // Tìm kiếm thông tin về đơn hàng
            if (query.Contains("đơn hàng") || query.Contains("order") || query.Contains("mua hàng"))
            {
                return QueryType.OrderInfo;
            }

            // Tìm kiếm danh sách sản phẩm
            if (query.Contains("sản phẩm") || query.Contains("product") || 
                query.Contains("hàng hóa") || query.Contains("goods"))
            {
                return QueryType.ProductList;
            }

            return QueryType.General;
        }

        private async Task<string> GetGeneralContext()
        {
            var allProducts = await _sanPhamRepository.GetAllWithDetailsAsync();
            var categories = await _danhMucRepository.GetAllAsync();
            var totalOrders = (await _donHangRepository.GetAllAsync()).Count();

            return $@"
Context information about the e-commerce seafood system:
- Total products in system: {allProducts.Count()}
- Product categories: {string.Join(", ", categories.Select(c => c.TenDanhMuc))}
- Total orders: {totalOrders}
- Some popular products: {string.Join(", ", allProducts.Take(5).Select(p => p.TenSanPham))}
";
        }

        private async Task<string> GetProductListContext()
        {
            try
            {
                var allProducts = await _sanPhamRepository.GetAllWithDetailsAsync();
                var categories = await _danhMucRepository.GetAllAsync();
                
                // Danh sách sản phẩm cơ bản
                var topProducts = allProducts
                    .OrderByDescending(p => p.ChiTietDonHangs != null && p.ChiTietDonHangs.Any()
                         ? p.ChiTietDonHangs.Sum(ct => ct.SoLuong)
                         : 0)
                    .Take(10)
                    .Select(p => new {
                        p.TenSanPham,
                        p.GiaTheoKG,
                        NguonGoc = p.NguonGoc ?? "Không rõ",
                        Category = p.DanhMuc?.TenDanhMuc ?? "Không phân loại",
                        Rating = p.DanhGias != null && p.DanhGias.Any() ? p.DanhGias.Average(d => d.XepHang) : 0,
                        Stock = p.SoLuong > 0 ? $"Còn hàng ({p.SoLuong} kg)" : "Hết hàng",
                        SaleCount = p.ChiTietDonHangs != null && p.ChiTietDonHangs.Any() ? p.ChiTietDonHangs.Sum(ct => ct.SoLuong) : 0
                    }).ToList();
                
                // Sản phẩm mới nhất (dựa trên ngày thu hoạch)
                var freshestProducts = allProducts
                    .Where(p => p.NgayThuHoach != default(DateTime) && p.SoLuong > 0)
                    .OrderByDescending(p => p.NgayThuHoach)
                    .Take(5)
                    .Select(p => $"- {p.TenSanPham} - Thu hoạch ngày {p.NgayThuHoach.ToString("dd/MM/yyyy")} - {p.GiaTheoKG:N0} VNĐ/kg");
                
                // Sản phẩm theo mùa hiện tại
                var currentMonth = DateTime.Now.Month;
                List<string> seasonalProducts;
                
                try
                {
                    seasonalProducts = allProducts
                        .Where(p => p.ChiTietMuas != null && p.ChiTietMuas.Any() &&
                                    p.ChiTietMuas.Any(cm => cm.Mua != null &&
                                   (cm.Mua.ThoiGianVaoMua.Month <= currentMonth && currentMonth <= cm.Mua.ThoiGianHetMua.Month ||
                                   cm.Mua.ThoiGianVaoMua.Month > cm.Mua.ThoiGianHetMua.Month &&
                                    (currentMonth >= cm.Mua.ThoiGianVaoMua.Month || currentMonth <= cm.Mua.ThoiGianHetMua.Month))))
                        .Take(5)
                        .Select(p => $"- {p.TenSanPham} - {p.GiaTheoKG:N0} VNĐ/kg")
                        .ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi lấy sản phẩm theo mùa: {ex.Message}");
                    seasonalProducts = new List<string> { "Không thể lấy thông tin sản phẩm theo mùa" };
                }
                
                // Phân loại sản phẩm theo danh mục
                var productsByCategory = new List<string>();
                foreach (var category in categories.Take(5))
                {
                    try
                    {
                        var productsInCategory = allProducts
                            .Where(p => p.DanhMucID == category.MaDanhMuc)
                            .Take(3)
                            .Select(p => p.TenSanPham);
                        
                        if (productsInCategory.Any())
                        {
                            productsByCategory.Add($"- {category.TenDanhMuc}: {string.Join(", ", productsInCategory)}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi khi lấy sản phẩm theo danh mục {category.TenDanhMuc}: {ex.Message}");
                        continue;
                    }
                }
                
                // Thống kê giá
                decimal minPrice = 0, maxPrice = 0, avgPrice = 0;
                try
                {
                    if (allProducts.Any())
                    {
                        minPrice = allProducts.Min(p => p.GiaTheoKG);
                        maxPrice = allProducts.Max(p => p.GiaTheoKG);
                        avgPrice = allProducts.Average(p => p.GiaTheoKG);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi tính thống kê giá: {ex.Message}");
                }
                
                // Tạo phản hồi
                var response = $@"
=== THÔNG TIN SẢN PHẨM HẢI SẢN ===

Tổng số sản phẩm: {allProducts.Count()}
Sản phẩm có sẵn: {allProducts.Count(p => p.SoLuong > 0)}
Phạm vi giá: {minPrice:N0} - {maxPrice:N0} VNĐ/kg (Trung bình: {avgPrice:N0} VNĐ/kg)

----- SẢN PHẨM NỔI BẬT -----
{string.Join("\n", topProducts.Select((p, i) => $"{i+1}. {p.TenSanPham} - {p.GiaTheoKG:N0} VNĐ/kg - Đánh giá: {p.Rating:F1}/5 - Đã bán: {p.SaleCount} kg - {p.Stock}"))}

----- SẢN PHẨM TƯƠI NHẤT -----
{(freshestProducts.Any() ? string.Join("\n", freshestProducts) : "Không có thông tin sản phẩm tươi")}

----- SẢN PHẨM THEO MÙA -----
{(seasonalProducts.Any() ? string.Join("\n", seasonalProducts) : "Không có sản phẩm theo mùa hiện tại")}

----- SẢN PHẨM THEO DANH MỤC -----
{string.Join("\n", productsByCategory)}

Bạn có thể yêu cầu thông tin chi tiết về bất kỳ sản phẩm cụ thể nào.
";
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong GetProductListContext: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return "Xin lỗi, không thể lấy danh sách sản phẩm do lỗi hệ thống. Chi tiết: " + ex.Message;
            }
        }

        private async Task<string> GetProductDetailContext(string query)
        {
            var allProducts = await _sanPhamRepository.GetAllWithDetailsAsync();
            
            // Thử tìm sản phẩm cụ thể được đề cập trong truy vấn
            string productNameInQuery = ExtractProductName(query);
            var matchingProducts = allProducts
                .Where(p => p.TenSanPham.ToLower().Contains(productNameInQuery) || 
                           (p.MoTa != null && p.MoTa.ToLower().Contains(productNameInQuery)))
                .Take(3)
                .ToList();

            if (matchingProducts.Any() && !string.IsNullOrEmpty(productNameInQuery))
            {
                var productsDetailedInfo = new List<string>();
                
                foreach (var product in matchingProducts)
                {
                    // Tổng hợp đánh giá
                    double averageRating = 0;
                    int ratingCount = 0;
                    if (product.DanhGias != null && product.DanhGias.Any())
                    {
                        averageRating = product.DanhGias.Average(d => d.XepHang);
                        ratingCount = product.DanhGias.Count();
                    }
                    
                    // Số lượng đã bán
                    int totalSold = 0;
                    if (product.ChiTietDonHangs != null && product.ChiTietDonHangs.Any())
                    {
                        totalSold = product.ChiTietDonHangs.Sum(ct => ct.SoLuong);
                    }
                    
                    // Phân loại theo mùa
                    string seasonInfo = "Không có thông tin mùa vụ";
                    if (product.ChiTietMuas != null && product.ChiTietMuas.Any())
                    {
                        var seasons = product.ChiTietMuas.Select(cm => cm.Mua?.TenMua).Where(m => m != null);
                        seasonInfo = seasons.Any() ? string.Join(", ", seasons) : "Không có thông tin mùa vụ";
                    }
                    
                    // Thông tin chi tiết về chất lượng và bảo quản
                    string preservationInfo = string.IsNullOrEmpty(product.LoaiBaoQuan) ? 
                        "Không có thông tin bảo quản" : product.LoaiBaoQuan;
                    
                    // Tính ngày từ lúc thu hoạch
                    string harvestInfo = "Không có thông tin ngày thu hoạch";
                    if (product.NgayThuHoach != default(DateTime))
                    {
                        int daysFromHarvest = (DateTime.Now - product.NgayThuHoach).Days;
                        harvestInfo = $"Thu hoạch ngày {product.NgayThuHoach.ToString("dd/MM/yyyy")} ({daysFromHarvest} ngày trước)";
                    }
                    
                    // Thông tin kho hàng
                    string warehouseInfo = "Không có thông tin kho hàng";
                    if (product.ChiTietKhoHangs != null && product.ChiTietKhoHangs.Any())
                    {
                        var warehouses = product.ChiTietKhoHangs
                            .Select(ck => ck.NhaKho?.TenNhaKho)
                            .Where(w => w != null);
                        warehouseInfo = warehouses.Any() ? 
                            $"Được lưu trữ tại: {string.Join(", ", warehouses)}" : 
                            "Không có thông tin kho hàng";
                    }
                    
                    // Thông tin hình ảnh
                    string imageInfo = "Không có hình ảnh bổ sung";
                    if (product.HinhAnhSanPhams != null && product.HinhAnhSanPhams.Any())
                    {
                        imageInfo = $"Có {product.HinhAnhSanPhams.Count} hình ảnh bổ sung";
                    }
                    
                    // Độ tươi của sản phẩm
                    string freshnessInfo = "Độ tươi không xác định";
                    if (product.NgayThuHoach != default(DateTime))
                    {
                        int daysFromHarvest = (DateTime.Now - product.NgayThuHoach).Days;
                        if (daysFromHarvest <= 1)
                            freshnessInfo = "Cực kỳ tươi (mới thu hoạch trong 24h)";
                        else if (daysFromHarvest <= 3)
                            freshnessInfo = "Rất tươi (thu hoạch trong 3 ngày)";
                        else if (daysFromHarvest <= 7)
                            freshnessInfo = "Tươi (thu hoạch trong tuần)";
                        else if (daysFromHarvest <= 14)
                            freshnessInfo = "Độ tươi trung bình (thu hoạch trong 2 tuần)";
                        else
                            freshnessInfo = "Đã được bảo quản lâu (trên 2 tuần)";
                    }

                    var detailedInfo = $@"
===== CHI TIẾT SẢN PHẨM =====
Tên sản phẩm: {product.TenSanPham}
Giá: {product.GiaTheoKG:N0} VNĐ/kg
Danh mục: {(product.DanhMuc != null ? product.DanhMuc.TenDanhMuc : "Không phân loại")}
Loại sản phẩm: {(product.LoaiSanPham != null ? product.LoaiSanPham.TenLoai : "Không xác định")}
Xuất xứ: {product.NguonGoc ?? "Không có thông tin"}
Tình trạng: {(product.SoLuong > 0 ? $"Còn hàng ({product.SoLuong} kg)" : "Hết hàng")}
Mùa vụ: {seasonInfo}

----- THÔNG TIN CHẤT LƯỢNG -----
{freshnessInfo}
{harvestInfo}
Phương pháp bảo quản: {preservationInfo}
{warehouseInfo}

----- MÔ TẢ SẢN PHẨM -----
{product.MoTa ?? "Không có mô tả chi tiết"}

----- THÔNG TIN BỔ SUNG -----
Số lượng đã bán: {totalSold} kg
Đánh giá: {averageRating:F1}/5 ({ratingCount} đánh giá)
{imageInfo}
";

                    productsDetailedInfo.Add(detailedInfo);
                }

                return $@"
Thông tin chi tiết về sản phẩm '{productNameInQuery}':

{string.Join("\n\n", productsDetailedInfo)}

Tìm thấy {matchingProducts.Count} sản phẩm phù hợp với truy vấn của bạn về '{productNameInQuery}'.
";
            }
            else
            {
                // Nếu không tìm thấy sản phẩm cụ thể, cung cấp danh sách chung
                return await GetProductListContext();
            }
        }

        private async Task<string> GetCategoryContext()
        {
            var categories = await _danhMucRepository.GetAllAsync();
            var allProducts = await _sanPhamRepository.GetAllWithDetailsAsync();
            
            var categoryDetails = new List<string>();
            foreach (var category in categories)
            {
                var productsInCategory = allProducts.Where(p => p.DanhMucID == category.MaDanhMuc).Count();
                categoryDetails.Add($"- {category.TenDanhMuc}: {productsInCategory} sản phẩm");
            }

            return $@"
Category information from the seafood e-commerce system:

Total categories: {categories.Count()}

Categories and product counts:
{string.Join("\n", categoryDetails)}
";
        }

        private async Task<string> GetPriceContext(string query)
        {
            var allProducts = await _sanPhamRepository.GetAllWithDetailsAsync();
            
            // Thử tìm sản phẩm cụ thể được đề cập trong truy vấn về giá
            string productNameInQuery = ExtractProductName(query);
            var matchingProducts = allProducts
                .Where(p => p.TenSanPham.ToLower().Contains(productNameInQuery) || 
                           (p.MoTa != null && p.MoTa.ToLower().Contains(productNameInQuery)))
                .Take(5)
                .ToList();

            if (matchingProducts.Any() && !string.IsNullOrEmpty(productNameInQuery))
            {
                var priceInfo = matchingProducts.Select(p => $"- {p.TenSanPham}: {p.GiaTheoKG:N0} VNĐ/kg");

                return $@"
Price information for products matching '{productNameInQuery}':

{string.Join("\n", priceInfo)}

Price range: {matchingProducts.Min(p => p.GiaTheoKG):N0} VNĐ to {matchingProducts.Max(p => p.GiaTheoKG):N0} VNĐ per kg
";
            }
            else
            {
                // Nếu không tìm thấy sản phẩm cụ thể, cung cấp thông tin giá chung
                var priceRanges = allProducts
                    .GroupBy(p => p.DanhMuc?.TenDanhMuc ?? "Không có danh mục")
                    .Select(g => new {
                        Category = g.Key,
                        MinPrice = g.Min(p => p.GiaTheoKG),
                        MaxPrice = g.Max(p => p.GiaTheoKG),
                        AvgPrice = g.Average(p => p.GiaTheoKG)
                    })
                    .ToList();

                return $@"
Price information from the seafood e-commerce system:

Price ranges by category:
{string.Join("\n", priceRanges.Select(pr => $"- {pr.Category}: {pr.MinPrice:N0} VNĐ to {pr.MaxPrice:N0} VNĐ per kg (Average: {pr.AvgPrice:N0} VNĐ)"))}

Overall price range: {allProducts.Min(p => p.GiaTheoKG):N0} VNĐ to {allProducts.Max(p => p.GiaTheoKG):N0} VNĐ per kg
";
            }
        }

        private async Task<string> GetOrderContext()
        {
            var orders = await _donHangRepository.GetAllAsync();
            
            // Truy cập trạng thái đơn hàng thông qua ChiTietTrangThaiDonHangs
            var orderStats = new {
                TotalOrders = orders.Count(),
                
                // Thay vì truy cập trực tiếp TrangThaiDonHang, chúng ta sử dụng ChiTietTrangThaiDonHangs
                CompletedOrders = orders.Count(o => o.ChiTietTrangThaiDonHangs != null && 
                    o.ChiTietTrangThaiDonHangs.Any(ct => ct.TrangThaiDonHang.TenTrangThai == "Đã giao hàng")),
                
                PendingOrders = orders.Count(o => o.ChiTietTrangThaiDonHangs != null && 
                    o.ChiTietTrangThaiDonHangs.Any(ct => ct.TrangThaiDonHang.TenTrangThai == "Đang xử lý")),
                
                CanceledOrders = orders.Count(o => o.ChiTietTrangThaiDonHangs != null && 
                    o.ChiTietTrangThaiDonHangs.Any(ct => ct.TrangThaiDonHang.TenTrangThai == "Đã hủy")),
                
                // Sử dụng TongSoTien thay vì TongTien
                AverageOrderValue = orders.Any() ? orders.Average(o => o.TongSoTien) : 0
            };

            return $@"
Order information from the seafood e-commerce system:

Order statistics:
- Total orders: {orderStats.TotalOrders}
- Completed orders: {orderStats.CompletedOrders}
- Pending orders: {orderStats.PendingOrders}
- Canceled orders: {orderStats.CanceledOrders}
- Average order value: {orderStats.AverageOrderValue:N0} VNĐ
";
        }

        // Hàm trích xuất tên sản phẩm từ truy vấn
        private string ExtractProductName(string query)
        {
            query = query.ToLower();
            
            // Danh sách các từ khóa để xóa
            var wordsToRemove = new[] {
                // Từ khóa về sản phẩm
                "sản phẩm", "product", "chi tiết", "detail", "giá", "price",
                "thông tin", "information", "về", "about", "cho", "for",
                "của", "của sản phẩm", "of", "of product", "tôi", "i", "muốn", "want",
                "biết", "know", "mô tả", "description", "là gì", "what is", "bao nhiêu", "how much",
                "hiện có", "available", "giá bán", "selling price", "?", ",", ".", "!", "có", "has",
                
                // Từ khóa bổ sung
                "cho tôi", "tell me", "nói cho tôi", "hãy", "làm ơn", "please",
                "như thế nào", "how", "chất lượng", "quality", "đặc điểm", "features",
                "còn", "hết", "xuất xứ", "origin", "nguồn gốc", "source",
                "in stock", "out of stock", "khi nào", "when",
                
                // Từ khóa về thời gian
                "ngày", "day", "tháng", "month", "hôm nay", "today", "hôm qua", "yesterday",
                "tuần", "week", "vừa", "mới", "recently", "gần đây", "latest", "mới nhất",
                
                // Từ khóa số lượng
                "tất cả", "all", "một số", "some", "vài", "a few", "nhiều", "many",
                "số lượng", "quantity", "cái", "con", "kg", "kilogram",
                
                // Từ khóa so sánh
                "tốt nhất", "best", "rẻ nhất", "cheapest", "đắt nhất", "most expensive",
                "phổ biến nhất", "most popular", "bán chạy nhất", "bestselling",
                "ngon nhất", "tastiest", "tươi nhất", "freshest", "mới nhất", "newest",
                
                // Từ nối
                "và", "and", "hoặc", "or", "cùng", "with", "từ", "from"
            };
            
            // Tìm các sản phẩm hải sản phổ biến và đánh dấu chúng để giữ lại
            var seafoodProductNames = new[] {
                "cá", "fish", "tôm", "shrimp", "cua", "crab", "ghẹ", "blue swimmer crab",
                "mực", "squid", "bạch tuộc", "octopus", "cá hồi", "salmon", "cá thu", "mackerel",
                "cá ngừ", "tuna", "cá trích", "herring", "cá chép", "carp", "cá lóc", "snakehead",
                "cá trê", "catfish", "cá rô", "anabas", "cá nục", "scad", "tôm hùm", "lobster",
                "tôm sú", "black tiger shrimp", "tôm thẻ", "white leg shrimp", "sò", "clam",
                "hào", "oyster", "vẹm", "mussel", "ốc", "snail", "sứa", "jellyfish",
                "rong biển", "seaweed", "cá chim", "pomfret", "cá diêu hồng", "red tilapia"
            };
            
            // Đánh dấu từ khóa tên sản phẩm để giữ lại
            foreach (var productName in seafoodProductNames)
            {
                if (query.Contains(productName))
                {
                    // Thay thế từ khóa với một mã đặc biệt để giữ lại
                    query = query.Replace(productName, $"__KEEP__{productName}__KEEP__");
                }
            }
            
            // Xóa các từ khóa để giữ lại tên sản phẩm
            foreach (var word in wordsToRemove)
            {
                query = query.Replace(word, " ");
            }
            
            // Khôi phục các từ khóa tên sản phẩm đã được đánh dấu
            query = query.Replace("__KEEP__", "");
            
            // Loại bỏ các khoảng trắng dư thừa
            query = string.Join(" ", query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            
            // Nếu kết quả rỗng, trả về một chuỗi mặc định
            return string.IsNullOrWhiteSpace(query) ? "hải sản" : query.Trim();
        }

        // Enum phân loại loại truy vấn
        private enum QueryType
        {
            General,
            ProductList,
            ProductDetail,
            CategoryInfo,
            PriceInfo,
            OrderInfo
        }
    }
} 