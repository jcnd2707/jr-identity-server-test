var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{
    options.Authority = "https://localhost:5001";
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters 
    {
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options => 
{
    options.AddPolicy("ApiScope", policy => 
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "jr-auth");
    });
});

var app = builder.Build();

app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => 
{
    endpoints.MapControllers()
    .RequireAuthorization("ApiScope");
});

app.MapControllers();

app.Run();
