using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Ordem
    {
        public int Id { get; set; }

        public string Ativo { get; set; } = null!; // PETR4, VALE3, VIIA4

        public char Lado { get; set; } // 'C' = Compra, 'V' = Venda

        public int Quantidade { get; set; }

        public decimal Preco { get; set; }

        public decimal ValorTotal => Quantidade * Preco;

        public bool Aceita { get; set; }

        public string? MensagemErro { get; set; }

        public DateTime DataHoraCriacao { get; set; } = DateTime.UtcNow;
    }
}
