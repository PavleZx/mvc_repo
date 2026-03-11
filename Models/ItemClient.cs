using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc_app.Models
{

    public class ItemClient
    {
        public int Id { get; set; }
        public int ItemId { get; set; }

        public Item Item {get; set; }

        public int ClientId {get; set; }

        public Client Client {get; set; }



    }


}