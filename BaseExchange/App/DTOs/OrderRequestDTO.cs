using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DTOs
{
    public class OrderRequestDTO
    {
        public string Ativo { get; set; } = null!;
        public string Lado { get; set; } = null!;
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
    }
}
