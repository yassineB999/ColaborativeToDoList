using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CollaborativeToDoList.Models
{
    public class Utils
    {
       public string GenerateSharedUrl(string baseUrl, string title)
{
    var sanitizedTitle = SanitizeTitle(title);
    var uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);
    return $"{baseUrl}/Home/AccessSharedUrl?sharedUrl={sanitizedTitle}-{uniqueId}";
}   

        private string SanitizeTitle(string title)
        {
            var sanitized = Regex.Replace(title, @"[^a-zA-Z0-9\-]", string.Empty)
                                 .Replace(" ", "-")
                                 .ToLower();
            return sanitized;
        }
    }
}
