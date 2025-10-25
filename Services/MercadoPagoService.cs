using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrupoCeleste.Services
{
    public class MercadoPagoService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public MercadoPagoService(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<string> CrearPreferenciaPagoAsync(decimal monto, string membresiaNombre, string email)
        {
            var accessToken = _config["MercadoPago:AccessToken"];
            var url = "https://api.mercadopago.com/checkout/preferences";

            var body = new
            {
                items = new[]
                {
                    new
                    {
                        title = membresiaNombre,
                        quantity = 1,
                        unit_price = monto
                    }
                },
                payer = new { email = email },
                back_urls = new
                {
                    success = "https://localhost:5001/Payment/Success",
                    failure = "https://localhost:5001/Payment/Failure",
                    pending = "https://localhost:5001/Payment/Pending"
                },
                auto_return = "approved"
            };

            var json = JsonSerializer.Serialize(body);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                // Parsear la respuesta para obtener la URL de pago
                using var doc = JsonDocument.Parse(responseContent);
                var initPoint = doc.RootElement.GetProperty("init_point").GetString();
                return initPoint ?? "Error: No se pudo obtener la URL de pago";
            }
            else
            {
                return $"Error: {response.StatusCode} - {responseContent}";
            }
        }
    }
}
