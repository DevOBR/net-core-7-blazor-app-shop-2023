using System;
using AppShop.API.Helper;
using AppShop.API.Services;
using AppShop.Share.Entities;
using AppShop.Share.Enums;
using AppShop.Share.Reponses;
using Microsoft.EntityFrameworkCore;

namespace AppShop.API.Data
{
	public class SeedDb
	{
        private readonly DataContext _context;
        private readonly IApiService _apiService;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IApiService apiService, IUserHelper userHelper)
		{
            _context = context;
            _apiService = apiService;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            // Update database via code
            await _context.Database.EnsureCreatedAsync().ConfigureAwait(false);
            await CheckCountriesAsync().ConfigureAwait(false);
            await CheckCategoriesAsync().ConfigureAwait(false);
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Admin", "Admin    ", "oscarb@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", UserType.Admin);

        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                var city = await _context.Cities.FirstOrDefaultAsync(x => x.Name == "Monterrey");
                if (city == null)
                {
                    city = await _context.Cities.FirstOrDefaultAsync();
                }

                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = city,
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
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

