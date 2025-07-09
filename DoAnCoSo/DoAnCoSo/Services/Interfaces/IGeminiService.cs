using DoAnCoSo.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DoAnCoSo.Services.Interfaces
{
    public interface IGeminiService
    {
        Task<string> GenerateContentAsync(string prompt);
        Task<string> GenerateChatResponseAsync(List<ChatMessage> messages);
        Task<string> GenerateDataAwareResponseAsync(string userQuery);
    }
} 