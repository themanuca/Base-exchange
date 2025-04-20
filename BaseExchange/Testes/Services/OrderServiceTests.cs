using App.DTOs;
using App.Services;
using Domain.Models;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testes.Services
{
    public class OrdemServicesTests
    {
        [Fact]
        public async Task Deve_Aceitar_Ordem_Se_Exposicao_Estiver_Dentro_Do_Limite()
        {
            var context = CriarDbContextInMemory();
            var service = new OrdemServices(context);
            var request = new OrderRequestDTO
            {
                Ativo = "PETR4",
                Lado = "C",
                Quantidade = 1000,
                Preco = 10.00m
            };

            var response = await service.ProcessarOrdemAsync(request);

            Assert.True(response.Sucesso);
            Assert.Equal(10_000m, response.ExposicaoAtual);
        }

        [Fact]
        public async Task Deve_Rejeitar_Ordem_Se_Ultrapassar_Exposicao_Maxima()
        {
            var context = CriarDbContextInMemory();

            var exposicao = context.ExposicaoFinanceira.First(e => e.Ativo == "PETR4");
            exposicao.Valor = 999_000m;
            context.SaveChanges();

            var service = new OrdemServices(context);

            var request = new OrderRequestDTO
            {
                Ativo = "PETR4",
                Lado = "C",
                Quantidade = 200,
                Preco = 10.00m
            };

            var response = await service.ProcessarOrdemAsync(request);

            Assert.False(response.Sucesso);
            Assert.Equal(999_000m, response.ExposicaoAtual);
            Assert.NotNull(response.MsgErro);
        }

        [Fact]
        public async Task Deve_Reduzir_Exposicao_Com_Ordem_De_Venda()
        {
            var context = CriarDbContextInMemory();
            var exposicao = context.ExposicaoFinanceira.First(e => e.Ativo == "PETR4");
            exposicao.Valor = 20_000m;
            context.SaveChanges();

            var service = new OrdemServices(context);

            var request = new OrderRequestDTO
            {
                Ativo = "PETR4",
                Lado = "V",
                Quantidade = 500,
                Preco = 10.00m
            };

            var response = await service.ProcessarOrdemAsync(request);

            Assert.True(response.Sucesso);
            Assert.Equal(15_000m, response.ExposicaoAtual);
        }

        [Fact]
        public async Task Deve_Retornar_Erro_Se_Ativo_Nao_Existir()
        {
            var context = CriarDbContextInMemory();
            var service = new OrdemServices(context);

            var request = new OrderRequestDTO
            {
                Ativo = "XYZ123",
                Lado = "C",
                Quantidade = 100,
                Preco = 10.00m
            };

            var response = await service.ProcessarOrdemAsync(request);

            Assert.False(response.Sucesso);
            Assert.Equal(0m, response.ExposicaoAtual);
            Assert.Equal("Ativo não encontrado", response.MsgErro);
        }

        private DBContext CriarDbContextInMemory()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new DBContext(options);

            context.ExposicaoFinanceira.AddRange(
                new ExposicaoFinanceira { Id = 1, Ativo = "PETR4", Valor = 0 },
                new ExposicaoFinanceira { Id = 2, Ativo = "VALE3", Valor = 0 },
                new ExposicaoFinanceira { Id = 3, Ativo = "VIIA4", Valor = 0 }
            );

            context.SaveChanges();

            return context;
        }
    }
}
