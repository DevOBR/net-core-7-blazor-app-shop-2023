using System;
using AppShop.API.Services;
using AppShop.Share.Entities;
using AppShop.Share.Reponses;
using Microsoft.EntityFrameworkCore;

namespace AppShop.API.Data
{
	public class SeedDb
	{
        private readonly DataContext _context;
        private readonly IApiService _apiService;

        public SeedDb(DataContext context, IApiService apiService)
		{
            _context = context;
            _apiService = apiService;
        }

        public async Task SeedAsync()
        {
            // Update database via code
            await _context.Database.EnsureCreatedAsync().ConfigureAwait(false);
            await CheckCountriesAsync().ConfigureAwait(false);
            await CheckCategoriesAsync().ConfigureAwait(false);
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Electronics" });
                _context.Categories.Add(new Category { Name = "Entertainment" });
                _context.Categories.Add(new Category { Name = "Home and Lifestyle" });
                _context.Categories.Add(new Category { Name = "Clothing" });
                _context.Categories.Add(new Category { Name = "Books" });
                _context.Categories.Add(new Category { Name = "Sports" });
                _context.Categories.Add(new Category { Name = "Toys" });
                _context.Categories.Add(new Category { Name = "Food" });
                _context.Categories.Add(new Category { Name = "Health and Beauty" });
                _context.Categories.Add(new Category { Name = "Automotive" });
                _context.Categories.Add(new Category { Name = "Garden and Outdoor" });
                _context.Categories.Add(new Category { Name = "Jewelry" });
                _context.Categories.Add(new Category { Name = "Movies" });
                _context.Categories.Add(new Category { Name = "Music" });
                _context.Categories.Add(new Category { Name = "Furniture" });
                _context.Categories.Add(new Category { Name = "Appliances" });
                _context.Categories.Add(new Category { Name = "Pet Supplies" });
                _context.Categories.Add(new Category { Name = "Travel" });
                _context.Categories.Add(new Category { Name = "Hobbies" });
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                Response responseCountries = await _apiService.GetListAsync<CountryResponse>("/v1", "/countries");
                if (responseCountries.IsSuccess)
                {
                    List<CountryResponse> countries = (List<CountryResponse>)responseCountries.Result!;
                    foreach (CountryResponse countryResponse in countries)
                    {
                        Country country = await _context.Countries!.FirstOrDefaultAsync(c => c.Name == countryResponse.Name!)!;
                        if (country == null)
                        {
                            country = new() { Name = countryResponse.Name!, States = new List<State>() };
                            Response responseStates = await _apiService.GetListAsync<StateResponse>("/v1", $"/countries/{countryResponse.Iso2}/states");
                            if (responseStates.IsSuccess)
                            {
                                List<StateResponse> states = (List<StateResponse>)responseStates.Result!;
                                foreach (StateResponse stateResponse in states!)
                                {
                                    State state = country.States!.FirstOrDefault(s => s.Name == stateResponse.Name!)!;
                                    if (state == null)
                                    {
                                        state = new() { Name = stateResponse.Name!, Cities = new List<City>() };
                                        Response responseCities = await _apiService.GetListAsync<CityResponse>("/v1", $"/countries/{countryResponse.Iso2}/states/{stateResponse.Iso2}/cities");
                                        if (responseCities.IsSuccess)
                                        {
                                            List<CityResponse> cities = (List<CityResponse>)responseCities.Result!;
                                            foreach (CityResponse cityResponse in cities)
                                            {
                                                if (cityResponse.Name == "Mosfellsbær" || cityResponse.Name == "Șăulița")
                                                {
                                                    continue;
                                                }
                                                City city = state.Cities!.FirstOrDefault(c => c.Name == cityResponse.Name!)!;
                                                if (city == null)
                                                {
                                                    state.Cities.Add(new City() { Name = cityResponse.Name! });
                                                }
                                            }
                                        }
                                        if (state.CitiesNumber > 0)
                                        {
                                            country.States.Add(state);
                                        }
                                    }
                                }
                            }
                            if (country.StatesNumber > 0)
                            {
                                _context.Countries.Add(country);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
        }
    }
}

