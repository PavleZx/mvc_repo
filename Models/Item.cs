using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc_app.Models
{
    
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [Range(0.00001, double.MaxValue, ErrorMessage = "Quantity must be greater than zero")]
        public double Price { get; set; }

        public int? SerialNumberId { get; set; }

        public SerialNumber? SerialNumber { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]

        public Category? Category { get; set; }

    }


}