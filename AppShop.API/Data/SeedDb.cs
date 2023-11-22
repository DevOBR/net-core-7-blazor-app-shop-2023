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
        private readonly IFileStorage _fileStorage;

        public SeedDb(DataContext context, IApiService apiService, IUserHelper userHelper, IFileStorage fileStorage)
		{
            _context = context;
            _apiService = apiService;
            _userHelper = userHelper;
            _fileStorage = fileStorage;
        }

        public async Task SeedAsync()
        {
            // Update database via code
            await _context.Database.EnsureCreatedAsync().ConfigureAwait(false);
            await CheckCountriesAsync().ConfigureAwait(false);
            await CheckCategoriesAsync().ConfigureAwait(false);
            await CheckRolesAsync().ConfigureAwait(false);
            await CheckUserAsync("1010", "Admin", "Admin    ", "oscarb@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "Jackblack.png", UserType.Admin).ConfigureAwait(false);
            await CheckUserAsync("2020", "Ledys", "Bedoya", "ledys@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "LedysBedoya.jpeg", UserType.User).ConfigureAwait(false);
            await CheckUserAsync("3030", "Brad", "Pitt", "brad@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "Brad.jpg", UserType.User).ConfigureAwait(false);
            await CheckUserAsync("4040", "Angelina", "Jolie", "angelina@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "Angelina.jpg", UserType.User).ConfigureAwait(false);
            await CheckUserAsync("5050", "Bob", "Marley", "bob@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "bob.jpg", UserType.User).ConfigureAwait(false);
            await CheckProductsAsync().ConfigureAwait(false);


        }

        private async Task CheckProductsAsync()
        {
            if (!_context.Products.Any())
            {
                await AddProductAsync("Adidas Barracuda", 270000M, 12F, new List<string>() { "Shoes", "Sports" }, new List<string>() { "adidas_barracuda.png" }).ConfigureAwait(false);
                await AddProductAsync("Adidas Superstar", 250000M, 12F, new List<string>() { "Shoes", "Sports" }, new List<string>() { "Adidas_superstar.png" }).ConfigureAwait(false);
                await AddProductAsync("AirPods", 1300000M, 12F, new List<string>() { "Technology", "Apple" }, new List<string>() { "airpos.png", "airpos2.png" }).ConfigureAwait(false);
                await AddProductAsync("Earphones Bose", 870000M, 12F, new List<string>() { "Technology" }, new List<string>() { "audifonos_bose.png" }).ConfigureAwait(false);
                await AddProductAsync("Bicycle Ribble", 12000000M, 6F, new List<string>() { "Sports" }, new List<string>() { "bicicleta_ribble.png" }).ConfigureAwait(false);
                await AddProductAsync("Checkered shirt", 56000M, 24F, new List<string>() { "Clothes" }, new List<string>() { "camisa_cuadros.png" }).ConfigureAwait(false);
                await AddProductAsync("Bicycle Helmet", 820000M, 12F, new List<string>() { "Sports" }, new List<string>() { "casco_bicicleta.png", "casco.png" }).ConfigureAwait(false);
                await AddProductAsync("iPad", 2300000M, 6F, new List<string>() { "Technology", "Apple" }, new List<string>() { "ipad.png" }).ConfigureAwait(false);
                await AddProductAsync("iPhone 13", 5200000M, 6F, new List<string>() { "Technology", "Apple" }, new List<string>() { "iphone13.png", "iphone13b.png", "iphone13c.png", "iphone13d.png" }).ConfigureAwait(false);
                await AddProductAsync("Mac Book Pro", 12100000M, 6F, new List<string>() { "Technology", "Apple" }, new List<string>() { "mac_book_pro.png" }).ConfigureAwait(false);
                await AddProductAsync("Dumbbells", 370000M, 12F, new List<string>() { "Sports" }, new List<string>() { "mancuernas.png" }).ConfigureAwait(false);
                await AddProductAsync("Face Mask", 26000M, 100F, new List<string>() { "Beauty" }, new List<string>() { "mascarilla_cara.png" }).ConfigureAwait(false);
                await AddProductAsync("New Balance 530", 180000M, 12F, new List<string>() { "Shoes", "Sports" }, new List<string>() { "newbalance530.png" }).ConfigureAwait(false);
                await AddProductAsync("New Balance 565", 179000M, 12F, new List<string>() { "Shoes", "Sports" }, new List<string>() { "newbalance565.png" }).ConfigureAwait(false);
                await AddProductAsync("Nike Air", 233000M, 12F, new List<string>() { "Shoes", "Sports" }, new List<string>() { "nike_air.png" }).ConfigureAwait(false);
                await AddProductAsync("Nike Zoom", 249900M, 12F, new List<string>() { "Shoes", "Sports" }, new List<string>() { "nike_zoom.png" }).ConfigureAwait(false);
                await AddProductAsync("Adidas Women's Buso", 134000M, 12F, new List<string>() { "Clothes", "Sports" }, new List<string>() { "buso_adidas.png" }).ConfigureAwait(false);
                await AddProductAsync("Boots Original Supplement", 15600M, 12F, new List<string>() { "Nutrition" }, new List<string>() { "Boost_Original.png" }).ConfigureAwait(false);
                await AddProductAsync("Whey Protein", 252000M, 12F, new List<string>() { "Nutrition" }, new List<string>() { "whey_protein.png" }).ConfigureAwait(false);
                await AddProductAsync("Pet Harness", 25000M, 12F, new List<string>() { "Pets" }, new List<string>() { "arnes_mascota.png" }).ConfigureAwait(false);
                await AddProductAsync("Pet Bed", 99000M, 12F, new List<string>() { "Pets" }, new List<string>() { "cama_mascota.png" }).ConfigureAwait(false);
                await AddProductAsync("Gamer Keyboard", 67000M, 12F, new List<string>() { "Gamer", "Technology" }, new List<string>() { "teclado_gamer.png" }).ConfigureAwait(false);
                await AddProductAsync("Gamer Chair", 980000M, 12F, new List<string>() { "Gamer", "Technology" }, new List<string>() { "silla_gamer.png" }).ConfigureAwait(false);
                await AddProductAsync("Mouse Gamer", 132000M, 12F, new List<string>() { "Gamer", "Technology" }, new List<string>() { "mouse_gamer.png" }).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private async Task AddProductAsync(string name, decimal price, float stock, List<string> categories, List<string> images)
        {
            Product prodcut = new()
            {
                Description = name,
                Name = name,
                Price = price,
                Stock = stock,
                ProductCategories = new List<ProductCategory>(),
                ProductImages = new List<ProductImage>()
            };

            foreach (var categoryName in categories)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName).ConfigureAwait(false);
                if (category is not null)
                {
                    prodcut.ProductCategories.Add(new ProductCategory { Category = category });
                }
            }

            foreach (string? image in images)
            {
                var filePath = Path.Combine(Environment.CurrentDirectory, "Images", "products", image);
                var fileBytes = File.ReadAllBytes(filePath);
                var imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "products").ConfigureAwait(false);
                prodcut.ProductImages.Add(new ProductImage { Image = imagePath });
            }

            _context.Products.Add(prodcut);
        }


        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, string image, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email).ConfigureAwait(false);
            if (user is null)
            {
                var city = await _context.Cities.FirstOrDefaultAsync(x => x.Name == "Monterrey").ConfigureAwait(false);
                if (city is null)
                {
                    city = await _context.Cities.FirstOrDefaultAsync().ConfigureAwait(false);
                }

                var filePath = Path.Combine(Environment.CurrentDirectory, "Images", "users", image);
                var fileBytes = File.ReadAllBytes(filePath);
                var imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "users").ConfigureAwait(false);

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
                    Photo = imagePath
                };

                await _userHelper.AddUserAsync(user, "123456").ConfigureAwait(false);
                await _userHelper.AddUserToRoleAsync(user, userType.ToString()).ConfigureAwait(false);

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
                await _userHelper.ConfirmEmailAsync(user, token).ConfigureAwait(false);
            }
            
            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString()).ConfigureAwait(false);
            await _userHelper.CheckRoleAsync(UserType.User.ToString()).ConfigureAwait(false);
        }


        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Apple" });
                _context.Categories.Add(new Category { Name = "Cars" });
                _context.Categories.Add(new Category { Name = "Beauty" });
                _context.Categories.Add(new Category { Name = "Shoes" });
                _context.Categories.Add(new Category { Name = "Food" });
                _context.Categories.Add(new Category { Name = "Cosmetics" });
                _context.Categories.Add(new Category { Name = "Sports" });
                _context.Categories.Add(new Category { Name = "Erotica" });
                _context.Categories.Add(new Category { Name = "Hardware store" });
                _context.Categories.Add(new Category { Name = "Gamer" });
                _context.Categories.Add(new Category { Name = "Home" });
                _context.Categories.Add(new Category { Name = "Garden" });
                _context.Categories.Add(new Category { Name = "Toys" });
                _context.Categories.Add(new Category { Name = "Lingerie" });
                _context.Categories.Add(new Category { Name = "Pets" });
                _context.Categories.Add(new Category { Name = "Nutrition" });
                _context.Categories.Add(new Category { Name = "Clothes" });
                _context.Categories.Add(new Category { Name = "Technology" });

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                Response responseCountries = await _apiService.GetListAsync<CountryResponse>("/v1", "/countries").ConfigureAwait(false);
                if (responseCountries.IsSuccess)
                {
                    List<CountryResponse> countries = (List<CountryResponse>)responseCountries.Result!;
                    foreach (CountryResponse countryResponse in countries)
                    {
                        Country country = await _context.Countries!.FirstOrDefaultAsync(c => c.Name == countryResponse.Name!)!;
                        if (country == null)
                        {
                            country = new() { Name = countryResponse.Name!, States = new List<State>() };
                            Response responseStates = await _apiService.GetListAsync<StateResponse>("/v1", $"/countries/{countryResponse.Iso2}/states").ConfigureAwait(false);
                            if (responseStates.IsSuccess)
                            {
                                List<StateResponse> states = (List<StateResponse>)responseStates.Result!;
                                foreach (StateResponse stateResponse in states!)
                                {
                                    State state = country.States!.FirstOrDefault(s => s.Name == stateResponse.Name!)!;
                                    if (state == null)
                                    {
                                        state = new() { Name = stateResponse.Name!, Cities = new List<City>() };
                                        Response responseCities = await _apiService.GetListAsync<CityResponse>("/v1", $"/countries/{countryResponse.Iso2}/states/{stateResponse.Iso2}/cities").ConfigureAwait(false);
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
                                await _context.SaveChangesAsync().ConfigureAwait(false);
                            }
                        }
                    }
                }
            }
        }
    }
}

