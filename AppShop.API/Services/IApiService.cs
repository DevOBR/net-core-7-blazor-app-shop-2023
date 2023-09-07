using AppShop.Share.Reponses;

namespace AppShop.API.Services
{
    public interface IApiService
    {
        Task<Response> GetListAsync<T>(string servicePrefix, string controller);
    }
}

