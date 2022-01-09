using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Models
{
    
    public class ViewCollection
    {

        public int CollectionId { get; set; }
        public string Name { get; set; }
        public int? MuseumId { get; set; }

        public string? MuseumName { get; set; }

        public ViewCollection(int collectionId, string name, int? museumId, string museumName)
        {
            CollectionId = collectionId;
            Name = name;
            MuseumId = museumId;
            MuseumName = museumName;
        }
    }
}
