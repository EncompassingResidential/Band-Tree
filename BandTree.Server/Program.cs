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
app.MapPut("wikipedia-page/{pageid}", async (string pageid, BandDBServices bandController) =>
{
    if (!int.TryParse(pageid, out int numericPageId))
    {
        // The pageid is not a number
        return Results.BadRequest();
        // return Results.NotFound("pageid is not a number");
        // return Task.FromResult(Results.NotFound("pageid is not a number") as IResult);
    }

    IActionResult? actionResult = await bandController.GetBandByPageIDAsync(pageid);

    if (actionResult is NotFoundResult)
    {
        // The pageid not found in Database
        return Results.NotFound("no pageid found ");
    }

    BandModel bandFoundinDB = new BandModel();
    if (actionResult is OkObjectResult okObjectResult)
    {
        bandFoundinDB = okObjectResult.Value as BandModel ?? new BandModel();
    }
    else
    {
        return Results.NotFound("Wikipedia returns an unknown type.");        // Handle any other unexpected cases
    }

    var updated = await bandController.UpdateBandAsync(bandFoundinDB);

    return updated ? Results.Ok(bandFoundinDB) : Results.NotFound();
});

// DB Init Here

// var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
// await databaseInitializer.InitializeAsync();

app.MapControllers();

// Middleware registration ENDs here

app.Run();
