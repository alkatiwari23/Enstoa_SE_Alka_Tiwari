using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Model;

namespace Task.Data
{
    public class TaskAPIContext : DbContext
    {
        public TaskAPIContext(DbContextOptions<TaskAPIContext> options) : base(options) { }
        public DbSet<Record> records { get; set; }
        public DbSet<ShoppingCart> cart { get; set; }
    }
}
        