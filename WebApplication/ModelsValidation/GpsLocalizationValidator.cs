using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApplication.Models;

namespace WebApplication.ModelsValidation
{
    public class GpsLocalizationValidator : AbstractValidator<Gpslocalization>
    {
        public GpsLocalizationValidator()
        {
            RuleFor(d => d.Latitude)
            .NotEmpty().WithMessage("Latitude is required");
            RuleFor(d => d.Longitude)
            .NotEmpty().WithMessage("Longitude is required");
            RuleFor(d => d.MarginOfError)
            .NotEmpty().WithMessage("Margin of error is required");
        }
    }
}
