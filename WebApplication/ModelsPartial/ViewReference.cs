using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Models
{
    
    public class ViewReference
    {
        public ViewReference(int referenceId, int? speciesId, string? speciesFullName, int? typeOfReferenceId, string? typeOfReferenceName, int? authorId, string? authorTitle, string? authorName, string? authorLastName, string title, DateTime? year, string description)
        {
            ReferenceId = referenceId;
            SpeciesId = speciesId;
            SpeciesFullName = speciesFullName;
            TypeOfReferenceId = typeOfReferenceId;
            TypeOfReferenceName = typeOfReferenceName;
            AuthorId = authorId;
            AuthorFullName = authorTitle + " " + authorName + " " + authorLastName;
            Title = title;
            Year = year;
            Description = description;
        }

        public int ReferenceId { get; set; }
        public int? SpeciesId { get; set; }
        public string? SpeciesFullName { get; set; }
        public int? TypeOfReferenceId { get; set; }
        public string? TypeOfReferenceName { get; set; }
        public int? AuthorId { get; set; }
        public string? AuthorFullName { get; set; }
        public string Title { get; set; }
        public DateTime? Year { get; set; }
        public string Description { get; set; }

    }
}
