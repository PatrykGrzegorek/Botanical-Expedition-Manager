using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApplication.Models;

namespace WebApplication.ModelsValidation
{
    public class CollectionValidator : AbstractValidator<Collection>
    {
        public CollectionValidator()
        {
            RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Name is required");

        }
    }
}
