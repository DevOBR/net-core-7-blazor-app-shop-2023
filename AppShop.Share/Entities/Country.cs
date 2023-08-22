﻿using System.ComponentModel.DataAnnotations;

namespace AppShop.Share.Entities
{
    public class Country
	{
		public int Id { get; set; }

		[Display(Name = "Country")]
		[Required(ErrorMessage = "The field {0} is required.")]
		[MaxLength(100, ErrorMessage = "The field {0} can not have more than {1} characters.")]
		public string Name { get; set; } = null!;

	}
}

