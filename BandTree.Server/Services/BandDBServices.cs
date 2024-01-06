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

        public async Task<IActionResult?> GetBandByPageIDAsync(string pageid)
        {
            var result = await _EdgeDBclient.QueryAsync<BandModel>(
                "SELECT * " +
                " FROM Bands " +
                " WHERE Isbn = @Isbn LIMIT 1",
                new { pageid = pageid });

            var band = result.FirstOrDefault();
            if (band is null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(band);
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
