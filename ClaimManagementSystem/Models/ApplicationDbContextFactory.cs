﻿using ClaimManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        builder.UseSqlServer(connectionString);

        return new ApplicationDbContext(builder.Options);
    }
}