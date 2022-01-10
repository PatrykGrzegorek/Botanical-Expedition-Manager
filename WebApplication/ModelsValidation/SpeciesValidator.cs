using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApplication.Models;

namespace WebApplication.ModelsValidation
{
    public class SpeciesValidator : AbstractValidator<Species>
    {
        public SpeciesValidator()
        {
            RuleFor(d => d.LatinName)
            .NotEmpty().WithMessage("Latin name is required");
            RuleFor(d => d.TaxonomicTree)
            .NotEmpty().WithMessage("Taxonomic tree is required");
            RuleFor(d => d.IsEndemic)
            .NotEmpty().WithMessage("Is endemic is required");
            RuleFor(d => d.IsAutochthonous)
            .NotEmpty().WithMessage("Is autochthonous is required");
            RuleFor(d => d.IsWeed)
            .NotEmpty().WithMessage("Is weed is required");
            RuleFor(d => d.IsInvasive)
            .NotEmpty().WithMessage("Is invasive is required");
        }
    }
}
