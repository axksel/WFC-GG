namespace UE.Email
{
    /// <summary>
    /// This class offers some helper functions for HTML tags.
    /// </summary>
    public static class HTML
    {
        public static string Br => "<br>";
        public static string P => "<P>";
        
        public static string Bold(string text)
        {
            return "<b>" + text + "</b>";
        }
        
        public static string Italic(string text)
        {
            return "<i>" + text + "</i>";
        }
        
        public static string Underline(string text)
        {
            return "<u>" + text + "</u>";
        }
        
        public static string Par(string text)
        {
            return "<p>" + text + "</p>";
        }
    }
}