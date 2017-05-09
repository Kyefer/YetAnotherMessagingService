using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebApplication.Models
{
    [JsonConverter(typeof(EventConverter))]
    public class Event
    {
        public EventType type { get; set; }
        public dynamic data { get; set; }

        public Event(EventType type, dynamic data)
        {
            this.type = type;
            this.data = data;
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EventType
    {
        NEW_MESSAGE, MESSAGE_RELAY, AUTHENTICATION, AUTHENTICATION_RESULT, REGISTER, REGISTER_RESULT, MESSAGE_FETCH, MESSAGE_RETURN
    }

    public class RegisterResult
    {
        public bool success { get; set; }
        public string reason { get; set; }

        public RegisterResult(bool success, string reason)
        {
            this.success = success;
            this.reason = reason;
        }

        public RegisterResult(bool success)
        {
            this.success = success;
        }
    }

    public class Autentication
    {
        public string username { get; set; }

        public string password { get; set; }
    }
}