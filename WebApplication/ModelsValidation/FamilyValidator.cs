using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApplication.Models;

namespace WebApplication.ModelsValidation
{
    public class FamilyValidator : AbstractValidator<Family>
    {
        public FamilyValidator()
        {
            RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Name is required");
            RuleFor(d => d.OrderId)
            .NotEmpty().WithMessage("Order id is required");
        }
    }
}
