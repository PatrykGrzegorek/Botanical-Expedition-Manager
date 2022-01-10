using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApplication.Models;

namespace WebApplication.ModelsValidation
{
    public class GenuValidator : AbstractValidator<Genu>
    {
        public GenuValidator()
        {
            RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Name is required");
            RuleFor(d => d.FamilyId)
            .NotEmpty().WithMessage("Family id is required");
        }
    }
}
