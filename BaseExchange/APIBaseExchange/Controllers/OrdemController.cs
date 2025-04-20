using App.DTOs;
using App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIBaseExchange.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdemController(IOrdemServices ordemService) : ControllerBase
    {
        private readonly IOrdemServices _ordemService = ordemService;

        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] OrderRequestDTO request)
        {
            var response = await _ordemService.ProcessarOrdemAsync(request);

            if (!response.Sucesso)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
