using AppShop.Share.Reponses;

namespace AppShop.API.Helper
{
    public interface IMailHelper
    {
        Response SendMail(string toName, string toEmail, string subject, string body);
    }
}

