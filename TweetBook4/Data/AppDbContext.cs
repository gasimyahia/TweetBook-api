using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Domain;

namespace TweetBook4.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PostTag>().HasKey(x => new { x.PostId, x.TagId });
            builder.Entity<PostTag>().HasOne(pt => pt.post).WithMany(p => p.PostTags).HasForeignKey(pt => pt.PostId);
            builder.Entity<PostTag>().HasOne(pt => pt.tag).WithMany(p => p.PostTags).HasForeignKey(pt => pt.TagId);
        }
    }
}
