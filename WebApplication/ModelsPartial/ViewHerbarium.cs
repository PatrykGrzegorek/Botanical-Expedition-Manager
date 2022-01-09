using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Models
{
    
    public class ViewHerbarium
    {
        public ViewHerbarium(int herbariumId, int collectionId, DateTime? yearOfCollection, int inventoryNumber, string collectionName)
        {
            HerbariumId = herbariumId;
            CollectionId = collectionId;
            YearOfCollection = yearOfCollection;
            InventoryNumber = inventoryNumber;
            CollectionName = collectionName;
        }

        public int HerbariumId { get; set; }
        public int CollectionId { get; set; }
        public DateTime? YearOfCollection { get; set; }
        public int InventoryNumber { get; set; }

        public string? CollectionName { get; set; }

    }
}
