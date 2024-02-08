using SeriLog.LogSanitizingFormatter;

namespace WebApplicationELK.Infrastructure
{
    public class DemoSanitzingRule : ISanitizingFormatRule
    {
        public string Sanitize(string content)
        {
            return content.ToLower().Replace("cat", "dog");
        }
    }
}
