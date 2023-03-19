﻿using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using ProjectCMS.Models;
using System.Security.Cryptography;

namespace ProjectCMS.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Interactions>()
                .HasOne(x => x.User)
                .WithMany(x => x.Iteractions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<Comment>()
                .HasOne(x => x.User)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<Idea>()
                .HasOne(x => x.User)
                .WithMany(x => x.Ideas)
                .HasForeignKey(x => x.UserId);
            builder.Entity<Idea>()
                .HasOne(x => x.Event)
                .WithMany(x => x.Ideas)
                .HasForeignKey(x => x.EvId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();
            base.OnModelCreating(builder);
            SeedCate(builder);
            SeedDepartment(builder);
            SeedUser(builder);
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");

            builder.Properties<DateOnly?>()
                .HaveConversion<NullableDateOnlyConverter>()
                .HaveColumnType("date");
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private void SeedDepartment(ModelBuilder builder)
        {
            builder.Entity<Department>().HasData
                (
                new Department
                {
                    DepId = 1,
                    Name = "Business"
                },
                new Department
                {
                    DepId = 2,
                    Name = "Information technology"
                },
                new Department
                {
                    DepId = 3,
                    Name = "Design"
                }
                );
        }
        private void SeedUser (ModelBuilder builder)
        {
            CreatePasswordHash("123456", out byte[] passwordHash, out byte[] passwordSalt);
            builder.Entity<User>().HasData
                (
                    new User
                    {
                        UserId = 1,
                        UserName = "admin",
                        Email = "duongtdgch17587@fpt.edu.vn",
                        DepartmentID = 2,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        Phone = "0333804202",
                        DoB = DateTime.Parse("1998-11-22"),
                        Address = "Ha Noi",
                        Avatar = "example.jpg",
                        AddedDate = DateTime.Now,
                        Role = "Admin"
                    }
                );


        }
        private void SeedCate(ModelBuilder builder)
        {
            builder.Entity<Category>().HasData
                (
                new Category
                {
                    Id = 1,
                    Name = "Information technology",
                    Content = "This is content of this category",
                    AddedDate = DateTime.Now                   
                }
                );
        }

        public DbSet<Category> _categories { get; set; }
        public DbSet<Comment> _comments { get; set; }
        public DbSet<Department> _departments { get; set; }
        public DbSet<Event> _events { get; set; }
        public DbSet<Idea> _idea { get; set; }
        public DbSet<Interactions> _interactions { get; set; }
        public DbSet<User> _users { get; set; }
    }

    /// <summary>
    /// Converts <see cref="DateOnly" /> to <see cref="DateTime"/> and vice versa.
    /// </summary>
    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        /// <summary>
        /// Creates a new instance of this converter.
        /// </summary>
        public DateOnlyConverter() : base(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d))
        { }
    }

    /// <summary>
    /// Converts <see cref="DateOnly?" /> to <see cref="DateTime?"/> and vice versa.
    /// </summary>
    public class NullableDateOnlyConverter : ValueConverter<DateOnly?, DateTime?>
    {
        /// <summary>
        /// Creates a new instance of this converter.
        /// </summary>
        public NullableDateOnlyConverter() : base(
            d => d == null
                ? null
                : new DateTime?(d.Value.ToDateTime(TimeOnly.MinValue)),
            d => d == null
                ? null
                : new DateOnly?(DateOnly.FromDateTime(d.Value)))
        { }


    }
}