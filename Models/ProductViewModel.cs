using Microsoft.AspNetCore.Razor.Language.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Storage.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public int InventoryValue { get; set; }
    }
}
