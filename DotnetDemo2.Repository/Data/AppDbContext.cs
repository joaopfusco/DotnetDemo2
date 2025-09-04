﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DotnetDemo2.Domain.Models;
using DotnetDemo2.Repository.Mappings;

namespace DotnetDemo2.Repository.Data
{
    public partial class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            _connectionString = GetConnectionString(configuration);
            Database.Migrate();
        }

        private static string GetConnectionString(IConfiguration configuration)
        {
            var envConnection = Environment.GetEnvironmentVariable("DEFAULT_CONNECTION");
            var appsettingsConnection = configuration.GetConnectionString("DefaultConnection");

            if (!string.IsNullOrWhiteSpace(envConnection))
                return envConnection;

            if (!string.IsNullOrWhiteSpace(appsettingsConnection))
                return appsettingsConnection;

            throw new Exception("Não há ConnectionString.");
        }

        //public DbSet<Model> NomeTabel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new ModelMapping());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }

        public override int SaveChanges()
        {
            BeforeSaveChanges();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            BeforeSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void BeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var entries = ChangeTracker.Entries<BaseModel>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            var now = DateTime.UtcNow;
            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = now;
                }
                else
                {
                    entry.Property(nameof(BaseModel.CreatedAt)).IsModified = false;
                }
                entity.UpdatedAt = now;
            }
        }
    }
}
