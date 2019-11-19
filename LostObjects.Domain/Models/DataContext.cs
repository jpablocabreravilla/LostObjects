﻿namespace LostObjects.Domain.Models
{
    using System.Data.Entity;
    using Common.Models;


    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {
        }

        public DbSet<Objectt> Objectts { get; set; }
    }
}
