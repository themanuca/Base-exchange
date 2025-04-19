using App.DTOs;
using App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Interfaces
{
    public interface IOrdemServices
    {
       Task<OrderResponseDTO> ProcessarOrdemAsync(OrderRequestDTO request);
    }
}
