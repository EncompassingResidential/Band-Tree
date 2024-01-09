using EdgeDB;
using Microsoft.AspNetCore.Mvc;
using BandTree.Server.Model;

namespace BandTree.Server.Services
{
    public class BandDBServices : IBandDBServices
    {
        private readonly EdgeDBClient _EdgeDBclient;

        public BandDBServices(EdgeDBClient client)
        {
            _EdgeDBclient = client;
        }

        public async Task<IActionResult?> GetBandByPageIDAsync(int WikiPageId)
        {
            Console.WriteLine("SELECT BandModel " +
                " { title, currentmembers, pastmembers, genres, labels, wikipediapageid, wikipediapagetimestamp, id } " +
                $" FILTER .wikipediapageid = @{WikiPageId} LIMIT 1");
            // EdgeDB ChatGPT said QuerySingleOrDefaultAsync existed.
            // exists QuerySingleAsync returns result or null
            var result = await _EdgeDBclient.QuerySingleAsync<BandModel>(
                "SELECT BandModel " +
                " { title, currentmembers, pastmembers, genres, labels, wikipediapageid, wikipediapagetimestamp, id } " +
                " FILTER .wikipediapageid = <int32>$pageid LIMIT 1",
                new { pageid = WikiPageId  });

            if (result is null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(result);
        }

        public async Task<bool> UpdateBandAsync(BandModel band)
        {
            var existingBand = await GetBandByPageIDAsync(band.WikipediaPageID);
            if (existingBand is null)
            {
                return false;
            }

            var result = await _EdgeDBclient.QueryAsync<object>(
                               @"UPDATE Bands " +
                                " SET Title = @Title, " +
                                " Author = @Author, " +
                                " ShortDescription = @ShortDescription, " +
                                " PageCount = @PageCount, " +
                                " ReleaseDate = @ReleaseDate " +
                                " WHERE Isbn = @Isbn",
                                band);
            return result.Any();
        }

    }
}
