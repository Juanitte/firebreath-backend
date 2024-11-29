﻿using EasyWeb.UserMicroservice.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EasyWeb.UserMicroservice.Models.Context
{
    public class UsersDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<User> UsersDB { get; set; }

        public UsersDbContext(DbContextOptions options) : base(options)
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
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles").HasKey(r => r.Id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Habilita el sensitive logging
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
