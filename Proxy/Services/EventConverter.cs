using System;
using WebApplication.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApplication {
        class EventConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Event));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            var type = ((JValue)obj["type"]).ToObject<EventType>(serializer);

            if (type == EventType.MESSAGE_FETCH)
            {
                return new Event(type, null);
            }


            var dataObj = (JObject)obj["data"];

            dynamic data;

            switch (type)
            {
                case EventType.NEW_MESSAGE:
                    data = dataObj.ToObject<Message>(serializer);
                    break;
                case EventType.REGISTER:
                case EventType.AUTHENTICATION:
                    data = dataObj.ToObject<Autentication>(serializer);
                    break;
                default:
                    data = dataObj.ToObject<dynamic>(serializer);
                    break;
            }

            return new Event(type, data);

        }

        public override bool CanWrite { get { return false; } }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}