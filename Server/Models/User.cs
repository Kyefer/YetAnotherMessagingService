using System;
using System.Text;

namespace WebApplication.Models
{
    public class User
    {
        public string username { get; private set;}
        public string password { get; private set;}


        public User()
        {

        }
        public User(string username, string password)
        {
            this.username = username;
            this.password = HashPassword(password);
        }

        public bool PasswordMatches(string password)
        {
            return HashPassword(password).Equals(this.password);
        }

        private static string HashPassword(string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            var hashBytes = System.Security.Cryptography.SHA256.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }


    }
}