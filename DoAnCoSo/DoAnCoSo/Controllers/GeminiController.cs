using DoAnCoSo.Extensions;
using DoAnCoSo.Models;
using DoAnCoSo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace DoAnCoSo.Controllers
{
    public class GeminiController : Controller
    {
        private readonly IGeminiService _geminiService;
        private const string SessionKeyMessages = "_GeminiMessages";

        public GeminiController(IGeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        public IActionResult Index()
        {
            var messages = HttpContext.Session.GetObjectFromJson<List<ChatMessage>>(SessionKeyMessages) ?? new List<ChatMessage>();
            
            var viewModel = new ChatViewModel
            {
                Messages = messages
            };
            
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ChatViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.UserMessage))
            {
                return RedirectToAction(nameof(Index));
            }

            // Get existing messages from session or create new list
            var messages = HttpContext.Session.GetObjectFromJson<List<ChatMessage>>(SessionKeyMessages) ?? new List<ChatMessage>();

            // Add user message
            var userMessage = new ChatMessage
            {
                Role = "user",
                Content = model.UserMessage
            };
            messages.Add(userMessage);

            // Kiểm tra nếu người dùng đang hỏi về dữ liệu hệ thống
            string response;
            if (IsDataQuery(model.UserMessage))
            {
                // Sử dụng phương thức mới có nhận thức về dữ liệu
                response = await _geminiService.GenerateDataAwareResponseAsync(model.UserMessage);
            }
            else
            {
                // Sử dụng phương thức thông thường
                response = await _geminiService.GenerateChatResponseAsync(messages);
            }

            // Add AI response
            var aiMessage = new ChatMessage
            {
                Role = "model",
                Content = response
            };
            messages.Add(aiMessage);

            // Save updated messages to session
            HttpContext.Session.SetObjectAsJson(SessionKeyMessages, messages);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ClearChat()
        {
            HttpContext.Session.Remove(SessionKeyMessages);
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        [Route("api/gemini/chat")]
        public async Task<IActionResult> ChatAPI([FromBody] ChatApiRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new { response = "Tin nhắn không được để trống." });
            }

            // Lấy lịch sử chat từ session hoặc tạo mới
            var chatHistory = HttpContext.Session.GetObjectFromJson<List<ChatMessage>>("_WidgetChatHistory") ?? new List<ChatMessage>();
            
            // Thêm tin nhắn của người dùng
            var userMessage = new ChatMessage
            {
                Role = "user",
                Content = request.Message
            };
            chatHistory.Add(userMessage);

            // Kiểm tra nếu người dùng đang hỏi về dữ liệu hệ thống
            string response;
            if (IsDataQuery(request.Message))
            {
                // Sử dụng phương thức có nhận thức về dữ liệu
                response = await _geminiService.GenerateDataAwareResponseAsync(request.Message);
            }
            else
            {
                // Sử dụng phương thức chat với lịch sử
                response = await _geminiService.GenerateChatResponseAsync(chatHistory);
            }

            // Thêm phản hồi của AI vào lịch sử
            var aiMessage = new ChatMessage
            {
                Role = "model",
                Content = response
            };
            chatHistory.Add(aiMessage);
            
            // Giới hạn lịch sử chat (tối đa 20 tin nhắn để tránh quá lớn)
            if (chatHistory.Count > 20)
            {
                chatHistory = chatHistory.Skip(chatHistory.Count - 20).ToList();
            }

            // Lưu lịch sử chat vào session
            HttpContext.Session.SetObjectAsJson("_WidgetChatHistory", chatHistory);

            return Json(new { response = response });
        }
        
        [HttpGet]
        [Route("api/gemini/history")]
        public IActionResult GetChatHistory()
        {
            var chatHistory = HttpContext.Session.GetObjectFromJson<List<ChatMessage>>("_WidgetChatHistory") ?? new List<ChatMessage>();
            return Json(chatHistory);
        }
        
        [HttpPost]
        [Route("api/gemini/clear")]
        public IActionResult ClearChatHistory()
        {
            HttpContext.Session.Remove("_WidgetChatHistory");
            return Json(new { success = true });
        }
        
        // Phương thức để xác định xem câu hỏi có liên quan đến dữ liệu hệ thống hay không
        private bool IsDataQuery(string query)
        {
            // Chuyển câu hỏi sang chữ thường để dễ so sánh
            query = query.ToLower();
            
            // Các từ khóa liên quan đến dữ liệu hệ thống
            var dataKeywords = new[] {
                // Sản phẩm
                "sản phẩm", "product", "hàng hóa", "goods", "mặt hàng", "item",
                
                // Danh mục
                "danh mục", "category", "loại", "type", "phân loại", "classification",
                
                // Giá cả
                "giá", "price", "cost", "bao nhiêu", "how much", "giá bán", "selling price",
                "giá cả", "pricing", "tiền", "money", "đắt", "expensive", "rẻ", "cheap",
                
                // Đơn hàng
                "đơn hàng", "order", "mua hàng", "purchase", "giao hàng", "delivery",
                
                // Số lượng
                "số lượng", "quantity", "amount", "how many", "còn bao nhiêu", "available",
                "tồn kho", "stock", "inventory", "hết hàng", "out of stock", "còn hàng", "in stock",
                
                // Thông tin chi tiết sản phẩm
                "chi tiết", "detail", "mô tả", "description", "thông tin", "information",
                "đặc điểm", "features", "tính năng", "characteristics", "xuất xứ", "origin",
                
                // Thông tin doanh nghiệp
                "công ty", "company", "doanh nghiệp", "business", "cửa hàng", "store",
                "liên hệ", "contact", "hotline", "email", "địa chỉ", "address",
                
                // Từ khóa tổng hợp/thống kê
                "thống kê", "statistics", "tổng số", "total", "bán chạy", "bestseller",
                "popular", "phổ biến", "top", "best", "tốt nhất"
            };
            
            // Kiểm tra nếu có bất kỳ từ khóa nào xuất hiện trong câu hỏi
            return dataKeywords.Any(keyword => query.Contains(keyword));
        }
    }
    
    public class ChatApiRequest
    {
        public string Message { get; set; }
    }
} 