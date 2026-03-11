using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc_app.Models
{

    public class Client
    {
        
        public int Id { get; set;}
        public string Name {get; set; } = null!;

        public List<ItemClient>? ItemClients {get; set; }





    }


}