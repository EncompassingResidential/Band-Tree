/*
 * My current versions on 01/04/2024 are:
 * C# version 9.0
 * Language 11.0
 * Using .NET 7
 * MinimalAPIs
 */

using BandTree.Server.Services;
using BandTree.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Console;
using Microsoft.AspNetCore.Http.Extensions;
using EdgeDB;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configure logging to include timestamps
builder.Logging.ClearProviders();
builder.Logging.AddConsole(options =>
{
    options.FormatterName = ConsoleFormatterNames.Simple;  // Use simple formatter
}).SetMinimumLevel(LogLevel.Information);

builder.Logging.AddSimpleConsole(options =>
{
    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
    options.IncludeScopes = false;
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient(); // Add HttpClient support - talking to Wikipedia API

// Deals with CORS - Cross Origin Resource Sharing
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:3000") // Specify the exact origin of your React app
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Register EdgeDBClient
builder.Services.AddScoped<EdgeDBClient>(serviceProvider =>
{
    // Assuming EdgeDBClient takes a connection string or similar parameter
    var connectionString = builder.Configuration.GetConnectionString("EdgeDB");
    var connection = EdgeDBConnection.Parse(connectionString);
    return new EdgeDBClient(connection);
});

// Register BandDBServices
builder.Services.AddScoped<IBandDBServices, BandDBServices>(); // Adjust as per your implementation


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*
 * app.UseHttpsRedirection() = Adds middleware for redirecting HTTP Requests to HTTPS
 *
 *    Returns:
 *    The lApplicationBuilder for HttpsRedirection.
 *
 *    GitHub Examples and Documentation (Alt+O)
 *
 */
app.UseHttpsRedirection();

/*
 * app.UseAuthentication() = Adds the AuthenticationMiddleware to the specified IApplicationBuilder, 
 * which enables authentication capabilities to responsible for authenticating the user identity from the incoming request.
 *  
 *  It decodes and validates the credentials presented in the request (like tokens, cookies, headers)
 *  and sets the user identity based on these credentials.
 */
// app.UseAuthentication();

/*
 * 
 */
app.UseAuthorization();

// Configure the HTTP request pipeline.
// Deals with CORS - Cross Origin Resource Sharing
app.UseCors("AllowSpecificOrigin"); // Use the policy

// https://localhost:7100/
// returns Hello World!
app.MapGet("/", () =>
{
    var formattedDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    return $"Hello World! John back from Hawaii {formattedDateTime} Local Date & Time";
});

/*
 * Band Tree endpoint is 
 * https://localhost:7088/wikipedia-search?searchTerm=Microsoft
 * 
 * which is then calling the Wikipedia API with
 * https://en.wikipedia.org/w/api.php?action=query&list=search&srsearch=band%20journey&format=json
 */
app.MapGet("/wikipedia-search", async (HttpContext context, string searchTerm, IHttpClientFactory clientFactory) =>
{
    /*
     * If I need to see the URL that was requested, just route or entire URL
     * var requestUrl = context.Request.Path;
     * Console.WriteLine($"Received route for URL: {requestUrl}");
     * var displayUrl = context.Request.GetDisplayUrl();
     * Console.WriteLine($"Received URL GetDisplayUrl: {displayUrl}");
     */
    if (string.IsNullOrWhiteSpace(searchTerm))
    {
        return Results.BadRequest("Search term is required.");
    }

    var client = clientFactory.CreateClient();
    var wikiApiUrl = $"https://en.wikipedia.org/w/api.php?action=query&list=search&srsearch={Uri.EscapeDataString(searchTerm)}&format=json";

    try
    {
        var response = await client.GetAsync(wikiApiUrl);
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadAsStringAsync();

        return Results.Content(data, "application/json");
    }
    catch (HttpRequestException e)
    {
        return Results.Problem($"Error calling Wikipedia API: {e.Message}");
    }
});


/* 
 * PUT  Get the Band, Artist, Group (BAG) Wikipedia Page
 *     using a PUT 
 *     because the Artist will be created in the Relationship database if it does not exist.
 *     
 * Returning JSON data with the Band, Artist, Group (BAG) data;
 * Current and Past Members
 * 
 * Future version will have to decide if on this call or another EndPoint the Band Client
 * will periodically check if more members have been added to the Band, Artist, Group (BAG)
 * family tree, cousins, grand parents, etc.
 * 
 * If takes less than a second then I want to get children and grandchilden members of the BAG in question
 * the one that was originally requested by the user.
 *     
 *  * Band Tree endpoint is 
 * https://localhost:7088/wikipedia-page/152447
 * 
 * which is then calling the Wikipedia API with
 * https://en.wikipedia.org/w/api.php?action=parse&pageid=152447&prop=text&format=json
 */
app.MapPut("wikipedia-page/{pageid}", async (string pageid, HttpContext httpContext, IHttpClientFactory clientFactory) =>
{
    var bandService = httpContext.RequestServices.GetRequiredService<IBandDBServices>();

    var requestUrl = httpContext.Request.Path;
    Console.WriteLine($"Received route for URL: {requestUrl}");
    var displayUrl = httpContext.Request.GetDisplayUrl();
    Console.WriteLine($"Received URL GetDisplayUrl: {displayUrl}");

    /*
     * I didn't think of this until 1/08/24
     * This code at this point doesn't distinguish between a Band, Artist, Group (BAG) pageid
     * so after I get the Wikipedia data I have to check if it is a Band, Artist, Group (BAG) pageid
     */
    if (!int.TryParse(pageid, out int numericPageId))
    {
        return Results.BadRequest($"The Wikipedia identifier, pageid {pageid} is not a number");
    }

    // This call is specifically for BandModel
    IActionResult? actionResult = await bandService.GetBandByPageIDAsync(int.Parse(pageid));

    BandModel band = new BandModel();

    if (actionResult is OkObjectResult okObjectResult)
    {
        band = okObjectResult.Value as BandModel ?? new BandModel();
    }

    string WikipediaData = string.Empty;
    BandModelProcessor bandModelProcessor = new BandModelProcessor();

    // Band pageid was not found in the Band Tree database
    if (band is BandModel 
        && ( string.IsNullOrEmpty(band.Title) || band.WikipediaPageID == 0))
    {
        var client = clientFactory.CreateClient();
        // for Rush (band) pageid = 25432
        //                 https://en.wikipedia.org/w/api.php?action=parse&pageid=  25432 &prop=text&format=json
        var wikiApiUrl = $"https://en.wikipedia.org/w/api.php?action=parse&pageid={pageid}&prop=text&format=json";

        // The Band pageid was not found in Band Tree Database so call Wikipedia API
        try
        {
            var response = await client.GetAsync(wikiApiUrl);
            response.EnsureSuccessStatusCode();
            // Chat said data is a JSON string
            WikipediaData = await response.Content.ReadAsStringAsync();

            /*
             * now we have the JSON data from Wikipedia
             * we need to parse it and get the Band, Artist, Group (BAG) data
             * Version 1 - title, current members, past members, Genres, Labels, page Timestamp
             * and then save it to the database
             */
            band = bandModelProcessor.FromJson(WikipediaData);
            band.Title = "John Testing Title";
            band.WikipediaPageID = int.Parse(pageid);
            band.PageTimeStamp = DateTimeOffset.Now;
            band.PastBandMembers = new List<Artist>();
            band.CurrentBandMembers = new List<Artist>();
            band.CompanyLabelList = new List<MusicCompanyLabel>();
            band.GenreList = new List<Genre>();
            band.CompanyLabelList.Add(new MusicCompanyLabel { LabelName = "John Testing Label" });
            band.GenreList.Add(new Genre { GenreName = "John Testing Genre 1" }); 
            band.PastBandMembers.Add(new Artist { ArtistName = "Captain Caveman and the Teen Angels", WikipediaPageID = 999999, GenreList = new List<Genre>(), PageTimeStamp = DateTimeOffset.Now });
            band.PastBandMembers.Add(new Artist { ArtistName = "John Testing Artist 2", WikipediaPageID = 99999999, GenreList = new List<Genre>(), PageTimeStamp = DateTimeOffset.Now });
            band.CurrentBandMembers.Add( new Artist { ArtistName = "John Testing Artist 3", WikipediaPageID = 88888888, GenreList = new List<Genre>(), PageTimeStamp = DateTimeOffset.Now } );
            band.CurrentBandMembers.Add(new Artist { ArtistName = "John Testing Artist 4", WikipediaPageID = 77777777, GenreList = new List<Genre>(), PageTimeStamp = DateTimeOffset.Now } );
            band.PastBandMembers[0].GenreList.Add(new Genre { GenreName = "John Testing Genre 2" });
            band.PastBandMembers[1].GenreList.Add(new Genre { GenreName = "John Testing Genre 3" });
            band.CurrentBandMembers[0].GenreList.Add(new Genre { GenreName = "John Testing Genre 4" });
            band.CurrentBandMembers[1].GenreList.Add(new Genre { GenreName = "John Testing Genre 5" });

            // Save the Band, Artist, Group (BAG) data to the Band Tree database
            var updated = await bandService.UpdateBandAsync(band);
        }
        catch (HttpRequestException e)
        {
            return Results.Problem($"No BAG in the Database and Error calling Wikipedia API: {e.Message}");
        }
    }

    /*
     * The Band pageid was found in the database
     * or the pageid was found in Wikipedia and saved to the database
     * 
     * return the Current and Past Members
     *
     * thinking out loud:
     * this return will not be the BandModel
     * 
     * this return needs to be a new model that has the Current and Past Members
     *   New Model:
     *   current members (1 Artist)
     *   past members    (List of Artists)
     *   I also return the pageid of the current and past members
     *   
     *   Minimum to return now is:
     *   current Artist - name, pageid
     *   past Artists   - List of name, pageid
     *   
     *   Easiest might be to return the Artist Model serialized.
     *   But then BandClient will know how Database stores things.
     *   But not really, it will just know how the Band Tree Server returns things.
     * 
     * This is where I have to decide if I want to 
     * get the children and grandchilden members of the BAG in question
     * And if not how will BandClient know to get them at a future time / call?
     * The chunking question.
     * 
     * There is this current PUT call which is (wikipedia-page/{pageid}):
     *   User - I have a BAG pageid
     *   Server - I have that BAG's current and past members
     *   
     *   User - I have 1st level current members (Artists).
     *          Now I want to see the 2nd level Artists which you sent back to me as past members.
     *          In future this could also be producers, other artists found on the BAG's page.
     *          
     *          Those past Band members will have current and past members 
     *          the 2nd level Artists have current and past members
     *          The 2nd level Artists have current Artists which are 2nd level Artists
     *          The 2nd level Artists have past member Artists which are 3rd level Artists
     *          etc.
     *          
     *   This next call from the BandClient will be a GET call
     *   for the 2nd level Artists which are past members of the BAG
     *   it will be wiki
     *   
     *   Maybe when I return the current members and past members 
     *   I also return the pageid of the current and past members
     *   and I start a process to go find 1, 2, 3 or more levels of children and grandchildren
     */
    return Results.Ok(bandModelProcessor.ToJson(band));
});

// DB Init Here

// var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
// await databaseInitializer.InitializeAsync();

app.MapControllers();

// Middleware registration ENDs here

app.Run();

