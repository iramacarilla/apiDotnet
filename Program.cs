using System.Text;
using DotnetApi;
using DotnetApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors((options)=> {
    options.AddPolicy("DevCors", (corsBilder) => 
    {
        corsBilder.WithOrigins("http://localhost:4200","http://localhost:3000","http://localhost:8000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();   
    });
      options.AddPolicy("ProdCors", (corsBilder) => 
    {
        corsBilder.WithOrigins("http://myProductionSite.com") //your domain
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();   
    });
});
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("AppSettings:TokenKey").Value
        )),
        ValidateIssuer = false,
        ValidateAudience = false
    };

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else {
    app.UseHttpsRedirection();
    app.UseCors("ProdCors");
}


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
