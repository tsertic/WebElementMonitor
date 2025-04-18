using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using HtmlAgilityPack;

namespace WebElementMonitor.Services
{
    /// <summary>
    /// Provides functionality to fetch web pages, extract HTML elements, and compute content hashes.
    /// </summary>
    public class WebService : IDisposable
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebService"/> class and configures the HTTP client.
        /// </summary>
        public WebService()
        {
            _httpClient = new HttpClient();
            ConfigureHttpClient();
        }

        /// <summary>
        /// Configures default headers and timeouts for the HTTP client.
        /// </summary>
        private void ConfigureHttpClient()
        {
            if (!_httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                    "Chrome/91.0.4472.124 Safari/537.36"))
            {
                // Header parsing failed; unlikely scenario
            }
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// Asynchronously retrieves the content of the specified URL.
        /// </summary>
        /// <param name="url">An absolute HTTP or HTTPS URL to fetch.</param>
        /// <returns>The response body as a string, or throws on failure.</returns>
        /// <exception cref="ArgumentException">Thrown when the URL is invalid.</exception>
        /// <exception cref="HttpRequestException">Thrown when the HTTP response indicates failure.</exception>
        public async Task<string> GetWebsiteContentAsync(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                throw new ArgumentException($"Invalid URL format: {url}", nameof(url));
            }

            HttpResponseMessage response = await _httpClient.GetAsync(uri).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Failed to fetch resource at {uri}. Status code: {response.StatusCode}");
            }

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Extracts and returns the outer HTML of the element with the specified ID.
        /// </summary>
        /// <param name="htmlContent">The full HTML document content.</param>
        /// <param name="elementId">The ID attribute of the target element.</param>
        /// <returns>The element's outer HTML, or <c>null</c> if not found.</returns>
        /// <exception cref="ArgumentException">Thrown when inputs are null or empty.</exception>
        public string ExtractElementHtml(string htmlContent, string elementId)
        {
            if (string.IsNullOrWhiteSpace(htmlContent))
                throw new ArgumentException("HTML content must not be null or empty.", nameof(htmlContent));

            if (string.IsNullOrWhiteSpace(elementId))
                throw new ArgumentException("Element ID must not be null or empty.", nameof(elementId));

            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(htmlContent);
            var node = document.GetElementbyId(elementId);
            return node?.OuterHtml;
        }

        /// <summary>
        /// Computes the SHA256 hash of the given content and returns its hexadecimal representation.
        /// </summary>
        /// <param name="content">The text to hash; must not be null.</param>
        /// <returns>Hexadecimal string of the SHA256 hash.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="content"/> is null.</exception>
        public static string CalculateHash(string content)
        {
            if (content is null)
                throw new ArgumentNullException(nameof(content));

            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(content));
            var builder = new StringBuilder(hashBytes.Length * 2);

            foreach (byte b in hashBytes)
                builder.Append(b.ToString("x2"));

            return builder.ToString();
        }

        /// <summary>
        /// Releases the HTTP client resources.
        /// </summary>
        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
