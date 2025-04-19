using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data
{
    public class DBContext(DbContextOptions<DBContext> options) : DbContext(options)
    {
        public DbSet<Ordem> Ordem { get; set; }
        public DbSet<ExposicaoFinanceira> ExposicaoFinanceira { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var dataFixa = new DateTime(2024, 04, 18, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<ExposicaoFinanceira>().HasData(
                new ExposicaoFinanceira { Id = 1, Ativo = "PETR4", Valor = 0, AtualizadoEm = dataFixa },
                new ExposicaoFinanceira { Id = 2, Ativo = "VALE3", Valor = 0, AtualizadoEm = dataFixa },
                new ExposicaoFinanceira { Id = 3, Ativo = "VIIA4", Valor = 0 , AtualizadoEm = dataFixa }
            );

            modelBuilder.Entity<Ordem>()
                .Property(o => o.Preco)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ExposicaoFinanceira>()
                .Property(e => e.Valor)
                .HasPrecision(18, 2);

        }
    }
}
