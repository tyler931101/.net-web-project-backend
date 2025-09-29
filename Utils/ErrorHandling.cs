namespace backend.Utils
{
    public static class ErrorHandling
    {
        public static string FormatError(string message, string source = "")
        {
            return $"Error: {message}, Source: {source}";
        }
    }
}