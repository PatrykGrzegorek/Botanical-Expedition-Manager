using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApplication.Models;

namespace WebApplication.ModelsValidation
{
    public class ObservationValidator : AbstractValidator<Observation>
    {
        public ObservationValidator()
        {
            RuleFor(d => d.Date)
            .NotEmpty().WithMessage("Date is required");
            RuleFor(d => d.ExpeditionId)
            .NotEmpty().WithMessage("Expedition id is required");
            RuleFor(d => d.SpeciesId)
            .NotEmpty().WithMessage("Species id is required");
        }
    }
}
