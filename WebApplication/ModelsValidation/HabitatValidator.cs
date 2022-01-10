using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApplication.Models;

namespace WebApplication.ModelsValidation
{
    public class HabitatValidator : AbstractValidator<Habitat>
    {
        public HabitatValidator()
        {
            RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Name is required");
            RuleFor(d => d.NationalClassification)
            .NotEmpty().WithMessage("National classification id is required");
        }
    }
}
