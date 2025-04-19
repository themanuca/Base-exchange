using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ExposicaoFinanceira
    {
        public int Id { get; set; }

        public string Ativo { get; set; } = null!;

        public decimal Valor { get; set; }

        public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow; 
    }
}
