﻿using Microsoft.EntityFrameworkCore;

namespace FinShark.DAL.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions dbContextOptions) 
        : base(dbContextOptions)
    {

    }

    // Link our models to the DBs
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
}