namespace AppShop.Web.Auth
{
    public interface ILoginService
	{
        Task LoginAsync(string token);

        Task LogoutAsync();
    }
}

