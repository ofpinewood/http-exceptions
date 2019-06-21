using System.ComponentModel.DataAnnotations;

namespace Opw.HttpExceptions.AspNetCore._Test
{
    public class Product
    {
        [Required]
        public string Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }
    }
}
