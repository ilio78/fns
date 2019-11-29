using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodAndStyleOrderPlanning.Core
{
    public class Supplier
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [EmailAddress]
        public string EmailCC { get; set; }

        [Required]
        [Phone]
        public string Telephone { get; set; }

        public ICollection<Product> Products { get; set; }

        [Required]
        public bool IsActive { get; set; }
        //ALTER TABLE [Suppliers] ADD [IsActive] bit NOT NULL DEFAULT 0;
    }


}
