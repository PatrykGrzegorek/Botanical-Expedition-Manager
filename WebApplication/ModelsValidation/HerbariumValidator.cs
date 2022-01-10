using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApplication.Models;

namespace WebApplication.ModelsValidation
{
    public class HerbariumValidator : AbstractValidator<Herbarium>
    {
        public HerbariumValidator()
        {
            RuleFor(d => d.InventoryNumber)
            .NotEmpty().WithMessage("Inventory number is required");
            RuleFor(d => d.CollectionId)
            .NotEmpty().WithMessage("Collection is required");
            RuleFor(d => d.PiecesOfPlantsId)
            .NotEmpty().WithMessage("Pieces of plant is required");
            RuleFor(d => d.SpiecesId)
            .NotEmpty().WithMessage("Species is required");
            
        }
    }
}
