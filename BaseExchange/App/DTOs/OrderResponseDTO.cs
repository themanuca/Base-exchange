using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DTOs
{
    public class OrderResponseDTO
    {
        public bool Sucesso { get; set; }
        public decimal ExposicaoAtual { get; set; }
        public string? MsgErro { get; set; }
    }
}
