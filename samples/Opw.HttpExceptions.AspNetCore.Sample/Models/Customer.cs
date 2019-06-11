using System.ComponentModel.DataAnnotations;

namespace Opw.HttpExceptions.AspNetCore.Sample.Models
{
    public class Customer
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Name { get; set; }
    }
}
