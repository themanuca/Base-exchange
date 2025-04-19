using App.DTOs;
using App.Interfaces;
using Domain.Models;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services
{
    public class OrdemServices(DBContext context) : IOrdemServices
    {
        private readonly DBContext _context = context;
        private const decimal LIMITE_EXP = 1_000_000;

        public async Task<OrderResponseDTO> ProcessarOrdemAsync(OrderRequestDTO request)
        {
            var valorOrdem = request.Quantidade * request.Preco;

            var exposicao = await _context.ExposicaoFinanceira
                .FirstOrDefaultAsync(e => e.Ativo == request.Ativo);

            if (exposicao == null)
            {
                return new OrderResponseDTO
                {
                    Sucesso = false,
                    ExposicaoAtual = 0,
                    MsgErro = "Ativo não encontrado"
                };
            }

            var novaExposicao = exposicao.Valor;

            novaExposicao += request.Lado == "C" ? valorOrdem : -valorOrdem;

            if (Math.Abs(novaExposicao) > LIMITE_EXP)
            {
                return new OrderResponseDTO
                {
                    Sucesso = false,
                    ExposicaoAtual = exposicao.Valor,
                    MsgErro = "Limite de exposição ultrapassado"
                };
            }

            exposicao.Valor = novaExposicao;
            exposicao.AtualizadoEm = DateTime.UtcNow;

            var ordem = new Ordem
            {
                Ativo = request.Ativo,
                Lado = request.Lado == "C" ? 'C' : 'V',
                Quantidade = request.Quantidade,
                Preco = request.Preco,
                Aceita = true,
                DataHoraCriacao = DateTime.UtcNow
            };

            _context.Ordem.Add(ordem);
            await _context.SaveChangesAsync();

            return new OrderResponseDTO
            {
                Sucesso = true,
                ExposicaoAtual = novaExposicao
            };
        }
    }
}
