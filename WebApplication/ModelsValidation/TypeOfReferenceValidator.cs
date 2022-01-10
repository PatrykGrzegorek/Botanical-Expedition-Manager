using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApplication.Models;

namespace WebApplication.ModelsValidation
{
    public class TypeOfReferenceValidator : AbstractValidator<TypeOfReference>
    {
        public TypeOfReferenceValidator()
        {
            RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Name is required");
            
        }
    }
}
