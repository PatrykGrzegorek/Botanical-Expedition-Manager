using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApplication.Models;

namespace WebApplication.ModelsValidation
{
    public class ExpeditionValidator : AbstractValidator<Expedition>
    {
        public ExpeditionValidator()
        {
            RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Name is required");
            RuleFor(d => d.Discription)
            .NotEmpty().WithMessage("Description is required");
        }
    }
}
