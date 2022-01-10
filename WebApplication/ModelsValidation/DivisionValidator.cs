using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApplication.Models;

namespace WebApplication.ModelsValidation
{
    public class DivisionValidator : AbstractValidator<Division>
    {
        public DivisionValidator()
        {
            RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Name is required");
            RuleFor(d => d.KingdomId)
            .NotEmpty().WithMessage("Kingdom id is required");
        }
    }
}
