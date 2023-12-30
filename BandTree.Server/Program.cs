var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// https://localhost:7100/
// returns Hello World!
app.MapGet("/", () =>
{
    var formattedDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    return $"Hello World! John back from Hawaii {formattedDateTime} Local Date & Time";
});




app.MapControllers();

app.Run();
