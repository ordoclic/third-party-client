using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ordoclic.Auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // args[0] -> PEM path
                // args[1] -> partnerId
                // args[2] -> rpps
                AuthRequestService svc = new AuthRequestService(args[0], args[1]);
                string result = svc.GetTokenForRPPS(args[2]);

                Console.WriteLine("Response:");
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ex.StackTrace.ToString();
            }
        }
    }

    public class AuthRequestDTO
    {
        [JsonPropertyName("partnerId")]
        public string PartnerId { get; set; }

        [JsonPropertyName("rpps")]
        public string RPPS { get; set; }

        [JsonPropertyName("dateTime")]
        public string DateTime { get; set; }

        [JsonPropertyName("sig")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Signature { get; set; }

        public AuthRequestDTO(string partnerId, string rpps, string dateTime)
        {
            PartnerId = partnerId;
            RPPS = rpps;
            DateTime = dateTime;
        }
    }

    public class AuthRequestService
    {
        private const string DatePattern = "yyyy-MM-ddTHH:mm:ss.fffZ";
        private const string AuthUrl = "https://partners.staging.ordoclic.fr/v1/auth";

        private readonly RSACryptoServiceProvider _rsa;
        private readonly string _partnerId;

        public AuthRequestService(string pemFilePath, string partnerId)
        {
            _partnerId = partnerId;
            string privateKey = File.ReadAllText(pemFilePath);
            _rsa = ImportPrivateKey(privateKey);
        }

        public string GetTokenForRPPS(string rpps)
        {
            string timestamp = DateTime.Now.ToString(DatePattern);
            var body = new AuthRequestDTO(_partnerId, rpps, timestamp);

            try
            {
                Console.WriteLine("Unsigned JSON:");
                string unsignedJson = JsonSerializer.Serialize(body);
                Console.WriteLine(unsignedJson);

                // Create signature
                byte[] dataBytes = Encoding.UTF8.GetBytes(unsignedJson);
                byte[] signatureBytes = _rsa.SignData(dataBytes, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);

                // Convert signature to Base64 and add to the request body
                body.Signature = Convert.ToBase64String(signatureBytes);
                string signedPayload = JsonSerializer.Serialize(body);

                Console.WriteLine("Signed JSON:");
                Console.WriteLine(signedPayload);

                // Send HTTP request
                using var httpClient = new HttpClient();
                var content = new StringContent(signedPayload, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync(AuthUrl, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    Console.WriteLine($"HTTP Error: {response.StatusCode}");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during request: {ex.Message}");
                throw;
            }
        }

        private static RSACryptoServiceProvider ImportPrivateKey(string privateKey)
        {
            try
            {
                privateKey = privateKey
                    .Replace("-----BEGIN PRIVATE KEY-----", string.Empty)
                    .Replace("-----END PRIVATE KEY-----", string.Empty)
                    .Replace("\n", string.Empty)
                    .Replace("\r", string.Empty);

                byte[] keyBytes = Convert.FromBase64String(privateKey);

                var rsa = new RSACryptoServiceProvider();
                rsa.ImportPkcs8PrivateKey(keyBytes, out _);
                return rsa;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing private key: {ex.Message}");
                throw;
            }
        }
    }
}