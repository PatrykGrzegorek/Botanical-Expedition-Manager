using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Models
{
    
    public class ViewSpecies
    {
        public ViewSpecies(int id, string latinName, string fullName, string isEndemic, string isAutochthonous, string isWeed, string isInvasive, string genus, string order, string clas, string family, string kingom, string division)
        {
            Id = id;
            LatinName = latinName;
            CommonName = latinName + " ";
            FullName = fullName;
            IsEndemic = isEndemic;
            IsAutochthonous = isAutochthonous;
            IsWeed = isWeed;
            IsInvasive = isInvasive;
            Genus = genus;
            Order = order;
            Clas = clas;
            Family = family;
            Kingom = kingom;
            Division = division;
        }

        public int Id { get; set; }
        public string LatinName { get; set; }
        public string CommonName { get; set; }
        public string FullName { get; set; }
        public string IsEndemic { get; set; }
        public string IsAutochthonous { get; set; }
        public string IsWeed { get; set; }
        public string IsInvasive { get; set; }
        public string Genus { get; set; }
        public string Order { get; set; }
        public string Clas { get; set; }
        public string Family { get; set; }
        public string Division { get; set; }
        public string Kingom { get; set; }

    }
}
