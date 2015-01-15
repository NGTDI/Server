
namespace Site.JsonObjects
{
    public class LoginResult
    {
        public string Result { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string AuthorizationToken { get; set; }
        public string NeedsEula { get; set; }
        public string EulaGuid { get; set; }
        public string EulaText { get; set; }
    }
}