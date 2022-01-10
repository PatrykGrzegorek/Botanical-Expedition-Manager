using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApplication.Models;

namespace WebApplication.ModelsValidation
{
    public class CommonNameValidator : AbstractValidator<CommonName>
    {
        public CommonNameValidator()
        {
            RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Name is required");
            RuleFor(d => d.ReferenceId)
            .NotEmpty().WithMessage("Reference id is required");
        }
    }
}
