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
            await CheckCategoriesAsync().ConfigureAwait(false);
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_dataContext.Categories.Any())
            {
                _dataContext.Categories.Add(new Category { Name = "Electronics" });
                _dataContext.Categories.Add(new Category { Name = "Entertainment" });
                _dataContext.Categories.Add(new Category { Name = "Home and Lifestyle" });
                await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private async Task CheckCountriesAsync()
        {
            if (!_dataContext.Countries.Any())
            {
                _dataContext.Countries.Add(new Country
                {
                    Name = "Colombia",
                    States = new List<State>()
                    {
                        new State()
                        {
                            Name = "Antioquia",
                            Cities = new List<City>() {
                                new City() { Name = "Medellín" },
                                new City() { Name = "Itagüí" },
                                new City() { Name = "Envigado" },
                                new City() { Name = "Bello" },
                                new City() { Name = "Rionegro" },
                            }
                        },
                        new State()
                        {
                            Name = "Bogotá",
                            Cities = new List<City>() {
                                new City() { Name = "Usaquen" },
                                new City() { Name = "Champinero" },
                                new City() { Name = "Santa fe" },
                                new City() { Name = "Useme" },
                                new City() { Name = "Bosa" },
                            }
                        },
                    }
                });

                _dataContext.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = new List<State>()
                    {
                        new State()
                        {
                            Name = "Florida",
                            Cities = new List<City>() {
                                new City() { Name = "Orlando" },
                                new City() { Name = "Miami" },
                                new City() { Name = "Tampa" },
                                new City() { Name = "Fort Lauderdale" },
                                new City() { Name = "Key West" },
                            }
                        },
                        new State()
                        {
                            Name = "Texas",
                            Cities = new List<City>() {
                                new City() { Name = "Houston" },
                                new City() { Name = "San Antonio" },
                                new City() { Name = "Dallas" },
                                new City() { Name = "Austin" },
                                new City() { Name = "El Paso" },
                            }
                        },
                    }
                });

                await _dataContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}

