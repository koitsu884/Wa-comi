namespace Wacomi.API.Helper
{
    public class AuthMessageSenderOptions
    {
        public string SMTPServer { get; set;}
        public int SMTPPort { get; set;}
        public string UserName{get; set;}
        public string Password{ get; set;}
        public string ApiKey { get; set;}
    }
}