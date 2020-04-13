using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Realist.Data.Model;

namespace Realist.Data.Services
{
  public  class DataContext:IdentityDbContext<User>
    {
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public  virtual  DbSet<Reply> Replies { get; set; }
        public  virtual DbSet<Photo> Photos { get; set; }
        public  virtual  DbSet<Videos> Videos { get; set; }
        public  virtual  DbSet<UserInfo> UserInfo { get; set; }
        public  virtual  DbSet<BotInfo> BotInfo { get; set; }

        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        base.OnModelCreating(builder);
        builder.Entity<User>(opt =>
        {
            opt.HasKey(o => o.Id);
            opt.HasMany(o => o.Photo)
                .WithOne()
                .HasForeignKey(r => r.UserId);
            opt.HasMany(o => o.Videos)
                .WithOne()
                .HasForeignKey(r => r.UserId)
               
                ;

        });
        builder.Entity<Post>(opt =>
        {
            opt.HasKey(o => o.Id);
            opt.HasMany(o => o.Comments)
                .WithOne()
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            opt.HasOne(o => o.User).WithMany(o => o.Posts)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        });
        builder.Entity<Comment>(opt =>
        {
            opt.HasMany(o => o.Replies)
                .WithOne()
                .HasForeignKey(r => r.CommentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<Reply>(opt =>
        {
            opt.HasOne(o => o.Comment)
                .WithMany(o => o.Replies)
                .HasForeignKey(r => r.CommentId).OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<UserInfo>(opt =>
        {
            opt.HasKey(o => o.Id);
            opt.HasOne<User>()
                .WithMany().HasForeignKey(o => o.UserId);

        });
        builder.Entity<Post>(opt =>
        {
            opt.HasMany(o => o.Photos)
                .WithOne().HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            opt.HasMany(o => o.Videos)
                .WithOne()
                .HasForeignKey(o => o.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        });
     
        builder.Entity<Videos>(opt =>
        {
            opt.HasKey(o => o.Id);
        });
        builder.Entity<Photo>(opt => opt.HasKey(o => o.Id));
        builder.Entity<UserInfo>(opt =>
        {
            opt.HasKey(r => r.Id);
            opt.HasOne<User>()
                .WithMany(o => o.UserInfos)
                .HasForeignKey(r=>r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        });
        builder.Entity<BotInfo>(opt => { opt.HasKey(o => o.Id); });
        }
    }
}
