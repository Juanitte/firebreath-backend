﻿// <auto-generated />
using System;
using FireBreath.PostsMicroservice.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FireBreath.PostsMicroservice.Migrations
{
    [DbContext(typeof(PostsDbContext))]
    [Migration("20250312033056_db5")]
    partial class db5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FireBreath.PostsMicroservice.Models.Entities.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("MessageId")
                        .HasColumnType("int");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PostId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.HasIndex("PostId");

                    b.ToTable("Attachments", (string)null);
                });

            modelBuilder.Entity("FireBreath.PostsMicroservice.Models.Entities.Like", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "PostId");

                    b.ToTable("Likes", (string)null);
                });

            modelBuilder.Entity("FireBreath.PostsMicroservice.Models.Entities.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Messages", (string)null);
                });

            modelBuilder.Entity("FireBreath.PostsMicroservice.Models.Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AuthorAvatar")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AuthorTag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Posts", (string)null);
                });

            modelBuilder.Entity("FireBreath.PostsMicroservice.Models.Entities.Share", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "PostId");

                    b.ToTable("Shares", (string)null);
                });

            modelBuilder.Entity("FireBreath.PostsMicroservice.Models.Entities.Attachment", b =>
                {
                    b.HasOne("FireBreath.PostsMicroservice.Models.Entities.Message", "Message")
                        .WithMany("Attachments")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("FireBreath.PostsMicroservice.Models.Entities.Post", "Post")
                        .WithMany("Attachments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Message");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("FireBreath.PostsMicroservice.Models.Entities.Message", b =>
                {
                    b.Navigation("Attachments");
                });

            modelBuilder.Entity("FireBreath.PostsMicroservice.Models.Entities.Post", b =>
                {
                    b.Navigation("Attachments");
                });
#pragma warning restore 612, 618
        }
    }
}
