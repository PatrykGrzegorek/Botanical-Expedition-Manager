using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApplication.Models;

namespace WebApplication.ModelsValidation
{
    public class ReferenceValidator : AbstractValidator<Reference>
    {
        public ReferenceValidator()
        {
            RuleFor(d => d.SpeciesId)
            .NotEmpty().WithMessage("Species id is required");
            RuleFor(d => d.TypeOfReferenceId)
            .NotEmpty().WithMessage("Type of reference is required");
            RuleFor(d => d.Title)
            .NotEmpty().WithMessage("Title is required");
            RuleFor(d => d.Description)
            .NotEmpty().WithMessage("Description is required");
        }
    }
}
