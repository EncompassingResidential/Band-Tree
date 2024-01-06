using BandTree.Server.Model;
using Microsoft.AspNetCore.Mvc;

namespace BandTree.Server.Services
{
    public interface IBandDBServices
    {
        public Task<IActionResult?> GetBandByPageIDAsync(string pageid);

        public Task<bool> UpdateBandAsync(BandModel band);
    }
}
