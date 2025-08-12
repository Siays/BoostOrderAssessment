using BoostOrderAssessment.Models;
using dotenv.net;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;


namespace BoostOrderAssessment.Services
{
    public static class ApiService
    {
        private static readonly HttpClient _http;

        static ApiService()
        {
            // Load environment variables
            var envPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.env");
            DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { envPath }));

            var username = Environment.GetEnvironmentVariable("API_USER");
            var password = Environment.GetEnvironmentVariable("API_PASS");

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new Exception("API_USER or API_PASS is missing in .env file.");

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            _http = new HttpClient(handler);

            var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<List<Product>> GetVariableProductsAsync()
        {
            var apiUrl = Environment.GetEnvironmentVariable("API_URL");

            if (string.IsNullOrWhiteSpace(apiUrl))
                throw new Exception("API_URL is missing or empty. Check your .env file.");

            var allProducts = new List<Product>();
            int page = 1;
            int totalPages = 1;

            do
            {
                var response = await _http.GetAsync($"{apiUrl}?page={page}");
                var raw = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"API returned error: {response.StatusCode} - {raw}");

                var obj = JObject.Parse(raw);
                var pageProducts = obj["products"]?.ToObject<List<Product>>() ?? new List<Product>();

                allProducts.AddRange(pageProducts
                    .Where(p => string.Equals(p.Type, "variable", StringComparison.OrdinalIgnoreCase)));

                if (response.Headers.TryGetValues("X-WC-TotalPages", out var values))
                {
                    if (int.TryParse(values.FirstOrDefault(), out var tp))
                        totalPages = tp;
                }

                page++;
            }
            while (page <= totalPages);

            return allProducts;
        }
    }
}
