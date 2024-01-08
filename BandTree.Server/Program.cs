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
 * PUT  Get the Band, Artist, Group (BAG) Wikiepedia Page
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
    // took out of IBandDBServices bandService
    var bandService = httpContext.RequestServices.GetRequiredService<IBandDBServices>();

    var requestUrl = httpContext.Request.Path;
    Console.WriteLine($"Received route for URL: {requestUrl}");
    var displayUrl = httpContext.Request.GetDisplayUrl();
    Console.WriteLine($"Received URL GetDisplayUrl: {displayUrl}");

    if (!int.TryParse(pageid, out int numericPageId))
    {
        // The pageid is not a number
        return Results.BadRequest();
        // return Results.NotFound("pageid is not a number");
        // return Task.FromResult(Results.NotFound("pageid is not a number") as IResult);
    }

    IActionResult? actionResult = await bandService.GetBandByPageIDAsync(pageid);

    BandModel band = new BandModel();

    // If the pageid / Band is not found in the Band Tree database, then call the Wikipedia API
    if (actionResult is NotFoundResult)
    {
        var client = clientFactory.CreateClient();
        // for Rush (band) pageid = 25432
        //                 https://en.wikipedia.org/w/api.php?action=parse&pageid=  25432 &prop=text&format=json
        var wikiApiUrl = $"https://en.wikipedia.org/w/api.php?action=parse&pageid={pageid}&prop=text&format=json";

        // The pageid / Band was not found in Band Tree Database so call Wikipedia API
        try
        {
            var response = await client.GetAsync(wikiApiUrl);
            response.EnsureSuccessStatusCode();
            // Chat said data is a JSON string
            var data = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            return Results.Problem($"Error calling Wikipedia API: {e.Message}");
        }

        // now we have the data from Wikipedia
        // we need to parse it and get the Band, Artist, Group (BAG) data
        // and then save it to the database

        BandModel bandFoundinWikipedia = new BandModel();
        if (actionResult is OkObjectResult okObjectResult)
        {
            bandFoundinWikipedia = okObjectResult.Value as BandModel ?? new BandModel();
        }
        else
        {
            return Results.NotFound("Wikipedia returns an unknown type.");        // Handle any other unexpected cases
        }

        // Save the Band, Artist, Group (BAG) data to the Band Tree database
        var updated = await bandService.UpdateBandAsync(bandFoundinWikipedia);

    }
    else
    {
        if (actionResult is OkObjectResult okObjectResult)
        {
            band = okObjectResult.Value as BandModel;
            // Now you can use 'band'
        }
        // the pageid of the Band was found in the Band Tree database
        // band = (BandModel) actionResult.OkObjectResult.Value ?? new BandModel();
    }

    /* The pageid was found in the database
     * or the pageid was found in Wikipedia and saved to the database
     * 
     * so return the Current and Past Members
     */

    // return band.CurrentBandMembers + ' ' + band.PastBandMembers;
    // how do I return band.PastBandMembers also?
    return Results.Ok(new { band.CurrentBandMembers, band.PastBandMembers });
});

// DB Init Here

// var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
// await databaseInitializer.InitializeAsync();

app.MapControllers();

// Middleware registration ENDs here

app.Run();

