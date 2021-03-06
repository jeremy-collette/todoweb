﻿namespace todoweb.Server.Core
{
    using Microsoft.EntityFrameworkCore;

    using todoweb.Server.Models;

    public class DatabaseContext<TResource>
        : DbContext
        where TResource : class, IServerResource
    {
        public DbSet<TResource> Resources { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext<TResource>> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TResource>().ToTable(typeof(TResource).Name);
        }
    }
}
