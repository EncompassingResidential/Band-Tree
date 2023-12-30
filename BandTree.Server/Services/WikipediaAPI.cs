/*
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create an instance of the WikipediaAPI class.
            WikipediaAPI wikipediaAPI = new WikipediaAPI();

            // Define the title of the Wikipedia page you want to retrieve.
            string pageTitle = "Microsoft";

            // Call the GetWikipediaPageAsync method to make the API request.
            string pageContent = await wikipediaAPI.GetWikipediaPageAsync(pageTitle);

            // Display the retrieved Wikipedia page content.
            Console.WriteLine(pageContent);
        }
    }

 */

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BandTree.Server.Services
{
    public class WikipediaAPI
    {
        private readonly HttpClient _httpClient;

        public WikipediaAPI()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://en.wikipedia.org/w/api.php");
        }

        public async Task<string> GetWikipediaPageAsync(string pageTitle)
        {
            try
            {
                // Make an HTTP GET request to retrieve the Wikipedia page content.
                HttpResponseMessage response = await _httpClient.GetAsync($"?action=query&format=json&titles={pageTitle}");

                // Check if the request was successful.
                if (response.IsSuccessStatusCode)
                {
                    // Read and return the response content.
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Handle the error or return an error message.
                    return "Error: Unable to retrieve Wikipedia page.";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions.
                return $"Error: {ex.Message}";
            }
        }
    } // public class WikipediaAPI
} // namespace BandTree.Server.Services
