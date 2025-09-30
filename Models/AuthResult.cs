namespace backend.Models
{
    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        // ✅ Additional fields for frontend
        public List<string>? Errors { get; set; }
        public string? UserId { get; set; }

        public AuthResult(bool isSuccess, string message, string? userId = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            UserId = userId;
        }

        public AuthResult() { }
    }
}