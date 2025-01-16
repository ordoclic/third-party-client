using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Ordoclic.Auth
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // args[0] -> chemin du fichier PEM
                // args[1] -> partnerId
                // args[2] -> rpps
                AuthRequestService svc = new AuthRequestService("private_key.pem", "eee1fc83-1781-4275-a5a0-c5505cfebd55");
                string result = svc.GetTokenForRPPS("90000000001");

                Console.WriteLine("Response:");
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }

    public class AuthRequestDTO
    {
        [JsonProperty("partnerId")]
        public string PartnerId { get; set; }

        [JsonProperty("rpps")]
        public string RPPS { get; set; }

        [JsonProperty("dateTime")]
        public string DateTime { get; set; }

        [JsonProperty("sig", NullValueHandling = NullValueHandling.Ignore)]
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
        private const string AuthUrl = "https://partners.staging.ordoclic.fr/v1/auth";

        private readonly ISigner _signer;
        private readonly string _partnerId;

        public AuthRequestService(string pemFilePath, string partnerId)
        {
            _partnerId = partnerId;
            _signer = CreateSigner(pemFilePath);
        }

        public string GetTokenForRPPS(string rpps)
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var body = new AuthRequestDTO(_partnerId, rpps, timestamp);

            try
            {
                // Exclure le champ "sig" pour la signature
                var unsignedBody = new
                {
                    partnerId = body.PartnerId,
                    rpps = body.RPPS,
                    dateTime = body.DateTime
                };

                string unsignedJson = JsonConvert.SerializeObject(unsignedBody, Formatting.None);
                Console.WriteLine("Unsigned JSON:");
                Console.WriteLine(unsignedJson);

                // Signer les données
                byte[] dataBytes = Encoding.UTF8.GetBytes(unsignedJson);
                _signer.BlockUpdate(dataBytes, 0, dataBytes.Length);
                byte[] signatureBytes = _signer.GenerateSignature();

                // Ajouter la signature en Base64 au corps de la requête
                body.Signature = Convert.ToBase64String(signatureBytes);
                string signedPayload = JsonConvert.SerializeObject(body, Formatting.Indented);

                Console.WriteLine("Signed JSON:");
                Console.WriteLine(signedPayload);

                // Envoyer la requête HTTP
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    try
                    {
                        string response = client.UploadString(AuthUrl, "POST", signedPayload);
                        Console.WriteLine("HTTP Response Body:");
                        Console.WriteLine(response);
                        return response;
                    }
                    catch (WebException webEx)
                    {
                        using (var stream = webEx.Response?.GetResponseStream())
                        using (var reader = new StreamReader(stream ?? Stream.Null))
                        {
                            string errorResponse = reader.ReadToEnd();
                            Console.WriteLine($"HTTP Error Code: {(int)((HttpWebResponse)webEx.Response).StatusCode}");
                            Console.WriteLine("HTTP Error Body:");
                            Console.WriteLine(errorResponse);
                        }

                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during request: {ex.Message}");
                throw;
            }
        }

        private static ISigner CreateSigner(string pemFilePath)
        {
            try
            {
                using (var reader = File.OpenText(pemFilePath))
                {
                    PemReader pemReader = new PemReader(reader);
                    object keyObject = pemReader.ReadObject();

                    if (keyObject is AsymmetricCipherKeyPair keyPair)
                    {
                        var privateKey = keyPair.Private as RsaPrivateCrtKeyParameters;
                        if (privateKey == null) throw new Exception("Invalid RSA private key.");

                        var signer = SignerUtilities.GetSigner("SHA512withRSA");
                        signer.Init(true, privateKey);
                        return signer;
                    }

                    if (keyObject is RsaPrivateCrtKeyParameters privateKeyParams)
                    {
                        var signer = SignerUtilities.GetSigner("SHA512withRSA");
                        signer.Init(true, privateKeyParams);
                        return signer;
                    }

                    throw new Exception("Unsupported key format.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading private key: {ex.Message}");
                throw;
            }
        }
    }
}
