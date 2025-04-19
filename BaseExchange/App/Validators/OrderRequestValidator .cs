using App.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Validators
{
    public class OrderRequestValidator : AbstractValidator<OrderRequestDTO>
    {
        public OrderRequestValidator()
        {
            RuleFor(x => x.Ativo)
                .NotEmpty()
                .Must(ativo => ativo == "PETR4" || ativo == "VALE3" || ativo == "VIIA4")
                .WithMessage("Ativo deve ser PETR4, VALE3 ou VIIA4.");

            RuleFor(x => x.Lado)
                .NotEmpty()
                .Must(lado => lado == "C" || lado == "V")
                .WithMessage("Lado deve ser 'C' (Compra) ou 'V' (Venda).");

            RuleFor(x => x.Quantidade)
                .GreaterThan(0)
                .LessThan(100_000)
                .WithMessage("Quantidade deve ser maior que 0 e menor que 100.000.");

            RuleFor(x => x.Preco)
                .GreaterThan(0)
                .LessThan(1000)
                .Must(preco => preco % 0.01m == 0)
                .WithMessage("Preço deve ser múltiplo de 0.01 e menor que 1000.");
        }
    }
}
