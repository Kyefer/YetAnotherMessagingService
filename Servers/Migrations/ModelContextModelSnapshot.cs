using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WebApplication.Models;

namespace Project3.Migrations
{
    [DbContext(typeof(ModelContext))]
    partial class ModelContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("WebApplication.Models.Message", b =>
                {
                    b.Property<long>("timestamp");

                    b.Property<string>("sender");

                    b.Property<string>("content");

                    b.HasKey("timestamp", "sender");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("WebApplication.Models.User", b =>
                {
                    b.Property<string>("username");

                    b.Property<string>("password");

                    b.HasKey("username");

                    b.ToTable("Users");
                });
        }
    }
}
