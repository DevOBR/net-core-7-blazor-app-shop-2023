using System;
using AppShop.Share.Entities;

namespace AppShop.API.Data
{
	public class SeedDb
	{
        private readonly DataContext _dataContext;

        public SeedDb(DataContext dataContext)
		{
            _dataContext = dataContext;
        }

        public async Task SeedAsync()
        {
            // Update database via code
            await _dataContext.Database.EnsureCreatedAsync().ConfigureAwait(false);
            await CheckCountriesAsync().ConfigureAwait(false);
        }

        private async Task CheckCountriesAsync()
        {
            if (!_dataContext.Countries.Any())
            {
                _dataContext.Countries.Add(new Country { Name = "Colombia" });
                _dataContext.Countries.Add(new Country { Name = "Peru" });
                _dataContext.Countries.Add(new Country { Name = "Mexico" });
                await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            }


        }
    }
}

