using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public class OrderService : IOrderService
    {
        public readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUsername(string username)
        {
            var response = await _httpClient.GetAsync($"api/v1/Order/{username}");
            return await response.ReadContentAs<List<OrderResponseModel>>();
        }
    }
}

