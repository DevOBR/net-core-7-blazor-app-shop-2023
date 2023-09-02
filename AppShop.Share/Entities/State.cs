using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppShop.Share.Entities
{
    public class State
	{
        public int Id { get; set; }

        [Display(Name = "State/Departament")]
        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(100, ErrorMessage = "The field {0} can not have more than {1} characters.")]
        public string Name { get; set; } = null!;

        public int CountryId { get; set; }

        public Country? Country { get; set; }
        public ICollection<City>? Cities { get; set; }
        public int CitiesNumber => Cities == null ? 0 : Cities.Count;
    }
}

