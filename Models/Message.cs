using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{

    public class Message
    {
        public string sender { set; get; }

        public string content { set; get; }

        public long timestamp { set; get; }

        [NotMappedAttribute]
        public bool? tagged { set; get; }


        public Message() { }

        public Message(Message other){
            this.sender = other.sender;
            this.content = other.content;
            this.timestamp = other.timestamp;
        }
    }
}