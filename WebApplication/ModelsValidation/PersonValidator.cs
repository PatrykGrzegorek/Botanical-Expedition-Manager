using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApplication.Models;

namespace WebApplication.ModelsValidation
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Name is required");
            RuleFor(d => d.LastName)
            .NotEmpty().WithMessage("Last name is required");
            RuleFor(d => d.Title)
            .NotEmpty().WithMessage("Title is required");
        }
    }
}
