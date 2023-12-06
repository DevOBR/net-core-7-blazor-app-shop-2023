using AppShop.Share.Reponses;

namespace AppShop.API 
{
    public interface IOrdersHelper
    {
        Task<Response> ProcessOrderAsync(string email, string remarks);
    }
}

