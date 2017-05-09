using Microsoft.EntityFrameworkCore;

namespace WebApplication.Models
{
    // public class ModelContext : DbContext
    // {
    //     public DbSet<Message> Messages { get; set; }
    //     public DbSet<User> Users { get; set; }


    //     protected override void OnModelCreating(ModelBuilder modelBuilder)
    //     {
    //         modelBuilder.Entity<Message>().HasKey("timestamp", "sender");
    //         modelBuilder.Entity<User>().HasKey("username");
    //         base.OnModelCreating(modelBuilder);
    //     }
    //     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     {
    //         optionsBuilder.UseSqlite("Data Source=messaging.db");
    //     }
    // }

    public class Model
    {
        private static Model instance;

        public delegate void MessageHandler(Message m);

        public event MessageHandler NewClientMessageChange;

        public event MessageHandler SendToAllServersChange;

        private Model()
        {

        }

        public static Model getInstance()
        {
            if (instance == null)
                instance = new Model();
            return instance;
        }

        public void NewClientMessage(Message m)
        {
            NewClientMessageChange?.Invoke(m);
            SendToAllServersChange?.Invoke(m);
        }

        public void NewServerMessage(Message m)
        {
            NewClientMessageChange?.Invoke(m);
        }

    }
}