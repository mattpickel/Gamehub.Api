namespace Gamehub.Api.Models
{
    public class WebhookPayload
    {
        public UserData data { get; set; }
    }

    public class UserData
    {
        public string username { get; set; }
        public string id { get; set; }
    }
}