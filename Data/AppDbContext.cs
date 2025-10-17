using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BeatBox.Areas.Media.Models;
using BeatBox.Areas.Admin.Models;

namespace BeatBox.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<MediaElement> Medias { get; set; } = null!;
        public DbSet<MediaType> MediaTypes { get; set; } = null!;
        public DbSet<UserFavorites> UserFavorites { get; set; } = null!;
        public DbSet<CloudinaryUploads> cloudinaryUploads { get; set; }
        public DbSet<ListeningHistory> ListeningHistory { get; set; }


        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }
        

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CloudinaryUploads>().HasKey(x => x.Id);

            modelBuilder.Entity<MediaElement>()
                .HasOne(m => m.User)
                .WithMany(u => u.Medias)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
        
    }
}