using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApplication.Models;

namespace WebApplication.ModelsValidation
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Name is required");
            RuleFor(d => d.ClassId)
            .NotEmpty().WithMessage("Class id is required");
        }
    }
}
