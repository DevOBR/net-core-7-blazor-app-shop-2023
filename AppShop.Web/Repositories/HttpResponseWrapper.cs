using System.Net;

namespace AppShop.Web.Repositories
{
    public class HttpResponseWrapper<T>
	{
		public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
		{
			Error = error;
			Response = response;
			HttpResponseMessage = httpResponseMessage;
		}

		public bool Error { get; set; }
		public T? Response { get; set; }
		public HttpResponseMessage HttpResponseMessage { get; set; }

		public async Task<string?> GetErrorMessage()
		{
			if (!Error)
			{
				return null;
			}

			var statusCode = HttpResponseMessage.StatusCode;
			if (statusCode == HttpStatusCode.NotFound)
			{
				return "Resource not found";
			}
			else if (statusCode == HttpStatusCode.BadRequest)
			{
				return await HttpResponseMessage.Content.ReadAsStringAsync();
			}
			else if (statusCode == HttpStatusCode.Unauthorized)
			{
				return "You have to Login to perform an operation.";
			}
			else if (statusCode == HttpStatusCode.Forbidden)
			{
				return "You don't have enough permissions to perform this action.";
			}

			return "Woops! there was an unexpected error";

		}
	}
}

