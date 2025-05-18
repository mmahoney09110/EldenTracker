using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenAIServer.Entities;

namespace OpenAIServer.Data
{
    public class OpenAIServerContext : DbContext
    {
        public OpenAIServerContext (DbContextOptions<OpenAIServerContext> options)
            : base(options)
        {
        }

        public DbSet<OpenAIServer.Entities.ERStats> ERStats { get; set; } = default!;
    }
}
