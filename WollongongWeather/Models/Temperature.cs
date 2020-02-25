using System.ComponentModel.DataAnnotations;

namespace TempApp.Models
{
    public class Temperature
    {
        public int Id { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Temp { get; set; }
    }
}
