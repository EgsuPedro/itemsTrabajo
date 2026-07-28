using ItemsMicroservice.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ItemsMicroservice.Data
{
    public class ItemsDbContext : DbContext
    {
        public ItemsDbContext(DbContextOptions<ItemsDbContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeo explicito de la tabla
            modelBuilder.Entity<Item>().ToTable("Items");

            // Indice unico por Codigo
            modelBuilder.Entity<Item>()
                .HasIndex(i => i.Codigo)
                .IsUnique();

            // SOFT DELETE GLOBAL FILTER: Excluye automaticamente items inactivos en las consultas SELECT
            modelBuilder.Entity<Item>().HasQueryFilter(i => i.Estado);
        }
    }
}