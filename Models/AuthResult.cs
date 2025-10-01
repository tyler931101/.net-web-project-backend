namespace backend.Models
{
    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        public List<string>? Errors { get; set; }
        public string? UserId { get; set; }

        // ✅ Add JWT Token
        public string? Token { get; set; }

        public AuthResult(bool isSuccess, string message, string? userId = null, string? token = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            UserId = userId;
            Token = token;
        }

        public AuthResult() { }
    }
}