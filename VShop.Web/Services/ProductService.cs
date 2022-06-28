using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _clientFactory;
        private const string apiEndPoint = "/api/products/";
        private readonly JsonSerializerOptions _options;
        private ProductViewModel? productView;
        private IEnumerable<ProductViewModel>? listProductsView;

        public ProductService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProducts(string token)
        {
            var client = _clientFactory.CreateClient("ProductApi");
            PutTokenInHeader(token, client);

            using (var response = await client.GetAsync(apiEndPoint))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    listProductsView = await JsonSerializer
                        .DeserializeAsync<IEnumerable<ProductViewModel>>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return listProductsView;
        }

        private static void PutTokenInHeader(string token, HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<ProductViewModel> FindProductById(int id, string token)
        {
            var client = _clientFactory.CreateClient("ProductApi");

            PutTokenInHeader(token, client);

            using (var response = await client.GetAsync(apiEndPoint + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    productView = await JsonSerializer
                        .DeserializeAsync<ProductViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return productView;
        }


        public async Task<ProductViewModel> CreateProduct(ProductViewModel productVM, string token)
        {
            var client = _clientFactory.CreateClient("ProductApi");

            PutTokenInHeader(token, client);

            StringContent content = new(JsonSerializer.Serialize(productVM), Encoding.UTF8, "application/json");

            using (var response = await client.PostAsync(apiEndPoint, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    productView = await JsonSerializer
                        .DeserializeAsync<ProductViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return productView;
        }

        public async Task<ProductViewModel> UpdateProduct(ProductViewModel productVM, string token)
        {
            var client = _clientFactory.CreateClient("ProductApi");

            PutTokenInHeader(token, client);

            var productUpdated = new ProductViewModel();

            using (var response = await client.PutAsJsonAsync(apiEndPoint, productVM))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    productUpdated = await JsonSerializer
                        .DeserializeAsync<ProductViewModel>(apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return productUpdated;
        }

        public async Task<bool> DeleteProductById(int id, string token)
        {
            var client = _clientFactory.CreateClient("ProductApi");

            PutTokenInHeader(token, client);

            using (var response = await client.DeleteAsync(apiEndPoint + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
