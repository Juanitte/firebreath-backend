using FireBreath.PostsMicroservice.Models.Dtos.EntityDto;
using FireBreath.PostsMicroservice.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FireBreath.PostsMicroservice.Models.Context
{
    public class PostsDbContext : DbContext
    {
        #region DBSets

        public DbSet<Post> PostsDb { get; set; }
        public DbSet<Message> MessagesDb { get; set; }
        public DbSet<Attachment> AttachmentsDb { get; set; }
        public DbSet<Like> LikesDb { get; set; }

        #endregion

        public PostsDbContext(DbContextOptions options)
        : base(options)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="modelBuilder"><see cref="ModelBuilder"/></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Deshabilita la eliminación en cascada
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Message>().ToTable("Messages");
            modelBuilder.Entity<Attachment>().ToTable("Attachments");
            modelBuilder.Entity<Like>().ToTable("Likes");
            modelBuilder.Entity<Share>().ToTable("Shares");

            modelBuilder.Entity<Attachment>()
                .HasOne(t => t.Post)
                .WithMany(m => m.AttachmentPaths)
                .HasForeignKey(m => m.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Attachment>()
                .HasOne(t => t.Message)
                .WithMany(m => m.AttachmentPaths)
                .HasForeignKey(t => t.MessageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
