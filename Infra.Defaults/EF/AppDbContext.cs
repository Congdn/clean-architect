using Infra.Defaults.EF.Configurations;
using Infra.Defaults.EF.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Defaults.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        //public DbSet<CategoryItem> CategoryItems { get; set; }
        //public DbSet<Files> Files { get; set; }
        //public DbSet<Layer> Layers { get; set; }
        //public DbSet<GeometryType> GeometryTypes { get; set; }
        //public DbSet<Geometry> Geometries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Configure using Fluent API
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            //modelBuilder.ApplyConfiguration(new CategoryItemConfiguration());
            //modelBuilder.ApplyConfiguration(new FileConfiguration());
            //modelBuilder.ApplyConfiguration(new LayerConfiguration());
            //modelBuilder.ApplyConfiguration(new GeometryTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new GeometryConfiguration());
        }
    }
}
