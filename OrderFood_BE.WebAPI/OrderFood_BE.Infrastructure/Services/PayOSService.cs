using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json;
using OrderFood_BE.Application.Models.Requests.Payment;
using OrderFood_BE.Application.Models.Response.Payment;
using OrderFood_BE.Application.Services;
using OrderFood_BE.Infrastructure.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OrderFood_BE.Infrastructure.Services
{
    public class PayOSService : IPayOSService
    {
        private readonly PayOSSettings _settings;
        private readonly HttpClient _httpClient;

        public PayOSService(PayOSSettings settings, HttpClient httpClient)
        {
            _settings = settings;
            _httpClient = httpClient;
        }

        public async Task<string> CreatePaymentLinkAsync(PaymentRequestModel model)
        {

            var dataString = $"amount={model.Amount}&cancelUrl={model.CancelUrl}&description={model.Description}&orderCode={model.OrderCode}&returnUrl={model.ReturnUrl}";
            var signature = GenerateChecksum(dataString, _settings.ChecksumKey);

            var payload = new
            {
                amount = model.Amount,
                orderCode = model.OrderCode,
                description = model.Description,
                cancelUrl = model.CancelUrl,
                returnUrl = model.ReturnUrl,
                signature
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.BaseUrl}/v2/payment-requests")
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };

            request.Headers.Add("x-client-id", _settings.ClientId);
            request.Headers.Add("x-api-key", _settings.ApiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(responseContent);
            var root = json.RootElement;
            string code = root.GetProperty("code").GetString()!;
            string desc = root.GetProperty("desc").GetString()!;
            if (code == "00")
            {
                string checkoutUrl = root.GetProperty("data").GetProperty("checkoutUrl").GetString()!;
                Console.WriteLine("PayOS checkout URL: " + checkoutUrl);
                //string qrCode = root.GetProperty("data").GetProperty("qrCode").GetString()!;
                return checkoutUrl;
            }
            else
            {
                return $"Create link failed: {desc}";
            }
            //Console.WriteLine("PayOS response: " + responseContent);
            //Console.WriteLine("json: " + json);
            //var data = json.RootElement.GetProperty("data");
            //string checkoutUrl = data.GetProperty("checkoutUrl").GetString();
            //string qrCode = data.GetProperty("qrCode").GetString();
            //string status = data.GetProperty("status").GetString();
        }

        public bool ValidateSignature(PayOSWebhookModel model)
        {
            var secretKey = _settings.ChecksumKey;
            var rawData = $"{model.OrderCode}|{model.Amount}|{model.Status}";

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var calculatedSignature = Convert.ToHexString(hash).ToLower();

            return calculatedSignature == model.Signature?.ToLower();
        }

        private string GenerateChecksum(string body, string key)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(body));
            return Convert.ToHexString(hash).ToLower();
        }
    }
}
