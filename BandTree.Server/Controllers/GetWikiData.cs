using System.Net.Http;

namespace BandTree.Server.Controllers
{
    public class GetWikiData
    {
        // Constructor
        public GetWikiData()
        {
            // Constructor logic here (if needed)
        }

        public void RetrieveData()
        {
            using (var client = new HttpClient())
            {
                var EndPoint = new Uri("https://en.wikipedia.org/w/api.php?action=query&origin=*&format=json&generator=search&gsrnamespace=0&gsrlimit=5&gsrsearch='Journey_(band)'");
                var result = client.GetAsync(EndPoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
