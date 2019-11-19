using docker_sandbox_dev_api.Dal.Model;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace docker_sandbox_dev_api.Dal
{
    public class SandboxDbContext : DbContext
    {
        public SandboxDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Sandbox> Sandboxes { get; set; }
    }
}
