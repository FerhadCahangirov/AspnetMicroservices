using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System.Net;

namespace Shopping.Aggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
            _orderService = orderService;
        }

        [HttpGet("{userName}", Name="GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
        {
            // get basket with username
            // iterate basket items and consume products with basket item productId member
            // map product related members with basket item dto with extended columns
            // consume ordering microservices in order to retrieve order list
            // return root Shooping model class with including all responses

            var basket = await _basketService.GetBasket(userName);
            foreach (var basketItem in basket.Items)
            {
                var product = await _catalogService.GetCatalog(basketItem.ProductId);

                //set additional product fields onto basket item

                basketItem.ProductName = product.Name;
                basketItem.Category = product.Category;
                basketItem.Summary = product.Summary;
                basketItem.Description = product.Description;
                basketItem.ImageFile = product.ImageFile;

            }

            var orders = await _orderService.GetOrdersByUsername(userName);

            var shoppingModel = new ShoppingModel
            {
                UserName = userName,
                BasketWithProducts = basket,
                Orders = orders
            };

            return Ok(shoppingModel);
        }

    }
}
