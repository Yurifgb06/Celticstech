using Celticstech.Models;
using Microsoft.EntityFrameworkCore;

namespace Celticstech.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Regiao> Regioes { get; set; }

        public DbSet<Associacao> Associacoes { get; set; }

        public DbSet<Agricultor> Agricultores { get; set; }

        public DbSet<Cultivo> Cultivos { get; set; }

        public DbSet<Recomendacao> Recomendacoes { get; set; }
    }
}