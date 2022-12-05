﻿using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
